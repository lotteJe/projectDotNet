using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class EenDecimalViewModel
    {
        public decimal Jaarbedrag { get; set; }

        public EenDecimalViewModel()
        {
            
        }

        public EenDecimalViewModel(Berekening besparing) : this()
        {
            Jaarbedrag = (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.Key.Equals("jaarbedrag")).Value;
        }
    }
}
