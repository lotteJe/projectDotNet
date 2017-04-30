using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class TypeBedragViewModel
    {
        public int LijnId { get; set; }
        public int AnalyseId { get; set; }
        public IEnumerable<TypeBedragLijstObjectViewModel> Lijst { get; set; }
        public string Type { get; set; }
        public decimal Bedrag { get; set; }

        public TypeBedragViewModel()
        {

        }

        public TypeBedragViewModel(Berekening berekening, int analyseId) : this()
        {
            AnalyseId = analyseId;
            string veld1 =
               berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.OutsourcingBesparing") || berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.UitzendkrachtenBesparing")
                   ? "beschrijving"
                   : "type";
            if (berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.AndereBesparing") )
            {
                veld1 = "type besparing";
            }
            string veld2 = berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.AndereBesparing") || berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.OutsourcingBesparing") || berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.UitzendkrachtenBesparing")
                  ? "jaarbedrag"
                      : "bedrag";
            Lijst = berekening.Lijnen.Select(lijn => new TypeBedragLijstObjectViewModel(lijn, veld1, veld2)).ToList();
            LijnId = 0;
        }

        public TypeBedragViewModel(Lijn lijn, Berekening berekening, int analysId) : this(berekening, analysId)
        {
            LijnId = lijn.LijnId;
            string veld1 =
                berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.OutsourcingBesparing") || berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.UitzendkrachtenBesparing")
                    ? "beschrijving"
                    : "type";
            if (berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.AndereBesparing"))
            {
                veld1 = "type besparing";
            }
            string veld2 = berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.AndereBesparing") || berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.OutsourcingBesparing") || berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.UitzendkrachtenBesparing")
                ? "jaarbedrag"
                    : "bedrag";
            Type = lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals(veld1)).Value.ToString();
            Bedrag = (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals(veld2)).Value;
        }

    }
}


