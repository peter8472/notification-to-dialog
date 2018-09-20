using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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

namespace notification_to_dialog
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // system.timers.timer is not available here.  What is?
            DispatcherTimer t = new DispatcherTimer();
            t.Interval = new System.TimeSpan(0, 0, 0, 30);
            t.Start();
            sendToast();
        }
        private void sendToast()
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
                            Text = "content"
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
            toast.Tag = "100";
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
