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
            Organisatie o = new Organisatie("a", "b", "c", "1000", "d");
            o.UrenWerkWeek = 40.0M;
            o.PatronaleBijdrage = 0.35M;
            Analyse a = new Analyse(o);
            _baat = new LogistiekeBesparing(a);
        }

        [Fact]
        public void BerekenKostPerLijn_Geeft0EnkelTransportKostenIngevuld()
        {
            _baat.VulVeldIn(0, "transportkosten jaarbedrag", 100M);
            Assert.Equal(_baat.BerekenBedragPerLijn(0), 100M);
            Assert.Equal(_baat.Lijnen[0].VeldenWaarden.First(v => v.Key.Equals("totaalbedrag")).Value, 100M);
        }

        [Fact]
        public void BerekenKostPerLijn_Geeft0EnkelLogistiekeKostenIngevuld()
        {
            _baat.VulVeldIn(0, "logistieke kosten jaarbedrag", 100M);
            Assert.Equal(_baat.BerekenBedragPerLijn(0), 100M);
            Assert.Equal(_baat.Lijnen[0].VeldenWaarden.First(v => v.Key.Equals("totaalbedrag")).Value, 100M);
        }

        [Fact]
        public void BerekenKostPerLijn_Geeft0NietsIngevuld()
        {
            Assert.Equal(_baat.BerekenBedragPerLijn(0), 0M);
            Assert.Equal(_baat.Lijnen[0].VeldenWaarden.First(v => v.Key.Equals("totaalbedrag")).Value, 0M);
        }

        [Fact]
        public void BerekenResultaat_Geeft0NietsIngevuld()
        {
            Assert.Equal(_baat.BerekenResultaat(), 0M);

        }

    }
}
