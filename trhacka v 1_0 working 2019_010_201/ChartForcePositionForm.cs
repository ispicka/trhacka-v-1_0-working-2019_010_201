using CsvHelper;
using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace trhacka_v_1_0_working_2019_010_201
{
    public partial class ChartForcePositionForm : Form
    {

        private UAClientForm mainForm = null;
        bool timerDiv = false;
        bool buttonWriteDataState = false;
        public ChartForcePositionForm()
        {
            InitializeComponent();
        }
        public ChartForcePositionForm(Form callingForm)
        {
            InitializeComponent();
            mainForm = callingForm as UAClientForm;
        }



        private void ChartForcePositionForm_Load(object sender, EventArgs e)
        {
            //cartesianChartPositionForce.AxisX.Clear();
            //cartesianChartPositionForce.AxisY.Clear();
            //cartesianChartPositionForce.AxisX.Add(ChartsData.PositionAxisX);
            //cartesianChartPositionForce.AxisY.Add(ChartsData.StrainAxisPositionY);
            //cartesianChartPositionForce.Series.Add(ChartsData.CartesianChartPositionStrainSeries);

            //cartesianChartPositionForce.DisableAnimations = true;
            //cartesianChartPositionForce.AnimationsSpeed = new System.TimeSpan(0, 0, 0, 0, 1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            SetText(textBoxPoint, ChartsData.ChartPositionStrainValues.Count.ToString());

#if liveCharts
            if (timerDiv)
            {


                ChartsData.lockPositionStrain = true;

                //cartesianChartPositionForce.UpdaterState = UpdaterState.Running;
                //cartesianChartPositionForce.Update();


                ChartsData.lockPositionStrain = false;
            }
            else
            {


                ChartsData.WriteValuesToChart(ChartsData.textBoxPosition.Text, ChartsData.textBoxStrain.Text, ChartsData.ChartPositionStrainValues, ChartsData.ChartPositionStrainValuesLock,
                    ref ChartsData.lockPositionStrain, ref ChartsData.lastValuePositionStrainPosition, ref ChartsData.lastValuePositionStrainStrain);
                //lockPositionTime = false;
                // while (lockVelocityTime) ;
                //lockVelocityTime = true;

                ChartsData.lockPositionStrain = true;
                cartesianChartPositionForce.UpdaterState = UpdaterState.Paused;
                if (ChartsData.ChartPositionStrainValuesCopy.Count > 0)
                    ChartsData.ChartPositionStrainValuesCopy.Clear();
                if (ChartsData.ChartPositionStrainValues.Count > 0)
                {
                    ChartsData.ChartPositionStrainValuesCopy.AddRange(ChartsData.ChartVelocityStrainValues);
                    ChartsData.CartesianChartPositionStrainValues.AddRange(ChartsData.ChartVelocityStrainValuesCopy);
                    ChartsData.ChartPositionStrainValues.Clear();
                }
                //cartesianChartPositionForce.UpdaterState = UpdaterState.Running;

                ChartsData.lockPositionStrain = false;

            }
            timerDiv = !timerDiv;
#else
                ScottPlot();
#endif

        }
        private void ScottPlot()

        {
            bool plotPositionForce = ChartsData.ChartPositionStrainValues.Count > 0;
            if (!plotPositionForce) return;
            double[][] scottPlotPosition = new double[2][];
            scottPlotPosition[0] = new double[ChartsData.ChartPositionStrainValues.Count];
            scottPlotPosition[1] = new double[ChartsData.ChartPositionStrainValues.Count];
            formsPlotPositionForce.plt.Clear();
            plotDraw(plotPositionForce, scottPlotPosition, ChartsData.ChartPositionStrainValues);
            formsPlotPositionForce.Render();
        }
        private void plotDraw(bool plot, double[][] scottPlot, ChartValues<ObservablePoint> observablePoints)
        {

            int index = 0;
            if (observablePoints.Count != 0)
            {

                foreach (ObservablePoint observablePoint in observablePoints)
                {
                    scottPlot[0][index] = observablePoint.X;
                    scottPlot[1][index++] = observablePoint.Y;
                }
                if (plot)
                {
                    formsPlotPositionForce.plt.AxisAuto();
                    formsPlotPositionForce.plt.PlotScatter(scottPlot[0], scottPlot[1], markerSize: 0);
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

        }

        private void buttonWriteData_Click_1(object sender, EventArgs e)
        {
            buttonWriteDataState = !buttonWriteDataState;

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
            List<DataRecordForcePosition> datas = new List<DataRecordForcePosition>();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.FileName);
                CsvWriter writerCSV = new CsvWriter(writer);
                int lengthMax = ChartsData.CartesianChartPositionStrainValues.Count;
                for (int i = 0; i < lengthMax; i++)
                {
                    DataRecordForcePosition data = new DataRecordForcePosition();
                    data.ValueForce = (int)ChartsData.CartesianChartPositionStrainValues[i].Y;
                    data.ValuePosition = ChartsData.CartesianChartPositionStrainValues[i].X;
                    datas.Add(data);
                }
                writerCSV.WriteRecords(datas);
                writer.Close();
            }
        }

        private void ButtonClearChart_Click(object sender, EventArgs e)
        {
            ChartsData.CartesianChartPositionStrainValues.Clear();
        }

        private void NumericUpDownTimeToWriteToChart_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = (int)numericUpDownTimeToWriteToChart.Value / 2;
        }
    }
}
