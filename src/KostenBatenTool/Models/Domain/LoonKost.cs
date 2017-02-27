using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.Domain
{
    public class LoonKost : Kost
    {

        #region Properties
        public Analyse Analyse { get; set; }
        #endregion

        #region Constructors

        public LoonKost(Analyse analyse)
        {
            Analyse = analyse;
            Velden.Add("functie", typeof(string));
            Velden.Add("uren per week", typeof(decimal));
            Velden.Add("bruto maandloon fulltime", typeof(decimal));
            Velden.Add("doelgroep", typeof(Doelgroep));
            Velden.Add("% Vlaamse ondersteuningspremie", typeof(decimal));
            Velden.Add("bruto loon per maand incl patronale bijdragen", typeof(decimal));
            Velden.Add("gemiddelde VOP per maand", typeof(decimal));
            Velden.Add("doelgroepvermindering per maand", typeof(decimal));
            Velden.Add("aantal maanden IBO", typeof(decimal));
            Velden.Add("totale productiviteitspremie IBO", typeof(decimal));
            Velden.Add("totale loonkost eerste jaar", typeof(decimal));

            VoegLijnToe(0);
        }

        #endregion

        #region Methods


        public decimal BerekenTotaleLoonKost()
        {
            return Enumerable.Range(0, Lijnen.Count).ToList().Select(x => BerekenKostPerLijn(x)).ToList().Sum();


        }

        public override decimal BerekenResultaat()
        {
            return Enumerable.Range(0, Lijnen.Count).ToList().Select(x => BerekenMaandloonPatronaalPerLijn(x)).ToList().Sum()*12;
        }

        public override decimal BerekenKostPerLijn(int index)
        {
            ControleerIndex(index);
            BerekenMaandloonPatronaalPerLijn(index);
            BerekenDoelgroepVermindering(index);
            BerekenGemiddeldeVopPerMaand(index);
            Lijnen[index]["totale loonkost eerste jaar"] = ((decimal)Lijnen[index]["bruto loon per maand incl patronale bijdragen"] 
                                - (decimal)Lijnen[index]["gemiddelde VOP per maand"] 
                                - (decimal)Lijnen[index]["doelgroepvermindering per maand"])
                                * 
                                (13.92M 
                                - (decimal)Lijnen[index]["aantal maanden IBO"]) 
                                + (decimal)Lijnen[index]["totale productiviteitspremie IBO"];

            
            return (decimal)Lijnen[index]["totale loonkost eerste jaar"];

        }

        public decimal BerekenMaandloonPatronaalPerLijn(int index)
        {
            ControleerIndex(index);
            if (Analyse.Organisatie.UrenWerkWeek != 0)
            {
                Lijnen[index]["bruto loon per maand incl patronale bijdragen"] =
                    ((decimal) Lijnen[index]["bruto maandloon fulltime"]/(decimal) Analyse.Organisatie.UrenWerkWeek)*
                    (decimal) Lijnen[index]["uren per week"]*(1 + Analyse.Organisatie.PatronaleBijdrage);
            }
            else
            {
                throw new ArgumentException("Uren per werkweek van de organisatie mag niet 0 zijn.");
            }
            return (decimal)Lijnen[index]["bruto loon per maand incl patronale bijdragen"];

        }

        public void BerekenGemiddeldeVopPerMaand(int index)
        {
            ControleerIndex(index);
            Lijnen[index]["gemiddelde VOP per maand"] = 
                ((decimal)Lijnen[index]["bruto loon per maand incl patronale bijdragen"] 
                - ((decimal)Lijnen[index]["doelgroepvermindering per maand"]) /3 )
                * (decimal)Lijnen[index]["% Vlaamse ondersteuningspremie"];
        }

        public void BerekenDoelgroepVermindering(int index)
        {
            ControleerIndex(index);
            decimal urenWerkWeek = Analyse.Organisatie.UrenWerkWeek;
            decimal maandloon = (decimal)Lijnen[index]["bruto maandloon fulltime"];
            decimal urenPerWeek = (decimal)Lijnen[index]["uren per week"];
            Lijnen[index]["doelgroepvermindering per maand"] = 0M;
            switch ((Doelgroep)Lijnen[index]["doelgroep"])
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

        public void ControleerIndex(int index)
        {
            if (index >= Lijnen.Count || index < 0)
            {
                throw new ArgumentException("ïndex is ongeldig!");
            }
        }

        #endregion


    }
}
