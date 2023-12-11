using System;
using System.Collections.Generic;
using TicTacToeXamarinForms.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicTacToeXamarinForms.Views.GameComponent
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameView
    {
        private readonly Game _game;
        private List<List<Button>> _btns = new List<List<Button>>();
        private IDisposable _gameRunningSubscription;
        private IDisposable _gameStatSubscription;
        private IDisposable _gameStateChangedSubscription;
        public GameView(int gameDifficulty)
        {
            InitializeComponent();
            _game = new Game(gameDifficulty);
            PrepareView();
        }

        
        private void Reset(object sender, EventArgs eventArgs)
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

            GameTextView.Text = _game.GameText;
        }

        private void CreateFieldView()
        {
            for (var i = 0; i < _game.GetBoardResolution(); i++)
            {
                var rowToAdd = new StackLayout
                {
                    StyleClass = new List<string>{"buttons-wrapper"},
                    Orientation = StackOrientation.Horizontal
                };
                var curBtnsRow = new List<Button>();
                for (var j = 0; j < _game.GetBoardResolution(); j++)
                {
                    var btnToAdd = new Button();
                    var i1 = i;
                    var j1 = j;
                    curBtnsRow.Add(btnToAdd);
                    btnToAdd.Clicked += (o, e) => _game.MakeMove(i1, j1);
                    btnToAdd.StyleClass = new List<string>{"button"};

                    switch (_game.GetBoardResolution())
                    {
                        case 3:
                        {
                            btnToAdd.StyleClass.Add("default-button");
                            break;
                        }
                        case 4:
                        {
                            btnToAdd.StyleClass.Add("s-button");
                            break;
                        }
                        default:
                        {
                            btnToAdd.StyleClass.Add("xs-button");
                            break;
                        }
                    }

                    rowToAdd.Children.Add(btnToAdd);
                }

                _btns.Add(curBtnsRow);
                BtnsWrapper.Children.Add(rowToAdd);
            }
        }

        private void EnableGameField()
        {
            _btns.ForEach(list => list.ForEach(btn => btn.IsEnabled = true));
        }

        private void DisableGameField()
        {
            _btns.ForEach(list => list.ForEach(btn => btn.IsEnabled = false));
        }

        private void PrepareView()
        {
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

            _gameStatSubscription = _game.GameStat.Subscribe(x => GameStat.Text = $"X : {x.Item1} | {x.Item2} : O");
            _gameStateChangedSubscription = _game.GameStateChanged.Subscribe((x) => RenderBoard());
        }

        public void Destroy()
        {
            BtnsWrapper.Children.Clear();
            BtnsWrapper = null;
            GameTextView = null;
            GameStat = null;
            _btns = new List<List<Button>>();
            _gameRunningSubscription.Dispose();
            _gameStatSubscription.Dispose();
            _gameStateChangedSubscription.Dispose();
        }
    }
}