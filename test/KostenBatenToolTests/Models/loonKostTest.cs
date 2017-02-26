using System;
using KostenBatenTool.Models.Domain;
using Xunit;
using Moq;

namespace KostenBatenToolTests.Models
{
    public class LoonKostTest
    {
        [Fact]
        public void NewArbeidsBemiddelaarTest()
        {
            Persoon p = new Administrator(" ", "fdg", "h");
            Assert.Equal(" ", p.Naam);
        }

        private Mock<Analyse> _analyse;
        //ctor testen

        [Fact]
        public void LoonKostMakenTest()
        {
            _analyse = new Mock<Analyse>();
            //_analyse.Setup(m => m.Organisatie.UrenWerkWeek).Returns(100.0);
            //_analyse.Setup(m => m.Organisatie.PatronaleBijdrage).Returns(35M);
            Kost kost = new LoonKost(_analyse.Object);
            Assert.Equal(kost.Velden["functie"], typeof(string));
        }

        //BerekenMaandloonPatronaalPerLijn

        //BerekenGemiddeldeVopPerMaandPerLijn

        //BerekenDoelgroepVerminderingPerLijn_DoelgroepLaaggeschooldMaandloonMinderDan2500

        //BerekenDoelgroepVerminderingPerLijn_DoelgroepLaaggeschooldMaandloonMeerDan2500

        //BerekenDoelgroepVerminderingPerLijnDoelgroepMiddengeschooldMaandloonMinderDan2500

        //BerekenDoelgroepVerminderingPerLijn_DoelgroepLaaggeschooldMaandloonMeerDan2500

        //BerekenDoelgroepVerminderingPerLijn_DoelgroepTussen55En60dMaandloonMinderDan4466

        //BerekenDoelgroepVerminderingPerLijn_DoelgroepTussen55En60dMaandloonMeerDan4466

        //BerekenDoelgroepVerminderingPerLijn_DoelgroepBoven60MaandloonMinderDan4466

        //BerekenDoelgroepVerminderingPerLijn_DoelgroepBoven60MaandloonMeerDan4466

        // Ook klasse Kost overal testen

    }
}
