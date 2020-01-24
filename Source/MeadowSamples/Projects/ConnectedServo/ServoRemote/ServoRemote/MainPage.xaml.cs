using ServoRemote.ViewModel;
using Xamarin.Forms;

namespace ServoRemote
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