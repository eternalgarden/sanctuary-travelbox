using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TravelBox.Common
{
    public class SanctuaryCountdown : MonoBehaviour, ICountdownCapable
    {
        // -------------

        CancellationTokenSource o_cancellationTokenSource;
        TimeSpan _desiredCountdownLength;
        TimeSpan _activeRunningLength;
        DateTime _endTime;
        DateTime _pauseTime;
        bool isRunning = false;
        bool isPaused = false;

        void Update()
        {
            // -------------

            if (isRunning)
            {
                if (isPaused is false)
                {
                    TimeSpan remainingTime = _endTime - DateTime.UtcNow;

                    if (remainingTime < TimeSpan.Zero)
                    {
                        OnCountdownComplete.Invoke();
                        StopCountdown();
                    }
                    else
                    {
                        OnCountdownTick?.Invoke(remainingTime);
                    }
                }
            }

            // -------------
        }

        //
        // ⛺ ─── ICountdownCapable Interface ───────────────────────────────────────────────────
        //

        #region ICountdownCapable Interface

        public event Action OnCountdownComplete;

        public event Action<TimeSpan> OnCountdownTick;

        public void RunCountdown(TimeSpan length)
        {
            /* ⭐ ---- ---- */

            if (isRunning)
            {
                throw new Exception("Countdown is already running.");
            }

            var start = DateTime.UtcNow; // Use UtcNow instead of Now
            _endTime = start.AddTicks(length.Ticks);
            _desiredCountdownLength = length;

            this.enabled = true;

            isRunning = true;

            /* ---- ---- 🌠 */
        }

        public void ToggleCountdownPause(out bool isPaused)
        {
            /* ⭐ ---- ---- */

            if (this.isPaused)
            {
                TimeSpan timeDifference = DateTime.UtcNow - _pauseTime;
                _endTime += timeDifference;
            }
            else
            {
                _pauseTime = DateTime.UtcNow;
            }

            this.isPaused = !this.isPaused;

            isPaused = this.isPaused;

            /* ---- ---- 🌠 */
        }

        public void StopCountdown()
        {
            /* ⭐ ---- ---- */

            if (isRunning)
            {
                OnCountdownTick?.Invoke(TimeSpan.Zero);
            }

            isPaused = false;
            isRunning = false;
            this.enabled = false;

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

        [Obsolete]
        async Task CountdownLoop(CancellationToken cancelToken)
        {
            /* ⭐ ---- ---- */

            // * That stays here as a grim tomb of using tpl library without
            // * understainding how it works and how it works in unity

            // try
            // {
            while (true)
            {
                TimeSpan remainingTime = _endTime - DateTime.UtcNow;

                _activeRunningLength = _desiredCountdownLength - remainingTime;

                Debug.Log($"COUN TDOWN LOOP TICK");

                Debug.Log(remainingTime);
                Debug.Log(remainingTime.Minutes);
                Debug.Log(_endTime);

                if (remainingTime < TimeSpan.Zero)
                {
                    Debug.Log($"complete");

                    OnCountdownComplete.Invoke();
                }
                else
                {
                    Debug.Log($"ooops");

                    OnCountdownTick?.Invoke(remainingTime);
                }

                Thread.Sleep(50);

                // * This thing is actually most important'
                // ? Adding token here below is the way to out of this loop
                // https://stackoverflow.com/questions/13695499/proper-way-to-implement-a-never-ending-task-timers-vs-task
                // else could be:
                // token.ThrowIfCancellationRequested();
                // This way the cancellation will happen instantaneously if inside
                // the Task.Delay, rather than having to wait for the Thread.Sleep to finish.
                // await Task.Delay(TICK_LENGTH, cancelToken);
            }
            // }
            // catch(Exception ex)
            // {
            //     throw ex;
            // }
            // catch (TaskCanceledException)
            // {
            //     // TODO Hmm, is it oki? I kindof expect it to happen and whatevs
            //     Debug.Log($"countdown loop cancelled");
            // }

            Debug.Log($"out of loop");
            o_cancellationTokenSource?.Dispose();

            /* ---- ---- 🌠 */
        }

        public TimeSpan GetRunningLength()
        {
            /* ⭐ ---- ---- */

            return _activeRunningLength;

            /* ---- ---- 🌠 */
        }

        #endregion // Private Implementation

        // -------------
    }
}
