using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class OmzetverliesBesparingViewModel
    {
        public int AnalyseId { get; set; }

        [RegularExpression("[0-9]*([,][0-9]+)?", ErrorMessage = "Het getal moet positief zijn met eventueel een komma.")]
        public string Veld1 { get; set; }

        [RegularExpression("^[0-9][0-9]?([,][0-9]+)?$|^100$", ErrorMessage = "De waarde moet tussen 0 en 100 liggen.")]
        public string Veld2 { get; set; }
        public decimal Veld3 { get; set; }

        public OmzetverliesBesparingViewModel()
        {
            
        }

        public OmzetverliesBesparingViewModel(Berekening besparing, int id) : this()
        {
            AnalyseId = id;
            Veld1 = string.Format("{0:0.##}", (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag omzetverlies")).Value);
            Veld2 = string.Format("{0:0.##}", (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("% besparing")).Value*100);
            Veld3 = (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("totaalbesparing")).Value;
        }
    }
}
