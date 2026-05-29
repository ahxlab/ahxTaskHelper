using System;
using System.Threading;
using System.Threading.Tasks;
using Logging4net;

namespace ThreadTestApp
{
    /// <summary>
    /// ThreadTestApp から分離された、生スレッドやTaskの非同期制御、ビジネスロジックを担当するワーカークラス
    /// </summary>
    public class ThreadTestWorker
    {
        private Thread _th = null;
        private int _state = 0;

        /// <summary>
        /// 5秒間スリープする共通の重い模擬処理
        /// </summary>
        private void heavyProcess()
        {
            Log.TR_IN(null);
            Thread.Sleep(5 * 1000);
            Log.TR_OUT(null);
        }

        /// <summary>
        /// 1. 生のスレッドを起こして処理を行い、完了後にコールバックを呼び出す制御
        /// </summary>
        public void StartThread(Action onCompleted)
        {
            var th = new Thread(() =>
            {
                heavyProcess();
                onCompleted?.Invoke();
            });

            // 💡 達人修正：[name] の文字化け（というか固定表記）を防ぎ、数値IDで統一するためコメントアウト
            // th.Name = "name"; 

            th.Start();
        }

        /// <summary>
        /// 2. 生のスレッドを起こし、内部ステート変化を利用して完了まで待ち合わせを行う制御
        /// </summary>
        public void StartThread2AndWait()
        {
            _th = new Thread(threadProc);
            _th.IsBackground = true;
            _state = 1;
            _th.Start();

            // 完了状態 (3) になるまで呼び出し元スレッドをブロッキングして待機
            while (_state != 3)
            {
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// スレッド2専用の内部プロシージャ
        /// </summary>
        private void threadProc()
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
            {
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 3. Task.Run を用いてスレッドプール上で非同期に処理を実行する制御
        /// </summary>
        public Task RunTaskAsync()
        {
            return Task.Run(() =>
            {
                Log.TR(null, "In Task");
                heavyProcess();
            });
        }

        /// <summary>
        /// 4. 内部でさらに非同期メソッドをアウェートする純粋な非同期パターン制御
        /// </summary>
        public async Task HeavyProcessAsync()
        {
            Log.TR_IN(null);
            await Task.Run(() => {
                heavyProcess();
            });
            Log.TR_OUT(null);
        }
    }
}