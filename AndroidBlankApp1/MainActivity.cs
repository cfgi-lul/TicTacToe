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
        private readonly List<Button> _btns = new List<Button>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Buttons list filling
            this._btns.Add(FindViewById<Button>(Resource.Id.button1));
            this._btns.Add(FindViewById<Button>(Resource.Id.button2));
            this._btns.Add(FindViewById<Button>(Resource.Id.button3));
            this._btns.Add(FindViewById<Button>(Resource.Id.button4));
            this._btns.Add(FindViewById<Button>(Resource.Id.button5));
            this._btns.Add(FindViewById<Button>(Resource.Id.button6));
            this._btns.Add(FindViewById<Button>(Resource.Id.button7));
            this._btns.Add(FindViewById<Button>(Resource.Id.button8));
            this._btns.Add(FindViewById<Button>(Resource.Id.button9));

            this._gameTextView = FindViewById<TextView>(Resource.Id.textView3);

            this._game = new Game();
            this._game.GameRunning.Subscribe(x =>
            {
                if (x)
                {
                    this.EnableGameField();
                }
                else
                {
                    this.DisableGameField();
                }
            });
        }

        [Export("BoardClickBtnHandler")]
        public void BoardClickBtnHandler(View view)
        {
            var buttonTag = (view as Button)?.Tag;
            var currentBtn = this._btns.Find(x => x?.Tag == buttonTag);
            this._game.MakeMove(Int32.Parse(buttonTag!.ToString()));
            this.RenderBoard();
        }

        [Export("Reset")]
        public void Reset(View view)
        {
            this._game.ResetBoard();
            this.RenderBoard();
        }

        private void RenderBoard()
        {
            var boardSnapshot = this._game.GetBoard();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    this._btns[i * 3 + j].Text = boardSnapshot[j, i] == BoardCellValue.Empty
                        ? ""
                        : boardSnapshot[j, i].ToString();
                }
            }

            this._gameTextView.Text = this._game.GameText;
        }

        private void DisableGameField()
        {
            this._btns.ForEach(x => x.Enabled = false);
        }

        private void EnableGameField()
        {
            this._btns.ForEach(x => x.Enabled = true);
        }
    }
}