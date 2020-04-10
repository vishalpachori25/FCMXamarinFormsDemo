
using Android.App;
using Android.Content;
using Firebase.Iid;
using Firebase.Messaging;

namespace FCMXamarinFormDemo.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseService : FirebaseMessagingService
    {
        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);
            // can send token to backend server or store in secure storage
            
        }
      
    }
}