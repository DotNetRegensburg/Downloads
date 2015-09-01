using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Articles.Workflows.ActivityAndThreading.Util
{
    public class ObjectThread
    {
        private const int STANDARD_HEARTBEAT = 500;

        [ThreadStatic]
        private static ObjectThread s_current;

        private ObjectThread m_parent;

        //Members for thread configuration
        private string m_name;
        private volatile int m_heartBeat;
        private bool m_isBackground;

        //Members for thread runtime
        private Stopwatch m_stopWatch;
        private ObjectThreadTimer m_timer;
        private List<Action> m_taskQueue;
        private object m_taskQueueLock;
        private EventWaitHandle m_waitHandle;
        private EventWaitHandle m_stopWaitHandle;
        private List<WorkflowApplication> m_runningActivities;
        private ObjectThreadSynchronizationContext m_syncContext;
        private Thread m_thread;
        private ObjectThreadState m_currentState;

        /// <summary>
        /// Called when the thread ist starting.
        /// </summary>
        public event EventHandler Starting;

        /// <summary>
        /// Called on each heartbeat.
        /// </summary>
        public event EventHandler Tick;

        /// <summary>
        /// Called when the thread is stopping.
        /// </summary>
        public event EventHandler Stopping;

        /// <summary>
        /// Called when an unhandled exception occurred.
        /// </summary>
        public event ObjectThreadExceptionEventHandler ThreadException;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectThread"/> class.
        /// </summary>
        public ObjectThread()
            : this(string.Empty, true)
        {
            m_parent = ObjectThread.Current;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectThread"/> class.
        /// </summary>
        /// <param name="name">The name of the generated thread.</param>
        protected ObjectThread(string name, bool isBackground)
        {
            m_taskQueue = new List<Action>();
            m_taskQueueLock = new object();

            m_name = name;
            m_isBackground = isBackground;
            m_heartBeat = STANDARD_HEARTBEAT;

            m_runningActivities = new List<WorkflowApplication>();

            m_parent = ObjectThread.Current;
            m_timer = new ObjectThreadTimer();
        }

        /// <summary>
        /// Starts the thread.
        /// </summary>
        public void Start()
        {
            if (m_currentState != ObjectThreadState.None) { throw new InvalidOperationException("Unable to start thread: Illegal state: " + m_currentState.ToString() + "!"); }

            //Create the waithandle
            m_waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            m_stopWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

            //Create the thread
            m_thread = new Thread(new ThreadStart(ThreadMainMethod));
            m_thread.Name = m_name;
            m_thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
            m_thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            m_thread.SetApartmentState(ApartmentState.STA);
            m_thread.IsBackground = m_isBackground;

            //Start the thread
            m_currentState = ObjectThreadState.Starting;
            m_thread.Start();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (m_currentState != ObjectThreadState.Running) { throw new InvalidOperationException("Unable to stop thread: Illegal state: " + m_currentState.ToString() + "!"); }
            m_currentState = ObjectThreadState.Stopping;

            //Clears current taskqueue after stopping
            lock (m_taskQueueLock)
            {
                m_taskQueue.Clear();
            }

            //Trigger next update
            this.Trigger();
        }

        /// <summary>
        /// Triggers the stop process and wait until stopped.
        /// </summary>
        /// <param name="timeout">Total time to wait the thread to be stopped.</param>
        public void StopAndWait(TimeSpan timeout)
        {
            if (m_currentState != ObjectThreadState.Running) { throw new InvalidOperationException("Unable to stop thread: Illegal state: " + m_currentState.ToString() + "!"); }
            m_currentState = ObjectThreadState.Stopping;

            //Clears current taskqueue after stopping
            lock (m_taskQueueLock)
            {
                m_taskQueue.Clear();
            }

            //Trigger next update
            this.Trigger();

            //Wait for stopping event
            EventWaitHandle stopWaitHandle = m_stopWaitHandle;
            if (stopWaitHandle != null)
            {
                stopWaitHandle.WaitOne(timeout);
                if (m_currentState != ObjectThreadState.None)
                {
                    throw new ApplicationException("Unable to stop the thread " + this.m_name + ": Timeout");
                }
            }
        }

        /// <summary>
        /// Aborts this thread.
        /// </summary>
        public void Abort()
        {
            if (m_currentState == ObjectThreadState.None) { throw new InvalidOperationException("Unable to stop thread: Illegal state: " + m_currentState.ToString() + "!"); }

            //Abort the thread
            m_thread.Abort();
        }

        /// <summary>
        /// Triggers a new heartbeat.
        /// </summary>
        public virtual void Trigger()
        {
            EventWaitHandle waitHandle = m_waitHandle;
            if (waitHandle != null) { waitHandle.Set(); }
        }

        /// <summary>
        /// Invokes the given delegate within the thread of this object.
        /// </summary>
        /// <param name="delegateToInvoke">The delegate to invoke.</param>
        public void Invoke(Delegate delegateToInvoke)
        {
            if (delegateToInvoke == null) { throw new ArgumentNullException("delegateToInvoke"); }

            if (!this.InvokeRequired)
            {
                //No invoke required, so call method directly
                delegateToInvoke.DynamicInvoke();
            }
            else
            {
                //Prepare variables for calling
                EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
                Exception executeException = null;

                //Append new task to threadqueue
                lock (m_taskQueueLock)
                {
                    m_taskQueue.Add(new Action(() =>
                    {
                        try { delegateToInvoke.DynamicInvoke(); }
                        catch (Exception ex) { executeException = ex; }
                        waitHandle.Set();
                    }));
                }

                //Wait until action has finished (ensur synchron call)
                this.Trigger();
                waitHandle.WaitOne();

                //Throw exception in current thread if there was raised one
                if (executeException != null)
                {
                    throw new TargetInvocationException(executeException);
                }
            }
        }

        /// <summary>
        /// Invokes the given delegate within the thread of this object.
        /// </summary>
        /// <param name="delegateToInvoke">The delegate to invoke.</param>
        /// <param name="parameters">Parameters to be passed to the given delegate.</param>
        public void Invoke(Delegate delegateToInvoke, params object[] parameters)
        {
            if (delegateToInvoke == null) { throw new ArgumentNullException("delegateToInvoke"); }

            if (!this.InvokeRequired)
            {
                //No invoke required, so call method directly
                delegateToInvoke.DynamicInvoke(parameters);
            }
            else
            {
                //Prepare variables for calling
                EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
                Exception executeException = null;

                //Append new task to threadqueue
                lock (m_taskQueueLock)
                {
                    m_taskQueue.Add(new Action(() =>
                    {
                        try { delegateToInvoke.DynamicInvoke(parameters); }
                        catch (Exception ex) { executeException = ex; }
                        waitHandle.Set();
                    }));
                }

                //Wait until action has finished (ensur synchron call)
                this.Trigger();
                waitHandle.WaitOne();

                //Throw exception in current thread if there was raised one
                if (executeException != null)
                {
                    throw new TargetInvocationException(executeException);
                }
            }
        }

        /// <summary>
        /// Executes the given activity in the scope of this ObjectThread.
        /// </summary>
        /// <param name="activity">The activity to be executed.</param>
        public void BeginInvoke(Activity activity)
        {
            this.BeginInvoke(activity, null);
        }

        /// <summary>
        /// Executes the given activity in the scope of this ObjectThread.
        /// </summary>
        /// <param name="activity">The activity to be executed.</param>
        /// <param name="customizeApplicationAction">A lambda that can manipulate the generated WorkflowApplication object.</param>
        public void BeginInvoke(Activity activity, Action<WorkflowApplication> customizeApplicationAction)
        {
            Action addAction = () =>
            {
                WorkflowApplication workflowApplication = new WorkflowApplication(activity);
                workflowApplication.SynchronizationContext = m_syncContext;
                workflowApplication.Extensions.Add(this);
                workflowApplication.Completed = (eArgs) =>
                {
                    m_runningActivities.Remove(workflowApplication);
                    OnActivityCompleted(workflowApplication, eArgs);
                };
                workflowApplication.Aborted = (eArgs) =>
                {
                    m_runningActivities.Remove(workflowApplication);
                    OnActivityAborted(workflowApplication, eArgs);
                };

                if (customizeApplicationAction != null)
                {
                    customizeApplicationAction(workflowApplication);
                }

                workflowApplication.Run();
                m_runningActivities.Add(workflowApplication);
            };

            this.BeginInvoke(addAction);
        }

        /// <summary>
        /// Invokes the given delegate within the thread of this object.
        /// </summary>
        /// <param name="delegateToInvoke">The delegate to invoke.</param>
        public void BeginInvoke(Delegate delegateToInvoke)
        {
            if (delegateToInvoke == null) { throw new ArgumentNullException("delegateToInvoke"); }

            lock (m_taskQueueLock)
            {
                m_taskQueue.Add(new Action(() => delegateToInvoke.DynamicInvoke()));
            }

            this.Trigger();
        }

        /// <summary>
        /// Invokes the given delegate within the thread of this object.
        /// </summary>
        /// <param name="delegateToInvoke">The delegate to invoke.</param>
        /// <param name="parameters">Parameters to be passed to the given delegate.</param>
        public void BeginInvoke(Delegate delegateToInvoke, params object[] parameters)
        {
            if (delegateToInvoke == null) { throw new ArgumentNullException("delegateToInvoke"); }

            lock (m_taskQueueLock)
            {
                m_taskQueue.Add(new Action(() => delegateToInvoke.DynamicInvoke(parameters)));
            }

            this.Trigger();
        }

        /// <summary>
        /// Thread is starting.
        /// </summary>
        protected virtual void OnStarting(EventArgs eArgs)
        {
            if (Starting != null) { Starting(this, eArgs); }
        }

        /// <summary>
        /// Called on each tick.
        /// </summary>
        protected virtual void OnTick(EventArgs eArgs)
        {
            if (Tick != null) { Tick(this, eArgs); }
        }

        /// <summary>
        /// Called on each occurred exception.
        /// </summary>
        protected virtual void OnThreadException(ObjectThreadExceptionEventArgs eArgs)
        {
            if (ThreadException != null) { ThreadException(this, eArgs); }
        }

        /// <summary>
        /// Thread is stopping.
        /// </summary>
        protected virtual void OnStopping(EventArgs eArgs)
        {
            if (Stopping != null) { Stopping(this, eArgs); }
        }

        /// <summary>
        /// Called when an activity has completed.
        /// </summary>
        /// <param name="wfApplication">The WorkflowApplication that has completed.</param>
        /// <param name="eArgs">Some event args.</param>
        private void OnActivityCompleted(WorkflowApplication wfApplication, WorkflowApplicationCompletedEventArgs eArgs)
        {
            
        }

        /// <summary>
        /// Called when an activity has canceled.
        /// </summary>
        /// <param name="wfApplication">The WorkflowApplication that has canceled.</param>
        /// <param name="eArgs">Some event args.</param>
        private void OnActivityAborted(WorkflowApplication wfApplication, WorkflowApplicationAbortedEventArgs eArgs)
        {

        }

        /// <summary>
        /// The thread's main method.
        /// </summary>
        [STAThread]
        private void ThreadMainMethod()
        {
            try
            {
                m_stopWatch = new Stopwatch();
                m_stopWatch.Start();
                s_current = this;

                //Set synchronization context for this thread
                m_syncContext = new ObjectThreadSynchronizationContext(this);
                SynchronizationContext.SetSynchronizationContext(m_syncContext);

                //Notify start process
                try { OnStarting(EventArgs.Empty); }
                catch (Exception ex)
                {
                    OnThreadException(new ObjectThreadExceptionEventArgs(m_currentState, ex));
                    m_currentState = ObjectThreadState.None;
                }

                //Run main-thread
                if (m_currentState != ObjectThreadState.None)
                {
                    m_currentState = ObjectThreadState.Running;
                    while (m_currentState == ObjectThreadState.Running)
                    {
                        try
                        {
                            //Wait for next action to perform
                            if (m_heartBeat > 0)
                            {
                                m_waitHandle.WaitOne(m_heartBeat);
                            }

                            //Measure current time
                            m_stopWatch.Stop();
                            m_timer.Add(m_stopWatch.Elapsed);
                            m_stopWatch.Reset();
                            m_stopWatch.Start();

                            //Get current taskqueue
                            List<Action> localTaskQueue = new List<Action>();
                            lock (m_taskQueueLock)
                            {
                                localTaskQueue.AddRange(m_taskQueue);
                                m_taskQueue.Clear();
                            }

                            //Execute all tasks
                            foreach (Action actTask in localTaskQueue)
                            {
                                try { actTask(); }
                                catch (Exception ex)
                                {
                                    OnThreadException(new ObjectThreadExceptionEventArgs(m_currentState, ex));
                                }
                            }

                            //Perfoms a tick
                            OnTick(EventArgs.Empty);
                        }
                        catch (Exception ex)
                        {
                            OnThreadException(new ObjectThreadExceptionEventArgs(m_currentState, ex));
                        }
                    }

                    //Notify stop process
                    try { OnStopping(EventArgs.Empty); }
                    catch (Exception ex)
                    {
                        OnThreadException(new ObjectThreadExceptionEventArgs(m_currentState, ex));
                    }
                }

                m_thread = null;
                m_waitHandle = null;
                m_currentState = ObjectThreadState.None;

                //Signal sopped event
                m_stopWaitHandle.Set();

                m_stopWatch.Stop();
                m_stopWatch = null;
            }
            catch (ThreadAbortException)
            {
                m_thread = null;
                m_waitHandle = null;
                m_currentState = ObjectThreadState.None;

                m_stopWatch.Stop();
                m_stopWatch = null;
            }
        }

        /// <summary>
        /// Is invoke call required?
        /// </summary>
        public bool InvokeRequired
        {
            get { return m_thread != Thread.CurrentThread; }
        }

        /// <summary>
        /// Gets or sets the thread's heartbeat.
        /// </summary>
        protected int HeartBeat
        {
            get { return m_heartBeat; }
            set { m_heartBeat = value; }
        }

        /// <summary>
        /// Gets the parent thread.
        /// </summary>
        public ObjectThread Parent
        {
            get { return m_parent; }
        }

        /// <summary>
        /// Gets current ObjectThread object.
        /// </summary>
        public static ObjectThread Current
        {
            get { return s_current; }
        }

        /// <summary>
        /// Gets current thread time.
        /// </summary>
        public DateTime ThreadTime
        {
            get { return m_timer.Now; }
        }

        /// <summary>
        /// Gets current timer of the thread.
        /// </summary>
        public ObjectThreadTimer Timer
        {
            get { return m_timer; }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// Synchronization object for threads within ObjectThread class.
        /// </summary>
        private class ObjectThreadSynchronizationContext : SynchronizationContext
        {
            private ObjectThread m_owner;

            /// <summary>
            /// Initializes a new instance of the <see cref="ObjectThreadSynchronizationContext"/> class.
            /// </summary>
            /// <param name="owner">The owner of this context.</param>
            public ObjectThreadSynchronizationContext(ObjectThread owner)
            {
                m_owner = owner;
            }

            /// <summary>
            /// When overridden in a derived class, dispatches an asynchronous message to a synchronization context.
            /// </summary>
            /// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback"/> delegate to call.</param>
            /// <param name="state">The object passed to the delegate.</param>
            public override void Post(SendOrPostCallback d, object state)
            {
                m_owner.BeginInvoke(d, state);
            }

            /// <summary>
            /// When overridden in a derived class, dispatches a synchronous message to a synchronization context.
            /// </summary>
            /// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback"/> delegate to call.</param>
            /// <param name="state">The object passed to the delegate.</param>
            public override void Send(SendOrPostCallback d, object state)
            {
                m_owner.Invoke(d, state);
            }
        }
    }
}
