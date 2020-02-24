namespace trhacka_v_1_0_working_2019_010_201
{
    partial class ChartForcePositionForm
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
            this.cartesianChartPositionForce = new LiveCharts.WinForms.CartesianChart();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.label34 = new System.Windows.Forms.Label();
            this.textBoxPoint = new System.Windows.Forms.TextBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.buttonWriteData = new System.Windows.Forms.Button();
            this.label33 = new System.Windows.Forms.Label();
            this.numericUpDownTimeToWriteToChart = new System.Windows.Forms.NumericUpDown();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.buttonSaveData = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.formsPlotPositionForce = new ScottPlot.FormsPlot();
            buttonClearChart = new System.Windows.Forms.Button();
            this.groupBox15.SuspendLayout();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeToWriteToChart)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClearChart
            // 
            buttonClearChart.Location = new System.Drawing.Point(1085, 915);
            buttonClearChart.Name = "buttonClearChart";
            buttonClearChart.Size = new System.Drawing.Size(75, 23);
            buttonClearChart.TabIndex = 41;
            buttonClearChart.Text = "Clear Chart";
            buttonClearChart.UseVisualStyleBackColor = true;
            buttonClearChart.Click += new System.EventHandler(this.ButtonClearChart_Click);
            // 
            // cartesianChartPositionForce
            // 
            this.cartesianChartPositionForce.Location = new System.Drawing.Point(20, 12);
            this.cartesianChartPositionForce.MaximumSize = new System.Drawing.Size(1000, 700);
            this.cartesianChartPositionForce.MinimumSize = new System.Drawing.Size(1000, 700);
            this.cartesianChartPositionForce.Name = "cartesianChartPositionForce";
            this.cartesianChartPositionForce.Size = new System.Drawing.Size(1000, 700);
            this.cartesianChartPositionForce.TabIndex = 38;
            this.cartesianChartPositionForce.Text = "Position";
            this.cartesianChartPositionForce.Visible = false;
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.label34);
            this.groupBox15.Controls.Add(this.textBoxPoint);
            this.groupBox15.Location = new System.Drawing.Point(1026, 188);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(151, 170);
            this.groupBox15.TabIndex = 43;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Zapsáno bodů";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(5, 18);
            this.label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(27, 13);
            this.label34.TabIndex = 7;
            this.label34.Text = "Graf";
            // 
            // textBoxPoint
            // 
            this.textBoxPoint.Location = new System.Drawing.Point(57, 15);
            this.textBoxPoint.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPoint.Name = "textBoxPoint";
            this.textBoxPoint.Size = new System.Drawing.Size(76, 20);
            this.textBoxPoint.TabIndex = 3;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.buttonWriteData);
            this.groupBox10.Controls.Add(this.label33);
            this.groupBox10.Controls.Add(this.numericUpDownTimeToWriteToChart);
            this.groupBox10.Location = new System.Drawing.Point(1026, 12);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(154, 170);
            this.groupBox10.TabIndex = 42;
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
            this.buttonWriteData.Click += new System.EventHandler(this.buttonWriteData_Click_1);
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
            this.numericUpDownTimeToWriteToChart.ValueChanged += new System.EventHandler(this.NumericUpDownTimeToWriteToChart_ValueChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // buttonSaveData
            // 
            this.buttonSaveData.Location = new System.Drawing.Point(1085, 871);
            this.buttonSaveData.Name = "buttonSaveData";
            this.buttonSaveData.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveData.TabIndex = 44;
            this.buttonSaveData.Text = "Ulož";
            this.buttonSaveData.UseVisualStyleBackColor = true;
            this.buttonSaveData.Click += new System.EventHandler(this.buttonSaveData_Click);
            // 
            // formsPlotPositionForce
            // 
            this.formsPlotPositionForce.Location = new System.Drawing.Point(31, 30);
            this.formsPlotPositionForce.Name = "formsPlotPositionForce";
            this.formsPlotPositionForce.Size = new System.Drawing.Size(989, 701);
            this.formsPlotPositionForce.TabIndex = 45;
            // 
            // ChartForcePositionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 962);
            this.ControlBox = false;
            this.Controls.Add(this.formsPlotPositionForce);
            this.Controls.Add(this.buttonSaveData);
            this.Controls.Add(this.groupBox15);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(buttonClearChart);
            this.Controls.Add(this.cartesianChartPositionForce);
            this.MaximumSize = new System.Drawing.Size(1200, 1000);
            this.MinimumSize = new System.Drawing.Size(1200, 1000);
            this.Name = "ChartForcePositionForm";
            this.Text = "Graf pozice - síla";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ChartForcePositionForm_Load);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeToWriteToChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public LiveCharts.WinForms.CartesianChart cartesianChartPositionForce;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox textBoxPoint;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Button buttonWriteData;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.NumericUpDown numericUpDownTimeToWriteToChart;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button buttonSaveData;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private ScottPlot.FormsPlot formsPlotPositionForce;
    }
}