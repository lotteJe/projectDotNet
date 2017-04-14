using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class LogistiekeBesparingViewModel
    {
        public decimal Transport { get; set; }
        public decimal Logistiek { get; set; }
       
        public LogistiekeBesparingViewModel()
        {
            
        }

        public LogistiekeBesparingViewModel(LogistiekeBesparing besparing) : this()
        {
            Transport = (decimal) besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("transportkosten jaarbedrag")).Value;
            Logistiek = (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("logistieke kosten jaarbedrag")).Value;
        }
    }
}
