﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using KostenBatenTool.Models.Domain;

namespace KostenBatenToolTests.Models
{
    public class OmzetverliesBesparingTest
    {
       
        #region Fields
        private readonly Berekening _baat;
        #endregion

        #region Constructors
        public OmzetverliesBesparingTest()
        {
            Organisatie o = new Organisatie("a", "b", "c", "1000", "d");
            o.UrenWerkWeek = 40.0M;
            o.PatronaleBijdrage = 0.35M;
            Analyse a = new Analyse(o);
            _baat = new OmzetverliesBesparing(a);
        }
        #endregion

        #region Tests

        [Fact]
        public void OmzetverliesBesparing_MaaktJuisteVeldenAan()
        {
            Assert.Equal(_baat.Velden.Find(v => v.VeldKey.Equals("jaarbedrag omzetverlies")).Value, typeof(decimal));
            Assert.Equal(_baat.Velden.Find(v => v.VeldKey.Equals("% besparing")).Value, typeof(decimal));
            Assert.Equal(_baat.Velden.Find(v => v.VeldKey.Equals("totaalbesparing")).Value, typeof(decimal));
            
        }

        [Fact]
        public void OmzetverliesBesparing_MaaktJuisteLijnAan()
        {
            Assert.True(_baat.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("jaarbedrag omzetverlies")));
            Assert.True(_baat.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("% besparing"))); 
            Assert.True(_baat.Lijnen[0].VeldenWaarden.Any(v => v.VeldKey.Equals("totaalbesparing"))); 
        }

        [Fact]
        public void OmzetverliesBesparing_ZetBedragOp0()
        {
            Assert.Equal(_baat.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag omzetverlies")).Value, 0M);
            Assert.Equal(_baat.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("% besparing")).Value, 0M);
            Assert.Equal(_baat.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("totaalbesparing")).Value, 0M);
        }

        [Fact]
        public void VulJaarbedragIn()
        {
            _baat.VulVeldIn(0, "jaarbedrag omzetverlies", 1200M);
            Assert.Equal(_baat.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag omzetverlies")).Value, 1200M);
        }

        [Fact]
        public void VulJaarbedragIn_GooitExceptieNegatieveWaarde()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(0, "jaarbedrag omzetverlies", -1M));
        }

        [Fact]
        public void VulJaarbedragIn_GooitExceptieDouble()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(0, "jaarbedrag omzetverlies", 1.0));
        }

        [Fact]
        public void VulJaarbedragIn_GooitExceptieString()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(0, "jaarbedrag omzetverlies", "100"));
        }

        [Fact]
        public void VulJaarbedragIn_GooitExceptieIndexNegatief()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(-1, "jaarbedrag omzetverlies", 1000M));
        }

        [Fact]
        public void vulJaarbedragIn_GooitExceptieIndexTeGroot()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(2, "jaarbedrag omzetverlies", 1000M));

        }
        [Fact]
        public void vulJaarbedragIn_VoegtLijnToeVorigeLijnNietIngevuld()
        {
            _baat.VulVeldIn(1, "jaarbedrag omzetverlies", 1200M);
            Assert.Equal(_baat.Lijnen[1].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag omzetverlies")).Value, 1200M);
        }

        [Fact]
        public void VulPercentageIn()
        {
            _baat.VulVeldIn(0, "% besparing", 0.75M);
            Assert.Equal(_baat.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("% besparing")).Value, 0.75M);
        }

        [Fact]
        public void VulPercentageIn_GooitExceptieGroterDan1()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(0, "% besparing", 1.2M));
        }

        [Fact]
        public void BerekenBaatPerLijn()
        {
            _baat.VulVeldIn(0, "jaarbedrag omzetverlies", 1200M);
            _baat.VulVeldIn(0, "% besparing", 0.01M);
            Assert.Equal(_baat.BerekenBedragPerLijn(0), 12M);
            Assert.Equal(_baat.Lijnen[0].VeldenWaarden.First(v => v.VeldKey.Equals("totaalbesparing")).Value, 12M);
        }


        [Fact]
        public void BerekenResultaat()
        {
            _baat.VulVeldIn(0, "jaarbedrag omzetverlies", 1200M);
            _baat.VulVeldIn(0, "% besparing", 0.01M);
            Assert.Equal(_baat.BerekenResultaat(), 12M);
            Assert.Equal(_baat.Resultaat, 12M);
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
