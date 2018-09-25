using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace email_checker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ApplicationTrigger _AppTrigger;
        public MainPage()
        {
            this.InitializeComponent();
            _AppTrigger = new ApplicationTrigger();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            UserNotificationListener c = UserNotificationListener.Current;
            UserNotificationListenerAccessStatus x = await c.RequestAccessAsync();

            var toast = await c.GetNotificationsAsync(Windows.UI.Notifications.NotificationKinds.Toast);
            foreach (var notif in toast)
            {
                var notification = notif.Notification;
                StringBuilder stringBuilder = new StringBuilder();
                var appname = notif.AppInfo.DisplayInfo.DisplayName;
                if (appname != "Mail")
                    continue;
                foreach (NotificationBinding binding in notification.Visual.Bindings)
                {
                    foreach (var textElelment in binding.GetTextElements())
                    {
                        stringBuilder.Append(textElelment.Text);
                        stringBuilder.Append("\n");


                    }
                    ContentDialog mydialog = new ContentDialog
                    {
                        Title = appname,
                        Content = stringBuilder.ToString(),
                        CloseButtonText = "ok"
                    };
                    ContentDialogResult contentDialogResult = await mydialog.ShowAsync();
                }
            }
        }
        private async void Show_Dialog_Messages()
        {
            headers.Text = "";
            UserNotificationListener c = UserNotificationListener.Current;
            UserNotificationListenerAccessStatus x = await c.RequestAccessAsync();
            int i = 100;
            var toast = await c.GetNotificationsAsync(Windows.UI.Notifications.NotificationKinds.Toast);
            foreach (var notif in toast)
            {
                var notification = notif.Notification;
                StringBuilder stringBuilder = new StringBuilder();
                var appname = notif.AppInfo.DisplayInfo.DisplayName;
                if (appname != "Mail")
                    continue;
                foreach (NotificationBinding binding in notification.Visual.Bindings)
                {
                    foreach (var textElelment in binding.GetTextElements())
                    {
                        stringBuilder.Append(textElelment.Text);
                        stringBuilder.Append("\n");
                    }
                    SendToast(appname + "\n" + stringBuilder.ToString(), i.ToString());
                    headers.Text = headers.Text + "\n" + stringBuilder.ToString();
                    //ContentDialog mydialog = new ContentDialog
                    //{
                    //    Title = appname,
                    //    Content = stringBuilder.ToString(),
                    //    CloseButtonText = "ok"
                    //};
                    //try
                    //{
                    //    ContentDialogResult contentDialogResult = await mydialog.ShowAsync();
                    //}
                    //catch (System.Runtime.InteropServices.COMException e)
                    //{
                    //    SendToast(
                    //    e.Message);
                    //}

                    i = i + 1;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // system.timers.timer is not available here.  What is?
            DispatcherTimer t = new DispatcherTimer();
            t.Interval = new System.TimeSpan(0, 0, 1, 0);
            Show_Dialog_Messages();
            t.Tick += DispatcherTimer_Tick;
            t.Start();

        }
        private void SendToast(String message, String tag)
        {
        
            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = "title"
                        },
                        new AdaptiveText()
                        {
                            Text = message
                        }
                    }
                }
            };
            ToastContent toastContent = new ToastContent()
            {
                Visual = visual
            };
            var toast = new ToastNotification(toastContent.GetXml());
            toast.ExpirationTime = DateTime.Now.AddSeconds(60);
            toast.Tag = tag;
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
        void DispatcherTimer_Tick(object sender, object e)
        {
            Show_Dialog_Messages();
        }

        private async void bgrun_Click(object sender, RoutedEventArgs e)
            // ui element removed for production
        {
            var builder = new BackgroundTaskBuilder();
            var requestStatus = await Windows.ApplicationModel.Background.BackgroundExecutionManager.RequestAccessAsync();
            if (requestStatus != BackgroundAccessStatus.AlwaysAllowed)
            {
                SendToast("not always allowd", "notallowed");

                // Depending on the value of requestStatus, provide an appropriate response
                // such as notifying the user which functionality won't work as expected
            }
            //builder.Name = "My Background Trigger";
            //builder.SetTrigger(new TimeTrigger(2, true));
            //// Do not set builder.TaskEntryPoint for in-process background tasks
            //// Here we register the task and work will start based on the time trigger.
            //BackgroundTaskRegistration task = builder.Register();

        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

        }
    }
}
