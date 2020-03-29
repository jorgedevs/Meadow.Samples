﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;

namespace ServoRemote.Droid
{
    [Activity(Label = "ServoRemote", Icon = "@mipmap/icon_launcher", RoundIcon = "@mipmap/icon_round_launcher", Theme = "@style/Theme.Splash", 
        MainLauncher = true, NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (!IsTaskRoot
                && Intent.HasCategory(Intent.CategoryLauncher)
                && !string.IsNullOrEmpty(Intent.Action)
                && Intent.Action == Intent.ActionMain)
            {
                Finish();
                return;
            }

            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
    }
}