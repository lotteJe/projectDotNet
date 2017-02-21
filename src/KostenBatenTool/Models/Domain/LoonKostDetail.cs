using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class LoonKostDetail : DetailKost
    {
        #region Fields
        private Analyse _analyse;  
        #endregion
        #region Properties

        public string Functie { get; set; }
        public double UrenPerWeek { get; set; }
        public decimal Maandloon { get; set; }
        public Doelgroep Doelgroep { get; set; }
        public decimal Ondersteuningspremie { get; set; }
        public decimal MaandloonPatronaal { get; set; }
        public decimal GemiddeldeVop { get; set; }
        public decimal Doelgroepvermindering { get; set; } = 0;
        public double Ibo { get; set; }
        public decimal PremieIbo { get; set; }
        public decimal FunctieKost { get; set; }
       

        #endregion

        #region Constructors

        public LoonKostDetail(Analyse analyse)
        {
            _analyse = analyse;
        }
        

        #endregion
        #region Methods
        public void BerekenMaandLoonPatronaal()
        {
            decimal patronaleBijdrage = (decimal) _analyse.Organisatie.PatronaleBijdrage;
            decimal urenWerkWeek = (decimal) _analyse.Organisatie.UrenWerkWeek;
            if (_analyse.Organisatie.UrenWerkWeek != 0)
            MaandloonPatronaal = ((Maandloon/(decimal)urenWerkWeek)*(decimal)UrenPerWeek) * (decimal) (1 +patronaleBijdrage);
        }

        public void BerekenGemiddeldeVop()
        {
            GemiddeldeVop = (MaandloonPatronaal - Doelgroepvermindering)*Ondersteuningspremie;
        }

        public void BerekenDoelgroepVermindering()
        {
            decimal urenWerkWeek = (decimal)_analyse.Organisatie.UrenWerkWeek;
            switch (Doelgroep)
            {
                case Doelgroep.Laaggeschoold:
                    if (Maandloon < 2500)
                        Doelgroepvermindering = (1550/ urenWerkWeek)*(decimal)UrenPerWeek/4;
                    break;
                case Doelgroep.Middengeschoold:
                    if (Maandloon < 2500)
                        Doelgroepvermindering = (1000 / urenWerkWeek) * (decimal)UrenPerWeek / 4;
                    break;
                case Doelgroep.Tussen55En60:
                    if (Maandloon < 4466.66M)
                        Doelgroepvermindering = (1150 / urenWerkWeek) * (decimal)UrenPerWeek / 4;
                    break;
                case Doelgroep.Boven60:
                    if (Maandloon < 4466.66M)
                        Doelgroepvermindering = (1500 / urenWerkWeek) * (decimal)UrenPerWeek / 4;
                    break;

            }
        } 
        #endregion

         
    }
}
