using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Logging4net;

namespace TaskHelper
{
    /// <summary>
    /// 例外の代わりに使用する処理結果・エラーコード保持クラス
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string ErrorCode { get; }
        public string ErrorMessage { get; }

        protected Result(bool isSuccess, string errorCode, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new Result(true, null, null);
        public static Result Failure(string errorCode, string errorMessage = null) => new Result(false, errorCode, errorMessage);
    }

    /// <summary>
    /// 戻り値としてデータを返したい場合に使用するジェネリック版Resultクラス
    /// </summary>
    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(bool isSuccess, T value, string errorCode, string errorMessage)
            : base(isSuccess, errorCode, errorMessage)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, null, null);
        public static new Result<T> Failure(string errorCode, string errorMessage = null) => new Result<T>(false, default, errorCode, errorMessage);
    }

    public class Util
    {
        public static T DeepCopy<T>(T target)
        {
            if (target == null)
            {
                return default(T);
            }
            string json = System.Text.Json.JsonSerializer.Serialize(target);
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
    }

    public class QueueObj : Object
    {
    }

    public class StopQueueObj : QueueObj
    {
    }

    /// <summary>
    /// 常駐タスク（スレッド）をサポートします
    /// </summary>
    public class PersistentTaskBase
    {
        public enum ItemCloneEnum
        {
            direct = 1,
            clone = 2,
        }

        private readonly BlockingCollection<QueueObj> _queue = new BlockingCollection<QueueObj>();
        private Task _runningTask = null;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public bool Enqueue(QueueObj obj, ItemCloneEnum ice = ItemCloneEnum.clone)
        {
            try
            {
                Log.TR(null, "Enqueue() -->", Log.CP("ItemCloneEnum", ice));
                QueueObj itemToEnqueue = (ice == ItemCloneEnum.clone) ? Util.DeepCopy(obj) : obj;
                _queue.Add(itemToEnqueue);
                Log.TR(null, "Enqueue() <--");
                return true;
            }
            catch (Exception ex)
            {
                Log.TR_ERR(null, ex);
                return false;
            }
        }

        public void Stop()
        {
            Log.TR_IN(null, "Stop()");
            _queue.Add(new StopQueueObj());
            _queue.CompleteAdding();
        }

        public async Task WaitForCompletionAsync()
        {
            if (_runningTask != null)
            {
                await _runningTask;
            }
        }

        /// <summary>
        /// 【コア機能拡張】派生先（プロトコル受信ルーター等）がキャンセル・タイムアウトを検知できるよう、第2引数にトークンを追加
        /// </summary>
        public virtual async Task TreatmentAsync(QueueObj obj = null, CancellationToken cancellationToken = default)
        {
            await Task.Delay(100, cancellationToken);
        }

        public void Start()
        {
            _runningTask = Task.Factory.StartNew(async () =>
            {
                Log.TR_IN(null, "task start");
                try
                {
                    foreach (var qo in _queue.GetConsumingEnumerable(_cts.Token))
                    {
                        if (qo is StopQueueObj)
                        {
                            Log.TR(null, "stop request detected(StopQueueObj).");
                            break;
                        }

                        // 【コア機能拡張】この常駐タスクが持つ _cts.Token を末端の処理へパススルーします
                        await TreatmentAsync(qo, _cts.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    Log.TR(null, "task canceled.");
                }
                finally
                {
                    Log.TR_OUT(null, "task end");
                }
            }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default).Unwrap();
        }
    }

    /// <summary>
    /// Task を チェイン実行する仕掛けをサポートします
    /// </summary>
    public class ChainTaskRunner
    {
        public bool IsEnableException { get; set; }
        public bool IsEnableCancel { get; set; }
        public string Title { get; set; }

        public ChainTaskRunner()
        {
            IsEnableException = true;
            IsEnableCancel = true;
        }

        /// <summary>
        /// 【コア機能拡張】一括キャンセルやタイムアウトを制御できるよう、CancellationToken を引数とデリゲートに追加
        /// </summary>
        public async Task<Result> ForEachAsync(IEnumerable<Func<CancellationToken, Task<Result>>> tasks, string title, CancellationToken cancellationToken = default)
        {
            Title = title;
            Log.TR_IN(null, Log.CP("Title", Title));

            try
            {
                foreach (Func<CancellationToken, Task<Result>> func in tasks)
                {
                    // 次のタスクに移る前に、既にキャンセル要求（タイムアウト含む）が来ていれば即座に離脱
                    cancellationToken.ThrowIfCancellationRequested();

                    // 各タスクを実行する際、親のトークンをそのまま引き渡して安全に待つ
                    Result result = await func(cancellationToken);

                    if (result.IsFailure)
                    {
                        Log.TR(null, "=== Task Failed (Execution Interrupted) ===", Log.CP("Title", Title), Log.CP("ErrorCode", result.ErrorCode));
                        return result;
                    }
                }
                Log.TR(null, "=== OnlyOnRanToCompletion ===", Log.CP("Title", Title));
                return Result.Success();
            }
            catch (OperationCanceledException)
            {
                Log.TR(null, "=== OnlyOnCanceled ===", Log.CP("Title", Title));
                return Result.Failure("OPERATION_CANCELED", "処理がキャンセルされました。");
            }
            catch (Exception ex)
            {
                Log.TR(null, "=== OnlyOnFaulted ===", Log.CP("Title", Title));
                if (IsEnableException)
                {
                    return Result.Failure("UNHANDLED_EXCEPTION", ex.Message);
                }
                return Result.Success();
            }
            finally
            {
                Log.TR_OUT(null, Log.CP("Title", Title));
            }
        }
    }
}