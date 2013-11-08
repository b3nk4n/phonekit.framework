using System;
using System.Windows.Input;

namespace PhoneKit.Framework.MVVM
{
    /// <summary>
    /// Represents a command which is reusable among all the view models of the MVVM pattern.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        #region Members

        /// <summary>
        /// The binded action.
        /// </summary>
        private readonly Action _execute;

        /// <summary>
        /// The validator to check if the action can be executed.
        /// </summary>
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// The changed event handler for can execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a DelegateCommand instance.
        /// </summary>
        /// <param name="execute">Action to be launched when the command is executed.</param>
        public DelegateCommand(Action execute)
            : this(execute, null) { }

        /// <summary>
        /// Creates a DelegateCommand instance.
        /// </summary>
        /// <param name="execute">Action to be launched when the command is executed.</param>
        /// <param name="canExecute">Func to be executed to evaluate if a command can or can´t be executed.</param>
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// ICommand call this method to evaluate if the command can be executed.
        /// When called, invoke the Func we have stored in canExecute if it is null return always true.
        /// </summary>
        /// <param name="parameter">Command parameter, which is ignored in this implementation.</param>
        /// <returns>True if the command can be execute, otherwise false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        /// <summary>
        /// ICommand call this method to execute the command action.
        /// When called, invoke the Action we have stored in execute if it isn´t null.
        /// </summary>
        /// <param name="parameter">Command parameter, ignored in this implementation.</param>
        public void Execute(object parameter)
        {
            if (_execute != null)
                _execute();
        }

        /// <summary>
        /// This method can be used to manually launch the command CanExecute evaluation.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
                handler(this, new EventArgs());
        }

        #endregion
    }

    /// <summary>
    /// Represents a generic command which is reusable among all the view models of the MVVM pattern.
    /// </summary>
    /// <typeparam name="T">Type to use in the Execute and CanExecute parameters.</typeparam>
    public class DelegateCommand<T> : ICommand
    {
        #region Members

        /// <summary>
        /// The binded action.
        /// </summary>
        private readonly Action<T> _execute;

        /// <summary>
        /// The validator to check if the action can be executed.
        /// </summary>
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// The changed event handler for can execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a generic DelegateCommand instance.
        /// </summary>
        /// <param name="execute">Action to be launched when the command is executed.</param>
        public DelegateCommand(Action<T> execute)
            : this(execute, null) { }

        /// <summary>
        /// Creates a generic DelegateCommand instance.
        /// </summary>
        /// <param name="execute">Action to be launched when the command is executed.</param>
        /// <param name="canExecute">Func to be executed to evaluate if a command can or can´t be executed.</param>
        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// ICommand call this method to evaluate if the command can be executed.
        /// When called, invoke the Func we have stored in canExecute if it is null return always true.
        /// </summary>
        /// <param name="parameter">Command parameter, we try to cast it to T.</param>
        /// <returns>True if the command can be execute, otherwise false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        /// <summary>
        /// ICommand call this method to execute the command action.
        /// When called, invoke the Action we have stored in execute if it isn´t null.
        /// </summary>
        /// <param name="parameter">Command parameter, we try to cast it to T.</param>
        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                if (parameter == null)
                    _execute(default(T));
                else
                    _execute((T)parameter);
            }
        }

        /// <summary>
        /// This method can be used to manually launch the command CanExecute evaluation.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
                handler(this, new EventArgs());
        }

        #endregion
    }
}
