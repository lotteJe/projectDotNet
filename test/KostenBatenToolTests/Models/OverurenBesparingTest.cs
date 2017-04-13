using KostenBatenTool.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KostenBatenToolTests.Models
{
    public class OverurenBesparingTest
    {
        #region Fields
        private readonly Berekening _baat;
        #endregion

        #region Constructors
        public OverurenBesparingTest()
        {
            Organisatie o = new Organisatie("a", "b", "c", "1000", "d");
            o.UrenWerkWeek = 40.0M;
            o.PatronaleBijdrage = 0.35M;
            Analyse a = new Analyse(o);
            _baat = new OverurenBesparing(a);
        }
        #endregion

        #region Tests

        [Fact]
        public void OverurenBesparing_MaaktJuisteVeldenAan()
        {
            Assert.Equal(_baat.Velden.Find(v => v.VeldKey.Equals("jaarbedrag")).Value, typeof(decimal));

        }

        [Fact]
        public void OverurenBesparing_MaaktJuisteLijnAan()
        {
            Assert.True(_baat.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("jaarbedrag")));
        }

        [Fact]
        public void OverurenBesparing_ZetBedragOp0()
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
        public void BerekenBaatPerLijn()
        {
            _baat.VulVeldIn(0, "jaarbedrag", 1200M);
            Assert.Equal(_baat.BerekenBedragPerLijn(0), 1200M);
        }


        [Fact]
        public void BerekenResultaat()
        {
            _baat.VulVeldIn(0, "jaarbedrag", 1200M);
            Assert.Equal(_baat.BerekenResultaat(), 1200);
            Assert.Equal(_baat.Resultaat, 1200M);
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
