﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using KostenBatenTool.Models.Domain;

namespace KostenBatenToolTests.Models
{
    public class VoorbereidingsKostTest
    {
        #region Fields
        private readonly Berekening _kost;
        #endregion

        #region Constructors
        public VoorbereidingsKostTest()
        {
            _kost = new VoorbereidingsKost();
        }
        #endregion

        #region Tests

        [Fact]
        public void VoorbereidingsKost_MaaktJuisteVeldenAan()
        {
            Assert.Equal(_kost.Velden["type"], typeof(string));
            Assert.Equal(_kost.Velden["bedrag"], typeof(decimal));

        }

        [Fact]
        public void VoorbereidingsKost_MaaktJuisteLijnAan()
        {
            Assert.True(_kost.Lijnen[0].Any(v => v.Key.Equals("type")));
            Assert.True(_kost.Lijnen[0].Any(v => v.Key.Equals("bedrag")));
        }

        [Fact]
        public void VoorbereidingsKost_ZetBedragOp0()
        {
            Assert.Equal(_kost.Lijnen[0].First(v => v.Key.Equals("bedrag")).Value, 0M);
        }

        [Fact]
        public void VulBedragIn()
        {
            _kost.VulVeldIn(0, "bedrag", 1200M);
            Assert.Equal(_kost.Lijnen[0].First(v => v.Key.Equals("bedrag")).Value, 1200M);
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
        public void vulBedragIn_VoegtLijnToeVorigeLijnNietIngevuld()
        {
            _kost.VulVeldIn(1, "bedrag", 1200M);
            Assert.Equal(_kost.Lijnen[1].First(v => v.Key.Equals("bedrag")).Value, 1200M);
        }

        [Fact]
        public void VulBedragIn_VoegtNieuweLijnToe()
        {
            _kost.VulVeldIn(0, "bedrag", 1000M);
            _kost.VulVeldIn(1, "bedrag", 1200M);
            Assert.Equal(_kost.Lijnen[1].First(v => v.Key.Equals("bedrag")).Value, 1200M);
        }

        [Fact]
        public void VulTypeIn()
        {
            _kost.VulVeldIn(0, "type", "test");
            Assert.Equal(_kost.Lijnen[0].First(v => v.Key.Equals("type")).Value, "test");
        }

        [Fact]
        public void VulTypeIn_VoegtNieuweLijnToe()
        {
            _kost.VulVeldIn(0, "type", "test0");
            _kost.VulVeldIn(1, "type", "test");
            Assert.Equal(_kost.Lijnen[1].First(v => v.Key.Equals("type")).Value, "test");
        }

        [Fact]
        public void VulTypeIn_VoegtNieuweLijnToeVorigeNietLeeg()
        {
            _kost.VulVeldIn(0, "bedrag", 1000M);
            _kost.VulVeldIn(1, "type", "test");
            Assert.Equal(_kost.Lijnen[1].First(v => v.Key.Equals("type")).Value, "test");
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
        public void VulTypeIn_WaardeWijzigen()
        {
            _kost.VulVeldIn(0, "type", "test");
            _kost.VulVeldIn(0, "type", "test2");
            Assert.Equal(_kost.Lijnen[0].First(v => v.Key.Equals("type")).Value, "test2");
        }

        [Fact]
        public void BerekenKostPerLijn()
        {
            _kost.VulVeldIn(0, "bedrag", 1200M);
            Assert.Equal(_kost.BerekenBedragPerLijn(0), 1200M);
        }

        [Fact]
        public void BerekenKostPerLijn_TweedeLijn()
        {
            _kost.VulVeldIn(0, "bedrag", 1200M);
            _kost.VulVeldIn(1, "bedrag", 1000M);
            Assert.Equal(_kost.BerekenBedragPerLijn(1), 1000M);
        }

        [Fact]
        public void BerekenResultaat()
        {
            _kost.VulVeldIn(0, "bedrag", 1200M);
            _kost.VulVeldIn(1, "bedrag", 1000M);
            Assert.Equal(_kost.BerekenResultaat(), 2200M);
        }

        [Fact]
        public void BerekenKostPerLijn_NietsIngevuldGeeft0()
        {
            Assert.Equal(_kost.BerekenBedragPerLijn(0), 0M);
        }

        [Fact]
        public void BerekenResultaat_NietsIngevuldGeeft0()
        {
            Assert.Equal(_kost.BerekenResultaat(), 0M);
        }
        #endregion
    }
}
