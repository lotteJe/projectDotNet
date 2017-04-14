using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class OmzetverliesBesparingViewModel
    {
        public int AnalyseId { get; set; }
        public decimal Veld1 { get; set; }
        public decimal Veld2 { get; set; }
        public decimal Veld3 { get; set; }

        public OmzetverliesBesparingViewModel()
        {
            
        }

        public OmzetverliesBesparingViewModel(Berekening besparing, int id) : this()
        {
            AnalyseId = id;
            Veld1 = (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag omzetverlies")).Value;
            Veld2 = (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("% besparing")).Value;
            Veld3 = (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("totaalbesparing")).Value;
        }
    }
}
