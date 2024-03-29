using System;
using Xamarin.Forms;

namespace TicTacToeXamarinForms.Views.DiscreteSlider
{
    public class DiscreteSlider : Slider
    {
        public int StepValue { get; set; } = 1;

        public DiscreteSlider()
        {
            ValueChanged += OnDiscreteSliderValueChanged;
        }

        private void OnDiscreteSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
                Value = Math.Max(Minimum, Math.Min(Maximum, Minimum + StepValue * (int) Math.Round((e.NewValue - Minimum) / StepValue)));
        }
    }
}