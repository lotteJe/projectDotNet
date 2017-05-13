﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public interface IArbeidsBemiddelaarRepository
    {

        ArbeidsBemiddelaar GetBy(string emailadres);
        ArbeidsBemiddelaar GetArbeidsBemiddelaarVolledig(string email);
        void Add(ArbeidsBemiddelaar arbeidsBemiddelaar);
        void Delete(ArbeidsBemiddelaar arbeidsBemiddelaar);
        IEnumerable<Analyse> GetAllAnalyses(string email);
        void SaveChanges();
        void SerialiseerVelden(Analyse analyse);
        Analyse GetAnalyse(string email, int id);
        Analyse GetLaatsteAnalyse(string email);
        IEnumerable<Organisatie> GetOrganisaties(string email);
        Organisatie GetOrganisatie(string email, int id);
        void VerwijderAnalyse(Analyse analyse);
        void VerwijderLijn(Lijn lijn);
        LoonKostLijn GetLoonKostLijn(int lijnId, List<Veld> velden);
        IList<LoonKostLijn> GetLoonKostLijnen(int BerekeningId, List<Veld> velden);
        IEnumerable<Analyse> ZoekAnalysesWerkgever(string email, string searchString);
     
        IEnumerable<Analyse> ZoekAnalysesGemeente(string email, string searchString);
    }
}
