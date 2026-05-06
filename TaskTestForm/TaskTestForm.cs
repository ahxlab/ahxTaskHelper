using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using Logging4net;

namespace TaskTest
{

    public partial class TaskTestForm : Form
    {
        private LogManager lm = null;

        public static Task GoodFunc()
        {
            Log.TR_IN(null);
            return Task.Delay(300)
                .ContinueWith((t) => 
                { 
                    Log.TR(null, "... processing ..."); 
                    Log.TR_OUT(null); 
                });
        }
        public static Task GoodFunc2()
        {
            Log.TR_IN(null);
            return Task.Delay(300)
                .ContinueWith((t) => 
                {
                    Log.TR(null, "... processing ...");
                    Log.TR_OUT(null);
                });
        }

        public static Task ExceptionFunc()
        {
            Log.TR_IN(null);
            return Task.Factory.StartNew(() => 
                { 
                    throw new Exception("user exception"); 
                });
        }
        
        
        public TaskTestForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lm = new LogManager();
            Log.TR(this, "start");

            btnChainTasks.Image = MakeBitmap(Color.AliceBlue, btnChainTasks.Width, btnChainTasks.Height);
            btnChainTasks.Image = new Bitmap("JPEG.JPG");

        }

        // Create a bitmap object and fill it with the specified color.   
        // To make it look like a custom image, draw an ellipse in it.
        private Bitmap MakeBitmap(Color color, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(color), 0, 0, bmp.Width, bmp.Height);
            //g.DrawEllipse(new Pen(Color.DarkGray), 3, 3, width - 6, height - 6);
            g.Dispose();

            return bmp;
        }

        private void btnChainTasks_Click(object sender, EventArgs e)
        {

            string s = string.Format("{0:d2}", 1234);
            Log.TR(this, Log.CP("s", s));

            btnChainTasks.Image = MakeBitmap(Color.Red, btnChainTasks.Width, btnChainTasks.Height);

            TaskHelper.ChainTaskRunnerTest test1 = new TaskHelper.ChainTaskRunnerTest();
            test1.Test();

            return;

            List<Func<Task>> list1 = new List<Func<Task>>();
            list1.Add(() => GoodFunc());
            list1.Add(() => GoodFunc2());
            list1.Add(() => GoodFunc());
            list1.Add(() => ExceptionFunc());     // ここだと Faulted になる

            List<Func<Task>> list2 = new List<Func<Task>>();
            list2.Add(() => GoodFunc());
            list2.Add(() => GoodFunc2());
            list2.Add(() => ExceptionFunc());   // ここだと Canceled になる
            list2.Add(() => GoodFunc());
            list2.Add(() => ExceptionFunc());     // ここだと Faulted になる

            List<Func<Task>> list3 = new List<Func<Task>>();
            list3.Add(() => GoodFunc());
            list3.Add(() => GoodFunc2());
            list3.Add(() => GoodFunc());

            TaskHelper.ChainTaskRunner cTask = new TaskHelper.ChainTaskRunner();
            cTask.IsEnableCancel = true;
            cTask.IsEnableException = true;

            Task task = cTask.ForEachAsync(list2, "*list2*");


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


        TaskHelper.PersistentTaskBase pt = null;

        private void btnRegidentTask_Click(object sender, EventArgs e)
        {
            pt = new PersistentTask();
            pt.Start();
        }

        private void btnSetEvent_Click(object sender, EventArgs e)
        {
            pt.Enqueue(new TaskHelper.QueueObj());
            pt.Set();
            return;

            pt.Enqueue(new TaskHelper.QueueObj());
            pt.Enqueue(new TaskHelper.QueueObj());
            pt.Set();
            System.Threading.Thread.Sleep(30);

            pt.Enqueue(new TaskHelper.QueueObj());
            pt.Enqueue(new TaskHelper.QueueObj());
            pt.Enqueue(new TaskHelper.QueueObj());
            pt.Set();
        }

        private void TaskTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pt!=null)
            {
                pt.Stop();
            }
        }

        private void TaskTestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Threading.Thread.Sleep(100); // Task の終了待ち合わせのつもり
            Log.TR_OUT(this);
        }
    }

    /// <summary>
    /// 永続的なタスク
    /// </summary>
    public class PersistentTask : TaskHelper.PersistentTaskBase
    {
        private void heavyProcess(int x)
        {
            Log.TR_IN(this);
            Log.TR(this, Log.CP("x", x));
            Thread.Sleep(x * 1000);
            Log.TR_OUT(this);
        }

        public override async void Treatment(TaskHelper.QueueObj obj = null)
        {
            Log.TR_IN(null);
            await heavyProcessAsync_呼び出し元スレッドに復帰指定(2);
            await heavyProcessAsync_呼び出し元スレッド復帰しない指定(1);
            Log.TR_OUT(null);
        }

        private async Task heavyProcessAsync_呼び出し元スレッドに復帰指定(int x)
        {
            Log.TR_IN(null, Log.CP("x", x));
            await Task.Run(() => { heavyProcess(x); });
            Log.TR_OUT(null);
        }

        private async Task heavyProcessAsync_呼び出し元スレッド復帰しない指定(int x)
        {
            Log.TR_IN(null, Log.CP("x", x));
            await Task.Run(() => { heavyProcess(x); }).ConfigureAwait(false);
            Log.TR_OUT(null);
        }

    }
}
