using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logging4net;

namespace BusyApp
{
    public partial class Form1 : Form
    {
        LogManager lm = null;
        public Form1()
        {
            lm = new LogManager();
            InitializeComponent();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            Log.TR(this, "==>");
            button2.Enabled = false;

            int x = await doWork();

            Log.TR(this, "<==", Log.CP("x", x));
            button2.Enabled = true;
        }

        private Task<int> doWork()
        {
            return Task.Run(() =>
            {
                Log.TR(this, "==>...doWork()");
                System.Threading.Thread.Sleep(10 * 1000);
                Log.TR(this, "<==...doWork()");
                return 20;
            });
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            Log.TR(this, "==>");

            int sleepSeconds = 5;

            await Task.Run(() =>
            {
                Log.TR(this, "==>...Task.Run()", Log.CP("sleepSeconds", sleepSeconds));
                System.Threading.Thread.Sleep(sleepSeconds * 1000);
                Log.TR(this, "<==...Task.Run()");
            });

            Log.TR(this, "<==");
            button3.Enabled = true;
        }

        // 修正: イベントハンドラを async にし、ボタンの多重連打を抑制しながらフリーズを防止
        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            try
            {
                TestTask tt = new TestTask();
                await tt.RunAsync();
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        /// <summary>
        /// Taskテスト用クラス
        /// </summary>
        private class TestTask
        {
            // 修正: 明示的に Task.Run を用いてスレッドプールで実行するクリーンなファクトリメソッド構造に修正
            private Task CreateTaskAsync(int count)
            {
                return Task.Run(() =>
                {
                    int sleepTime = new Random().Next(1000);
                    System.Threading.Thread.Sleep(sleepTime);

                    Log.TR(this, Log.CP("count", count), Log.CP("経過時間", DateTime.Now.ToString("HH:mm:ss.fff")), Log.CP("Sleep", sleepTime));
                });
            }

            /// <summary>
            /// Taskテスト（非同期・ノンブロッキング版）
            /// </summary>
            public async Task RunAsync()
            {
                List<Task> tasks = new List<Task>();

                for (int count = 0; count < 10; count++)
                {
                    tasks.Add(this.CreateTaskAsync(count));
                }

                // 修正: UIスレッドをロックする Task.WaitAll を完全廃止し、ノンブロッキングに同期する WhenAll に変更
                await Task.WhenAll(tasks);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TestPartial tp = new TestPartial();
            tp.TestCallCaller();
        }
    }
}