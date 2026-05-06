using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

            Log.TR(this, "<==", Log.CP("x",x));
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

            await Task.Run(()=>
                {
                    Log.TR(this, "==>...Task.Run()", Log.CP("sleepSeconds",sleepSeconds));
                    System.Threading.Thread.Sleep(sleepSeconds * 1000);
                    Log.TR(this, "<==...Task.Run()");
                });

            Log.TR(this, "<==");
            button3.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TestTask tt = new TestTask();
            tt.Run();
        }


        /// <summary>
        /// Taskテスト用クラス
        /// </summary>
        private class TestTask
        {
            /// <summary>
            /// Task生成メソッド
            /// </summary>
            /// <param name="count">引数サンプル</param>
            /// <returns>Taskインスタンス</returns>
            private Task CreateTask(int count)
            {
                return new Task(() =>
                {
                    //0-1000ミリ秒待つ
                    int sleepTime = new Random().Next(1000);
                    System.Threading.Thread.Sleep(sleepTime);

                    //引数と現在時間、Sleep時間を表示する
                    Log.TR(this, Log.CP("count", count), Log.CP("経過時間", DateTime.Now.ToString("HH:mm:ss.fff")), Log.CP("Sleep", sleepTime));
                });
            }

            /// <summary>
            /// Taskテスト
            /// </summary>
            public void Run()
            {
                List<Task> tasks = new List<Task>();

                //Taskを10個作成
                for (int count = 0; count < 10; count++)
                {
                    Task newTask = this.CreateTask(count);

                    tasks.Add(newTask);
                }

                //Taskを実行
                foreach (Task task in tasks)
                {
                    task.Start();
                }

                //Taskが終了するまで待つ
                Task.WaitAll(tasks.ToArray());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TestPartial tp = new TestPartial();

            tp.TestCallCaller();
        }

    }


}
