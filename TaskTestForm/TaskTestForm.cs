using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logging4net;

namespace TaskTest
{
    public partial class TaskTestForm : Form
    {
        private LogManager lm = null;

        public static async Task GoodFunc()
        {
            Log.TR_IN(null);
            await Task.Delay(300);
            Log.TR(null, "... processing ...");
            Log.TR_OUT(null);
        }

        public static async Task GoodFunc2()
        {
            Log.TR_IN(null);
            await Task.Delay(300);
            Log.TR(null, "... processing ...");
            Log.TR_OUT(null);
        }

        public static Task ExceptionFunc()
        {
            Log.TR_IN(null);
            return Task.Run(() =>
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

            // 修正(No.7): メモリおよびGDIハンドルリークの解消
            btnChainTasks.Image = new Bitmap("JPEG.JPG");
        }

        private Bitmap MakeBitmap(Color color, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(new SolidBrush(color), 0, 0, bmp.Width, bmp.Height);
            }
            return bmp;
        }

        private async void btnChainTasks_Click(object sender, EventArgs e)
        {
            string s = string.Format("{0:d2}", 12);
            Log.TR(this, Log.CP("s", s));

            if (btnChainTasks.Image != null)
            {
                btnChainTasks.Image.Dispose();
            }
            btnChainTasks.Image = MakeBitmap(Color.Red, btnChainTasks.Width, btnChainTasks.Height);

            List<Func<Task>> list2 = new List<Func<Task>>
            {
                () => GoodFunc(),
                () => GoodFunc2(),
                () => ExceptionFunc(),
                () => GoodFunc()
            };

            TaskHelper.ChainTaskRunner cTask = new TaskHelper.ChainTaskRunner
            {
                IsEnableCancel = true,
                IsEnableException = true
            };

            try
            {
                await cTask.ForEachAsync(list2, "*list2*");
                Log.TR(null, "+++ Completion +++");
            }
            catch (OperationCanceledException)
            {
                Log.TR(null, "+++ Canceled +++");
            }
            catch (Exception ex)
            {
                Log.TR_ERR(null, ex);
                Log.TR(null, "+++ Faulted +++");
            }
        }

        private PersistentTask pt = null;

        // 修正: 常駐タスク起動時にボタンを非活性化
        private void btnRegidentTask_Click(object sender, EventArgs e)
        {
            btnRegidentTask.Enabled = false; // ボタンをdisableにする処理を追加

            pt = new PersistentTask();
            pt.Start();
        }

        // 修正: イベントセット完了時にボタンを活性化状態に戻す
        private void btnSetEvent_Click(object sender, EventArgs e)
        {
            if (pt != null)
            {
                pt.Enqueue(new TaskHelper.QueueObj(), TaskHelper.PersistentTaskBase.ItemCloneEnum.direct);
            }

            btnRegidentTask.Enabled = true; // ボタンをenableに戻す処理を追加
        }

        private void TaskTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pt != null)
            {
                pt.Stop();
            }
        }

        private async void TaskTestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 修正(No.3): タスクの終了を確実に同期して待つ
            if (pt != null)
            {
                await pt.WaitForCompletionAsync();
            }
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

        /// <summary>
        /// 修正(No.2): async void から async Task への変更
        /// </summary>
        public override async Task TreatmentAsync(TaskHelper.QueueObj obj = null)
        {
            Log.TR_IN(null);
            //await heavyProcessAsync_呼び出し元スレッドに復帰指定(2);
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