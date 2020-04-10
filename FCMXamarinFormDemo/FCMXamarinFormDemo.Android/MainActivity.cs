using Android.App;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Tasks;
using Android.OS;
using Firebase.Iid;
using Java.Lang;
using Prism;
using Prism.Ioc;

namespace FCMXamarinFormDemo.Droid
{
    [Activity(Label = "FCMXamarinFormDemo", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity , IOnSuccessListener
    {
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly string CHANNEL_NAME = "my_notification_channel_name";
        internal static readonly int NOTIFICATION_ID = 100;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));

            IsPlayServicesAvailable();

            CreateNotificationChannel();
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    //the google play service is not available and result code is User readable
                    // here we can log or show the error to user
                    return false;
                }
                else
                {
                    //google play service is not available in this device
                    return false;
                }
                
            }

            //Add success listner to the your firebase instance, can also add IOnCompleteListener,IOnFailureListener provided by FCm according to Requirement
            FirebaseInstanceId.Instance.GetInstanceId().AddOnSuccessListener(this);
           //google play service is available
            return true;
           
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID, CHANNEL_NAME, NotificationImportance.Default)
            {
                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public void OnSuccess(Object result)
        {
            // get the result from the completed task.Result as In xamarin.Firebase.messaging plugin there is no direct method present to get token
            // When new token is generated OnNewToken method is called in MyFirebaseService
            var token = result.Class.GetMethod("getToken").Invoke(result).ToString();
            System.Console.WriteLine(token);
            // can send token to backend server or store in secure storage
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

