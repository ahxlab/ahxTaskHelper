using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logging4net;
using TaskHelper;

namespace TaskTest
{
    public partial class TaskTestForm : Form
    {
        private LogManager lm = null;

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

        public TaskTestForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lm = new LogManager();
            Log.TR(this, "start");
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

            // 【コンパイルエラー修正】引数に CancellationToken を受け取るように型とラムダ式を変更
            List<Func<CancellationToken, Task<Result>>> listTasks = new List<Func<CancellationToken, Task<Result>>>
            {
                ct => GoodFunc(),
                ct => GoodFunc2(),
                ct => ExceptionFunc(),
                ct => GoodFunc()
            };

            TaskHelper.ChainTaskRunner cTask = new TaskHelper.ChainTaskRunner
            {
                IsEnableCancel = true,
                IsEnableException = true
            };

            // ForEachAsync を呼び出します（第3引数は省略可能ですが default を明示しても動作します）
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

        private PersistentTask pt = null;

        private void btnRegidentTask_Click(object sender, EventArgs e)
        {
            btnRegidentTask.Enabled = false;

            pt = new PersistentTask();
            pt.Start();
        }

        private void btnSetEvent_Click(object sender, EventArgs e)
        {
            if (pt != null)
            {
                pt.Enqueue(new TaskHelper.QueueObj(), TaskHelper.PersistentTaskBase.ItemCloneEnum.direct);
            }

            btnRegidentTask.Enabled = true;
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

        // 【コンパイルエラー修正】基底クラスの変更に合わせて CancellationToken 引数を追加
        public override async Task TreatmentAsync(TaskHelper.QueueObj obj = null, CancellationToken cancellationToken = default)
        {
            Log.TR_IN(null);
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