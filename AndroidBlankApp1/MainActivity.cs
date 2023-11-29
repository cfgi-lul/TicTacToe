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
        private TextView _gameStat;
        private SeekBar _seekBarView;
        private List<List<Button>> _btns = new List<List<Button>>();
        private LinearLayout _tableLayoutView;
        private IDisposable _gameRunningSubscription;
        private IDisposable _gameStatSubscription;
        private IDisposable _gameStateChangedSubscription;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            _game = new Game(3);
            
            _seekBarView = FindViewById<SeekBar>(Resource.Id.seekBar3);
            _seekBarView!.ProgressChanged += (sender,e) => {
                if (e.FromUser)
                {
                    ResetGameWithView(e.Progress);
                }
            };

            PrepareView();
        }

        [Export("Reset")]
        public void Reset(View view)
        {
            _game.ResetBoard();
        }

        private void RenderBoard()
        {
            var boardSnapshot = _game.GetBoard();
            for (int i = 0; i < _game.GetBoardResolution(); i++)
            {
                for (int j = 0; j < _game.GetBoardResolution(); j++)
                {
                    switch (boardSnapshot[i][j])
                    {
                        case BoardCellValue.O:
                            _btns[i][j].Text = "O";
                            break;
                        case BoardCellValue.X:
                            _btns[i][j].Text = "X";
                            break;
                        default:
                            _btns[i][j].Text = "";
                            break;
                    }
                }
            }

            _gameTextView.Text = _game.GameText;
        }

        private void CreateFieldView()
        {
            for (var i = 0; i < _game.GetBoardResolution(); i++)
            {
                var rowToAdd = new LinearLayout(this);
                rowToAdd.Orientation = Orientation.Horizontal;
                rowToAdd.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.WrapContent);
                rowToAdd.SetPadding(0, 24, 0, 0);
                rowToAdd.SetGravity(GravityFlags.Center);
                var curBtnsRow = new List<Button>();
                for (var j = 0; j < _game.GetBoardResolution(); j++)
                {
                    var btnToAdd = new Button(this);
                    var i1 = i;
                    var j1 = j;
                    curBtnsRow.Add(btnToAdd);
                    btnToAdd.Click += (o, e) => _game.MakeMove(i1, j1);
                    btnToAdd.Gravity = GravityFlags.Center;
                    btnToAdd.TextSize = 34;
                    LinearLayout.LayoutParams layoutParams;

                    switch (_game.GetBoardResolution())
                    {
                        case 3:
                        {
                            layoutParams = new LinearLayout.LayoutParams(300, 300);
                            break;
                        }
                        case 4:
                        {
                            layoutParams = new LinearLayout.LayoutParams(250, 250);
                            break;
                        }
                        default:
                        {
                            layoutParams = new LinearLayout.LayoutParams(200, 200);
                            break;
                        }
                    }

                    btnToAdd.LayoutParameters = layoutParams;
                    rowToAdd.AddView(btnToAdd);
                }

                _btns.Add(curBtnsRow);
                _tableLayoutView.AddView(rowToAdd);
            }
        }

        private void ResetGameWithView(int difficulty)
        {
            ClearView();
            _game = new Game(difficulty + 3);
            PrepareView();
        }

        private void EnableGameField()
        {
            _btns.ForEach(list => list.ForEach(btn => btn.Enabled = true));
        }

        private void DisableGameField()
        {
            _btns.ForEach(list => list.ForEach(btn => btn.Enabled = false));
        }
        
        private void PrepareView()
        {
            _tableLayoutView = FindViewById<LinearLayout>(Resource.Id.boardLayout);
            _gameTextView = FindViewById<TextView>(Resource.Id.textView3);
            _gameStat = FindViewById<TextView>(Resource.Id.textView2);

            CreateFieldView();

            _gameRunningSubscription = _game.GameRunning.Subscribe(x =>
            {
                if (x)
                {
                    EnableGameField();
                }
                else
                {
                    DisableGameField();
                }
            });

            _gameStatSubscription = _game.GameStat.Subscribe(x => _gameStat.Text = $"X : {x.Item1} | {x.Item2} : O");
            _gameStateChangedSubscription = _game.GameStateChanged.Subscribe((x) => RenderBoard());
        }

        private void ClearView()
        {
            _tableLayoutView.RemoveAllViews();
            _tableLayoutView = null;
            _gameTextView = null;
            _gameStat = null;
            _btns = new List<List<Button>>();
            _gameRunningSubscription.Dispose();
            _gameStatSubscription.Dispose();
            _gameStateChangedSubscription.Dispose();
        }
    }
}