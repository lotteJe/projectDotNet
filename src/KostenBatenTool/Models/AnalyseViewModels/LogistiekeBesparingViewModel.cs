using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class LogistiekeBesparingViewModel
    {
        [RegularExpression("[0-9]*([,][0-9]+)?", ErrorMessage = "Het getal moet positief zijn met eventueel een komma.")]
        public string Transport { get; set; }
        [RegularExpression("[0-9]*([,][0-9]+)?", ErrorMessage = "Het getal moet positief zijn met eventueel een komma.")]
        public string Logistiek { get; set; }
        public int AnalyseId { get; set; }
       
        public LogistiekeBesparingViewModel()
        {
            
        }

        public LogistiekeBesparingViewModel(LogistiekeBesparing besparing,int id) : this()
        {
            AnalyseId = id;
            Transport = string.Format("{0:0.##}", (decimal) besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("transportkosten jaarbedrag")).Value);
            Logistiek = string.Format("{0:0.##}", (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("logistieke kosten jaarbedrag")).Value);
        }
    }
}
