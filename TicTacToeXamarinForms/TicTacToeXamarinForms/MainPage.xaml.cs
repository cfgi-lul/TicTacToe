using TicTacToeXamarinForms.Views.GameComponent;
using Xamarin.Forms;

namespace TicTacToeXamarinForms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            GameView.Children.Add(new GameView(3));
        }
        
        private void SliderValueChangeHandler(object sender, ValueChangedEventArgs e)
        {
            ((GameView) GameView.Children[0]).Destroy();
            GameView.Children.Clear();
            GameView.Children.Add(new GameView((int)e.NewValue + 3));
        }
    }
}