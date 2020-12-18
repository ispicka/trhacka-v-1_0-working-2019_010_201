using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace trhacka_v_1_0_working_2019_010_201
{
    sealed class DataRecordAllTimeMap : ClassMap<DataRecordAllTime>
    {
        public DataRecordAllTimeMap()
        {
            AutoMap(System.Globalization. CultureInfo.InvariantCulture);

        }
    }
}
