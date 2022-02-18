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

        const int TICK_LENGTH = 10;
        CancellationTokenSource o_cancellationTokenSource;
        DateTime _endTime;
        DateTime pauseTime;
        bool isRunning = false;
        bool isPaused = false;

        void Update()
        {
            // -------------
            // 
            

            if (isRunning)
            {
            Debug.Log($"asdfasdf");

                if (isPaused is false)
                {
            Debug.Log($"111111111111");

                    TimeSpan remainingTime = _endTime - DateTime.UtcNow;

                    if (remainingTime < TimeSpan.Zero)
                    {
                        OnCountdownComplete?.Invoke();
                    }
                    else
                    {
                        Debug.Log($"ooops");

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

            Debug.Log($"help");
            

            if (isRunning)
            {
                throw new Exception("Countdown is already running.");
            }

            var start = DateTime.UtcNow; // Use UtcNow instead of Now
            _endTime = start.AddTicks(length.Ticks);

            this.enabled = true;

            Debug.Log($"yuppp");


            isRunning = true;

            /* ---- ---- 🌠 */
        }

        public void ToggleCountdownPause()
        {
            /* ⭐ ---- ---- */

            if (isPaused)
            {
                TimeSpan timeDifference = DateTime.UtcNow - pauseTime;
                _endTime += timeDifference;
            }
            else
            {
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

            this.enabled = false;

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

        private bool IsPastEndtime(out TimeSpan remainingTime)
        {
            throw new NotImplementedException();
        }

        #endregion // Private Implementation

        // -------------
    }
}
