using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.Domain
{
    public class LoonKost : Kost
    {

        #region Fields
        private Analyse _analyse; 
        #endregion

        #region Constructors

        public LoonKost(Analyse analyse)
        {
            _analyse = analyse;
            Velden.Add("functie", typeof(String));
            Velden.Add("uren per week", typeof(double));
            Velden.Add("bruto maandloon fulltime", typeof(decimal));
            Velden.Add("doelgroep", typeof(Doelgroep));
            Velden.Add("% Vlaamse ondersteuningspremie", typeof(decimal));
            Velden.Add("bruto loon per maand incl patronale bijdragen", typeof(decimal));
            Velden.Add("gemiddelde VOP per maand", typeof(decimal));
            Velden.Add("doelgroepvermindering per maand", typeof(decimal));
            Velden.Add("aantal maanden IBO", typeof(double));
            Velden.Add("totale productiviteitspremie IBO", typeof(decimal));
            Velden.Add("totale loonkost eerste jaar", typeof(decimal));
            
        }
        #endregion

        #region Methods

        
        public override decimal BerekenResultaat()
        {
           return Enumerable.Range(0, Lijnen.Count).ToList().Select(x => BerekenKostPerLijn(x)).ToList().Sum();

        }

        public override decimal BerekenKostPerLijn(int index)
        {
            BerekenMaandloonPatronaalPerLijn(index);
            BerekenDoelgroepVermindering(index);
            BerekenGemiddeldeVopPerMaand(index);
           decimal lijnkost = ((decimal) Lijnen[index]["bruto loon per maand incl patronale bijdragen"] - (decimal)Lijnen[index]["gemiddelde VOP per maand"] - (decimal)Lijnen[index]["doelgroepvermindering per maand"])
            *(13.92M - (decimal) Lijnen[index]["aantal maanden IBO"]) + (decimal) Lijnen[index]["totale productiviteitspremie IBO"];
            Lijnen[index]["totale loonkost eerste jaar"] = lijnkost;
            return (decimal)Lijnen[index]["totale loonkost eerste jaar"];

        }

        public void BerekenMaandloonPatronaalPerLijn(int index)
        {
            if (_analyse.Organisatie.UrenWerkWeek != 0)
            {
                Lijnen[index]["bruto loon per maand incl patronale bijdragen"] = ((decimal)Lijnen[index]["bruto maandloon fulltime"] / (decimal)_analyse.Organisatie.UrenWerkWeek) *
                   (decimal)Lijnen[index]["uren per week"] * (1 + _analyse.Organisatie.PatronaleBijdrage);
            }
            
        }

        public void BerekenGemiddeldeVopPerMaand(int index)
        {
            Lijnen[index]["gemiddelde VOP per maand"] =
                (decimal) Lijnen[index]["bruto loon per maand incl patronale bijdragen"] -
                (decimal) Lijnen[index]["doelgroepvermindering per maand"]*(decimal) Lijnen[index]["% Vlaamse ondersteuningspremie"];
        }

        public void BerekenDoelgroepVermindering(int index)
        {
            decimal urenWerkWeek = (decimal)_analyse.Organisatie.UrenWerkWeek;
            decimal maandloon = (decimal) Lijnen[index]["bruto maandloon fulltime"];
            decimal urenPerWeek = (decimal) Lijnen[index]["uren per week"];
            switch ( (Doelgroep) Lijnen[index]["doelgroep"])
            {
                case Doelgroep.Laaggeschoold:
                    if (maandloon < 2500)
                        Lijnen[index]["doelgroepvermindering per maand"] = (1550 / urenWerkWeek) * urenPerWeek / 4;
                    break;
                case Doelgroep.Middengeschoold:
                    if (maandloon < 2500)
                        Lijnen[index]["doelgroepvermindering per maand"] = (1000 / urenWerkWeek) * urenPerWeek / 4;
                    break;
                case Doelgroep.Tussen55En60:
                    if (maandloon < 4466.66M)
                        Lijnen[index]["doelgroepvermindering per maand"] = (1150 / urenWerkWeek) * urenPerWeek / 4;
                    break;
                case Doelgroep.Boven60:
                    if (maandloon < 4466.66M)
                        Lijnen[index]["doelgroepvermindering per maand"] = (1500 / urenWerkWeek) * urenPerWeek / 4;
                    break;

            }
        }

        #endregion


    }
}
