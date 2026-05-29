using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logging4net;

namespace ThreadTestApp
{
    public partial class ThreadTestApp : Form
    {
        private LogManager _lm = null;

        // スレッド・タスクロジックを担当するワーカークラスのインスタンスを生成
        private readonly ThreadTestWorker _worker = new ThreadTestWorker();

        public ThreadTestApp()
        {
            _lm = new LogManager();
            InitializeComponent();
        }

        private string getTimeString()
        {
            return DateTime.Now.ToString("hh:mm:ss.fff");
        }

        private void btnThreadStart_Click(object sender, EventArgs e)
        {
            Log.TR_IN(null);
            btnThreadStart.Enabled = false;
            txtMessage.Text = getTimeString() + " start";

            // ワーカーにスレッド処理を委譲。完了後のUI復帰処理をActionとして渡します
            _worker.StartThread(() =>
            {
                this.BeginInvoke((Action)(() =>
                {
                    Log.TR(null, "in BeginInvoke()");
                    btnThreadStart.Enabled = true;
                    txtMessage.Text = getTimeString() + " end";
                }));
            });

            Log.TR_OUT(null);
        }

        private void btnThreadStart2_Click(object sender, EventArgs e)
        {
            Log.TR_IN(null);
            btnThreadStart2.Enabled = false;
            txtMessage.Text = getTimeString() + " start";

            // ワーカー側でバックグラウンドスレッドを立ち上げ、完了まで安全に待ち合わせます
            _worker.StartThread2AndWait();

            btnThreadStart2.Enabled = true;
            txtMessage.Text = getTimeString() + " end";
            Log.TR_OUT(null);
        }

        private async void btnTaskStart_Click(object sender, EventArgs e)
        {
            Log.TR_IN(null);
            this.btnTaskStart.Enabled = false;
            txtMessage.Text = getTimeString() + " start";

            // Task.Run による非同期処理をワーカーに委譲
            await _worker.RunTaskAsync();

            Log.TR(null, "end await");
            btnTaskStart.Enabled = true;
            txtMessage.Text = getTimeString() + " end";
            Log.TR_OUT(null);
        }

        private async void btnTaskStart2_Click(object sender, EventArgs e)
        {
            Log.TR_IN(null);
            this.btnTaskStart2.Enabled = false;
            txtMessage.Text = getTimeString() + " start";

            // 内部でアウェートする純粋非同期処理をワーカーに委譲
            await _worker.HeavyProcessAsync();

            Log.TR(null, "end await none.");
            btnTaskStart2.Enabled = true;
            txtMessage.Text = getTimeString() + " end";
            Log.TR_OUT(null);
        }
    }
}