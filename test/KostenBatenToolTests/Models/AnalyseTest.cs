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



        public AnalyseTest()
        {
            o = new Organisatie("a", "b", "c", "1000", "d");
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
        public void VulVeldInKost()
        {
            _analyse.VulVeldIn("AndereKost", 0, "bedrag",100M);
            Assert.Equal(_analyse.Kosten.First(k => k is AndereKost).Lijnen[0].First(l => l.Key == "bedrag").Value, 100M);
        }
        [Fact]
        public void BerekenNettoResultaat()
        {
            //Mock<Analyse> analyse = new Mock<Analyse>();
            //analyse.Setup(a => a.BerekenBatenResultaat()).Returns(9999M);
            //analyse.Setup(a => a.BerekenKostenResultaat()).Returns(5000M);

            //Assert.Equal(analyse.Object.BerekenNettoResultaat(), 4999M);

            //analyse.Verify(a => a.BerekenBatenResultaat(), Times.Once);
            //analyse.Verify(a => a.BerekenKostenResultaat(), Times.Once);
        }

        [Fact]
        public void BerekenKostenResultaat()
        {
            
            _analyse.Kosten = new List<Berekening>();
            Mock<Berekening> kostBerekening1 = new Mock<Berekening>();
            kostBerekening1.Setup(k => k.BerekenResultaat()).Returns(1000M);

            Mock<Berekening> kostBerekening2 = new Mock<Berekening>();
            kostBerekening2.Setup(k => k.BerekenResultaat()).Returns(500M);

            Mock<Berekening> kostBerekening3 = new Mock<Berekening>();
            kostBerekening3.Setup(k => k.BerekenResultaat()).Returns(250M);

            _analyse.Kosten.Add(kostBerekening1.Object);
            _analyse.Kosten.Add(kostBerekening2.Object);
            _analyse.Kosten.Add(kostBerekening3.Object);

            Assert.Equal(_analyse.BerekenKostenResultaat(), 1750M);
            kostBerekening1.Verify(a => a.BerekenResultaat(), Times.Once);
            kostBerekening2.Verify(a => a.BerekenResultaat(), Times.Once);
            kostBerekening3.Verify(a => a.BerekenResultaat(), Times.Once);
        }

        [Fact]
        public void BerekenBatenResultaat()
        {
            
            _analyse.Baten = new List<Berekening>();
            
            Mock<Berekening> baatBerekening1 = new Mock<Berekening>();
            baatBerekening1.Setup(b => b.BerekenResultaat()).Returns(1000M);

            Mock<Berekening> baatBerekening2 = new Mock<Berekening>();
            baatBerekening2.Setup(b => b.BerekenResultaat()).Returns(5000M);

            Mock<Berekening> baatBerekening3 = new Mock<Berekening>();
            baatBerekening3.Setup(b => b.BerekenResultaat()).Returns(2500M);

            _analyse.Baten.Add(baatBerekening1.Object);
            _analyse.Baten.Add(baatBerekening2.Object);
            _analyse.Baten.Add(baatBerekening3.Object);

            Assert.Equal(_analyse.BerekenBatenResultaat(), 8500M);
            baatBerekening1.Verify(a => a.BerekenResultaat(), Times.Once);
            baatBerekening2.Verify(a => a.BerekenResultaat(), Times.Once);
            baatBerekening3.Verify(a => a.BerekenResultaat(), Times.Once);


        }




        #endregion
    }
}
