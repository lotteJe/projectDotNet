﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.Domain
{
    public class LoonKost : Berekening
    {

        #region Properties
        public Analyse Analyse { get; set; }
        #endregion

        #region Constructors

        protected LoonKost()
        {
            
        }
        public LoonKost(Analyse analyse)
        {
            Analyse = analyse;
            Velden.Add(new Veld("functie", typeof(string),1));
            Velden.Add(new Veld("uren per week", typeof(decimal),2));
            Velden.Add(new Veld("bruto maandloon fulltime", typeof(decimal),3));
            Velden.Add(new Veld("% Vlaamse ondersteuningspremie", typeof(decimal),4));
            Velden.Add(new Veld("bruto loon per maand incl patronale bijdragen", typeof(decimal),5));
            Velden.Add(new Veld("gemiddelde VOP per maand", typeof(decimal),6));
            Velden.Add(new Veld("doelgroepvermindering per maand", typeof(decimal),7));
            Velden.Add(new Veld("aantal maanden IBO", typeof(decimal),8));
            Velden.Add(new Veld("totale productiviteitspremie IBO", typeof(decimal),9));
            Velden.Add(new Veld("totale loonkost eerste jaar", typeof(decimal),10));

            Beschrijving = "Loonkosten medewerkers met grote afstand tot arbeidsmarkt";
            Volgorde = 1;
            //VoegLijnToe();
        }

        #endregion

        #region Methods

        public override void VoegLijnToe()
        {
            Lijn lijn = new LoonKostLijn(Velden);
            lijn.VoegLijnToe();
            Lijnen.Add(lijn);
        }


        public decimal BerekenTotaleLoonKost()
        {
            return Enumerable.Range(0, Lijnen.Count).ToList().Select(x => BerekenBedragPerLijn(x)).ToList().Sum();


        }

        public override decimal BerekenResultaat()
        {
            Resultaat = Enumerable.Range(0, Lijnen.Count).ToList().Select(x => BerekenMaandloonPatronaalPerLijn(x)).ToList().Sum()*12;
            return Resultaat;
        }

        public override decimal BerekenBedragPerLijn(int index)
        {
            ControleerIndex(index);
            BerekenMaandloonPatronaalPerLijn(index);
            BerekenDoelgroepVermindering(index);
            BerekenGemiddeldeVopPerMaand(index);
            Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("totale loonkost eerste jaar")).Value = ((decimal)Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("bruto loon per maand incl patronale bijdragen")).Value 
                                - (decimal)Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("gemiddelde VOP per maand")).Value
                                - (decimal)Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value)
                                * 
                                (13.92M 
                                - (decimal)Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("aantal maanden IBO")).Value)
                                + (decimal)Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("totale productiviteitspremie IBO")).Value;

            
            return (decimal)Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("totale loonkost eerste jaar")).Value;


            
        }

        public decimal BerekenMaandloonPatronaalPerLijn(int index)
        {
            ControleerIndex(index);
            if (Analyse.Organisatie.UrenWerkWeek != 0)
            {
                Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("bruto loon per maand incl patronale bijdragen")).Value =
                    ((decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("bruto maandloon fulltime")).Value/(decimal) Analyse.Organisatie.UrenWerkWeek)*
                    (decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("uren per week")).Value*(1 + Analyse.Organisatie.PatronaleBijdrage);
            }
            else
            {
                throw new ArgumentException("Uren per werkweek van de organisatie mag niet 0 zijn.");
            }
            return (decimal)Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("bruto loon per maand incl patronale bijdragen")).Value;

        }

        public void BerekenGemiddeldeVopPerMaand(int index)
        {
            ControleerIndex(index);
            //if ((decimal) Lijnen[index]["bruto loon per maand incl patronale bijdragen"] == 0
            //    || (decimal) Lijnen[index]["doelgroepvermindering per maand"] == 0 ||
            //    (decimal) Lijnen[index]["% Vlaamse ondersteuningspremie"] == 0)
            //    Lijnen[index]["gemiddelde VOP per maand"] = 0M;
            //else
            {
                Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("gemiddelde VOP per maand")).Value =
                    ((decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("bruto loon per maand incl patronale bijdragen")).Value
                     - ((decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value)/3)
                    *(decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("% Vlaamse ondersteuningspremie")).Value;
            }
        }

        public void BerekenDoelgroepVermindering(int index)
        {
            ControleerIndex(index);
            decimal urenWerkWeek = Analyse.Organisatie.UrenWerkWeek;
            decimal maandloon = (decimal)Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("bruto maandloon fulltime")).Value;
            decimal urenPerWeek = (decimal)Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("uren per week")).Value;
            if (((LoonKostLijn)Lijnen[index]).Doelgroep == null)
               return;
            if(urenWerkWeek == 0M)
                throw new ArgumentException("Uren werk week mag niet 0 zijn.");
            if (!((LoonKostLijn) Lijnen[index]).Doelgroep.Soort.Equals("Ander"))
            {
                if (maandloon < ((LoonKostLijn) Lijnen[index]).Doelgroep.Bedrag1)
                {
                    Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value =
                        (((LoonKostLijn) Lijnen[index]).Doelgroep.Bedrag2/urenWerkWeek)*urenPerWeek/4;
                }
            }
            else
            {
                Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value = 0M;
            }
        }

       
        #endregion


    }
}
