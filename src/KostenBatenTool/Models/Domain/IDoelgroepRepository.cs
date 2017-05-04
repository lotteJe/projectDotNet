using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public interface IDoelgroepRepository
    {
        List<Doelgroep> GetAll();
        Doelgroep GetBySoort(string soort);
        Doelgroep GetById(int id);
    }
}
