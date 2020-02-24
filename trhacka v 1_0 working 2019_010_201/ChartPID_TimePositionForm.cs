using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Opc.Ua;
using Opc.Ua.Client;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Siemens.UAClientHelper;
using System.Diagnostics;

namespace trhacka_v_1_0_working_2019_010_201
{


    public partial class ChartPID_TimePositionForm : Form
    {

        #region Fields
        private UAClientHelperAPI myClientHelperAPI = new UAClientHelperAPI();

        private UAClientForm mainForm = null;
        private Subscription mySubscription;
        private MonitoredItem myMonitoredItem;
#pragma warning disable CS0169 // The field 'ChartPID_TimePositionForm.myMonitoredItems' is never used
        private List<MonitoredItem> myMonitoredItems;
#pragma warning restore CS0169 // The field 'ChartPID_TimePositionForm.myMonitoredItems' is never used
#pragma warning disable CS0169 // The field 'ChartPID_TimePositionForm.actTime' is never used
#pragma warning disable CS0169 // The field 'ChartPID_TimePositionForm.startTime' is never used
        private System.DateTime startTime, actTime;
#pragma warning restore CS0169 // The field 'ChartPID_TimePositionForm.startTime' is never used
#pragma warning restore CS0169 // The field 'ChartPID_TimePositionForm.actTime' is never used
        private double diffTime;
        Stopwatch stopWatch = new Stopwatch();

        private DataPID dataPID;
        private string vTrue = "True";
#pragma warning disable CS0414 // The field 'ChartPID_TimePositionForm.vFalse' is assigned but its value is never used
        private string vFalse = "False";
#pragma warning restore CS0414 // The field 'ChartPID_TimePositionForm.vFalse' is assigned but its value is never used
        private static Func<double, string> formatFunc = (x) => string.Format("{0:0.00}", x);
        private Axis _PidX = new Axis
        {
            Title = "Time [s]",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Black,
            LabelFormatter = formatFunc
        };
        private Axis _PidU = new Axis
        {
            Title = "U",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Green,
            LabelFormatter = formatFunc

        };
        private Axis _PidW = new Axis
        {
            Title = "W",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Blue,
            LabelFormatter = formatFunc

        };
        private Axis _PidY = new Axis
        {
            Title = "Y",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Red,
            LabelFormatter = formatFunc

        };
        private Axis _PidE = new Axis
        {
            Title = "E",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Black,
            LabelFormatter = formatFunc

        };
        private Axis _PidI = new Axis
        {
            Title = "I",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Yellow,
            LabelFormatter = formatFunc

        };
        private Axis _PidD = new Axis
        {
            Title = "D",
            Position = AxisPosition.LeftBottom,
            Foreground = System.Windows.Media.Brushes.Magenta,
            LabelFormatter = formatFunc

        };

        public Axis PidD { get => _PidD; set => _PidD = value; }
        public Axis PidI { get => _PidI; set => _PidI = value; }
        public Axis PidE { get => _PidE; set => _PidE = value; }
        public Axis PidY { get => _PidY; set => _PidY = value; }
        public Axis PidW { get => _PidW; set => _PidW = value; }
        public Axis PidU { get => _PidU; set => _PidU = value; }
        public Axis PidX { get => _PidX; set => _PidX = value; }

        #endregion Fields

        public void ResetForm()
        {

            stopWatch.Start();
            ObservablePoint oP = new ObservablePoint();
            oP.X = 0;
            oP.Y = 0;
            ChartsData.ChartPidTimePositionWSeries.Values.Add(oP);
            ChartsData.ChartPidTimePositionYSeries.Values.Add(oP);
            ChartsData.ChartPidTimePositionUSeries.Values.Add(oP);
            ChartsData.ChartPidTimePositionISeries.Values.Add(oP);
            ChartsData.ChartPidTimePositionDSeries.Values.Add(oP);
            cartesianChartPositionPidTime.AxisX.Clear();
            cartesianChartPositionPidTime.AxisY.Clear();
            cartesianChartPositionPidTime.AxisX.Add(PidX);
            cartesianChartPositionPidTime.AxisY.Add(PidW);  //axis 0
            cartesianChartPositionPidTime.AxisY.Add(PidY);  //axis 1
            cartesianChartPositionPidTime.AxisY.Add(PidU);  //axis 2
            cartesianChartPositionPidTime.AxisY.Add(PidI);  //axis 3
            cartesianChartPositionPidTime.AxisY.Add(PidD);  //axis 4
            cartesianChartPositionPidTime.Series.Add(ChartsData.ChartPidTimePositionWSeries);
            cartesianChartPositionPidTime.Series.Add(ChartsData.ChartPidTimePositionYSeries);
            cartesianChartPositionPidTime.Series.Add(ChartsData.ChartPidTimePositionUSeries);
            cartesianChartPositionPidTime.Series.Add(ChartsData.ChartPidTimePositionISeries);
            cartesianChartPositionPidTime.Series.Add(ChartsData.ChartPidTimePositionDSeries);





            cartesianChartPositionPidTime.DisableAnimations = true;
            cartesianChartPositionPidTime.AnimationsSpeed = new System.TimeSpan(0, 0, 0, 0, 1);
            myClientHelperAPI = ChartsData.myClientHelperAPI;
            if (mySubscription != null)
            {
                try
                {
                    myMonitoredItem = myClientHelperAPI.RemoveMonitoredItem(mySubscription, myMonitoredItem);
                }
                catch
                {
                    //ignore
                    ;
                }
            }
            timer1.Start();

        }

        public ChartPID_TimePositionForm()
        {
            InitializeComponent();
            ResetForm();
        }

        public ChartPID_TimePositionForm(Form callingForm)
        {
            mainForm = callingForm as UAClientForm;
            InitializeComponent();
            ResetForm();
        }

        private void ChartPID_TimePositionForm_Load(object sender, EventArgs e)
        {


            ResetForm();

            int numberIfItems = this.mainForm.namesOfVariblesPLCPidPosition.GetLength(1);
            try
            {
                if (mySubscription == null)
                {
                    mySubscription = myClientHelperAPI.Subscribe(1000);
                }
                for (int i = 0; i < numberIfItems; i++)

                {
                    //use different item names for correct assignment at the notificatino event
                    string monitoredItemName = this.mainForm.namesOfVariblesPLCPidPosition[1, i];
                    string nodeName = this.mainForm.namesOfVariblesPLCPidPosition[0, i];
                    myMonitoredItem = myClientHelperAPI.AddMonitoredItem(mySubscription, nodeName, monitoredItemName, 20, ChartPID_TimePositionNotification_MonitoredItem);

                }
                myClientHelperAPI.ItemChangedNotification += new MonitoredItemNotificationEventHandler(ChartPID_TimePositionNotification_MonitoredItem);
                ChartsData.myClientHelperAPI = myClientHelperAPI;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");

            }


        }

        private void ChartPID_TimePositionNotification_MonitoredItem(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MonitoredItemNotificationEventHandler(ChartPID_TimePositionNotification_MonitoredItem), monitoredItem, e);
                return;
            }
            MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
            if (notification == null)
            {
                return;
            }

            NotificationWriteToValuesPidPosition(monitoredItem, notification);

            notification = null;


        }

        private void colorSliderW_ValueChanged(object sender, EventArgs e)
        {
            labelW.Text = colorSliderW.Value.ToString();

        }

        private void colorSliderY_ValueChanged(object sender, EventArgs e)
        {
            labelY.Text = colorSliderY.Value.ToString();
        }

        private void colorSliderU_ValueChanged(object sender, EventArgs e)
        {
            labelU.Text = colorSliderU.Value.ToString();
        }

        private void colorSliderI_ValueChanged(object sender, EventArgs e)
        {
            labelI.Text = colorSliderI.Value.ToString();
        }

        private void colorSliderD_ValueChanged(object sender, EventArgs e)
        {
            labelD.Text = colorSliderD.Value.ToString();
        }

        private void colorSliderP_ValueChanged(object sender, EventArgs e)
        {
            labelP.Text = colorSliderP.Value.ToString();
        }

        private void colorSliderE_ValueChanged(object sender, EventArgs e)
        {
            labelE.Text = colorSliderE.Value.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            diffTime = stopWatch.ElapsedMilliseconds / 1000;
            ObservablePoint oP = new ObservablePoint();
            double val;
            oP.X = diffTime;
            val = dataPID.W;
            oP.Y = val;
            setColorSliderValue(val, colorSliderW);
            ChartsData.ChartPidTimePositionW.Add(oP);
            val = dataPID.Y;
            oP.Y = val;
            setColorSliderValue(val, colorSliderY);
            ChartsData.ChartPidTimePositionY.Add(oP);
            val = dataPID.E;
            oP.Y = val;
            setColorSliderValue(val, colorSliderE);
            val = dataPID.D;
            oP.Y = val;
            setColorSliderValue(val, colorSliderD);
            ChartsData.ChartPidTimePositionD.Add(oP);
            val = dataPID.I;
            oP.Y = val;
            setColorSliderValue(val, colorSliderI);
            ChartsData.ChartPidTimePositionI.Add(oP);
            val = dataPID.U;
            oP.Y = val;
            setColorSliderValue(val, colorSliderU);
            ChartsData.ChartPidTimePositionU.Add(oP);
            val = dataPID.P;
            oP.Y = val;
            setColorSliderValue(val, colorSliderP);
            //ChartsData.ChartPidTimePositionP.Add(oP);
        }

        private void setColorSliderValue(double val, ColorSlider.ColorSlider CS)
        {
            if (CS.Maximum < val)
            {
                CS.Value = CS.Maximum;
            }
            else
            {
                if (CS.Minimum > val)
                {
                    CS.Value = CS.Minimum;
                }
                else
                {
                    CS.Value = (int)val;
                }
            }

        }

        private void numericUpDownMinW_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownDiffW.Value = numericUpDownMaxW.Value - numericUpDownMinW.Value;
            PidW.MinValue = (double)numericUpDownMinW.Value;
        }

        private void numericUpDownMaxW_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownDiffW.Value = numericUpDownMaxW.Value - numericUpDownMinW.Value;
            PidW.MaxValue = (double)numericUpDownMaxW.Value;
        }

        private void numericUpDownDiffW_ValueChanged(object sender, EventArgs e)
        {
            float delta = ((float)numericUpDownDiffW.Value - ((float)numericUpDownMaxW.Value - (float)numericUpDownMinW.Value)) / 2;
            numericUpDownMaxW.Value += (decimal)delta;
            numericUpDownMinW.Value -= (decimal)delta;
        }

        private void checkBoxRangeAutoW_CheckedChanged(object sender, EventArgs e)
        {
            panelW.Enabled = !checkBoxRangeAutoW.Checked;
            if (panelW.Enabled)
            {
                try
                {
                    numericUpDownMaxW.Value = (decimal)PidW.MaxValue;
                    numericUpDownMinW.Value = (decimal)PidW.MinValue;
                    numericUpDownDiffW.Value = numericUpDownMaxW.Value - numericUpDownMinW.Value;
                }
                catch (Exception)
                {

                    ;
                }
            }
        }

        private void numericUpDownMinY_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownDiffY.Value = numericUpDownMaxY.Value - numericUpDownMinY.Value;
            PidY.MinValue = (double)numericUpDownMinY.Value;
        }

        private void numericUpDownMaxY_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownDiffY.Value = numericUpDownMaxY.Value - numericUpDownMinY.Value;
            PidY.MaxValue = (double)numericUpDownMaxY.Value;
        }

        private void numericUpDownDiffY_ValueChanged(object sender, EventArgs e)
        {
            float delta = ((float)numericUpDownDiffY.Value - ((float)numericUpDownMaxY.Value - (float)numericUpDownMinY.Value)) / 2;
            numericUpDownMaxY.Value += (decimal)delta;
            numericUpDownMinY.Value -= (decimal)delta;
        }

        private void checkBoxRangeAutoY_CheckedChanged(object sender, EventArgs e)
        {
            panelY.Enabled = !checkBoxRangeAutoY.Checked;
            if (panelY.Enabled)
            {
                try
                {
                    numericUpDownMaxY.Value = (decimal)PidY.MaxValue;
                    numericUpDownMinY.Value = (decimal)PidY.MinValue;
                    numericUpDownDiffY.Value = numericUpDownMaxY.Value - numericUpDownMinY.Value;
                }
                catch (Exception)
                {

                    ;
                }
            }
        }


        private void numericUpDownMinU_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownDiffU.Value = numericUpDownMaxU.Value - numericUpDownMinU.Value;
            PidU.MinValue = (double)numericUpDownMinU.Value;
        }

        private void numericUpDownMaxU_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownDiffU.Value = numericUpDownMaxU.Value - numericUpDownMinU.Value;
            PidU.MaxValue = (double)numericUpDownMaxU.Value;
        }

        private void numericUpDownDiffU_ValueChanged(object sender, EventArgs e)
        {
            float delta = ((float)numericUpDownDiffU.Value - ((float)numericUpDownMaxU.Value - (float)numericUpDownMinU.Value)) / 2;
            numericUpDownMaxU.Value += (decimal)delta;
            numericUpDownMinU.Value -= (decimal)delta;
        }

        private void checkBoxRangeAutoU_CheckedChanged(object sender, EventArgs e)
        {
            panelU.Enabled = !checkBoxRangeAutoU.Checked;
            if (panelU.Enabled)
            {
                try
                {
                    numericUpDownMaxU.Value = (decimal)PidU.MaxValue;
                    numericUpDownMinU.Value = (decimal)PidU.MinValue;
                    numericUpDownDiffU.Value = numericUpDownMaxU.Value - numericUpDownMinU.Value;
                }
                catch (Exception)
                {

                    ;
                }
            }
        }

        private void numericUpDownMinI_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownDiffI.Value = numericUpDownmaxI.Value - numericUpDownMinI.Value;
            PidI.MinValue = (double)numericUpDownMinI.Value;
        }

        private void numericUpDownMaxI_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownDiffI.Value = numericUpDownmaxI.Value - numericUpDownMinI.Value;
            PidI.MaxValue = (double)numericUpDownmaxI.Value;
        }

        private void numericUpDownDiffI_ValueChanged(object sender, EventArgs e)
        {
            float delta = ((float)numericUpDownDiffI.Value - ((float)numericUpDownmaxI.Value - (float)numericUpDownMinI.Value)) / 2;
            numericUpDownmaxI.Value += (decimal)delta;
            numericUpDownMinI.Value -= (decimal)delta;
        }

        private void checkBoxRangeAutoI_CheckedChanged(object sender, EventArgs e)
        {
            panelI.Enabled = !checkBoxRangeAutoI.Checked;
            if (panelI.Enabled)
            {
                try
                {
                    numericUpDownmaxI.Value = (decimal)PidI.MaxValue;
                    numericUpDownMinI.Value = (decimal)PidI.MinValue;
                    numericUpDownDiffI.Value = numericUpDownmaxI.Value - numericUpDownMinI.Value;
                }
                catch (Exception)
                {

                    ;
                }
            }
        }

        private void numericUpDownMinD_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownDiffD.Value = numericUpDownMaxD.Value - numericUpDownMinD.Value;
            PidD.MinValue = (double)numericUpDownMinD.Value;
        }

        private void numericUpDownMaxD_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownDiffD.Value = numericUpDownMaxD.Value - numericUpDownMinD.Value;
            PidD.MaxValue = (double)numericUpDownMaxD.Value;
        }

        private void numericUpDownDiffD_ValueChanged(object sender, EventArgs e)
        {
            float delta = ((float)numericUpDownDiffD.Value - ((float)numericUpDownMaxD.Value - (float)numericUpDownMinD.Value)) / 2;
            numericUpDownMaxD.Value += (decimal)delta;
            numericUpDownMinD.Value -= (decimal)delta;
        }

        private void checkBoxRangeAutoD_CheckedChanged(object sender, EventArgs e)
        {
            panelD.Enabled = !checkBoxRangeAutoD.Checked;
            if (panelD.Enabled)
            {
                try
                {
                    numericUpDownMaxD.Value = (decimal)PidD.MaxValue;
                    numericUpDownMinD.Value = (decimal)PidD.MinValue;
                    numericUpDownDiffD.Value = numericUpDownMaxD.Value - numericUpDownMinD.Value;
                }
                catch (Exception)
                {

                    ;
                }
            }
        }

        private void buttonPidTune_Click(object sender, EventArgs e)
        {
            if (mainForm.checkBoxControlOnPosition.Checked)
            {
                writeNode(vTrue, "MachineControl_strainControl_command_runTuning");
            }
            else
            {
                MessageBox.Show("Tuning is possible only if force is under control", "Error");
            }

        }

        private void writeNode(string TextValue, string TextNode)
        {
            List<String> values = new List<string>();
            List<String> nodeIdStrings = new List<string>();
            values.Add(TextValue);
            nodeIdStrings.Add(TextNode);
            writeValuesToNode(TextNode, values, nodeIdStrings);
        }

        private void writeValuesToNode(string TextNode, List<string> values, List<string> nodeIdStrings)
        {

            try
            {
                myClientHelperAPI.WriteValues(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void buttonPidSimulation_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxTuningActive_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTuningActive.Checked)
            {
                MessageBox.Show("Tuning starting", "Information");
            }
            else
            {
                MessageBox.Show("Tuning finished", "Information");
            }
        }

        private void numericUpDownTim_ValueChanged(object sender, EventArgs e)
        {

        }

        private void NotificationWriteToValuesPidPosition(MonitoredItem monitoredItem, MonitoredItemNotification notification)
        {
            try
            {

                diffTime = stopWatch.ElapsedMilliseconds / 1000;
                ObservablePoint oP = new ObservablePoint();
                oP.X = diffTime;
                if (notification == null) return;


                if (monitoredItem.DisplayName == "PositionSt_PID_ActValue")
                {
                    float val = (float)notification.Value.Value;
                    oP.Y = val;
                    if (dataPID.Y == oP.Y) return;
                    dataPID.Y = val;
                    colorSliderY.Value = (int)oP.Y;

                    ChartsData.ChartPidTimePositionY.Add(oP);


                }
                if (monitoredItem.DisplayName == "PositionSt_PID_SetValue")
                {
                    float val = (float)notification.Value.Value;
                    oP.Y = val;
                    if (dataPID.W == oP.Y) return;
                    dataPID.W = val;
                    colorSliderW.Value = (int)oP.Y;

                    ChartsData.ChartPidTimePositionW.Add(oP);

                    //SetText(textBoxVelocity, notification.Value.ToString());
                    //WriteValuesToChart(textBoxPositionTime.Text, notification.Value.ToString(), ChartsData.ChartVelocityTimeValues, ChartsData.ChartVelocityTimeValuesLock,
                    //    ref lockVelocityTime, ref lastTimeVelocity, ref lastValueVelocity);
                    //WriteValuesToChart(textBoxVelocity.Text, textBoxStrain.Text, ChartsData.ChartVelocityStrainValues, ChartsData.ChartVelocityStrainValuesLock,
                    //     ref lockVelocityStrain, ref lastValueVelocityStrainVelocity, ref lastValueVelocityStrainStrain);

                }
                if (monitoredItem.DisplayName == "PositionSt_PID_ControlError")
                {
                    float val = (float)notification.Value.Value;
                    oP.Y = val;
                    if (dataPID.E == oP.Y) return;
                    dataPID.E = val;
                    colorSliderE.Value = (int)oP.Y;


                }
                if (monitoredItem.DisplayName == "PositionSt_PID_Error")
                {


                }

                if (monitoredItem.DisplayName == "PositionSt_PID_DerivativePart")
                {

                    float val = (float)notification.Value.Value;
                    oP.Y = val;
                    if (dataPID.D == oP.Y) return;
                    dataPID.D = val;
                    colorSliderD.Value = (int)oP.Y;

                    ChartsData.ChartPidTimePositionD.Add(oP);

                }
                if (monitoredItem.DisplayName == "PositionSt_PID_IntegrationPart")
                {

                    float val = (float)notification.Value.Value;
                    oP.Y = val;
                    if (dataPID.I == oP.Y) return;
                    dataPID.I = val;
                    colorSliderI.Value = (int)oP.Y;

                    ChartsData.ChartPidTimePositionI.Add(oP);
                }
                if (monitoredItem.DisplayName == "PositionSt_PID_Out")
                {
                    float val = (float)notification.Value.Value;
                    oP.Y = val;
                    if (dataPID.U == oP.Y) return;
                    dataPID.U = val;
                    colorSliderU.Value = (int)oP.Y;

                    ChartsData.ChartPidTimePositionU.Add(oP);

                }

                if (monitoredItem.DisplayName == "PositionSt_PID_ProportionalPart")
                {

                    float val = (float)notification.Value.Value;
                    oP.Y = val;
                    if (colorSliderP.Value == oP.Y) return;
                    dataPID.P = val;
                    colorSliderP.Value = (int)oP.Y;

                    //ChartsData.ChartPidTimePositionP.Add(oP);
                }



            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                //throw;
            }

        }


    }

}
