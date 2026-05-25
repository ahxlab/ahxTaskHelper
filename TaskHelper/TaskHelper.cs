using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Logging4net;

namespace TaskHelper
{
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
        /// 修正(No.6): ContinueWith や Unwrap の複雑な連鎖を完全廃止し、直感的な async/await ループにリファクタリング
        /// </summary>
        public async Task ForEachAsync(IEnumerable<Func<Task>> tasks, string title)
        {
            Title = title;
            Log.TR_IN(null, Log.CP("Title", Title));

            try
            {
                foreach (Func<Task> function in tasks)
                {
                    // 各タスクを順番に実行し、完了を安全に待つ
                    await function();
                }
                Log.TR(null, "=== OnlyOnRanToCompletion ===", Log.CP("Title", Title));
            }
            catch (OperationCanceledException)
            {
                Log.TR(null, "=== OnlyOnCanceled ===", Log.CP("Title", Title));
                if (IsEnableCancel) throw;
            }
            catch (Exception ex)
            {
                Log.TR(null, "=== OnlyOnFaulted ===", Log.CP("Title", Title));
                if (IsEnableException) throw;
            }
            finally
            {
                Log.TR_OUT(null, Log.CP("Title", Title));
            }
        }
    }

    public class ChainTaskRunnerTest
    {
        private async Task GoodTask()
        {
            Log.TR_IN(null);
            await Task.Delay(300);
            Log.TR(null, "... processing ...");
            Log.TR_OUT(null);
        }

        public async Task TestAsync()
        {
            ChainTaskRunner ctr = new ChainTaskRunner();
            List<Func<Task>> list2 = new List<Func<Task>> { () => GoodTask() };

            try
            {
                await ctr.ForEachAsync(list2, "*list2*");
                Log.TR(null, "+++ Completion +++");
            }
            catch (OperationCanceledException)
            {
                Log.TR(null, "+++ Canceled +++");
            }
            catch (Exception ex)
            {
                Log.TR(null, "+++ Faulted +++");
                Log.TR(null, ex.ToString());
            }
        }
    }
}