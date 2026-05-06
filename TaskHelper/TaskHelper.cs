using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
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

        private ManualResetEvent _event = null;
        private ConcurrentQueue<QueueObj> _queue = new ConcurrentQueue<QueueObj>();

        /// <summary>
        /// 処理要求等をEnqueueします。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ice"></param>
        /// <returns></returns>
        public bool Enqueue(QueueObj obj, ItemCloneEnum ice = ItemCloneEnum.clone)
        {
            try
            {
                Log.TR(null, "Enqueue() -->", Log.CP("ItemCloneEnum", ice));
                if (ice == ItemCloneEnum.clone)
                {
                    QueueObj clone = Util.DeepCopy<QueueObj>(obj);
                    _queue.Enqueue(clone);
                }
                else
                {
                    _queue.Enqueue(obj);
                }
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
        /// _queue の先頭を取り出します
        /// デキューできない時は null を返します
        /// </summary>
        /// <param name="ice"></param>
        /// <returns></returns>
        private QueueObj TryDequeue(ItemCloneEnum ice = ItemCloneEnum.clone)
        {
            QueueObj obj = null;
            try
            {
                Log.TR(null, "TryDequeue() -->", Log.CP("ItemCloneEnum", ice));
                if (_queue.TryDequeue(out obj))
                {
                    if (ice == ItemCloneEnum.clone)
                    {
                        QueueObj clone = Util.DeepCopy<QueueObj>(obj);
                        return clone;
                    }
                    else
                    {
                        return obj;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.TR_ERR(null, ex);
                return null;
            }
        }

        /// <summary>
        /// Eventがシグナル状態になるのを待ち合せます
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public bool WaitOne(int millisecondsTimeout)
        {
            // パラメータのミリ秒指定が 0 以下は、無限待ちとします
            if (millisecondsTimeout <= 0) millisecondsTimeout = Timeout.Infinite;
            return _event.WaitOne(millisecondsTimeout);
        }


        /// <summary>
        /// 停止指示します
        /// </summary>
        public void Stop()
        {
            Log.TR_IN(null, "Stop()");
            StopQueueObj o = new StopQueueObj();
            Enqueue(o);
            Set();
        }

        /// <summary>
        /// WaitOne()で待っている処理を実行できるようにします（_event をシグナルにします）
        /// </summary>
        /// <returns></returns>
        public bool Set()
        {
            Log.TR_IN(null, "Set()");
            _event.Set();
            return true;
        }

        /// <summary>
        /// Reset()中はWaitOne()は待ちになります（_event は非シグナル状態です）
        /// </summary>
        /// <returns></returns>
        private bool Reset()
        {
            return false;
        }

        static volatile int _Count = 0;
        /// <summary>
        /// この関数をオーバーライドします。
        /// Queueから取り出した処理指示に従った処理を実装します
        /// </summary>
        public virtual void Treatment(QueueObj obj = null)
        {
            Action asyncProcess = () =>
            {
                Log.TR(null, "asyncProcess() start");
                Thread.Sleep(100);     // 仮
                _Count++;
                Log.TR(null, Log.CP("_Count", _Count));
                Log.TR(null, "asyncProcess() end");
            };

            asyncProcess();
        }

        /// <summary>
        /// Task を作成します。
        /// </summary>
        /// <returns></returns>
        public Task CreateTask()
        {
            // 非同期処理です
            Task task = new Task(() =>
            {
                Log.TR_IN(null, "task start");

                // _event が シグナル状態になるまで待ちます
                WaitOne(0);

                do
                {
                    QueueObj qo = null;
                    qo = TryDequeue();

                    // 再度 WaitOne() できるように Reset() します
                    _event.Reset();

                    if (qo == null) break;

                    if (qo is StopQueueObj)
                    {
                        Log.TR(null, "stop request detected(StopQueueObj).");
                        Log.TR_OUT(null, "task end");
                        return;
                    }

                    // 非同期（並列）に処理したい部分を呼び出します。
                    // （GUI部品にアクセスする場合は、別途Taskを生成して アクセスします）
                    Treatment(qo);

                } while (true);

                // Taskを作成し、開始します。このスレッドは別のワーカースレッドとして動作します。
                CreateTask().Start();

                // タスクを終了します
                Log.TR_OUT(null, "task end");
                return;
            });

            Log.TR_OUT(null);
            return task;
        }


        public PersistentTaskBase()
        {
            _event = new ManualResetEvent(false);
        }

        public void Start()
        {
            CreateTask().Start();
        }

    };
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

        public Task ForEachAsync(IEnumerable<Func<Task>> tasks, string title)
        {
            Title = title;
            Log.TR_IN(null, Log.CP("Title", Title));

            var tcs = new TaskCompletionSource<bool>();

            Task currentTask = Task.FromResult(false);

            foreach (Func<Task> function in tasks)
            {
                currentTask.ContinueWith((t) =>
                {
                    if (IsEnableException)
                        tcs.TrySetException(t.Exception.InnerExceptions);
                    Log.TR(null, "--- OnlyOnFaulted ---"); 
                }, TaskContinuationOptions.OnlyOnFaulted);

                currentTask.ContinueWith((t) =>
                {
                    if (IsEnableCancel)
                        tcs.TrySetCanceled();
                    Log.TR(null, "--- OnlyOnCanceled ---");
                }, TaskContinuationOptions.OnlyOnCanceled);

                Task<Task> continuation = currentTask.ContinueWith((t) =>
                        function(), TaskContinuationOptions.OnlyOnRanToCompletion);

                currentTask = continuation.Unwrap();
            }

            //OnlyOnFaulted
            currentTask.ContinueWith((t) =>
            {
                if (IsEnableException)
                    tcs.TrySetException(t.Exception.InnerExceptions);
                Log.TR(null, "=== OnlyOnFaulted ===", Log.CP("Title", Title));
            }, TaskContinuationOptions.OnlyOnFaulted);

            //OnlyOnCanceled
            currentTask.ContinueWith((t) =>
            {
                if (IsEnableCancel)
                    tcs.TrySetCanceled();
                Log.TR(null, "=== OnlyOnCanceled ===", Log.CP("Title", Title));
            }, TaskContinuationOptions.OnlyOnCanceled);

            //OnlyOnRanToCompletion
            currentTask.ContinueWith((t) =>
            {
                tcs.TrySetResult(true);
                Log.TR(null, "=== OnlyOnRanToCompletion ===", Log.CP("Title", Title));
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            Log.TR_OUT(null, Log.CP("Title", Title));
            return tcs.Task;
        }
    }

    public class ChainTaskRunnerTest
    {
        /// <summary>
        /// Taskサンプル
        /// </summary>
        /// <returns></returns>
        private Task GoodTask()
        {
            Log.TR_IN(null);
            return Task.Delay(300)
                .ContinueWith((t) =>
                {
                    Log.TR(null, "... processing ...");
                    Log.TR_OUT(null);
                });
        }

        public void Test()
        {
            ChainTaskRunner ctr = new ChainTaskRunner();
            List<Func<Task>> list1 = new List<Func<Task>>();
            list1.Add(() => GoodTask());

            List<Func<Task>> list2 = new List<Func<Task>>();
            list2.Add(() => GoodTask());

            Task task = ctr.ForEachAsync(list2, "*list2*");

            //Faulted
            task.ContinueWith((t) =>
            {
                Log.TR(null, "+++ Faulted +++");
                Log.TR(null, t.Exception.ToString());
            }, TaskContinuationOptions.OnlyOnFaulted);

            //Canceled
            task.ContinueWith((t) =>
            {
                Log.TR(null, "+++ Canceled +++");
            }, TaskContinuationOptions.OnlyOnCanceled);

            //Completion
            task.ContinueWith((t) =>
            {
                Log.TR(null, "+++ Completion +++");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

        }
    }

}
