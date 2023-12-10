using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace TicTacToeXamarinForms.Models
{
    public class Game
    {
        private BoardCellValue _turn = BoardCellValue.O;
        private readonly Board _board;

        public BehaviorSubject<Tuple<int, int>> GameStat { get; } =
            new BehaviorSubject<Tuple<int, int>>(new Tuple<int, int>(0, 0));

        public string GameText { get; private set; } = "";
        public BehaviorSubject<bool> GameRunning { get; } = new BehaviorSubject<bool>(false);
        public Subject<bool> GameStateChanged { get; } = new Subject<bool>();

        public Game(int boardResolution)
        {
            _board = new Board(boardResolution);
        }

        public List<List<BoardCellValue>> GetBoard()
        {
            return _board.GetBoard();
        }

        private void StartGame()
        {
            GameText = "Turn is " + _turn;
            GameRunning.OnNext(true);
            GameStateChanged.OnNext(true);
        }

        public int GetBoardResolution()
        {
            return _board.BoardResolution;
        }

        private void StopGame()
        {
            GameRunning.OnNext(false);
        }

        public void ResetBoard()
        {
            StopGame();
            _board.ClearBoard();
            this._turn = BoardCellValue.O;
            GameStateChanged.OnNext(true);
            StartGame();
        }

        private bool IsGameShouldBeStopped()
        {
            return _board.CheckWinner() != BoardCellValue.Empty ||
                   _board.GetAllowedMovesList().Count == 0;
        }

        public void MakeMove(int x, int y)
        {
            if (_board.IsMoveAllowed(x, y))
            {
                _board.SetValueToPosition(_turn, x, y);
                _turn = _turn == BoardCellValue.O ? BoardCellValue.X : BoardCellValue.O;
                GameText = "Turn is " + _turn;
                GameStateChanged.OnNext(true);
            }

            if (IsGameShouldBeStopped())
            {
                StopGame();
                if (_board.CheckWinner() != BoardCellValue.Empty)
                {
                    var winner = (_turn == BoardCellValue.O ? BoardCellValue.X : BoardCellValue.O);
                    GameText = "Winner is " + winner;
                    EmitNewStat(winner);
                }
                else
                {
                    GameText = "It's a toe";
                }

                GameStateChanged.OnNext(true);
            }
        }

        private void EmitNewStat(BoardCellValue winner)
        {
            var prevStat = GameStat.Value;
            GameStat.OnNext(winner == BoardCellValue.O
                ? new Tuple<int, int>(prevStat.Item1, prevStat.Item2 + 1)
                : new Tuple<int, int>(prevStat.Item1 + 1, prevStat.Item2));
        }
    }
}