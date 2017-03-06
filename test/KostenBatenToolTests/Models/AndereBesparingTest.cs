﻿using KostenBatenTool.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KostenBatenToolTests.Models
{
    public class AndereBesparingTest
    {
        #region Fields
        private readonly Berekening _baat;
        #endregion

        #region Constructors
        public AndereBesparingTest()
        {
            _baat = new AndereBesparing();
        }
        #endregion

        #region Tests

        [Fact]
        public void AndereBesparing_MaaktJuisteVeldenAan()
        {
            Assert.Equal(_baat.Velden["type besparing"], typeof(string));
            Assert.Equal(_baat.Velden["jaarbedrag"], typeof(decimal));

        }

        [Fact]
        public void AndereBesparing_MaaktJuisteLijnAan()
        {
            Assert.True(_baat.Lijnen[0].ContainsKey("type besparing"));
            Assert.True(_baat.Lijnen[0].ContainsKey("jaarbedrag"));
        }

        [Fact]
        public void VulJaarbedragIn()
        {
            _baat.VulVeldIn(0, "jaarbedrag", 1200M);
            Assert.Equal(_baat.Lijnen[0]["jaarbedrag"], 1200M);
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
        public void vulJaarbedragIn_GooitExceptieVorigeLijnNietIngevuld()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(1, "jaarbedrag", 1000M));

        }

        [Fact]
        public void VulJaarbedragIn_VoegtNieuweLijnToe()
        {
            _baat.VulVeldIn(0, "jaarbedrag", 1000M);
            _baat.VulVeldIn(1, "jaarbedrag", 1200M);
            Assert.Equal(_baat.Lijnen[1]["jaarbedrag"], 1200M);
        }

        [Fact]
        public void VulTypeBesparingIn()
        {
            _baat.VulVeldIn(0, "type besparing", "test");
            Assert.Equal(_baat.Lijnen[0]["type besparing"], "test");
        }

        [Fact]
        public void VulTypeBesparingIn_VoegtNieuweLijnToe()
        {
            _baat.VulVeldIn(0, "type besparing", "test0");
            _baat.VulVeldIn(1, "type besparing", "test");
            Assert.Equal(_baat.Lijnen[1]["type besparing"], "test");
        }

        [Fact]
        public void VulTypeBesparingIn_VoegtNieuweLijnToeVorigeNietLeeg()
        {
            _baat.VulVeldIn(0, "jaarbedrag", 1000M);
            _baat.VulVeldIn(1, "type besparing", "test");
            Assert.Equal(_baat.Lijnen[1]["type besparing"], "test");
        }

        [Fact]
        public void VulTypeBesparingIn_GooitExceptieKeyBestaatNiet()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(0, "type besparingen", "test"));
        }

        [Fact]
        public void VulTypeBesparingIn_GooitExceptieKeyBestaatNietTweedeLijn()
        {
            _baat.VulVeldIn(0, "jaarbedrag", 1000M);
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(1, "type besparingen", "test"));
        }

        [Fact]
        public void vulTypeBesparingIn_GooitExceptieVorigeLijnNietIngevuld()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(1, "type besparing", "test"));

        }

        [Fact]
        public void VulTypeBesparingIn_WaardeWijzigen()
        {
            _baat.VulVeldIn(0, "type besparing", "test");
            _baat.VulVeldIn(0, "type besparing", "test2");
            Assert.Equal(_baat.Lijnen[0]["type besparing"], "test2");
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
        }
        #endregion
    }
}
