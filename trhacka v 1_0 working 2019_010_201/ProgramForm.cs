using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace trhacka_v_1_0_working_2019_010_201
{
    public partial class ProgramForm : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        public ProgramForm()
        {
            InitializeComponent();
        }

        private void dataGridViewActualProgram_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStripButtonNový_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            var fileName = string.Empty;
            var fileNameWithPath = string.Empty;
            filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            fileName = "new.xml";
            fileNameWithPath = filePath + "\\" + fileName;
            labelPath.Text = fileNameWithPath;
            labelProgram.Text = fileName;
            try
            {
                if (File.Exists(fileNameWithPath))
                {
                    filePath = writeToDatagridView(dataGridViewActualProgram, fileNameWithPath);
                }
                else
                {
                    filePath = FileRead(dataGridViewActualProgram);


                }
            }


            catch (Exception)
            {


            }


        }

        public string FileRead(DataGridView dataGridview)
        {
            openFileDialog.CheckFileExists = false;
            string filePath = null;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;

                ////Read the contents of the file into a stream
                labelPath.Text = filePath;
                int index = filePath.LastIndexOf(@"\") + 1;
                labelProgram.Text = filePath.Substring(index, filePath.Length - index);
                return writeToDatagridView(dataGridview, filePath);

            }

            return filePath;
        }

        private string writeToDatagridView(DataGridView dataGridview, string filePath)
        {
            try
            {
                dataSet1.Clear();
                dataSet1.ReadXml(filePath);

                // Create a new row first as it will include the columns you've created at design-time.

                dataGridview.Rows.Clear();

                int actualRow = 0;
                //}
                foreach (DataRow dr in dataSet1.Tables[0].Rows)
                {


                    // Grab the new row!
                    DataGridViewRow row = new DataGridViewRow();
                    row = (DataGridViewRow)dataGridview.Rows[0].Clone();
                    bool added = false;
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (dr.ItemArray[i].ToString() != "-")
                        {
                            row.Cells[i].Value = dr.ItemArray[i];
                            added = true;
                        }
                        else
                        {
                            if (i == 0)
                            {
                                row.Cells[i].Value = actualRow++.ToString();
                            }
                            else
                            {
                                row.Cells[i].Value = null;
                            }
                        }


                    }

                    if (added)
                    {
                        dataGridview.Rows.Add(row);

                    }



                }
                return filePath;


            }
            catch (Exception)
            {

                labelPath.Text = "";
                labelProgram.Text = "";
                return null;
            }
        }

        private void toolStripButtonRead_Click(object sender, EventArgs e)
        {
            if (FileRead(dataGridViewActualProgram) != null)
            {

            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            var fileName = string.Empty;



            saveFileDialog.FileName = labelPath.Text;
            saveFile(ref filePath);

        }

        private void saveFile(ref string filePath)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DataTable dt = new DataTable();
                dt.TableName = "Steps";
                for (int i = 0; i < dataGridViewActualProgram.Columns.Count; i++)
                {
                    //if (dataGridView1.Columns[i].Visible) // Add's only Visible columns (if you need it)
                    //{
                    string headerText = dataGridViewActualProgram.Columns[i].HeaderText;
                    headerText = Regex.Replace(headerText, "[-/, ]", "_");

                    DataColumn column = new DataColumn(headerText);
                    dt.Columns.Add(column);
                    //}
                }
                if (dataGridViewActualProgram.Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    foreach (DataGridViewRow DataGVRow in dataGridViewActualProgram.Rows)
                    {
                        DataRow dataRow = dt.NewRow();

                        for (int i = 0; i < dataGridViewActualProgram.Columns.Count; i++)
                        {

                            string headerText = dataGridViewActualProgram.Columns[i].HeaderText;
                            string headerTextRegular = Regex.Replace(headerText, "[-/, ]", "_");
                            if (DataGVRow.Cells[i].Value == null)
                            {
                                dataRow[headerTextRegular] = "-";
                            }

                            else
                            {
                                dataRow[headerTextRegular] = DataGVRow.Cells[i].Value;
                            }


                        }

                        dt.Rows.Add(dataRow); //dt.Columns.Add();
                    }


                }
                var fileStream = saveFileDialog.OpenFile();
                fileStream.Flush();
                filePath = saveFileDialog.FileName;
                labelPath.Text = filePath;
                int index = filePath.LastIndexOf(@"\") + 1;

                labelProgram.Text = filePath.Substring(index, filePath.Length - index);
                DataSet ds = new DataSet();
                ds.DataSetName = "Program";
                ds.Tables.Add(dt);
                var programName = string.Empty;
                index = labelProgram.Text.IndexOf('.');
                if (index == -1)
                {
                    ds.DataSetName = labelProgram.Text;
                }
                else
                {
                    ds.DataSetName = labelProgram.Text.Substring(0, index);
                }

                filePath = openFileDialog.FileName;
                //Get the path of specified file


                //Read the contents of the file into a stream


                ds.WriteXml(fileStream);


                fileStream.Close();

                //using (StreamReader reader = new StreamReader(fileStream))
                //{
                //    fileContent = reader.ReadToEnd();
                //}

            }
        }

        private void toolStripButtonSaveAs_Click(object sender, EventArgs e)
        {
            {
                var fileContent = string.Empty;
                var filePath = string.Empty;
                var fileName = string.Empty;



                saveFileDialog.FileName = "";
                saveFile(ref filePath);
            }
        }

        private void ProgramForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonAddNewLine_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonTest_Click(object sender, EventArgs e)
        {

        }
    }
}
