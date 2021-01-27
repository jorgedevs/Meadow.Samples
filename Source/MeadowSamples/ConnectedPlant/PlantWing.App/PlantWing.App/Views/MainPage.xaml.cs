using PlantWing.App.ViewModels;
using Xamarin.Forms;

namespace PlantWing.App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}
