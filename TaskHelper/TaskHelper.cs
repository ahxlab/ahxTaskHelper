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
            _queue.CompleteAdding();
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
        /// 呼び出し元が完了を確実に待機(await)できるよう、戻り値を Task に変更
        /// </summary>
        public virtual async Task TreatmentAsync(QueueObj obj = null)
        {
            await Task.Delay(100);
        }

        public void Start()
        {
            // 修正(No.9): async ラムダを廃止。同期アクション内で GetAwaiter().GetResult() を呼ぶことで、
            // 2回目以降のループも含めて、すべての GetConsumingEnumerable() によるブロッキングをこの専用スレッド（LongRunning）に固定します。
            _runningTask = Task.Factory.StartNew(() =>
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

                        // 専用スレッド上での安全な同期的待機。これによりスレッドプールの枯渇を完全に防ぎます。
                        TreatmentAsync(qo).GetAwaiter().GetResult();
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
            }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default); // Unwrap() も不要になります
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

        public async Task ForEachAsync(IEnumerable<Func<Task>> tasks, string title)
        {
            Title = title;
            Log.TR_IN(null, Log.CP("Title", Title));

            try
            {
                foreach (Func<Task> function in tasks)
                {
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