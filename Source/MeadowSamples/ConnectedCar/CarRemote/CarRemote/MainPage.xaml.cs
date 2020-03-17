using CarRemote.Model;
using CarRemote.ViewModel;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace CarRemote
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        MainViewModel vm;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = vm = new MainViewModel();
        }

        async void BtnUpPressed(object sender, EventArgs e)
        {
            await vm.SendCommandAsync(CommandConstants.MOVE_FORWARD);
        }

        async void BtnUpReleased(object sender, EventArgs e)
        {
            await vm.SendCommandAsync(CommandConstants.STOP);
        }

        async void BtnDownPressed(object sender, EventArgs e)
        {
            await vm.SendCommandAsync(CommandConstants.MOVE_BACKWARD);
        }

        async void BtnDownReleased(object sender, EventArgs e)
        {
            await vm.SendCommandAsync(CommandConstants.STOP);
        }

        async void BtnLeftPressed(object sender, EventArgs e)
        {
            await vm.SendCommandAsync(CommandConstants.TURN_LEFT);
        }

        async void BtnLeftReleased(object sender, EventArgs e)
        {
            await vm.SendCommandAsync(CommandConstants.STOP);
        }

        async void BtnRightPressed(object sender, EventArgs e)
        {
            await vm.SendCommandAsync(CommandConstants.TURN_RIGHT);
        }

        async void BtnRightReleased(object sender, EventArgs e)
        {
            await vm.SendCommandAsync(CommandConstants.STOP);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            btnUp.Pressed += BtnUpPressed;
            btnUp.Released += BtnUpReleased;
            btnDown.Pressed += BtnDownPressed;
            btnDown.Released += BtnDownReleased;
            btnLeft.Pressed += BtnLeftPressed;
            btnLeft.Released += BtnLeftReleased;
            btnRight.Pressed += BtnRightPressed;
            btnRight.Released += BtnRightReleased;
        }

        protected override void OnDisappearing()
        {
            btnUp.Pressed -= BtnUpPressed;
            btnUp.Released -= BtnUpReleased;
            btnDown.Pressed -= BtnDownPressed;
            btnDown.Released -= BtnDownReleased;
            btnLeft.Pressed -= BtnLeftPressed;
            btnLeft.Released -= BtnLeftReleased;
            btnRight.Pressed -= BtnRightPressed;
            btnRight.Released -= BtnRightReleased;

            base.OnDisappearing();
        }
    }
}