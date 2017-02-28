using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;
using Xunit;
namespace KostenBatenToolTests.Models
{
    public class OrganisatieTest
    {
        #region Fields
        private Organisatie _organisatie;
        #endregion

        #region Constructors
        public OrganisatieTest()
        {
            _organisatie = new Organisatie("naam","straat", "1a", 1000, "gemeente");
        }
        #endregion

        #region Tests
        [Fact]
        public void Constructor_Test()
        {
            Assert.Equal(_organisatie.Naam, "naam");
            Assert.Equal(_organisatie.Straat, "straat");
            Assert.Equal(_organisatie.Huisnummer, "1a");
            Assert.Equal(_organisatie.Postcode, 1000);
            Assert.Equal(_organisatie.Gemeente, "gemeente");
        }
        [Fact]
        public void SetPatronaleBijdrage()
        {
            _organisatie.PatronaleBijdrage = 0.35M;
            Assert.Equal(_organisatie.PatronaleBijdrage, 0.35M);
        }

        [Fact]
        public void SetPatronaleBijdrage_GooitExceptieNegatiefGetal()
        {
            Assert.Throws<ArgumentException>(() => _organisatie.PatronaleBijdrage = -1M);
        }
        [Fact]
        public void SetPatronaleBijdrage_GooitExceptieGroterDan1()
        {
            Assert.Throws<ArgumentException>(() => _organisatie.PatronaleBijdrage = 1.01M);
        }
        #endregion
    }
}
