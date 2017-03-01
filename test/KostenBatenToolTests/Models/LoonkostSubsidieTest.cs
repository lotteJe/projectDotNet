using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using KostenBatenTool.Models.Domain;
namespace KostenBatenToolTests.Models
{
    public class LoonkostSubsidieTest
    {

        #region Fields

        private Berekening _baat;
        private LoonKost _kost;
        private Analyse _analyse;

        #endregion

        #region Constructors

        public LoonkostSubsidieTest()
        {
            Organisatie o = new Organisatie("a", "b", "c", 1000, "d");
            _analyse = new Analyse(o);
            _kost = new LoonKost(_analyse);
            _kost.VulVeldIn(0, "bruto maandloon fulltime", 1000M);
            _kost.VulVeldIn(0, "uren per week", 40.0M);
            _kost.VulVeldIn(0, "doelgroep", Doelgroep.Boven60);
            _kost.VulVeldIn(0, "% Vlaamse ondersteuningspremie", 0.2M);
            _kost.VulVeldIn(0, "aantal maanden IBO", 2M);
            _kost.VulVeldIn(0, "totale productiviteitspremie IBO", 100M);
            _kost.VulVeldIn(1, "bruto maandloon fulltime", 1200M);
            _kost.VulVeldIn(1, "uren per week", 40.0M);
            _kost.VulVeldIn(1, "doelgroep", Doelgroep.Laaggeschoold);
            _kost.VulVeldIn(1, "% Vlaamse ondersteuningspremie", 0.4M);
            _kost.VulVeldIn(1, "aantal maanden IBO", 3M);
            _kost.VulVeldIn(1, "totale productiviteitspremie IBO", 200M);
            _baat = new LoonkostSubsidie(_kost);
        }
        #endregion

        #region Tests

        [Fact]
        public void LoonKostSubsidie_MaaktJuisteVeldenAan()
        {
            Assert.Equal(_baat.Velden["Totale loonkostsubsidie"], typeof(decimal));
        }

        [Fact]
        public void LoonKost_MaaktJuisteLijnAan()
        {
            Assert.True(_baat.Lijnen[0].ContainsKey("Totale loonkostsubsidie"));
        }

        [Fact]
        public void BerekenBedragPerLijn()
        {
            //return testen
            //invulling veld testen
            //Assert.Equal(_baat.Velden["Totale loonkostsubsidie"],);
        }

        [Fact]
        public void BerekenBedragPerLijn_GooitExceptieIndexGroterDanNul()
        {
            
        }

        [Fact]
        public void BerekenResultaat()
        {
            
        }

        #endregion
        }
}
