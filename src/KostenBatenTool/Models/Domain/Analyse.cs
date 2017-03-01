using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace KostenBatenTool.Models.Domain
{
    public class Analyse
    {
        #region Properties

        public IList<Berekening> Kosten { get; set; }
        public IList<Berekening> Baten { get; set; }
        public Organisatie Organisatie { get; set; }
        public DateTime AanmaakDatum { get; private set; }

        #endregion

        #region Constructors


        public Analyse(Organisatie organisatie)
        {
            Organisatie = organisatie;
            AanmaakDatum = DateTime.Now;
            Kosten = new List<Berekening>();
            Baten = new List<Berekening>();
            Kosten.Add(new LoonKost(this));
            Kosten.Add(new VoorbereidingsKost());
            Kosten.Add(new WerkkledijKost());
            Kosten.Add(new AanpassingsKost());
            Kosten.Add(new OpleidingsKost());
            Kosten.Add(new AdministratieBegeleidingsKost(this));
            Kosten.Add(new AndereKost());

            Baten.Add(new LoonkostSubsidie((LoonKost) Kosten.First(k => k is LoonKost)));
            Baten.Add(new AanpassingsSubsidie());
            Baten.Add(new MedewerkerZelfdeNiveauBesparing(this));
            Baten.Add(new MedewerkerHogereNiveauBesparing(this));
            Baten.Add(new UitzendkrachtenBesparing());
            Baten.Add(new OmzetverliesBesparing());
            Baten.Add(new ProductiviteitsWinst());
            Baten.Add(new OverurenBesparing());
            Baten.Add(new OutsourcingBesparing());
            Baten.Add(new LogistiekeBesparing());
            Baten.Add(new AndereBesparing());
        }

        #endregion

        #region Methods

        public decimal GeefTotaalBedragPerBerekening(Berekening berekening)
        {
            return berekening.BerekenResultaat();
        }

        public void BerekenBedragPerLijn(Berekening b, int index)
        {
            b.BerekenBedragPerLijn(index);
        }

        public void VulVeldIn(Berekening b, int index, string key, Object waarde)
        {
            b.VulVeldIn(index, key, waarde);
        }
        
        public decimal BerekenNettoResultaat()
        {
            return BerekenBatenResultaat() - BerekenKostenResultaat();
        }

        public decimal BerekenKostenResultaat()
        {
            return Kosten.Select(k => k.BerekenResultaat()).Sum();
        }

        public decimal BerekenBatenResultaat()
        {
            return Baten.Select(b => b.BerekenResultaat()).Sum();
        }



        #endregion
    }
}

