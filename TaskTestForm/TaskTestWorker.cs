using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Logging4net;
using TaskHelper;

namespace TaskTest
{
    /// <summary>
    /// TaskTestForm から分離された、タスクチェーンや常駐タスクの実行・管理を担当するロジッククラス
    /// </summary>
    public class TaskTestWorker
    {
        private PersistentTask _pt = null;

        #region タスク定義メソッド群

        public static async Task<Result> GoodFunc()
        {
            Log.TR_IN(null);
            await Task.Delay(300);
            Log.TR(null, "... processing ...");
            Log.TR_OUT(null);
            return Result.Success();
        }

        public static async Task<Result> GoodFunc2()
        {
            Log.TR_IN(null);
            await Task.Delay(300);
            Log.TR(null, "... processing ...");
            Log.TR_OUT(null);
            return Result.Success();
        }

        public static async Task<Result> ExceptionFunc()
        {
            Log.TR_IN(null);
            try
            {
                await Task.Run(() =>
                {
                    throw new Exception("user exception");
                });
                return Result.Success();
            }
            catch (Exception ex)
            {
                Log.TR_ERR(null, ex);
                return Result.Failure("ERR_USER_EXCEPTION", ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// タスクチェーンの一括構築と非同期実行を制御します
        /// </summary>
        public async Task RunChainTasksAsync(object caller)
        {
            // 新しいTaskHelper仕様（CancellationToken対応型）のデリゲートリストを作成
            List<Func<CancellationToken, Task<Result>>> listTasks = new List<Func<CancellationToken, Task<Result>>>
            {
                ct => GoodFunc(),
                ct => GoodFunc2(),
                ct => ExceptionFunc(),
                ct => GoodFunc()
            };

            ChainTaskRunner cTask = new ChainTaskRunner
            {
                IsEnableCancel = true,
                IsEnableException = true
            };

            Result result = await cTask.ForEachAsync(listTasks, "*listTasks*");

            if (result.IsSuccess)
            {
                Log.TR(null, "+++ Completion +++");
            }
            else
            {
                Log.TR(null, $"+++ Faulted (Error Handled) +++ Code: {result.ErrorCode}, Message: {result.ErrorMessage}");
            }
        }

        #region 常駐タスクライフサイクル制御

        public void StartPersistentTask()
        {
            _pt = new PersistentTask();
            _pt.Start();
        }

        public void EnqueuePersistentTask()
        {
            if (_pt != null)
            {
                _pt.Enqueue(new QueueObj(), PersistentTaskBase.ItemCloneEnum.direct);
            }
        }

        public void StopPersistentTask()
        {
            if (_pt != null)
            {
                _pt.Stop();
            }
        }

        public async Task WaitForPersistentTaskAsync()
        {
            if (_pt != null)
            {
                await _pt.WaitForCompletionAsync();
            }
        }

        #endregion
    }

    /// <summary>
    /// 永続的な処理を行う常駐タスククラス
    /// </summary>
    public class PersistentTask : PersistentTaskBase
    {
        private void heavyProcess(int x)
        {
            Log.TR_IN(this);
            Log.TR(this, Log.CP("x", x));
            Thread.Sleep(x * 1000);
            Log.TR_OUT(this);
        }

        public override async Task TreatmentAsync(QueueObj obj = null, CancellationToken cancellationToken = default)
        {
            Log.TR_IN(null);
            // 基底クラスから引き渡された cancellationToken があるため、必要に応じて heavyProcess 側へも伝播可能です
            await heavyProcessAsync_呼び元スレッド復帰しない指定(1);
            Log.TR_OUT(null);
        }

        private async Task heavyProcessAsync_呼び出し元スレッドに復帰指定(int x)
        {
            Log.TR_IN(null, Log.CP("x", x));
            await Task.Run(() => { heavyProcess(x); });
            Log.TR_OUT(null);
        }

        private async Task heavyProcessAsync_呼び元スレッド復帰しない指定(int x)
        {
            Log.TR_IN(null, Log.CP("x", x));
            await Task.Run(() => { heavyProcess(x); }).ConfigureAwait(false);
            Log.TR_OUT(null);
        }
    }
}