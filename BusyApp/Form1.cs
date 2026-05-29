using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logging4net;

namespace BusyApp
{
    public partial class Form1 : Form
    {
        private readonly LogManager _lm = null;

        // ビジネスロジックを担当するワーカークラスのインスタンスを生成
        private readonly BusyAppWorker _worker = new BusyAppWorker();

        public Form1()
        {
            _lm = new LogManager();
            InitializeComponent();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            Log.TR(this, "==>");
            button2.Enabled = false;

            // 処理ロジックをワーカーに委譲
            int x = await _worker.DoWorkAsync(this);

            Log.TR(this, "<==", Log.CP("x", x));
            button2.Enabled = true;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            Log.TR(this, "==>");

            int sleepSeconds = 5;

            // 処理ロジックをワーカーに委譲
            await _worker.SleepAsync(this, sleepSeconds);

            Log.TR(this, "<==");
            button3.Enabled = true;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            try
            {
                // テストタスクの実行をワーカーに委譲
                await _worker.RunTestTasksAsync();
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TestPartial tp = new TestPartial();
            tp.TestCallCaller();
        }
    }
}