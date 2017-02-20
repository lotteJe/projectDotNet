using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models
{
    public enum KostBenaming
    {
        Loonkost = 1,
        Voorbereidingskost = 2,
        Werkkledij = 3,
        AanpassingsKost = 4,
        Opleiding = 5,
        AdministratieBegeleiding = 6,
        Andere = 7
    }

    public enum BaatBenaming
    {
        Loonsubsidie = 1,
        Aanpassingssubsidie = 2,
        MedewerkerZelfdeNiveau = 3,
        MedewerkerHogerNiveau = 4,
        Uitzendkrachten = 5,
        OmzetVerlies = 6,
        ProductiviteitsWinst = 7,
        Overuren = 8,
        Outsourcing = 9,
        Logistiek = 10,
        Andere = 11
    }
}
