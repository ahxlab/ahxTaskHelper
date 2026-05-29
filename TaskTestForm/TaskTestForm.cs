using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logging4net;

namespace TaskTest
{
    public partial class TaskTestForm : Form
    {
        private LogManager lm = null;

        // ロジック層を担当するワーカーのインスタンスを生成
        private readonly TaskTestWorker _worker = new TaskTestWorker();

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

            // 【ロジック分離】実際のタスクチェーン実行処理はワーカーへ委譲
            await _worker.RunChainTasksAsync(this);
        }

        private void btnRegidentTask_Click(object sender, EventArgs e)
        {
            btnRegidentTask.Enabled = false;

            // 【ロジック分離】常駐タスクの起動をワーカーへ委譲
            _worker.StartPersistentTask();
        }

        private void btnSetEvent_Click(object sender, EventArgs e)
        {
            // 【ロジック分離】キューへの追加をワーカーへ委譲
            _worker.EnqueuePersistentTask();

            btnRegidentTask.Enabled = true;
        }

        private void TaskTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 【ロジック分離】常駐タスクの停止をワーカーへ委譲
            _worker.StopPersistentTask();
        }

        private async void TaskTestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 【ロジック分離】タスクの完全終了待ち合わせをワーカーへ委譲
            await _worker.WaitForPersistentTaskAsync();

            Log.TR_OUT(this);
        }
    }
}