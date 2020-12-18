namespace trhacka_v_1_0_working_2019_010_201
{
    partial class ChartTimeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Button buttonClearChart;
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.textBoxPointPozition = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.textBoxPointsForce = new System.Windows.Forms.TextBox();
            this.textBoxPointsSpeed = new System.Windows.Forms.TextBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.buttonWriteData = new System.Windows.Forms.Button();
            this.label33 = new System.Windows.Forms.Label();
            this.numericUpDownTimeToWriteToChart = new System.Windows.Forms.NumericUpDown();
            this.cartesianChartAllTime = new LiveCharts.WinForms.CartesianChart();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.buttonSaveData = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.formsPlotTimeSeries = new ScottPlot.FormsPlot();
            buttonClearChart = new System.Windows.Forms.Button();
            this.groupBox15.SuspendLayout();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeToWriteToChart)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClearChart
            // 
            buttonClearChart.Location = new System.Drawing.Point(1078, 909);
            buttonClearChart.Name = "buttonClearChart";
            buttonClearChart.Size = new System.Drawing.Size(75, 23);
            buttonClearChart.TabIndex = 40;
            buttonClearChart.Text = "Clear Chart";
            buttonClearChart.UseVisualStyleBackColor = true;
            buttonClearChart.Click += new System.EventHandler(this.buttonClearChart_Click);
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.label34);
            this.groupBox15.Controls.Add(this.label36);
            this.groupBox15.Controls.Add(this.textBoxPointPozition);
            this.groupBox15.Controls.Add(this.label35);
            this.groupBox15.Controls.Add(this.textBoxPointsForce);
            this.groupBox15.Controls.Add(this.textBoxPointsSpeed);
            this.groupBox15.Location = new System.Drawing.Point(1018, 188);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(151, 170);
            this.groupBox15.TabIndex = 13;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Zapsáno bodů";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(5, 18);
            this.label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(39, 13);
            this.label34.TabIndex = 7;
            this.label34.Text = "Pozice";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(5, 56);
            this.label36.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(48, 13);
            this.label36.TabIndex = 9;
            this.label36.Text = "Rychlost";
            // 
            // textBoxPointPozition
            // 
            this.textBoxPointPozition.Location = new System.Drawing.Point(57, 15);
            this.textBoxPointPozition.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPointPozition.Name = "textBoxPointPozition";
            this.textBoxPointPozition.Size = new System.Drawing.Size(76, 20);
            this.textBoxPointPozition.TabIndex = 3;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(5, 92);
            this.label35.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(26, 13);
            this.label35.TabIndex = 8;
            this.label35.Text = "Síla";
            // 
            // textBoxPointsForce
            // 
            this.textBoxPointsForce.Location = new System.Drawing.Point(57, 90);
            this.textBoxPointsForce.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPointsForce.Name = "textBoxPointsForce";
            this.textBoxPointsForce.Size = new System.Drawing.Size(76, 20);
            this.textBoxPointsForce.TabIndex = 5;
            // 
            // textBoxPointsSpeed
            // 
            this.textBoxPointsSpeed.Location = new System.Drawing.Point(57, 51);
            this.textBoxPointsSpeed.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPointsSpeed.Name = "textBoxPointsSpeed";
            this.textBoxPointsSpeed.Size = new System.Drawing.Size(76, 20);
            this.textBoxPointsSpeed.TabIndex = 6;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.buttonWriteData);
            this.groupBox10.Controls.Add(this.label33);
            this.groupBox10.Controls.Add(this.numericUpDownTimeToWriteToChart);
            this.groupBox10.Location = new System.Drawing.Point(1018, 12);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(154, 170);
            this.groupBox10.TabIndex = 12;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Parametry grafu";
            // 
            // buttonWriteData
            // 
            this.buttonWriteData.Location = new System.Drawing.Point(7, 77);
            this.buttonWriteData.Name = "buttonWriteData";
            this.buttonWriteData.Size = new System.Drawing.Size(75, 23);
            this.buttonWriteData.TabIndex = 3;
            this.buttonWriteData.Text = "Zapisuj";
            this.buttonWriteData.UseVisualStyleBackColor = true;
            this.buttonWriteData.Click += new System.EventHandler(this.buttonWriteData_Click);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Cursor = System.Windows.Forms.Cursors.Default;
            this.label33.Location = new System.Drawing.Point(7, 18);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(97, 13);
            this.label33.TabIndex = 2;
            this.label33.Text = "Interval zápisu [ms]";
            // 
            // numericUpDownTimeToWriteToChart
            // 
            this.numericUpDownTimeToWriteToChart.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownTimeToWriteToChart.Location = new System.Drawing.Point(6, 37);
            this.numericUpDownTimeToWriteToChart.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownTimeToWriteToChart.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownTimeToWriteToChart.Name = "numericUpDownTimeToWriteToChart";
            this.numericUpDownTimeToWriteToChart.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownTimeToWriteToChart.TabIndex = 1;
            this.numericUpDownTimeToWriteToChart.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDownTimeToWriteToChart.ValueChanged += new System.EventHandler(this.numericUpDownTimeToWriteToChart_ValueChanged);
            // 
            // cartesianChartAllTime
            // 
            this.cartesianChartAllTime.Location = new System.Drawing.Point(12, 12);
            this.cartesianChartAllTime.MaximumSize = new System.Drawing.Size(1000, 700);
            this.cartesianChartAllTime.MinimumSize = new System.Drawing.Size(1000, 350);
            this.cartesianChartAllTime.Name = "cartesianChartAllTime";
            this.cartesianChartAllTime.Size = new System.Drawing.Size(1000, 700);
            this.cartesianChartAllTime.TabIndex = 11;
            this.cartesianChartAllTime.Text = "Time";
            this.cartesianChartAllTime.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // buttonSaveData
            // 
            this.buttonSaveData.Location = new System.Drawing.Point(1075, 869);
            this.buttonSaveData.Name = "buttonSaveData";
            this.buttonSaveData.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveData.TabIndex = 41;
            this.buttonSaveData.Text = "Ulož";
            this.buttonSaveData.UseVisualStyleBackColor = true;
            this.buttonSaveData.Click += new System.EventHandler(this.buttonSaveData_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "csv";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.SupportMultiDottedExtensions = true;
            this.saveFileDialog1.Title = "Zápis";
            // 
            // formsPlotTimeSeries
            // 
            this.formsPlotTimeSeries.Location = new System.Drawing.Point(29, 12);
            this.formsPlotTimeSeries.Name = "formsPlotTimeSeries";
            this.formsPlotTimeSeries.Size = new System.Drawing.Size(970, 677);
            this.formsPlotTimeSeries.TabIndex = 42;
            // 
            // ChartTimeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 962);
            this.ControlBox = false;
            this.Controls.Add(this.formsPlotTimeSeries);
            this.Controls.Add(this.buttonSaveData);
            this.Controls.Add(buttonClearChart);
            this.Controls.Add(this.groupBox15);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.cartesianChartAllTime);
            this.DoubleBuffered = true;
            this.MaximumSize = new System.Drawing.Size(1200, 1000);
            this.MinimumSize = new System.Drawing.Size(1200, 1000);
            this.Name = "ChartTimeForm";
            this.Text = "Časový graf";
            this.Load += new System.EventHandler(this.ChartTimeForm_Load);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeToWriteToChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox textBoxPointPozition;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.TextBox textBoxPointsForce;
        private System.Windows.Forms.TextBox textBoxPointsSpeed;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.NumericUpDown numericUpDownTimeToWriteToChart;
        public LiveCharts.WinForms.CartesianChart cartesianChartAllTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button buttonWriteData;
        private System.Windows.Forms.Button buttonSaveData;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private ScottPlot.FormsPlot formsPlotTimeSeries;
    }
}