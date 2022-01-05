using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TravelBox.Common
{
    public class Countdown : MonoBehaviour
    {
        // -------------

        DateTime endTime;
        DateTime pauseTime;
        bool isTimerEnabled = false;
        bool isTimerPaused = false;

        public Action OnCountdownDone;
        public Action<TimeSpan> OnCountdownTick;

        void Update()
        {
            // -------------

            if (isTimerEnabled)
            {
                TimeSpan remainingTime = endTime - DateTime.UtcNow;

                if (remainingTime < TimeSpan.Zero)
                {
                    OnCountdownDone.Invoke();
                    isTimerEnabled = false;
                }
                else
                {
                    OnCountdownTick.Invoke(remainingTime);
                }
            }

            // -------------
        }


        public void StatTimer(int minutes)
        {
            // -------------

            var start = DateTime.UtcNow; // Use UtcNow instead of Now
            endTime = start.AddMinutes(minutes);
            isTimerEnabled = true;

            // -------------
        }

        public void ToggleTimerPause()
        {
            // -------------
            
            if (isTimerPaused)
            {
                TimeSpan timeDifference = DateTime.UtcNow - pauseTime;
                endTime += timeDifference;
                isTimerEnabled = true;
            }
            else
            {
                isTimerEnabled = false;
                pauseTime = DateTime.UtcNow;
            }

            isTimerPaused = !isTimerPaused;
            
            // -------------
        }

        public void StopTimer()
        {
            /* ⭐ ---- ---- */
            
            isTimerEnabled = false;
            OnCountdownTick?.Invoke(TimeSpan.Zero);
            
            /* ---- ---- 🌠 */
        }

        // -------------
    }
}
