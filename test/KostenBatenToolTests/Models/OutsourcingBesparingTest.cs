﻿using KostenBatenTool.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KostenBatenToolTests.Models
{
    public class OutsourcingBesparingTest
    {
        #region Fields
        private readonly Berekening _baat;
        #endregion

        #region Constructors
        public OutsourcingBesparingTest()
        {
            _baat = new OutsourcingBesparing();
        }
        #endregion

        #region Tests

        [Fact]
        public void OutsourcingBesparing_MaaktJuisteVeldenAan()
        {
            Assert.Equal(_baat.Velden["beschrijving"], typeof(string));
            Assert.Equal(_baat.Velden["jaarbedrag"], typeof(decimal));

        }

        [Fact]
        public void OutsourcingBesparing_MaaktJuisteLijnAan()
        {
            Assert.True(_baat.Lijnen[0].ContainsKey("beschrijving"));
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
        public void VulBeschrijvingIn()
        {
            _baat.VulVeldIn(0, "beschrijving", "test");
            Assert.Equal(_baat.Lijnen[0]["beschrijving"], "test");
        }

        [Fact]
        public void VulBeschrijvingIn_VoegtNieuweLijnToe()
        {
            _baat.VulVeldIn(0, "beschrijving", "test0");
            _baat.VulVeldIn(1, "beschrijving", "test");
            Assert.Equal(_baat.Lijnen[1]["beschrijving"], "test");
        }

        [Fact]
        public void VulBeschrijvingIn_VoegtNieuweLijnToeVorigeNietLeeg()
        {
            _baat.VulVeldIn(0, "jaarbedrag", 1000M);
            _baat.VulVeldIn(1, "beschrijving", "test");
            Assert.Equal(_baat.Lijnen[1]["beschrijving"], "test");
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
        public void vulBeschrijvingIn_GooitExceptieVorigeLijnNietIngevuld()
        {
            Assert.Throws<ArgumentException>(() => _baat.VulVeldIn(1, "beschrijving", "test"));

        }

        [Fact]
        public void VulBeschrijvingeIn_WaardeWijzigen()
        {
            _baat.VulVeldIn(0, "beschrijving", "test");
            _baat.VulVeldIn(0, "beschrijving", "test2");
            Assert.Equal(_baat.Lijnen[0]["beschrijving"], "test2");
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