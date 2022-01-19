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
    public class SanctuaryTimer : IDisposable
    {
        /* ⭐ ---- ---- */

        // Task timer;
        TimeSpan totalSpan;
        TimeSpan currentSpan;
        CancellationTokenSource timerTokenSource;

        bool isTimerEnabled = false;
        bool isTimerPaused = false;

        public Action<TimeSpan> OnTimerTick;

        public void StartTimer()
        {
            /* ⭐ ---- ---- */

            isTimerEnabled = false;
            isTimerPaused = false;
            totalSpan = TimeSpan.Zero;
            RunTimer();

            /* ---- ---- 🌠 */
        }

        public void ToggleTimerPause()
        {
            /* ⭐ ---- ---- */

            isTimerPaused = !isTimerPaused;

            if (isTimerPaused)
            {
                totalSpan += currentSpan;
                StopTimer();
            }
            else
            {
                RunTimer();
            }

            /* ---- ---- 🌠 */
        }

        public void StopTimer()
        {
            /* ⭐ ---- ---- */

            timerTokenSource.Cancel();

            /* ---- ---- 🌠 */
        }

        // * lol don't forget to dispose it
        public void Dispose()
        {
            /* ⭐ ---- ---- */
            
            StopTimer();
            
            /* ---- ---- 🌠 */
        }

        void RunTimer()
        {
            /* ⭐ ---- ---- */

            timerTokenSource = new CancellationTokenSource();

            Task timerLoop = Task.Factory.StartNew(
                action: async () => TimerLoop(timerTokenSource.Token),
                cancellationToken: timerTokenSource.Token,
                creationOptions: TaskCreationOptions.LongRunning,
                scheduler: TaskScheduler.Default);

            /* ---- ---- 🌠 */
        }

        async void TimerLoop(CancellationToken token)
        {
            /* ⭐ ---- ---- */

            DateTime startTime = DateTime.UtcNow; // Use UtcNow instead of Now

            while (true)
            {
                currentSpan = DateTime.UtcNow - startTime;
                OnTimerTick?.Invoke(totalSpan + currentSpan);
                // * This thing is actually most important'
                // ? Adding token here below is the way to out of this loop
                // https://stackoverflow.com/questions/13695499/proper-way-to-implement-a-never-ending-task-timers-vs-task
                // else could be:
                // token.ThrowIfCancellationRequested();
                // This way the cancellation will happen instantaneously if inside
                // the Task.Delay, rather than having to wait for the Thread.Sleep to finish.
                await Task.Delay(100, token);
            }

            /* ---- ---- 🌠 */
        }

        /* ---- ---- 🌠 */
    }
}
/* maria aurelia at 09 November 2021 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */