using System;
using System.Collections.Generic;

namespace AndroidBlankApp1.TicTacToe
{
    public class Board
    {
        private  BoardCellValue[,] _board = {{0, 0, 0}, {0, 0, 0}, {0, 0, 0}};

        public BoardCellValue[,] GetBoard()
        {
            return this._board;
        }

        public void ClearBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    this._board[i, j] = 0;
                }
            }
        }

        public void SetValueToPosition(BoardCellValue val, int x, int y)
        {
            if (this._board[x, y] == BoardCellValue.Empty)
                this._board[x, y] = val;
        }

        public BoardCellValue CheckWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                if (this._board[i, 0] == this._board[i, 1] && this._board[i, 2] == this._board[i, 1] &&
                    this._board[i, 1] != BoardCellValue.Empty)
                {
                    return this._board[i, 0];
                }

                if (this._board[0, i] == this._board[1, i] && this._board[2, i] == this._board[0, i] &&
                    this._board[0, i] != BoardCellValue.Empty)
                {
                    return this._board[0, i];
                }
            }

            // check diags
            if (this._board[0, 0] == this._board[1, 1] && this._board[2, 2] == this._board[1, 1] &&
                this._board[1, 1] != BoardCellValue.Empty)
            {
                return this._board[0, 0];
            }

            if (this._board[0, 2] == this._board[2, 2] && this._board[2, 0] == this._board[2, 2] &&
                this._board[2, 1] != BoardCellValue.Empty)
            {
                return this._board[2, 2];
            }

            return BoardCellValue.Empty;
        }

        public bool IsMoveAllowed(int x, int y)
        {
            return this._board[x, y] == BoardCellValue.Empty;
        }

        public List<Tuple<int, int>> GetAllowedMovesList()
        {
            var allowedMovesList = new List<Tuple<int, int>>();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (this._board[i, j] == BoardCellValue.Empty)
                        allowedMovesList.Add(Tuple.Create(i, j));
                }
            }

            return allowedMovesList;
        }
    }
}