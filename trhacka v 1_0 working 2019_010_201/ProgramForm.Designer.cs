namespace trhacka_v_1_0_working_2019_010_201
{
    partial class ProgramForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewActualProgram = new System.Windows.Forms.DataGridView();
            this.dataSet1 = new System.Data.DataSet();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNový = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRead = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonTest = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.labelProgram = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelPath = new System.Windows.Forms.Label();
            this.Step = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Command = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Velocity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Force = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Acceleration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Duration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActualDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndForce = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewActualProgram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewActualProgram
            // 
            this.dataGridViewActualProgram.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewActualProgram.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Step,
            this.Command,
            this.Position,
            this.Velocity,
            this.Force,
            this.Acceleration,
            this.Duration,
            this.ActualDuration,
            this.EndForce});
            this.dataGridViewActualProgram.Location = new System.Drawing.Point(0, 75);
            this.dataGridViewActualProgram.Name = "dataGridViewActualProgram";
            this.dataGridViewActualProgram.Size = new System.Drawing.Size(894, 385);
            this.dataGridViewActualProgram.TabIndex = 41;
            this.dataGridViewActualProgram.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewActualProgram_CellContentClick);
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "xls";
            this.openFileDialog.Filter = "programy (*.xml)|*.xml|All files (*.*)|*.*;";
            this.openFileDialog.RestoreDirectory = true;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "xml";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNový,
            this.toolStripButtonRead,
            this.toolStripButtonSave,
            this.toolStripButtonSaveAs,
            this.toolStripSeparator1,
            this.toolStripButtonTest});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(894, 25);
            this.toolStrip1.TabIndex = 42;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonNový
            // 
            this.toolStripButtonNový.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonNový.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNový.Image")));
            this.toolStripButtonNový.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNový.Name = "toolStripButtonNový";
            this.toolStripButtonNový.Size = new System.Drawing.Size(39, 22);
            this.toolStripButtonNový.Text = "Nový";
            this.toolStripButtonNový.Click += new System.EventHandler(this.toolStripButtonNový_Click);
            // 
            // toolStripButtonRead
            // 
            this.toolStripButtonRead.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonRead.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRead.Image")));
            this.toolStripButtonRead.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRead.Name = "toolStripButtonRead";
            this.toolStripButtonRead.Size = new System.Drawing.Size(39, 22);
            this.toolStripButtonRead.Text = "Načti";
            this.toolStripButtonRead.Click += new System.EventHandler(this.toolStripButtonRead_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(34, 22);
            this.toolStripButtonSave.Text = "Ulož";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonSaveAs
            // 
            this.toolStripButtonSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveAs.Image")));
            this.toolStripButtonSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveAs.Name = "toolStripButtonSaveAs";
            this.toolStripButtonSaveAs.Size = new System.Drawing.Size(59, 22);
            this.toolStripButtonSaveAs.Text = "Ulož jako";
            this.toolStripButtonSaveAs.Click += new System.EventHandler(this.toolStripButtonSaveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonTest
            // 
            this.toolStripButtonTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonTest.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTest.Image")));
            this.toolStripButtonTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTest.Name = "toolStripButtonTest";
            this.toolStripButtonTest.Size = new System.Drawing.Size(43, 22);
            this.toolStripButtonTest.Text = "Testuj";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "Program:";
            // 
            // labelProgram
            // 
            this.labelProgram.AutoSize = true;
            this.labelProgram.Location = new System.Drawing.Point(69, 29);
            this.labelProgram.Name = "labelProgram";
            this.labelProgram.Size = new System.Drawing.Size(0, 13);
            this.labelProgram.TabIndex = 44;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 45;
            this.label2.Text = "Cesta:";
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(69, 45);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(0, 13);
            this.labelPath.TabIndex = 46;
            // 
            // Step
            // 
            this.Step.HeaderText = "Krok";
            this.Step.Name = "Step";
            this.Step.Width = 50;
            // 
            // Command
            // 
            this.Command.HeaderText = "Cmd";
            this.Command.Items.AddRange(new object[] {
            "STEP",
            "STOP_PROGRAM",
            "START_PROGRAM",
            "POSITION_CONTROL",
            "VELOCITY_CONTROL",
            "FORCE_CONTROL",
            "SET_ACCELERATION",
            "SET_VELOCITY",
            "SET_SPEED",
            "SET_MINIMUM_FORCE",
            "SET_HOME",
            "SET_POSITION"});
            this.Command.Name = "Command";
            this.Command.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Command.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Position
            // 
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.Position.DefaultCellStyle = dataGridViewCellStyle1;
            this.Position.HeaderText = "Pozice";
            this.Position.Name = "Position";
            // 
            // Velocity
            // 
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.Velocity.DefaultCellStyle = dataGridViewCellStyle2;
            this.Velocity.HeaderText = "Rychlost";
            this.Velocity.Name = "Velocity";
            // 
            // Force
            // 
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.Force.DefaultCellStyle = dataGridViewCellStyle3;
            this.Force.HeaderText = "Síla";
            this.Force.Name = "Force";
            // 
            // Acceleration
            // 
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.Acceleration.DefaultCellStyle = dataGridViewCellStyle4;
            this.Acceleration.HeaderText = "Zrychlení";
            this.Acceleration.Name = "Acceleration";
            // 
            // Duration
            // 
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.Duration.DefaultCellStyle = dataGridViewCellStyle5;
            this.Duration.HeaderText = "Doba";
            this.Duration.Name = "Duration";
            // 
            // ActualDuration
            // 
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = null;
            this.ActualDuration.DefaultCellStyle = dataGridViewCellStyle6;
            this.ActualDuration.HeaderText = "Průběh";
            this.ActualDuration.Name = "ActualDuration";
            // 
            // EndForce
            // 
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = null;
            this.EndForce.DefaultCellStyle = dataGridViewCellStyle7;
            this.EndForce.HeaderText = "Síla STOP";
            this.EndForce.Name = "EndForce";
            // 
            // ProgramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 462);
            this.Controls.Add(this.labelPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelProgram);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dataGridViewActualProgram);
            this.Name = "ProgramForm";
            this.Text = "ProgramForm";
            this.Load += new System.EventHandler(this.ProgramForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewActualProgram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewActualProgram;
        private System.Data.DataSet dataSet1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonRead;
        private System.Windows.Forms.ToolStripButton toolStripButtonNový;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelProgram;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn Step;
        private System.Windows.Forms.DataGridViewComboBoxColumn Command;
        private System.Windows.Forms.DataGridViewTextBoxColumn Position;
        private System.Windows.Forms.DataGridViewTextBoxColumn Velocity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Force;
        private System.Windows.Forms.DataGridViewTextBoxColumn Acceleration;
        private System.Windows.Forms.DataGridViewTextBoxColumn Duration;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActualDuration;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndForce;
    }
}