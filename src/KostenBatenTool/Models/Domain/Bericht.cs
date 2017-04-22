using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class Bericht
    {
        public int BerichtId { get; set; }
        public string Onderwerp { get; set; }
        public DateTime AanmaakDatum { get; set; }
        public string Tekst { get; set; }
       
        protected Bericht()
        {
           
        }

        public Bericht(string onderwerp, string tekst)
        {
            Onderwerp = onderwerp;
            Tekst = tekst;
            AanmaakDatum = DateTime.Now;
        }
    }
}
