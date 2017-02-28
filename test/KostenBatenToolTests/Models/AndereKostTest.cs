using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using KostenBatenTool.Models.Domain;

namespace KostenBatenToolTests.Models
{
    public class AndereKostTest
    {
        #region Fields
        private readonly Kost _kost;
        #endregion

        #region Constructors
        public AndereKostTest()
        {
            _kost = new AndereKost();
        }
        #endregion

        #region Tests

        [Fact]
        public void AndereKost_MaaktJuisteVeldenAan()
        {
            Assert.Equal(_kost.Velden["type"], typeof(string));
            Assert.Equal(_kost.Velden["bedrag"], typeof(decimal));

        }

        [Fact]
        public void AndereKost_MaaktJuisteLijnAan()
        {
            Assert.True(_kost.Lijnen[0].ContainsKey("type"));
            Assert.True(_kost.Lijnen[0].ContainsKey("bedrag"));
        }

        [Fact]
        public void VulBedragIn()
        {
            _kost.VulVeldIn(0, "bedrag", 1200M);
            Assert.Equal(_kost.Lijnen[0]["bedrag"], 1200M);
        }

        [Fact]
        public void VulBedragIn_GooitExceptieNegatieveWaarde()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "bedrag", -1M));
        }

        [Fact]
        public void VulBedragIn_GooitExceptieDouble()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "bedrag", 1.0));
        }

        [Fact]
        public void VulBedragIn_GooitExceptieString()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "bedrag", "100"));
        }

        [Fact]
        public void VulBedragIn_GooitExceptieIndexNegatief()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(-1, "bedrag", 1000M));
        }

        [Fact]
        public void vulBedragIn_GooitExceptieIndexTeGroot()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(2, "bedrag", 1000M));

        }
        [Fact]
        public void vulBedragIn_GooitExceptieVorigeLijnNietIngevuld()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(1, "bedrag", 1000M));

        }

        [Fact]
        public void VulBedragIn_VoegtNieuweLijnToe()
        {
            _kost.VulVeldIn(0, "bedrag", 1000M);
            _kost.VulVeldIn(1, "bedrag", 1200M);
            Assert.Equal(_kost.Lijnen[1]["bedrag"], 1200M);
        }

        [Fact]
        public void VulTypeIn()
        {
            _kost.VulVeldIn(0, "type", "test");
            Assert.Equal(_kost.Lijnen[0]["type"], "test");
        }

        [Fact]
        public void VulTypeIn_VoegtNieuweLijnToe()
        {
            _kost.VulVeldIn(0, "type", "test0");
            _kost.VulVeldIn(1, "type", "test");
            Assert.Equal(_kost.Lijnen[1]["type"], "test");
        }

        [Fact]
        public void VulTypeIn_VoegtNieuweLijnToeVorigeNietLeeg()
        {
            _kost.VulVeldIn(0, "bedrag", 1000M);
            _kost.VulVeldIn(1, "type", "test");
            Assert.Equal(_kost.Lijnen[1]["type"], "test");
        }

        [Fact]
        public void VulTypeIn_GooitExceptieKeyBestaatNiet()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "types", "test"));
        }

        [Fact]
        public void VulTypeIn_GooitExceptieKeyBestaatNietTweedeLijn()
        {
            _kost.VulVeldIn(0, "type", "test");
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(1, "types", "test"));
        }

        [Fact]
        public void vulTypeIn_GooitExceptieVorigeLijnNietIngevuld()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(1, "type", "test"));

        }

        [Fact]
        public void VulTypeIn_WaardeWijzigen()
        {
            _kost.VulVeldIn(0, "type", "test");
            _kost.VulVeldIn(0, "type", "test2");
            Assert.Equal(_kost.Lijnen[0]["type"], "test2");
        }

        [Fact]
        public void BerekenKostPerLijn()
        {
            _kost.VulVeldIn(0, "bedrag", 1200M);
            Assert.Equal(_kost.BerekenKostPerLijn(0), 1200M);
        }

        [Fact]
        public void BerekenKostPerLijn_TweedeLijn()
        {
            _kost.VulVeldIn(0, "bedrag", 1200M);
            _kost.VulVeldIn(1, "bedrag", 1000M);
            Assert.Equal(_kost.BerekenKostPerLijn(1), 1000M);
        }

        [Fact]
        public void BerekenResultaat()
        {
            _kost.VulVeldIn(0, "bedrag", 1200M);
            _kost.VulVeldIn(1, "bedrag", 1000M);
            Assert.Equal(_kost.BerekenResultaat(), 2200M);
        }
        #endregion
    }
}
