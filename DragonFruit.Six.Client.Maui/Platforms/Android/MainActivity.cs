﻿// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using Android.App;
using Android.Content;
using Android.Content.PM;
using Microsoft.Maui;

namespace DragonFruit.Six.Client.Maui
{
    [Activity(Name = "com.dragon.six.app", Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataSchemes = new[] { "http", "https" }, DataHost = "dragon6.dragonfruit.network", DataPathPrefix = "/stats/", AutoVerify = true)]
    public class MainActivity : MauiAppCompatActivity
    {
    }
}
