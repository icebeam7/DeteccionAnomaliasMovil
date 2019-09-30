using Xamarin.Forms;

namespace DeteccionAnomaliasMovil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Views.AnomalyDetectorView();
        }
    }
}
