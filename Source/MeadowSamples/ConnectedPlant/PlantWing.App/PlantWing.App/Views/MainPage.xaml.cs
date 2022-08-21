using MobilePlantSample.ViewModels;
using Xamarin.Forms;

namespace MobilePlantSample
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
