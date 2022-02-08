using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TravelBox.Common
{
    public interface ICountdownCapable
    {
        event Action OnCountdownComplete;
        event Action<TimeSpan> OnCountdownTick;

        void StartCountdown(TimeSpan length);
        void ToggleCountdownPause();
        void StopCountdown();
    }

    public class Countdown : ICountdownCapable, IDisposable
    {
        // -------------

        const int TICK_LENGTH = 9;
        CancellationTokenSource o_cancellationTokenSource;
        DateTime endTime;
        DateTime pauseTime;
        bool isRunning = false;
        bool isPaused = false;

        //
        // ⛺ ─── ICountdownCapable Interface ───────────────────────────────────────────────────
        //

        #region ICountdownCapable Interface

        public event Action OnCountdownComplete;

        public event Action<TimeSpan> OnCountdownTick;

        public void StartCountdown(TimeSpan length)
        {
            /* ⭐ ---- ---- */

            if (isRunning)
            {
                throw new Exception("Countdown is already running.");
            }

            var start = DateTime.UtcNow; // Use UtcNow instead of Now
            endTime = start.AddTicks(length.Ticks);

            RunCountdownClock();

            isRunning = true;

            /* ---- ---- 🌠 */
        }

        public void ToggleCountdownPause()
        {
            /* ⭐ ---- ---- */

            if (isPaused)
            {
                TimeSpan timeDifference = DateTime.UtcNow - pauseTime;
                endTime += timeDifference;
                RunCountdownClock();
            }
            else
            {
                StopCountdownClock();
                pauseTime = DateTime.UtcNow;
            }

            isPaused = !isPaused;

            /* ---- ---- 🌠 */
        }

        public void StopCountdown()
        {
            /* ⭐ ---- ---- */

            isPaused = false;
            isRunning = false;
            OnCountdownTick?.Invoke(TimeSpan.Zero);

            /* ---- ---- 🌠 */
        }

        public void Dispose()
        {
            /* ⭐ ---- ---- */

            o_cancellationTokenSource.Cancel();
            o_cancellationTokenSource = null;

            /* ---- ---- 🌠 */
        }

        #endregion // ICountdownCapable Interface
        
        //
        // ⛺ ─── Private Implementation ───────────────────────────────────────────────────
        //
        
        #region Private Implementation
        
        void RunCountdownClock()
        {
            /* ⭐ ---- ---- */

            o_cancellationTokenSource = new CancellationTokenSource();

            Task timerLoop = Task.Factory.StartNew(
                action: async () => CountdownLoop(o_cancellationTokenSource.Token),
                cancellationToken: o_cancellationTokenSource.Token,
                creationOptions: TaskCreationOptions.LongRunning,
                scheduler: TaskScheduler.FromCurrentSynchronizationContext());

            /* ---- ---- 🌠 */
        }

        void StopCountdownClock()
        {
            /* ⭐ ---- ---- */

            o_cancellationTokenSource.Cancel();

            /* ---- ---- 🌠 */
        }

        async void CountdownLoop(CancellationToken cancelToken)
        {
            /* ⭐ ---- ---- */

            try
            {
                while (true)
                {
                    TimeSpan remainingTime = endTime - DateTime.UtcNow;

                    if (remainingTime < TimeSpan.Zero)
                    {
                        OnCountdownComplete.Invoke();
                    }
                    else
                    {
                        OnCountdownTick.Invoke(remainingTime);
                    }

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
                o_cancellationTokenSource.Dispose();
            }

            /* ---- ---- 🌠 */
        }
        
        #endregion // Private Implementation

        // -------------
    }
}
