using System;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Gms.Gcm;
using Android.Gms.Gcm.Iid;

namespace RemoteNotifyP1
{
     [Service(Exported = false)]
    class RegistrationIntentService : IntentService
    {
        static object locker = new object();
        static string YOUR_SENDER_ID = "679864379096";
        static string TOPIC = @"/topics/global";
        public RegistrationIntentService() : base("RegistrationIntentService") { }

        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                Log.Info("RegistrationIntentService", "Calling InstanceID.GetToken");
                lock (locker)
                {
                    var instanceID = InstanceID.GetInstance(this);
                    var token = instanceID.GetToken(
                        YOUR_SENDER_ID, GoogleCloudMessaging.InstanceIdScope, null);

                    Log.Info("RegistrationIntentService", "GCM Registration Token: " + token);
                    SendRegistrationToAppServer(token);
                    Subscribe(token);
                }
            }
            catch (Exception e)
            {
                Log.Debug("RegistrationIntentService", "Failed to get a registration token");
                return;
            }
        }

        void SendRegistrationToAppServer(string token)
        {
            // Add custom implementation here as needed.
        }

        void Subscribe(string token)
        {
            var pubSub = GcmPubSub.GetInstance(this);
            pubSub.Subscribe(token, TOPIC, null);
        }
    }
    
}