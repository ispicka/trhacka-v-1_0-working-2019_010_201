using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;



namespace trhacka_v_1_0_working_2019_010_201
{
    public class Programming
    {
        bool ready;
        bool error;
        UAClientForm forms;
        private string vTrue = "True";
        private string vFalse = "False";
        Timer programTimer = new Timer();
        StepType step = new StepType();
        int row = new int();
        int downloadStepIndex = new int();
        int programStepIndex = new int();
        bool programmingStart = false;
        int actualStep = new int();
        int actualRowOfProgram = new int();
        public int ActualStep { get => actualStep; set => actualStep = value; }
        public bool Ready { get => ready; set => ready = value; }
        public bool Error { get => error; set => error = value; }
        public StepType ActualProgramStep { get => actualProgramStep; set => actualProgramStep = value; }
        public int ProgramStepIndex { get => programStepIndex; set => programStepIndex = value; }
        public int DownloadStepIndex { get => downloadStepIndex; set => downloadStepIndex = value; }
        public StepType Step { get => step; set => step = value; }
        public int Row { get => row; set => row = value; }

        StepType actualProgramStep;



        public Programming(UAClientForm f)
        {
            forms = f;
            programTimer.Enabled = false;
            programTimer.Interval = 1;
            programTimer.Tick += new EventHandler(programControl);
        }

        void programControl(Object myObject, EventArgs myEventArgs)
        {
            int result;
            if (!programmingStart)
            {
                return;
            }
            else
            {
                switch (ProgramStepIndex)
                {
                    case 0:
                        Row = 0;
                        ActualStep = 0;
                        DownloadStepIndex = 0;
                        ProgramStepIndex++;
                        break;
                    case 1:
                        result = DownloadProgramStep(forms.dataGridViewActualProgram, Row);
                        if (result < 0)
                        {
                            forms.buttonDownloadProgram.BackColor = Color.Red;
                        }
                        else
                        {
                            forms.buttonDownloadProgram.BackColor = Color.Green;
                            ProgramStepIndex++;
                        }
                        break;
                    case 2:
                        result = DownloadProgramStep(forms.dataGridViewActualProgram, Row);
                        if (result < 0)
                        {
                            forms.buttonDownloadProgram.BackColor = Color.Red;
                        }
                        if (result == 1)
                        {

                            ProgramStepIndex++;
                            Row++;

                        }
                        break;
                    case 3:
                        if (Row >= forms.dataGridViewActualProgram.Rows.Count - 1)
                        {
                            ProgramStepIndex++;

                        }
                        else
                        {
                            ProgramStepIndex = 1;
                        }

                        break;
                    case 4:
                        forms.writeNode(vTrue, "ns=6;s=::AsGlobalPV:Program.transfer.WriteProgram");
                        ProgramStepIndex++;
                        break;
                    case 5:
                        if (forms.transfer.WritingProgramDone)
                        {
                            ProgramStepIndex++;
                        }
                        break;
                    case 6:
                        forms.writeNode(vFalse, "ns=6;s=::AsGlobalPV:Program.transfer.WriteProgram");
                        forms.buttonDownloadProgram.BackColor = SystemColors.Control;
                        stopDownload();
                        break;

                    default:
                        break;
                }
            }
        }

        public void startDownload()
        {
            DownloadStepIndex = 0;
            Row = 0;
            ProgramStepIndex = 0;
            programTimer.Enabled = true;
            programTimer.Start();
            programmingStart = true;
            forms.writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.programming");
            forms.buttonStartProgram.Visible = false;

        }
        public void stopDownload()
        {
            DownloadStepIndex = 0;
            Row = 0;
            ProgramStepIndex = 0;
            programTimer.Enabled = false;
            programTimer.Stop();
            programmingStart = false;
            forms.writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.programming");
            if (forms.transfer.WritingProgramDone)
            {
                forms.buttonStartProgram.Visible = true;
            }
        }
        public int DownloadProgramStep(DataGridView dataGridView, int row)
        {


            int retVal = 0;

            DataGridViewRow actualRow = new DataGridViewRow();
            if (dataGridView.RowCount == 0)
            {
                return -1;
            }

            switch (downloadStepIndex)
            {
                case 0:
                    actualRow = dataGridView.Rows[row];
                    forms.stepType.Command = (programCodes)(-1);

                    if (programCommand(actualRow, row) < 0)
                    {
                        return -1;
                    }

                    downloadStepIndex++;

                    retVal = 0;
                    break;
                case 1:
                    if (testStepEq(Step, forms.stepType))
                    {
                        downloadStepIndex++;
                        forms.writeNode(vTrue, "ns=6;s=::AsGlobalPV:Program.transfer.WriteStep");
                    }
                    retVal = 0;
                    break;
                case 2:
                    if (forms.transfer.WritingDone)
                    {
                        forms.writeNode(vFalse, "ns=6;s=::AsGlobalPV:Program.transfer.WriteStep");
                        retVal = 1;
                        downloadStepIndex = 0;
                    }
                    break;


                default:
                    retVal = -1;
                    break;

            }


            return retVal;


        }

        bool testStepEq(StepType step1, StepType step2)
        {
            bool eq;
            eq = step1.Acceleration == step2.Acceleration &&
                step1.Command == step2.Command &&
                step1.Duration == step2.Duration &&
                step1.EndForce == step2.EndForce &&
                step1.Force == step2.Force &&
                step1.Position == step2.Position &&
                step1.SetAcceleration == step2.SetAcceleration &&
                step1.SetDuration == step2.SetDuration &&
                step1.SetEndForce == step2.SetEndForce &&
                step1.SetForce == step2.SetForce &&
                step1.SetHome == step2.SetHome &&
                step1.SetPosition == step2.SetPosition &&
                step1.SetSpeed == step2.SetSpeed &&
                step1.SetZeroPosition == step2.SetZeroPosition &&
                step1.Speed == step2.Speed;



            return eq;
        }


        public int programCommand(DataGridViewRow dataGridViewRow, int step)
        {
            int resultOfFunction = 0;
            int command;
            /*
            "ns=6;s=::AsGlobalPV:Program.step.Acceleration",
                "ns=6;s=::AsGlobalPV:Program.step.Command",
                "ns=6;s=::AsGlobalPV:Program.step.Duration",
                "ns=6;s=::AsGlobalPV:Program.step.EndForce",
                "ns=6;s=::AsGlobalPV:Program.step.Force",
                "ns=6;s=::AsGlobalPV:Program.step.Passed",
                "ns=6;s=::AsGlobalPV:Program.step.Position",
                "ns=6;s=::AsGlobalPV:Program.step.SetAcceleration",
                "ns=6;s=::AsGlobalPV:Program.step.SetEndForce",
                "ns=6;s=::AsGlobalPV:Program.step.SetForce",
                "ns=6;s=::AsGlobalPV:Program.step.SetHome",
                "ns=6;s=::AsGlobalPV:Program.step.SetPosition",
                "ns=6;s=::AsGlobalPV:Program.step.SetSpeed",
                "ns=6;s=::AsGlobalPV:Program.step.SetZeroPosition",
                "ns=6;s=::AsGlobalPV:Program.step.Speed",
                */
            /*
            Start
            Stop
            Pozice
            Rychlost
            Síla
            Nastav nulovou pozici
            Nastav pozici
            */

            /*
 STEP				1	 	 	 	
 STOP_PROGRAM	    2			 	 	 	 	
 START_PROGRAM		3		 	 	 	 	
 POSITION_CONTROL	4			 	 	 	 	
 VELOCITY_CONTROL	5
 FORCE_CONTROL		6	
 SET_ACCELERATION	7			 	 	 	 	
 SET_VELOCITY		8
 SET_SPEED			9	 	 	 	 	
 SET_MINIMUM_FORCE	10			 	 	 	 	
 SET_HOME			11	 	 	 	 	
 SET_POSITION		12	 	 	 	 	
 	 	 	 	


              
            */
            var map = new Dictionary<string, int>();

            // ... Add some keys and values.
            map.Add("STEP", 1);
            map.Add("STOP_PROGRAM", 2);
            map.Add("START_PROGRAM", 3);
            map.Add("POSITION_CONTROL", 4);
            map.Add("VELOCITY_CONTROL", 5);
            map.Add("FORCE_CONTROL", 6);
            map.Add("SET_ACCELERATION", 7);
            map.Add("SET_VELOCITY", 8);
            map.Add("SET_SPEED", 9);
            map.Add("SET_MINIMUM_FORCE", 10);
            map.Add("SET_HOME", 11);
            map.Add("SET_POSITION", 12);

            float fVal = 0;
            int lVal = 0;
            StepType newStep = new StepType();
            try
            {
                if (dataGridViewRow.Cells[0].Value == null && dataGridViewRow.Cells[1].Value == null)
                {
                    return resultOfFunction;
                }
                else
                {
                    if (dataGridViewRow.Cells[0].Value != null && dataGridViewRow.Cells[1].Value == null)
                    {
                        resultOfFunction = -1;
                    }
                    else
                    {
                        if (dataGridViewRow.Cells[0].Value == null)
                        {
                            forms.writeNode(step.ToString(), "ns=6;s=::AsGlobalPV:Program.transfer.ActualStep");
                        }
                        else
                        {
                            forms.writeNode(dataGridViewRow.Cells[0].Value.ToString(), "ns=6;s=::AsGlobalPV:Program.transfer.ActualStep");
                        }
                    }
                    if (map.TryGetValue(dataGridViewRow.Cells[1].Value.ToString(), out command))
                    {
                        forms.writeNode(command.ToString(), "ns=6;s=::AsGlobalPV:Program.step.Command");
                        newStep.Command = (programCodes)command;
                    }
                    else
                    {
                        resultOfFunction = -1;
                    }
                    if (dataGridViewRow.Cells[2].Value != null)   //position
                    {

                        forms.writeNode(dataGridViewRow.Cells[2].Value.ToString(), "ns=6;s=::AsGlobalPV:Program.step.Position");
                        forms.writeNode(vTrue, "ns=6;s=::AsGlobalPV:Program.step.SetPosition");
                        fVal = float.Parse(dataGridViewRow.Cells[2].Value.ToString());
                        newStep.Position = fVal;
                        newStep.SetPosition = true;
                    }
                    else
                    {
                        forms.writeNode("0", "ns=6;s=::AsGlobalPV:Program.step.Position");
                        forms.writeNode(vFalse, "ns=6;s=::AsGlobalPV:Program.step.SetPosition");
                        newStep.Position = 0;
                        newStep.SetPosition = false;
                    }
                    if (dataGridViewRow.Cells[3].Value != null)   //speed
                    {
                        forms.writeNode(dataGridViewRow.Cells[3].Value.ToString(), "ns=6;s=::AsGlobalPV:Program.step.Speed");
                        forms.writeNode(vTrue, "ns=6;s=::AsGlobalPV:Program.step.SetSpeed");
                        fVal = float.Parse(dataGridViewRow.Cells[3].Value.ToString());
                        newStep.Speed = fVal;
                        newStep.SetSpeed = true;
                    }
                    else
                    {
                        forms.writeNode("0", "ns=6;s=::AsGlobalPV:Program.step.Speed");
                        forms.writeNode(vFalse, "ns=6;s=::AsGlobalPV:Program.step.SetSpeed");
                        newStep.Speed = 0;
                        newStep.SetSpeed = false;
                    }
                    if (dataGridViewRow.Cells[4].Value != null)   //force
                    {
                        forms.writeNode(dataGridViewRow.Cells[4].Value.ToString(), "ns=6;s=::AsGlobalPV:Program.step.Force");
                        forms.writeNode(vTrue, "ns=6;s=::AsGlobalPV:Program.step.SetForce");
                        fVal = float.Parse(dataGridViewRow.Cells[4].Value.ToString());
                        newStep.Force = fVal;
                        newStep.SetForce = true;
                    }
                    else
                    {
                        forms.writeNode("0", "ns=6;s=::AsGlobalPV:Program.step.Force");
                        forms.writeNode(vFalse, "ns=6;s=::AsGlobalPV:Program.step.SetForce");
                        newStep.Force = 0;
                        newStep.SetForce = false;
                    }
                    if (dataGridViewRow.Cells[5].Value != null)   //acceleration
                    {
                        forms.writeNode(dataGridViewRow.Cells[5].Value.ToString(), "ns=6;s=::AsGlobalPV:Program.step.Acceleration");
                        forms.writeNode(vTrue, "ns=6;s=::AsGlobalPV:Program.step.SetAcceleration");
                        fVal = float.Parse(dataGridViewRow.Cells[5].Value.ToString());
                        newStep.Acceleration = fVal;
                        newStep.SetAcceleration = true;
                    }
                    else
                    {
                        forms.writeNode("0", "ns=6;s=::AsGlobalPV:Program.step.Acceleration");
                        forms.writeNode(vFalse, "ns=6;s=::AsGlobalPV:Program.step.SetAcceleration");
                        newStep.Acceleration = 0;
                        newStep.SetAcceleration = false;
                    }
                    if (dataGridViewRow.Cells[6].Value != null)   //duration
                    {
                        forms.writeNode(dataGridViewRow.Cells[6].Value.ToString(), "ns=6;s=::AsGlobalPV:Program.step.Duration");
                        forms.writeNode(vTrue, "ns=6;s=::AsGlobalPV:Program.step.SetDuration");
                        lVal = int.Parse(dataGridViewRow.Cells[6].Value.ToString());
                        newStep.Duration = lVal;
                        newStep.SetDuration = true;
                    }
                    else
                    {
                        forms.writeNode("0", "ns=6;s=::AsGlobalPV:Program.step.Duration");
                        forms.writeNode(vFalse, "ns=6;s=::AsGlobalPV:Program.step.SetDuration");
                        newStep.Duration = 0;
                        newStep.SetDuration = false;
                    }
                    if (dataGridViewRow.Cells[8].Value != null)   //EndForce
                    {
                        forms.writeNode(dataGridViewRow.Cells[8].Value.ToString(), "ns=6;s=::AsGlobalPV:Program.step.EndForce");
                        forms.writeNode(vTrue, "ns=6;s=::AsGlobalPV:Program.step.SetEndForce");
                        fVal = float.Parse(dataGridViewRow.Cells[8].Value.ToString());
                        newStep.EndForce = fVal;
                        newStep.SetEndForce = true;
                    }
                    else
                    {
                        forms.writeNode("0", "ns=6;s=::AsGlobalPV:Program.step.EndForce");
                        forms.writeNode(vFalse, "ns=6;s=::AsGlobalPV:Program.step.SetEndForce");
                        newStep.EndForce = 0;
                        newStep.SetEndForce = false;
                    }





                }
                Step = newStep;

            }
            catch (Exception)
            {
                string message = "Chyba programu, řádek: " + step.ToString();
                string caption = "Chybný program";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Error);

            }

            return resultOfFunction;
        }

    }
}
