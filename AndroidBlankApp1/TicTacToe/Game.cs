using System.Reactive.Subjects;


namespace AndroidBlankApp1.TicTacToe
{
    public class Game
    {
        private BoardCellValue _turn = BoardCellValue.O;
        private readonly Board _board = new Board();
        public string GameText { get; private set; } = "";
        public BehaviorSubject<bool> GameRunning { get; } = new BehaviorSubject<bool>(false); 

        public BoardCellValue[,] GetBoard()
        {
            return this._board.GetBoard();
        }

        private void StartGame()
        {
            this.GameText = "Turn is " + this._turn;
            this.GameRunning.OnNext(true);
        }

        private void StopGame()
        {
            this.GameRunning.OnNext(false);
        }

        public void ResetBoard()
        {
            this.StopGame();
            this._board.ClearBoard();
            this.StartGame();
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
                this.GameText = "Turn is " + this._turn;
            }

            if (this.IsGameShouldBeStopped())
            {
                this.StopGame();
                if (this._board.CheckWinner() != BoardCellValue.Empty)
                {
                    this.GameText = "Winner is " +
                                         (this._turn == BoardCellValue.O ? BoardCellValue.X : BoardCellValue.O);
                }
                else
                {
                    this.GameText = "It's a toe";
                }
            }
        }
    }
}