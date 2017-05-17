using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class LoonkostViewModel
    {
        public int LijnId { get; set; }
        [Required(ErrorMessage = "Functie is verplicht.")]
        public string Functie { get; set; }
        [RegularExpression("[0-9]*([,][0-9]+)?", ErrorMessage = "Het getal moet positief zijn met eventueel een komma.")]
        public string UrenPerWeek { get; set; }
        [RegularExpression("[0-9]*([,][0-9]+)?", ErrorMessage = "Het getal moet positief zijn met eventueel een komma.")]
        public string BrutoMaandloon { get; set; }
        public string Doelgroep { get; set; }
        public decimal Vop { get; set; }
        [RegularExpression("[0-9]*([,][0-9]+)?", ErrorMessage = "Het getal moet positief zijn met eventueel een komma.")]
        public string AantalMaanden { get; set; }
        [RegularExpression("[0-9]*([,][0-9]+)?", ErrorMessage = "Het getal moet positief zijn met eventueel een komma.")]
        public string Ibo { get; set; }
        public int AnalyseId { get; set; }
        public IEnumerable<LoonkostLijnViewModel> Lijnen { get; set; }
        public List<Doelgroep> DoelgroepList { get; set; }
        public int DoelgroepId { get; set; }
        public List<Veld> VopList { get; set; }
        public decimal VopId { get; set; }
        public LoonkostViewModel()
        {
            
        }

       public LoonkostViewModel(IList<LoonKostLijn> loonkostlijnen, int analyseId) : this()
        {
            AnalyseId = analyseId;
            Lijnen = loonkostlijnen.Select(lijn => new LoonkostLijnViewModel(lijn)).ToList();
            LijnId = 0;

        }
        public LoonkostViewModel(Lijn  lijn, IList<LoonKostLijn> loonkostlijnen, int analyseId) : this(loonkostlijnen,analyseId)
        {
            LijnId = lijn.LijnId;
            Functie = lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("functie")).Value.ToString();
            UrenPerWeek = string.Format("{0:0.##}", (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("uren per week")).Value);
            BrutoMaandloon =
                string.Format("{0:0.##}", (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("bruto maandloon fulltime")).Value);
            Doelgroep = ((LoonKostLijn)lijn).Doelgroep.Soort;
            Vop =
                (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("% Vlaamse ondersteuningspremie")).Value;
            AantalMaanden = string.Format("{0:0.##}", (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("aantal maanden IBO")).Value);
            Ibo =
              string.Format("{0:0.##}", (decimal)
                    lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("totale productiviteitspremie IBO")).Value);
            DoelgroepId = ((LoonKostLijn) lijn).Doelgroep.DoelgroepId;
            VopId = Vop;
        }
    }
}
