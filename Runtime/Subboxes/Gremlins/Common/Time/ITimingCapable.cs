/* 
      |\      _,,,---,,_
ZZZzz /,`.-'`'    -.  ;-;;,_
     |,4-  ) )-,_. ,\ (  `'-'
    '---''(_/--'  `-'\_)
*/
using System;

namespace TravelBox.Common
{
    public interface ITimingCapable
    {
        event Action<TimeSpan> TimerTicked;

        void StartTimer();
        void ToggleTimerPause(out bool currentState);
        void StopTimer();
        TimeSpan GetRunningLength();
    }
}
/* maria aurelia at 09 November 2021 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */