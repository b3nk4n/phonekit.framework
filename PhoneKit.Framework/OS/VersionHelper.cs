using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PhoneKit.Framework.OS
{
    /// <summary>
    /// Helper class for version information.
    /// </summary>
    public static class VersionHelper
    {
        /// <summary>
        /// The GDR3 update version of Windows Phone 8.
        /// </summary>
        public static readonly Version VersionGDR3 = new Version(8, 0, 10492);

        /// <summary>
        /// The initial version of Windows Phone 8.1 realease.
        /// </summary>
        public static readonly Version Version8_1 = new Version(8, 10);

        /// <summary>
        /// Indicates whether the phone is GDR3 update compatible, which is required
        /// for some features.
        /// </summary>
        public static bool IsPhoneGDR3
        {
            get { return Environment.OSVersion.Version >= VersionGDR3; }
        }

        /// <summary>
        /// Indicates whether the phone is Windows Phone 8.1 compatible.
        /// </summary>
        public static bool IsPhoneWP8_1
        {
            get { return Environment.OSVersion.Version >= VersionGDR3; }
        }

        /// <summary>
        /// Gets the version of the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The version of the assembly.</returns>
        public static Version GetVersion(Assembly assembly)
        {
            AssemblyName an = new AssemblyName(assembly.FullName);
            return an.Version;
        }

        /// <summary>
        /// Gets the version text of the assemby in the format 'X.Y' with major and minor.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The version text of the assembly.</returns>
        public static string GetVersionText(Assembly assembly)
        {
            var version = GetVersion(assembly);
            return string.Format("{0}.{1}", version.Major, version.Minor);
        }

        /// <summary>
        /// Gets the version of the PhoneKit Framework.
        /// </summary>
        /// <returns>The version of the assembly.</returns>
        public static Version GetFrameworkVersion()
        {
            AssemblyName an = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            return an.Version;
        }

        /// <summary>
        /// Gets the version text of the PhoneKit Framework in the format 'X.Y' with major and minor.
        /// </summary>
        /// <returns>The version text of the assembly.</returns>
        public static string GetFrameworkVersionText()
        {
            var version = GetFrameworkVersion();
            return string.Format("{0}.{1}", version.Major, version.Minor);
        }
    }
}
