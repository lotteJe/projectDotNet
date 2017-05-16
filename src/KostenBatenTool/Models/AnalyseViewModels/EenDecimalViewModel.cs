using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class EenDecimalViewModel
    {
        public int AnalyseId { get; set; }

        [RegularExpression("[0-9]*([,][0-9]+)?", ErrorMessage = "Het getal moet positief zijn met eventueel een komma.")]
        public string Jaarbedrag { get; set; }

        public EenDecimalViewModel()
        {
            
        }

        public EenDecimalViewModel(Berekening besparing, int id) : this()
        {
            AnalyseId = id;
            Jaarbedrag = string.Format("{0:0.##}", (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag")).Value);
        }
    }
}
