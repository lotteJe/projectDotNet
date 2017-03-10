using KostenBatenTool.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KostenBatenToolTests.Models
{
    public class LogistiekeBesparingTest
    {
        private readonly Berekening _baat;

        public LogistiekeBesparingTest()
        {
            _baat = new LogistiekeBesparing();
        }

        [Fact]
        public void BerekenKostPerLijn_Geeft0EnkelTransportKostenIngevuld()
        {
            _baat.VulVeldIn(0, "transportkosten jaarbedrag", 100M);
            Assert.Equal(_baat.BerekenBedragPerLijn(0), 100M);
            Assert.Equal(_baat.Lijnen[0]["totaalbedrag"],100M);
        }

        [Fact]
        public void BerekenKostPerLijn_Geeft0EnkelLogistiekeKostenIngevuld()
        {
            _baat.VulVeldIn(0, "logistieke kosten jaarbedrag", 100M);
            Assert.Equal(_baat.BerekenBedragPerLijn(0), 100M);
            Assert.Equal(_baat.Lijnen[0]["totaalbedrag"], 100M);
        }

        [Fact]
        public void BerekenKostPerLijn_Geeft0NietsIngevuld()
        {
            Assert.Equal(_baat.BerekenBedragPerLijn(0), 0M);
            Assert.Equal(_baat.Lijnen[0]["totaalbedrag"], 0M);
        }

        [Fact]
        public void BerekenResultaat_Geeft0NietsIngevuld()
        {
            Assert.Equal(_baat.BerekenResultaat(), 0M);

        }

    }
}
