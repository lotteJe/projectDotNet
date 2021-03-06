﻿using System;
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
        private readonly Berekening _kost;
        #endregion

        #region Constructors
        public AndereKostTest()
        {
            Organisatie o = new Organisatie("a", "b", "c", "1000", "d");
            o.UrenWerkWeek = 40.0M;
            o.PatronaleBijdrage = 0.35M;
            Analyse a = new Analyse(o);
            _kost = new AndereKost(a);
        }
        #endregion

        #region Tests

        [Fact]
        public void AndereKost_MaaktJuisteVeldenAan()
        {
            Assert.Equal(_kost.Velden.Find(v => v.VeldKey.Equals("type")).Value, typeof(string));
            Assert.Equal(_kost.Velden.Find(v => v.VeldKey.Equals("bedrag")).Value, typeof(decimal));

        }

        [Fact]
        public void AndereKost_MaaktJuisteLijnAan()
        {
            Assert.True(_kost.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("type")));
            Assert.True(_kost.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("bedrag")));
        }

        [Fact]
        public void AndereKost_ZetBedragOp0()
        {
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("bedrag")).Value, 0M);
        }

        [Fact]
        public void VulBedragIn()
        {
            _kost.VulVeldIn(0, "bedrag", 1200M);
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("bedrag")).Value, 1200M);
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
            Assert.Equal(_kost.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("bedrag")).Value, 1200M);

        }

        [Fact]
        public void VulBedragIn_VoegtNieuweLijnToe()
        {
            _kost.VulVeldIn(0, "bedrag", 1000M);
            _kost.VulVeldIn(1, "bedrag", 1200M);
            Assert.Equal(_kost.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("bedrag")).Value, 1200M);
        }

        [Fact]
        public void VulTypeIn()
        {
            _kost.VulVeldIn(0, "type", "test");
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("type")).Value, "test");
        }

        [Fact]
        public void VulTypeIn_VoegtNieuweLijnToe()
        {
            _kost.VulVeldIn(0, "type", "test0");
            _kost.VulVeldIn(1, "type", "test");
            Assert.Equal(_kost.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("type")).Value, "test");
        }

        [Fact]
        public void VulTypeIn_VoegtNieuweLijnToeZetBedragOp0()
        {
            _kost.VulVeldIn(0, "type", "test0");
            _kost.VulVeldIn(1, "type", "test");
            Assert.Equal(_kost.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("bedrag")).Value, 0M);
        }

        [Fact]
        public void VulTypeIn_VoegtNieuweLijnToeVorigeNietLeeg()
        {
            _kost.VulVeldIn(0, "bedrag", 1000M);
            _kost.VulVeldIn(1, "type", "test");
            Assert.Equal(_kost.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("type")).Value, "test");
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
        public void vulTypeIn_VoegtLijnToeVorigeLijnNietIngevuld()
        {
            _kost.VulVeldIn(1, "type", "test2");
            Assert.Equal(_kost.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("type")).Value, "test2");

        }

        [Fact]
        public void VulTypeIn_WaardeWijzigen()
        {
            _kost.VulVeldIn(0, "type", "test");
            _kost.VulVeldIn(0, "type", "test2");
            Assert.Equal(_kost.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("type")).Value, "test2");
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

        public void BerekenKostPerLijn_NietsIngevuldGeeft0()
        {
            Assert.Equal(_kost.BerekenBedragPerLijn(0), 0M);
        }

        [Fact]

        public void BerekenResultaat()
        {
            _kost.VulVeldIn(0, "bedrag", 1200M);
            _kost.VulVeldIn(1, "bedrag", 1000M);
            Assert.Equal(_kost.BerekenResultaat(), 2200M);
            Assert.Equal(_kost.Resultaat, 2200M);
        }
        
        [Fact]
        public void BerekenResultaat_NietsIngevuldGeeft0()
        {
            Assert.Equal(_kost.BerekenResultaat(), 0M);
        }
        #endregion
    }
}
