using System.Collections.Generic;
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
            MainContainer.StyleClass = new List<string>{"main-container"};
        }
        
        private void SliderValueChangeHandler(object sender, ValueChangedEventArgs e)
        {
            ((GameView) GameView.Children[0]).Destroy();
            GameView.Children.Clear();
            GameView.Children.Add(new GameView((int)e.NewValue + 3));
        }

        private void ThemeToggleHandler(object sender, ToggledEventArgs e)
        {
            MainContainer.StyleClass = e.Value ? new List<string>{"main-container", "_dark"} : new List<string>{"main-container"};
        }
    }
}