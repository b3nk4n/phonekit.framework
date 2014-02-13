using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneKit.Framework.Controls
{
    /// <summary>
    /// The contributors model to add to the about page.
    /// </summary>
    public class ContributorModel
    {
        /// <summary>
        /// The icon path to flag or symbol
        /// </summary>
        public string IconPath { get; private set; }

        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Creates a contributor model instance.
        /// </summary>
        /// <param name="iconPath">The icon path, use e.g. <code>"/Assets/Languages/italy.png"</code>.</param>
        /// <param name="name">The name.</param>
        public ContributorModel(string iconPath, string name)
        {
            IconPath = iconPath;
            Name = name;
        }
    }
}
