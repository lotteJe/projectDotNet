using KostenBatenTool.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KostenBatenToolTests.Models
{
    public class MedewerkerZelfdeNiveauBesparingTest
    {
        #region Fields
        private readonly Berekening _baat;
        private Analyse _analyse;
        private Organisatie _organisatie;
        #endregion

        #region Constructors
        public MedewerkerZelfdeNiveauBesparingTest()
        {
            _organisatie = new Organisatie("a", "b", "c", 1000, "d");
            _organisatie.UrenWerkWeek = 40.0M;
            _organisatie.PatronaleBijdrage = 0.35M;
            _analyse = new Analyse(_organisatie);
            _baat = new MedewerkerZelfdeNiveauBesparing(_analyse);
        }
        #endregion

        #region Tests

        [Fact]
        public void MedewerkerZelfdeNiveauBesparing_MaaktJuisteVeldenAan()
        {
            Assert.Equal(_baat.Velden["uren"], typeof(decimal));
            Assert.Equal(_baat.Velden["bruto maandloon fulltime"], typeof(decimal));
            Assert.Equal(_baat.Velden["totale loonkost per jaar"], typeof(decimal));

        }

        [Fact]
        public void MedewerkerZelfdeNiveauBesparing_MaaktJuisteLijnAan()
        {
            Assert.True(_baat.Lijnen[0].ContainsKey("uren"));
            Assert.True(_baat.Lijnen[0].ContainsKey("bruto maandloon fulltime"));
            Assert.True(_baat.Lijnen[0].ContainsKey("totale loonkost per jaar"));
        }

        [Fact]
        public void VulUrenIn()
        {
            _baat.VulVeldIn(0, "uren", 38M);
            Assert.Equal(_baat.Lijnen[0]["uren"], 38M);
        }

        [Fact]
        public void VulUrenIn_GooitExceptieNegatieveWaarde()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(0, "uren", -1M));
        }

        [Fact]
        public void VulUrenIn_GooitExceptieDouble()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(0, "uren", 1.0));
        }

        [Fact]
        public void VulUrenIn_GooitExceptieString()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(0, "uren", "100"));
        }

        [Fact]
        public void VulUrenIn_GooitExceptieIndexNegatief()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(-1, "uren", 1000M));
        }

        [Fact]
        public void vulUrenIn_GooitExceptieIndexTeGroot()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(2, "uren", 1000M));

        }

        [Fact]
        public void vulUrenIn_VoegtLijnToeVorigeLijnNietIngevuld()
        {
            _baat.VulVeldIn(1, "uren", 38.5M);
            Assert.Equal(_baat.Lijnen[1]["uren"], 38.5M);

        }

        [Fact]
        public void VulUrenIn_VoegtNieuweLijnToe()
        {
            _baat.VulVeldIn(0, "uren", 40M);
            _baat.VulVeldIn(1, "uren", 38.5M);
            Assert.Equal(_baat.Lijnen[1]["uren"], 38.5M);
        }

        [Fact]
        public void VulMaandloonIn()
        {
            _baat.VulVeldIn(0, "bruto maandloon fulltime", 1200M);
            Assert.Equal(_baat.Lijnen[0]["bruto maandloon fulltime"], 1200M);
        }

        [Fact]
        public void VulMaandloonIn_VoegtNieuweLijnToe()
        {
            _baat.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _baat.VulVeldIn(1, "bruto maandloon fulltime", 1200M);
            Assert.Equal(_baat.Lijnen[1]["bruto maandloon fulltime"], 1200M);
        }

        [Fact]
        public void VulMaandloonIn_GooitExceptieKeyBestaatNiet()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(0, "functie", 1200M));
        }


        [Fact]
        public void VulMaandloonIn_GooitExceptieKeyBestaatNietTweedeLijn()
        {
            _baat.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(1, "functie", 1200M));
        }

        [Fact]
        public void vulMaandloonIn_GooitExceptieVorigeLijnNietIngevuld()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(1, "bruto maandloon fulltime", "test"));

        }

        [Fact]
        public void VulMaandloonIn_WaardeWijzigen()
        {
            _baat.VulVeldIn(0, "bruto maandloon fulltime", 1200M);
            _baat.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            Assert.Equal(_baat.Lijnen[0]["bruto maandloon fulltime"], 1000M);
        }

        [Fact]
        public void VulMaadloonIn_VoegtNieuweLijnToeVorigeNietLeeg()
        {
            _baat.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _baat.VulVeldIn(1, "uren", 40M);
            Assert.Equal(_baat.Lijnen[1]["uren"], 40M);
        }

        [Fact]
        public void BerekenBaatPerLijn()
        {

            _baat.VulVeldIn(0, "uren", 38.5M);
            _baat.VulVeldIn(0, "bruto maandloon fulltime", 1200M);
            Assert.Equal(_baat.BerekenBedragPerLijn(0), 21704.76M);
            Assert.Equal(_baat.Lijnen[0]["totale loonkost per jaar"], 21704.76M);
        }

        [Fact]
        public void BerekenBaatPerLijn_TweedeLijn()
        {
            _baat.VulVeldIn(0, "uren", 38.5M);
            _baat.VulVeldIn(0, "bruto maandloon fulltime", 1200M);
            _baat.VulVeldIn(1, "uren", 40M);
            _baat.VulVeldIn(1, "bruto maandloon fulltime", 1000M);
            Assert.Equal(_baat.BerekenBedragPerLijn(1), 18792M);
            Assert.Equal(_baat.Lijnen[1]["totale loonkost per jaar"], 18792M);
        }

        [Fact]
        public void BerekenBaatPerLijn_Geeft0UrenNietIngevuld()
        {
            _baat.VulVeldIn(0, "bruto maandloon fulltime", 1200M);
            Assert.Equal(_baat.BerekenBedragPerLijn(0),0M);
            Assert.Equal(_baat.Lijnen[0]["totale loonkost per jaar"], 0M);

        }

        [Fact]
        public void BerekenBaatPerLijn_Geeft0NietsIngevuld()
        {
            Assert.Equal(_baat.BerekenBedragPerLijn(0), 0M);
            Assert.Equal(_baat.Lijnen[0]["totale loonkost per jaar"], 0M);
        }

        [Fact]
        public void BerekenBaatPerLijn_GooitExceptieUrenWerkweek0()
        {
            ((MedewerkerHogerNiveauBesparing)_baat).Analyse.Organisatie.UrenWerkWeek = 0M;
            _baat.VulVeldIn(0, "uren", 40M);
            Assert.Throws<ArgumentException>(() => _baat.BerekenBedragPerLijn(0));

        }

        [Fact]
        public void BerekenBaatPerLijn_Geeft0MaandloonNietIngevuld()
        {
            _baat.VulVeldIn(0, "uren", 40M);
            Assert.Equal(_baat.BerekenBedragPerLijn(0), 0M);
            Assert.Equal(_baat.Lijnen[0]["totale loonkost per jaar"], 0M);
        }


        [Fact]
        public void BerekenBaatPerLijn_Geeft0UrenNull()
        {
            _baat.VulVeldIn(0, "uren", 40M);
            Assert.Equal(_baat.BerekenBedragPerLijn(0), 0M);
            Assert.Equal(_baat.Lijnen[0]["totale loonkost per jaar"], 0M);
        }


        [Fact]
        public void BerekenResultaat_Geeft0NietsIngevuld()
        {
            Assert.Equal(_baat.BerekenResultaat(), 0M);
            Assert.Equal(_baat.Lijnen[0]["totale loonkost per jaar"], 0M);

        }

        [Fact]
        public void BerekenResultaat()
        {
            _baat.VulVeldIn(0, "uren", 38.5M);
            _baat.VulVeldIn(0, "bruto maandloon fulltime", 1200M);
            _baat.VulVeldIn(1, "uren", 40M);
            _baat.VulVeldIn(1, "bruto maandloon fulltime", 1000M);
            Assert.Equal(_baat.BerekenResultaat(), 40496.76M);
        }
        #endregion
    }
}
