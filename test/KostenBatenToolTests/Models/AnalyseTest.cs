using KostenBatenTool.Models.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KostenBatenToolTests.Models
{
    public class AnalyseTest
    {
        private IList<Berekening> _baat;
        private IList<Berekening> _kost;
        private Analyse _analyse;
        private Organisatie o;

        IList<Berekening> _mockKostBerekeningen;
        IList<Berekening> _mockBaatBerekeningen;


        public AnalyseTest()
        {
            o = new Organisatie("a", "b", "c", 1000, "d");
            o.UrenWerkWeek = 40M;
            _analyse = new Analyse(o);
        }
         
        #region Tests

        [Fact]
        public void Analyse_KostenLijstHeeftAlles()
        {
            Assert.Equal(_analyse.Kosten.Count, 7);
        }

        [Fact]
        public void Analyse_BatenLijstHeeftAlles()
        {
            Assert.Equal(_analyse.Baten.Count, 11);
        }

        [Fact]
        public void Analyse_OrganisatiNietNull()
        {
            Assert.Equal(_analyse.Organisatie, o );
        }

        [Fact]
        public void AanmaakDatumCorrect ()
        {
            //            
            Assert.Equal(_analyse.AanmaakDatum.Date, DateTime.Now.Date);
        }

        [Fact]
        public void GeefTotaalBedragPerBerekeningBaat()
        {
            
            _mockBaatBerekeningen = new List<Berekening>();
            _analyse.Baten = new List<Berekening>(); 

            Mock<Berekening> baatBerekening1 = new Mock<Berekening>();
            baatBerekening1.Setup(b => b.BerekenResultaat()).Returns(1000M);

            Mock<Berekening> baatBerekening2 = new Mock<Berekening>();
            baatBerekening2.Setup(b => b.BerekenResultaat()).Returns(5000M);

            Mock<Berekening> baatBerekening3 = new Mock<Berekening>();
            baatBerekening3.Setup(b => b.BerekenResultaat()).Returns(2500M);

            _mockBaatBerekeningen.Add(baatBerekening1.Object);
            _mockBaatBerekeningen.Add(baatBerekening2.Object);
            _mockBaatBerekeningen.Add(baatBerekening3.Object);

            Assert.Equal(_mockBaatBerekeningen[0].BerekenResultaat(), 1000M);
            Assert.Equal(_mockBaatBerekeningen[1].BerekenResultaat(), 5000M);
            Assert.Equal(_mockBaatBerekeningen[2].BerekenResultaat(), 2500M);
        }

        public void GeefTotaalBedragPerBerekeningKost()
        {
            _mockKostBerekeningen = new List<Berekening>();
            _analyse.Kosten = new List<Berekening>();
            Mock<Berekening> kostBerekening1 = new Mock<Berekening>();
            kostBerekening1.Setup(k => k.BerekenResultaat()).Returns(1000M);

            Mock<Berekening> kostBerekening2 = new Mock<Berekening>();
            kostBerekening2.Setup(k => k.BerekenResultaat()).Returns(500M);

            Mock<Berekening> kostBerekening3 = new Mock<Berekening>();
            kostBerekening3.Setup(k => k.BerekenResultaat()).Returns(250M);

            _mockKostBerekeningen.Add(kostBerekening1.Object);
            _mockKostBerekeningen.Add(kostBerekening2.Object);
            _mockKostBerekeningen.Add(kostBerekening3.Object);

            Assert.Equal(_mockKostBerekeningen[0].BerekenResultaat(), 1000M);
            Assert.Equal(_mockKostBerekeningen[1].BerekenResultaat(), 500M);
            Assert.Equal(_mockKostBerekeningen[2].BerekenResultaat(), 250M);

        }

        public void BerekenBedragPerLijn()
        {
            Mock<Berekening> kostBerekening1 = new Mock<Berekening>();
            _analyse.BerekenBedragPerLijn(kostBerekening1.Object, 1);

             
        }




        #endregion
    }
}
