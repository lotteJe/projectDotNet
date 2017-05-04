using System;
using KostenBatenTool.Models.Domain;
using Xunit;
using Moq;
using Xunit.Sdk;
using System.Linq;


namespace KostenBatenToolTests.Models
{
    public class LoonKostTest
    {
        #region Fields
        private Analyse _analyse;
        private Organisatie _organisatie;
        private Berekening _kost;
        #endregion

        #region Constructors
        public LoonKostTest()
        {
            _organisatie = new Organisatie("a", "b", "c", "1000", "d");
            _organisatie.UrenWerkWeek = 40.0M;
            _organisatie.PatronaleBijdrage = 0.35M;
            _analyse = new Analyse(_organisatie);
            _kost = new LoonKost(_analyse);
            _kost.VoegLijnToe();
        }
        #endregion

        #region Tests
        [Fact]
        public void LoonKost_MaaktJuisteVeldenAan()
        {
            Assert.Equal(_kost.Velden.Find(v => v.VeldKey.Equals("functie")).Value, typeof(string));
            Assert.Equal(_kost.Velden.Find(v => v.VeldKey.Equals("uren per week")).Value, typeof(decimal));
            Assert.Equal(_kost.Velden.Find(v => v.VeldKey.Equals("bruto maandloon fulltime")).Value, typeof(decimal));
            Assert.Equal(_kost.Velden.Find(v => v.VeldKey.Equals("% Vlaamse ondersteuningspremie")).Value, typeof(decimal));
            Assert.Equal(_kost.Velden.Find(v => v.VeldKey.Equals("bruto loon per maand incl patronale bijdragen")).Value, typeof(decimal));
            Assert.Equal(_kost.Velden.Find(v => v.VeldKey.Equals("gemiddelde VOP per maand")).Value, typeof(decimal));
            Assert.Equal(_kost.Velden.Find(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, typeof(decimal));
            Assert.Equal(_kost.Velden.Find(v => v.VeldKey.Equals("aantal maanden IBO")).Value, typeof(decimal));
            Assert.Equal(_kost.Velden.Find(v => v.VeldKey.Equals("totale productiviteitspremie IBO")).Value, typeof(decimal));
            Assert.Equal(_kost.Velden.Find(v => v.VeldKey.Equals("totale loonkost eerste jaar")).Value, typeof(decimal));

        }

        [Fact]
        public void LoonKost_MaaktJuisteLijnAan()
        {
            Assert.True(_kost.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("functie")));
            Assert.True(_kost.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("uren per week")));
            Assert.True(_kost.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("bruto maandloon fulltime")));
            Assert.True(_kost.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("% Vlaamse ondersteuningspremie")));
            Assert.True(_kost.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("bruto loon per maand incl patronale bijdragen")));
            Assert.True(_kost.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("doelgroepvermindering per maand")));
            Assert.True(_kost.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("aantal maanden IBO")));
            Assert.True(_kost.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("totale productiviteitspremie IBO")));
            Assert.True(_kost.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("totale loonkost eerste jaar")));
         
        }

        [Fact]
        public void LoonKost_ZetBedragenOp0()
        {
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("uren per week")).Value, 0M);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("bruto maandloon fulltime")).Value, 0M);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("% Vlaamse ondersteuningspremie")).Value, 0M);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("bruto loon per maand incl patronale bijdragen")).Value, 0M);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("gemiddelde VOP per maand")).Value, 0M);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, 0M);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("aantal maanden IBO")).Value, 0M);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("totale productiviteitspremie IBO")).Value, 0M);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("totale loonkost eerste jaar")).Value, 0M);
        }

        [Fact]
        public void VulVeldInFunctie()
        {
            _kost.VulVeldIn(0, "functie", "test");
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("functie")).Value, "test");
        }

        [Fact]
        public void VulVeldInFunctie_WaardeWijzigen()
        {
            _kost.VulVeldIn(0, "functie", "test");
            _kost.VulVeldIn(0, "functie", "test2");
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("functie")).Value, "test2");
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


        [Fact]
        public void VulVeldInFunctie_VoegtNieuweLijnToe()
        {
            _kost.VulVeldIn(0, "functie", "test");
            _kost.VoegLijnToe();
            _kost.VulVeldIn(1, "functie", "test2");
            Assert.Equal(_kost.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("functie")).Value, "test2");
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
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("uren per week")).Value, 1.2M);
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

        [Fact]
        public void VulVeldInUrenPerWeek_VoegtLijnToe()
        {
            _kost.VulVeldIn(0, "uren per week", 1M);
            _kost.VoegLijnToe();
            _kost.VulVeldIn(1, "uren per week", 1.2M);
            Assert.Equal(_kost.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("uren per week")).Value, 1.2M);
        }

        [Fact]
        public void VulPercentageIn()
        {
           _kost.VulVeldIn(0, "% Vlaamse ondersteuningspremie", 0.75M);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("% Vlaamse ondersteuningspremie")).Value, 0.75M);
        }
        
        [Fact]
        public void VulPercentageIn_GooitExceptieGroterDan1()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "% Vlaamse ondersteuningspremie", 1.2M));
        }
        
        [Fact]
        public void BerekenMaandloonPatronaalPerLijn()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            ((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("bruto loon per maand incl patronale bijdragen")).Value, 1350M);
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
        public void BerekenMaandloonPatronaalPerLijn_Geeft0NietsIngevuld()
        {
            Assert.Equal(((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(0), 0M);
        }

        [Fact]
        public void BerekenMaandloonPatronaalPerLijn_Geeft0BrutoMaandloonNietIngevuld()
        {
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Assert.Equal(((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(0), 0M);
        }

        [Fact]
        public void BerekenMaandloonPatronaalPerLijn_Geeft0UrenPerWeekNietIngevuld()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            Assert.Equal(((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(0), 0M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepLaaggeschooldMaandloonMinderDan2500()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep laaggeschoold = new Doelgroep("Laaggeschoold", 2500M, 1550M);
            ((LoonKostLijn) _kost.Lijnen[0]).Doelgroep = laaggeschoold;
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, 387.5M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepLaaggeschooldMaandloonMeerDan2500()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 2600M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep laaggeschoold = new Doelgroep("Laaggeschoold", 2500M, 1550M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = laaggeschoold;
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, 0M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijnDoelgroepMiddengeschooldMaandloonMinderDan2500()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep middengeschoold = new Doelgroep("Middengeschoold", 2500M, 1000M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = middengeschoold;
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, 250M); 
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepMiddengeschooldMaandloonMeerDan2500()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 2600M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep middengeschoold = new Doelgroep("Middengeschoold", 2500M, 1000M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = middengeschoold;
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, 0M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepTussen55En60dMaandloonMinderDan4466()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep middengeschoold = new Doelgroep("Tussen55En60", 4466.66M, 1150M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = middengeschoold;
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, 287.5M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepTussen55En60dMaandloonMeerDan4466()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 4700M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep middengeschoold = new Doelgroep("Tussen55En60", 4466.66M, 1150M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = middengeschoold;
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, 0M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepBoven60MaandloonMinderDan4466()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep boven60 = new Doelgroep("Boven60", 4466.66M, 1500M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = boven60;
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, 375M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepBoven60MaandloonMeerDan4466()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 4700M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep boven60 = new Doelgroep("Boven60", 4466.66M, 1500M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = boven60;
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, 0M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepAnder()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep ander = new Doelgroep("Ander", 0M, 0M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = ander;
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, 0M);
        }

        [Fact]
        public void BerekenDoelgroepVermindering_Geeft0DoelgroepNietIngevuld()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, 0M);
        }

        [Fact]
        public void BerekenDoelgroepVermindering_GooitExceptieUrenWerkWeek0()
        {
            ((LoonKost) _kost).Analyse.Organisatie.UrenWerkWeek = 0M;
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep ander = new Doelgroep("Ander", 0M, 0M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = ander;
            Assert.Throws<ArgumentException>(() => ((LoonKost)_kost).BerekenDoelgroepVermindering(0));

        }

        [Fact]
        public void BerekenDoelgroepVermindering_Geeft0BrutoMaandloonNietIngevuld()
        {
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep ander = new Doelgroep("Ander", 0M, 0M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = ander;
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, 0M);
        }

        [Fact]
        public void BerekenDoelgroepVermindering_Geeft0UrenPerWeekNietIngevuld()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            Doelgroep ander = new Doelgroep("Ander", 0M, 0M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = ander;
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, 0M);
        }

        [Fact]
        public void BerekenGemiddeldeVopPerMaandPerLijn()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep boven60 = new Doelgroep("Boven60", 4466.66M, 1500M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = boven60;
            _kost.VulVeldIn(0, "% Vlaamse ondersteuningspremie", 0.20M);
            ((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(0); //geeft 1350M
            ((LoonKost)_kost).BerekenDoelgroepVermindering(0); //geeft 375M
            ((LoonKost)_kost).BerekenGemiddeldeVopPerMaand(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("gemiddelde VOP per maand")).Value, 245M);
        }
        
        [Fact]
        public void BerekenGemiddeldeVopPerMaandPerLijn_Geeft0NietsIngevuld()
        {
            ((LoonKost) _kost).BerekenGemiddeldeVopPerMaand(0);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("gemiddelde VOP per maand")).Value, 0M);
        }

        //[Fact]
        //public void BerekenGemiddeldeVopPerMaandPerLijn_Geeft0BrutoloonNietIngevuld()
        //{
        //    _kost.VulVeldIn(0, "uren per week", 40.0M);
        //    _kost.VulVeldIn(0, "doelgroep", Doelgroep.Boven60);
        //    _kost.VulVeldIn(0, "% Vlaamse ondersteuningspremie", 0.20M);
        //    ((LoonKost)_kost).BerekenDoelgroepVermindering(0);
        //    ((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(0);
        //    ((LoonKost)_kost).BerekenGemiddeldeVopPerMaand(0);
        //    Assert.Equal(_kost.Lijnen[0]["gemiddelde VOP per maand"], 0M);
        //}

        //[Fact]
        //public void BerekenGemiddeldeVopPerMaandPerLijn_Geeft0VopNietIngevuld()
        //{
        //    _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
        //    _kost.VulVeldIn(0, "uren per week", 40.0M);
        //    _kost.VulVeldIn(0, "doelgroep", Doelgroep.Boven60);
        //    ((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(0); //geeft 1350M
        //    ((LoonKost)_kost).BerekenDoelgroepVermindering(0); //geeft 375M
        //    ((LoonKost)_kost).BerekenGemiddeldeVopPerMaand(0);
        //    Assert.Equal(_kost.Lijnen[0]["gemiddelde VOP per maand"], 0M);
        //}

        //[Fact]
        //public void BerekenGemiddeldeVopPerMaandPerLijn_Geeft0DoelgroepverminderingNietIngevuld()
        //{
        //    _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
        //    _kost.VulVeldIn(0, "uren per week", 40.0M);
        //    _kost.VulVeldIn(0, "% Vlaamse ondersteuningspremie", 0.20M);
        //    ((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(0); //geeft 1350M
        //    ((LoonKost)_kost).BerekenDoelgroepVermindering(0); //geeft 375M
        //    ((LoonKost)_kost).BerekenGemiddeldeVopPerMaand(0);
        //    Assert.Equal(_kost.Lijnen[0]["gemiddelde VOP per maand"], 0M);
        //}

       
        [Fact]
        public void BerekenKostPerLijn()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep boven60 = new Doelgroep("Boven60", 4466.66M, 1500M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = boven60;
            _kost.VulVeldIn(0, "% Vlaamse ondersteuningspremie", 0.2M);
            _kost.VulVeldIn(0, "aantal maanden IBO", 2M);
            _kost.VulVeldIn(0, "totale productiviteitspremie IBO",100M);
            Assert.Equal(_kost.BerekenBedragPerLijn(0), 8801.6M);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("totale loonkost eerste jaar")).Value, 8801.6M);

        }

        [Fact]
        public void BerekenMaandloonPatronaalPerLijn_TweedeLijn()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VoegLijnToe();
            _kost.VulVeldIn(1, "bruto maandloon fulltime", 1200M);
            _kost.VulVeldIn(1, "uren per week", 40.0M);
            ((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(1);
            Assert.Equal(_kost.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("bruto loon per maand incl patronale bijdragen")).Value, 1620M);
        }

        [Fact]
        public void BerekenDoelgroepVerminderingPerLijn_DoelgroepLaaggeschooldMaandloonMinderDan2500_TweedeLijn()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VoegLijnToe();
            _kost.VulVeldIn(1, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(1, "uren per week", 40.0M);
            Doelgroep laaggeschoold = new Doelgroep("Laaggeschoold", 2500M, 1550M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = laaggeschoold;
            ((LoonKost)_kost).BerekenDoelgroepVermindering(1);
            Assert.Equal(_kost.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("doelgroepvermindering per maand")).Value, 387.5M);
        }
        
        [Fact]
        public void BerekenGemiddeldeVopPerMaandPerLijn_TweedeLijn()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VoegLijnToe();
            _kost.VulVeldIn(1, "bruto maandloon fulltime", 1200M);
            _kost.VulVeldIn(1, "uren per week", 40.0M);
            Doelgroep laaggeschoold = new Doelgroep("Laaggeschoold", 2500M, 1550M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = laaggeschoold;
            _kost.VulVeldIn(1, "% Vlaamse ondersteuningspremie", 0.40M);
            ((LoonKost)_kost).BerekenMaandloonPatronaalPerLijn(1); 
            ((LoonKost)_kost).BerekenDoelgroepVermindering(1); 
            ((LoonKost)_kost).BerekenGemiddeldeVopPerMaand(1);
            Assert.Equal(Math.Round((decimal)_kost.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("gemiddelde VOP per maand")).Value, 2), 596.33M);
        }

        [Fact]
        public void BerekenKostPerLijn_TweedeLijn()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep boven60 = new Doelgroep("Boven60", 4466.66M, 1500M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = boven60;
            _kost.VulVeldIn(0, "% Vlaamse ondersteuningspremie", 0.2M);
            _kost.VulVeldIn(0, "aantal maanden IBO", 2M);
            _kost.VoegLijnToe();
            _kost.VulVeldIn(0, "totale productiviteitspremie IBO", 100M);
            _kost.VulVeldIn(1, "bruto maandloon fulltime", 1200M);
            _kost.VulVeldIn(1, "uren per week", 40.0M);
            Doelgroep laaggeschoold = new Doelgroep("Laaggeschoold", 2500M, 1550M);
            ((LoonKostLijn)_kost.Lijnen[1]).Doelgroep = laaggeschoold;
            _kost.VulVeldIn(1, "% Vlaamse ondersteuningspremie", 0.4M);
            _kost.VulVeldIn(1, "aantal maanden IBO", 3M);
            _kost.VulVeldIn(1, "totale productiviteitspremie IBO", 200M);
            Assert.Equal(Math.Round(_kost.BerekenBedragPerLijn(1),2), 7146.94M);
            Assert.Equal( Math.Round((decimal)_kost.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("totale loonkost eerste jaar")).Value, 2), 7146.94M);

        }

        [Fact]
        public void BerekenBedragPerLijn_Geeft0NietsIngevuld()
        {
            Assert.Equal(_kost.BerekenBedragPerLijn(0), 0M);
        }

        [Fact]
        public void BerekenTotaleLoonKost_Geeft0NietsIngevuld()
        {
            Assert.Equal(((LoonKost)_kost).BerekenTotaleLoonKost() ,0M);
        }

        [Fact]
        public void BerekenResultaat_Geeft0NietsIngevuld()
        {
            Assert.Equal(_kost.BerekenResultaat(), 0M);
        }

        [Fact]
        public void BerekenTotaleLoonKost()
        {
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            Doelgroep boven60 = new Doelgroep("Boven60", 4466.66M, 1500M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = boven60;
            _kost.VulVeldIn(0, "% Vlaamse ondersteuningspremie", 0.2M);
            _kost.VulVeldIn(0, "aantal maanden IBO", 2M);
            _kost.VulVeldIn(0, "totale productiviteitspremie IBO", 100M);
            _kost.VoegLijnToe();
            _kost.VulVeldIn(1, "bruto maandloon fulltime", 1200M);
            _kost.VulVeldIn(1, "uren per week", 40.0M);
            Doelgroep laaggeschoold = new Doelgroep("Laaggeschoold", 2500M, 1550M);
            ((LoonKostLijn)_kost.Lijnen[0]).Doelgroep = laaggeschoold;
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
            _kost.VoegLijnToe();
            _kost.VulVeldIn(1, "bruto maandloon fulltime", 1200M);
            _kost.VulVeldIn(1, "uren per week", 40.0M);
            Assert.Equal(_kost.BerekenResultaat(), 35640M);
            Assert.Equal(_kost.Resultaat, 35640M);
        }

        #endregion
    }


}
