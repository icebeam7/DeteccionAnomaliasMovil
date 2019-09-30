using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DeteccionAnomaliasMovil.ViewModels;

namespace DeteccionAnomaliasMovil.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnomalyDetectorView : ContentPage
    {
        AnomalyDetectorViewModel vm;

        public AnomalyDetectorView()
        {
            InitializeComponent();

            vm = new AnomalyDetectorViewModel();
            BindingContext = vm;
        }
    }
}