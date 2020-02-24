using System;
using System.Threading;
using System.Threading.Tasks;
using LiveCharts.Geared;
using LiveCharts.Defaults;
namespace trhacka_v_1_0_working_2019_010_201
{
    public class ChartsWrite
    {


        public bool IsReading { get; set; }
        public GearedValues<ObservablePoint> Values { get; set; }
        public double Count { get; set; }
        public double CurrentLecture { get; set; }
        public bool IsHot { get; set; }
        static GearedValues<ObservablePoint> chartValue = new GearedValues<ObservablePoint>();
        public void Stop()
        {
            IsReading = false;
        }

        public void Clear()
        {
            Values.Clear();
        }

        public void Read()
        {
            GearedValues<ObservablePoint> copyValues = new GearedValues<ObservablePoint>();
            if (IsReading) return;

            //lets keep in memory only the last 20000 records,
            //to keep everything running faster

            IsReading = true;

            Action readFromTread = () =>
            {
                while (IsReading)
                {
                    Thread.Sleep(1000);

                    //when multi threading avoid indexed calls like -> Values[0] 
                    //instead enumerate the collection
                    //ChartValues/GearedValues returns a thread safe copy once you enumerate it.
                    //TIPS: use foreach instead of for
                    //LINQ methods also enumerate the collections

                    chartValue.Clear();
                    chartValue.AddRange(ChartsData.ChartPositionTimeValues);
                    int len = chartValue.Count;
                    if (len > 0)
                    {
                        //copyValues.AddRange(UAClientForm.ChartPositionTimeValues);

                        ChartsData.CartesianChartPositionTimeValues.AddRange(chartValue);

                        for (int i = 0; i < len & ChartsData.ChartPositionTimeValues.Count > 1; i++)
                            ChartsData.ChartPositionTimeValues.RemoveAt(0);

                    }
                    chartValue.Clear();
                }
            };

            //2 different tasks adding a value every ms
            //add as many tasks as you want to test this feature
            Task.Factory.StartNew(readFromTread);
            //Task.Factory.StartNew(readFromTread);
            //Task.Factory.StartNew(readFromTread);
            //Task.Factory.StartNew(readFromTread);
            //Task.Factory.StartNew(readFromTread);
            //Task.Factory.StartNew(readFromTread);
            //Task.Factory.StartNew(readFromTread);
            //Task.Factory.StartNew(readFromTread);
        }
    }
}
