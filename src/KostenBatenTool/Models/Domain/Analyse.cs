﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace KostenBatenTool.Models.Domain
{
    public class Analyse
    {
        #region Properties

        public List<Berekening> Kosten { get; set; }
        public List<Berekening> Baten { get; set; }
        public Organisatie Organisatie { get; set; }
        public DateTime AanmaakDatum { get; private set; }
        public int AnalyseId { get; set; }
        public decimal Resultaat { get; set; } = 0M;
        public decimal KostenResultaat { get; set; } = 0M;
        public decimal BatenResultaat { get; set; } = 0M;
        public bool Afgewerkt { get; set; } = false;
        public bool Verwijderd { get; set; } = false;
        #endregion

        #region Constructors 

        protected Analyse()
        {
            
        }
       
        public Analyse(Organisatie organisatie)
        {
            Organisatie = organisatie;
            AanmaakDatum = DateTime.Now;
            Kosten = new List<Berekening>();
            Baten = new List<Berekening>();
            Kosten.Add(new LoonKost(this));
            Kosten.Add(new VoorbereidingsKost(this));
            Kosten.Add(new WerkkledijKost(this));
            Kosten.Add(new AanpassingsKost(this));
            Kosten.Add(new OpleidingsKost(this));
            Kosten.Add(new AdministratieBegeleidingsKost(this));
            Kosten.Add(new AndereKost(this));

            Baten.Add(new LoonkostSubsidie((LoonKost) Kosten.First(k => k is LoonKost)));
            Baten.Add(new AanpassingsSubsidie(this));
            Baten.Add(new MedewerkerZelfdeNiveauBesparing(this));
            Baten.Add(new MedewerkerHogerNiveauBesparing(this));
            Baten.Add(new UitzendkrachtenBesparing(this));
            Baten.Add(new OmzetverliesBesparing(this));
            Baten.Add(new ProductiviteitsWinst(this));
            Baten.Add(new OverurenBesparing(this));
            Baten.Add(new OutsourcingBesparing(this));
            Baten.Add(new LogistiekeBesparing(this));
            Baten.Add(new AndereBesparing(this));
        }

        #endregion

        #region Methods

        public void VulVeldIn(string berekeningNaam, int index, string key, Object waarde)
        {

            if ((Kosten.Any(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain." + berekeningNaam))))
            {
                Berekening berekening =
                    Kosten.First(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain." + berekeningNaam));
                berekening.VulVeldIn(index, key, waarde);
            } else if ((Baten.Any(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain." + berekeningNaam))))
            {
                Berekening berekening = Baten.First(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain." + berekeningNaam));
                berekening.VulVeldIn(index, key, waarde);
            }
            else throw new ArgumentException("Berekening bestaat niet");
            BerekenNettoResultaat();
        }

        public Berekening GetBerekening(string berekeningNaam)
        {
            if ((Kosten.Any(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain." + berekeningNaam))))
            {
                return Kosten.First(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain." + berekeningNaam));

            }
            if ((Baten.Any(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain." + berekeningNaam))))
            {
                return Baten.First(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain." + berekeningNaam));


            }
            throw new ArgumentException("Berekening bestaat niet");

        }
        

        public decimal BerekenNettoResultaat()
        {
            Resultaat = BerekenBatenResultaat() - BerekenKostenResultaat();
            return Resultaat;
        }

        public decimal BerekenKostenResultaat()
        {
            KostenResultaat = Kosten.Sum(k => k.BerekenResultaat());
            return KostenResultaat;
        }

        public decimal BerekenBatenResultaat()
        {
            BatenResultaat = Baten.Sum(b => b.BerekenResultaat());
            return BatenResultaat;
        }



        #endregion
    }
}



