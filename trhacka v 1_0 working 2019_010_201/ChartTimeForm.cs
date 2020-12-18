using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Defaults;
using CsvHelper;

namespace trhacka_v_1_0_working_2019_010_201
{
    public partial class ChartTimeForm : Form
    {
        private UAClientForm mainForm = null;
        bool timerDiv = false;
        bool buttonWriteDataState = false;
        public ChartTimeForm()
        {
            InitializeComponent();
        }

        public ChartTimeForm(Form callingForm)
        {
            mainForm = callingForm as UAClientForm;
            InitializeComponent();
            //ResetForm();
        }

        private void ChartTimeForm_Load(object sender, EventArgs e)
        {
            //cartesianChartAllTime.AxisX.Clear();
            //cartesianChartAllTime.AxisY.Clear();
            //cartesianChartAllTime.AxisX.Add(ChartsData.TimeAxis);
            //cartesianChartAllTime.AxisY.Add(ChartsData.SpeedAxisY);
            //cartesianChartAllTime.AxisY.Add(ChartsData.PositionAxisY);
            //cartesianChartAllTime.AxisY.Add(ChartsData.StrainAxisY);
            //cartesianChartAllTime.Series.Add(ChartsData.CartesianChartVelocityTimeSeries);
            //cartesianChartAllTime.Series.Add(ChartsData.CartesianChartPositionTimeSeries);
            //cartesianChartAllTime.Series.Add(ChartsData.CartesianChartStrainTimeSeries);
            //cartesianChartAllTime.DisableAnimations = true;
            //cartesianChartAllTime.DataTooltip = null;
            //cartesianChartAllTime.Hoverable = false;
            //cartesianChartAllTime.AnimationsSpeed = new System.TimeSpan(0, 0, 0, 0, 1);
            //timer1.Enabled = true;

        }

        private void numericUpDownTimeToWriteToChart_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = (int)numericUpDownTimeToWriteToChart.Value / 2;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            SetText(textBoxPointPozition, ChartsData.ChartPositionTimeValues.Count.ToString());
            SetText(textBoxPointsSpeed, ChartsData.ChartVelocityTimeValues.Count.ToString());
            SetText(textBoxPointsForce, ChartsData.ChartStrainTimeValues.Count.ToString());
#if liveCharts
            if (timerDiv)
            {

                ChartsData.lockPositionStrain = ChartsData.lockPositionTime = ChartsData.lockStrainTime = ChartsData.lockVelocityStrain = ChartsData.lockVelocityTime = true;
                //cartesianChartAllTime.UpdaterState = UpdaterState.Running;
                cartesianChartAllTime.Update();

                ChartsData.lockPositionTime = ChartsData.lockStrainTime = ChartsData.lockVelocityTime = false;
            }
            else
            {

                ChartsData.WriteValuesToChart(ChartsData.textBoxPositionTime.Text, ChartsData.textBoxPosition.Text, ChartsData.ChartPositionTimeValues, ChartsData.ChartPositionTimeValuesLock,
                    ref ChartsData.lockPositionTime, ref ChartsData.lastTimePosition, ref ChartsData.lastValuePosition);
                //lockPositionTime = false;
                // while (lockVelocityTime) ;
                //lockVelocityTime = true;
                ChartsData.WriteValuesToChart(ChartsData.textBoxVelocityTime.Text, ChartsData.textBoxVelocity.Text, ChartsData.ChartVelocityTimeValues, ChartsData.ChartVelocityTimeValuesLock,
                    ref ChartsData.lockVelocityTime, ref ChartsData.lastTimeVelocity, ref ChartsData.lastValueVelocity);
                //lockVelocityTime = false;
                //while (lockStrainTime) ;
                //lockStrainTime = true;
                ChartsData.WriteValuesToChart(ChartsData.textBoxStrainTime.Text, ChartsData.textBoxStrain.Text, ChartsData.ChartStrainTimeValues, ChartsData.ChartStrainTimeValuesLock,
                    ref ChartsData.lockStrainTime, ref ChartsData.lastTimeStrain, ref ChartsData.lastValueStrain);
                //lockStrainTime = false;
                ChartsData.lockPositionTime = true;
                if (ChartsData.ChartPositionTimeValuesCopy.Count > 0)
                    ChartsData.ChartPositionTimeValuesCopy.Clear();
                cartesianChartAllTime.UpdaterState = UpdaterState.Paused;
                if (ChartsData.ChartPositionTimeValues.Count > 0)
                {
                    ChartsData.ChartPositionTimeValuesCopy.AddRange(ChartsData.ChartPositionTimeValues);
                    ChartsData.CartesianChartPositionTimeValues.AddRange(ChartsData.ChartPositionTimeValuesCopy);
                    ChartsData.ChartPositionTimeValues.Clear();
                }
                //cartesianChartAllTime.UpdaterState = UpdaterState.Running;
                ChartsData.lockPositionTime = false;
                ChartsData.lockVelocityTime = true;
                if (ChartsData.ChartVelocityTimeValuesCopy.Count > 0)
                    ChartsData.ChartVelocityTimeValuesCopy.Clear();
                if (ChartsData.ChartVelocityTimeValues.Count > 0)
                {
                    ChartsData.ChartVelocityTimeValuesCopy.AddRange(ChartsData.ChartVelocityTimeValues);
                    ChartsData.CartesianChartVelocityTimeValues.AddRange(ChartsData.ChartVelocityTimeValuesCopy);
                    ChartsData.ChartVelocityTimeValues.Clear();
                }

                ChartsData.lockVelocityTime = false;
                ChartsData.lockStrainTime = true;
                if (ChartsData.ChartStrainTimeValuesCopy.Count > 0)
                    ChartsData.ChartStrainTimeValuesCopy.Clear();
                if (ChartsData.ChartStrainTimeValues.Count > 0)
                {
                    ChartsData.ChartStrainTimeValuesCopy.AddRange(ChartsData.ChartStrainTimeValues);
                    ChartsData.CartesianChartStrainTimeValues.AddRange(ChartsData.ChartStrainTimeValuesCopy);
                    ChartsData.ChartStrainTimeValues.Clear();
                }
                ChartsData.lockStrainTime = false;
                cartesianChartAllTime.UpdaterState = UpdaterState.Paused;



            }
            timerDiv = !timerDiv;
#else
            ScottPlot();
#endif

        }
        private void ScottPlot()

        {
            bool plotPosition = ChartsData.ChartPositionTimeValues.Count > 0;
            double[][] scottPlotPosition = new double[2][];
            scottPlotPosition[0] = new double[ChartsData.ChartPositionTimeValues.Count];
            scottPlotPosition[1] = new double[ChartsData.ChartPositionTimeValues.Count];

            bool plotStrain = ChartsData.ChartStrainTimeValues.Count > 0;
            double[][] scottPlotStrain = new double[2][];
            scottPlotStrain[0] = new double[ChartsData.ChartStrainTimeValues.Count];
            scottPlotStrain[1] = new double[ChartsData.ChartStrainTimeValues.Count];

            bool plotVelocity = ChartsData.ChartVelocityTimeValues.Count > 0;
            double[][] scottPlotVelocity = new double[2][];
            scottPlotVelocity[0] = new double[ChartsData.ChartVelocityTimeValues.Count];
            scottPlotVelocity[1] = new double[ChartsData.ChartVelocityTimeValues.Count];
            formsPlotTimeSeries.plt.Clear();
            plotDraw(plotPosition, scottPlotPosition, ChartsData.ChartPositionTimeValues);
            plotDraw(plotStrain, scottPlotStrain, ChartsData.ChartStrainTimeValues);
            plotDraw(plotVelocity, scottPlotVelocity, ChartsData.ChartVelocityTimeValues);

            formsPlotTimeSeries.Render();
        }

        private void plotDraw(bool plot, double[][] scottPlot, ChartValues<ObservablePoint> observablePoints)
        {
            int index = 0;
            if (ChartsData.ChartPositionTimeValues.Count != 0)
            {
                double startTime = ChartsData.ChartPositionTimeValues[0].X;
                foreach (ObservablePoint observablePoint in observablePoints)
                {
                    scottPlot[0][index] = observablePoint.X - startTime;
                    scottPlot[1][index++] = observablePoint.Y;
                }
                if (plot)
                {
                    formsPlotTimeSeries.plt.AxisAuto();
                    formsPlotTimeSeries.plt.PlotScatter(scottPlot[0], scottPlot[1], markerSize: 0);
                }
            }

        }

        private void SetText(TextBox txt, string text)
        {
            if (txt.InvokeRequired)
            {
                Invoke((MethodInvoker)(() => txt.Text = text));
            }
            else
            {
                txt.Text = text;
            }
        }

        private void buttonWriteData_Click(object sender, EventArgs e)
        {
            ChartsData.checkBoxDisplayValueChecked = buttonWriteDataState = !buttonWriteDataState;

            if (buttonWriteDataState)
            {
                buttonWriteData.BackColor = Color.Red;
                timer1.Start();
            }
            else
            {
                buttonWriteData.BackColor = SystemColors.Control;
                timer1.Stop();
            }

        }

        private void buttonSaveData_Click(object sender, EventArgs e)
        {

            DataRecordAllTimeMap header = new DataRecordAllTimeMap();

            List<DataRecordAllTime> datas = new List<DataRecordAllTime>();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.FileName);
                CsvWriter writerCSV = new CsvWriter(writer);
                //writerCSV.WriteHeader<DataRecordAllTimeMap>();
                //writerCSV.NextRecord();
                int lengthPosition = ChartsData.CartesianChartPositionTimeValues.Count;
                int lengthForce = ChartsData.CartesianChartVelocityStrainValues.Count;
                int lengthVelocity = ChartsData.CartesianChartVelocityTimeValues.Count;
                int lengthMax = Math.Max(Math.Max(lengthPosition, lengthForce), lengthVelocity);
                for (int i = 0; i < lengthMax; i++)
                {
                    DataRecordAllTime data = new DataRecordAllTime();
                    data.ValueTemperature = double.NaN;
                    data.TimeTemperature = -1;
                    if (lengthPosition > i)
                    {
                        data.TimePosition = (int)ChartsData.CartesianChartPositionTimeValues[i].X;
                        data.ValuePosition = ChartsData.CartesianChartPositionTimeValues[i].Y;
                    }
                    else
                    {
                        data.TimePosition = -1;
                        data.ValuePosition = double.NaN;
                    }
                    if (lengthVelocity > i)
                    {
                        data.TimeVelocity = (int)ChartsData.CartesianChartVelocityTimeValues[i].X;
                        data.ValueVelocity = ChartsData.CartesianChartVelocityTimeValues[i].Y;
                    }
                    else
                    {
                        data.TimeVelocity = -1;
                        data.ValueVelocity = double.NaN;
                    }
                    if (lengthForce > i)
                    {
                        data.TimeForce = (int)ChartsData.CartesianChartStrainTimeValues[i].X;
                        data.ValueForce = ChartsData.CartesianChartStrainTimeValues[i].Y;
                    }
                    else
                    {
                        data.TimeForce = -1;
                        data.ValueForce = double.NaN;
                    }
                    datas.Add(data);





                }
                writerCSV.WriteRecords(datas);
                writer.Close();




            }
        }

        private void buttonClearChart_Click(object sender, EventArgs e)
        {
            ChartsData.CartesianChartStrainTimeValues.Clear();
            ChartsData.CartesianChartVelocityTimeValues.Clear();
            ChartsData.CartesianChartPositionTimeValues.Clear();


        }
    }
}
