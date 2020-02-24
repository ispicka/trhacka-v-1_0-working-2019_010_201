using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trhacka_v_1_0_working_2019_010_201
{
    class DataRecordAllTime
    {
        private double valuePosition;
        private int timePosition;
        private double valueForce;
        private int timeForce;
        private double valueVelocity;
        private int timeVelocity;
        private double valueTemperature;
        private int timeTemperature;

        public double ValuePosition { get => valuePosition; set => valuePosition = value; }
        public int TimePosition { get => timePosition; set => timePosition = value; }
        public double ValueForce { get => valueForce; set => valueForce = value; }
        public int TimeForce { get => timeForce; set => timeForce = value; }
        public double ValueVelocity { get => valueVelocity; set => valueVelocity = value; }
        public int TimeVelocity { get => timeVelocity; set => timeVelocity = value; }
        public double ValueTemperature { get => valueTemperature; set => valueTemperature = value; }
        public int TimeTemperature { get => timeTemperature; set => timeTemperature = value; }
    }
}
