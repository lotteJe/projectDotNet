﻿using KostenBatenTool.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KostenBatenToolTests.Models
{
    public class ProductiviteitsWinstTest
    {
        #region Fields
        private readonly Berekening _baat;
        #endregion

        #region Constructors
        public ProductiviteitsWinstTest()
        {
            _baat = new ProductiviteitsWinst();
        }
        #endregion

        #region Tests

        [Fact]
        public void ProductiviteitsWinst_MaaktJuisteVeldenAan()
        {
            Assert.Equal(_baat.Velden["jaarbedrag"], typeof(decimal));

        }

        [Fact]
        public void ProductiviteitsWinst_MaaktJuisteLijnAan()
        {
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
        }
        #endregion
    }
}