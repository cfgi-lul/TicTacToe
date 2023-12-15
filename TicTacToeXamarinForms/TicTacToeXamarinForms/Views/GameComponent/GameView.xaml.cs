using System;
using System.Collections.Generic;
using System.Linq;
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
        private string _startGameButtonText = "start";
        private IDisposable _gameWinningCombination;

        public GameView(int gameDifficulty)
        {
            InitializeComponent();
            _game = new Game(gameDifficulty);
            PrepareView();
        }


        private void Reset(object sender, EventArgs eventArgs)
        {
            _game.StopGame();
            _btns.ForEach(btnRow => btnRow.ForEach(RemoveHighlight));
            _game.StartGame();
        }

        private void RenderBoard()
        {
            var boardSnapshot = _game.GetBoard();
            for (var i = 0; i < _game.GetBoardResolution(); i++)
            {
                for (var j = 0; j < _game.GetBoardResolution(); j++)
                {
                    if (boardSnapshot[i][j] == null)
                    {
                        _btns[i][j].Text = "";
                    }
                    else
                    {
                        _btns[i][j].Text = boardSnapshot[i][j].ToString();
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
                    StyleClass = new List<string> {"buttons-wrapper"},
                    Orientation = StackOrientation.Horizontal
                };
                var curBtnsRow = new List<Button>();
                for (var j = 0; j < _game.GetBoardResolution(); j++)
                {
                    var btnToAdd = new Button();
                    var i1 = i;
                    var j1 = j;
                    curBtnsRow.Add(btnToAdd);
                    btnToAdd.Clicked += (o, e) => _game.MakeMove(new Tuple<int, int>(i1, j1));
                    btnToAdd.StyleClass = new List<string> {"button"};

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

            _gameStatSubscription = _game.GameStat
                .Subscribe(x => GameStat.Text =
                    String.Join(" | ", x.Aggregate(new List<String>(), (acc, cur) =>
                    {
                        acc.Add($"{cur.Item1}: {cur.Item2}");
                        return acc;
                    })));
            _gameStateChangedSubscription = _game.GameStateChanged.Subscribe((x) => RenderBoard());

            _gameWinningCombination = _game.WinningCombination
                .Subscribe(winCoords =>
                {
                    winCoords.ForEach(coord => { AddHighlight(_btns[coord.Item1][coord.Item2]); });
                });
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
            _gameWinningCombination.Dispose();
        }

        private void AddHighlight(View view)
        {
            view.StyleClass.Add("_highlight");
            view.StyleClass = new List<string>(view.StyleClass);
        }

        private void RemoveHighlight(View view)
        {
            if (view.StyleClass.Remove("_highlight"))
                view.StyleClass = new List<string>(view.StyleClass);
        }
    }
}