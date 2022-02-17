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
    public class SanctuaryTimer : ITimingCapable
    {
        /* ⭐ ---- ---- */

        const int TICK_LENGTH = 9;

        DateTime v_startTime;
        DateTime v_pauseTime;

        CancellationTokenSource o_cancellationTokenSource;
        CancellationTokenSource timerTokenSource;

        bool isRunning = false;
        bool isPaused = false;
        
        //
        // ⛺ ─── Public Interface ───────────────────────────────────────────────────
        //
        
        #region Public Interface
        
        public event Action<TimeSpan> TimerTicked;

        public void StartTimer()
        {
            /* ⭐ ---- ---- */

            Debug.Log($"blopp");
            
            
            if (isRunning)
            {
                throw new Exception("Timer is already running.");
            }

            v_startTime = DateTime.UtcNow;

            RunTimerClock();

            isRunning = true;

            /* ---- ---- 🌠 */
        }

        public void ToggleTimerPause()
        {
            /* ⭐ ---- ---- */

            if (isPaused)
            {
                TimeSpan timeDifference = DateTime.UtcNow - v_pauseTime;
                v_startTime += timeDifference;
                RunTimerClock();
            }
            else
            {
                StopTimerClock();
                v_pauseTime = DateTime.UtcNow;
            }

            isPaused = !isPaused;

            /* ---- ---- 🌠 */
        }

        public void StopTimer()
        {
            /* ⭐ ---- ---- */

            o_cancellationTokenSource.Cancel();
            o_cancellationTokenSource = null;
            isPaused = false;
            isRunning = false;

            /* ---- ---- 🌠 */
        }

        // * lol don't forget to dispose it
        public void Dispose()
        {
            /* ⭐ ---- ---- */
            
            o_cancellationTokenSource?.Cancel();
            o_cancellationTokenSource = null;
            
            /* ---- ---- 🌠 */
        }

        
        #endregion // Public Interface
        
        //
        // ⛺ ─── Private Implementation ───────────────────────────────────────────────────
        //
        
        #region Private Implementation
        
        void RunTimerClock()
        {
            /* ⭐ ---- ---- */

            o_cancellationTokenSource = new CancellationTokenSource();
            
            Task timerLoop = Task.Factory.StartNew(
                action: async () => TimerLoop(o_cancellationTokenSource.Token),
                cancellationToken: o_cancellationTokenSource.Token,
                creationOptions: TaskCreationOptions.LongRunning,
                scheduler: TaskScheduler.FromCurrentSynchronizationContext());
            
            /* ---- ---- 🌠 */
        }

        void StopTimerClock()
        {
            /* ⭐ ---- ---- */

            o_cancellationTokenSource.Cancel();

            /* ---- ---- 🌠 */
        }

        async void TimerLoop(CancellationToken cancelToken)
        {
            /* ⭐ ---- ---- */

            try
            {
                while (true)
                {
                    TimeSpan totalTime = DateTime.UtcNow - v_startTime;

                    TimerTicked.Invoke(totalTime);

                    // * This thing is actually most important'
                    // ? Adding token here below is the way to out of this loop
                    // https://stackoverflow.com/questions/13695499/proper-way-to-implement-a-never-ending-task-timers-vs-task
                    // else could be:
                    // token.ThrowIfCancellationRequested();
                    // This way the cancellation will happen instantaneously if inside
                    // the Task.Delay, rather than having to wait for the Thread.Sleep to finish.
                    await Task.Delay(TICK_LENGTH, cancelToken);
                }
            }
            catch (TaskCanceledException) 
            { 
                // TODO Hmm, is it oki? I kindof expect it to happen and whatevs
            }
            finally
            {
                o_cancellationTokenSource?.Dispose();
            }

            /* ---- ---- 🌠 */
        }
        
        #endregion // Private Implementation

        /* ---- ---- 🌠 */
    }
}
/* maria aurelia at 09 November 2021 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */