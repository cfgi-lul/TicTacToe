using System.Collections.Generic;
using Android.Widget;

namespace AndroidBlankApp1.TicTacToe
{
    public class Game
    {
        private BoardCellValue _turn = BoardCellValue.O;
        private readonly Board _board = new Board();
        private readonly List<Button> _gameFieldBtns;
        private readonly TextView _gameTextView;

        public Game(List<Button> gameField, TextView gameTextView)
        {
            this._gameFieldBtns = gameField;
            this._gameTextView = gameTextView;
        }

        private void StartGame()
        {
            this.AllowGameField();
            this._gameTextView.Text = "Turn is " + this._turn;
        }

        private void StopGame()
        {
            this.DisableGameField();
        }

        public void ResetBoard()
        {
            this.StopGame();
            this._board.ClearBoard();
            this.RenderBoard();
            this.StartGame();
        }

        public void RenderBoard()
        {
            var boardSnapshot = this._board.GetBoard();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    this._gameFieldBtns[i * 3 + j].Text = boardSnapshot[j, i] == BoardCellValue.Empty
                        ? ""
                        : boardSnapshot[j, i].ToString();
                }
            }
        }

        private bool IsGameShouldBeStopped()
        {
            return this._board.CheckWinner() != BoardCellValue.Empty ||
                   this._board.GetAllowedMovesList().Count == 0;
        }

        public void MakeMove(int position)
        {
            if (this._board.IsMoveAllowed(position % 3, position / 3))
            {
                this._board.SetValueToPosition(_turn, position % 3, position / 3);
                this._turn = this._turn == BoardCellValue.O ? BoardCellValue.X : BoardCellValue.O;
                this._gameTextView.Text = "Turn is " + this._turn;
            }

            if (this.IsGameShouldBeStopped())
            {
                this.StopGame();
                if (this._board.CheckWinner() != BoardCellValue.Empty)
                {
                    this._gameTextView.Text = "Winner is " +
                                              (this._turn == BoardCellValue.O ? BoardCellValue.X : BoardCellValue.O);
                }
                else
                {
                    this._gameTextView.Text = "It's a toe";
                }
            }
            this.RenderBoard();
        }
        
        private void DisableGameField()
        {
            this._gameFieldBtns.ForEach(x => x.Enabled = false);
        }

        private void AllowGameField()
        {
            this._gameFieldBtns.ForEach(x => x.Enabled = true);
        }
    }
}