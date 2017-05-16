using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public interface IBerekeningRepository
    {
        Berekening GetBerekening(int berekeningId);
    }
}
