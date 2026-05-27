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

        // 修正(No.4): イベントとQueueをBlockingCollectionに一本化し、データの目詰まり・遅延を解消
        private readonly BlockingCollection<QueueObj> _queue = new BlockingCollection<QueueObj>();
        private Task _runningTask = null;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        /// <summary>
        /// 処理要求等をEnqueueします。
        /// </summary>
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

        /// <summary>
        /// 停止指示します
        /// </summary>
        public void Stop()
        {
            Log.TR_IN(null, "Stop()");
            _queue.Add(new StopQueueObj());
            _queue.CompleteAdding(); // 新しいアイテムの追加をブロック
        }

        /// <summary>
        /// タスクの完全終了を確実にお見送りするための待ち合わせメソッド
        /// </summary>
        public async Task WaitForCompletionAsync()
        {
            if (_runningTask != null)
            {
                await _runningTask;
            }
        }

        /// <summary>
        /// 修正(No.2): 呼び出し元が完了を確実に待機(await)できるよう、戻り値を Task に変更
        /// </summary>
        public virtual async Task TreatmentAsync(QueueObj obj = null)
        {
            await Task.Delay(100); // デフォルト実装の仮ディレイ
        }

        public void Start()
        {
            // 修正(No.1, No.5): 無限再帰を完全に排除。さらに LongRunning を指定してスレッドプールの占有を防ぐ
            _runningTask = Task.Factory.StartNew(async () =>
            {
                Log.TR_IN(null, "task start");
                try
                {
                    // GetConsumingEnumerableは、データが空の時は自動で効率的にスリープし、データが入ると即座に処理します
                    foreach (var qo in _queue.GetConsumingEnumerable(_cts.Token))
                    {
                        if (qo is StopQueueObj)
                        {
                            Log.TR(null, "stop request detected(StopQueueObj).");
                            break;
                        }

                        // 修正(No.2): 非同期オーバーライドの完了を正しく追跡して順序を保証
                        await TreatmentAsync(qo);
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
        /// 修正(No.6 改): Func&lt;Task&lt;Result&gt;&gt; をループ実行し、エラーコードを検知したら後続を中断して返却
        /// </summary>
        public async Task<Result> ForEachAsync(IEnumerable<Func<Task<Result>>> tasks, string title)
        {
            Title = title;
            Log.TR_IN(null, Log.CP("Title", Title));

            try
            {
                foreach (Func<Task<Result>> function in tasks)
                {
                    // 各タスクを実行し、戻り値(Result)を安全に待つ
                    Result result = await function();

                    // エラーコードによる失敗を検出した場合は即時離脱（後続は実行しない）
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

    public class ChainTaskRunnerTest
    {
        private async Task<Result> GoodTask()
        {
            Log.TR_IN(null);
            await Task.Delay(300);
            Log.TR(null, "... processing ...");
            Log.TR_OUT(null);
            return Result.Success();
        }

        public async Task TestAsync()
        {
            ChainTaskRunner ctr = new ChainTaskRunner();
            List<Func<Task<Result>>> list2 = new List<Func<Task<Result>>> { () => GoodTask() };

            Result result = await ctr.ForEachAsync(list2, "*list2*");
            if (result.IsSuccess)
            {
                Log.TR(null, "+++ Completion +++");
            }
            else
            {
                Log.TR(null, $"+++ Faulted +++ ErrorCode: {result.ErrorCode}");
            }
        }
    }
}