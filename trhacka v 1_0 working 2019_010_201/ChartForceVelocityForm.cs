using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Defaults;
using CsvHelper;

namespace trhacka_v_1_0_working_2019_010_201
{
    public partial class ChartForceVelocityForm : Form
    {
        private UAClientForm mainForm = null;
        bool timerDiv = false;
        bool buttonWriteDataState = false;
        public ChartForceVelocityForm()
        {
            InitializeComponent();
        }
        public ChartForceVelocityForm(Form callingForm)
        {
            InitializeComponent();
            mainForm = callingForm as UAClientForm;
        }

        private void ChartForceVelocityForm_Load(object sender, EventArgs e)
        {
            //cartesianChartVelocityForce.AxisX.Add(ChartsData.SpeedAxisX);
            //cartesianChartVelocityForce.AxisY.Add(ChartsData.StrainAxisVelocityY);
            //cartesianChartVelocityForce.Series.Add(ChartsData.CartesianChartVelocityStrainSeries);
            //cartesianChartVelocityForce.DisableAnimations = true;
            //cartesianChartVelocityForce.AnimationsSpeed = new System.TimeSpan(0, 0, 0, 0, 1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            SetText(textBoxPoint, ChartsData.CartesianChartVelocityStrainValues.Count.ToString());


            if (timerDiv)
            {


                ChartsData.lockVelocityStrain = true;

                //cartesianChartVelocityForce.UpdaterState = UpdaterState.Running;
                //cartesianChartVelocityForce.Update();


                ChartsData.lockVelocityStrain = false;
            }
            else
            {


                ChartsData.WriteValuesToChart(ChartsData.textBoxVelocity.Text, ChartsData.textBoxStrain.Text, ChartsData.ChartVelocityStrainValues, ChartsData.ChartVelocityStrainValuesLock,
                    ref ChartsData.lockVelocityStrain, ref ChartsData.lastValueVelocityStrainVelocity, ref ChartsData.lastValueVelocityStrainStrain);
                //lockPositionTime = false;
                // while (lockVelocityTime) ;
                //lockVelocityTime = true;

                ChartsData.lockVelocityStrain = true;
                cartesianChartVelocityForce.UpdaterState = UpdaterState.Paused;
                if (ChartsData.ChartVelocityStrainValuesCopy.Count > 0)
                    ChartsData.ChartVelocityStrainValuesCopy.Clear();
                if (ChartsData.ChartVelocityStrainValues.Count > 0)
                {
                    ChartsData.ChartVelocityStrainValuesCopy.AddRange(ChartsData.ChartVelocityStrainValues);
                    ChartsData.CartesianChartVelocityStrainValues.AddRange(ChartsData.ChartVelocityStrainValuesCopy);
                    ChartsData.ChartVelocityStrainValues.Clear();
                }
                //cartesianChartVelocityForce.UpdaterState = UpdaterState.Running;

                ChartsData.lockVelocityStrain = false;
                ScottPlot();
            }
            timerDiv = !timerDiv;

        }
        private void ScottPlot()

        {
            bool plotVelocityForce = ChartsData.CartesianChartVelocityStrainValues.Count > 1;
            if (!plotVelocityForce) return;
            double[][] scottPlotData = new double[2][];
            scottPlotData[0] = new double[ChartsData.CartesianChartVelocityStrainValues.Count];
            scottPlotData[1] = new double[ChartsData.CartesianChartVelocityStrainValues.Count];
            formsPlotVelocityForce.plt.Clear();
            plotDraw(plotVelocityForce, scottPlotData, ChartsData.CartesianChartVelocityStrainValues);
            formsPlotVelocityForce.Render();
        }
        private void plotDraw(bool plot, double[][] scottPlot, ChartValues<ObservablePoint> observablePoints)
        {
            int index = 0;
            double startTime = ChartsData.CartesianChartVelocityStrainValues[0].X;
            foreach (ObservablePoint observablePoint in observablePoints)
            {
                scottPlot[0][index] = observablePoint.X - startTime;
                scottPlot[1][index++] = observablePoint.Y;
            }
            if (plot)
            {
                formsPlotVelocityForce.plt.AxisAuto();
                formsPlotVelocityForce.plt.PlotScatter(scottPlot[0], scottPlot[1], markerSize: 0);
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
            List<DataRecordForceVelocity> datas = new List<DataRecordForceVelocity>();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.FileName);
                CsvWriter writerCSV = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);
                int lengthMax = ChartsData.CartesianChartVelocityStrainValues.Count;
                for (int i = 0; i < lengthMax; i++)
                {
                    DataRecordForceVelocity data = new DataRecordForceVelocity();
                    data.ValueForce = (int)ChartsData.CartesianChartVelocityStrainValues[i].Y;
                    data.ValueVelocity = ChartsData.CartesianChartVelocityStrainValues[i].X;
                    datas.Add(data);
                }
                writerCSV.WriteRecords(datas);
                writer.Close();
            }
        }

        private void ButtonClearChart_Click(object sender, EventArgs e)
        {
            ChartsData.ChartVelocityStrainValues.Clear();
            formsPlotVelocityForce.plt.Clear();
        }

        private void NumericUpDownTimeToWriteToChart_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = (int)numericUpDownTimeToWriteToChart.Value / 2;
        }
    }
}
