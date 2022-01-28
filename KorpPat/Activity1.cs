using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using System;
using Android.Runtime;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using AndroidX;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using AndroidX.ConstraintLayout.Widget;
using Xamarin.Forms;
using Android.Widget;
using Android.Util;
using Android.Content;
using Xamarin.Essentials;

namespace KorpPat
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/Icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Landscape,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Activity1 : AndroidGameActivity
    {
        private TRexRunnerGame _game;
        private Android.Views.View _view;
        public int HighScore { get; set; }
        public DateTime HighScoreDate { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            SetContentView(Resource.Layout.activity_main);

            if (bundle != null)
                HighScore = bundle.GetInt("HighScore");
            if (Preferences.ContainsKey("HighScore"))
                HighScore = Preferences.Get("HighScore", 0);

            DisplayMetrics displayMetrics = Resources.DisplayMetrics;
            int width = displayMetrics.WidthPixels;
            int height = displayMetrics.HeightPixels;
            Console.WriteLine(width);
            Console.WriteLine(width/TRexRunnerGame.GAME_WINDOW_WIDTH);
            Console.WriteLine(height);

            
            //TRexRunnerGame.GAME_WINDOW_HEIGHT;
            _game = new TRexRunnerGame(this, (double) width/ (double) TRexRunnerGame.GAME_WINDOW_WIDTH, (double)width / (double)TRexRunnerGame.GAME_WINDOW_WIDTH);
            _view = _game.Services.GetService(typeof(Android.Views.View)) as Android.Views.View;

            Android.Widget.RelativeLayout relativeLayout = (Android.Widget.RelativeLayout) FindViewById(Resource.Id.rootlayout);
            relativeLayout.AddView(_view);
       
            _game.Run();




        }

        public override void OnSaveInstanceState(Bundle outState, PersistableBundle outPersistentState)
        {
            base.OnSaveInstanceState(outState, outPersistentState);

            outState.PutInt("HighScore", HighScore);
            
        }

        protected override void OnPause()
        {
            base.OnPause();
            Preferences.Set("HighScore", HighScore);
        }

    }
    
}
