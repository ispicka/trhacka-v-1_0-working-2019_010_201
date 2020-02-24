#define geared
//Define normal
using System;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Windows;
using Siemens.UAClientHelper;
#if geared

using LiveCharts.Geared;

#endif


namespace trhacka_v_1_0_working_2019_010_201
{
    struct DataPID
    {
        public double W;
        public double Y;
        public double E;
        public double U;
        public double P;
        public double I;
        public double D;
#pragma warning disable CS0649 // Field 'DataPID.error' is never assigned to, and will always have its default value false
        public bool error;
#pragma warning restore CS0649 // Field 'DataPID.error' is never assigned to, and will always have its default value false
    };

#if geard
    public class ChartsData
    {
        static double setSmoothness = 0;
    #region variables
        public struct ValuesWithString
        {
            public string Text;
            public double Value;
        };
        public static UAClientHelperAPI myClientHelperAPI = new UAClientHelperAPI();
        public static ValuesWithString textBoxPositionTime = new ValuesWithString();
        public static ValuesWithString textBoxPosition = new ValuesWithString();
        public static ValuesWithString textBoxVelocityTime = new ValuesWithString();
        public static ValuesWithString textBoxVelocity = new ValuesWithString();
        public static ValuesWithString textBoxStrainTime = new ValuesWithString();
        public static ValuesWithString textBoxStrain = new ValuesWithString();


        public static bool endOfSubstription = true;
        public static bool lockVelocityTime = false;
        public static bool lockPositionTime = false;
        public static bool lockStrainTime = false;
        public static bool lockVelocityStrain = false;
        public static bool lockPositionStrain = false;
        public static bool checkBoxDisplayValueChecked = false;
        public static double lastTimeVelocity = 0, lastTimePosition = 0, lastTimeStrain = 0, lastValueVelocity = 0, lastValuePosition = 0,
            lastValueStrain = 0, lastValuePositionStrainPosition = 0, lastValuePositionStrainStrain = 0,
            lastValueVelocityStrainVelocity = 0, lastValueVelocityStrainStrain = 0;
    #endregion variables
    #region charts values
        public static GearedValues<ObservablePoint> _cartesianChartVelocityTimeValues = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static GearedValues<ObservablePoint> _cartesianChartPositionTimeValues = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static GearedValues<ObservablePoint> _cartesianChartStrainTimeValues = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };

        public static GearedValues<ObservablePoint> _ChartVelocityTimeValues = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static GearedValues<ObservablePoint> _ChartPositionTimeValues = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static GearedValues<ObservablePoint> _ChartStrainTimeValues = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static GearedValues<ObservablePoint> _ChartVelocityTimeValuesLock = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static GearedValues<ObservablePoint> _ChartPositionTimeValuesLock = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static GearedValues<ObservablePoint> _ChartStrainTimeValuesLock = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };

        public static GearedValues<ObservablePoint> _cartesianChartVelocityStrainValues = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static GearedValues<ObservablePoint> _cartesianChartPositionStrainValues = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };

        public static GearedValues<ObservablePoint> _ChartVelocityStrainValues = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static GearedValues<ObservablePoint> _ChartPositionStrainValues = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };

        public static GearedValues<ObservablePoint> _ChartVelocityStrainValuesLock = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static GearedValues<ObservablePoint> _ChartPositionStrainValuesLock = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };

        public static GearedValues<ObservablePoint> _ChartVelocityStrainValuesCopy = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static GearedValues<ObservablePoint> _ChartPositionStrainValuesCopy = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };

        public static GearedValues<ObservablePoint> _ChartVelocityTimeValuesCopy = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static GearedValues<ObservablePoint> _ChartPositionTimeValuesCopy = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static GearedValues<ObservablePoint> _ChartStrainTimeValuesCopy = new GearedValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        private static GearedValues<ObservablePoint> chartPidTimeVelocity = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidTimePosition = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidTimeForce = new GearedValues<ObservablePoint> { };
        private static GearedValues<double> chartPidVelocity = new GearedValues<double> { };
        private static GearedValues<double> chartPidPosition = new GearedValues<double> { };
        private static GearedValues<double> chartPidForce = new GearedValues<double> { };
        private static GearedValues<ObservablePoint> chartPidTimeVelocityW = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidTimePositionW = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidTimeForceW = new GearedValues<ObservablePoint> { };
        private static GearedValues<double> chartPidVelocityW = new GearedValues<double> { };
        private static GearedValues<double> chartPidPositionW = new GearedValues<double> { };
        private static GearedValues<double> chartPidForceW = new GearedValues<double> { };
        private static GearedValues<ObservablePoint> chartPidTimeVelocityY = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidTimePositionY = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidTimeForceY = new GearedValues<ObservablePoint> { };
        private static GearedValues<double> chartPidVelocityY = new GearedValues<double> { };
        private static GearedValues<double> chartPidPositionY = new GearedValues<double> { };
        private static GearedValues<double> chartPidForceY = new GearedValues<double> { };
        private static GearedValues<ObservablePoint> chartPidTimeVelocityE = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidTimePositionE = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidTimeForceE = new GearedValues<ObservablePoint> { };
        private static GearedValues<double> chartPidVelocityE = new GearedValues<double> { };
        private static GearedValues<double> chartPidPositionE = new GearedValues<double> { };
        private static GearedValues<double> chartPidForceE = new GearedValues<double> { };
        private static GearedValues<ObservablePoint> chartPidTimeVelocityI = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidTimePositionI = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidTimeForceI = new GearedValues<ObservablePoint> { };
        private static GearedValues<double> chartPidVelocityI = new GearedValues<double> { };
        private static GearedValues<double> chartPidPositionI = new GearedValues<double> { };
        private static GearedValues<double> chartPidForceI = new GearedValues<double> { };
        private static GearedValues<ObservablePoint> chartPidTimeVelocityD = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidTimePositionD = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidTimeForceD = new GearedValues<ObservablePoint> { };
        private static GearedValues<double> chartPidVelocityD = new GearedValues<double> { };
        private static GearedValues<double> chartPidPositionD = new GearedValues<double> { };
        private static GearedValues<double> chartPidForceD = new GearedValues<double> { };
        private static GearedValues<ObservablePoint> chartPidTimeVelocityU = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidTimePositionU = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidTimeForceU = new GearedValues<ObservablePoint> { };


        private static GearedValues<ObservablePoint> chartPidBuffVelocity = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffPosition = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffForce = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffVelocityW = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffPositionW = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffForceW = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffVelocityY = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffPositionY = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffForceY = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffVelocityE = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffPositionE = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffForceE = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffVelocityI = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffPositionI = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffForceI = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffVelocityD = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffPositionD = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffForceD = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffVelocityU = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffPositionU = new GearedValues<ObservablePoint> { };
        private static GearedValues<ObservablePoint> chartPidBuffForceU = new GearedValues<ObservablePoint> { };

    #endregion
    #region Axes
        public static Func<double, string> formatFunc = (x) => string.Format("{0:0.00}", x);
        static Axis _TimeAxis = new Axis
        {
            Title = "Čas [ms]"
        };
        static Axis _SpeedAxisX = new Axis
        {
            Title = "Rychlost [mm/s]"
        };
        static Axis _SpeedAxisY = new Axis
        {
            Title = "Rychlost [mm/s]",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.DodgerBlue,
            LabelFormatter = formatFunc
        };
        static Axis _PositionAxisX = new Axis
        {
            Title = "Pozice [mm]"
        };
        static Axis _PositionAxisY = new Axis
        {
            Title = "Pozice [mm]",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.IndianRed,
            LabelFormatter = formatFunc
        };
        static Axis _StrainAxisY = new Axis
        {
            Title = "Síla [kN]",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.DarkOliveGreen,
            LabelFormatter = formatFunc
        };
        static Axis _StrainAxisPositionY = new Axis
        {
            Title = "Síla [kN]",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.DarkOliveGreen,
            LabelFormatter = formatFunc
        };
        static Axis _StrainAxisVelocityY = new Axis
        {
            Title = "Síla [kN]",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.DarkOliveGreen,
            LabelFormatter = formatFunc
        };
        static Axis _PidX = new Axis
        {
            Title = "Time [s]",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Black,
            LabelFormatter = formatFunc
        };
        static Axis _PidU = new Axis
        {
            Title = "U",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Green,
            LabelFormatter = formatFunc

        };
        static Axis _PidW = new Axis
        {
            Title = "W",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Blue,
            LabelFormatter = formatFunc

        };
        static Axis _PidY = new Axis
        {
            Title = "Y",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Red,
            LabelFormatter = formatFunc

        };
        static Axis _PidE = new Axis
        {
            Title = "E",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Black,
            LabelFormatter = formatFunc

        };
        static Axis _PidI = new Axis
        {
            Title = "I",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Yellow,
            LabelFormatter = formatFunc

        };
        static Axis _PidD = new Axis
        {
            Title = "D",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Magenta,
            LabelFormatter = formatFunc

        };
    #endregion
    #region charts series
        static GLineSeries _cartesianChartVelocityTimeSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = CartesianChartVelocityTimeValues,
            Stroke = System.Windows.Media.Brushes.DodgerBlue,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };

        static GLineSeries _cartesianChartPositionTimeSeries = new GLineSeries
        {
            ScalesYAt = 1,
            Values = CartesianChartPositionTimeValues,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };

        static GLineSeries _cartesianChartStrainTimeSeries = new GLineSeries
        {
            ScalesYAt = 2,
            Values = CartesianChartStrainTimeValues,
            Stroke = System.Windows.Media.Brushes.DarkOliveGreen,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };

        static GLineSeries _cartesianChartVelocityStrainSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = CartesianChartVelocityStrainValues,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };
        static GLineSeries _cartesianChartPositionStrainSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = _cartesianChartPositionStrainValues,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };

        static GLineSeries _ChartVelocityTimeSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };

        static GLineSeries _ChartPositionTimeSeries = new GLineSeries
        {
            ScalesYAt = 1,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };

        static GLineSeries _ChartStrainTimeSeries = new GLineSeries
        {
            ScalesYAt = 2,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };

        static GLineSeries _ChartVelocityStrainSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };
        static GLineSeries _ChartPositionStrainSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };
        static ColumnSeries _ChartPidVelocityESeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidVelocityWSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidVelocityYSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidVelocityUSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidVelocityDSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1
        }; static ColumnSeries _ChartPidVelocityISeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidPositionESeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidPositionWSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidPositionYSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidPositionUSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidPositionDSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1
        }; static ColumnSeries _ChartPidPositionISeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidForceESeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidForceWSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidForceYSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidForceUSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidForceDSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1
        }; static ColumnSeries _ChartPidForceISeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1
        };


        static GLineSeries _ChartPidTimeVelocityESeries = new GLineSeries
        {
            ScalesYAt = 5          ,
            Values = chartPidTimeVelocityE,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimeVelocityWSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimeVelocityW,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimeVelocityYSeries = new GLineSeries
        {
            ScalesYAt = 1,
            Values = chartPidTimeVelocityY,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimeVelocityUSeries = new GLineSeries
        {
            ScalesYAt = 2,
            Values = chartPidTimeVelocityU,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimeVelocityDSeries = new GLineSeries
        {
            ScalesYAt = 4,
            Values = chartPidTimeVelocityD,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimeVelocityISeries = new GLineSeries
        {
            ScalesYAt = 3,
            Values = chartPidTimeVelocityI,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };

        static GLineSeries _ChartPidTimePositionESeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimePositionE,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimePositionWSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionW,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimePositionYSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionY,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimePositionUSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionU,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimePositionDSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionD,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimePositionISeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionI,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };

        static GLineSeries _ChartPidTimeForceESeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceE,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimeForceWSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceW,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimeForceYSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceY,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimeForceUSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceU,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimeForceDSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceD,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidTimeForceISeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceI,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };

        static GLineSeries _ChartPidBuffVelocityESeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimeVelocityE,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffVelocityWSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimeVelocityW,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffVelocityYSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimeVelocityY,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffVelocityUSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimeVelocityU,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffVelocityDSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimeVelocityD,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffVelocityISeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimeVelocityI,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffPositionESeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimePositionE,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffPositionWSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionW,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffPositionYSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionY,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffPositionUSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionU,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffPositionDSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionD,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffPositionISeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionI,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffForceESeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceE,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffForceWSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceW,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffForceYSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceY,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffForceUSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceU,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffForceDSeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceD,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static GLineSeries _ChartPidBuffForceISeries = new GLineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceI,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };

        public int refreshPointsVelocityChart, countRefreshVelocityChart = 0;

        static public ChartValues<ObservablePoint> CartesianChartVelocityTimeValues { get => _cartesianChartVelocityTimeValues; set => _cartesianChartVelocityTimeValues = value; }
        static public ChartValues<ObservablePoint> CartesianChartPositionTimeValues { get => _cartesianChartPositionTimeValues; set => _cartesianChartPositionTimeValues = value; }
        static public ChartValues<ObservablePoint> CartesianChartStrainTimeValues { get => _cartesianChartStrainTimeValues; set => _cartesianChartStrainTimeValues = value; }
        static public ChartValues<ObservablePoint> ChartVelocityTimeValues { get => _ChartVelocityTimeValues; set => _ChartVelocityTimeValues = value; }
        static public ChartValues<ObservablePoint> ChartPositionTimeValues { get => _ChartPositionTimeValues; set => _ChartPositionTimeValues = value; }
        public static ChartValues<ObservablePoint> ChartStrainTimeValues { get => _ChartStrainTimeValues; set => _ChartStrainTimeValues = value; }
        static public ChartValues<ObservablePoint> CartesianChartVelocityStrainValues { get => _cartesianChartVelocityStrainValues; set => _cartesianChartVelocityStrainValues = value; }
        static public ChartValues<ObservablePoint> CartesianChartPositionStrainValues { get => _cartesianChartPositionStrainValues; set => _cartesianChartPositionStrainValues = value; }
        static public ChartValues<ObservablePoint> ChartVelocityStrainValues { get => _ChartVelocityStrainValues; set => _ChartVelocityStrainValues = value; }
        static public ChartValues<ObservablePoint> ChartPositionStrainValues { get => _ChartPositionStrainValues; set => _ChartPositionStrainValues = value; }
        static public Axis TimeAxis { get => _TimeAxis; set => _TimeAxis = value; }
        static public Axis SpeedAxisX { get => _SpeedAxisX; set => _SpeedAxisX = value; }
        static public Axis SpeedAxisY { get => _SpeedAxisY; set => _SpeedAxisY = value; }
        static public Axis PositionAxisX { get => _PositionAxisX; set => _PositionAxisX = value; }
        static public Axis PositionAxisY { get => _PositionAxisY; set => _PositionAxisY = value; }
        static public Axis StrainAxisY { get => _StrainAxisY; set => _StrainAxisY = value; }
        static public Axis StrainAxisPositionY { get => _StrainAxisPositionY; set => _StrainAxisPositionY = value; }
        static public Axis StrainAxisVelocityY { get => _StrainAxisVelocityY; set => _StrainAxisVelocityY = value; }
        public static GLineSeries CartesianChartVelocityTimeSeries { get => _cartesianChartVelocityTimeSeries; set => _cartesianChartVelocityTimeSeries = value; }
        public static GLineSeries CartesianChartPositionTimeSeries { get => _cartesianChartPositionTimeSeries; set => _cartesianChartPositionTimeSeries = value; }
        public static GLineSeries CartesianChartStrainTimeSeries { get => _cartesianChartStrainTimeSeries; set => _cartesianChartStrainTimeSeries = value; }
        public static GLineSeries CartesianChartVelocityStrainSeries { get => _cartesianChartVelocityStrainSeries; set => _cartesianChartVelocityStrainSeries = value; }
        public static GLineSeries CartesianChartPositionStrainSeries { get => _cartesianChartPositionStrainSeries; set => _cartesianChartPositionStrainSeries = value; }
        public static GLineSeries ChartVelocityTimeSeries { get => _ChartVelocityTimeSeries; set => _ChartVelocityTimeSeries = value; }
        public static GLineSeries ChartPositionTimeSeries { get => _ChartPositionTimeSeries; set => _ChartPositionTimeSeries = value; }
        public static GLineSeries ChartStrainTimeSeries { get => _ChartStrainTimeSeries; set => _ChartStrainTimeSeries = value; }
        public static GLineSeries ChartVelocityStrainSeries { get => _ChartVelocityStrainSeries; set => _ChartVelocityStrainSeries = value; }
        public static GLineSeries ChartPositionStrainSeries { get => _ChartPositionStrainSeries; set => _ChartPositionStrainSeries = value; }
        public static ChartValues<ObservablePoint> ChartVelocityStrainValuesLock { get => _ChartVelocityStrainValuesLock; set => _ChartVelocityStrainValuesLock = value; }
        public static ChartValues<ObservablePoint> ChartPositionStrainValuesLock { get => _ChartPositionStrainValuesLock; set => _ChartPositionStrainValuesLock = value; }
        public static ChartValues<ObservablePoint> ChartVelocityTimeValuesLock { get => _ChartVelocityTimeValuesLock; set => _ChartVelocityTimeValuesLock = value; }
        public static ChartValues<ObservablePoint> ChartPositionTimeValuesLock { get => _ChartPositionTimeValuesLock; set => _ChartPositionTimeValuesLock = value; }
        public static ChartValues<ObservablePoint> ChartStrainTimeValuesLock { get => _ChartStrainTimeValuesLock; set => _ChartStrainTimeValuesLock = value; }
        public static ChartValues<ObservablePoint> ChartPositionStrainValuesCopy { get => _ChartPositionStrainValuesCopy; set => _ChartPositionStrainValuesCopy = value; }
        public static ChartValues<ObservablePoint> ChartVelocityStrainValuesCopy { get => _ChartVelocityStrainValuesCopy; set => _ChartVelocityStrainValuesCopy = value; }
        public static ChartValues<ObservablePoint> ChartVelocityTimeValuesCopy { get => _ChartVelocityTimeValuesCopy; set => _ChartVelocityTimeValuesCopy = value; }
        public static ChartValues<ObservablePoint> ChartPositionTimeValuesCopy { get => _ChartPositionTimeValuesCopy; set => _ChartPositionTimeValuesCopy = value; }
        public static ChartValues<ObservablePoint> ChartStrainTimeValuesCopy { get => _ChartStrainTimeValuesCopy; set => _ChartStrainTimeValuesCopy = value; }
        public static ColumnSeries ChartPidVelocityESeries { get => _ChartPidVelocityESeries; set => _ChartPidVelocityESeries = value; }
        public static ColumnSeries ChartPidVelocityWSeries { get => _ChartPidVelocityWSeries; set => _ChartPidVelocityWSeries = value; }
        public static ColumnSeries ChartPidVelocityYSeries { get => _ChartPidVelocityYSeries; set => _ChartPidVelocityYSeries = value; }
        public static ColumnSeries ChartPidVelocityUSeries { get => _ChartPidVelocityUSeries; set => _ChartPidVelocityUSeries = value; }
        public static ColumnSeries ChartPidPositionESeries { get => _ChartPidPositionESeries; set => _ChartPidPositionESeries = value; }
        public static ColumnSeries ChartPidPositionWSeries { get => _ChartPidPositionWSeries; set => _ChartPidPositionWSeries = value; }
        public static ColumnSeries ChartPidForceESeries { get => _ChartPidForceESeries; set => _ChartPidForceESeries = value; }
        public static ColumnSeries ChartPidForceWSeries { get => _ChartPidForceWSeries; set => _ChartPidForceWSeries = value; }
        public static ColumnSeries ChartPidForceUSeries { get => _ChartPidForceUSeries; set => _ChartPidForceUSeries = value; }
        public static GLineSeries ChartPidTimeVelocityESeries { get => _ChartPidTimeVelocityESeries; set => _ChartPidTimeVelocityESeries = value; }
        public static GLineSeries ChartPidTimeVelocityWSeries { get => _ChartPidTimeVelocityWSeries; set => _ChartPidTimeVelocityWSeries = value; }
        public static GLineSeries ChartPidTimeVelocityYSeries { get => _ChartPidTimeVelocityYSeries; set => _ChartPidTimeVelocityYSeries = value; }
        public static GLineSeries ChartPidTimeVelocityUSeries { get => _ChartPidTimeVelocityUSeries; set => _ChartPidTimeVelocityUSeries = value; }
        public static GLineSeries ChartPidTimePositionESeries { get => _ChartPidTimePositionESeries; set => _ChartPidTimePositionESeries = value; }
        public static GLineSeries ChartPidTimePositionWSeries { get => _ChartPidTimePositionWSeries; set => _ChartPidTimePositionWSeries = value; }
        public static GLineSeries ChartPidTimePositionYSeries { get => _ChartPidTimePositionYSeries; set => _ChartPidTimePositionYSeries = value; }
        public static GLineSeries ChartPidTimePositionUSeries { get => _ChartPidTimePositionUSeries; set => _ChartPidTimePositionUSeries = value; }
        public static GLineSeries ChartPidTimeForceESeries { get => _ChartPidTimeForceESeries; set => _ChartPidTimeForceESeries = value; }
        public static GLineSeries ChartPidTimeForceYSeries { get => _ChartPidTimeForceYSeries; set => _ChartPidTimeForceYSeries = value; }
        public static GLineSeries ChartPidTimeForceUSeries { get => _ChartPidTimeForceUSeries; set => _ChartPidTimeForceUSeries = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeVelocity { get => chartPidTimeVelocity; set => chartPidTimeVelocity = value; }
        public static ChartValues<ObservablePoint> ChartPidTimePosition { get => chartPidTimePosition; set => chartPidTimePosition = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeForce { get => chartPidTimeForce; set => chartPidTimeForce = value; }
        public static ChartValues<double> ChartPidVelocity { get => chartPidVelocity; set => chartPidVelocity = value; }
        public static ChartValues<double> ChartPidPosition { get => chartPidPosition; set => chartPidPosition = value; }
        public static ChartValues<double> ChartPidForce { get => chartPidForce; set => chartPidForce = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeVelocityW { get => chartPidTimeVelocityW; set => chartPidTimeVelocityW = value; }
        public static ChartValues<ObservablePoint> ChartPidTimePositionW { get => chartPidTimePositionW; set => chartPidTimePositionW = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeForceW { get => chartPidTimeForceW; set => chartPidTimeForceW = value; }
        public static ChartValues<double> ChartPidVelocityW { get => chartPidVelocityW; set => chartPidVelocityW = value; }
        public static ChartValues<double> ChartPidPositionW { get => chartPidPositionW; set => chartPidPositionW = value; }
        public static ChartValues<double> ChartPidForceW { get => chartPidForceW; set => chartPidForceW = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeVelocityY { get => chartPidTimeVelocityY; set => chartPidTimeVelocityY = value; }
        public static ChartValues<ObservablePoint> ChartPidTimePositionY { get => chartPidTimePositionY; set => chartPidTimePositionY = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeForceY { get => chartPidTimeForceY; set => chartPidTimeForceY = value; }
        public static ChartValues<double> ChartPidVelocityY { get => chartPidVelocityY; set => chartPidVelocityY = value; }
        public static ChartValues<double> ChartPidPositionY { get => chartPidPositionY; set => chartPidPositionY = value; }
        public static ChartValues<double> ChartPidForceY { get => chartPidForceY; set => chartPidForceY = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeVelocityE { get => chartPidTimeVelocityE; set => chartPidTimeVelocityE = value; }
        public static ChartValues<ObservablePoint> ChartPidTimePositionE { get => chartPidTimePositionE; set => chartPidTimePositionE = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeForceE { get => chartPidTimeForceE; set => chartPidTimeForceE = value; }
        public static ChartValues<double> ChartPidVelocityE { get => chartPidVelocityE; set => chartPidVelocityE = value; }
        public static ChartValues<double> ChartPidPositionE { get => chartPidPositionE; set => chartPidPositionE = value; }
        public static ChartValues<double> ChartPidForceE { get => chartPidForceE; set => chartPidForceE = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeVelocityI { get => chartPidTimeVelocityI; set => chartPidTimeVelocityI = value; }
        public static ChartValues<ObservablePoint> ChartPidTimePositionI { get => chartPidTimePositionI; set => chartPidTimePositionI = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeForceI { get => chartPidTimeForceI; set => chartPidTimeForceI = value; }
        public static ChartValues<double> ChartPidVelocityI { get => chartPidVelocityI; set => chartPidVelocityI = value; }
        public static ChartValues<double> ChartPidPositionI { get => chartPidPositionI; set => chartPidPositionI = value; }
        public static ChartValues<double> ChartPidForceI { get => chartPidForceI; set => chartPidForceI = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeVelocityD { get => chartPidTimeVelocityD; set => chartPidTimeVelocityD = value; }
        public static ChartValues<ObservablePoint> ChartPidTimePositionD { get => chartPidTimePositionD; set => chartPidTimePositionD = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeForceD { get => chartPidTimeForceD; set => chartPidTimeForceD = value; }
        public static ChartValues<double> ChartPidVelocityD { get => chartPidVelocityD; set => chartPidVelocityD = value; }
        public static ChartValues<double> ChartPidPositionD { get => chartPidPositionD; set => chartPidPositionD = value; }
        public static ChartValues<double> ChartPidForceD { get => chartPidForceD; set => chartPidForceD = value; }
        public static Axis PidX { get => _PidX; set => _PidX = value; }
        public static Axis PidU { get => _PidU; set => _PidU = value; }
        public static Axis PidW { get => _PidW; set => _PidW = value; }
        public static Axis PidY { get => _PidY; set => _PidY = value; }
        public static Axis PidE { get => _PidE; set => _PidE = value; }
        public static Axis PidI { get => _PidI; set => _PidI = value; }
        public static Axis PidD { get => _PidD; set => _PidD = value; }
        public static GLineSeries ChartPidTimeForceISeries { get => _ChartPidTimeForceISeries; set => _ChartPidTimeForceISeries = value; }
        public static GLineSeries ChartPidTimeForceDSeries { get => _ChartPidTimeForceDSeries; set => _ChartPidTimeForceDSeries = value; }
        public static GLineSeries ChartPidTimeForceWSeries { get => _ChartPidTimeForceWSeries; set => _ChartPidTimeForceWSeries = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeVelocityU { get => chartPidTimeVelocityU; set => chartPidTimeVelocityU = value; }
        public static ChartValues<ObservablePoint> ChartPidTimePositionU { get => chartPidTimePositionU; set => chartPidTimePositionU = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeForceU { get => chartPidTimeForceU; set => chartPidTimeForceU = value; }
        public static GLineSeries ChartPidTimeVelocityDSeries { get => _ChartPidTimeVelocityDSeries; set => _ChartPidTimeVelocityDSeries = value; }
        public static GLineSeries ChartPidTimeVelocityISeries { get => _ChartPidTimeVelocityISeries; set => _ChartPidTimeVelocityISeries = value; }
        public static GLineSeries ChartPidTimePositionISeries { get => _ChartPidTimePositionISeries; set => _ChartPidTimePositionISeries = value; }
        public static GLineSeries ChartPidTimePositionDSeries { get => _ChartPidTimePositionDSeries; set => _ChartPidTimePositionDSeries = value; }
        public static ColumnSeries ChartPidForceDSeries { get => _ChartPidForceDSeries; set => _ChartPidForceDSeries = value; }
        public static ColumnSeries ChartPidForceISeries { get => _ChartPidForceISeries; set => _ChartPidForceISeries = value; }
        public static ColumnSeries ChartPidForceYSeries { get => _ChartPidForceYSeries; set => _ChartPidForceYSeries = value; }
        public static GLineSeries ChartPidBuffVelocityESeries { get => _ChartPidBuffVelocityESeries; set => _ChartPidBuffVelocityESeries = value; }
        public static GLineSeries ChartPidBuffVelocityWSeries { get => _ChartPidBuffVelocityWSeries; set => _ChartPidBuffVelocityWSeries = value; }
        public static GLineSeries ChartPidBuffVelocityYSeries { get => _ChartPidBuffVelocityYSeries; set => _ChartPidBuffVelocityYSeries = value; }
        public static GLineSeries ChartPidBuffVelocityUSeries { get => _ChartPidBuffVelocityUSeries; set => _ChartPidBuffVelocityUSeries = value; }
        public static GLineSeries ChartPidBuffVelocityDSeries { get => _ChartPidBuffVelocityDSeries; set => _ChartPidBuffVelocityDSeries = value; }
        public static GLineSeries ChartPidBuffVelocityISeries { get => _ChartPidBuffVelocityISeries; set => _ChartPidBuffVelocityISeries = value; }
        public static GLineSeries ChartPidBuffPositionESeries { get => _ChartPidBuffPositionESeries; set => _ChartPidBuffPositionESeries = value; }
        public static GLineSeries ChartPidBuffPositionWSeries { get => _ChartPidBuffPositionWSeries; set => _ChartPidBuffPositionWSeries = value; }
        public static GLineSeries ChartPidBuffPositionYSeries { get => _ChartPidBuffPositionYSeries; set => _ChartPidBuffPositionYSeries = value; }
        public static GLineSeries ChartPidBuffPositionUSeries { get => _ChartPidBuffPositionUSeries; set => _ChartPidBuffPositionUSeries = value; }
        public static GLineSeries ChartPidBuffPositionDSeries { get => _ChartPidBuffPositionDSeries; set => _ChartPidBuffPositionDSeries = value; }
        public static GLineSeries ChartPidBuffPositionISeries { get => _ChartPidBuffPositionISeries; set => _ChartPidBuffPositionISeries = value; }
        public static GLineSeries ChartPidBuffForceESeries { get => _ChartPidBuffForceESeries; set => _ChartPidBuffForceESeries = value; }
        public static GLineSeries ChartPidBuffForceWSeries { get => _ChartPidBuffForceWSeries; set => _ChartPidBuffForceWSeries = value; }
        public static GLineSeries ChartPidBuffForceYSeries { get => _ChartPidBuffForceYSeries; set => _ChartPidBuffForceYSeries = value; }
        public static GLineSeries ChartPidBuffForceUSeries { get => _ChartPidBuffForceUSeries; set => _ChartPidBuffForceUSeries = value; }
        public static GLineSeries ChartPidBuffForceDSeries { get => _ChartPidBuffForceDSeries; set => _ChartPidBuffForceDSeries = value; }
        public static GLineSeries ChartPidBuffForceISeries { get => _ChartPidBuffForceISeries; set => _ChartPidBuffForceISeries = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffVelocity { get => chartPidBuffVelocity; set => chartPidBuffVelocity = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffPosition { get => chartPidBuffPosition; set => chartPidBuffPosition = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffForce { get => chartPidBuffForce; set => chartPidBuffForce = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffVelocityW { get => chartPidBuffVelocityW; set => chartPidBuffVelocityW = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffPositionW { get => chartPidBuffPositionW; set => chartPidBuffPositionW = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffForceW { get => chartPidBuffForceW; set => chartPidBuffForceW = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffVelocityY { get => chartPidBuffVelocityY; set => chartPidBuffVelocityY = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffPositionY { get => chartPidBuffPositionY; set => chartPidBuffPositionY = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffForceY { get => chartPidBuffForceY; set => chartPidBuffForceY = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffVelocityE { get => chartPidBuffVelocityE; set => chartPidBuffVelocityE = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffPositionE { get => chartPidBuffPositionE; set => chartPidBuffPositionE = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffForceE { get => chartPidBuffForceE; set => chartPidBuffForceE = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffVelocityI { get => chartPidBuffVelocityI; set => chartPidBuffVelocityI = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffPositionI { get => chartPidBuffPositionI; set => chartPidBuffPositionI = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffForceI { get => chartPidBuffForceI; set => chartPidBuffForceI = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffVelocityD { get => chartPidBuffVelocityD; set => chartPidBuffVelocityD = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffPositionD { get => chartPidBuffPositionD; set => chartPidBuffPositionD = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffForceD { get => chartPidBuffForceD; set => chartPidBuffForceD = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffVelocityU { get => chartPidBuffVelocityU; set => chartPidBuffVelocityU = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffPositionU { get => chartPidBuffPositionU; set => chartPidBuffPositionU = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffForceU { get => chartPidBuffForceU; set => chartPidBuffForceU = value; }


    #endregion

        public static void WriteValuesToChart(string time, string vel, ChartValues<ObservablePoint> ChartValues, ChartValues<ObservablePoint> ChartValuesLock, ref bool lck, ref double lastTime, ref double lastVal)
        {
            //if (cartesianChartVelocityForce.InvokeRequired)
            //    cartesianChartVelocityForce.BeginInvoke(new writeValuesToChartEventHandler(WriteValuesToChart), time, vel, ChartValues);
            double val, valTick;
            if (!ChartsData.checkBoxDisplayValueChecked)
            {
                return;
            }

            bool parsedVel = double.TryParse(vel, out val);
            bool parsedTime = double.TryParse(time, out valTick);

            try
            {

                //if (ChartValues.Count != 0)
                //{
                if (val == lastVal && valTick == lastTime || !parsedTime || !parsedVel)
                {

                    return;
                }

                ObservablePoint oPoint = new ObservablePoint(valTick, val);

                if (lck)
                {
                    ChartValuesLock.Add(oPoint);
                }
                else
                {
                    lck = true;

                    if (ChartValuesLock.Count > 0)
                    {
                        ChartValues.AddRange(ChartValuesLock);
                        ChartValuesLock.Clear();
                    }
                    ChartValues.Add(oPoint);
                    lck = false;
                }

                lastVal = val; lastTime = valTick;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                endOfSubstription = true;
            }

        }


    }
#else
    public class ChartsData
    {
        static double setSmoothness = 0;
        #region variables
        public struct ValuesWithString
        {
            public string Text;
            public double Value;
        };
        public static UAClientHelperAPI myClientHelperAPI = new UAClientHelperAPI();
        public static ValuesWithString textBoxPositionTime = new ValuesWithString();
        public static ValuesWithString textBoxPosition = new ValuesWithString();
        public static ValuesWithString textBoxVelocityTime = new ValuesWithString();
        public static ValuesWithString textBoxVelocity = new ValuesWithString();
        public static ValuesWithString textBoxStrainTime = new ValuesWithString();
        public static ValuesWithString textBoxStrain = new ValuesWithString();


        public static bool endOfSubstription = true;
        public static bool lockVelocityTime = false;
        public static bool lockPositionTime = false;
        public static bool lockStrainTime = false;
        public static bool lockVelocityStrain = false;
        public static bool lockPositionStrain = false;
        public static bool checkBoxDisplayValueChecked = false;
        public static double lastTimeVelocity = 0, lastTimePosition = 0, lastTimeStrain = 0, lastValueVelocity = 0, lastValuePosition = 0,
            lastValueStrain = 0, lastValuePositionStrainPosition = 0, lastValuePositionStrainStrain = 0,
            lastValueVelocityStrainVelocity = 0, lastValueVelocityStrainStrain = 0;
        #endregion variables
        #region charts values
        public static ChartValues<ObservablePoint> _cartesianChartVelocityTimeValues = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static ChartValues<ObservablePoint> _cartesianChartPositionTimeValues = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static ChartValues<ObservablePoint> _cartesianChartStrainTimeValues = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };

        public static ChartValues<ObservablePoint> _ChartVelocityTimeValues = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static ChartValues<ObservablePoint> _ChartPositionTimeValues = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static ChartValues<ObservablePoint> _ChartStrainTimeValues = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static ChartValues<ObservablePoint> _ChartVelocityTimeValuesLock = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static ChartValues<ObservablePoint> _ChartPositionTimeValuesLock = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static ChartValues<ObservablePoint> _ChartStrainTimeValuesLock = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };

        public static ChartValues<ObservablePoint> _cartesianChartVelocityStrainValues = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static ChartValues<ObservablePoint> _cartesianChartPositionStrainValues = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };

        public static ChartValues<ObservablePoint> _ChartVelocityStrainValues = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static ChartValues<ObservablePoint> _ChartPositionStrainValues = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };

        public static ChartValues<ObservablePoint> _ChartVelocityStrainValuesLock = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static ChartValues<ObservablePoint> _ChartPositionStrainValuesLock = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };

        public static ChartValues<ObservablePoint> _ChartVelocityStrainValuesCopy = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static ChartValues<ObservablePoint> _ChartPositionStrainValuesCopy = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };

        public static ChartValues<ObservablePoint> _ChartVelocityTimeValuesCopy = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static ChartValues<ObservablePoint> _ChartPositionTimeValuesCopy = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        public static ChartValues<ObservablePoint> _ChartStrainTimeValuesCopy = new ChartValues<ObservablePoint>
        {
            //new ObservablePoint(0, 0)
        };
        private static ChartValues<ObservablePoint> chartPidTimeVelocity = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidTimePosition = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidTimeForce = new ChartValues<ObservablePoint> { };
        private static ChartValues<double> chartPidVelocity = new ChartValues<double> { };
        private static ChartValues<double> chartPidPosition = new ChartValues<double> { };
        private static ChartValues<double> chartPidForce = new ChartValues<double> { };
        private static ChartValues<ObservablePoint> chartPidTimeVelocityW = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidTimePositionW = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidTimeForceW = new ChartValues<ObservablePoint> { };
        private static ChartValues<double> chartPidVelocityW = new ChartValues<double> { };
        private static ChartValues<double> chartPidPositionW = new ChartValues<double> { };
        private static ChartValues<double> chartPidForceW = new ChartValues<double> { };
        private static ChartValues<ObservablePoint> chartPidTimeVelocityY = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidTimePositionY = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidTimeForceY = new ChartValues<ObservablePoint> { };
        private static ChartValues<double> chartPidVelocityY = new ChartValues<double> { };
        private static ChartValues<double> chartPidPositionY = new ChartValues<double> { };
        private static ChartValues<double> chartPidForceY = new ChartValues<double> { };
        private static ChartValues<ObservablePoint> chartPidTimeVelocityE = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidTimePositionE = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidTimeForceE = new ChartValues<ObservablePoint> { };
        private static ChartValues<double> chartPidVelocityE = new ChartValues<double> { };
        private static ChartValues<double> chartPidPositionE = new ChartValues<double> { };
        private static ChartValues<double> chartPidForceE = new ChartValues<double> { };
        private static ChartValues<ObservablePoint> chartPidTimeVelocityI = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidTimePositionI = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidTimeForceI = new ChartValues<ObservablePoint> { };
        private static ChartValues<double> chartPidVelocityI = new ChartValues<double> { };
        private static ChartValues<double> chartPidPositionI = new ChartValues<double> { };
        private static ChartValues<double> chartPidForceI = new ChartValues<double> { };
        private static ChartValues<ObservablePoint> chartPidTimeVelocityD = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidTimePositionD = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidTimeForceD = new ChartValues<ObservablePoint> { };
        private static ChartValues<double> chartPidVelocityD = new ChartValues<double> { };
        private static ChartValues<double> chartPidPositionD = new ChartValues<double> { };
        private static ChartValues<double> chartPidForceD = new ChartValues<double> { };
        private static ChartValues<ObservablePoint> chartPidTimeVelocityU = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidTimePositionU = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidTimeForceU = new ChartValues<ObservablePoint> { };


        private static ChartValues<ObservablePoint> chartPidBuffVelocity = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffPosition = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffForce = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffVelocityW = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffPositionW = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffForceW = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffVelocityY = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffPositionY = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffForceY = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffVelocityE = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffPositionE = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffForceE = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffVelocityI = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffPositionI = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffForceI = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffVelocityD = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffPositionD = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffForceD = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffVelocityU = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffPositionU = new ChartValues<ObservablePoint> { };
        private static ChartValues<ObservablePoint> chartPidBuffForceU = new ChartValues<ObservablePoint> { };

        #endregion
        #region Axes
        public static Func<double, string> formatFunc = (x) => string.Format("{0:0.00}", x);
        static Axis _TimeAxis = new Axis
        {
            Title = "Čas [ms]"
        };
        static Axis _SpeedAxisX = new Axis
        {
            Title = "Rychlost [mm/s]"
        };
        static Axis _SpeedAxisY = new Axis
        {
            Title = "Rychlost [mm/s]",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.DodgerBlue,
            LabelFormatter = formatFunc
        };
        static Axis _PositionAxisX = new Axis
        {
            Title = "Pozice [mm]"
        };
        static Axis _PositionAxisY = new Axis
        {
            Title = "Pozice [mm]",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.IndianRed,
            LabelFormatter = formatFunc
        };
        static Axis _StrainAxisY = new Axis
        {
            Title = "Síla [kN]",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.DarkOliveGreen,
            LabelFormatter = formatFunc
        };
        static Axis _StrainAxisPositionY = new Axis
        {
            Title = "Síla [kN]",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.DarkOliveGreen,
            LabelFormatter = formatFunc
        };
        static Axis _StrainAxisVelocityY = new Axis
        {
            Title = "Síla [kN]",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.DarkOliveGreen,
            LabelFormatter = formatFunc
        };
        static Axis _PidX = new Axis
        {
            Title = "Time [s]",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Black,
            LabelFormatter = formatFunc
        };
        static Axis _PidU = new Axis
        {
            Title = "U",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Green,
            LabelFormatter = formatFunc

        };
        static Axis _PidW = new Axis
        {
            Title = "W",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Blue,
            LabelFormatter = formatFunc

        };
        static Axis _PidY = new Axis
        {
            Title = "Y",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Red,
            LabelFormatter = formatFunc

        };
        static Axis _PidE = new Axis
        {
            Title = "E",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Black,
            LabelFormatter = formatFunc

        };
        static Axis _PidI = new Axis
        {
            Title = "I",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Yellow,
            LabelFormatter = formatFunc

        };
        static Axis _PidD = new Axis
        {
            Title = "D",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Magenta,
            LabelFormatter = formatFunc

        };
        #endregion
        #region charts series
        static LineSeries _cartesianChartVelocityTimeSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = CartesianChartVelocityTimeValues,
            Stroke = System.Windows.Media.Brushes.DodgerBlue,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };

        static LineSeries _cartesianChartPositionTimeSeries = new LineSeries
        {
            ScalesYAt = 1,
            Values = CartesianChartPositionTimeValues,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };

        static LineSeries _cartesianChartStrainTimeSeries = new LineSeries
        {
            ScalesYAt = 2,
            Values = CartesianChartStrainTimeValues,
            Stroke = System.Windows.Media.Brushes.DarkOliveGreen,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };

        static LineSeries _cartesianChartVelocityStrainSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = CartesianChartVelocityStrainValues,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };
        static LineSeries _cartesianChartPositionStrainSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = _cartesianChartPositionStrainValues,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };

        static LineSeries _ChartVelocityTimeSeries = new LineSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };

        static LineSeries _ChartPositionTimeSeries = new LineSeries
        {
            ScalesYAt = 1,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };

        static LineSeries _ChartStrainTimeSeries = new LineSeries
        {
            ScalesYAt = 2,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };

        static LineSeries _ChartVelocityStrainSeries = new LineSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };
        static LineSeries _ChartPositionStrainSeries = new LineSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Red,
            Fill = System.Windows.Media.Brushes.Transparent,
            //StrokeDashArray = new DoubleCollection { 2 },
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            //PointGeometry = DefaultGeometries.Diamond,
            StrokeThickness = 0.5,
            PointGeometry = null //use a null geometry when you have many seri
        };
        static ColumnSeries _ChartPidVelocityESeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidVelocityWSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidVelocityYSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidVelocityUSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidVelocityDSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1
        }; static ColumnSeries _ChartPidVelocityISeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidPositionESeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidPositionWSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidPositionYSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidPositionUSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidPositionDSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1
        }; static ColumnSeries _ChartPidPositionISeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidForceESeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidForceWSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidForceYSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidForceUSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1
        };
        static ColumnSeries _ChartPidForceDSeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1
        }; static ColumnSeries _ChartPidForceISeries = new ColumnSeries
        {
            ScalesYAt = 0,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1
        };


        static LineSeries _ChartPidTimeVelocityESeries = new LineSeries
        {
            ScalesYAt = 5,
            Values = chartPidTimeVelocityE,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimeVelocityWSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimeVelocityW,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimeVelocityYSeries = new LineSeries
        {
            ScalesYAt = 1,
            Values = chartPidTimeVelocityY,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimeVelocityUSeries = new LineSeries
        {
            ScalesYAt = 2,
            Values = chartPidTimeVelocityU,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimeVelocityDSeries = new LineSeries
        {
            ScalesYAt = 4,
            Values = chartPidTimeVelocityD,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimeVelocityISeries = new LineSeries
        {
            ScalesYAt = 3,
            Values = chartPidTimeVelocityI,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };

        static LineSeries _ChartPidTimePositionESeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimePositionE,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimePositionWSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionW,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimePositionYSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionY,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimePositionUSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionU,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimePositionDSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionD,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimePositionISeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionI,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };

        static LineSeries _ChartPidTimeForceESeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceE,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimeForceWSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceW,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimeForceYSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceY,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimeForceUSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceU,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimeForceDSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceD,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidTimeForceISeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceI,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };

        static LineSeries _ChartPidBuffVelocityESeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimeVelocityE,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffVelocityWSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimeVelocityW,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffVelocityYSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimeVelocityY,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffVelocityUSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimeVelocityU,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffVelocityDSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimeVelocityD,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffVelocityISeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimeVelocityI,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffPositionESeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = chartPidTimePositionE,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffPositionWSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionW,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffPositionYSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionY,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffPositionUSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionU,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffPositionDSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionD,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffPositionISeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimePositionI,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffForceESeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceE,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffForceWSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceW,
            Stroke = System.Windows.Media.Brushes.Blue,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffForceYSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceY,
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffForceUSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceU,
            Stroke = System.Windows.Media.Brushes.Green,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffForceDSeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceD,
            Stroke = System.Windows.Media.Brushes.Magenta,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };
        static LineSeries _ChartPidBuffForceISeries = new LineSeries
        {
            ScalesYAt = 0,
            Values = ChartPidTimeForceI,
            Stroke = System.Windows.Media.Brushes.Yellow,
            StrokeThickness = 1,
            PointGeometrySize = 0,
            LineSmoothness = setSmoothness,
            PointGeometry = null,
            Fill = System.Windows.Media.Brushes.Transparent
        };

        public int refreshPointsVelocityChart, countRefreshVelocityChart = 0;

        static public ChartValues<ObservablePoint> CartesianChartVelocityTimeValues { get => _cartesianChartVelocityTimeValues; set => _cartesianChartVelocityTimeValues = value; }
        static public ChartValues<ObservablePoint> CartesianChartPositionTimeValues { get => _cartesianChartPositionTimeValues; set => _cartesianChartPositionTimeValues = value; }
        static public ChartValues<ObservablePoint> CartesianChartStrainTimeValues { get => _cartesianChartStrainTimeValues; set => _cartesianChartStrainTimeValues = value; }
        static public ChartValues<ObservablePoint> ChartVelocityTimeValues { get => _ChartVelocityTimeValues; set => _ChartVelocityTimeValues = value; }
        static public ChartValues<ObservablePoint> ChartPositionTimeValues { get => _ChartPositionTimeValues; set => _ChartPositionTimeValues = value; }
        public static ChartValues<ObservablePoint> ChartStrainTimeValues { get => _ChartStrainTimeValues; set => _ChartStrainTimeValues = value; }
        static public ChartValues<ObservablePoint> CartesianChartVelocityStrainValues { get => _cartesianChartVelocityStrainValues; set => _cartesianChartVelocityStrainValues = value; }
        static public ChartValues<ObservablePoint> CartesianChartPositionStrainValues { get => _cartesianChartPositionStrainValues; set => _cartesianChartPositionStrainValues = value; }
        static public ChartValues<ObservablePoint> ChartVelocityStrainValues { get => _ChartVelocityStrainValues; set => _ChartVelocityStrainValues = value; }
        static public ChartValues<ObservablePoint> ChartPositionStrainValues { get => _ChartPositionStrainValues; set => _ChartPositionStrainValues = value; }
        static public Axis TimeAxis { get => _TimeAxis; set => _TimeAxis = value; }
        static public Axis SpeedAxisX { get => _SpeedAxisX; set => _SpeedAxisX = value; }
        static public Axis SpeedAxisY { get => _SpeedAxisY; set => _SpeedAxisY = value; }
        static public Axis PositionAxisX { get => _PositionAxisX; set => _PositionAxisX = value; }
        static public Axis PositionAxisY { get => _PositionAxisY; set => _PositionAxisY = value; }
        static public Axis StrainAxisY { get => _StrainAxisY; set => _StrainAxisY = value; }
        static public Axis StrainAxisPositionY { get => _StrainAxisPositionY; set => _StrainAxisPositionY = value; }
        static public Axis StrainAxisVelocityY { get => _StrainAxisVelocityY; set => _StrainAxisVelocityY = value; }
        public static LineSeries CartesianChartVelocityTimeSeries { get => _cartesianChartVelocityTimeSeries; set => _cartesianChartVelocityTimeSeries = value; }
        public static LineSeries CartesianChartPositionTimeSeries { get => _cartesianChartPositionTimeSeries; set => _cartesianChartPositionTimeSeries = value; }
        public static LineSeries CartesianChartStrainTimeSeries { get => _cartesianChartStrainTimeSeries; set => _cartesianChartStrainTimeSeries = value; }
        public static LineSeries CartesianChartVelocityStrainSeries { get => _cartesianChartVelocityStrainSeries; set => _cartesianChartVelocityStrainSeries = value; }
        public static LineSeries CartesianChartPositionStrainSeries { get => _cartesianChartPositionStrainSeries; set => _cartesianChartPositionStrainSeries = value; }
        public static LineSeries ChartVelocityTimeSeries { get => _ChartVelocityTimeSeries; set => _ChartVelocityTimeSeries = value; }
        public static LineSeries ChartPositionTimeSeries { get => _ChartPositionTimeSeries; set => _ChartPositionTimeSeries = value; }
        public static LineSeries ChartStrainTimeSeries { get => _ChartStrainTimeSeries; set => _ChartStrainTimeSeries = value; }
        public static LineSeries ChartVelocityStrainSeries { get => _ChartVelocityStrainSeries; set => _ChartVelocityStrainSeries = value; }
        public static LineSeries ChartPositionStrainSeries { get => _ChartPositionStrainSeries; set => _ChartPositionStrainSeries = value; }
        public static ChartValues<ObservablePoint> ChartVelocityStrainValuesLock { get => _ChartVelocityStrainValuesLock; set => _ChartVelocityStrainValuesLock = value; }
        public static ChartValues<ObservablePoint> ChartPositionStrainValuesLock { get => _ChartPositionStrainValuesLock; set => _ChartPositionStrainValuesLock = value; }
        public static ChartValues<ObservablePoint> ChartVelocityTimeValuesLock { get => _ChartVelocityTimeValuesLock; set => _ChartVelocityTimeValuesLock = value; }
        public static ChartValues<ObservablePoint> ChartPositionTimeValuesLock { get => _ChartPositionTimeValuesLock; set => _ChartPositionTimeValuesLock = value; }
        public static ChartValues<ObservablePoint> ChartStrainTimeValuesLock { get => _ChartStrainTimeValuesLock; set => _ChartStrainTimeValuesLock = value; }
        public static ChartValues<ObservablePoint> ChartPositionStrainValuesCopy { get => _ChartPositionStrainValuesCopy; set => _ChartPositionStrainValuesCopy = value; }
        public static ChartValues<ObservablePoint> ChartVelocityStrainValuesCopy { get => _ChartVelocityStrainValuesCopy; set => _ChartVelocityStrainValuesCopy = value; }
        public static ChartValues<ObservablePoint> ChartVelocityTimeValuesCopy { get => _ChartVelocityTimeValuesCopy; set => _ChartVelocityTimeValuesCopy = value; }
        public static ChartValues<ObservablePoint> ChartPositionTimeValuesCopy { get => _ChartPositionTimeValuesCopy; set => _ChartPositionTimeValuesCopy = value; }
        public static ChartValues<ObservablePoint> ChartStrainTimeValuesCopy { get => _ChartStrainTimeValuesCopy; set => _ChartStrainTimeValuesCopy = value; }
        public static ColumnSeries ChartPidVelocityESeries { get => _ChartPidVelocityESeries; set => _ChartPidVelocityESeries = value; }
        public static ColumnSeries ChartPidVelocityWSeries { get => _ChartPidVelocityWSeries; set => _ChartPidVelocityWSeries = value; }
        public static ColumnSeries ChartPidVelocityYSeries { get => _ChartPidVelocityYSeries; set => _ChartPidVelocityYSeries = value; }
        public static ColumnSeries ChartPidVelocityUSeries { get => _ChartPidVelocityUSeries; set => _ChartPidVelocityUSeries = value; }
        public static ColumnSeries ChartPidPositionESeries { get => _ChartPidPositionESeries; set => _ChartPidPositionESeries = value; }
        public static ColumnSeries ChartPidPositionWSeries { get => _ChartPidPositionWSeries; set => _ChartPidPositionWSeries = value; }
        public static ColumnSeries ChartPidForceESeries { get => _ChartPidForceESeries; set => _ChartPidForceESeries = value; }
        public static ColumnSeries ChartPidForceWSeries { get => _ChartPidForceWSeries; set => _ChartPidForceWSeries = value; }
        public static ColumnSeries ChartPidForceUSeries { get => _ChartPidForceUSeries; set => _ChartPidForceUSeries = value; }
        public static LineSeries ChartPidTimeVelocityESeries { get => _ChartPidTimeVelocityESeries; set => _ChartPidTimeVelocityESeries = value; }
        public static LineSeries ChartPidTimeVelocityWSeries { get => _ChartPidTimeVelocityWSeries; set => _ChartPidTimeVelocityWSeries = value; }
        public static LineSeries ChartPidTimeVelocityYSeries { get => _ChartPidTimeVelocityYSeries; set => _ChartPidTimeVelocityYSeries = value; }
        public static LineSeries ChartPidTimeVelocityUSeries { get => _ChartPidTimeVelocityUSeries; set => _ChartPidTimeVelocityUSeries = value; }
        public static LineSeries ChartPidTimePositionESeries { get => _ChartPidTimePositionESeries; set => _ChartPidTimePositionESeries = value; }
        public static LineSeries ChartPidTimePositionWSeries { get => _ChartPidTimePositionWSeries; set => _ChartPidTimePositionWSeries = value; }
        public static LineSeries ChartPidTimePositionYSeries { get => _ChartPidTimePositionYSeries; set => _ChartPidTimePositionYSeries = value; }
        public static LineSeries ChartPidTimePositionUSeries { get => _ChartPidTimePositionUSeries; set => _ChartPidTimePositionUSeries = value; }
        public static LineSeries ChartPidTimeForceESeries { get => _ChartPidTimeForceESeries; set => _ChartPidTimeForceESeries = value; }
        public static LineSeries ChartPidTimeForceYSeries { get => _ChartPidTimeForceYSeries; set => _ChartPidTimeForceYSeries = value; }
        public static LineSeries ChartPidTimeForceUSeries { get => _ChartPidTimeForceUSeries; set => _ChartPidTimeForceUSeries = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeVelocity { get => chartPidTimeVelocity; set => chartPidTimeVelocity = value; }
        public static ChartValues<ObservablePoint> ChartPidTimePosition { get => chartPidTimePosition; set => chartPidTimePosition = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeForce { get => chartPidTimeForce; set => chartPidTimeForce = value; }
        public static ChartValues<double> ChartPidVelocity { get => chartPidVelocity; set => chartPidVelocity = value; }
        public static ChartValues<double> ChartPidPosition { get => chartPidPosition; set => chartPidPosition = value; }
        public static ChartValues<double> ChartPidForce { get => chartPidForce; set => chartPidForce = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeVelocityW { get => chartPidTimeVelocityW; set => chartPidTimeVelocityW = value; }
        public static ChartValues<ObservablePoint> ChartPidTimePositionW { get => chartPidTimePositionW; set => chartPidTimePositionW = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeForceW { get => chartPidTimeForceW; set => chartPidTimeForceW = value; }
        public static ChartValues<double> ChartPidVelocityW { get => chartPidVelocityW; set => chartPidVelocityW = value; }
        public static ChartValues<double> ChartPidPositionW { get => chartPidPositionW; set => chartPidPositionW = value; }
        public static ChartValues<double> ChartPidForceW { get => chartPidForceW; set => chartPidForceW = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeVelocityY { get => chartPidTimeVelocityY; set => chartPidTimeVelocityY = value; }
        public static ChartValues<ObservablePoint> ChartPidTimePositionY { get => chartPidTimePositionY; set => chartPidTimePositionY = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeForceY { get => chartPidTimeForceY; set => chartPidTimeForceY = value; }
        public static ChartValues<double> ChartPidVelocityY { get => chartPidVelocityY; set => chartPidVelocityY = value; }
        public static ChartValues<double> ChartPidPositionY { get => chartPidPositionY; set => chartPidPositionY = value; }
        public static ChartValues<double> ChartPidForceY { get => chartPidForceY; set => chartPidForceY = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeVelocityE { get => chartPidTimeVelocityE; set => chartPidTimeVelocityE = value; }
        public static ChartValues<ObservablePoint> ChartPidTimePositionE { get => chartPidTimePositionE; set => chartPidTimePositionE = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeForceE { get => chartPidTimeForceE; set => chartPidTimeForceE = value; }
        public static ChartValues<double> ChartPidVelocityE { get => chartPidVelocityE; set => chartPidVelocityE = value; }
        public static ChartValues<double> ChartPidPositionE { get => chartPidPositionE; set => chartPidPositionE = value; }
        public static ChartValues<double> ChartPidForceE { get => chartPidForceE; set => chartPidForceE = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeVelocityI { get => chartPidTimeVelocityI; set => chartPidTimeVelocityI = value; }
        public static ChartValues<ObservablePoint> ChartPidTimePositionI { get => chartPidTimePositionI; set => chartPidTimePositionI = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeForceI { get => chartPidTimeForceI; set => chartPidTimeForceI = value; }
        public static ChartValues<double> ChartPidVelocityI { get => chartPidVelocityI; set => chartPidVelocityI = value; }
        public static ChartValues<double> ChartPidPositionI { get => chartPidPositionI; set => chartPidPositionI = value; }
        public static ChartValues<double> ChartPidForceI { get => chartPidForceI; set => chartPidForceI = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeVelocityD { get => chartPidTimeVelocityD; set => chartPidTimeVelocityD = value; }
        public static ChartValues<ObservablePoint> ChartPidTimePositionD { get => chartPidTimePositionD; set => chartPidTimePositionD = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeForceD { get => chartPidTimeForceD; set => chartPidTimeForceD = value; }
        public static ChartValues<double> ChartPidVelocityD { get => chartPidVelocityD; set => chartPidVelocityD = value; }
        public static ChartValues<double> ChartPidPositionD { get => chartPidPositionD; set => chartPidPositionD = value; }
        public static ChartValues<double> ChartPidForceD { get => chartPidForceD; set => chartPidForceD = value; }
        public static Axis PidX { get => _PidX; set => _PidX = value; }
        public static Axis PidU { get => _PidU; set => _PidU = value; }
        public static Axis PidW { get => _PidW; set => _PidW = value; }
        public static Axis PidY { get => _PidY; set => _PidY = value; }
        public static Axis PidE { get => _PidE; set => _PidE = value; }
        public static Axis PidI { get => _PidI; set => _PidI = value; }
        public static Axis PidD { get => _PidD; set => _PidD = value; }
        public static LineSeries ChartPidTimeForceISeries { get => _ChartPidTimeForceISeries; set => _ChartPidTimeForceISeries = value; }
        public static LineSeries ChartPidTimeForceDSeries { get => _ChartPidTimeForceDSeries; set => _ChartPidTimeForceDSeries = value; }
        public static LineSeries ChartPidTimeForceWSeries { get => _ChartPidTimeForceWSeries; set => _ChartPidTimeForceWSeries = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeVelocityU { get => chartPidTimeVelocityU; set => chartPidTimeVelocityU = value; }
        public static ChartValues<ObservablePoint> ChartPidTimePositionU { get => chartPidTimePositionU; set => chartPidTimePositionU = value; }
        public static ChartValues<ObservablePoint> ChartPidTimeForceU { get => chartPidTimeForceU; set => chartPidTimeForceU = value; }
        public static LineSeries ChartPidTimeVelocityDSeries { get => _ChartPidTimeVelocityDSeries; set => _ChartPidTimeVelocityDSeries = value; }
        public static LineSeries ChartPidTimeVelocityISeries { get => _ChartPidTimeVelocityISeries; set => _ChartPidTimeVelocityISeries = value; }
        public static LineSeries ChartPidTimePositionISeries { get => _ChartPidTimePositionISeries; set => _ChartPidTimePositionISeries = value; }
        public static LineSeries ChartPidTimePositionDSeries { get => _ChartPidTimePositionDSeries; set => _ChartPidTimePositionDSeries = value; }
        public static ColumnSeries ChartPidForceDSeries { get => _ChartPidForceDSeries; set => _ChartPidForceDSeries = value; }
        public static ColumnSeries ChartPidForceISeries { get => _ChartPidForceISeries; set => _ChartPidForceISeries = value; }
        public static ColumnSeries ChartPidForceYSeries { get => _ChartPidForceYSeries; set => _ChartPidForceYSeries = value; }
        public static LineSeries ChartPidBuffVelocityESeries { get => _ChartPidBuffVelocityESeries; set => _ChartPidBuffVelocityESeries = value; }
        public static LineSeries ChartPidBuffVelocityWSeries { get => _ChartPidBuffVelocityWSeries; set => _ChartPidBuffVelocityWSeries = value; }
        public static LineSeries ChartPidBuffVelocityYSeries { get => _ChartPidBuffVelocityYSeries; set => _ChartPidBuffVelocityYSeries = value; }
        public static LineSeries ChartPidBuffVelocityUSeries { get => _ChartPidBuffVelocityUSeries; set => _ChartPidBuffVelocityUSeries = value; }
        public static LineSeries ChartPidBuffVelocityDSeries { get => _ChartPidBuffVelocityDSeries; set => _ChartPidBuffVelocityDSeries = value; }
        public static LineSeries ChartPidBuffVelocityISeries { get => _ChartPidBuffVelocityISeries; set => _ChartPidBuffVelocityISeries = value; }
        public static LineSeries ChartPidBuffPositionESeries { get => _ChartPidBuffPositionESeries; set => _ChartPidBuffPositionESeries = value; }
        public static LineSeries ChartPidBuffPositionWSeries { get => _ChartPidBuffPositionWSeries; set => _ChartPidBuffPositionWSeries = value; }
        public static LineSeries ChartPidBuffPositionYSeries { get => _ChartPidBuffPositionYSeries; set => _ChartPidBuffPositionYSeries = value; }
        public static LineSeries ChartPidBuffPositionUSeries { get => _ChartPidBuffPositionUSeries; set => _ChartPidBuffPositionUSeries = value; }
        public static LineSeries ChartPidBuffPositionDSeries { get => _ChartPidBuffPositionDSeries; set => _ChartPidBuffPositionDSeries = value; }
        public static LineSeries ChartPidBuffPositionISeries { get => _ChartPidBuffPositionISeries; set => _ChartPidBuffPositionISeries = value; }
        public static LineSeries ChartPidBuffForceESeries { get => _ChartPidBuffForceESeries; set => _ChartPidBuffForceESeries = value; }
        public static LineSeries ChartPidBuffForceWSeries { get => _ChartPidBuffForceWSeries; set => _ChartPidBuffForceWSeries = value; }
        public static LineSeries ChartPidBuffForceYSeries { get => _ChartPidBuffForceYSeries; set => _ChartPidBuffForceYSeries = value; }
        public static LineSeries ChartPidBuffForceUSeries { get => _ChartPidBuffForceUSeries; set => _ChartPidBuffForceUSeries = value; }
        public static LineSeries ChartPidBuffForceDSeries { get => _ChartPidBuffForceDSeries; set => _ChartPidBuffForceDSeries = value; }
        public static LineSeries ChartPidBuffForceISeries { get => _ChartPidBuffForceISeries; set => _ChartPidBuffForceISeries = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffVelocity { get => chartPidBuffVelocity; set => chartPidBuffVelocity = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffPosition { get => chartPidBuffPosition; set => chartPidBuffPosition = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffForce { get => chartPidBuffForce; set => chartPidBuffForce = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffVelocityW { get => chartPidBuffVelocityW; set => chartPidBuffVelocityW = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffPositionW { get => chartPidBuffPositionW; set => chartPidBuffPositionW = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffForceW { get => chartPidBuffForceW; set => chartPidBuffForceW = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffVelocityY { get => chartPidBuffVelocityY; set => chartPidBuffVelocityY = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffPositionY { get => chartPidBuffPositionY; set => chartPidBuffPositionY = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffForceY { get => chartPidBuffForceY; set => chartPidBuffForceY = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffVelocityE { get => chartPidBuffVelocityE; set => chartPidBuffVelocityE = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffPositionE { get => chartPidBuffPositionE; set => chartPidBuffPositionE = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffForceE { get => chartPidBuffForceE; set => chartPidBuffForceE = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffVelocityI { get => chartPidBuffVelocityI; set => chartPidBuffVelocityI = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffPositionI { get => chartPidBuffPositionI; set => chartPidBuffPositionI = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffForceI { get => chartPidBuffForceI; set => chartPidBuffForceI = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffVelocityD { get => chartPidBuffVelocityD; set => chartPidBuffVelocityD = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffPositionD { get => chartPidBuffPositionD; set => chartPidBuffPositionD = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffForceD { get => chartPidBuffForceD; set => chartPidBuffForceD = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffVelocityU { get => chartPidBuffVelocityU; set => chartPidBuffVelocityU = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffPositionU { get => chartPidBuffPositionU; set => chartPidBuffPositionU = value; }
        public static ChartValues<ObservablePoint> ChartPidBuffForceU { get => chartPidBuffForceU; set => chartPidBuffForceU = value; }


        #endregion

        public static void WriteValuesToChart(string time, string vel, ChartValues<ObservablePoint> ChartValues, ChartValues<ObservablePoint> ChartValuesLock, ref bool lck, ref double lastTime, ref double lastVal)
        {
            //if (cartesianChartVelocityForce.InvokeRequired)
            //    cartesianChartVelocityForce.BeginInvoke(new writeValuesToChartEventHandler(WriteValuesToChart), time, vel, ChartValues);
            double val, valTick;
            if (!ChartsData.checkBoxDisplayValueChecked)
            {
                return;
            }

            bool parsedVel = double.TryParse(vel, out val);
            bool parsedTime = double.TryParse(time, out valTick);

            try
            {

                //if (ChartValues.Count != 0)
                //{
                if (val == lastVal && valTick == lastTime || !parsedTime || !parsedVel)
                {

                    return;
                }

                ObservablePoint oPoint = new ObservablePoint(valTick, val);

                if (lck)
                {
                    ChartValuesLock.Add(oPoint);
                }
                else
                {
                    lck = true;

                    if (ChartValuesLock.Count > 0)
                    {
                        ChartValues.AddRange(ChartValuesLock);
                        ChartValuesLock.Clear();
                    }
                    ChartValues.Add(oPoint);
                    lck = false;
                }

                lastVal = val; lastTime = valTick;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                endOfSubstription = true;
            }

        }


    }
#endif

}
