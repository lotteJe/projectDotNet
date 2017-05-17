using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public interface ILijnRepository
    {
        void VerwijderLijn(Lijn lijn);
        LoonKostLijn GetLoonKostLijn(int lijnId, List<Veld> velden);
        Domain.LoonKostLijn GetLoonKostLijn(int lijnId);
        IList<LoonKostLijn> GetLoonKostLijnen(int berekeningId, List<Veld> velden);
    }
}
