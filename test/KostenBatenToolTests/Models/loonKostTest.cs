using System;
using KostenBatenTool.Models.Domain;
using Xunit;
using Moq;
using Xunit.Sdk;

namespace KostenBatenToolTests.Models
{
    public class LoonKostTest
    {
        #region Fields
        private Analyse _analyse;
        private Organisatie _organisatie;
        private Kost _kost;
        #endregion

        #region Constructors
        public LoonKostTest()
        {
            _organisatie = new Organisatie("a", "b", "c", 1000, "d");
            _organisatie.UrenWerkWeek = 40.0M;
            _organisatie.PatronaleBijdrage = 35M;
            _analyse = new Analyse(_organisatie);
            _kost = new LoonKost(_analyse);
        }
        #endregion

        #region Tests
        [Fact]
        public void LoonKost_MaaktJuisteVeldenAan()
        {
            Assert.Equal(_kost.Velden["functie"], typeof(string));
            Assert.Equal(_kost.Velden["uren per week"], typeof(decimal));
            Assert.Equal(_kost.Velden["bruto maandloon fulltime"], typeof(decimal));
            Assert.Equal(_kost.Velden["doelgroep"], typeof(Doelgroep));
            Assert.Equal(_kost.Velden["% Vlaamse ondersteuningspremie"], typeof(decimal));
            Assert.Equal(_kost.Velden["bruto loon per maand incl patronale bijdragen"], typeof(decimal));
            Assert.Equal(_kost.Velden["gemiddelde VOP per maand"], typeof(decimal));
            Assert.Equal(_kost.Velden["doelgroepvermindering per maand"], typeof(decimal));
            Assert.Equal(_kost.Velden["aantal maanden IBO"], typeof(decimal));
            Assert.Equal(_kost.Velden["totale productiviteitspremie IBO"], typeof(decimal));
            Assert.Equal(_kost.Velden["totale loonkost eerste jaar"], typeof(decimal));

        }

        [Fact]
        public void LoonKost_MaaktJuisteLijnAan()
        {
            Assert.True(_kost.Lijnen[0].ContainsKey("functie"));
            Assert.True(_kost.Lijnen[0].ContainsKey("uren per week"));
            Assert.True(_kost.Lijnen[0].ContainsKey("bruto maandloon fulltime"));
            Assert.True(_kost.Lijnen[0].ContainsKey("doelgroep"));
            Assert.True(_kost.Lijnen[0].ContainsKey("% Vlaamse ondersteuningspremie"));
            Assert.True(_kost.Lijnen[0].ContainsKey("bruto loon per maand incl patronale bijdragen"));
            Assert.True(_kost.Lijnen[0].ContainsKey("gemiddelde VOP per maand"));
            Assert.True(_kost.Lijnen[0].ContainsKey("doelgroepvermindering per maand"));
            Assert.True(_kost.Lijnen[0].ContainsKey("aantal maanden IBO"));
            Assert.True(_kost.Lijnen[0].ContainsKey("totale productiviteitspremie IBO"));
            Assert.True(_kost.Lijnen[0].ContainsKey("totale loonkost eerste jaar"));

        }

        [Fact]
        public void VulVeldInFunctie()
        {
            _kost.VulVeldIn(0, "functie", "test");
            Assert.Equal(_kost.Lijnen[0]["functie"], "test");
        }

        [Fact]
        public void VulVeldInFunctie_WaardeWijzigen()
        {
            _kost.VulVeldIn(0, "functie", "test");
            _kost.VulVeldIn(0, "functie", "test2");
            Assert.Equal(_kost.Lijnen[0]["functie"], "test2");
        }

        [Fact]
        public void VulVeldInFunctie_GooitExceptieDecimal()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "functie", 0M));
        }

        [Fact]
        public void VulVeldInFunctie_GooitExceptieKeyBestaatNiet()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "fnctie", "test"));
        }

        //[Fact]
        //public void VulVeldInFunctie_MoetFalen()
        //{
        //   _kost.VulVeldIn(0, "functie", 0M);
        //   Assert.Equal(_kost.Lijnen[0]["functie"], 0M);
        //}

        [Fact]
        public void VulVeldInFunctie_VoegtNieuweLijnToe()
        {
            _kost.VulVeldIn(1, "functie", "test2");
            Assert.Equal(_kost.Lijnen[1]["functie"], "test2");
        }

        [Fact]
        public void VulVeldInFunctie_GooitExceptieIndexNegatief()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(-1, "functie", "test"));
        }

        [Fact]
        public void VulVeldFunctie_GooitExceptieIndexTeGroot()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(2, "functie", "test2"));
        }

        [Fact]
        public void VulVeldInUrenPerWeek()
        {
            _kost.VulVeldIn(0, "uren per week", 1.2M);
            Assert.Equal(_kost.Lijnen[0]["uren per week"], 1.2M);
        }

        [Fact]
        public void VulVeldInUrenPerWeek_GooitExceptieString()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "uren per week", "test"));
        }

        [Fact]
        public void VulVeldInUrenPerWeek_GooitExceptieDouble()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "uren per week", 3.0));
        }

        [Fact]
        public void VulVeldIn_GooitExceptieNegatieveWaarde()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "uren per week", -1.0M));

        }


        //[Fact]
        //public void VulVeldInUrenPerWeek_MoetFalen()
        //{
        //    _kost.VulVeldIn(0, "uren per week", "test");
        //    Assert.Equal(_kost.Lijnen[0]["uren per week"], "test");
        //}

        [Fact]
        public void VulVeldInUrenPerWeek_VoegtLijnToe()
        {
            _kost.VulVeldIn(1, "uren per week", 1.2M);
            Assert.Equal(_kost.Lijnen[1]["uren per week"], 1.2M);
        }

        [Fact]
        public void VulDoelgroepIn()
        {
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Ander);
            Assert.Equal(_kost.Lijnen[0]["doelgroep"], Doelgroep.Ander);
        }

        [Fact]
        public void VulDoelgroepIn_GooitExceptieString()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "doelgroep", "test"));
        }

        [Fact]
        public void BerekenMaandloonPatronaalPerLijn()
        {
            //_analyse.SetupGet(m => m.Organisatie.UrenWerkWeek).Returns(20.0);
            //_analyse.SetupGet(m => m.Organisatie.PatronaleBijdrage).Returns(0.35M);
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            ((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(0);
            Assert.Equal(_kost.Lijnen[0]["bruto loon per maand incl patronale bijdragen"], 1350M);
        }

        [Fact]
        public void BerekenMaandloonPatronaalPerLijn_GooitExceptieIndexNegatief()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Assert.Throws<ArgumentException>(() => ((LoonKost) _kost).BerekenMaandloonPatronaalPerLijn(-1));
        }

        [Fact]
        public void BerekenMaandloonPatronaalPerLijn_GooitExceptieIndexTeGroot()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Assert.Throws<ArgumentException>(() => ((LoonKost) _kost).BerekenMaandloonPatronaalPerLijn(1));
        }
        [Fact]
        public void BerekenMaandloonPatronaalPerLijn_GooitExceptieDelenDoorNul()
        {
            ((LoonKost) _kost).Analyse.Organisatie.UrenWerkWeek = 0M;
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Assert.Throws<ArgumentException>(() => ((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(0));
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepLaaggeschooldMaandloonMinderDan2500()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Laaggeschoold);
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0]["doelgroepvermindering per maand"], 387.5M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepLaaggeschooldMaandloonMeerDan2500()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 2600M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Laaggeschoold);
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0]["doelgroepvermindering per maand"], 0M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijnDoelgroepMiddengeschooldMaandloonMinderDan2500()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Middengeschoold);
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0]["doelgroepvermindering per maand"], 250M); 
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepMiddengeschooldMaandloonMeerDan2500()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 2600M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Middengeschoold);
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0]["doelgroepvermindering per maand"], 0M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepTussen55En60dMaandloonMinderDan4466()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Tussen55En60);
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0]["doelgroepvermindering per maand"], 287.5M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepTussen55En60dMaandloonMeerDan4466()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 4700M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Tussen55En60);
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0]["doelgroepvermindering per maand"], 0M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepBoven60MaandloonMinderDan4466()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Boven60);
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0]["doelgroepvermindering per maand"], 375M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepBoven60MaandloonMeerDan4466()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 4700M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Boven60);
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0]["doelgroepvermindering per maand"], 0M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepAnder()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Ander);
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0]["doelgroepvermindering per maand"], 0M);
        }

        [Fact]
        public void BerekenGemiddeldeVopPerMaandPerLijn()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Boven60);
            _kost.VulVeldIn(0, "% Vlaamse ondersteuningspremie", 0.20M);
            ((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(0); //geeft 1350M
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0); //geeft 375M
            ((LoonKost)_kost).BerekenGemiddeldeVopPerMaand(0);
            Assert.Equal(_kost.Lijnen[0]["gemiddelde VOP per maand"], 245M);
        }
        [Fact]
        public void BerekenGemiddeldeVopPerMaandPerLijn_Nul()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Boven60);
            _kost.VulVeldIn(0, "% Vlaamse ondersteuningspremie", 0M);
            ((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(0); //geeft 1350M
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0); //geeft 375M
            ((LoonKost)_kost).BerekenGemiddeldeVopPerMaand(0);
            Assert.Equal(_kost.Lijnen[0]["gemiddelde VOP per maand"], 0M);
        }

        [Fact]
        public void BerekenKostPerLijn()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Boven60);
            _kost.VulVeldIn(0, "% Vlaamse ondersteuningspremie", 0.2M);
            _kost.VulVeldIn(0, "aantal maanden IBO", 2M);
            _kost.VulVeldIn(0, "totale productiviteitspremie IBO",100M);
            Assert.Equal(_kost.BerekenKostPerLijn(0), 8801.6M);
            Assert.Equal(_kost.Lijnen[0]["totale loonkost eerste jaar"], 8801.6M);

        }

        [Fact]
        public void BerekenMaandloonPatronaalPerLijn_TweedeLijn()
        {
            _kost.VulVeldIn(1, "bruto maandloon fulltime", 1200M);
            _kost.VulVeldIn(1, "uren per week", 40.0M);
            ((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(1);
            Assert.Equal(_kost.Lijnen[1]["bruto loon per maand incl patronale bijdragen"], 1620M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepLaaggeschooldMaandloonMinderDan2500_TweedeLijn()
        {
            _kost.VulVeldIn(1, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(1, "uren per week", 40.0M);
            _kost.VulVeldIn(1, "doelgroep", Doelgroep.Laaggeschoold);
            ((LoonKost)_kost).BerekenDoelgroepVermindering(1);
            Assert.Equal(_kost.Lijnen[1]["doelgroepvermindering per maand"], 387.5M);
        }
        
        [Fact]
        public void BerekenGemiddeldeVopPerMaandPerLijn_TweedeLijn()
        {
            _kost.VulVeldIn(1, "bruto maandloon fulltime", 1200M);
            _kost.VulVeldIn(1, "uren per week", 40.0M);
            _kost.VulVeldIn(1, "doelgroep", Doelgroep.Laaggeschoold);
            _kost.VulVeldIn(1, "% Vlaamse ondersteuningspremie", 0.40M);
            ((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(1); 
            ((LoonKost)_kost).BerekenDoelgroepVermindering(1); 
            ((LoonKost)_kost).BerekenGemiddeldeVopPerMaand(1);
            Assert.Equal(Math.Round((decimal)_kost.Lijnen[1]["gemiddelde VOP per maand"],2), 596.33M);
        }

        [Fact]
        public void BerekenKostPerLijn_TweedeLijn()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Boven60);
            _kost.VulVeldIn(0, "% Vlaamse ondersteuningspremie", 0.2M);
            _kost.VulVeldIn(0, "aantal maanden IBO", 2M);
            _kost.VulVeldIn(0, "totale productiviteitspremie IBO", 100M);
            _kost.VulVeldIn(1, "bruto maandloon fulltime", 1200M);
            _kost.VulVeldIn(1, "uren per week", 40.0M);
            _kost.VulVeldIn(1, "doelgroep", Doelgroep.Laaggeschoold);
            _kost.VulVeldIn(1, "% Vlaamse ondersteuningspremie", 0.4M);
            _kost.VulVeldIn(1, "aantal maanden IBO", 3M);
            _kost.VulVeldIn(1, "totale productiviteitspremie IBO", 200M);
            Assert.Equal(Math.Round(_kost.BerekenKostPerLijn(1),2), 7146.94M);
            Assert.Equal( Math.Round((decimal)_kost.Lijnen[1]["totale loonkost eerste jaar"], 2), 7146.94M);

        }

        [Fact]
        public void BerekenTotaleLoonKost()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Boven60);
            _kost.VulVeldIn(0, "% Vlaamse ondersteuningspremie", 0.2M);
            _kost.VulVeldIn(0, "aantal maanden IBO", 2M);
            _kost.VulVeldIn(0, "totale productiviteitspremie IBO", 100M);
            _kost.VulVeldIn(1, "bruto maandloon fulltime", 1200M);
            _kost.VulVeldIn(1, "uren per week", 40.0M);
            _kost.VulVeldIn(1, "doelgroep", Doelgroep.Laaggeschoold);
            _kost.VulVeldIn(1, "% Vlaamse ondersteuningspremie", 0.4M);
            _kost.VulVeldIn(1, "aantal maanden IBO", 3M);
            _kost.VulVeldIn(1, "totale productiviteitspremie IBO", 200M);
            Assert.Equal(((LoonKost)_kost).BerekenTotaleLoonKost(), 15948.54M);
        }

        [Fact]
        public void BerekenResultaat()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(1, "bruto maandloon fulltime", 1200M);
            _kost.VulVeldIn(1, "uren per week", 40.0M);
            Assert.Equal(_kost.BerekenResultaat(), 35640M);
        }

        #endregion
    }


}
