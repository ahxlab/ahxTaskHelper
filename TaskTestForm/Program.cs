using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskTest
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイント...
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TaskTestForm());
        }
    }
}
