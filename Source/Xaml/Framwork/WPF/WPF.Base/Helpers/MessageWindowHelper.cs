using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using WPF.Base.Controls;

namespace WPF.Base.Helpers
{
    public static class MessageWindowHelper
    {
        static MetroDialogSettings settings = new MetroDialogSettings();
        public static async Task<MessageDialogResult> ShowMessage(object context, string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            settings.DialogMessageFontSize = 24;
            settings.DialogTitleFontSize = 36;
            return await DialogCoordinator.Instance.ShowMessageAsync(context, title, message, style, settings);
        }

        public static async Task<ProgressDialogController> ShowProgressAsync(object context, string title, string message, bool isCancelable = false)
        {
            settings.DialogMessageFontSize = 24;
            settings.DialogTitleFontSize = 36;
            return await DialogCoordinator.Instance.ShowProgressAsync(context, title, message, isCancelable, settings);
        }

        public static async Task ShowProgress(string title, string description, Action action, CancellationTokenSource token = null, bool isAutoClose = false)
        {
            await Application.Current.MainWindow.ShowChildWindowAsync(new UnieyeProgressControl(title, description, action, token, isAutoClose));
        }

        public static async Task ShowProgress(string title, string description, List<Action> actionList, CancellationTokenSource token = null)
        {
            await Application.Current.MainWindow.ShowChildWindowAsync(new UnieyeProgressControl(title, description, actionList, token));
        }
    }
}
