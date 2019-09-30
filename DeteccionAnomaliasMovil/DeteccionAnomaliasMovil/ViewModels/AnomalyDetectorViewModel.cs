using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using DeteccionAnomaliasMovil.Models;
using DeteccionAnomaliasMovil.Services;

using SkiaSharp;
using Microcharts;
using Xamarin.Forms;

namespace DeteccionAnomaliasMovil.ViewModels
{
    public class AnomalyDetectorViewModel : BaseViewModel
    {
        public Command AnalyzePriceDataCommand { get; set; }

        private int sensitivity;

        public int Sensitivity
        {
            get { return sensitivity; }
            set { sensitivity = value; OnPropertyChanged(); }
        }

        private PriceInfo priceInfo;

        public PriceInfo PriceInfo
        {
            get { return priceInfo; }
            set { priceInfo = value; OnPropertyChanged(); }
        }

        private PriceResult priceResult;

        public PriceResult PriceResult
        {
            get { return priceResult; }
            set { priceResult = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Series> priceDataSeries;

        public ObservableCollection<Series> PriceDataSeries
        {
            get { return priceDataSeries; }
            set { priceDataSeries = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Anomaly> priceAnomalies;

        public ObservableCollection<Anomaly> PriceAnomalies
        {
            get { return priceAnomalies; }
            set { priceAnomalies = value; OnPropertyChanged(); }
        }

        private Chart priceChart;

        public Chart PriceChart
        {
            get { return priceChart; }
            set { priceChart = value; OnPropertyChanged(); }
        }

        public AnomalyDetectorViewModel()
        {
            GetPriceData();
            CreateChart(anomalies: false);
            AnalyzePriceDataCommand = new Command(async () => await AnalyzeData());
        }

        private void GetPriceData()
        {
            var priceSeries = PriceDataService.GetPriceDataSeries();
            PriceDataSeries = new ObservableCollection<Series>(priceSeries);

            Sensitivity = 95;

            PriceInfo = new PriceInfo()
            {
                granularity = "daily",
                maxAnomalyRatio = 0.25,
                sensitivity = Sensitivity,
                series = priceSeries
            };
        }

        private void CreateChart(bool anomalies)
        {
            PriceChart = new LineChart()
            {
                LineMode = LineMode.Spline,
                LabelTextSize = 0
            };

            PriceChart.Entries = PriceInfo.series.Select(
                (v, index) => new Microcharts.Entry(v.value)
                {
                    Color = !anomalies 
                                ? SKColors.Green
                                : !PriceAnomalies.Any(x => x.Timestamp.ToShortDateString() == v.timestamp.ToShortDateString())
                                    ? SKColors.Green 
                                    : SKColors.Red,
                    Label = v.timestamp.ToShortDateString()
                });
        }

        private async Task AnalyzeData()
        {
            PriceInfo.sensitivity = Sensitivity;
            PriceAnomalies = new ObservableCollection<Anomaly>();
            PriceResult = await AnomalyDetectorService.AnalyzeData(priceInfo);

            if (PriceResult != null)
            {
                for (int i = 0; i < PriceResult.IsAnomaly.Length; i++)
                {
                    if (PriceResult.IsAnomaly[i])
                    {
                        var priceData = PriceInfo.series[i];

                        PriceAnomalies.Add(new Anomaly()
                        {
                            Value = priceData.value,
                            Timestamp = priceData.timestamp,
                            IsPositive = PriceResult.IsPositiveAnomaly[i]
                        });
                    }
                }

                CreateChart(anomalies: true);
            }
        }
    }
}
