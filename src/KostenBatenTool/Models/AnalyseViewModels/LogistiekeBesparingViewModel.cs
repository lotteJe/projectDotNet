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
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Het getal moet positief zijn.")]
        public decimal Transport { get; set; }
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Het getal moet positief zijn.")]
        public decimal Logistiek { get; set; }
        public int AnalyseId { get; set; }
       
        public LogistiekeBesparingViewModel()
        {
            
        }

        public LogistiekeBesparingViewModel(LogistiekeBesparing besparing,int id) : this()
        {
            AnalyseId = id;
            Transport = (decimal) besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("transportkosten jaarbedrag")).Value;
            Logistiek = (decimal)besparing.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("logistieke kosten jaarbedrag")).Value;
        }
    }
}
