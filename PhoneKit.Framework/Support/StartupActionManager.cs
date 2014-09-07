using PhoneKit.Framework.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace PhoneKit.Framework.Support
{
    public enum ActionExecutionRule
    {
        LessThan,
        LessOrEquals,
        Equals,
        MoreOrEquals,
        MoreThan
    }

    /// <summary>
    /// Logs the number of start ups and performs appropriate actions.
    /// </summary>
    public class StartupActionManager
    {
        #region Members

        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static StartupActionManager _instance;

        /// <summary>
        /// The persistent startup counter.
        /// </summary>
        private StoredObject<int> _count = new StoredObject<int>("_startups_count_", 0);

        /// <summary>
        /// The registered actions, which are fired when the startup count is equal.
        /// </summary>
        private Dictionary<int, List<StartupAction>> _actionsEquals = new Dictionary<int, List<StartupAction>>();

        /// <summary>
        /// The registered actions, which are fired when the startup count is less than the specified value.
        /// </summary>
        private Dictionary<int, List<StartupAction>> _actionsLessThan = new Dictionary<int, List<StartupAction>>();

        /// <summary>
        /// The registered actions, which are fired when the startup count is more than the specified value.
        /// </summary>
        private Dictionary<int, List<StartupAction>> _actionsMoreThan = new Dictionary<int, List<StartupAction>>();

        /// <summary>
        /// Indicates whether the actions have already been fired.
        /// </summary>
        private bool _hasFired = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a StartupManager instance.
        /// </summary>
        private StartupActionManager()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers a new action to a specified number of application startups.
        /// </summary>
        /// <param name="startupCount">The number of startups.</param>
        /// <param name="executionRule">The execution rule.</param>
        /// <param name="action">The action to fire.</param>
        /// <param name="persistent">
        /// Specifies whether the action is persistent or a one time shot. This is needed for actions which are probably
        /// not fired when the user doesn't reach the state where the actions are fired.
        /// </param>
        public void Register(int startupCount, ActionExecutionRule executionRule, Action action, bool persistent = false)
        {
            var startupAction = new StartupAction(action, persistent);

            switch (executionRule)
            {
                case ActionExecutionRule.LessThan:
                    AddAction(_actionsLessThan, startupCount, startupAction);
                    break;
                case ActionExecutionRule.LessOrEquals:
                    AddAction(_actionsLessThan, startupCount, startupAction);
                    AddAction(_actionsEquals, startupCount, startupAction);
                    break;
                case ActionExecutionRule.Equals:
                    AddAction(_actionsEquals, startupCount, startupAction);
                    break;
                case ActionExecutionRule.MoreOrEquals:
                    AddAction(_actionsEquals, startupCount, startupAction);
                    AddAction(_actionsMoreThan, startupCount, startupAction);
                    break;
                case ActionExecutionRule.MoreThan:
                    AddAction(_actionsMoreThan, startupCount, startupAction);
                    break;
            }
        }

        /// <summary>
        /// Adds an action to the given dictionary-list container.
        /// </summary>
        /// <param name="container">The dictionary container.</param>
        /// <param name="startupCount">The startup count key.</param>
        /// <param name="action">The startup action.</param>
        private void AddAction(Dictionary<int, List<StartupAction>> container, int startupCount, StartupAction action)
        {
            if (!container.ContainsKey(startupCount))
                container[startupCount] = new List<StartupAction>();

            container[startupCount].Add(action);
        }

        /// <summary>
        /// Checks and fires the appropriate action if there is any
        /// matching action for the current startup count.
        /// <remarks>
        /// Ensures only one action firing per app lifetime, because each fire is equivalent to a startup.
        /// </remarks>
        /// <param name="e">The navigation event args.</param>
        /// </summary>
        public void Fire(NavigationEventArgs e)
        {
            // verify has not already been fired
            if (_hasFired || e.NavigationMode != NavigationMode.New)
                return;

            _count.Value += 1; 
            _hasFired = true;

            // equals
            foreach (var key in _actionsEquals.Keys)
            {
                if (key == Count)
                    FireActions(_actionsEquals[key]);
            }
            // less
            foreach (var key in _actionsLessThan.Keys)
            {
                if (key > Count)
                    FireActions(_actionsLessThan[key]);
            }
            // more
            foreach (var key in _actionsMoreThan.Keys)
            {
                if (key < Count)
                    FireActions(_actionsMoreThan[key]);
            }
        }

        /// <summary>
        /// Fires all active actions.
        /// </summary>
        /// <param name="actions">The actions list</param>
        private void FireActions(List<StartupAction> actions)
        {
            if (actions == null)
                return;

            foreach (var action in actions)
            {
                if (!action.IsActive)
                    continue;

                if (!action.IsPersistent)
                    action.IsActive = false;

                // fire action
                action.Action();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the startup logger instance.
        /// </summary>
        public static StartupActionManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new StartupActionManager();
                return _instance;
            }
        }

        /// <summary>
        /// Gets the number of startups.
        /// </summary>
        public int Count
        {
            get
            {
                return _count.Value;
            }
        }

        /// <summary>
        /// Gets whether the startup actions have already been fired or not.
        /// </summary>
        public bool HasFired
        {
            get
            {
                return _hasFired;
            }
        }

        #endregion
    }
}
