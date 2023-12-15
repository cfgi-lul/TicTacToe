namespace TicTacToeXamarinForms.Models
{
    public class Player
    {
        public int WinCount;
        public readonly char Symbol;
        public readonly bool IsComputerMode;

        public Player(char symbol, bool isComputerMode = true)
        {
            Symbol = symbol;
            IsComputerMode = isComputerMode;
            WinCount = 0;
        }
    }
}