//============================================================================
//=============================================================================
// Siemens AG
// (c)Copyright (2017) All Rights Reserved
//----------------------------------------------------------------------------- 
// Tested with: Windows 10 Enterprise x64
// Engineering: Visual Studio 2013
// Functionality: Wrapps up important classes/methods of the OPC UA .NET Stack to help
// with simple client implementations
//-----------------------------------------------------------------------------
// Change log table:
// Version Date Expert in charge Changes applied
// 01.00.00 31.08.2016 (Siemens) First released version
// 01.01.00 22.02.2017 (Siemens) Implement user authentication, SHA256 Cert, Basic256Rsa256 connection,
// Basic256Rsa256 connections, read/write structs/UDTs
// 01.02.00 14.12.2017 (Siemens) Implements method calling, alarms&conditions
//=============================================================================
#define geared
//Define normal

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IdentityModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Opc.Ua;
using Opc.Ua.Client;
using Siemens.UAClientHelper;
using System.Xml.Serialization;
using System.IO;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
#if  geared

using LiveCharts.Geared;

#endif






namespace trhacka_v_1_0_working_2019_010_201
{
    public struct ProgramState
    {
        public int ActualState;
        public uint ActualStep;
        public bool Error;
        public long PassedTime;
        public long StartTime;
    }

    public struct Transfer
    {
        public bool WriteProgram;
        public bool WritingProgramDone;
        public bool WriteStep;
        public bool WritingDone;
        public int ActualStep;
        public bool ItemDone;
        public bool WriteItem;
        public int ActualItem;

    }

    public enum programCodes
    {
        STEP = 1,
        STOP_PROGRAM,
        START_PROGRAM,
        POSITION_CONTROL,
        SET_ACCELERATION,
        SET_SPEED,
        SET_VELOCITY,
        VELOCITY_CONTROL,
        SET_MINIMUM_FORCE,
        SET_HOME,
        SET_POSION,
        FORCE_CONTROL

    }

    public struct StepType
    {
        public programCodes Command;
        public float Position;
        public float Speed;
        public float Force;
        public float Acceleration;
        public Int32 Duration;
        public Int32 Passed;
        public float EndForce;
        public bool SetPosition;
        public bool SetSpeed;
        public bool SetForce;
        public bool SetHome;
        public bool SetZeroPosition;
        public bool SetEndForce;
        public bool SetAcceleration;
        public bool SetDuration;

        public StepType(programCodes code)
        {
            Command = code;
            Position = 0;
            Speed = 0;
            Force = 0;
            Acceleration = 0;
            Duration = 0;
            Passed = 0;
            EndForce = 0;
            SetPosition = false;
            SetSpeed = false;
            SetForce = false;
            SetHome = false;
            SetZeroPosition = false;
            SetEndForce = false;
            SetAcceleration = false;
            SetDuration = false;

        }




    }


    public partial class UAClientForm : Form
    {

        private Single global_PVMachineControlpositionControlinputSetValue;
        /// <summary>
        /// Fields
        /// </summary>
#region Fields
        private Session mySession;
        private Subscription mySubscription, mySubscriptionFast, mySubscriptionProgram, mySubscriptionFiltersAndRamps, mySubscriptionConstants;
        //public UAClientHelperAPI myClientHelperAPI;
        private EndpointDescription mySelectedEndpoint;
        private MonitoredItem myMonitoredItem;
        private List<MonitoredItem> myMonitoredItems;
        private List<String> myRegisteredNodeIdStrings;
        private ReferenceDescriptionCollection myReferenceDescriptionCollection;
        private List<string[]> myStructList;
        private UAClientCertForm myCertForm;
        private Int16 itemCount;
        private string fNameOfEndpoint = "Endpoints";
        private UAClientForm mainForm = null;
        private ChartTimeForm chartTimeForm = new ChartTimeForm();
        private ChartForcePositionForm chartForcePositionForm;
        private ChartForceVelocityForm chartForceVelocityForm;
        private ChartPID_TimeForceForm formChartPidTimeForce;
        private ChartPID_TimePositionForm formChartPidTimePosition;
        private ChartPID_TimeVelocityForm formChartPidTimeVelocity;
        private ProgramForm formProgramForm;


        private bool initializeChartVelocity = true;
        static double x = 0, y = 0;
        double lastval, lastTisk = -1;
        double val, valTick = 0;
        int numPointsInVelocityChart = 100;
        private string vTrue = "True";
        private string vFalse = "False";
        bool buttonPowerOnStateMemory = false;
        bool machineReady = false;
        bool machineError = false;
        bool machinePowerOn = false;
        bool machineIoOK = false;
        bool velocityOK = false;
        bool forceOK = false;
        bool positionOK = false;
        bool commandTaraStart = false;
        bool commandReadCellParametersHigh = false;

        bool commandLowForceStart = false;
        bool commandHighForceStart = false;
        bool commandHomeStart = false;
        static bool timerDiv = false;
        string[,] namesOfVariblesPLC = new string[,]
     {
        {
            "ns=6;s=::AsGlobalPV:MachineControl.command.control",
            "ns=6;s=::AsGlobalPV:MachineControl.command.filterOn",
            "ns=6;s=::AsGlobalPV:MachineControl.command.force",
            "ns=6;s=::AsGlobalPV:MachineControl.command.position",
            "ns=6;s=::AsGlobalPV:MachineControl.command.powerOn",
            "ns=6;s=::AsGlobalPV:MachineControl.command.programControl",
            "ns=6;s=::AsGlobalPV:MachineControl.command.programming",
            "ns=6;s=::AsGlobalPV:MachineControl.command.rampOn",
            "ns=6;s=::AsGlobalPV:MachineControl.command.restart",
            "ns=6;s=::AsGlobalPV:MachineControl.command.manual.downFast",
            "ns=6;s=::AsGlobalPV:MachineControl.command.manual.downLow",
            "ns=6;s=::AsGlobalPV:MachineControl.command.manual.manual",
            "ns=6;s=::AsGlobalPV:MachineControl.command.manual.upFast",
            "ns=6;s=::AsGlobalPV:MachineControl.command.manual.upLow",
            "ns=6;s=::AsGlobalPV:MachineControl.command.position",
            "ns=6;s=::AsGlobalPV:MachineControl.command.powerOn",
            "ns=6;s=::AsGlobalPV:MachineControl.command.programControl",
            "ns=6;s=::AsGlobalPV:MachineControl.command.programming",
            "ns=6;s=::AsGlobalPV:MachineControl.command.rampOn",
            "ns=6;s=::AsGlobalPV:MachineControl.command.restart",
            "ns=6;s=::AsGlobalPV:MachineControl.command.runTuning",
            "ns=6;s=::AsGlobalPV:MachineControl.command.simulate",
            "ns=6;s=::AsGlobalPV:MachineControl.command.tara",
            "ns=6;s=::AsGlobalPV:MachineControl.command.velocity",
            "ns=6;s=::AsGlobalPV:MachineControl.counterControl.commands.SetZeroPosition",
            "ns=6;s=::AsGlobalPV:MachineControl.counterControl.commands.setHommePosition",
            "ns=6;s=::AsGlobalPV:MachineControl.counterControl.outputs.homePosition",
            "ns=6;s=::AsGlobalPV:MachineControl.counterControl.status.doneHommePosition",
            "ns=6;s=::AsGlobalPV:MachineControl.counterControl.status.doneZeroPosition",
            "ns=6;s=::AsGlobalPV:MachineControl.counterControl.status.ready",
            "ns=6;s=::AsGlobalPV:MachineControl.input.position.ActValue",
            "ns=6;s=::AsGlobalPV:MachineControl.input.position.SetValue",
            "ns=6;s=::AsGlobalPV:MachineControl.input.strain.actValue",
            "ns=6;s=::AsGlobalPV:MachineControl.input.strain.setValue",
            "ns=6;s=::AsGlobalPV:MachineControl.input.velocity.ActValue",
            "ns=6;s=::AsGlobalPV:MachineControl.input.velocity.SetValue",
            "ns=6;s=::AsGlobalPV:MachineControl.input.velocity.ingValue",
            "ns=6;s=::AsGlobalPV:MachineControl.output.position.IntegrationHold",
            "ns=6;s=::AsGlobalPV:MachineControl.output.position.active",
            "ns=6;s=::AsGlobalPV:MachineControl.output.position.error",
            "ns=6;s=::AsGlobalPV:MachineControl.output.position.trackingActive",
            "ns=6;s=::AsGlobalPV:MachineControl.output.position.value",
            "ns=6;s=::AsGlobalPV:MachineControl.output.strain.IntegrationHold",
            "ns=6;s=::AsGlobalPV:MachineControl.output.strain.active",
            "ns=6;s=::AsGlobalPV:MachineControl.output.strain.error",
            "ns=6;s=::AsGlobalPV:MachineControl.output.strain.value",
            "ns=6;s=::AsGlobalPV:MachineControl.output.strain.value",
            "ns=6;s=::AsGlobalPV:MachineControl.output.velocity.IntegrationHold",
            "ns=6;s=::AsGlobalPV:MachineControl.output.velocity.active",
            "ns=6;s=::AsGlobalPV:MachineControl.output.velocity.error",
            "ns=6;s=::AsGlobalPV:MachineControl.output.velocity.trackingActive",
            "ns=6;s=::AsGlobalPV:MachineControl.output.velocity.value",

            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.control",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.filterOn",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.rampOn",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.runTuning",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.simulate",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.manPID",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.runPID",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.setFilter",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.setRamp",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.readFilter",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.readRamp",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.setMaxVelocity",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.setZero",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.Home",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.MinPosition",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.MaxPosition",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.MaxVelocity",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.SetMaxPosition",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.SeMinPosition",

            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.input.ActValue",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.input.SetValue",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.output.active",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.output.error",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.output.value",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.status.FilterActive",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.status.PIDActive",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.status.RampActive",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.status.State",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.status.TuningActive",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.status.control",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.status.error",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.status.initialized",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.status.manual",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.status.ready",
            "ns=6;s=::AsGlobalPV:MachineControl.positionControl.status.simulating",
            "ns=6;s=::AsGlobalPV:MachineControl.status.error",
            "ns=6;s=::AsGlobalPV:MachineControl.status.filterOn",
            "ns=6;s=::AsGlobalPV:MachineControl.status.force",
            "ns=6;s=::AsGlobalPV:MachineControl.status.ioOK",
            "ns=6;s=::AsGlobalPV:MachineControl.status.manual.downFast",
            "ns=6;s=::AsGlobalPV:MachineControl.status.manual.downLow",
            "ns=6;s=::AsGlobalPV:MachineControl.status.manual.manual",
            "ns=6;s=::AsGlobalPV:MachineControl.status.manual.upFast",
            "ns=6;s=::AsGlobalPV:MachineControl.status.manual.upLow",
            "ns=6;s=::AsGlobalPV:MachineControl.status.position",
            "ns=6;s=::AsGlobalPV:MachineControl.status.powerOn",
            "ns=6;s=::AsGlobalPV:MachineControl.status.programming",
            "ns=6;s=::AsGlobalPV:MachineControl.status.programControl",
            "ns=6;s=::AsGlobalPV:MachineControl.status.rampOn",
            "ns=6;s=::AsGlobalPV:MachineControl.status.ready",
            "ns=6;s=::AsGlobalPV:MachineControl.status.state",
            "ns=6;s=::AsGlobalPV:MachineControl.status.tara",
            "ns=6;s=::AsGlobalPV:MachineControl.status.tuningActive",
            "ns=6;s=::AsGlobalPV:MachineControl.status.velocity",
            "ns=6;s=::AsGlobalPV:MachineControl.status.standartizeWeight1",
            "ns=6;s=::AsGlobalPV:MachineControl.status.standartizeWeight2",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.control",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.filterOn",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.rampOn",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.runPID",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.runTuning",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.simulate",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.manPID",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.runPID",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.setFilter",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.setRamp",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.readFilter",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.readRamp",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.maxVelocity",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.stopMinForce",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.setStopMinForce",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.setMaxVelocity",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.doStandartize1",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.doStandartize2",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.doTare",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.getParameters",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.getParameters1",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.getParameters2",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.writeToMemory",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.writeToMemory1",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.writeToMemory2",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.status.doneStandartize1",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.status.doneStandartize2",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.status.doneTare",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.status.enable",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.status.error",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.status.ready",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.status.actualCellType",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.standartizedValue1",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.standartizedValue2",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.ref1_raw",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.ref1_std",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.ref2_raw",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.ref2_std",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.tare_std",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.ref1_raw",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.ref1_std",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.ref2_raw",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.ref2_std",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.tare_std",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.scaleParameter.ref1_raw",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.scaleParameter.ref1_std",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.scaleParameter.ref2_raw",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.scaleParameter.ref2_std",
            //"ns=6;s=::AsGlobalPV:constMachineControl.weight.scaleParameter.tare_std",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.scaleParameter.ref1_raw",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.scaleParameter.ref1_std",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.scaleParameter.ref2_raw",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.scaleParameter.ref2_std",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.scaleParameter.tare_std",

            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.parameterLoadCell1.ref1_raw",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.parameterLoadCell1.ref1_std",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.parameterLoadCell1.ref2_raw",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.parameterLoadCell1.ref2_std",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.parameterLoadCell1.tare_std",

            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.parameterLoadCell2.ref1_raw",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.parameterLoadCell2.ref1_std",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.parameterLoadCell2.ref2_raw",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.parameterLoadCell2.ref2_std",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.parameterLoadCell2.tare_std",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.input.actValue",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.input.setValue",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.output.active",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.output.error",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.output.trackingActive",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.output.value",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.status.FilterActive",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.status.PIDActive",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.status.RampActive",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.status.State",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.status.TuningActive",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.status.control",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.status.error",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.status.initialized",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.status.manual",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.status.ready",
            "ns=6;s=::AsGlobalPV:MachineControl.strainControl.status.simulating",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.control",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.filterOn",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.initialize",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.rampOn",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.runTuning",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.simulate",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.runPID",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.rampOn",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.filterOn",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.setRamp",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.setFilter",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.raedFilter",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.readRamp",

            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.input.ActValue",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.input.SetValue",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.input.ingValue",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.output.IntegrationHold",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.output.active",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.output.trackingActive",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.output.value",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.status.FilterActive",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.status.PIDActive",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.status.RampActive",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.status.State",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.status.TuningActive",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.status.control",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.status.error",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.status.initialized",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.status.manual",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.status.ready",
            "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.status.simulating",
            "ns=6;s=::AsGlobalPV:constMachineControl.strain.rampSettings.MinOut",
            "ns=6;s=::AsGlobalPV:constMachineControl.strain.rampSettings.MaxOut",
            "ns=6;s=::AsGlobalPV:constMachineControl.strain.rampSettings.MaxPosSlewRate",
            "ns=6;s=::AsGlobalPV:constMachineControl.strain.rampSettings.MaxNegSlewRate",
            "ns=6;s=::AsGlobalPV:constMachineControl.strain.filterSettings.Type",
            "ns=6;s=::AsGlobalPV:constMachineControl.strain.filterSettings.Order",
            "ns=6;s=::AsGlobalPV:constMachineControl.strain.filterSettings.CutOffFrequency",
            "ns=6;s=::AsGlobalPV:constMachineControl.position.rampSettings.MinOut",
            "ns=6;s=::AsGlobalPV:constMachineControl.position.rampSettings.MaxOut",
            "ns=6;s=::AsGlobalPV:constMachineControl.position.rampSettings.MaxPosSlewRate",
            "ns=6;s=::AsGlobalPV:constMachineControl.position.rampSettings.MaxNegSlewRate",
            "ns=6;s=::AsGlobalPV:constMachineControl.position.filterSettings.Type",
            "ns=6;s=::AsGlobalPV:constMachineControl.position.filterSettings.Order",
            "ns=6;s=::AsGlobalPV:constMachineControl.position.filterSettings.CutOffFrequency",
            "ns=6;s=::AsGlobalPV:constMachineControl.velocity.rampSettings.MinOut",
            "ns=6;s=::AsGlobalPV:constMachineControl.velocity.rampSettings.MaxOut",
            "ns=6;s=::AsGlobalPV:constMachineControl.velocity.rampSettings.MaxPosSlewRate",
            "ns=6;s=::AsGlobalPV:constMachineControl.velocity.rampSettings.MaxNegSlewRate",
            "ns=6;s=::AsGlobalPV:constMachineControl.velocity.filterSettings.Type",
            "ns=6;s=::AsGlobalPV:constMachineControl.velocity.filterSettings.Order",
            "ns=6;s=::AsGlobalPV:constMachineControl.velocity.filterSettings.CutOffFrequency",

        },
        {
            "MachineControl_command_control",
            "MachineControl_command_filterOn",
            "MachineControl_command_force",
            "MachineControl_command_position",
            "MachineControl_command_powerOn",
            "MachineControl_command_programControl",
            "MachineControl_command_programming",
            "MachineControl_command_rampOn",
            "MachineControl_command_restart",
            "MachineControl_command_manual_downFast",
            "MachineControl_command_manual_downLow",
            "MachineControl_command_manual_manual",
            "MachineControl_command_manual_upFast",
            "MachineControl_command_manual_upLow",
            "MachineControl_command_position",
            "MachineControl_command_powerOn",
            "MachineControl_command_programControl",
            "MachineControl_command_programming",
            "MachineControl_command_rampOn",
            "MachineControl_command_restart",
            "MachineControl_command_runTuning",
            "MachineControl_command_simulate",
            "MachineControl_command_tara",
            "MachineControl_command_velocity",
            "MachineControl_counterControl_commands_SetZeroPosition",
            "MachineControl_counterControl_commands_setHommePosition",
            "MachineControl_counterControl_outputs_homePosition",
            "MachineControl_counterControl_status_doneHommePosition",
            "MachineControl_counterControl_status_doneZeroPosition",
            "MachineControl_counterControl_status_ready",
            "MachineControl_input_position_ActValue",
            "MachineControl_input_position_SetValue",
            "MachineControl_input_strain_actValue",
            "MachineControl_input_strain_setValue",
            "MachineControl_input_velocity_ActValue",
            "MachineControl_input_velocity_SetValue",
            "MachineControl_input_velocity_ingValue",
            "MachineControl_output_position_IntegrationHold",
            "MachineControl_output_position_active",
            "MachineControl_output_position_error",
            "MachineControl_output_position_trackingActive",
            "MachineControl_output_position_value",
            "MachineControl_output_strain_IntegrationHold",
            "MachineControl_output_strain_active",
            "MachineControl_output_strain_error",
            "MachineControl_output_strain_value",
            "MachineControl_output_strain_value",
            "MachineControl_output_velocity_IntegrationHold",
            "MachineControl_output_velocity_active",
            "MachineControl_output_velocity_error",
            "MachineControl_output_velocity_trackingActive",
            "MachineControl_output_velocity_value",
            "MachineControl_positionControl_command_control",
            "MachineControl_positionControl_command_filterOn",
            "MachineControl_positionControl_command_rampOn",
            "MachineControl_positionControl_command_runTuning",
            "MachineControl_positionControl_command_simulate",
            "MachineControl_positionControl_command_manPID",
            "MachineControl_positionControl_command_runPID",
            "MachineControl.positionControl.command.setFilter",
            "MachineControl.positionControl.command.setRamp",
            "MachineControl.positionControl.command.readFilter",
            "MachineControl.positionControl.command.readRamp",
            "MachineControl.positionControl.command.setMaxVelocity",
            "MachineControl.positionControl.command.setZero",
            "MachineControl.positionControl.command.Home",
            "MachineControl.positionControl.command.MinPosition",
            "MachineControl.positionControl.command.MaxPosition",
            "MachineControl.positionControl.command.MaxVelocity",
            "MachineControl.positionControl.command.SetMaxPosition",
            "MachineControl.positionControl.command.SeMinPosition",
             "MachineControl_positionControl_input_ActValue",
            "MachineControl_positionControl_input_SetValue",
            "MachineControl_positionControl_output_active",
            "MachineControl_positionControl_output_error",
            "MachineControl_positionControl_output_value",
            "MachineControl_positionControl_status_FilterActive",
            "MachineControl_positionControl_status_PIDActive",
            "MachineControl_positionControl_status_RampActive",
            "MachineControl_positionControl_status_State",
            "MachineControl_positionControl_status_TuningActive",
            "MachineControl_positionControl_status_control",
            "MachineControl_positionControl_status_error",
            "MachineControl_positionControl_status_initialized",
            "MachineControl_positionControl_status_manual",
            "MachineControl_positionControl_status_ready",
            "MachineControl_positionControl_status_simulating",
            "MachineControl_status_error",
            "MachineControl_status_filterOn",
            "MachineControl_status_force",
            "MachineControl_status_ioOK",
            "MachineControl_status_manual_downFast",
            "MachineControl_status_manual_downLow",
            "MachineControl_status_manual_manual",
            "MachineControl_status_manual_upFast",
            "MachineControl_status_manual_upLow",
            "MachineControl_status_position",
            "MachineControl_status_powerOn",
            "MachineControl_status_programming",
            "MachineControl_status_programControl",
            "MachineControl_status_rampOn",
            "MachineControl_status_ready",
            "MachineControl_status_state",
            "MachineControl_status_tara",
            "MachineControl_status_tuningActive",
            "MachineControl_status_velocity",
            "MachineControl_status_standartizeWeight1",
            "MachineControl_status_standartizeWeight2",
            "MachineControl_strainControl_command_control",
            "MachineControl_strainControl_command_filterOn",
            "MachineControl_strainControl_command_rampOn",
            "MachineControl_strainControl_command_runPID",
            "MachineControl_strainControl_command_runTuning",
            "MachineControl_strainControl_command_simulate",
            "MachineControl_strainControl_command_manPID",
            "MachineControl_strainControl_command_runPID",
            "MachineControl_strainControl_command_setFilter",
            "MachineControl_strainControl_command_setRamp",
            "MachineControl_strainControl_command_readFilter",
            "MachineControl_strainControl_command_readRamp",
            "MachineControl_strainControl_command_maxVelocity",
            "MachineControl_strainControl_stopMinForce",
            "MachineControl_strainControl_command_setStopMinForce",
            "MachineControl_strainControl_command_setMaxVelocity",

            "MachineControl_strainControl_controlWeight_command_doStandartize1",
            "MachineControl_strainControl_controlWeight_command_doStandartize2",
            "MachineControl_strainControl_controlWeight_command_doTare",
            "MachineControl_strainControl_controlWeight_command_getParameters",
            "MachineControl_strainControl_controlWeight_command_getParameters1",
            "MachineControl_strainControl_controlWeight_command_getParameters2",
            "MachineControl_strainControl_controlWeight_command_writeToMemory",
            "MachineControl_strainControl_controlWeight_command_writeToMemory1",
            "MachineControl_strainControl_controlWeight_command_writeToMemory2",
            "MachineControl_strainControl_controlWeight_status_doneStandartize1",
            "MachineControl_strainControl_controlWeight_status_doneStandartize2",
            "MachineControl_strainControl_controlWeight_status_doneTare",
            "MachineControl_strainControl_controlWeight_status_enable",
            "MachineControl_strainControl_controlWeight_status_error",
            "MachineControl_strainControl_controlWeight_status_ready",
            "MachineControl_strainControl_controlWeight_status_actualCellType",
            "MachineControl_strainControl_controlWeight_parameter_standartizedValue1",
            "MachineControl_strainControl_controlWeight_parameter_standartizedValue2",

            //"constMachineControl_weight_parameterLoadCell1_ref1_raw",
            //"constMachineControl_weight_parameterLoadCell1_ref1_std",
            //"constMachineControl_weight_parameterLoadCell1_ref2_raw",
            //"constMachineControl_weight_parameterLoadCell1_ref2_std",
            //"constMachineControl_weight_parameterLoadCell1_tare_std",
            //"constMachineControl_weight_parameterLoadCell2_ref1_raw",
            //"constMachineControl_weight_parameterLoadCell2_ref1_std",
            //"constMachineControl_weight_parameterLoadCell2_ref2_raw",
            //"constMachineControl_weight_parameterLoadCell2_ref2_std",
            //"constMachineControl_weight_parameterLoadCell2_tare_std",
            //"constMachineControl_weight_scaleParameter_ref1_raw",
            //"constMachineControl_weight_scaleParameter_ref1_std",
            //"constMachineControl_weight_scaleParameter_ref2_raw",
            //"constMachineControl_weight_scaleParameter_ref2_std",
            //"constMachineControl_weight_scaleParameter_tare_std",
            "MachineControl_strainControl_controlWeight_parameter_scaleParameter_ref1_raw",
            "MachineControl_strainControl_controlWeight_parameter_scaleParameter_ref1_std",
            "MachineControl_strainControl_controlWeight_parameter_scaleParameter_ref2_raw",
            "MachineControl_strainControl_controlWeight_parameter_scaleParameter_ref2_std",
            "MachineControl_strainControl_controlWeight_parameter_scaleParameter_tare_std",

            "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell1_ref1_raw",
            "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell1_ref1_std",
            "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell1_ref2_raw",
            "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell1_ref2_std",
            "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell1_tare_std",

            "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell2_ref1_raw",
            "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell2_ref1_std",
            "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell2_ref2_raw",
            "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell2_ref2_std",
            "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell2_tare_std",
            "MachineControl_strainControl_input_actValue",
            "MachineControl_strainControl_input_setValue",
            "MachineControl_strainControl_output_active",
            "MachineControl_strainControl_output_error",
            "MachineControl_strainControl_output_trackingActive",
            "MachineControl_strainControl_output_value",
            "MachineControl_strainControl_status_FilterActive",
            "MachineControl_strainControl_status_PIDActive",
            "MachineControl_strainControl_status_RampActive",
            "MachineControl_strainControl_status_State",
            "MachineControl_strainControl_status_TuningActive",
            "MachineControl_strainControl_status_control",
            "MachineControl_strainControl_status_error",
            "MachineControl_strainControl_status_initialized",
            "MachineControl_strainControl_status_manual",
            "MachineControl_strainControl_status_ready",
            "MachineControl_strainControl_status_simulating",
            "MachineControl_velocityControl_command_control",
            "MachineControl_velocityControl_command_filterOn",
            "MachineControl_velocityControl_command_initialize",
            "MachineControl_velocityControl_command_rampOn",
            "MachineControl_velocityControl_command_runTuning",
            "MachineControl_velocityControl_command_simulate",
            "MachineControl_velocityControl_command_manPID",
            "MachineControl_velocityControl_command_runPID",
            "MachineControl_velocityControl_command_rampOn",
            "MachineControl_velocityControl_command_filterOn",
            "MachineControl_velocityControl_command_setRamp",
            "MachineControl_velocityControl_command_setFilter",
            "MachineControl_velocityControl_command_raedFilter",
            "MachineControl_velocityControl_command_readRamp",
            "MachineControl_velocityControl_input_ActValue",
            "MachineControl_velocityControl_input_SetValue",
            "MachineControl_velocityControl_input_ingValue",
            "MachineControl_velocityControl_output_IntegrationHold",
            "MachineControl_velocityControl_output_active",
            "MachineControl_velocityControl_output_trackingActive",
            "MachineControl_velocityControl_output_value",
            "MachineControl_velocityControl_status_FilterActive",
            "MachineControl_velocityControl_status_PIDActive",
            "MachineControl_velocityControl_status_RampActive",
            "MachineControl_velocityControl_status_State",
            "MachineControl_velocityControl_status_TuningActive",
            "MachineControl_velocityControl_status_control",
            "MachineControl_velocityControl_status_error",
            "MachineControl_velocityControl_status_initialized",
            "MachineControl_velocityControl_status_manual",
            "MachineControl_velocityControl_status_ready",
            "MachineControl_velocityControl_status_simulating",
            "constMachineControl_strain_rampSettings_MinOut",
            "constMachineControl_strain_rampSettings_MaxOut",
            "constMachineControl_strain_rampSettings_MaxPosSlewRate",
            "constMachineControl_strain_rampSettings_MinPosSlewRate",
            "constMachineControl_strain_filterSettings_Type",
            "constMachineControl_strain_filterSettings_Order",
            "constMachineControl_strain_filterSettings_CutOffFrequency",
            "constMachineControl_position_rampSettings_MinOut",
            "constMachineControl_position_rampSettings_MaxOut",
            "constMachineControl_position_rampSettings_MaxPosSlewRate",
            "constMachineControl_position_rampSettings_MinPosSlewRate",
            "constMachineControl_position_filterSettings_Type",
            "constMachineControl_position_filterSettings_Order",
            "constMachineControl_position_filterSettings_CutOffFrequency",
            "constMachineControl_velocity_rampSettings_MinOut",
            "constMachineControl_velocity_rampSettings_MaxOut",
            "constMachineControl_velocity_rampSettings_MaxPosSlewRate",
            "constMachineControl_velocity_rampSettings_MinPosSlewRate",
            "constMachineControl_velocity_filterSettings_Type",
            "constMachineControl_velocity_filterSettings_Order",
            "constMachineControl_velocity_filterSettings_CutOffFrequency"


        }
     };

        string[,] namesOfVariblesPLCFast = new string[,]
         {
            {
                "ns=6;s=::AsGlobalPV:opcPositionFast.ingValue",
                "ns=6;s=::AsGlobalPV:opcPositionFast.timeStamp",
                "ns=6;s=::AsGlobalPV:opcStrainFast.ingValue",
                "ns=6;s=::AsGlobalPV:opcStrainFast.timeStamp",
                "ns=6;s=::AsGlobalPV:opcVlocityFast.ingValue",
                "ns=6;s=::AsGlobalPV:opcVlocityFast.timeStamp"

            },
            {
                "opcPositionFast_ingValue",
                "opcPositionFast_timeStamp",
                "opcStrainFast_ingValue",
                "opcStrainFast_timeStamp",
                "opcVlocityFast_ingValue",
                "opcVlocityFast_timeStamp"
            }
         };
        //public string[,] namesOfVariblesPLCConst = new string[,]
        //{
        //    {
        //        "ns=6;s=::AsGlobalPV:constMachineControl.weight.refRaw1",
        //        "ns=6;s=::AsGlobalPV:constMachineControl.weight.refRaw2",
        //        "ns=6;s=::AsGlobalPV:constMachineControl.weight.scaleParameter.ref1_raw",
        //        "ns=6;s=::AsGlobalPV:constMachineControl.weight.scaleParameter.ref2_raw",
        //        "ns=6;s=::AsGlobalPV:constMachineControl.weight.scaleParameter.ref1_std",
        //        "ns=6;s=::AsGlobalPV:constMachineControl.weight.scaleParameter.tare_std"
        //    },
        //    {
        //        "constMachineControl_weight_refRaw1",
        //        "constMachineControl_weight_refRaw2",
        //        "constMachineControl_weight_scaleParameter_ref1_raw",
        //        "constMachineControl_weight_scaleParameter_ref2_raw",
        //        "constMachineControl_weight_scaleParameter_ref1_std",
        //        "constMachineControl_weight_scaleParameter_tare_std" }
        //};

        public string[,] namesOfVariblesPLCPIDVelocity = new string[,]
            {
                {
                    "ns=6;s=::VelocitySt:PID.ActValue",
                    "ns=6;s=::VelocitySt:PID.ControlError",
                    "ns=6;s=::VelocitySt:PID.Error",
                    "ns=6;s=::VelocitySt:PID.Out",
                    "ns=6;s=::VelocitySt:PID.ProportionalPart",
                    "ns=6;s=::VelocitySt:PID.SetValue",
                    "ns=6;s=::VelocitySt:PID.DerivativePart",
                    "ns=6;s=::VelocitySt:PID.IntegrationPart" },
                {
                    "VelocitySt_PID_ActValue",
                    "VelocitySt_PID_ControlError",
                    "VelocitySt_PID_Error",
                    "VelocitySt_PID_Out",
                    "VelocitySt_PID_ProportionalPart",
                    "VelocitySt_PID_SetValue",
                    "VelocitySt_PID_DerivativePart",
                    "VelocitySt_PID_IntegrationPart"
                }
            };

        public string[,] namesOfVariblesPLCPidPosition = new string[,]
        {
            {
                "ns=6;s=::PositionSt:PID.ActValue",
                "ns=6;s=::PositionSt:PID.ControlError",
                "ns=6;s=::PositionSt:PID.Error",
                "ns=6;s=::PositionSt:PID.DerivativePart",
                "ns=6;s=::PositionSt:PID.IntegrationPart",
                "ns=6;s=::PositionSt:PID.Out",
                "ns=6;s=::PositionSt:PID.ProportionalPart",
                "ns=6;s=::PositionSt:PID.SetValue" },
            {
                "PositionSt_PID_ActValue",
                "PositionSt_PID_ControlError",
                "PositionSt_PID_Error",
                "PositionSt_PID_DerivativePart",
                "PositionSt_PID_IntegrationPart",
                "PositionSt_PID_Out",
                "PositionSt_PID_ProportionalPart",
                "PositionSt_PID_ProportionalPart"
            }
        };

        public string[,] namesOfVariblesPLCPIDStrain = new string[,]
        {
            {
                "ns=6;s=::StrainStat:PID.ActValue",
                "ns=6;s=::StrainStat:PID.ControlError",
                "ns=6;s=::StrainStat:PID.DerivativePart",
                "ns=6;s=::StrainStat:PID.Error",
                "ns=6;s=::StrainStat:PID.IntegrationPart",
                "ns=6;s=::StrainStat:PID.Out",
                "ns=6;s=::StrainStat:PID.ProportionalPart",
                "ns=6;s=::StrainStat:PID.SetValue"
            },
            {
                "StrainStat_PID_ActValue",
                "StrainStat_PID_ControlError",
                "StrainStat_PID_DerivativePart",
                "StrainStat_PID_Error",
                "StrainStat_PID_IntegrationPart",
                "StrainStat_PID_Out",
                "StrainStat_PID_ProportionalPart",
                "StrainStat_PID_SetValue"
            }
        };

        public string[,] namesOfVariblesPLCProgram = new string[,]
{
            {
                "ns=6;s=::AsGlobalPV:Program.state.ActualState",
                "ns=6;s=::AsGlobalPV:Program.state.ActualStep",
                "ns=6;s=::AsGlobalPV:Program.state.Error",
                "ns=6;s=::AsGlobalPV:Program.state.PassedTime",
                "ns=6;s=::AsGlobalPV:Program.state.StartTime",
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
                "ns=6;s=::AsGlobalPV:Program.step.SetDuration",
                "ns=6;s=::AsGlobalPV:Program.step.Speed",
                "ns=6;s=::AsGlobalPV:Program.transfer.ActualItem",
                "ns=6;s=::AsGlobalPV:Program.transfer.ActualStep",
                "ns=6;s=::AsGlobalPV:Program.transfer.ItemDone",
                "ns=6;s=::AsGlobalPV:Program.transfer.WriteItem",
                "ns=6;s=::AsGlobalPV:Program.transfer.WriteProgram",
                "ns=6;s=::AsGlobalPV:Program.transfer.WriteStep",
                "ns=6;s=::AsGlobalPV:Program.transfer.WritingDone",
                "ns=6;s=::AsGlobalPV:Program.transfer.WritingPorgramDone"
            },
            {
                "Program_state_ActualState",
                "Program_state_ActualStep",
                "Program_state_Error",
                "Program_state_PassedTime",
                "Program_state_StartTime",
                "Program_step_Acceleration",
                "Program_step_Command",
                "Program_step_Duration",
                "Program_step_EndForce",
                "Program_step_Force",
                "Program_step_Passed",
                "Program_step_Position",
                "Program_step_SetAcceleration",
                "Program_step_SetEndForce",
                "Program_step_SetForce",
                "Program_step_SetHome",
                "Program_step_SetPosition",
                "Program_step_SetSpeed",
                "Program_step_SetZeroPosition",
                "Program_step_SetDuration",
                "Program_step_Speed",
                "Program_transfer_ActualItem",
                "Program_transfer_ActualStep",
                "Program_transfer_ItemDone",
                "Program_transfer_WriteItem",
                "Program_transfer_WriteProgram",
                "Program_transfer_WriteStep",
                "Program_transfer_WritingDone",
                "Program_transfer_WritingPorgramDone"
            }
};

        string[,] namesOfFiltersRamps = new string[,]
        {
            {
                "ns=6;s=::PositionSt:FilterLowPass.Active"    ,
                "ns=6;s=::PositionSt:FilterLowPass.CutOffFrequency" ,
                "ns=6;s=::PositionSt:FilterLowPass.Enable"  ,
                "ns=6;s=::PositionSt:FilterLowPass.Error"   ,
                "ns=6;s=::PositionSt:FilterLowPass.Order"   ,
                "ns=6;s=::PositionSt:FilterLowPass.Type"    ,
                "ns=6;s=::PositionSt:FilterLowPass.Update"  ,
                "ns=6;s=::PositionSt:FilterLowPass.UpdateDone"  ,
                "ns=6;s=::PositionSt:Ramp.Active"   ,
                "ns=6;s=::PositionSt:Ramp.Enable"   ,
                "ns=6;s=::PositionSt:Ramp.Error"    ,
                "ns=6;s=::PositionSt:Ramp.MaxNegSlewRate"   ,
                "ns=6;s=::PositionSt:Ramp.MaxOut"   ,
                "ns=6;s=::PositionSt:Ramp.MaxPosSlewRate"   ,
                "ns=6;s=::PositionSt:Ramp.MinOut"   ,
                "ns=6;s=::PositionSt:Ramp.Update"   ,
                "ns=6;s=::PositionSt:Ramp.UpdateDone"   ,
                "ns=6;s=::PositionSt:constFilter.CutOffFrequency"   ,
                "ns=6;s=::PositionSt:constFilter.Order" ,
                "ns=6;s=::PositionSt:constRamp.MaxNegSlewRate"  ,
                "ns=6;s=::PositionSt:constRamp.MaxOut"  ,
                "ns=6;s=::PositionSt:constRamp.MaxPosSlewRate"  ,
                "ns=6;s=::PositionSt:constRamp.MinOut"  ,
                "ns=6;s=::StrainStat:FilterLowPass.Active"  ,
                "ns=6;s=::StrainStat:FilterLowPass.CutOffFrequency" ,
                "ns=6;s=::StrainStat:FilterLowPass.Enable"  ,
                "ns=6;s=::StrainStat:FilterLowPass.Error"   ,
                "ns=6;s=::StrainStat:FilterLowPass.Order"   ,
                "ns=6;s=::StrainStat:FilterLowPass.Type"    ,
                "ns=6;s=::StrainStat:FilterLowPass.Update"  ,
                "ns=6;s=::StrainStat:FilterLowPass.UpdateDone"  ,
                "ns=6;s=::StrainStat:Ramp.Active"   ,
                "ns=6;s=::StrainStat:Ramp.Enable"   ,
                "ns=6;s=::StrainStat:Ramp.Error"    ,
                "ns=6;s=::StrainStat:Ramp.MaxNegSlewRate"   ,
                "ns=6;s=::StrainStat:Ramp.MaxOut"   ,
                "ns=6;s=::StrainStat:Ramp.MaxPosSlewRate"   ,
                "ns=6;s=::StrainStat:Ramp.MinOut"   ,
                "ns=6;s=::StrainStat:Ramp.Update"   ,
                "ns=6;s=::StrainStat:Ramp.UpdateDone"   ,
                "ns=6;s=::StrainStat:constFilter.CutOffFrequency"   ,
                "ns=6;s=::StrainStat:constFilter.Order" ,
                "ns=6;s=::StrainStat:constRamp.MaxNegSlewRate"  ,
                "ns=6;s=::StrainStat:constRamp.MaxOut"  ,
                "ns=6;s=::StrainStat:constRamp.MaxPosSlewRate"  ,
                "ns=6;s=::StrainStat:constRamp.MinOut"  ,
                "ns=6;s=::VelocitySt:FilterLowPass.CutOffFrequency" ,
                "ns=6;s=::VelocitySt:FilterLowPass.Enable"  ,
                "ns=6;s=::VelocitySt:FilterLowPass.Error"   ,
                "ns=6;s=::VelocitySt:FilterLowPass.Order"   ,
                "ns=6;s=::VelocitySt:FilterLowPass.Type"    ,
                "ns=6;s=::VelocitySt:FilterLowPass.Update"  ,
                "ns=6;s=::VelocitySt:FilterLowPass.UpdateDone"  ,
                "ns=6;s=::VelocitySt:Ramp.MinOut"   ,
                "ns=6;s=::VelocitySt:Ramp.Active"   ,
                "ns=6;s=::VelocitySt:Ramp.Enable"   ,
                "ns=6;s=::VelocitySt:Ramp.Error"    ,
                "ns=6;s=::VelocitySt:Ramp.MaxOut"   ,
                "ns=6;s=::VelocitySt:Ramp.MaxPosSlewRate"   ,
                "ns=6;s=::VelocitySt:Ramp.Update"   ,
                "ns=6;s=::VelocitySt:Ramp.UpdateDone"   ,
                "ns=6;s=::VelocitySt:constFilter.CutOffFrequency"   ,
                "ns=6;s=::VelocitySt:constFilter.Order" ,
                "ns=6;s=::VelocitySt:constRamp.MaxNegSlewRate"  ,
                "ns=6;s=::VelocitySt:constRamp.MaxOut"  ,
                "ns=6;s=::VelocitySt:constRamp.MaxPosSlewRate"  ,
                "ns=6;s=::VelocitySt:constRamp.MinOut"
             },
             {
                "PositionSt_FilterLowPass_Active" ,
                "PositionSt_FilterLowPass_CutOffFrequency"  ,
                "PositionSt_FilterLowPass_Enable"   ,
                "PositionSt_FilterLowPass_Error"    ,
                "PositionSt_FilterLowPass_Order"    ,
                "PositionSt_FilterLowPass_Type" ,
                "PositionSt_FilterLowPass_Update"   ,
                "PositionSt_FilterLowPass_UpdateDone"   ,
                "PositionSt_Ramp_Active"    ,
                "PositionSt_Ramp_Enable"    ,
                "PositionSt_Ramp_Error" ,
                "PositionSt_Ramp_MaxNegSlewRate"    ,
                "PositionSt_Ramp_MaxOut"    ,
                "PositionSt_Ramp_MaxPosSlewRate"    ,
                "PositionSt_Ramp_MinOut"    ,
                "PositionSt_Ramp_Update"    ,
                "PositionSt_Ramp_UpdateDone"    ,
                "PositionSt_constFilter_CutOffFrequency"    ,
                "PositionSt_constFilter_Order"  ,
                "PositionSt_constRamp_MaxNegSlewRate"   ,
                "PositionSt_constRamp_MaxNegSlewRate"   ,
                "PositionSt_constRamp_MaxPosSlewRate"   ,
                "PositionSt_constRamp_MinOut"   ,
                "StrainStat_FilterLowPass_Active"   ,
                "StrainStat_FilterLowPass_CutOffFrequency"  ,
                "StrainStat_FilterLowPass_Enable"   ,
                "StrainStat_FilterLowPass_Error"    ,
                "StrainStat_FilterLowPass_Order"    ,
                "StrainStat_FilterLowPass_Type" ,
                "StrainStat_FilterLowPass_Update"   ,
                "StrainStat_FilterLowPass_UpdateDone"   ,
                "StrainStat_Ramp_Active"    ,
                "StrainStat_Ramp_Enable"    ,
                "StrainStat_Ramp_Error" ,
                "StrainStat_Ramp_MaxNegSlewRate"    ,
                "StrainStat_Ramp_MaxOut"    ,
                "StrainStat_Ramp_MaxPosSlewRate"    ,
                "StrainStat_Ramp_MinOut"    ,
                "StrainStat_Ramp_Update"    ,
                "StrainStat_Ramp_UpdateDone"    ,
                "StrainStat_constFilter_CutOffFrequency"    ,
                "StrainStat_constFilter_Order"  ,
                "StrainStat_constRamp_MaxNegSlewRate"   ,
                "StrainStat_constRamp_MaxOut"   ,
                "StrainStat_constRamp_MaxPosSlewRate"   ,
                "StrainStat_constRamp_MinOut"   ,
                "VelocitySt_FilterLowPass_CutOffFrequency"  ,
                "VelocitySt_FilterLowPass_Enable"   ,
                "VelocitySt_FilterLowPass_Error"    ,
                "VelocitySt_FilterLowPass_Order"    ,
                "VelocitySt_FilterLowPass_Type" ,
                "VelocitySt_FilterLowPass_Update"   ,
                "VelocitySt_FilterLowPass_UpdateDone"   ,
                "VelocitySt_Ramp_MinOut"    ,
                "VelocitySt_Ramp_Active"    ,
                "VelocitySt_Ramp_Enable"    ,
                "VelocitySt_Ramp_Error" ,
                "VelocitySt_Ramp_MaxOut"    ,
                "VelocitySt_Ramp_MaxPosSlewRate"    ,
                "VelocitySt_Ramp_Update"    ,
                "VelocitySt_Ramp_UpdateDone"    ,
                "VelocitySt_constFilter_CutOffFrequency"    ,
                "VelocitySt_constFilter_Order"  ,
                "VelocitySt_constRamp_MaxNegSlewRate"   ,
                "VelocitySt_constRamp_MaxOut"   ,
                "VelocitySt_constRamp_MaxPosSlewRate"   ,
                "VelocitySt_constRamp_MinOut"

              }
        };

        string[,] namesOfConstats = new string[,]
        {
            {


            "ns=6;s=::MachineSta:initMasterTask_0.constMConrol.weight.parameterLoadCell1.ref1_raw",
            "ns=6;s=::MachineSta:initMasterTask_0.constMConrol.weight.parameterLoadCell1.ref1_std",
            "ns=6;s=::MachineSta:initMasterTask_0.constMConrol.weight.parameterLoadCell1.ref2_raw",
            "ns=6;s=::MachineSta:initMasterTask_0.constMConrol.weight.parameterLoadCell1.ref2_std",
            "ns=6;s=::MachineSta:initMasterTask_0.constMConrol.weight.parameterLoadCell1.tare_std",
            "ns=6;s=::MachineSta:initMasterTask_0.constMConrol.weight.parameterLoadCell2.ref1_raw",
            "ns=6;s=::MachineSta:initMasterTask_0.constMConrol.weight.parameterLoadCell2.ref1_std",
            "ns=6;s=::MachineSta:initMasterTask_0.constMConrol.weight.parameterLoadCell2.ref2_raw",
            "ns=6;s=::MachineSta:initMasterTask_0.constMConrol.weight.parameterLoadCell2.ref2_std",
            "ns=6;s=::MachineSta:initMasterTask_0.constMConrol.weight.parameterLoadCell2.tare_std",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.ref1_raw",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.ref1_std",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.ref2_raw",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.ref2_std",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.tare_std",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.ref1_raw",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.ref1_std",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.ref2_raw",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.ref2_std",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.tare_std",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.scaleParameter.ref1_raw",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.scaleParameter.ref1_std",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.scaleParameter.ref2_raw",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.scaleParameter.ref2_std",
            "ns=6;s=::AsGlobalPV:constMachineControl.weight.scaleParameter.tare_std"
            },
            {


            "constMConrol_weight_parameterLoadCell1_ref1_raw",
            "constMConrol_weight_parameterLoadCell1_ref1_std",
            "constMConrol_weight_parameterLoadCell1_ref2_raw",
            "constMConrol_weight_parameterLoadCell1_ref2_std",
            "constMConrol_weight_parameterLoadCell1_tare_std",
            "constMConrol_weight_parameterLoadCell2_ref1_raw",
            "constMConrol_weight_parameterLoadCell2_ref1_std",
            "constMConrol_weight_parameterLoadCell2_ref2_raw",
            "constMConrol_weight_parameterLoadCell2_ref2_std",
            "constMConrol_weight_parameterLoadCell2_tare_std",
            "constMachineControl_weight_parameterLoadCell1_ref1_raw",
            "constMachineControl_weight_parameterLoadCell1_ref1_std",
            "constMachineControl_weight_parameterLoadCell1_ref2_raw",
            "constMachineControl_weight_parameterLoadCell1_ref2_std",
            "constMachineControl_weight_parameterLoadCell1_tare_std",
            "constMachineControl_weight_parameterLoadCell2_ref1_raw",
            "constMachineControl_weight_parameterLoadCell2_ref1_std",
            "constMachineControl_weight_parameterLoadCell2_ref2_raw",
            "constMachineControl_weight_parameterLoadCell2_ref2_std",
            "constMachineControl_weight_parameterLoadCell2_tare_std",
            "constMachineControl_weight_scaleParameter_ref1_raw",
            "constMachineControl_weight_scaleParameter_ref1_std",
            "constMachineControl_weight_scaleParameter_ref2_raw",
            "constMachineControl_weight_scaleParameter_ref2_std",
            "constMachineControl_weight_scaleParameter_tare_std"

            }
        };

        public Transfer transfer = new Transfer();
        public ProgramState programState = new ProgramState();
        public StepType stepType = new StepType();
        Programming programming;
        private readonly object PositionSt_FilterLowPass_Enable;


        #endregion


        /// <summary>               
        /// Form Construction
        /// </summary>
        #region Construction
        public UAClientForm()
        {

            mainForm = this;
            InitializeComponent();
            programming = new Programming(this);
            //myClientHelperAPI = new UAClientHelperAPI();
            myRegisteredNodeIdStrings = new List<String>();
            browsePage.Enabled = false;
            rwPage.Enabled = false;
            subscribePage.Enabled = false;
            structPage.Enabled = false;
            methodPage.Enabled = false;
            itemCount = 0;



            //ChartsWrite writer = new ChartsWrite();
            //writer.IsReading = false;
            //writer.Read();



            chartTimeForm = new ChartTimeForm(this);
            chartForcePositionForm = new ChartForcePositionForm(this);
            chartForceVelocityForm = new ChartForceVelocityForm(this);
            formChartPidTimeForce = new ChartPID_TimeForceForm(this);
            formChartPidTimePosition = new ChartPID_TimePositionForm(this);
            formChartPidTimeVelocity = new ChartPID_TimeVelocityForm(this);
            formProgramForm = new ProgramForm();





            //         cartesianChartVelocityForce.Series[0].Values = cartesianChartVelocityValues;

        }

        #endregion

        /// <summary>
        /// Event handlers called by the UI
        /// </summary>
        #region UserInteractionHandlers
        private void EndpointButton_Click(object sender, EventArgs e)
        {
            DiscoveryEndpoints();
        }

        private void DiscoveryEndpoints()
        {
            endpointListView.Items.Clear();
            //The local discovery URL for the discovery server
            string discoveryUrl = discoveryTextBox.SelectedText;
            try
            {
                ApplicationDescriptionCollection servers = ChartsData.myClientHelperAPI.FindServers(discoveryUrl);

                //servers[0].DiscoveryUrls[0] = "opc.tcp://10.0.1.19:4848";
                //Replace("opc.tcp://br-automation:4848", "opc.tcp://10.0.1.19:4848");
                foreach (ApplicationDescription ad in servers)
                {
                    foreach (string url in ad.DiscoveryUrls)
                    {
                        EndpointDescriptionCollection endpoints = ChartsData.myClientHelperAPI.GetEndpoints(url);
                        foreach (EndpointDescription ep in endpoints)
                        {
                            string securityPolicy = ep.SecurityPolicyUri.Remove(0, 42);
                            endpointListView.Items.Add("[" + ad.ApplicationName + "] " + " [" + ep.SecurityMode + "] " + " [" + securityPolicy + "] " + " [" + ep.EndpointUrl + "]").Tag = ep;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void SubscribeButton_Click(object sender, EventArgs e)
        {
            string Text = subscriptionIdTextBox.Text;
            string monitoredItemName = "myItem" + itemCount.ToString();
            SubsribeURL(Text, monitoredItemName);
        }

        private void SubsribeURL(string Text, string monitoredItemName)
        {
            //this example only supports one item per subscription; remove the following IF loop to add more items
            //if (myMonitoredItem != null)
            //{
            //    try
            //    {
            //        myMonitoredItem = myClientHelperAPI.RemoveMonitoredItem(mySubscription, myMonitoredItem);
            //    }
            //    catch
            //    {
            //        //ignore
            //        ;
            //    }
            //}

            try
            {
                //use different item names for correct assignment at the notificatino event
                itemCount++;

                if (mySubscription == null)
                {
                    mySubscription = ChartsData.myClientHelperAPI.Subscribe(50);
                }
                myMonitoredItem = ChartsData.myClientHelperAPI.AddMonitoredItem(mySubscription, Text, monitoredItemName, 100);
                ChartsData.myClientHelperAPI.ItemChangedNotification += new MonitoredItemNotificationEventHandler(Notification_MonitoredItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void EpConnectButton_Click(object sender, EventArgs e)
        {

            EpConnect();

        }

        private void EpConnect()
        {
            //Check if sessions exists; If yes > delete subscriptions and disconnect
            if (mySession != null && !mySession.Disposed)
            {
                try
                {
                    mySubscription.Delete(true);
                }
                catch
                {
                    ;
                }

                ChartsData.myClientHelperAPI.Disconnect();
                mySession = ChartsData.myClientHelperAPI.Session;

                ResetUI();
            }
            else
            {
                try
                {
                    //Register mandatory events (cert and keep alive)
                    ChartsData.myClientHelperAPI.KeepAliveNotification += new KeepAliveEventHandler(Notification_KeepAlive);
                    ChartsData.myClientHelperAPI.CertificateValidationNotification += new CertificateValidationEventHandler(Notification_ServerCertificate);

                    //Check for a selected endpoint
                    if (mySelectedEndpoint != null)
                    {
                        //Call connect


#pragma warning disable CS0618 // Type or member is obsolete
                        //myClientHelperAPI.Connect("opc.tcp://10.0.1.19:4848", "", MessageSecurityMode.None, userPwButton.Checked, userTextBox.Text, pwTextBox.Text);
#pragma warning restore CS0618 // Type or member is obsolete
                        ChartsData.myClientHelperAPI.Connect(mySelectedEndpoint, userPwButton.Checked, userTextBox.Text, pwTextBox.Text);
                        //Extract the session object for further direct session interactions
                        mySession = ChartsData.myClientHelperAPI.Session;

                        //UI settings
                        epConnectServerButton.Text = "Disconnect from server";
                        browsePage.Enabled = true;
                        rwPage.Enabled = true;
                        subscribePage.Enabled = true;
                        structPage.Enabled = true;
                        methodPage.Enabled = true;
                        myCertForm = null;



                    }
                    else
                    {
                        MessageBox.Show("Please select an endpoint before connecting", "Error");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    myCertForm = null;
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void WriteValButton_Click(object sender, EventArgs e)
        {
            //          string TextNode = writeIdTextBox.Text;
            //          writeNode(TextNode);
            List<String> values = new List<string>();
            List<String> nodeIdStrings = new List<string>();
            values.Add(writeTextBox.Text);
            nodeIdStrings.Add(writeIdTextBox.Text);
            try
            {
                ChartsData.myClientHelperAPI.WriteValues(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        public void writeNode(string TextValue, string TextNode)
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
                ChartsData.myClientHelperAPI.WriteValues(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void UnsubscribeButton_Click(object sender, EventArgs e)
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            ChartsData.myClientHelperAPI.RemoveSubscription(mySubscription);
            mySubscription = null;
            itemCount = 0;
            subscriptionTextBox.Text = "";
        }

        private void NodeTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            descriptionGridView.Rows.Clear();

            try
            {
                ReferenceDescription refDesc = (ReferenceDescription)e.Node.Tag;
                Node node = ChartsData.myClientHelperAPI.ReadNode(refDesc.NodeId.ToString());
                VariableNode variableNode = new VariableNode();

                string[] row1 = new string[] { "Node Id", refDesc.NodeId.ToString() };
                string[] row2 = new string[] { "Namespace Index", refDesc.NodeId.NamespaceIndex.ToString() };
                string[] row3 = new string[] { "Identifier Type", refDesc.NodeId.IdType.ToString() };
                string[] row4 = new string[] { "Identifier", refDesc.NodeId.Identifier.ToString() };
                string[] row5 = new string[] { "Browse Name", refDesc.BrowseName.ToString() };
                string[] row6 = new string[] { "Display Name", refDesc.DisplayName.ToString() };
                string[] row7 = new string[] { "Node Class", refDesc.NodeClass.ToString() };
                string[] row8 = new string[] { "Description", "null" };
                try { row8 = new string[] { "Description", node.Description.ToString() }; }
                catch { row8 = new string[] { "Description", "null" }; }
                string[] row9 = new string[] { "Type Definition", refDesc.TypeDefinition.ToString() };
                string[] row10 = new string[] { "Write Mask", node.WriteMask.ToString() };
                string[] row11 = new string[] { "User Write Mask", node.UserWriteMask.ToString() };
                if (node.NodeClass == NodeClass.Variable)
                {
                    variableNode = (VariableNode)node.DataLock;
                    List<NodeId> nodeIds = new List<NodeId>();
                    List<string> displayNames = new List<string>();
                    List<ServiceResult> errors = new List<ServiceResult>();
                    NodeId nodeId = new NodeId(variableNode.DataType);
                    nodeIds.Add(nodeId);
                    mySession.ReadDisplayName(nodeIds, out displayNames, out errors);

                    string[] row12 = new string[] { "Data Type", displayNames[0] };
                    string[] row13 = new string[] { "Value Rank", variableNode.ValueRank.ToString() };
                    string[] row14 = new string[] { "Array Dimensions", variableNode.ArrayDimensions.Capacity.ToString() };
                    string[] row15 = new string[] { "Access Level", variableNode.AccessLevel.ToString() };
                    string[] row16 = new string[] { "Minimum Sampling Interval", variableNode.MinimumSamplingInterval.ToString() };
                    string[] row17 = new string[] { "Historizing", variableNode.Historizing.ToString() };

                    object[] rows = new object[] { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12, row13, row14, row15, row16, row17 };
                    foreach (string[] rowArray in rows)
                    {
                        descriptionGridView.Rows.Add(rowArray);
                    }
                }
                else
                {
                    object[] rows = new object[] { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11 };
                    foreach (string[] rowArray in rows)
                    {
                        descriptionGridView.Rows.Add(rowArray);
                    }
                }

                descriptionGridView.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ChartsData.myClientHelperAPI.Disconnect();
            }
            catch
            {
                ;
            }
        }
        private void NodeTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();

            ReferenceDescriptionCollection referenceDescriptionCollection;
            ReferenceDescription refDesc = (ReferenceDescription)e.Node.Tag;

            try
            {
                referenceDescriptionCollection = ChartsData.myClientHelperAPI.BrowseNode(refDesc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return;
            }

            foreach (ReferenceDescription tempRefDesc in referenceDescriptionCollection)
            {
                e.Node.Nodes.Add(tempRefDesc.DisplayName.ToString()).Tag = tempRefDesc;
            }
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Nodes.Add("");
            }
        }
        private void BrowsePage_Enter(object sender, EventArgs e)
        {
            BrowseRoootServer();
        }

        private void BrowseRoootServer()
        {
            if (myReferenceDescriptionCollection == null)
            {
                try
                {
                    myReferenceDescriptionCollection = ChartsData.myClientHelperAPI.BrowseRoot();
                    foreach (ReferenceDescription refDesc in myReferenceDescriptionCollection)
                    {
                        nodeTreeView.Nodes.Add(refDesc.DisplayName.ToString()).Tag = refDesc;
                        foreach (TreeNode node in nodeTreeView.Nodes)
                        {
                            node.Nodes.Add("");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void ReadValButton_Click(object sender, EventArgs e)
        {
            string Text = readIdTextBox.Text;
            ReadValueFromNode(Text);

        }

        private void ReadValueFromNode(string Text)
        {
            List<String> nodeIdStrings = new List<String>();
            List<String> values = new List<String>();
            nodeIdStrings.Add(Text);
            values = ReadValuesFromNodes(nodeIdStrings, values);
        }

        private List<string> ReadValuesFromNodes(List<string> nodeIdStrings, List<string> values)
        {
            try
            {
                values = ChartsData.myClientHelperAPI.ReadValues(nodeIdStrings);
                readTextBox.Text = values.ElementAt<String>(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            return values;
        }

        private void EndpointListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {


            mySelectedEndpoint = (EndpointDescription)e.Item.Tag;
            //mySelectedEndpoint.EndpointUrl = "opc.tcp://10.0.1.19:4848";
        }
        private void OpcTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            e.Cancel = !e.TabPage.Enabled;
            if (!e.TabPage.Enabled)
            {
                MessageBox.Show("Establish a connection to a server first.", "Error");
            }
        }
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            RegisterNode();

        }

        private void RegisterNode()
        {
            string Text = rgNodeIdTextBox.Text;
            List<String> nodeIdStrings = new List<String>();
            nodeIdStrings.Add(Text);
            RegisterAllNodes(nodeIdStrings);
        }

        private void RegisterAllNodes(List<string> nodeIdStrings)
        {
            try
            {
                myRegisteredNodeIdStrings = ChartsData.myClientHelperAPI.RegisterNodeIds(nodeIdStrings);
                regNodeIdTextBox.Text = myRegisteredNodeIdStrings.ElementAt<String>(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void UnregisterButton_Click(object sender, EventArgs e)
        {
            UnregisteredAllNodes();
            myRegisteredNodeIdStrings.Clear();
            regNodeIdTextBox.Text = "";
            rgReadTextBox.Text = "";
            rgWriteTextBox.Text = "";
        }

        private void UnregisteredAllNodes()
        {
            try
            {
                ChartsData.myClientHelperAPI.UnregisterNodeIds(myRegisteredNodeIdStrings);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void RgReadButton_Click(object sender, EventArgs e)
        {
            List<String> values = new List<String>();
            values = ReturnValuesFromNodeToTextbox(values);
        }

        private List<string> ReturnValuesFromNodeToTextbox(List<string> values)
        {
            try
            {
                values = ChartsData.myClientHelperAPI.ReadValues(myRegisteredNodeIdStrings);
                rgReadTextBox.Text = values.ElementAt<String>(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            return values;
        }

        private List<string> ReturnValuesFromNodeToList(List<string> values)
        {
            try
            {
                values = ChartsData.myClientHelperAPI.ReadValues(myRegisteredNodeIdStrings);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            return values;
        }

        private void RgWriteButton_Click(object sender, EventArgs e)
        {
            string Text = rgWriteTextBox.Text;
            rgWriteText(Text);
        }

        private void rgWriteText(string Text)
        {
            List<String> values = new List<string>();
            values.Add(Text);
            rgWriteList(values);
        }

        private void rgWriteList(List<string> values)
        {
            try
            {
                ChartsData.myClientHelperAPI.WriteValues(values, myRegisteredNodeIdStrings);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void UserPwButton_CheckedChanged(object sender, EventArgs e)
        {
            if (userPwButton.Checked)
            {
                userTextBox.Enabled = true;
                pwTextBox.Enabled = true;
            }
        }
        private void UserAnonButton_CheckedChanged(object sender, EventArgs e)
        {
            if (userAnonButton.Checked)
            {
                userTextBox.Enabled = false;
                pwTextBox.Enabled = false;
            }
        }
        private void StructReadButton_Click(object sender, EventArgs e)
        {
            structGridView.Rows.Clear();
            myStructList = new List<string[]>();
            Text = structNodeIdTextBox.Text;

            myStructList = ReadStructUdtToList(myStructList, Text);


            foreach (string[] val in myStructList)
            {
                string[] row = new string[] { val[0], val[1], val[2] };
                structGridView.Rows.Add(row);
                if (structGridView.Rows[structGridView.Rows.Count - 1].Cells[1].Value.ToString() == "..")
                {
                    structGridView.Rows[structGridView.Rows.Count - 1].Cells[1].Style.BackColor = Color.Gainsboro;
                    structGridView.Rows[structGridView.Rows.Count - 1].Cells[1].ReadOnly = true;
                }
            }

            structGridView.ClearSelection();
        }

        private List<string[]> ReadStructUdtToList(List<string[]> values, string Text)
        {
            try
            {
                values = ChartsData.myClientHelperAPI.ReadStructUdt(Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");

            }
            return values;
        }



        private void StructWriteButton_Click(object sender, EventArgs e)
        {
            if (structGridView.Rows.Count == 0)
            {
                MessageBox.Show("Read a struct/UDT first.", "Error");
                return;
            }

            //Clear the list and refill with values from GridView to get value changes
            myStructList.Clear();
            foreach (DataGridViewRow row in structGridView.Rows)
            {
                string[] tempString = new String[3];
                try
                {
                    tempString[0] = structGridView.Rows[row.Index].Cells[0].Value.ToString();
                }
                catch
                {
                    tempString[0] = "";
                }

                try
                {
                    tempString[1] = structGridView.Rows[row.Index].Cells[1].Value.ToString();
                }
                catch
                {
                    tempString[1] = "";
                }

                try
                {
                    tempString[2] = structGridView.Rows[row.Index].Cells[2].Value.ToString();
                }
                catch
                {
                    tempString[2] = "";
                }

                myStructList.Add(tempString);
            }

            try
            {
                ChartsData.myClientHelperAPI.WriteStructUdt(structNodeIdTextBox.Text, myStructList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void MethodInfoButton_Click(object sender, EventArgs e)
        {
            //Clear grid view first
            inputArgumentsGridView.Rows.Clear();
            outputArgumentsGridView.Rows.Clear();

            //Creata list of strings for the method's arguments
            List<string> methodArguments = new List<string>();

            //Get the arguments
            try
            {
                methodArguments = ChartsData.myClientHelperAPI.GetMethodArguments(methodNodeIdTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            //If arguemnt is null there's no method
            if (methodArguments == null)
            {
                MessageBox.Show("The Node Id doesn't refer to a method", "Error");
                return;
            }

            //Check for display name to determine if there are intput and/or output arguments for the method
            foreach (String argument in methodArguments)
            {
                String[] strArray = argument.Split(';');

                if (strArray[0] == "InputArguments")
                {
                    string[] row = new string[] { strArray[1], strArray[2], strArray[3] };
                    inputArgumentsGridView.Rows.Add(row);
                }

                if (strArray[0] == "OutputArguments")
                {
                    string[] row = new string[] { strArray[1], strArray[2], strArray[3] };
                    outputArgumentsGridView.Rows.Add(row);
                }
            }

            //If there's no argument stored in the gridview there's no argument to care about
            if (inputArgumentsGridView.Rows.Count == 0)
            {
                string[] row = new string[] { "-", "-", "none" };
                inputArgumentsGridView.Rows.Add(row);
            }

            if (outputArgumentsGridView.Rows.Count == 0)
            {
                string[] row = new string[] { "-", "-", "none" };
                outputArgumentsGridView.Rows.Add(row);
            }

            inputArgumentsGridView.ClearSelection();
            outputArgumentsGridView.ClearSelection();

            //Enable the call button after retrieving argument info
            callButton.Enabled = true;
        }
        private void CallButton_Click(object sender, EventArgs e)
        {
            //Call the method

            //Create a list of string arrays for the input arguments
            List<string[]> inputData = new List<string[]>();

            //Copy data from the gridview to the argument list (value at [0]; data type at [1]) 
            //First check for data type "none" > no input argument available
            if (inputArgumentsGridView.Rows[0].Cells[2].Value.ToString() != "none")
            {
                foreach (DataGridViewRow row in inputArgumentsGridView.Rows)
                {
                    inputData.Add(new String[2] { row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString() });
                }
            }

            //Create an object list for retrieving the output arguments
            IList<object> outputValues;
            try
            {
                //Call the method
                outputValues = ChartsData.myClientHelperAPI.CallMethod(methodNodeIdTextBox.Text, objectNodeIdTextBox.Text, inputData);

                if (outputValues != null)
                {
                    //Copy output arguments to the gridview
                    for (int i = 0; i < outputArgumentsGridView.Rows.Count; i++)
                    {
                        outputArgumentsGridView.Rows[i].Cells[1].Value = outputValues[i].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        #endregion

        /// <summary>
        /// Global OPC UA event handlers
        /// </summary>
        #region OpcEventHandlers
        private void Notification_ServerCertificate(CertificateValidator cert, CertificateValidationEventArgs e)
        {
            //Handle certificate here
            //To accept a certificate manually move it to the root folder (Start > mmc.exe > add snap-in > certificates)
            //Or handle via UAClientCertForm

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CertificateValidationEventHandler(Notification_ServerCertificate), cert, e);
                return;
            }

            try
            {
                //Search for the server's certificate in store; if found -> accept
                X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                X509CertificateCollection certCol = store.Certificates.Find(X509FindType.FindByThumbprint, e.Certificate.Thumbprint, true);
                store.Close();
                if (certCol.Capacity > 0)
                {
                    e.Accept = true;
                }

                //Show cert dialog if cert hasn't been accepted yet
                else
                {
                    if (!e.Accept & myCertForm == null)
                    {
                        myCertForm = new UAClientCertForm(e);
                        myCertForm.ShowDialog();
                    }
                }
            }
            catch
            {
                ;
            }
        }
        private void Notification_MonitoredItem(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MonitoredItemNotificationEventHandler(Notification_MonitoredItem), monitoredItem, e);
                return;
            }
            MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
            if (notification == null)
            {
                return;
            }
            if (ChartsData.endOfSubstription)
            {
                NotificationWriteToTexbox(monitoredItem, notification);
                NotificationWriteToValues(monitoredItem, notification);
                notification = null;

            }

        }

        private void Notification_MonitoredItem_Fast(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MonitoredItemNotificationEventHandler(Notification_MonitoredItem_Fast), monitoredItem, e);
                return;
            }
            MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
            if (notification == null)
            {
                return;
            }
            if (ChartsData.endOfSubstription)
            {

                NotificationWriteToValuesFast(monitoredItem, notification);
                notification = null;

            }

        }

        private void Notification_MonitoredItem_Program(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MonitoredItemNotificationEventHandler(Notification_MonitoredItem_Program), monitoredItem, e);
                return;
            }
            MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
            if (notification == null)
            {
                return;
            }
            if (ChartsData.endOfSubstription)
            {

                NotificationWriteToValuesProgram(monitoredItem, notification);
                notification = null;

            }

        }
        private void Notification_MonitoredItem_FiltersAndRamps(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MonitoredItemNotificationEventHandler(Notification_MonitoredItem_FiltersAndRamps), monitoredItem, e);
                return;
            }
            MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
            if (notification == null)
            {
                return;
            }
            if (ChartsData.endOfSubstription)
            {

                NotificationWriteToValuesFintersAndRamps(monitoredItem, notification);
                notification = null;

            }

        }
        private void Notification_MonitoredItem_Constants(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MonitoredItemNotificationEventHandler(Notification_MonitoredItem_Constants), monitoredItem, e);
                return;
            }
            MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
            if (notification == null)
            {
                return;
            }
            if (ChartsData.endOfSubstription)
            {

                NotificationWriteToValuesConstants(monitoredItem, notification);
                notification = null;

            }

        }

        private void NotificationWriteToTexbox(MonitoredItem monitoredItem, MonitoredItemNotification notification)
        {
            //subscriptionTextBox.Text = "Item name: " + monitoredItem.DisplayName;
            //subscriptionTextBox.Text += System.Environment.NewLine + "Value: " + Utils.Format("{0}", notification.Value.WrappedValue.ToString());
            //subscriptionTextBox.Text += System.Environment.NewLine + "Source timestamp: " + notification.Value.SourceTimestamp.ToString();
            //subscriptionTextBox.Text += System.Environment.NewLine + "Server timestamp: " + notification.Value.ServerTimestamp.ToString();
            return;

#pragma warning disable CS0162 // Unreachable code detected
            string text = "Item name: " + monitoredItem.DisplayName + " Value: " + Utils.Format("{0}", notification.Value.WrappedValue.ToString()) +
#pragma warning restore CS0162 // Unreachable code detected

                                            " Source timestamp: " + notification.Value.SourceTimestamp.ToString() +
                                            " Server timestamp: " + notification.Value.ServerTimestamp.ToString();




            List<string> lineList = new List<string>();
            lineList = subscriptionTextBox.Lines.ToList();

            int texboxLines = lineList.Count();
            if (texboxLines > 10)
            {
                lineList.RemoveRange(0, 1);

            }

            lineList.Add(text);
            subscriptionTextBox.Lines = lineList.ToArray();
        }
        private void NotificationWriteToValues(MonitoredItem monitoredItem, MonitoredItemNotification notification)
        {
            try
            {

                if (monitoredItem.DisplayName == "MachineControl_status_ioOK")
                {
                    machineIoOK = (bool)notification.Value.Value;
                    if (machineIoOK)
                    {
                        buttonIO.BackColor = Color.Green;
                    }
                    else
                    {
                        buttonIO.BackColor = Color.Red;

                    }

                }
                if (monitoredItem.DisplayName == "MachineControl_status_powerOn")
                {
                    machinePowerOn = (bool)notification.Value.Value;
                    if (!machinePowerOn)
                    {
                        buttonPowerOnState.BackColor = Color.Red;
                    }
                    else
                    {
                        buttonPowerOnState.BackColor = Color.Green;

                    }

                }
                if (monitoredItem.DisplayName == "MachineControl_status_ready")
                {
                    machineReady = (bool)notification.Value.Value;
                    if (!machineReady)
                    {
                        buttonReady.BackColor = SystemColors.Control;
                    }
                    else
                    {
                        buttonErr.BackColor = Color.Green;
                        buttonReady.BackColor = Color.Green;

                    }

                }
                if (monitoredItem.DisplayName == "MachineControl_status_manual_downLow")
                {

                    if ((bool)notification.Value.Value)
                    {
                        buttonDowm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

                    }
                    else
                    {
                        buttonDowm.FlatStyle = System.Windows.Forms.FlatStyle.Standard;

                    }

                }
                if (monitoredItem.DisplayName == "MachineControl_status_manual_downFast")
                {

                    if ((bool)notification.Value.Value)
                    {
                        buttonFastDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

                    }
                    else
                    {
                        buttonFastDown.FlatStyle = System.Windows.Forms.FlatStyle.Standard;

                    }

                }
                if (monitoredItem.DisplayName == "MachineControl_status_manual_upLow")
                {

                    if ((bool)notification.Value.Value)
                    {
                        buttonUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

                    }
                    else
                    {
                        buttonUp.FlatStyle = System.Windows.Forms.FlatStyle.Standard;

                    }

                }
                if (monitoredItem.DisplayName == "MachineControl_status_manual_upFast")
                {

                    if ((bool)notification.Value.Value)
                    {
                        buttonFastUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

                    }
                    else
                    {
                        buttonFastUp.FlatStyle = System.Windows.Forms.FlatStyle.Standard;

                    }

                }
                if (monitoredItem.DisplayName == "MachineControl_status_error")
                {

                    if ((bool)notification.Value.Value)
                    {
                        buttonErr.BackColor = Color.Red;
                    }
                    else
                    {
                        buttonErr.BackColor = Color.Green;

                    }

                }

                if (monitoredItem.DisplayName == "MachineControl_positionControl_status_ready")
                {
                    positionOK = (bool)notification.Value.Value;
                    if (!positionOK)
                    {
                        checkBoxStatusPosition.Checked = false;
                        checkBoxStatusProgram.Checked = false;


                    }
                    else
                    {
                        checkBoxStatusPosition.Checked = true;
                        checkBoxStatusPosition.BackColor = SystemColors.Control;
                        checkBoxStatusProgram.Checked = checkBoxStatusPosition.Checked && checkBoxStatusVelocity.Checked && checkBoxStatusForce.Checked;

                    }

                }

                if (monitoredItem.DisplayName == "MachineControl_positionControl_output_error")
                {

                    if ((bool)notification.Value.Value)
                    {
                        checkBoxStatusPosition.Checked = false;
                        checkBoxStatusProgram.Checked = false;
                        checkBoxStatusPosition.BackColor = Color.Red;


                    }
                    else
                    {

                        checkBoxStatusPosition.BackColor = SystemColors.Control;

                    }

                }

                if (monitoredItem.DisplayName == "MachineControl_velocityControl_status_ready")
                {
                    velocityOK = (bool)notification.Value.Value;
                    if (!velocityOK)
                    {
                        checkBoxStatusPosition.Checked = false;
                        checkBoxStatusProgram.Checked = false;
                        checkBoxStatusManual.Checked = false;


                    }
                    else
                    {
                        checkBoxStatusVelocity.Checked = true;
                        checkBoxStatusManual.Checked = true;
                        checkBoxStatusVelocity.BackColor = SystemColors.Control;
                        checkBoxStatusProgram.Checked = checkBoxStatusPosition.Checked && checkBoxStatusVelocity.Checked && checkBoxStatusForce.Checked;

                    }

                }

                if (monitoredItem.DisplayName == "MachineControl_velocityControl_status_error")
                {

                    if ((bool)notification.Value.Value)
                    {
                        checkBoxStatusVelocity.Checked = false;
                        checkBoxStatusProgram.Checked = false;
                        checkBoxStatusManual.Checked = false;
                        checkBoxStatusVelocity.BackColor = Color.Red;


                    }
                    else
                    {

                        checkBoxStatusVelocity.BackColor = SystemColors.Control;

                    }

                }
                if (monitoredItem.DisplayName == "MachineControl__status_simulating")
                {

                    if ((bool)notification.Value.Value)
                    {
                        labelSimulation.Visible = true;
                        labelSimulation.ForeColor = Color.Blue;
                    }
                    else
                    {
                        labelSimulation.Visible = false;
                        labelSimulation.ForeColor = Color.Black;
                    }

                }
                if (monitoredItem.DisplayName == "MachineControl_velocityControl_status_simulating")
                {

                    if ((bool)notification.Value.Value)
                    {
                        checkBoxStatusVelocity.BackColor = Color.Blue;
                        formChartPidTimeVelocity.checkBoxSimulationActive.Checked = true;
                    }
                    else
                    {
                        checkBoxStatusVelocity.BackColor = SystemColors.Control;
                        formChartPidTimeVelocity.checkBoxSimulationActive.Checked = false;
                    }

                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_status_simulating")
                {

                    if ((bool)notification.Value.Value)
                    {
                        checkBoxStatusForce.BackColor = Color.Blue;
                        formChartPidTimeForce.checkBoxSimulationActive.Checked = true;
                    }
                    else
                    {
                        checkBoxStatusForce.BackColor = SystemColors.Control;
                        formChartPidTimeForce.checkBoxSimulationActive.Checked = false;
                    }

                }
                if (monitoredItem.DisplayName == "MachineControl_positionControl_status_simulating")
                {

                    if ((bool)notification.Value.Value)
                    {
                        checkBoxStatusPosition.BackColor = Color.Blue;
                        formChartPidTimePosition.checkBoxSimulationActive.Checked = true;
                    }
                    else
                    {
                        checkBoxStatusPosition.BackColor = SystemColors.Control;
                        formChartPidTimePosition.checkBoxSimulationActive.Checked = false;
                    }

                }

                if (monitoredItem.DisplayName == "MachineControl_strainControl_status_RampActive")
                {
                    CheckBoxStatusWrite(notification, checkBoxRampOnForce);
                }

                if (monitoredItem.DisplayName == "MachineControl_strainControl_status_FilterActive")
                {
                    CheckBoxStatusWrite(notification, checkBoxFilterOnForce);
                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_status_PIDActive")
                {
                    CheckBoxStatusWrite(notification, checkBoxControlOnForce);
                }

                if (monitoredItem.DisplayName == "MachineControl_positionControl_status_RampActive")
                {
                    CheckBoxStatusWrite(notification, checkBoxRampOnPosition);
                }
                if (monitoredItem.DisplayName == "MachineControl_positionControl_status_FilterActive")
                {
                    CheckBoxStatusWrite(notification, checkBoxFilterOnPosition);
                }
                if (monitoredItem.DisplayName == "MachineControl_positionControl_status_PIDActive")
                {
                    CheckBoxStatusWrite(notification, checkBoxControlOnPosition);
                }

                if (monitoredItem.DisplayName == "MachineControl_velocityControl_status_RampActive")
                {
                    CheckBoxStatusWrite(notification, checkBoxRampOnVelocity);
                }

                if (monitoredItem.DisplayName == "MachineControl_velocityControl_status_FilterActive")
                {
                    CheckBoxStatusWrite(notification, checkBoxFilterOnVelocity);
                }
                if (monitoredItem.DisplayName == "MachineControl_velocityControl_status_PIDActive")
                {
                    CheckBoxStatusWrite(notification, checkBoxControlOnVelocity);
                }

                if (monitoredItem.DisplayName == "MachineControl_status_programControl")
                {
                    CheckBoxStatusWrite(notification, checkBoxControlOnProgram);
                }

                if (monitoredItem.DisplayName == "MachineControl_strainControl_status_ready")
                {
                    forceOK = (bool)notification.Value.Value;
                    if (!forceOK)
                    {
                        checkBoxStatusForce.Checked = false;
                        checkBoxStatusProgram.Checked = false;



                    }
                    else
                    {
                        checkBoxStatusForce.Checked = true;
                        checkBoxStatusForce.BackColor = SystemColors.Control;
                        checkBoxStatusProgram.Checked = checkBoxStatusPosition.Checked && checkBoxStatusVelocity.Checked && checkBoxStatusForce.Checked;

                    }

                }
                if (monitoredItem.DisplayName == "MachineControl_status_tara")
                {
                    if ((bool)notification.Value.Value)
                    {
                        buttonTara.BackColor = Color.Red;
                        buttonTara.ForeColor = Color.Black;
                    }
                    else
                    {
                        buttonTara.BackColor = SystemColors.Control;
                        buttonTara.Text = "Tara";
                        commandTaraStart = false;

                    }
                }

                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_command_writeToMemory")
                {
                    changeCommandButtonColor(notification, buttonSaveScaleParameters, "Zapiš");
                    if (!(bool)notification.Value.Value)
                    {
                        textBoxActualStdHighEcho.BackColor = Color.White;
                        textBoxActualStdLowEcho.BackColor = Color.White;
                        textBoxActualLRawHighEcho.BackColor = Color.White;
                        textBoxActualRawLowEcho.BackColor = Color.White;
                        textBoxTaraEcho.BackColor = Color.White;
                    }
                    else
                    {
                        textBoxActualStdHighEcho.BackColor = Color.LightBlue;
                        textBoxActualStdLowEcho.BackColor = Color.LightBlue;
                        textBoxActualRawLowEcho.BackColor = Color.LightBlue;
                        textBoxActualLRawHighEcho.BackColor = Color.LightBlue;
                        textBoxTaraEcho.BackColor = Color.LightBlue;
                    }

                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_command_writeToMemory1")
                {
                    changeCommandButtonColor(notification, buttonSaveForceParametersHigh, "Zapiš");
                    if (!(bool)notification.Value.Value)
                    {
                        textBoxHighForceRawLow.BackColor = Color.White;
                        textBoxHighForceStdLow.BackColor = Color.White;
                        textBoxHighForceRawHigh.BackColor = Color.White;
                        textBoxHighForceStdHigh.BackColor = Color.White;
                        textBoxHighForceTara.BackColor = Color.White;
                    }
                    else
                    {
                        textBoxHighForceRawLow.BackColor = Color.LightBlue;
                        textBoxHighForceStdLow.BackColor = Color.LightBlue;
                        textBoxHighForceRawHigh.BackColor = Color.LightBlue;
                        textBoxHighForceStdHigh.BackColor = Color.LightBlue;
                        textBoxHighForceTara.BackColor = Color.LightBlue;
                    }

                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_command_writeToMemory2")
                {
                    changeCommandButtonColor(notification, buttonSaveForceParametersLow, "Načti");
                    if (!(bool)notification.Value.Value)
                    {
                        textBoxLowForceRawLow.BackColor = Color.White;
                        textBoxLowForceStdLow.BackColor = Color.White;
                        textBoxLowForceRawHigh.BackColor = Color.White;
                        textBoxLowForceStdHigh.BackColor = Color.White;
                        textBoxLowForceTara.BackColor = Color.White;
                    }
                    else
                    {
                        textBoxLowForceRawLow.BackColor = Color.LightBlue;
                        textBoxLowForceStdLow.BackColor = Color.LightBlue;
                        textBoxLowForceRawHigh.BackColor = Color.LightBlue;
                        textBoxLowForceStdHigh.BackColor = Color.LightBlue;
                        textBoxLowForceTara.BackColor = Color.LightBlue;
                    }


                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_command_getParameters")
                {
                    changeCommandButtonColor(notification, buttonReadActualCellParameters, "Načti");
                    if (!(bool)notification.Value.Value)
                    {
                        textBoxActualStdHighEcho.BackColor = Color.White;
                        textBoxActualStdLowEcho.BackColor = Color.White;
                        textBoxActualLRawHighEcho.BackColor = Color.White;
                        textBoxActualRawLowEcho.BackColor = Color.White;
                        textBoxTaraEcho.BackColor = Color.White;
                    }
                    else
                    {
                        textBoxActualStdHighEcho.BackColor = Color.LightGreen;
                        textBoxActualStdLowEcho.BackColor = Color.LightGreen;
                        textBoxActualLRawHighEcho.BackColor = Color.LightGreen;
                        textBoxActualRawLowEcho.BackColor = Color.LightGreen;
                        textBoxTaraEcho.BackColor = Color.LightGreen;
                   }


                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_command_getParameters1")
                {
                    changeCommandButtonColor(notification, buttonReadCellParametersHigh, "Načti");
                    if (!(bool)notification.Value.Value)
                    {
                        textBoxHighForceRawLow.BackColor = Color.White;
                        textBoxHighForceStdLow.BackColor = Color.White;
                        textBoxHighForceRawHigh.BackColor = Color.White;
                        textBoxHighForceStdHigh.BackColor = Color.White;
                        textBoxHighForceTara.BackColor = Color.White;
                    }
                    else
                    {
                        textBoxHighForceRawLow.BackColor = Color.LightGreen;
                        textBoxHighForceStdLow.BackColor = Color.LightGreen;
                        textBoxHighForceRawHigh.BackColor = Color.LightGreen;
                        textBoxHighForceStdHigh.BackColor = Color.LightGreen;
                        textBoxHighForceTara.BackColor = Color.LightGreen;
                    }


                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_command_getParameters2")
                {
                    changeCommandButtonColor(notification, buttonReadCellParametersLow, "Načti");
                    if (!(bool)notification.Value.Value)
                    {
                        textBoxLowForceRawLow.BackColor = Color.White;
                        textBoxLowForceStdLow.BackColor = Color.White;
                        textBoxLowForceRawHigh.BackColor = Color.White;
                        textBoxLowForceStdHigh.BackColor = Color.White;
                        textBoxLowForceTara.BackColor = Color.White;
                    }
                    else
                    {
                        textBoxLowForceRawLow.BackColor = Color.LightGreen;
                        textBoxLowForceStdLow.BackColor = Color.LightGreen;
                        textBoxLowForceRawHigh.BackColor = Color.LightGreen;
                        textBoxLowForceStdHigh.BackColor = Color.LightGreen;
                        textBoxLowForceTara.BackColor = Color.LightGreen;
                    }


                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_scaleParameter_ref1_raw")
                {
                    textBoxActualRawLowEcho.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_scaleParameter_ref1_std")
                {
                    textBoxActualStdLowEcho.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_scaleParameter_ref2_raw")
                {
                    textBoxActualLRawHighEcho.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_scaleParameter_ref2_std")
                {
                    textBoxActualStdHighEcho.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_scaleParameter_tare_std")
                {
                    textBoxTaraEcho.Text = notification.Value.Value.ToString();
                }

                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell1_ref1_raw")
                {
                    textBoxHighForceRawLow.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell1_ref1_std")
                {
                    textBoxHighForceStdLow.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell1_ref2_raw")
                {
                    textBoxHighForceRawHigh.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell1_ref2_std")
                {
                    textBoxHighForceStdHigh.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell1_tare_std")
                {
                    textBoxHighForceTara.Text = notification.Value.Value.ToString();
                }


                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell2_ref_raw")
                {
                    textBoxLowForceRawLow.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell2_ref1_std")
                {
                    textBoxLowForceStdLow.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell2_ref2_raw")
                {
                    textBoxLowForceRawHigh.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell2_ref2_std")
                {
                    textBoxLowForceStdHigh.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_parameter_parameterLoadCell2_tare_std")
                {
                    textBoxLowForceTara.Text = notification.Value.Value.ToString();
                }

                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_status_doneStandartize1")
                {

                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_status_doneStandartize2")
                {

                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_status_doneTare")
                {

                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_status_enable")
                {

                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_status_error")
                {

                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_status_ready")
                {

                }
                               
                if (monitoredItem.DisplayName == "MachineControl_strainControl_controlWeight_status_actualCellType")
                {
                    int val;
                    string str;
                    try
                    {
                        str = notification.Value.Value.ToString();
                        int.TryParse(str, out val);
                        switch (val)
                        {
                            case 1:
                                radioButtonLoadCellHigh.Checked = true;
                                radioButtonLoadCellLow.Checked = false;
                                break;
                            case 2:
                                radioButtonLoadCellHigh.Checked = false;
                                radioButtonLoadCellLow.Checked = true;
                                break;
                            default:
                                radioButtonLoadCellHigh.Checked = false;
                                radioButtonLoadCellLow.Checked = false;
                                break;
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }


                }
                if (monitoredItem.DisplayName == "MachineControl_status_standartizeWeight1")
                {
                    if ((bool)notification.Value.Value)
                    {
                        buttonLowForce.BackColor = Color.Red;
                        buttonLowForce.ForeColor = Color.Black;
                    }
                    else
                    {
                        buttonLowForce.BackColor = SystemColors.Control;
                        buttonLowForce.Text = "Low Force";
                        commandLowForceStart = false;

                    }
                }
                if (monitoredItem.DisplayName == "MachineControl_status_standartizeWeight2")
                {
                    if ((bool)notification.Value.Value)
                    {
                        buttonHighForce.BackColor = Color.Red;
                        buttonHighForce.ForeColor = Color.Black;
                    }
                    else
                    {
                        buttonHighForce.BackColor = SystemColors.Control;
                        buttonHighForce.Text = "High Force";
                        commandHighForceStart = false;

                    }

                }
                if (monitoredItem.DisplayName == "MachineControl_positionControl_output_trackingActive")
                {
                    if ((bool)notification.Value.Value)
                    {
                        checkBoxPositionPidMan.BackColor = Color.Green;
                    }
                    else
                    {
                        checkBoxPositionPidMan.BackColor = SystemColors.Control;
                    }
                }
                if (monitoredItem.DisplayName == "MachineControl_strainControl_output_trackingActive")
                {
                    if ((bool)notification.Value.Value)
                    {
                        checkBoxForcePidMan.BackColor = Color.Green;
                    }
                    else
                    {
                        checkBoxForcePidMan.BackColor = SystemColors.Control;
                    }
                }
                if (monitoredItem.DisplayName == "MachineControl_velocityControl_output_trackingActive")
                {
                    if ((bool)notification.Value.Value)
                    {
                        checkBoxVelocityPidMan.BackColor = Color.Green;
                    }
                    else
                    {
                        checkBoxVelocityPidMan.BackColor = SystemColors.Control;
                    }
                }
                if (monitoredItem.DisplayName == "MachineControl_input_position_SetValue")
                {
                    textBoxPositionSetEcho.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "MachineControl_input_velocity_SetValue")
                {
                    textBoxVelocitySetEcho.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "MachineControl_input_strain_setValue")
                {
                    textBoxStrainSetEcho.Text = notification.Value.Value.ToString();
                }


                if (monitoredItem.DisplayName == "constMachineControl_strain_rampSettings_MinOut")
                {
                    textBoxStrainRampMin.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_strain_rampSettings_MaxOut")
                {
                    textBoxStrainRampMax.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_strain_rampSettings_MaxPosSlewRate")
                {
                    textBoxStrainRampSlewRate.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_strain_rampSettings_MinPosSlewRate")
                {
                    textBoxStrainRampSlewRateNeg.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_strain_filterSettings_Type")
                {

                }
                if (monitoredItem.DisplayName == "constMachineControl_strain_filterSettings_Order")
                {
                    textBoxStrainFiltrOrder.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_strain_filterSettings_CutOffFrequency")
                {
                    textBoxStrainFilterCutOffFreq.Text = notification.Value.Value.ToString();
                }

                if (monitoredItem.DisplayName == "constMachineControl_velocity_rampSettings_MinOut")
                {
                    textBoxVelocityRampMin.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_velocity_rampSettings_MaxOut")
                {
                    textBoxVelocityRampMax.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_velocity_rampSettings_MaxPosSlewRate")
                {
                    textBoxVelocityRampSlewRate.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_velocity_rampSettings_MinPosSlewRate")
                {
                    textBoxVelocityRampSlewRateNeg.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_velocity_filterSettings_Type")
                {

                }
                if (monitoredItem.DisplayName == "constMachineControl_velocity_filterSettings_Order")
                {
                    textBoxVelocityFiltrOrder.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_velocity_filterSettings_CutOffFrequency")
                {
                    textBoxVelocityFilterCutOffFreq.Text = notification.Value.Value.ToString();
                }

                if (monitoredItem.DisplayName == "constMachineControl_position_rampSettings_MinOut")
                {
                    textBoxPositionRampMin.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_position_rampSettings_MaxOut")
                {
                    textBoxPositionRampMax.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_position_rampSettings_MaxPosSlewRate")
                {
                    textBoxPositionRampSlewRate.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_position_rampSettings_MinPosSlewRate")
                {
                    textBoxPositionRampSlewRateNeg.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_position_filterSettings_Type")
                {

                }
                if (monitoredItem.DisplayName == "constMachineControl_position_filterSettings_Order")
                {
                    textBoxPositionFiltrOrder.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_position_filterSettings_CutOffFrequency")
                {
                    textBoxPositionFilterCutOffFreq.Text = notification.Value.Value.ToString();
                }

            }


#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                //throw;
            }

        }

        private void changeCommandButtonColor(MonitoredItemNotification notification, Button bt, string buttonText)
        {
            if ((bool)notification.Value.Value)
            {
                bt.BackColor = Color.Red;
                bt.ForeColor = Color.Black;
            }
            else
            {
                bt.BackColor = SystemColors.Control;
                bt.Text = buttonText;


            }
        }

        private void NotificationWriteToValuesFintersAndRamps(MonitoredItem monitoredItem, MonitoredItemNotification notification)
        {
            try
            {

                if (monitoredItem.DisplayName == "PositionSt_FilterLowPass_Active")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_FilterLowPass_CutOffFrequency")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_FilterLowPass_Enable")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_FilterLowPass_Error")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_FilterLowPass_Order")
                {

                }

                if (monitoredItem.DisplayName == "PositionSt_FilterLowPass_Type")
                {

                }

                if (monitoredItem.DisplayName == "PositionSt_FilterLowPass_Update")
                {

                }

                if (monitoredItem.DisplayName == "PositionSt_FilterLowPass_UpdateDone")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_Ramp_Active")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_Ramp_Enable")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_Ramp_Error")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_Ramp_MaxNegSlewRate")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_Ramp_MaxOut")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_Ramp_MaxPosSlewRate")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_Ramp_MinOut")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_Ramp_Update")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_Ramp_UpdateDone")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_constFilter_CutOffFrequency")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_constFilter_Order")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_constRamp_MaxNegSlewRate")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_constRamp_MaxNegSlewRate")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_constRamp_MaxPosSlewRate")
                {

                }
                if (monitoredItem.DisplayName == "PositionSt_constRamp_MinOut")
                {

                }
                if (monitoredItem.DisplayName == "StrainStat_FilterLowPass_Active")
                {

                }








            }


#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                //throw;
            }

        }
        /// <summary>
        /// NotificationWriteToValuesConstants
        /// </summary>
        /// <param name="monitoredItem"></param>
        /// <param name="notification"></param>
        private void NotificationWriteToValuesConstants(MonitoredItem monitoredItem, MonitoredItemNotification notification)
        {
            try
            {



                if (monitoredItem.DisplayName == "constMachineControl_weight_parameterLoadCell1_ref1_raw")
                {
                    textBoxHighForceRawLowEcho.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_weight_parameterLoadCell1_ref1_std")
                {
                    textBoxHighForceStdLowEcho.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_weight_parameterLoadCell1_ref2_raw")
                {
                    textBoxHighForceRawHighEcho.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_weight_parameterLoadCell1_ref2_std")
                {
                    textBoxHighForceStdHighEcho.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_weight_parameterLoadCell1_tare_std")
                {
                    textBoxHighForceTaraEcho.Text = notification.Value.Value.ToString();
                }


                if (monitoredItem.DisplayName == "constMachineControl_weight_parameterLoadCell2_ref1_raw")
                {
                    textBoxLowForceRawLowEcho.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_weight_parameterLoadCell2_ref1_std")
                {
                    textBoxLowForceStdLowEcho.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_weight_parameterLoadCell2_ref2_raw")
                {
                    textBoxLowForceRawHighEcho.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_weight_parameterLoadCell2_ref2_std")
                {
                    textBoxLowForceStdHighEcho.Text = notification.Value.Value.ToString();
                }
                if (monitoredItem.DisplayName == "constMachineControl_weight_parameterLoadCell2_tare_std")
                {
                    textBoxLowForceTaraEcho.Text = notification.Value.Value.ToString();
                }




            }

            ///
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                //throw;
            }

        }

        private void CheckBoxStatusWrite(MonitoredItemNotification notification, CheckBox box)
        {
            box.Checked = (bool)notification.Value.Value;
            if (box.Checked)
            {
                box.BackColor = Color.Green;
            }
            else
            {
                box.BackColor = SystemColors.Control;
            }
        }

        private void NotificationWriteToValuesFast(MonitoredItem monitoredItem, MonitoredItemNotification notification)
        {

            try
            {
                if (monitoredItem.DisplayName == "opcVlocityFast_ingValue")
                {

                    SetText(textBoxVelocity, ChartsData.textBoxVelocity.Text = notification.Value.ToString());
                    ChartsData.WriteValuesToChart(textBoxVelocityTime.Text, textBoxVelocity.Text, ChartsData.ChartVelocityTimeValues, ChartsData.ChartVelocityTimeValuesLock,
                        ref ChartsData.lockVelocityTime, ref ChartsData.lastTimeVelocity, ref ChartsData.lastValueVelocity);
                    ChartsData.WriteValuesToChart(textBoxVelocity.Text, textBoxStrain.Text, ChartsData.ChartVelocityStrainValues, ChartsData.ChartVelocityStrainValuesLock,
                         ref ChartsData.lockVelocityStrain, ref ChartsData.lastValueVelocityStrainVelocity, ref ChartsData.lastValueVelocityStrainStrain);

                }
                if (monitoredItem.DisplayName == "opcPositionFast_ingValue")
                {

                    SetText(textBoxPosition, ChartsData.textBoxPosition.Text = notification.Value.ToString());
                    ChartsData.WriteValuesToChart(textBoxPositionTime.Text, textBoxPosition.Text, ChartsData.ChartPositionTimeValues, ChartsData.ChartPositionTimeValuesLock,
                        ref ChartsData.lockPositionTime, ref ChartsData.lastTimePosition, ref ChartsData.lastValuePosition);
                    ChartsData.WriteValuesToChart(textBoxPosition.Text, textBoxStrain.Text, ChartsData.ChartPositionStrainValues, ChartsData.ChartPositionStrainValuesLock,
                        ref ChartsData.lockPositionStrain, ref ChartsData.lastValuePositionStrainPosition, ref ChartsData.lastValuePositionStrainStrain);
                }
                if (monitoredItem.DisplayName == "opcStrainFast_ingValue")
                {

                    SetText(textBoxStrain, ChartsData.textBoxStrain.Text = notification.Value.ToString());
                    WriteValuesToChart(textBoxStrainTime.Text, textBoxStrain.Text, ChartsData.ChartStrainTimeValues, ChartsData.ChartStrainTimeValuesLock,
                        ref ChartsData.lockStrainTime, ref ChartsData.lastTimeStrain, ref ChartsData.lastValueStrain);
                    WriteValuesToChart(textBoxVelocity.Text, textBoxStrain.Text, ChartsData.ChartVelocityStrainValues, ChartsData.ChartVelocityStrainValuesLock,
                        ref ChartsData.lockVelocityStrain, ref ChartsData.lastValueVelocityStrainVelocity, ref ChartsData.lastValueVelocityStrainStrain);
                    WriteValuesToChart(textBoxPosition.Text, textBoxStrain.Text, ChartsData.ChartPositionStrainValues, ChartsData.ChartPositionStrainValuesLock,
                        ref ChartsData.lockPositionStrain, ref ChartsData.lastValuePositionStrainPosition, ref ChartsData.lastValuePositionStrainStrain);

                }

                if (monitoredItem.DisplayName == "opcVlocityFast_timeStamp")
                {

                    SetText(textBoxVelocityTime, ChartsData.textBoxVelocityTime.Text = notification.Value.ToString());


                }
                if (monitoredItem.DisplayName == "opcPositionFast_timeStamp")
                {

                    SetText(textBoxPositionTime, ChartsData.textBoxPositionTime.Text = notification.Value.ToString());

                }
                if (monitoredItem.DisplayName == "opcStrainFast_timeStamp")
                {

                    SetText(textBoxStrainTime, ChartsData.textBoxStrainTime.Text = notification.Value.ToString());

                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                //throw;
            }

        }

        private void NotificationWriteToValuesProgram(MonitoredItem monitoredItem, MonitoredItemNotification notification)
        {
            string str;
            try
            {
                if (monitoredItem.DisplayName == "Program_state_ActualState")
                {
                    programState.ActualState = (int)notification.Value.Value;
                    return;



                }
                if (monitoredItem.DisplayName == "Program_state_ActualStep")
                {
                    programState.ActualStep = (uint)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_state_Error")
                {
                    programState.Error = (bool)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_state_PassedTime")
                {
                    programState.PassedTime = (Int32)notification.Value.Value;
                    return;
                }
                if (monitoredItem.DisplayName == "Program_state_StartTime")
                {

                    programState.StartTime = (Int32)notification.Value.Value;
                    return;
                }
                if (monitoredItem.DisplayName == "Program_step_Acceleration")
                {
                    stepType.Acceleration = (float)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_step_Command")
                {
                    stepType.Command = (programCodes)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_step_Duration")
                {
                    stepType.Duration = (Int32)notification.Value.Value;
                    return;

                }

                if (monitoredItem.DisplayName == "Program_step_EndForce")
                {
                    stepType.EndForce = (float)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_step_Force")
                {
                    stepType.Force = (float)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_step_Passed")
                {
                    stepType.Passed = (Int32)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_step_Position")
                {
                    stepType.Position = (float)notification.Value.Value;
                    return;


                }
                if (monitoredItem.DisplayName == "Program_step_SetAcceleration")
                {
                    stepType.SetAcceleration = (bool)notification.Value.Value;
                    return;


                }
                if (monitoredItem.DisplayName == "Program_step_SetEndForce")
                {
                    stepType.SetEndForce = (bool)notification.Value.Value;
                    return;


                }
                if (monitoredItem.DisplayName == "Program_step_SetForce")
                {
                    stepType.SetForce = (bool)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_step_SetHome")
                {

                    stepType.SetHome = (bool)notification.Value.Value;
                    return;
                }
                if (monitoredItem.DisplayName == "Program_step_SetPosition")
                {
                    stepType.SetPosition = (bool)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_step_SetSpeed")
                {
                    stepType.SetSpeed = (bool)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_step_SetZeroPosition")
                {
                    stepType.SetZeroPosition = (bool)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_step_SetDuration")
                {
                    stepType.SetDuration = (bool)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_step_Speed")
                {
                    stepType.Speed = (float)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_transfer_ActualItem")
                {
                    transfer.ActualItem = (int)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_transfer_ActualStep")
                {
                    transfer.ActualStep = (int)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_transfer_ItemDone")
                {
                    transfer.ItemDone = (bool)notification.Value.Value;
                    return;

                }


                if (monitoredItem.DisplayName == "Program_transfer_WriteProgram")
                {
                    transfer.WriteProgram = (bool)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_transfer_WriteStep")
                {
                    transfer.WriteStep = (bool)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_transfer_WritingDone")
                {
                    transfer.WritingDone = (bool)notification.Value.Value;
                    return;

                }
                if (monitoredItem.DisplayName == "Program_transfer_WritingPorgramDone")
                {
                    transfer.WritingProgramDone = (bool)notification.Value.Value;
                    if (transfer.WritingProgramDone)
                    {
                        buttonStartProgram.Visible = true;
                    }
                    return;

                }
                if (monitoredItem.DisplayName == "Program_transfer_WriteItem")
                {
                    transfer.WriteItem = (bool)notification.Value.Value;
                    return;

                }


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                //throw;
            }

        }

        private void NotificationWriteToValuesPidForce(MonitoredItem monitoredItem, MonitoredItemNotification notification)
        {
            try
            {
                if (monitoredItem.DisplayName == "PositionSt_PID_ActValue")
                {

                    //SetText(textBoxVelocity, notification.Value.ToString());
                    //WriteValuesToChart(textBoxPositionTime.Text, notification.Value.ToString(), ChartsData.ChartVelocityTimeValues, ChartsData.ChartVelocityTimeValuesLock,
                    //    ref lockVelocityTime, ref lastTimeVelocity, ref lastValueVelocity);
                    //WriteValuesToChart(textBoxVelocity.Text, textBoxStrain.Text, ChartsData.ChartVelocityStrainValues, ChartsData.ChartVelocityStrainValuesLock,
                    //     ref lockVelocityStrain, ref lastValueVelocityStrainVelocity, ref lastValueVelocityStrainStrain);

                }
                if (monitoredItem.DisplayName == "PositionSt_PID_ControlError")
                {

                    //SetText(textBoxPosition, notification.Value.ToString());
                    //WriteValuesToChart(textBoxPositionTime.Text, textBoxPosition.Text, ChartsData.ChartPositionTimeValues, ChartsData.ChartPositionTimeValuesLock,
                    //    ref lockPositionTime, ref lastTimePosition, ref lastValuePosition);
                    //WriteValuesToChart(textBoxPosition.Text, textBoxStrain.Text, ChartsData.ChartPositionStrainValues, ChartsData.ChartPositionStrainValuesLock,
                    //    ref lockPositionStrain, ref lastValuePositionStrainPosition, ref lastValuePositionStrainStrain);
                }
                if (monitoredItem.DisplayName == "PositionSt_PID_Error")
                {

                    //SetText(textBoxStrain, notification.Value.ToString());
                    //WriteValuesToChart(textBoxStrainTime.Text, textBoxStrain.Text, ChartsData.ChartStrainTimeValues, ChartsData.ChartStrainTimeValuesLock,
                    //    ref lockStrainTime, ref lastTimeStrain, ref lastValueStrain);
                    //WriteValuesToChart(textBoxVelocity.Text, textBoxStrain.Text, ChartsData.ChartVelocityStrainValues, ChartsData.ChartVelocityStrainValuesLock,
                    //    ref lockVelocityStrain, ref lastValueVelocityStrainVelocity, ref lastValueVelocityStrainStrain);
                    //WriteValuesToChart(textBoxPosition.Text, textBoxStrain.Text, ChartsData.ChartPositionStrainValues, ChartsData.ChartPositionStrainValuesLock,
                    //    ref lockPositionStrain, ref lastValuePositionStrainPosition, ref lastValuePositionStrainStrain);

                }

                if (monitoredItem.DisplayName == "PositionSt_PID_DerivativePart")
                {

                    //SetText(textBoxVelocityTime, notification.Value.ToString());
                    //int val;
                    //bool velParesed = Int32.TryParse(textBoxVelocityTime.Text, out val);
                    ////int truncVal = val / (int)numericUpDownTimeToWriteToChart.Value;
                    //if (val % 10 == 0)
                    //{
                    //    WriteValuesToChart(textBoxVelocityTime.Text, textBoxVelocity.Text, ChartVelocityTimeValues, ChartVelocityTimeValuesLock,
                    //        ref lockVelocityTime, ref lastTimeVelocity, ref lastValueVelocity);
                    //}
                    //writeVelocityToChart(textBoxVelocityTime.Text, textBoxVelocity.Text);

                }
                if (monitoredItem.DisplayName == "PositionSt_PID_IntegrationPart")
                {

                    //SetText(textBoxPositionTime, notification.Value.ToString());
                    //    int val;
                    //    bool velParesed = Int32.TryParse(textBoxPositionTime.Text, out val);

                    //    int truncVal = val / (int)numericUpDownTimeToWriteToChart.Value;
                    //    if (val % 10 == 0)
                    //    {
                    //        WriteValuesToChart(textBoxPositionTime.Text, textBoxPosition.Text, ChartPositionTimeValues, ChartPositionTimeValuesLock, lockPositionTime);
                    //    }
                }
                if (monitoredItem.DisplayName == "PositionSt_PID_Out")
                {


                }

                if (monitoredItem.DisplayName == "PositionSt_PID_ProportionalPart")
                {


                }
                if (monitoredItem.DisplayName == "PositionSt_PID_ProportionalPart")
                {


                }
                if (monitoredItem.DisplayName == "MachineControl_status_standartizeWeight1")
                {
                    if ((bool)notification.Value.Value)
                    {
                        buttonLowForce.BackColor = Color.Red;
                        buttonLowForce.ForeColor = Color.Black;


                    }
                    else
                    {
                        buttonLowForce.BackColor = SystemColors.Control;
                        buttonLowForce.Text = "Low Force";
                        commandLowForceStart = false;

                    }


                }
                if (monitoredItem.DisplayName == "MachineControl_status_standartizeWeight2")
                {
                    if ((bool)notification.Value.Value)
                    {
                        buttonHighForce.BackColor = Color.Red;
                        buttonHighForce.ForeColor = Color.Black;


                    }
                    else
                    {
                        buttonHighForce.BackColor = SystemColors.Control;
                        buttonHighForce.Text = "High Force";
                        commandHighForceStart = false;

                    }


                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                //throw;
            }

        }

        private void Notification_KeepAlive(Session sender, KeepAliveEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new KeepAliveEventHandler(Notification_KeepAlive), sender, e);
                return;
            }

            try
            {
                // check for events from discarded sessions.
                if (!Object.ReferenceEquals(sender, mySession))
                {
                    return;
                }

                // check for disconnected session.
                if (!ServiceResult.IsGood(e.Status))
                {
                    // try reconnecting using the existing session state
                    mySession.Reconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                ResetUI();
            }
        }
        #endregion

        /// <summary>
        /// Private methods for UI handling
        /// </summary>
        #region PrivateMethods
        private void ResetUI()
        {
            descriptionGridView.Rows.Clear();
            nodeTreeView.Nodes.Clear();
            myReferenceDescriptionCollection = null;
            structGridView.Rows.Clear();
            inputArgumentsGridView.Rows.Clear();
            outputArgumentsGridView.Rows.Clear();
            myStructList = null;

            subscriptionTextBox.Text = "";
            subscriptionIdTextBox.Text = "";
            readIdTextBox.Text = "";
            writeIdTextBox.Text = "";
            readTextBox.Text = "";
            writeTextBox.Text = "";
            rgReadTextBox.Text = "";
            rgWriteTextBox.Text = "";
            rgNodeIdTextBox.Text = "";
            regNodeIdTextBox.Text = "";
            epConnectServerButton.Text = "Connect to server";

            browsePage.Enabled = false;
            rwPage.Enabled = false;
            subscribePage.Enabled = false;
            structPage.Enabled = false;
            methodPage.Enabled = false;

            opcTabControl.SelectedIndex = 0;

        }


        private void buttonSaveSelectedEndPoint_Click(object sender, EventArgs e)
        {

        }

        private void buttonSubcsribeAll_Click(object sender, EventArgs e)
        {
            int numberIfItems = namesOfVariblesPLC.GetLength(1);

            ChartsData.endOfSubstription = false;
            try
            {
                //use different item names for correct assignment at the notificatino event
                if (mySubscription == null)
                {
                    mySubscription = ChartsData.myClientHelperAPI.Subscribe(500);
                    for (int i = 0; i < numberIfItems; i++)

                    {
                        //use different item names for correct assignment at the notificatino event

                        string monitoredItemName = namesOfVariblesPLC[1, i];
                        string nodeName = namesOfVariblesPLC[0, i];
                        //SubsribeURL(nodeName, monitoredItemName);
                        myMonitoredItem = ChartsData.myClientHelperAPI.AddMonitoredItem(mySubscription, nodeName, monitoredItemName, 500, Notification_MonitoredItem);

                    }
                }

                ChartsData.myClientHelperAPI.ItemChangedNotification += new MonitoredItemNotificationEventHandler(Notification_MonitoredItem);
                numberIfItems = namesOfVariblesPLCFast.GetLength(1);
                if (mySubscriptionFast == null)
                {
                    mySubscriptionFast = ChartsData.myClientHelperAPI.Subscribe(50);
                    for (int i = 0; i < numberIfItems; i++)

                    {
                        //use different item names for correct assignment at the notificatino event
                        itemCount++;
                        string monitoredItemName = namesOfVariblesPLCFast[1, i];
                        string nodeName = namesOfVariblesPLCFast[0, i];
                        //SubsribeURL(nodeName, monitoredItemName);
                        myMonitoredItem = ChartsData.myClientHelperAPI.AddMonitoredItem(mySubscriptionFast, nodeName, monitoredItemName, 50, Notification_MonitoredItem_Fast);

                    }
                    ChartsData.myClientHelperAPI.ItemChangedNotification += new MonitoredItemNotificationEventHandler(Notification_MonitoredItem_Fast);

                }
                numberIfItems = namesOfVariblesPLCProgram.GetLength(1);
                if (mySubscriptionProgram == null)
                {
                    mySubscriptionProgram = ChartsData.myClientHelperAPI.Subscribe(200);
                    for (int i = 0; i < numberIfItems; i++)

                    {
                        //use different item names for correct assignment at the notificatino event
                        itemCount++;
                        string monitoredItemName = namesOfVariblesPLCProgram[1, i];
                        string nodeName = namesOfVariblesPLCProgram[0, i];
                        //SubsribeURL(nodeName, monitoredItemName);
                        myMonitoredItem = ChartsData.myClientHelperAPI.AddMonitoredItem(mySubscriptionProgram, nodeName, monitoredItemName, 200, Notification_MonitoredItem_Program);

                    }
                    ChartsData.myClientHelperAPI.ItemChangedNotification += new MonitoredItemNotificationEventHandler(Notification_MonitoredItem_Program);

                }
                numberIfItems = namesOfFiltersRamps.GetLength(1);
                if (mySubscriptionFiltersAndRamps == null)
                {
                    mySubscriptionFiltersAndRamps = ChartsData.myClientHelperAPI.Subscribe(200);
                    for (int i = 0; i < numberIfItems; i++)

                    {
                        //use different item names for correct assignment at the notificatino event
                        itemCount++;
                        string monitoredItemName = namesOfFiltersRamps[1, i];
                        string nodeName = namesOfFiltersRamps[0, i];
                        //SubsribeURL(nodeName, monitoredItemName);
                        myMonitoredItem = ChartsData.myClientHelperAPI.AddMonitoredItem(mySubscriptionFiltersAndRamps, nodeName, monitoredItemName, 200, Notification_MonitoredItem_FiltersAndRamps);

                    }
                    ChartsData.myClientHelperAPI.ItemChangedNotification += new MonitoredItemNotificationEventHandler(Notification_MonitoredItem_FiltersAndRamps);

                }
                numberIfItems = namesOfConstats.GetLength(1);
                if (mySubscriptionConstants == null)
                {
                    mySubscriptionConstants = ChartsData.myClientHelperAPI.Subscribe(200);
                    for (int i = 0; i < numberIfItems; i++)

                    {
                        //use different item names for correct assignment at the notificatino event
                        itemCount++;
                        string monitoredItemName = namesOfConstats[1, i];
                        string nodeName = namesOfConstats[0, i];
                        //SubsribeURL(nodeName, monitoredItemName);
                        myMonitoredItem = ChartsData.myClientHelperAPI.AddMonitoredItem(mySubscriptionConstants, nodeName, monitoredItemName, 200, Notification_MonitoredItem_Constants);

                    }
                    ChartsData.myClientHelperAPI.ItemChangedNotification += new MonitoredItemNotificationEventHandler(Notification_MonitoredItem_Constants);

                }
                ChartsData.endOfSubstription = true;
                toolStripButtonChartTime.Enabled = true;
                toolStripButtonPositionForce.Enabled = true;
                toolStripButtonVelocityForce.Enabled = true;
                toolStripButtonPID.Enabled = true;
                groupBoxControl.Enabled = true;
                groupBoxForce.Enabled = true;
                groupBoxModeSelection.Enabled = true;
                groupBoxPosition.Enabled = true;
                groupBoxVelocity.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                ChartsData.endOfSubstription = true;
            }

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxDisplayValue_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBoxNumberOfPointsInChart_TextChanged(object sender, EventArgs e)
        {

        }

        private void SetAxisLimits(int points)
        {
            //double numValues = 0;
            //int maxValues = points;

            try
            {
                if (points < chartForceVelocityForm.cartesianChartVelocityForce.AxisX[0].MinValue - chartForceVelocityForm.cartesianChartVelocityForce.AxisX[0].MaxValue)
                {
                    chartForceVelocityForm.cartesianChartVelocityForce.AxisX[0].MinValue = chartForceVelocityForm.cartesianChartVelocityForce.AxisX[0].MaxValue - points;
                }
                else
                {
                    //myChart.AxisX[0].MinValue = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                ChartsData.endOfSubstription = true;
            }



        }
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
                ChartsData.endOfSubstription = true;
            }

        }

        private void buttonChartVelocityClear(object sender, EventArgs e)
        {
            chartForceVelocityForm.cartesianChartVelocityForce.Series[0].Erase(true);
        }


        public delegate void WriteToBoxEventHandler(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e);
        public delegate void WriteToValuesEventHandler(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e);


        private void buttonFastUp_Click(object sender, EventArgs e)
        {

        }

        private void buttonFastUp_MouseDown(object sender, MouseEventArgs e)
        {
            writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.manual.upFast");
        }

        private void buttonFastUp_MouseUp(object sender, MouseEventArgs e)
        {
            writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.manual.upFast");
        }

        private void buttonUp_MouseDown(object sender, MouseEventArgs e)
        {
            writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.manual.upLow");
        }

        private void buttonUp_MouseUp(object sender, MouseEventArgs e)
        {
            writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.manual.upLow");

        }

        private void buttonFastDown_MouseDown(object sender, MouseEventArgs e)
        {
            writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.manual.downFast");
        }

        private void buttonFastDown_MouseUp(object sender, MouseEventArgs e)
        {
            writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.manual.downFast");

        }

        private void buttonDowm_MouseDown(object sender, MouseEventArgs e)
        {
            writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.manual.downLow");
        }

        private void buttonDowm_MouseUp(object sender, MouseEventArgs e)
        {
            writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.manual.downLow");

        }

        private void buttonPoverOn_Click(object sender, EventArgs e)
        {
            if (buttonPowerOnStateMemory)
            {
                buttonPoverOnCommand.Text = "ZAPNOUT";
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.powerOn");
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.control");
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.simulate");
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.manual.manual");
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.position");
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.velocity");
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.force");
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.manPID");

                radioButtonForce.Checked = false;
                radioButtonManual.Checked = false;
                radioButtonProgram.Checked = false;
                radioButtonVelocity.Checked = false;
                radioButtonReady.Checked = false;
                textBoxPositionSet.Text = 0.ToString();
                textBoxVelocitySet.Text = 0.ToString();
                textBoxStrainSet.Text = 0.ToString();
                buttonStop.PerformClick();
                buttonDrive.BackColor = SystemColors.Control;
                buttonPowerOnStateMemory = false;


            }
            else
            {
                if (!machineError & buttonPowerOnState.BackColor == Color.Green)
                {
                    radioButtonReady.Checked = buttonPowerOnState.BackColor == Color.Green;
                    buttonDrive.BackColor = Color.Green;
                    buttonPoverOnCommand.Text = "VYPNOUT";
                    writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.powerOn");
                    writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.control");
                    buttonPowerOnStateMemory = true;
                }
                else
                {
                    return;
                }
            }

        }

        private void buttonStop_MouseDown(object sender, MouseEventArgs e)
        {
            //buttonPowerOnStateMemory = false;
            //buttonPoverOnCommand.Text = "ZAPNOUT";
            //writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.powerOn");
            //radioButtonForce.Checked = false;
            //radioButtonManual.Checked = false;
            //radioButtonPosition.Checked = false;
            //radioButtonVelocity.Checked = false;
            //radioButtonReady.Checked = false;
            //radioButtonProgram.Checked = false;
            //textBoxPositionSet.Text = 0.ToString();
            //textBoxVelocitySet.Text = 0.ToString();
            //textBoxStrainSet.Text = 0.ToString();
            //writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.restart");
            //writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.control");


        }

        private void radioButtonManual_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxStatusManual.Checked || !radioButtonReady.Checked)
            {
                radioButtonManual.Checked = false;
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.manual.manual");
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");
            }
            else
            {
                if (radioButtonManual.Checked)

                {
                    writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.manual.manual");
                    writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");
                }
                else
                {
                    writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.manual.manual");
                    writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");
                }

            }
        }




        private void radioButtonVelocity_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxStatusVelocity.Checked || !radioButtonReady.Checked)
            {
                radioButtonVelocity.Checked = false;
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.velocity");
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");
            }
            else
            {
                if (radioButtonVelocity.Checked)

                {
                    writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.velocity");
                    if (checkBoxVelocityPidMan.Checked)
                    {

                        writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");
                    }
                    else
                    {

                        writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");

                    }
                }
                else
                {
                    writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.velocity");
                    writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");
                }

            }
        }

        private void radioButtonForce_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxStatusForce.Checked || !radioButtonReady.Checked)
            {
                radioButtonForce.Checked = false;
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.force");
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.manPID");
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");
            }
            else
            {
                if (radioButtonForce.Checked)

                {
                    writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.force");
                    if (checkBoxVelocityPidMan.Checked)
                    {

                        writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");
                    }
                    else
                    {

                        writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");

                    }
                    if (checkBoxForcePidMan.Checked)
                    {

                        writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.manPID");
                    }
                    else
                    {

                        writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.manPID");

                    }

                }
                else
                {
                    writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.force");
                    writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.manPID");
                    writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");


                }

            }
        }

        private void radioButtonPosition_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxStatusPosition.Checked || !radioButtonReady.Checked)
            {
                radioButtonPosition.Checked = false;
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.position");
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.manPID");
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");
            }
            else
            {
                if (radioButtonPosition.Checked)

                {
                    writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.position");
                    writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.force");
                    if (checkBoxVelocityPidMan.Checked)
                    {

                        writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");
                    }
                    else
                    {

                        writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");

                    }
                    if (checkBoxVelocityPidMan.Checked)
                    {

                        writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.manPID");
                    }
                    else
                    {

                        writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.manPID");

                    }
                }
                else
                {
                    writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.position");
                    writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.manPID");
                    writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");
                }

            }
        }

        private void radioButtonProgram_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxStatusProgram.Checked || !radioButtonReady.Checked)
            {
                radioButtonProgram.Checked = false;
            }
            else
            {
                if (radioButtonProgram.Checked)

                {
                    writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.programControl");
                }
                else
                {
                    writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.programControl");
                }

            }
        }

        private void radioButtonReady_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonReady.Checked)
            {
                radioButtonProgram.Checked = false;
                radioButtonPosition.Checked = false;
                radioButtonManual.Checked = false;
                radioButtonForce.Checked = false;
                radioButtonVelocity.Checked = false;
            }

        }



        private void textBoxVelocitySet_TextChanged(object sender, EventArgs e)
        {
            double val;

            {
                if (!double.TryParse(textBoxVelocitySet.Text, out val))
                {
                    textBoxVelocitySet.BackColor = Color.Red;
                }
                else
                {
                    textBoxVelocitySet.BackColor = Color.White;
                };



            }

        }


        private void discoveryTextBox_TextChanged(object sender, EventArgs e)
        {

        }



        private void buttonTara_Click(object sender, EventArgs e)
        {
            string message = "Vytárovat snímač??";
            string caption = "Tárování";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (!commandTaraStart)
                {

                    buttonTara.Text = "Wait";
                    buttonTara.ForeColor = Color.Red;
                    writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.tara");
                    commandTaraStart = true;


                }
            }


        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            //
        }


        private void checkBoxFilterOnVelocity_CheckedChanged(object sender, EventArgs e)
        {
            string strVal;
            if (checkBoxFilterOnVelocity.Checked)
            {
                strVal = vTrue;
            }
            else
            {
                strVal = vFalse;
            }
            writeNode(strVal, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.filterOn");
        }

        private void checkBoxFilterOnForce_CheckedChanged(object sender, EventArgs e)
        {
            string strVal;
            if (checkBoxFilterOnForce.Checked)
            {
                strVal = vTrue;
            }
            else
            {
                strVal = vFalse;
            }
            writeNode(strVal, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.filterOn");
        }

        private void checkBoxFilterOnPosition_CheckedChanged(object sender, EventArgs e)
        {
            string strVal;
            if (checkBoxFilterOnPosition.Checked)
            {
                strVal = vTrue;
            }
            else
            {
                strVal = vFalse;
            }
            writeNode(strVal, "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.filterOn");
        }

        private void checkBoxRampOnVelocity_CheckedChanged(object sender, EventArgs e)
        {
            string strVal;
            if (checkBoxRampOnVelocity.Checked)
            {
                strVal = vTrue;
            }
            else
            {
                strVal = vFalse;
            }
            writeNode(strVal, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.rampOn");
        }

        private void checkBoxRampOnForce_CheckedChanged(object sender, EventArgs e)
        {
            string strVal;
            if (checkBoxRampOnForce.Checked)
            {
                strVal = vTrue;
            }
            else
            {
                strVal = vFalse;
            }
            writeNode(strVal, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.rampOn");
        }

        private void checkBoxRampOnPosition_CheckedChanged(object sender, EventArgs e)
        {
            string strVal;
            if (checkBoxRampOnPosition.Checked)
            {
                strVal = vTrue;
            }
            else
            {
                strVal = vFalse;
            }
            writeNode(strVal, "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.rampOn");
        }

        private void checkBoxStatusVelocity_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBoxVelocitySet_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                try
                {
                    double.Parse(textBoxVelocitySet.Text);
                    writeNode(textBoxVelocitySet.Text, "ns=6;s=::AsGlobalPV:MachineControl.input.velocity.SetValue");
                    textBoxVelocitySet.BackColor = Color.LightBlue;

                }
                catch (Exception)
                {

                    textBoxVelocitySet.BackColor = Color.Red;


                }
            }

        }

        private void toolStripButtonChartTime_Click(object sender, EventArgs e)
        {

            chartTimeForm.Visible = !chartTimeForm.Visible;
        }

        private void toolStripButtonChartForcePosition_Click(object sender, EventArgs e)
        {
            chartForcePositionForm.Visible = !chartForcePositionForm.Visible;
        }

        private void toolStripButtonForceVelocity_Click(object sender, EventArgs e)
        {

            chartForceVelocityForm.Visible = !chartForceVelocityForm.Visible;
        }

        private void cartesianChartPidTimeVelocity_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void toolStripButtonServis_Click(object sender, EventArgs e)
        {
            ChartPID_TimeForceForm formChartPidTimeForce = new ChartPID_TimeForceForm(this);
            formChartPidTimeForce.Show();

        }

        private void buttonTuneVelocity_Click(object sender, EventArgs e)
        {

        }

        private void textBoxPositionSet_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxPositionSet);
        }

        private static double testNumericValueAndSetColor(TextBox textBox)
        {
            double val;
            {
                if (!double.TryParse(textBox.Text, out val))
                {
                    textBox.BackColor = Color.Red;
                }
                else
                {
                    textBox.BackColor = Color.White;
                }



            }

            return val;
        }

        private void textBoxPositionSet_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:MachineControl.input.position.SetValue";
            sendValueFromTextBoxToPLC(e, command, textBoxPositionSet);
        }

        private void sendValueFromTextBoxToPLC(KeyPressEventArgs e, string command, TextBox textBox)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                try
                {
                    double.Parse(textBox.Text);
                    writeNode(textBox.Text, command);
                    textBox.BackColor = Color.LightBlue;

                }
                catch (Exception)
                {

                    textBox.BackColor = Color.Red;


                }
            }
        }

        private void textBoxStrainSet_TextChanged(object sender, EventArgs e)
        {
            double val;

            {
                if (!double.TryParse(textBoxStrainSet.Text, out val))
                {
                    textBoxStrainSet.BackColor = Color.Red;
                }
                else
                {
                    textBoxStrainSet.BackColor = Color.White;
                };



            }
        }

        private void textBoxStrainSet_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                try
                {
                    double.Parse(textBoxStrainSet.Text);
                    writeNode(textBoxStrainSet.Text, "ns=6;s=::AsGlobalPV:MachineControl.input.strain.setValue");
                    textBoxStrainSet.BackColor = Color.LightBlue;


                }
                catch (Exception)
                {

                    textBoxStrainSet.BackColor = Color.Red;


                }
            }
        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            buttonPowerOnStateMemory = false;
            buttonPoverOnCommand.Text = "ZAPNOUT";
            writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.powerOn");
            writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.simulate");
            radioButtonForce.Checked = false;
            radioButtonManual.Checked = false;
            radioButtonPosition.Checked = false;
            radioButtonVelocity.Checked = false;
            radioButtonReady.Checked = false;
            radioButtonProgram.Checked = false;
            textBoxPositionSet.Text = 0.ToString();
            textBoxVelocitySet.Text = 0.ToString();
            textBoxStrainSet.Text = 0.ToString();
            writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.restart");
            writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.control");
            writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.doStandartize2");
            writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.doStandartize1");
            writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.command.tara");
            buttonLowForce.Text = "Low Force";
            buttonHighForce.Text = "High Force";
            buttonTara.Text = "Tara";
            buttonLowForce.ForeColor = Color.Black;
            buttonHighForce.ForeColor = Color.Black;
            buttonTara.ForeColor = Color.Black;
            buttonLowForce.BackColor = SystemColors.Control;
            buttonHighForce.BackColor = SystemColors.Control;
            buttonTara.BackColor = SystemColors.Control;
            commandTaraStart = false;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }


        public delegate void writeValuesToChartEventHandler(string time, string vel, ChartValues<ObservablePoint> val, bool lck);

        private void pIDPoziceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formChartPidTimePosition.Visible = !formChartPidTimePosition.Visible;
            //if (formChartPidTimePosition == null)
            //{
            //    formChartPidTimePosition = new ChartPID_TimePositionForm(mainForm);
            //    formChartPidTimePosition.Visible = true;
            //}
            //else
            //{
            //    formChartPidTimePosition.Visible = !formChartPidTimePosition.Visible;

            //}
        }

        private void pIDRychlostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formChartPidTimeVelocity.Visible = !formChartPidTimeVelocity.Visible;
            //if (formChartPidTimeVelocity == null)
            //{
            //    formChartPidTimeVelocity = new ChartPID_TimeVelocityForm(mainForm);
            //    formChartPidTimeVelocity.Visible = true;
            //}
            //else
            //{
            //    formChartPidTimeVelocity.Visible = !formChartPidTimeVelocity.Visible;

            //}
        }

        private void pIDSílaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formChartPidTimeForce.Visible = !formChartPidTimeForce.Visible;
            //if (formChartPidTimeForce == null)
            //{
            //    formChartPidTimeForce = new ChartPID_TimeForceForm(mainForm);
            //    formChartPidTimeForce.Visible = true;
            //}
            //else
            //{
            //    formChartPidTimeForce.Visible = !formChartPidTimeForce.Visible;

            //}
        }

        private void buttonDrive_SystemColorsChanged(object sender, EventArgs e)
        {

        }

        private void buttonDrive_BackColorChanged(object sender, EventArgs e)
        {
            if (buttonDrive.BackColor == Color.Green)
            {
                radioButtonReady.Checked = true;
            }
            else
            {
                radioButtonReady.Checked = false;
            }
        }

        private void buttonPowerOnState_BackColorChanged(object sender, EventArgs e)
        {
            if (buttonPowerOnState.BackColor == Color.Red)
            {
                buttonStop.PerformClick();
            }

        }

        private void buttonErr_BackColorChanged(object sender, EventArgs e)
        {
            if (buttonErr.BackColor == Color.Red)
            {
                string message = "Restartovat systém?";
                string caption = "Chyba hardware";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    buttonPoverOnCommand.PerformClick();
                }


            }
        }

        private void buttonHighForce_Click(object sender, EventArgs e)
        {

            if (buttonHighForce.Text == "Wait")
            {
                return;
            }
            else
            {
                ;
                DialogResult result;
                float value;
                string message = "Chybně zadaná hodnota " + textBoxActualStdHighEcho.Text + "?";
                string caption = "Chyba zadání";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                if (!float.TryParse(textBoxActualStdHighEcho.Text, out value))
                {

                    MessageBox.Show(message, caption, buttons, MessageBoxIcon.Error);
                    return;
                }
                writeNode(textBoxActualStdHighEcho.Text, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.scaleParameter.ref2_std");
                message = "Nastavit horní hodnotu snímače: " + textBoxActualStdHighEcho.Text + "?";
                caption = "Nastavit horní hodnotu";
                buttons = MessageBoxButtons.YesNo;
                result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    buttonHighForce.Text = "Wait";
                    buttonHighForce.ForeColor = Color.Red;
                    commandHighForceStart = true;
                    writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.doStandartize2");
                }


            }
        }



        private void buttonLowForce_Click(object sender, EventArgs e)
        {
            if (buttonLowForce.Text == "Wait")
            {
                return;
            }
            else
            {
                float value;
                string message = "Chybně zadaná hodnota " + textBoxActualStdLowEcho.Text + "?";
                string caption = "Chyba zadání";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                if (!float.TryParse(textBoxActualStdLowEcho.Text, out value))
                {

                    MessageBox.Show(message, caption, buttons, MessageBoxIcon.Error);
                    return;
                }
                writeNode(textBoxActualStdLowEcho.Text, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.scaleParameter.ref1_std");
                message = "Nastavit spodní hodnotu snímače: " + textBoxActualStdLowEcho.Text + "?";
                caption = "Nastavit spodní hodnotu";
                buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    buttonLowForce.Text = "Wait";
                    buttonLowForce.ForeColor = Color.Red;
                    commandLowForceStart = true;
                    writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.doStandartize1");
                }
            }
        }

        private void connectPage_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxVelocityPidMan_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxVelocityPidMan.Checked)
            {
                writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");
            }

            else
            {
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.velocityControl.command.manPID");
            }
        }

        private void checkBoxForcePidMan_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxForcePidMan.Checked)
            {
                writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.manPID");
            }

            else
            {
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.strainControl.command.manPID");
            }
        }

        private void checkBoxPositionPidMan_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPositionPidMan.Checked)
            {
                writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.manPID");
            }

            else
            {
                writeNode(vFalse, "ns=6;s=::AsGlobalPV:MachineControl.positionControl.command.manPID");
            }
        }

        private void textBoxPositionHighLimitSet_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxPositionHighLimitSet);
        }

        private void textBoxPositionHighLimitSet_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBoxPositionLowLimitSet_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBoxPositionLowLimitSet_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxPositionLowLimitSet);
        }

        private void textBoxMaxVelocitySet_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBoxMaxVelocitySet_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxMaxVelocitySet);
        }

        private void textBoxMaxAccelerationSet_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBoxMaxAccelerationSet_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxMaxAccelerationSet);
        }

        private void buttonReadProgram_Click(object sender, EventArgs e)
        {

        }

        private void buttonNewProgram_Click(object sender, EventArgs e)
        {
            formProgramForm.Visible = true;
        }

        private void buttonWriteProgram_Click(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void buttonReadProgram_Click_1(object sender, EventArgs e)
        {
            string fileName = string.Empty;
            dataGridViewActualProgram.Rows.Clear();
            if ((fileName = formProgramForm.FileRead(dataGridViewActualProgram)) != null)
            {
                textBoxProgramFileName.Text = fileName;
                buttonDownloadProgram.Visible = true;
                dataGridViewActualProgram.Visible = true;
            }
            else
            {
                textBoxProgramFileName.Text = null;
                buttonDownloadProgram.Visible = false;
                buttonStartProgram.Visible = false;
                dataGridViewActualProgram.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridViewActualProgram.Rows.Clear();
        }

        private void buttonDownloadProgram_Click(object sender, EventArgs e)
        {
            programming.startDownload();



        }

        private void UAClientForm_Load(object sender, EventArgs e)
        {

        }

        public delegate void timerChartRedraw_TickEventHandler(object sender, EventArgs e);

        private void buttonTestProgram_Click(object sender, EventArgs e)
        {
            StepType step = new StepType();

            try
            {
                programming.Row = int.Parse(textBoxRow.Text);
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                dataGridViewRow = dataGridViewActualProgram.Rows[programming.Row];
                programming.programCommand(dataGridViewRow, programming.Row);
            }
            catch (Exception)
            {


            }

            return;




        }

        private void buttonStartProgram_Click(object sender, EventArgs e)
        {
            writeNode(vTrue, "ns=6;s=::AsGlobalPV:MachineControl.command.programControl");

        }

        private void dataGridViewActualProgram_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CheckBoxVelocityMax_CheckedChanged(object sender, EventArgs e)
        {
            int maxVelocity = 0;
            if (int.TryParse(textBoxMaxVelocitySet.Text, out maxVelocity))
            {

            }
            else
            {

            }
        }

        private void textBoxHighForceRawLow_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxHighForceRawLow);
        }


        private void textBoxHighForceRawHigh_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxHighForceRawHigh);
        }


        private void textBoxLowForceRawLow_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxLowForceRawLow);
        }

        private void textBoxLowForceEngLow_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxLowForceStdLow);
        }

        private void textBoxLowForceRawHigh_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxLowForceRawHigh);
        }

        private void textBoxLowForceEngHigh_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxLowForceStdHigh);
        }

        private void textBoxHighForceRawLow_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.ref1_raw";
            sendValueFromTextBoxToPLC(e, command, textBoxHighForceRawLow);
        }


        private void textBoxHighForceRawHigh_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.ref2_raw";
            sendValueFromTextBoxToPLC(e, command, textBoxHighForceRawHigh);
        }

        private void textBoxLowForceRawLow_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.ref1_raw";
            sendValueFromTextBoxToPLC(e, command, textBoxLowForceRawLow);
        }


        private void textBoxLowForceRawHigh_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.ref2_raw";
            sendValueFromTextBoxToPLC(e, command, textBoxLowForceRawHigh);
        }

        private void textBoxHighCellTara_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxHighForceTara);
        }

        private void textBoxHighCellTara_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.tare_std";
            sendValueFromTextBoxToPLC(e, command, textBoxPositionSet);
        }

        private void textBoxLowCellTara_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxLowForceTara);
        }

        private void textBoxLowCellTara_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:MachineControl.input.position.SetValue";
            sendValueFromTextBoxToPLC(e, command, textBoxPositionSet);
        }

        private void textBoxHighForceStdLow_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.ref1_std";
            sendValueFromTextBoxToPLC(e, command, textBoxHighForceStdLow);
        }

        private void textBoxHighForceStdHigh_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.ref2_std";
            sendValueFromTextBoxToPLC(e, command, textBoxHighForceStdHigh);
        }

        private void textBoxHighForceStdHigh_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxHighForceStdHigh);
        }

        private void textBoxHighForceStdLow_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxHighForceStdLow);
        }

        private void textBoxLowForceStdLow_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxLowForceStdLow);
        }

        private void textBoxLowForceStdHigh_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxLowForceStdHigh);
        }

        private void textBoxLowForceStdHigh_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.ref2_std";
            sendValueFromTextBoxToPLC(e, command, textBoxLowForceStdHigh);
        }

        private void textBoxLowForceStdLow_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.ref1_std";
            sendValueFromTextBoxToPLC(e, command, textBoxLowForceStdLow);
        }

        private void textBoxHighForceTara_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxHighForceTara);
        }

        private void textBoxHighForceTara_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell1.tare_std";
            sendValueFromTextBoxToPLC(e, command, textBoxHighForceTara);
        }

        private void textBoxLowForceTara_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxLowForceTara);
        }

        private void textBoxLowForceTara_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.tare_std";
            sendValueFromTextBoxToPLC(e, command, textBoxLowForceTara);
        }

        private void radioButtonLoadCellHigh_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLoadCellHigh.Checked)
            {
                radioButtonLoadCellLow.Checked = false;
                writeNode("1", "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.status.actualCellType");

            }
        }

        private void radioButtonLoadCellLow_EnabledChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonLoadCellLow_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLoadCellLow.Checked)
            {
                radioButtonLoadCellHigh.Checked = false;
                writeNode("2", "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.status.actualCellType");

            }
        }

        private void textBoxActualRawLowEcho_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxLowForceTara);
        }

        private void textBoxHighForceTaraEcho_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxHighForceTaraEcho);
        }

        private void textBoxLowForceTaraEcho_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxLowForceTaraEcho);
        }

        private void textBoxHighForceTaraEcho_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.tare_std";
            sendValueFromTextBoxToPLC(e, command, textBoxHighForceTaraEcho);
        }

        private void textBoxLowForceTaraEcho_KeyPress(object sender, KeyPressEventArgs e)
        {
            string command = "ns=6;s=::AsGlobalPV:constMachineControl.weight.parameterLoadCell2.tare_std";
            sendValueFromTextBoxToPLC(e, command, textBoxLowForceTaraEcho);
        }

        private void textBoxActualStdHighEcho_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxActualStdHighEcho);
        }

        private void textBoxActualStdLowEcho_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxActualStdLowEcho);
        }

        private void textBoxPositionFiltrOrder_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxPositionFiltrOrder);
        }

        private void textBoxPositionFilterCutOffFreq_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxPositionFilterCutOffFreq);
        }

        private void textBoxPositionRampMax_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxPositionRampMax);
        }

        private void textBoxPositionRampSlewRate_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxPositionRampSlewRate);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxPositionRampSlewRate);
        }

        private void textBoxPositionRampMin_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxPositionRampMin);
        }

        private void textBoxPositionRampSlewRateNeg_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxPositionRampSlewRateNeg);
        }

        private void textBoxVelocityFiltrOrder_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxVelocityFiltrOrder);
        }

        private void textBoxVelocityFilterCutOffFreq_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxVelocityFilterCutOffFreq);
        }



        private void textBoxVelocityRampMax_TextChanged_1(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxVelocityRampMax);
        }

        private void textBoxVelocityRampSlewRate_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxVelocityRampSlewRate);
        }

        private void textBoxVelocityRampMin_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxVelocityRampMin);
        }

        private void textBoxVelocityRampSlewRateNeg_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxVelocityRampSlewRateNeg);
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxVelocityRampSlewRateNeg);
        }

        private void textBoxStrainFiltrOrder_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxStrainFiltrOrder);
        }

        private void textBoxStrainFilterCutOffFreq_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxStrainFilterCutOffFreq);
        }

        private void textBoxStrainRampMax_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxStrainRampMax);
        }

        private void textBoxStrainRampSlewRate_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxStrainRampSlewRate);
        }

        private void textBoxStrainRampMin_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxStrainRampMin);
        }

        private void textBoxStrainRampSlewRateNeg_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxStrainRampSlewRateNeg);
        }

        private void buttonSaveForceParametersHigh_Click(object sender, EventArgs e)
        {
            sendCommandWithDialogWriteButtonText(buttonSaveForceParametersHigh,
                "Zapiš",
                "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.writeToMemory1",
                "Uložit parametry čidla do paměti?",
                "Zápis do paměti!");
        }

        private void buttonReadCellParametersHigh_Click(object sender, EventArgs e)
        {
            sendCommandWithDialog(buttonReadCellParametersHigh,
                commandReadCellParametersHigh,
                "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.getParameters1",
                "Načíst data?",
                "Čtení");
        }

        private void sendCommandWithDialog(Button button, bool command, string commandToPlc, string message, string caption)
        {

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (!command)
                {

                    button.Text = "Wait";
                    button.ForeColor = Color.Red;
                    writeNode(vTrue, commandToPlc);
                    command = true;


                }
            }
        }
        private void sendCommandWithDialogWriteButtonText(Button button, string text, string commandToPlc, string message, string caption)
        {

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (button.Text == text)
                {

                    button.Text = "Wait";
                    button.ForeColor = Color.Red;
                    writeNode(vTrue, commandToPlc);



                }
            }
        }
        private void buttonSaveScaleParameters_Click(object sender, EventArgs e)
        {
            string memory = "";
            string memoryPLC = "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.writeToMemory";
            string memoryPLC1 = "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.writeToMemory1";
            string memoryPLC2 = "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.writeToMemory2";
            int valI;
            double valD;
            if (radioButtonLoadCellHigh.Checked)
            {
                memory = "1";
            }
            else
            {
                memory = "2";
            }
            try
            {
                int.TryParse(textBoxActualLRawHighEcho.Text, out valI);
                int.TryParse(textBoxActualRawLowEcho.Text, out valI);
                int.TryParse(textBoxActualStdHighEcho.Text, out valI);
                int.TryParse(textBoxActualStdLowEcho.Text, out valI);
                int.TryParse(textBoxTaraEcho.Text, out valI);
                string command = "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.scaleParameter.ref2_raw";
                writeNodeAndSetColorLightBlue(textBoxActualLRawHighEcho, command);
                command = "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.scaleParameter.ref1_raw";
                writeNodeAndSetColorLightBlue(textBoxActualRawLowEcho, command);
                command = "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.scaleParameter.ref2_std";
                writeNodeAndSetColorLightBlue(textBoxActualStdHighEcho, command);
                command = "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.scaleParameter.ref1_std";
                writeNodeAndSetColorLightBlue(textBoxActualStdLowEcho, command);
                command = "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.parameter.scaleParameter.tare_std";
                writeNodeAndSetColorLightBlue(textBoxTaraEcho, command);


            }
            catch (Exception)
            {


            }
            sendCommandWithDialogWriteButtonText(buttonSaveScaleParameters,
                "Zapiš",
               memoryPLC,
                "Zapsat parametry do pameti" + memory,
                "Zápis do paměti");
            if (radioButtonLoadCellHigh.Checked)
            {
                writeNode(vTrue, memoryPLC1);
            }
            if (radioButtonLoadCellLow.Checked)
            {
                writeNode(vTrue, memoryPLC2);
            }

        }

        private void writeNodeAndSetColorLightBlue(TextBox tb, string command)
        {
            writeNode(tb.Text, command);
            tb.BackColor = Color.LightBlue;
        }

        private void textBoxTaraEcho_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxTaraEcho);
        }

        private void textBoxActualLRawHighEcho_TextChanged(object sender, EventArgs e)
        {
            double val;
            val = testNumericValueAndSetColor(textBoxActualLRawHighEcho);
        }

        private void buttonReadCellParametersLow_Click(object sender, EventArgs e)
        {
            sendCommandWithDialogWriteButtonText(buttonReadCellParametersLow,
                "Načti",
                "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.getParameters2",
                "Načíst data?",
                "Čtení");
        }


        private void buttonReadActualCellParameters_Click(object sender, EventArgs e)
        {
            sendCommandWithDialogWriteButtonText(buttonReadActualCellParameters,
                "Načti",
                "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.getParameters",
                "Načíst data?",
                "Čtení");
        }

        private void buttonSaveForceParametersLow_Click(object sender, EventArgs e)
        {
            sendCommandWithDialogWriteButtonText(buttonSaveForceParametersLow,
                "Zapiš",
                "ns=6;s=::AsGlobalPV:MachineControl.strainControl.controlWeight.command.writeToMemory2",
                "Uložit parametry čidla do paměti?",
                "Zápis do paměti!");
        }

        private void ButtonSetZero_Click(object sender, EventArgs e)
        {

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

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

        private void timerSendProgram_Tick(object sender, EventArgs e)
        {
            int step;
        }

        public delegate void NotificationWriteToValuesEventHandler(MonitoredItem monitoredItem, MonitoredItemNotification notification, MonitoredItemNotificationEventArgs e);
        public delegate void NotificationWriteToTexboxEventHandler(MonitoredItem monitoredItem, MonitoredItemNotification notification);


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




    }
}
#endregion
