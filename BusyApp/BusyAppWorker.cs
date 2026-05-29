using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logging4net;

namespace BusyApp
{
    /// <summary>
    /// Form1 から分離されたビジネスロジックを集約して担当するワーカークラス
    /// </summary>
    public class BusyAppWorker
    {
        /// <summary>
        /// 10秒待機する模擬非同期処理
        /// </summary>
        public Task<int> DoWorkAsync(object caller)
        {
            return Task.Run(() =>
            {
                Log.TR(caller, "==>...doWork()");
                System.Threading.Thread.Sleep(10 * 1000);
                Log.TR(caller, "<==...doWork()");
                return 20;
            });
        }

        /// <summary>
        /// 指定秒数待機する模擬非同期処理
        /// </summary>
        public Task SleepAsync(object caller, int sleepSeconds)
        {
            return Task.Run(() =>
            {
                Log.TR(caller, "==>...Task.Run()", Log.CP("sleepSeconds", sleepSeconds));
                System.Threading.Thread.Sleep(sleepSeconds * 1000);
                Log.TR(caller, "<==...Task.Run()");
            });
        }

        /// <summary>
        /// テストタスク群を一括実行するエントリーメソッド
        /// </summary>
        public async Task RunTestTasksAsync()
        {
            TestTask tt = new TestTask();
            await tt.RunAsync();
        }

        /// <summary>
        /// 移设されたTaskテスト用内部クラス
        /// </summary>
        private class TestTask
        {
            private Task CreateTaskAsync(int count)
            {
                return Task.Run(() =>
                {
                    int sleepTime = new Random().Next(1000);
                    System.Threading.Thread.Sleep(sleepTime);

                    Log.TR(this, Log.CP("count", count), Log.CP("経過時間", DateTime.Now.ToString("HH:mm:ss.fff")), Log.CP("Sleep", sleepTime));
                });
            }

            public async Task RunAsync()
            {
                List<Task> tasks = new List<Task>();

                for (int count = 0; count < 10; count++)
                {
                    tasks.Add(this.CreateTaskAsync(count));
                }

                await Task.WhenAll(tasks);
            }
        }
    }
}