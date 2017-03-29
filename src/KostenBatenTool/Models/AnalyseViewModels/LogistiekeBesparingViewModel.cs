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
        public decimal Bedrag { get; set; }


    }
}
