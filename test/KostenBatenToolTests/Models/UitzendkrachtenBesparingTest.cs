using KostenBatenTool.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KostenBatenToolTests.Models
{
    public class UitzendkrachtenBesparingTest
    {
        #region Fields
        private readonly Berekening _baat;
        #endregion

        #region Constructors
        public UitzendkrachtenBesparingTest()
        {
            Organisatie o = new Organisatie("a", "b", "c", "1000", "d");
            o.UrenWerkWeek = 40.0M;
            o.PatronaleBijdrage = 0.35M;
            Analyse a = new Analyse(o);
            _baat = new UitzendkrachtenBesparing(a);
        }
        #endregion

        #region Tests

        [Fact]
        public void UitzendkrachtenBesparing_MaaktJuisteVeldenAan()
        {
            Assert.Equal(_baat.Velden.Find(v => v.VeldKey.Equals("beschrijving")).Value, typeof(string));
            Assert.Equal(_baat.Velden.Find(v => v.VeldKey.Equals("jaarbedrag")).Value, typeof(decimal));

        }

        [Fact]
        public void UitzendkrachtenBesparing_MaaktJuisteLijnAan()
        {
            Assert.True(_baat.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("beschrijving")));
            Assert.True(_baat.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("jaarbedrag")));
        }

        [Fact]
        public void UitzendkrachtenBesparing_ZetBedragOp0()
        {
            Assert.Equal(_baat.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag")).Value, 0M);
        }

        [Fact]
        public void VulJaarbedragIn()
        {
            _baat.VulVeldIn(0, "jaarbedrag", 1200M);
            Assert.Equal(_baat.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag")).Value, 1200M);
        }

        [Fact]
        public void VulJaarbedragIn_GooitExceptieNegatieveWaarde()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(0, "jaarbedrag", -1M));
        }

        [Fact]
        public void VulJaarbedragIn_GooitExceptieDouble()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(0, "jaarbedrag", 1.0));
        }

        [Fact]
        public void VulJaarbedragIn_GooitExceptieString()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(0, "jaarbedrag", "100"));
        }

        [Fact]
        public void VulJaarbedragIn_GooitExceptieIndexNegatief()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(-1, "jaarbedrag", 1000M));
        }

        [Fact]
        public void vulJaarbedragIn_GooitExceptieIndexTeGroot()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(2, "jaarbedrag", 1000M));

        }
        [Fact]
        public void vulJaarbedragIn_VoegtLijnToeVorigeLijnNietIngevuld()
        {
            _baat.VulVeldIn(1, "jaarbedrag", 1200M);
            Assert.Equal(_baat.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag")).Value, 1200M);

        }

        [Fact]
        public void VulJaarbedragIn_VoegtNieuweLijnToe()
        {
            _baat.VulVeldIn(0, "jaarbedrag", 1000M);
            _baat.VulVeldIn(1, "jaarbedrag", 1200M);
            Assert.Equal(_baat.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag")).Value, 1200M);
        }

        [Fact]
        public void VulBeschrijvingIn()
        {
            _baat.VulVeldIn(0, "beschrijving", "test");
            Assert.Equal(_baat.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("beschrijving")).Value, "test");
        }

        [Fact]
        public void VulBeschrijvingIn_VoegtNieuweLijnToe()
        {
            _baat.VulVeldIn(0, "beschrijving", "test0");
            _baat.VulVeldIn(1, "beschrijving", "test");
            Assert.Equal(_baat.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("beschrijving")).Value, "test");
        }

        [Fact]
        public void VulBeschrijvingIn_VoegtNieuweLijnToeVorigeNietLeeg()
        {
            _baat.VulVeldIn(0, "jaarbedrag", 1000M);
            _baat.VulVeldIn(1, "beschrijving", "test");
            Assert.Equal(_baat.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("beschrijving")).Value, "test");
        }

        [Fact]
        public void VulBeschrijvingIn_GooitExceptieKeyBestaatNiet()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(0, "beschrijvingen", "test"));
        }

        [Fact]
        public void VulBeschrijvingIn_GooitExceptieKeyBestaatNietTweedeLijn()
        {
            _baat.VulVeldIn(0, "beschrijving", "test");
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(1, "beschrijvingen", "test"));
        }


        [Fact]
        public void VulBeschrijvingeIn_WaardeWijzigen()
        {
            _baat.VulVeldIn(0, "beschrijving", "test");
            _baat.VulVeldIn(0, "beschrijving", "test2");
            Assert.Equal(_baat.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("beschrijving")).Value, "test2");
        }

        [Fact]
        public void BerekenBaatPerLijn() 
        {
            _baat.VulVeldIn(0, "jaarbedrag", 1200M);
            Assert.Equal(_baat.BerekenBedragPerLijn(0), 1200M);
        }

        [Fact]
        public void BerekenBaatPerLijn_TweedeLijn()
        {
            _baat.VulVeldIn(0, "jaarbedrag", 1200M);
            _baat.VulVeldIn(1, "jaarbedrag", 1000M);
            Assert.Equal(_baat.BerekenBedragPerLijn(1), 1000M);
        }

        [Fact]
        public void BerekenResultaat()
        {
            _baat.VulVeldIn(0, "jaarbedrag", 1200M);
            _baat.VulVeldIn(1, "jaarbedrag", 1000M);
            Assert.Equal(_baat.BerekenResultaat(), 2200M);
            Assert.Equal(_baat.Resultaat, 2200M);
        }

        [Fact]
        public void BerekenKostPerLijn_NietsIngevuldGeeft0()
        {
            Assert.Equal(_baat.BerekenBedragPerLijn(0), 0M);
        }

        [Fact]
        public void BerekenResultaat_NietsIngevuldGeeft0()
        {
            Assert.Equal(_baat.BerekenResultaat(), 0M);
        }
        #endregion

    }
}
