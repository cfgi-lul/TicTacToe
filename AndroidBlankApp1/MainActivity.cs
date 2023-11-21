using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using AndroidBlankApp1.TicTacToe;
using Java.Interop;

namespace AndroidBlankApp1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Game _game;
        private TextView _gameTextView;
        private List<Button> btns = new List<Button>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Buttons list filling
            this.btns.Add(FindViewById<Button>(Resource.Id.button1));
            this.btns.Add(FindViewById<Button>(Resource.Id.button2));
            this.btns.Add(FindViewById<Button>(Resource.Id.button3));
            this.btns.Add(FindViewById<Button>(Resource.Id.button4));
            this.btns.Add(FindViewById<Button>(Resource.Id.button5));
            this.btns.Add(FindViewById<Button>(Resource.Id.button6));
            this.btns.Add(FindViewById<Button>(Resource.Id.button7));
            this.btns.Add(FindViewById<Button>(Resource.Id.button8));
            this.btns.Add(FindViewById<Button>(Resource.Id.button9));
            this._gameTextView = FindViewById<TextView>(Resource.Id.textView3);


            this._game = new Game(this.btns, this._gameTextView);
        }

        [Export("BoardClickBtnHandler")]
        public void BoardClickBtnHandler(View view)
        {
            var buttonTag = (view as Button)?.Tag;
            var currentBtn = this.btns.Find(x => x?.Tag == buttonTag);
            this._game.MakeMove(Int32.Parse(buttonTag!.ToString()));
        }

        [Export("Reset")]
        public void Reset(View view)
        {
            this._game.ResetBoard();
        }
    }
}