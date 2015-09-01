#if DESKTOP
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace RK.Common.Mvvm
{
    /// <summary>
    /// This class allows delegating the commanding logic to methods passed as parameters,
    /// and enables a View to bind commands to objects that are not part of the element tree.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action m__executeMethod = null;
        private readonly Func<bool> m_canExecuteMethod = null;
        private bool m_isAutomaticRequeryDisabled = false;
        private List<WeakReference> m_canExecuteChangedHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="executeMethod">The action to execute.</param>
        public DelegateCommand(Action executeMethod)
            : this(executeMethod, null, false)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="executeMethod">The action to execute.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <param name="isAutomaticRequeryDisabled">if set to <c>true</c> [is automatic requery disabled].</param>
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null)
            {
                throw new ArgumentNullException("executeMethod");
            }

            m__executeMethod = executeMethod;
            m_canExecuteMethod = canExecuteMethod;
            m_isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        /// <summary>
        /// Method to determine if the command can be executed
        /// </summary>
        public bool CanExecute()
        {
            if (m_canExecuteMethod != null)
            {
                return m_canExecuteMethod();
            }
            return true;
        }

        /// <summary>
        /// Execution of the command
        /// </summary>
        public void Execute()
        {
            if (m__executeMethod != null)
            {
                m__executeMethod();
            }
        }

        /// <summary>
        /// Raises the CanExecuteChaged event
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        ///     Protected virtual method to raise CanExecuteChanged event
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            CommandManagerHelper.CallWeakReferenceHandlers(m_canExecuteChangedHandlers);
        }

        /// <summary>
        /// Can execute this command?
        /// </summary>
        /// <param name="parameter">A parameter passed to the query.</param>
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">A parameter passed to the query.</param>
        void ICommand.Execute(object parameter)
        {
            Execute();
        }

        /// <summary>
        /// Property to enable or disable CommandManager's automatic requery on this command
        /// </summary>
        public bool IsAutomaticRequeryDisabled
        {
            get
            {
                return m_isAutomaticRequeryDisabled;
            }
            set
            {
                if (m_isAutomaticRequeryDisabled != value)
                {
                    if (value)
                    {
                        CommandManagerHelper.RemoveHandlersFromRequerySuggested(m_canExecuteChangedHandlers);
                    }
                    else
                    {
                        CommandManagerHelper.AddHandlersToRequerySuggested(m_canExecuteChangedHandlers);
                    }
                    m_isAutomaticRequeryDisabled = value;
                }
            }
        }

        /// <summary>
        /// Raised when the command changes its state.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!m_isAutomaticRequeryDisabled)
                {
                    CommandManager.RequerySuggested += value;
                }
                CommandManagerHelper.AddWeakReferenceHandler(ref m_canExecuteChangedHandlers, value, 2);
            }
            remove
            {
                if (!m_isAutomaticRequeryDisabled)
                {
                    CommandManager.RequerySuggested -= value;
                }
                CommandManagerHelper.RemoveWeakReferenceHandler(m_canExecuteChangedHandlers, value);
            }
        }
    }

    /// <summary>
    ///     This class allows delegating the commanding logic to methods passed as parameters,
    ///     and enables a View to bind commands to objects that are not part of the element tree.
    /// </summary>
    /// <typeparam name="T">Type of the parameter passed to the delegates</typeparam>
    public class DelegateCommand<T> : ICommand
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null, false)
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false)
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null)
            {
                throw new ArgumentNullException("executeMethod");
            }

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
            _isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Method to determine if the command can be executed
        /// </summary>
        public bool CanExecute(T parameter)
        {
            if (_canExecuteMethod != null)
            {
                return _canExecuteMethod(parameter);
            }
            return true;
        }

        /// <summary>
        ///     Execution of the command
        /// </summary>
        public void Execute(T parameter)
        {
            if (_executeMethod != null)
            {
                _executeMethod(parameter);
            }
        }

        /// <summary>
        ///     Raises the CanExecuteChaged event
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        ///     Protected virtual method to raise CanExecuteChanged event
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            CommandManagerHelper.CallWeakReferenceHandlers(_canExecuteChangedHandlers);
        }

        /// <summary>
        ///     Property to enable or disable CommandManager's automatic requery on this command
        /// </summary>
        public bool IsAutomaticRequeryDisabled
        {
            get
            {
                return _isAutomaticRequeryDisabled;
            }
            set
            {
                if (_isAutomaticRequeryDisabled != value)
                {
                    if (value)
                    {
                        CommandManagerHelper.RemoveHandlersFromRequerySuggested(_canExecuteChangedHandlers);
                    }
                    else
                    {
                        CommandManagerHelper.AddHandlersToRequerySuggested(_canExecuteChangedHandlers);
                    }
                    _isAutomaticRequeryDisabled = value;
                }
            }
        }

        #endregion

        #region ICommand Members

        /// <summary>
        ///     ICommand.CanExecuteChanged implementation
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!_isAutomaticRequeryDisabled)
                {
                    CommandManager.RequerySuggested += value;
                }
                CommandManagerHelper.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);
            }
            remove
            {
                if (!_isAutomaticRequeryDisabled)
                {
                    CommandManager.RequerySuggested -= value;
                }
                CommandManagerHelper.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            // if T is of value type and the parameter is not
            // set yet, then return false if CanExecute delegate
            // exists, else return true
            if (parameter == null &&
                typeof(T).IsValueType)
            {
                return (_canExecuteMethod == null);
            }
            return CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }

        #endregion

        #region Data

        private readonly Action<T> _executeMethod = null;
        private readonly Func<T, bool> _canExecuteMethod = null;
        private bool _isAutomaticRequeryDisabled = false;
        private List<WeakReference> _canExecuteChangedHandlers;

        #endregion
    }
}
#endif

#if WINRT
using System;

namespace RK.Common.Mvvm
{
    public class DelegateCommand : System.Windows.Input.ICommand
    {
        private readonly Action m_Execute;
        private readonly Func<bool> m_CanExecute;
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action execute)
            : this(execute, () => true) { /* empty */ }

        public DelegateCommand(Action execute, Func<bool> canexecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            m_Execute = execute;
            m_CanExecute = canexecute;
        }

        public bool CanExecute(object p)
        {
            return m_CanExecute == null ? true : m_CanExecute();
        }

        public void Execute(object p)
        {
            if (CanExecute(null))
                m_Execute();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
#endif