#if DESKTOP
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace RK.Common
{
    public static partial class CommonExtensions
    {
        public static Vector3 ToRKVector(this Vector3D wpfVector)
        {
            return new Vector3((float)wpfVector.X, (float)wpfVector.Y, (float)wpfVector.Z);
        }

        public static Vector3 ToRKVector(this Point3D wpfVector)
        {
            return new Vector3((float)wpfVector.X, (float)wpfVector.Y, (float)wpfVector.Z);
        }

        public static Vector3D ToWpfVector(this Point3D wpfPoint)
        {
            return new Vector3D(wpfPoint.X, wpfPoint.Y, wpfPoint.Z);
        }

        public static Color ToWpfColor(this Color4 color)
        {
            return Color.FromArgb(
                (byte)(color.A * 255),
                (byte)(color.R * 255),
                (byte)(color.G * 255),
                (byte)(color.B * 255));
        }

        /// <summary>
        /// Executes the given action after the given amount of time.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="delayTime">The delay time.</param>
        /// <param name="control">The used for the incoke call.</param>
        public static void InvokeDelayedWpf(this Control control, Action action, TimeSpan delayTime)
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
            System.Threading.Timer delayTimer = null;
            delayTimer = new System.Threading.Timer(
                new TimerCallback((arg) =>
                {
                    //Stop the delay timer
                    delayTimer.Dispose();

                    //Check if the control is still available
                    if (control.IsInitialized)
                    {
                        syncContext.Send(new SendOrPostCallback((innerArg) =>
                        {
                            //Handle exception by global exception handler
                            action();
                        }), null);
                    }
                }),
                null,
                delayTime,
                TimeSpan.FromDays(5));
        }

        /// <summary>
        /// Executes the given method more times until "condition" returns false. 
        /// </summary>
        /// <param name="control">The control used for dispatching.</param>
        /// <param name="condition">The condition to be checked before each call of <paramref name="loopAction"/>.</param>
        /// <param name="loopAction">The action to perform more times.</param>
        /// <param name="delayTime">The interval between callings of <paramref name="loopAction"/>.</param>
        public static void InvokeDelayedWhileWpf(this Control control, Func<bool> condition, Action loopAction, TimeSpan delayTime)
        {
            InvokeDelayedWhileWpf(control, condition, loopAction, delayTime, null);
        }

        /// <summary>
        /// Executes the given method more times until "condition" returns false. 
        /// </summary>
        /// <param name="control">The control used for dispatching.</param>
        /// <param name="condition">The condition to be checked before each call of <paramref name="loopAction"/>.</param>
        /// <param name="loopAction">The action to perform more times.</param>
        /// <param name="delayTime">The interval between callings of <paramref name="loopAction"/>.</param>
        /// <param name="finishingAction">This action is called once when <paramref name="condition"/> returns false.</param>
        public static void InvokeDelayedWhileWpf(this Control control, Func<bool> condition, Action loopAction, TimeSpan delayTime, Action finishingAction)
        {
            InvokeDelayedWhileWpf(control, condition, loopAction, delayTime, null, InvokeDelayedMode.FixedWaitTime);
        }

        /// <summary>
        /// Executes the given method more times until "condition" returns false. 
        /// </summary>
        /// <param name="control">The control used for dispatching.</param>
        /// <param name="condition">The condition to be checked before each call of <paramref name="loopAction"/>.</param>
        /// <param name="loopAction">The action to perform more times.</param>
        /// <param name="delayTime">The interval between callings of <paramref name="loopAction"/>.</param>
        /// <param name="finishingAction">This action is called once when <paramref name="condition"/> returns false.</param>
        public static void InvokeDelayedWhileWpf(this Control control, Func<bool> condition, Action loopAction, TimeSpan delayTime, Action finishingAction, InvokeDelayedMode mode)
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

                    control.InvokeDelayedWpf(outerLoopAction, getDelayTimeFunc((int)stopWatch.ElapsedMilliseconds));
                }
                else
                {
                    if (finishingAction != null) { finishingAction(); }
                }
            };
            InvokeDelayedWpf(control, outerLoopAction, delayTime);
        }
    }
}
#endif