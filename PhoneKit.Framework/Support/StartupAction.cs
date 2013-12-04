using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneKit.Framework.Support
{
    /// <summary>
    /// A startup action container.
    /// </summary>
    internal class StartupAction
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// Gets or sets whether the action has already been fired.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets whether the action is persistent or a one time shot.
        /// </summary>
        public bool IsPersistent { get; private set; }

        /// <summary>
        /// Creates a StartupAction instance.
        /// </summary>
        /// <param name="action">The startup action.</param>
        public StartupAction(Action action, bool persistent)
        {
            Action = action;
            IsActive = true;
            IsPersistent = persistent;
        }
    }
}
