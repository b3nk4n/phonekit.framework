using PhoneKit.Framework.Controls;
using PhoneKit.TestApp.Resources;

namespace PhoneKit.TestApp.Controls
{
    public class MyBackupControl : BackupControlBase
    {
        public MyBackupControl()
        {
            DataContext = new MyBackupControlViewModel();
        }

        /// <summary>
        /// Localizes the user controls contents and texts.
        /// </summary>
        protected override void LocalizeContent()
        {
            CreateBackupHeaderText = AppResources.CreateBackupHeaderText;
            RestoreBackupHeaderText = AppResources.RestoreBackupHeaderText;
            NameOfBackupHintText = AppResources.NameOfBackupHintText;
            BackupInfoText = AppResources.BackupInfoText;
            AttentionTitle = AppResources.AttentionTitle;
            RestoreInfoText = AppResources.RestoreInfoText;
            CommonBackupWarningText = AppResources.CommonBackupWarningText;
            LoginText = AppResources.Login;
        }
    }
}
