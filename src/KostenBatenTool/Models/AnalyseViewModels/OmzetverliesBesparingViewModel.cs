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
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Het getal moet positief zijn.")]
        public decimal Veld1 { get; set; }
        [Range(typeof(decimal), "0", "100", ErrorMessage = "De waarde moet tussen 0 en 100 liggen.")]
        public decimal Veld2 { get; set; }
        public decimal Veld3 { get; set; }

        public OmzetverliesBesparingViewModel()
        {
            
        }

        public OmzetverliesBesparingViewModel(Berekening besparing, int id) : this()
        {
            AnalyseId = id;
            Veld1 = (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag omzetverlies")).Value;
            Veld2 = (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("% besparing")).Value*100;
            Veld3 = (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("totaalbesparing")).Value;
        }
    }
}
