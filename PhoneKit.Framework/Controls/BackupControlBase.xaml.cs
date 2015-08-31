using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PhoneKit.Framework.Controls
{
    /// <summary>
    /// The backup control base.
    /// </summary>
    public abstract partial class BackupControlBase : UserControl
    {
        /// <summary>
        /// Gets or sets the background theme color.
        /// </summary>
        public SolidColorBrush BackgroundTheme
        {
            get { return (SolidColorBrush)GetValue(BackgroundThemeProperty); }
            set { SetValue(BackgroundThemeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundTheme.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundThemeProperty =
            DependencyProperty.Register("BackgroundTheme", typeof(SolidColorBrush), typeof(BackupControlBase), new PropertyMetadata(Application.Current.Resources["PhoneAccentBrush"]));

        public BackupControlBase()
        {
            InitializeComponent();
            LocalizeContent();
        }

        /// <summary>
        /// Localizes the user control content and texts.
        /// </summary>
        protected abstract void LocalizeContent();

        #region Properies

        public string CreateBackupHeaderText
        {
            set
            {
                BackupHeader.Text = value;
            }
        }

        public string RestoreBackupHeaderText
        {
            set
            {
                RestoreHeader.Text = value;
            }
        }

        public string NameOfBackupHintText
        {
            set
            {
                TextBoxBackupName.Hint = value;
            }
        }

        public string BackupInfoText
        {
            set
            {
                BackupInfo.Text = value;
            }
        }

        public string AttentionTitle
        {
            set
            {
                AttentionText1.Text = value;
                AttentionText2.Text = value;
            }
        }

        public string RestoreInfoText
        {
            set
            {
                RestoreInfo.Text = value;
            }
        }

        public string CommonBackupWarningText
        {
            set
            {
                CommonBackupWarning1.Text = value;
                CommonBackupWarning2.Text = value;
            }
        }

        public string LoginText
        {
            set
            {
                LoginButtonText.Text = value;
            }
        }

        #endregion
    }
}
