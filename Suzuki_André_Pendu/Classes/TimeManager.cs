using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Suzuki_André_Pendu.Classes
{
    internal class TimeManager
    {
        //---------------------------------- Initializations -----------------------------------
        // Variables
        DispatcherTimer timer;
        public bool isTimerRunning = false;
        // Classes
        private GameManager _GameManager;
        private MainWindow _mainWindow;
        // Constructor
        public TimeManager(GameManager gameManager)
        {
            // Class Initialization
            _GameManager = gameManager;
            _mainWindow = _GameManager._mainWindow;
        }
        //---------------------------------- Functions -----------------------------------

        /// <summary>
        /// Starts the timer 
        /// </summary>
        public void StartTimer()
        {
            isTimerRunning = true;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        /// <summary>
        /// Increments the timer and checks if the game has ended
        /// Stops the timer if the game has ended
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            _GameManager.PlaySoundEffect("Tick");

            if (_mainWindow.Timer_ProgressBar.Value >= _mainWindow.Timer_ProgressBar.Maximum | _GameManager.GameEnded)
            {
                StopTimer();
                isTimerRunning = false;

                if (_GameManager.GameEnded == false)
                {
                    _GameManager.DetectEndGame(true);
                }
                return;
            }

            _GameManager.time++;
            _mainWindow.Timer_ProgressBar.Value = _GameManager.time;
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void StopTimer()
        {
            isTimerRunning = false;
            timer.Stop();
        }

        /// <summary>
        /// Resets the timer
        /// </summary>
        public void ResetTimer()
        {
            _GameManager.time = 0;
            _mainWindow.Timer_ProgressBar.Value = _GameManager.time;
        }
    }
}
