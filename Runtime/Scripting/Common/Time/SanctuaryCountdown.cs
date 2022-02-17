using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TravelBox.Common
{
    public class SanctuaryCountdown : ICountdownCapable
    {
        // -------------

        const int TICK_LENGTH = 10;
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

            Debug.Log($"here4a {length.Minutes}");


            var start = DateTime.UtcNow; // Use UtcNow instead of Now
            endTime = start.AddTicks(length.Ticks);

            Debug.Log($"here4b");


            RunCountdownClock();

            Debug.Log($"here5");


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

            o_cancellationTokenSource?.Cancel();
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

            Debug.Log($"here6");


            o_cancellationTokenSource = new CancellationTokenSource();

            Task timerLoop = Task.Factory.StartNew(
                action: async () => CountdownLoop(o_cancellationTokenSource.Token),
                cancellationToken: o_cancellationTokenSource.Token,
                creationOptions: TaskCreationOptions.LongRunning,
                scheduler: TaskScheduler.Default);

            Debug.Log($"here7");

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

                    Debug.Log($"COUN TDOWN LOOP TICK");

                    Debug.Log(remainingTime);
                    Debug.Log(remainingTime.Minutes);
                    Debug.Log(endTime);

                    if (remainingTime < TimeSpan.Zero)
                    {
                        Debug.Log($"complete");

                        OnCountdownComplete.Invoke();
                    }
                    else
                    {
                        Debug.Log($"ooops");

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
            catch(Exception ex)
            {
                throw ex;
            }
            // catch (TaskCanceledException)
            // {
            //     // TODO Hmm, is it oki? I kindof expect it to happen and whatevs
            //     Debug.Log($"countdown loop cancelled");
            // }

            Debug.Log($"out of loop");
            o_cancellationTokenSource?.Dispose();
            // finally
            // {
            //     Debug.Log($"FINALLY");
            //     o_cancellationTokenSource?.Dispose();
            // }

            /* ---- ---- 🌠 */
        }

        #endregion // Private Implementation

        // -------------
    }
}
