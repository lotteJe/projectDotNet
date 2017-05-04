using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class Doelgroep
    {
        public int DoelgroepId { get; set; }
        public string Soort { get; set; }
        public decimal Bedrag1 { get; set; }
        public decimal Bedrag2 { get; set; }

        protected Doelgroep()
        {
            
        }

        public Doelgroep(string soort,decimal bedrag1, decimal bedrag2 )
        {
            Soort = soort;
            Bedrag1 = bedrag1;
            Bedrag2 = bedrag2;
        }
       

    }
}
