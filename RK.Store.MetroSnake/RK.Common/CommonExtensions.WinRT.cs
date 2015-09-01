using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if WINRT
using Windows.System.Threading;
using Windows.UI.Xaml;
#endif

namespace RK.Common
{
#if WINRT
    public static partial class CommonExtensions
    {
        /// <summary>
        /// Executes the given action after the given amount of time.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="delayTime">The delay time.</param>
        /// <param name="control">The used for the incoke call.</param>
        public static void InvokeDelayed(this UIElement control, Action action, TimeSpan delayTime)
        {
            //Gets current Synchronization context
            SynchronizationContext syncContext = SynchronizationContext.Current;
            if (syncContext == null)
            {
                throw new InvalidOperationException("No SynchronizationContext is available on current thread!");
            }
            if (syncContext.GetType() == typeof(SynchronizationContext))
            {
                throw new InvalidOperationException("This method is not available on default synchronization context!");
            }

            //Start a timer object wich fires an event after delay time
            ThreadPoolTimer.CreateTimer(
                new TimerElapsedHandler((arg) =>
                {
                    try
                    {
                        //Check if the control is still available
                        syncContext.Post(new SendOrPostCallback((innerArg) =>
                        {
                            //Handle exception by global exception handler
                            action();
                        }), null);
                    }
                    catch { }
                }),
                delayTime);
        }

        /// <summary>
        /// Executes the given method more times until "condition" returns false. 
        /// </summary>
        /// <param name="control">The control used for dispatching.</param>
        /// <param name="condition">The condition to be checked before each call of <paramref name="loopAction"/>.</param>
        /// <param name="loopAction">The action to perform more times.</param>
        /// <param name="delayTime">The interval between callings of <paramref name="loopAction"/>.</param>
        public static void InvokeDelayedWhile(this UIElement control, Func<bool> condition, Action loopAction, TimeSpan delayTime)
        {
            InvokeDelayedWhile(control, condition, loopAction, delayTime, null);
        }

        /// <summary>
        /// Executes the given method more times until "condition" returns false. 
        /// </summary>
        /// <param name="control">The control used for dispatching.</param>
        /// <param name="condition">The condition to be checked before each call of <paramref name="loopAction"/>.</param>
        /// <param name="loopAction">The action to perform more times.</param>
        /// <param name="delayTime">The interval between callings of <paramref name="loopAction"/>.</param>
        /// <param name="finishingAction">This action is called once when <paramref name="condition"/> returns false.</param>
        public static void InvokeDelayedWhile(this UIElement control, Func<bool> condition, Action loopAction, TimeSpan delayTime, Action finishingAction)
        {
            InvokeDelayedWhile(control, condition, loopAction, delayTime, null, InvokeDelayedMode.FixedWaitTime);
        }

        /// <summary>
        /// Executes the given method more times until "condition" returns false. 
        /// </summary>
        /// <param name="control">The control used for dispatching.</param>
        /// <param name="condition">The condition to be checked before each call of <paramref name="loopAction"/>.</param>
        /// <param name="loopAction">The action to perform more times.</param>
        /// <param name="delayTime">The interval between callings of <paramref name="loopAction"/>.</param>
        /// <param name="finishingAction">This action is called once when <paramref name="condition"/> returns false.</param>
        public static void InvokeDelayedWhile(this UIElement control, Func<bool> condition, Action loopAction, TimeSpan delayTime, Action finishingAction, InvokeDelayedMode mode)
        {
            Func<int, TimeSpan> getDelayTimeFunc = null;
            switch (mode)
            {
                case InvokeDelayedMode.EnsuredTimerInterval:
                    getDelayTimeFunc = (neededTime) =>
                    {
                        int remainingMilliseconds = (int)delayTime.TotalMilliseconds - neededTime;

                        if (remainingMilliseconds < 5) { return TimeSpan.FromMilliseconds(5.0); }
                        else { return TimeSpan.FromMilliseconds(remainingMilliseconds); }
                    };
                    break;

                case InvokeDelayedMode.FixedWaitTime:
                    getDelayTimeFunc = (neededTime) => delayTime;
                    break;
            }

            Action outerLoopAction = null;
            outerLoopAction = () =>
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                if (condition())
                {
                    loopAction();

                    stopWatch.Stop();


                    InvokeDelayed(control, outerLoopAction, getDelayTimeFunc((int)stopWatch.ElapsedMilliseconds));
                }
                else
                {
                    if (finishingAction != null) { finishingAction(); }
                }
            };
            InvokeDelayed(control, outerLoopAction, delayTime);
        }
    }
#endif
}
