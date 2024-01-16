using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;

namespace TicTacToeXamarinForms.Models
{
    public class Game
    {
        private readonly Board _board;
        private List<Player> _players;
        private int _curPlayerIndex;
        public BehaviorSubject<List<Tuple<string, int>>> GameStat { get; }
        public string GameText { get; private set; } = "";
        public BehaviorSubject<bool> GameRunning { get; } = new BehaviorSubject<bool>(false);

        public Subject<List<Tuple<int, int>>> WinningCombination { get; } =
            new Subject<List<Tuple<int, int>>>();

        public Subject<bool> GameStateChanged { get; } = new Subject<bool>();

        public Game(int boardResolution)
        {
            _board = new Board(boardResolution);
            _players = new List<Player>();
            AddNewPlayer(new Player('X', false));
            AddNewPlayer(new Player('O'));
            // AddNewPlayer(new Player('Y'));
            // AddNewPlayer(new Player('Z'));
            // AddNewPlayer(new Player('Q'));
            // AddNewPlayer(new Player('W'));
            GameStat = new BehaviorSubject<List<Tuple<string, int>>>(GetNewStat());
            _curPlayerIndex = 0;
        }

        public void AddNewPlayer(Player player)
        {
            _players.Add(player);
        }

        public List<List<char?>> GetBoard()
        {
            return _board.GetBoard();
        }

        public void StartGame()
        {
            _board.ClearBoard();
            GameText = "Turn is " + _players[_curPlayerIndex].Symbol;
            GameRunning.OnNext(true);
            GameStateChanged.OnNext(true);
            if (_players[_curPlayerIndex].IsComputerMode)
                MakeMove(GetComputerMove());
        }

        public int GetBoardResolution()
        {
            return _board.BoardResolution;
        }

        public void StopGame()
        {
            GameRunning.OnNext(false);
        }

        private bool IsGameShouldBeStopped()
        {
            return _board.CheckWinner() != null ||
                   _board.GetAllowedMovesList().Count == 0;
        }

        public void MakeMove(Tuple<int, int> coords)
        {
            if (!_board.IsMoveAllowed(coords.Item1, coords.Item2))
                return;
                
            _board.SetValueToPosition(_players[_curPlayerIndex].Symbol, coords.Item1, coords.Item2);

            if (IsGameShouldBeStopped())
            {
                StopGame();
                if (_board.CheckWinner() != null)
                {
                    var winner = _players[_curPlayerIndex].Symbol;
                    _players[_curPlayerIndex].WinCount++;
                    GameText = "Winner is " + winner;
                    WinningCombination.OnNext(_board.WinCombination);
                    GameStat.OnNext(GetNewStat());
                }
                else
                    GameText = "It's a toe";
            }
            else
                SetNextPlayer();

            GameStateChanged.OnNext(true);
        }

        private void SetNextPlayer()
        {
            _curPlayerIndex = (_curPlayerIndex + 1) % _players.Count;
            GameText = "Turn is " + _players[_curPlayerIndex].Symbol;
            if (_players[_curPlayerIndex].IsComputerMode)
                MakeMove(GetComputerMove());
        }

        private List<Tuple<string, int>> GetNewStat()
        {
            return _players.Aggregate(new List<Tuple<string, int>>(), (acc, cur) =>
            {
                acc.Add(new Tuple<string, int>(cur.Symbol.ToString(), cur.WinCount));
                return acc;
            });
        }

        private Tuple<int, int> GetComputerMove()
        {
            var allowedMovesList = _board.GetAllowedMovesList();
            var r = new Random();
            return allowedMovesList[r.Next(0, allowedMovesList.Count)];
        }
    }
}