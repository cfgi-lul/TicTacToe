using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeXamarinForms.Models
{
    public class Board
    {
        public int BoardResolution { get; }
        public List<Tuple<int, int>> WinCombination { get; private set; }
        private readonly List<List<char?>> _board;
        private readonly int _numInRowForWin;

        public Board(int boardResolution)
        {
            BoardResolution = boardResolution;
            _board = new List<List<char?>>(BoardResolution);
            _numInRowForWin = boardResolution == 3 ? 3 : 4;
            FillBoard();
        }

        private void FillBoard()
        {
            for (var i = 0; i < BoardResolution; i++)
            {
                _board.Add(new List<char?>(BoardResolution));
                for (var j = 0; j < BoardResolution; j++)
                {
                    _board[i].Add(null);
                }
            }
        }

        public List<List<char?>> GetBoard()
        {
            return _board;
        }

        public void ClearBoard()
        {
            for (int i = 0; i < BoardResolution; i++)
            {
                for (int j = 0; j < BoardResolution; j++)
                {
                    _board[i][j] = null;
                }
            }
        }

        public void SetValueToPosition(char val, int x, int y)
        {
            if (_board[x][y] == null)
                _board[x][y] = val;
        }

        private bool CheckListElementsEquality<T>(IReadOnlyList<T> list)
        {
            return list.All(o => o.Equals(list[0]));
        }

        public char? CheckWinner()
        {
            for (var i = 0; i < BoardResolution; i++)
            {
                for (var j = 0; j <= BoardResolution - _numInRowForWin; j++)
                {
                    if (CheckRow(i, j))
                    {
                        SetWinRow(i, j);
                        return _board[i][j];
                    }

                    if (CheckCol(j, i))
                    {
                        SetWinCol(j, i);
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
                        SetWinMainDiag(i, j);
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
                        SetWinSecDiag(i, j);
                        return _board[i][j];
                    }
                }
            }

            return null;
        }

        private bool CheckRow(int startX, int startY)
        {
            return _board[startX][startY] != null &&
                   CheckListElementsEquality(_board[startX].GetRange(startY, _numInRowForWin));
        }

        private bool CheckCol(int startX, int startY)
        {
            for (var i = startX; i < startX + _numInRowForWin; i++)
            {
                if (_board[startX][startY] == null || _board[startX][startY] != _board[i][startY])
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckMainDiag(int startX, int startY)
        {
            for (var i = 0; i < _numInRowForWin; i++)
            {
                if (_board[startX][startY] == null ||
                    _board[startX][startY] != _board[startX + i][startY + i])
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckSecDiag(int startX, int startY)
        {
            for (var i = 0; i < _numInRowForWin; i++)
            {
                if (_board[startX][startY] == null ||
                    _board[startX][startY] != _board[startX - i][startY + i])
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsMoveAllowed(int x, int y)
        {
            return _board[x][y] == null;
        }

        public List<Tuple<int, int>> GetAllowedMovesList()
        {
            var allowedMovesList = new List<Tuple<int, int>>();

            for (var i = 0; i < BoardResolution; i++)
            {
                for (var j = 0; j < BoardResolution; j++)
                {
                    if (_board[i][j] == null)
                        allowedMovesList.Add(Tuple.Create(i, j));
                }
            }

            return allowedMovesList;
        }

        private void SetWinRow(int x, int y)
        {
            WinCombination = Enumerable.Range(0, _numInRowForWin).Aggregate(new List<Tuple<int, int>>(), (acc, cur) =>
            {
                acc.Add(new Tuple<int, int>(x, y + cur));
                return acc;
            });
        }

        private void SetWinCol(int x, int y)
        {
            WinCombination = Enumerable.Range(0, _numInRowForWin).Aggregate(new List<Tuple<int, int>>(), (acc, cur) =>
            {
                acc.Add(new Tuple<int, int>(x + cur, y));
                return acc;
            });
        }

        private void SetWinMainDiag(int x, int y)
        {
            WinCombination = Enumerable.Range(0, _numInRowForWin).Aggregate(new List<Tuple<int, int>>(), (acc, cur) =>
            {
                acc.Add(new Tuple<int, int>(x + cur, y + cur));
                return acc;
            });
        }

        private void SetWinSecDiag(int x, int y)
        {
            WinCombination = Enumerable.Range(0, _numInRowForWin).Aggregate(new List<Tuple<int, int>>(), (acc, cur) =>
            {
                acc.Add(new Tuple<int, int>(x - cur, y + cur));
                return acc;
            });
        }
    }
}