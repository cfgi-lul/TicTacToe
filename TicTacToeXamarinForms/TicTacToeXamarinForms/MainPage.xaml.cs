using System;
using System.Collections.Generic;
using TicTacToeXamarinForms.Views.GameComponent;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace TicTacToeXamarinForms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            GameView.Children.Add(new GameView(3));

            var classes = new List<string>() {"main-container" };
            
            var themeFromPreferences = Preferences.Get("theme", "default");
            if (themeFromPreferences == "dark")
            {
                ThemeToggle.IsToggled = true;
                classes.Add("_dark");
            }

            MainContainer.StyleClass = classes;
        }
        
        private void SliderValueChangeHandler(object sender, ValueChangedEventArgs e)
        {
            ((GameView) GameView.Children[0]).Destroy();
            GameView.Children.Clear();
            GameView.Children.Add(new GameView((int)e.NewValue + 3));
        }

        private void ThemeToggleHandler(object sender, ToggledEventArgs toggleEvent)
        {
            if (toggleEvent.Value)
            {
                MainContainer.StyleClass = new List<string>{"main-container", "_dark"};
                Preferences.Set("theme", "dark");
            }
            else
            {
                MainContainer.StyleClass = new List<string>{"main-container"};
                Preferences.Set("theme", "default");
            }
        }
    }
}