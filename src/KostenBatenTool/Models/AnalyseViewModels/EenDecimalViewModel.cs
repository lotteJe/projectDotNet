using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class EenDecimalViewModel
    {
        public int AnalyseId { get; set; }
        public decimal Jaarbedrag { get; set; }

        public EenDecimalViewModel()
        {
            
        }

        public EenDecimalViewModel(Berekening besparing, int id) : this()
        {
            AnalyseId = id;
            Jaarbedrag = (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag")).Value;
        }
    }
}
