using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using KostenBatenTool.Models.Domain;

namespace KostenBatenToolTests.Models
{
    public class AdministratieBegeleidingsKostTest
    {
        #region Fields
        private readonly Berekening _kost;
        private Analyse _analyse;
        private Organisatie _organisatie;
        #endregion

        #region Constructors
        public AdministratieBegeleidingsKostTest()
        {
            _organisatie = new Organisatie("a", "b", "c", 1000, "d");
            _organisatie.UrenWerkWeek = 40.0M;
            _organisatie.PatronaleBijdrage = 0.35M;
            _analyse = new Analyse(_organisatie);
            _kost = new AdministratieBegeleidingsKost(_analyse);
        }
        #endregion

        #region Tests

        [Fact]
        public void AdministratieBegeleidingsKost_MaaktJuisteVeldenAan()
        {
            Assert.Equal(_kost.Velden["uren"], typeof(decimal));
            Assert.Equal(_kost.Velden["bruto maandloon begeleider"], typeof(decimal));
            Assert.Equal(_kost.Velden["jaarbedrag"], typeof(decimal));
            
        }

        [Fact]
        public void AdministratieBegeleidingsKost_MaaktJuisteLijnAan()
        {
            Assert.True(_kost.Lijnen[0].Any(v => v.Key.Equals("uren")));
            Assert.True(_kost.Lijnen[0].Any(v => v.Key.Equals("bruto maandloon begeleider")));
            Assert.True(_kost.Lijnen[0].Any(v => v.Key.Equals("jaarbedrag")));
        }

        [Fact]
        public void AanpassingsKost_ZetGetallenOp0()
        {
            Assert.Equal(_kost.Lijnen[0].First(v => v.Key.Equals("uren")).Value, 0M);
            Assert.Equal(_kost.Lijnen[0].First(v => v.Key.Equals("bruto maandloon begeleider")).Value, 0M);
            Assert.Equal(_kost.Lijnen[0].First(v => v.Key.Equals("jaarbedrag")).Value, 0M);
        }

        [Fact]
        public void VulUrenIn()
        {
            _kost.VulVeldIn(0, "uren", 38M);
            Assert.Equal(_kost.Lijnen[0].First(v => v.Key.Equals("uren")).Value, 38M);
        }

        [Fact]
        public void VulUrenIn_GooitExceptieNegatieveWaarde()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "uren", -1M));
        }

        [Fact]
        public void VulUrenIn_GooitExceptieDouble()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "uren", 1.0));
        }

        [Fact]
        public void VulUrenIn_GooitExceptieString()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "uren", "100"));
        }

        [Fact]
        public void VulUrenIn_GooitExceptieIndexNegatief()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(-1, "uren", 1000M));
        }

        [Fact]
        public void vulUrenIn_GooitExceptieIndexTeGroot()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(2, "uren", 1000M));

        }

        [Fact]
        public void vulUrenIn_VoegtNieuweLijnToeVorigeLijnNietIngevuld()
        {
            _kost.VulVeldIn(1, "uren", 38.5M);
            Assert.Equal(_kost.Lijnen[1].First(v => v.Key.Equals("uren")).Value, 38.5M);

        }

        [Fact]
        public void VulUrenIn_VoegtNieuweLijnToe()
        {
            _kost.VulVeldIn(0, "uren", 40M);
            _kost.VulVeldIn(1, "uren", 38.5M);
            Assert.Equal(_kost.Lijnen[1].First(v => v.Key.Equals("uren")).Value, 38.5M);
        }

        [Fact]
        public void VulUrenIn_VoegtNieuweLijnToeZetAndereBedragenOp0()
        {
            _kost.VulVeldIn(0, "uren", 40M);
            _kost.VulVeldIn(1, "uren", 38.5M);
            Assert.Equal(_kost.Lijnen[1].First(v => v.Key.Equals("bruto maandloon begeleider")).Value, 0M);
            Assert.Equal(_kost.Lijnen[1].First(v => v.Key.Equals("jaarbedrag")).Value, 0M);
        }

        [Fact]
        public void VulMaandloonIn()
        {
            _kost.VulVeldIn(0, "bruto maandloon begeleider", 1200M);
            Assert.Equal(_kost.Lijnen[0].First(v => v.Key.Equals("bruto maandloon begeleider")).Value, 1200M);
        }

        [Fact]
        public void VulMaandloonIn_VoegtNieuweLijnToe()
        {
            _kost.VulVeldIn(0, "bruto maandloon begeleider", 1000M);
            _kost.VulVeldIn(1, "bruto maandloon begeleider", 1200M);
            Assert.Equal(_kost.Lijnen[1].First(v => v.Key.Equals("bruto maandloon begeleider")).Value, 1200M);
        }

        [Fact]
        public void VulMaandloonIn_GooitExceptieKeyBestaatNiet()
        {
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(0, "functie", 1200M));
        }

        
        [Fact]
        public void VulMaandloonIn_GooitExceptieKeyBestaatNietTweedeLijn()
        {
            _kost.VulVeldIn(0, "bruto maandloon begeleider", 1000M);
            Assert.Throws<ArgumentException>(() => _kost.VulVeldIn(1, "functie", 1200M));
        }

        [Fact]
        public void vulMaandloonInIn_VoegtNieuweLijnToeVorigeLijnNietIngevuld()
        {
            _kost.VulVeldIn(1, "bruto maandloon begeleider", 1000M);
            Assert.Equal(_kost.Lijnen[1].First(v => v.Key.Equals("bruto maandloon begeleider")).Value, 1000M);

        }

        [Fact]
        public void VulMaandloonIn_WaardeWijzigen()
        {
            _kost.VulVeldIn(0,"bruto maandloon begeleider", 1200M);
            _kost.VulVeldIn(0, "bruto maandloon begeleider", 1000M);
            Assert.Equal(_kost.Lijnen[0].First(v => v.Key.Equals("bruto maandloon begeleider")).Value, 1000M);
        }

        
        [Fact]
        public void BerekenKostPerLijn()
        {
            
            _kost.VulVeldIn(0, "uren", 38.5M);
            _kost.VulVeldIn(0, "bruto maandloon begeleider", 1200M);
            Assert.Equal(_kost.BerekenBedragPerLijn(0), 20790M);
            Assert.Equal(_kost.Lijnen[0].First(v => v.Key.Equals("jaarbedrag")).Value, 20790M);
        }

        [Fact]
        public void BerekenKostPerLijn_TweedeLijn()
        {
            _kost.VulVeldIn(0, "uren", 38.5M);
            _kost.VulVeldIn(0, "bruto maandloon begeleider", 1200M);
            _kost.VulVeldIn(1, "uren", 40M);
            _kost.VulVeldIn(1, "bruto maandloon begeleider", 1000M);
            Assert.Equal(_kost.BerekenBedragPerLijn(1), 18000M);
            Assert.Equal(_kost.Lijnen[1].First(v => v.Key.Equals("jaarbedrag")).Value, 18000M);
        }

        [Fact]
        public void BerekenKostPerLijn_Geeft0UrenNietIngevuld()
        {
            _kost.VulVeldIn(0, "bruto maandloon begeleider", 1200M);
            Assert.Equal(_kost.BerekenBedragPerLijn(0), 0);
            Assert.Equal(_kost.Lijnen[0].First(v => v.Key.Equals("jaarbedrag")).Value, 0M);
        }

        [Fact]
        public void BerekenKostPerLijn_Geeft0MaandloonNietIngevuld()
        {
            _kost.VulVeldIn(0, "uren", 38.5M);
            Assert.Equal(_kost.BerekenBedragPerLijn(0), 0);
            Assert.Equal(_kost.Lijnen[0].First(v => v.Key.Equals("jaarbedrag")).Value, 0M);

        }

        [Fact]
        public void BerekenKostPerLijn_Geeft0NietsIngevuld()
        {
            Assert.Equal(_kost.BerekenBedragPerLijn(0), 0);
            Assert.Equal(_kost.Lijnen[0].First(v => v.Key.Equals("jaarbedrag")).Value, 0M);
        }

        [Fact]
        public void BerekenKostPerLijn_Geeft0UrenNietIngevuldTweedeLijn()
        {
            _kost.VulVeldIn(0, "uren", 38.5M);
            _kost.VulVeldIn(0, "bruto maandloon begeleider", 1200M);
            _kost.VulVeldIn(1, "bruto maandloon begeleider", 1000M);
            Assert.Equal(_kost.BerekenBedragPerLijn(1), 0);
            Assert.Equal(_kost.Lijnen[1].First(v => v.Key.Equals("jaarbedrag")).Value, 0M);

        }

        [Fact]
        public void BerekenKostPerLijn_Geeft0MaandloonNietIngevuldTweedeLijn()
        {
            _kost.VulVeldIn(0, "uren", 38.5M);
            _kost.VulVeldIn(0, "bruto maandloon begeleider", 1200M);
            _kost.VulVeldIn(1, "uren", 40M);
            Assert.Equal(_kost.BerekenBedragPerLijn(1),0);
            Assert.Equal(_kost.Lijnen[1].First(v => v.Key.Equals("jaarbedrag")).Value, 0M);

        }

        [Fact]
        public void BerekenResultaat()
        {
            _kost.VulVeldIn(0, "uren", 38.5M);
            _kost.VulVeldIn(0, "bruto maandloon begeleider", 1200M);
            _kost.VulVeldIn(1, "uren", 40M);
            _kost.VulVeldIn(1, "bruto maandloon begeleider", 1000M);
            Assert.Equal(_kost.BerekenResultaat(), 38790M);
        }

        [Fact]
        public void BerekenResultaat_NietsIngevuldGeeft0()
        {
            Assert.Equal(_kost.BerekenResultaat(), 0M);
        }

        [Fact]
        public void BerekenResultaat_TweedeLijnUrenNietIngevuld()
        {
            _kost.VulVeldIn(0, "uren", 38.5M);
            _kost.VulVeldIn(0, "bruto maandloon begeleider", 1200M);
            _kost.VulVeldIn(1, "bruto maandloon begeleider", 1000M);
            Assert.Equal(_kost.BerekenResultaat(), 20790M);
        }
        #endregion
    }
}
