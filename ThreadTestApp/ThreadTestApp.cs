using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Logging4net;

namespace ThreadTestApp
{
    public partial class ThreadTestApp : Form
    {
        public ThreadTestApp()
        {
            _lm = new LogManager();
            InitializeComponent();
        }

        private LogManager _lm = null;

        private void heavyProcess()
        {
            Log.TR_IN(null);
            Thread.Sleep(5 * 1000);
            Log.TR_OUT(null);
        }

        private async Task heavyProcessAsync()
        {
            Log.TR_IN(null);
            await Task.Run(() => {
                heavyProcess();
            });
            Log.TR_OUT(null);
        }


        private string getTimeString()
        {
            return DateTime.Now.ToString("hh:mm:ss.fff");
        }

        private  void threadProc()
        {
            if (_state == 1)
            {
                _state = 2;
                Log.TR(null, ">> heavyProcess() calling");
                heavyProcess();
                Log.TR(null, "<< heavyProcess() returned");
                _state = 3;
            }
            while (true)
                Thread.Sleep(1000);
        }

        private void btnThreadStart_Click(object sender, EventArgs e)
        {
            Log.TR_IN(null);
            btnThreadStart.Enabled = false;
            txtMessage.Text = getTimeString() + " start";

            var th = new Thread(() =>
            {
                heavyProcess();
                this.BeginInvoke((Action)(() =>
                {
                    Log.TR(null, "in BeginInvoke()");
                    btnThreadStart.Enabled = true;
                    txtMessage.Text = getTimeString() + " end";
                }));
            });
            th.Name = "name";
            th.Start();

            Log.TR_OUT(null);
        }
        
        private Thread _th = null;
        private int _state = 0;

        private void btnThreadStart2_Click(object sender, EventArgs e)
        {
            Log.TR_IN(null);
            btnThreadStart2.Enabled = false;
            txtMessage.Text = getTimeString() + " start";

            _th = new Thread(threadProc);
            _th.IsBackground = true;
            _state = 1;
            _th.Start();

            // 何らかの待ち合わせ機構
            while (_state != 3)
            {
                Thread.Sleep(10);
            }

            btnThreadStart2.Enabled = true;
            txtMessage.Text = getTimeString() + " end";
            Log.TR_OUT(null);
        }

        private async void btnTaskStart_Click(object sender, EventArgs e)
        {
            Log.TR_IN(null);
            this.btnTaskStart.Enabled = false;
            txtMessage.Text = getTimeString() + " start";
            await Task.Run(() => 
            {
                Log.TR(null, "In Task");
                heavyProcess();
            });
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

            await heavyProcessAsync();
            Log.TR(null, "end await none.");
            btnTaskStart2.Enabled = true;
            txtMessage.Text = getTimeString() + " end";
            Log.TR_OUT(null);
        }

    }
}
