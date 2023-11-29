using System;
using System.Collections.Generic;
using System.Linq;

namespace AndroidBlankApp1.TicTacToe
{
    public class Board
    {
        public int BoardResolution { get; }
        private List<List<BoardCellValue>> _board;
        private int _numInRowForWin;

        public Board(int boardResolution)
        {
            BoardResolution = boardResolution;
            _board = new List<List<BoardCellValue>>(BoardResolution);
            _numInRowForWin = boardResolution == 3 ? 3 : 4;
            FillBoard();
        }

        private void FillBoard()
        {
            for (int i = 0; i < BoardResolution; i++)
            {
                _board.Add(new List<BoardCellValue>(BoardResolution));
                for (int j = 0; j < BoardResolution; j++)
                {
                    _board[i].Add(BoardCellValue.Empty);
                }
            }
        }

        public List<List<BoardCellValue>> GetBoard()
        {
            return _board;
        }

        public void ClearBoard()
        {
            for (int i = 0; i < BoardResolution; i++)
            {
                for (int j = 0; j < BoardResolution; j++)
                {
                    _board[i][j] = BoardCellValue.Empty;
                }
            }
        }

        public void SetValueToPosition(BoardCellValue val, int x, int y)
        {
            if (_board[x][y] == BoardCellValue.Empty)
                _board[x][y] = val;
        }

        private bool CheckListElementsEquality<T>(List<T> list)
        {
            return list.All(o => o.Equals(list[0]));
        }

        public BoardCellValue CheckWinner()
        {
            for (var i = 0; i < BoardResolution; i++)
            {
                for (var j = 0; j <= BoardResolution - _numInRowForWin; j++)
                {
                    if (CheckRow(i, j))
                    {
                        return _board[i][j];
                    }

                    if (CheckCol(j, i))
                    {
                        return _board[j][i];
                    }
                }
            }

            for (var i = 0; i <= BoardResolution - _numInRowForWin; i++)
            {
                for (var j = 0; j <= BoardResolution - _numInRowForWin; j++)
                {
                    if (CheckMainDiag(i, j))
                    {
                        return _board[i][j];
                    }
                }
            }

            for (var i = _numInRowForWin - 1; i <= BoardResolution - 1; i++)
            {
                for (var j = 0; j <= BoardResolution - _numInRowForWin; j++)
                {
                    if (CheckSecDiag(i, j))
                    {
                        return _board[i][j];
                    }
                }
            }

            return BoardCellValue.Empty;
        }

        private bool CheckRow(int startX, int startY)
        {
            return _board[startX][startY] != BoardCellValue.Empty &&
                   CheckListElementsEquality(_board[startX].GetRange(startY, _numInRowForWin));
        }

        private bool CheckCol(int startX, int startY)
        {
            for (int i = startX; i < startX + _numInRowForWin; i++)
            {
                if (_board[startX][startY] == BoardCellValue.Empty || _board[startX][startY] != _board[i][startY])
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckMainDiag(int startX, int startY)
        {
            for (int i = 0; i < _numInRowForWin; i++)
            {
                if (_board[startX][startY] == BoardCellValue.Empty ||
                    _board[startX][startY] != _board[startX + i][startY + i])
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckSecDiag(int startX, int startY)
        {
            for (int i = 0; i < _numInRowForWin; i++)
            {
                if (_board[startX][startY] == BoardCellValue.Empty ||
                    _board[startX][startY] != _board[startX - i][startY + i])
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsMoveAllowed(int x, int y)
        {
            return _board[x][y] == BoardCellValue.Empty;
        }

        public List<Tuple<int, int>> GetAllowedMovesList()
        {
            var allowedMovesList = new List<Tuple<int, int>>();

            for (int i = 0; i < BoardResolution; i++)
            {
                for (int j = 0; j < BoardResolution; j++)
                {
                    if (_board[i][j] == BoardCellValue.Empty)
                        allowedMovesList.Add(Tuple.Create(i, j));
                }
            }

            return allowedMovesList;
        }
    }
}