/* 
      |\      _,,,---,,_
ZZZzz /,`.-'`'    -.  ;-;;,_
     |,4-  ) )-,_. ,\ (  `'-'
    '---''(_/--'  `-'\_)
*/
using System;
using UnityEngine;
// using TMPro;

namespace Sanctuary.Core
{

    public class Timer : MonoBehaviour
    {
        // -------------

        DateTime endTime;
        DateTime pauseTime;
        bool isTimerEnabled = false;
        bool isTimerPaused = false;

        public Action OnTimerStarted;
        public Action OnTimerDone;
        public Action<TimeSpan> OnTimeLeftChanged;

        void Update()
        {
            // -------------

            if (isTimerEnabled)
            {
                TimeSpan remainingTime = endTime - DateTime.UtcNow;

                if (remainingTime < TimeSpan.Zero)
                {
                    OnTimerDone.Invoke();
                    isTimerEnabled = false;
                }
                else
                {
                    OnTimeLeftChanged.Invoke(remainingTime);
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

        // -------------
    }
}
/* maria aurelia at 09 November 2021 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */