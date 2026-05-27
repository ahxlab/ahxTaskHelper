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
        private bool _canClose = false; // タスクが安全に終了し、本当にクローズして良いかのステートフラグ

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

        public static async Task ExceptionFunc()
        {
            Log.TR_IN(null);
            await Task.Run(() =>
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
            // 二重押下（連打）による非同期処理中の画像破棄および描画競合を防ぐため、ボタンを非活性化
            btnChainTasks.Enabled = false;

            try
            {
                string s = string.Format("{0:d2}", 12);
                Log.TR(this, Log.CP("s", s));

                if (btnChainTasks.Image != null)
                {
                    btnChainTasks.Image.Dispose();
                }
                btnChainTasks.Image = MakeBitmap(Color.Red, btnChainTasks.Width, btnChainTasks.Height);

                List<Func<Task>> listTasks = new List<Func<Task>>
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

                    await cTask.ForEachAsync(listTasks, "*listTasks*");
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
            finally
            {
                // 正常終了、例外発生に関わらず、最後に必ずボタンを活性状態に戻す
                btnChainTasks.Enabled = true;
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
                btnRegidentTask.Enabled = true;
            }
        }

        // 修正(No.8): メインループの強制終了によるログの欠損を防ぐため、FormClosing で終了シーケンスを制御
        private async void TaskTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // すでに安全な非同期待機が完了している場合はそのまま閉じる
            if (_canClose) return;

            if (pt != null)
            {
                // 一旦フォームのクローズ処理を保留にし、ユーザー操作をロック
                e.Cancel = true;
                this.Enabled = false;

                // 常駐タスクに終了命令を発行
                pt.Stop();

                // 画面オブジェクトが消滅する前のクリーンな状態で、タスクの終了を完璧に待機
                await pt.WaitForCompletionAsync();

                // 終了確認フラグを立てて、改めてフォームを閉じる（次回は最初の if (_canClose) を通過）
                _canClose = true;
                this.Close();
            }
        }

        private void TaskTestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // ここへ到達した段階で、常駐タスクの finally 部分まで100%安全に出力が完了しています
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

        public override async Task TreatmentAsync(TaskHelper.QueueObj obj = null)
        {
            Log.TR_IN(null);
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