/* 
      |\      _,,,---,,_
ZZZzz /,`.-'`'    -.  ;-;;,_
     |,4-  ) )-,_. ,\ (  `'-'
    '---''(_/--'  `-'\_)
*/
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TravelBox.Common
{
    public class SanctuaryTimer : MonoBehaviour, ITimingCapable
    {
        /* ⭐ ---- ---- */

        const int TICK_LENGTH = 9;

        DateTime _startTime;
        DateTime _pauseTime;
        TimeSpan _activeRunningLength;


        CancellationTokenSource o_cancellationTokenSource;
        CancellationTokenSource timerTokenSource;

        bool isRunning = false;
        bool isPaused = false;

        void Update()
        {
            // -------------

            if (isRunning)
            {
                if (isPaused is false)
                {
                    TimeSpan totalTime = DateTime.UtcNow - _startTime;

                    _activeRunningLength = totalTime;

                    TimerTicked?.Invoke(totalTime);
                }
            }

            // -------------
        }

        //
        // ⛺ ─── Public Interface ───────────────────────────────────────────────────
        //

        #region Public Interface

        public event Action<TimeSpan> TimerTicked;

        public void StartTimer()
        {
            /* ⭐ ---- ---- */

            if (isRunning)
            {
                throw new Exception("Timer is already running.");
            }

            _startTime = DateTime.UtcNow;

            this.enabled = true;

            isRunning = true;

            /* ---- ---- 🌠 */
        }

        public void ToggleTimerPause(out bool isPaused)
        {
            /* ⭐ ---- ---- */

            if (this.isPaused)
            {
                TimeSpan timeDifference = DateTime.UtcNow - _pauseTime;
                _startTime += timeDifference;
            }
            else
            {
                _pauseTime = DateTime.UtcNow;
            }

            this.isPaused = !this.isPaused;

            isPaused = this.isPaused;

            /* ---- ---- 🌠 */
        }

        public void StopTimer()
        {
            /* ⭐ ---- ---- */

            if (isRunning)
            {
                TimerTicked?.Invoke(TimeSpan.Zero);
            }

            this.enabled = false;
            isPaused = false;
            isRunning = false;

            /* ---- ---- 🌠 */
        }

        public TimeSpan GetRunningLength()
        {
            return _activeRunningLength;
        }

        #endregion // Public Interface

        /* ---- ---- 🌠 */
    }
}
/* maria aurelia at 09 November 2021 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */