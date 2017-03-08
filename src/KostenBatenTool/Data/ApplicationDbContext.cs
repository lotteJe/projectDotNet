using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KostenBatenTool.Models;
using KostenBatenTool.Models.Domain;
using KostenBatenToolTests.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KostenBatenTool.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Organisatie> Organisaties { get; set; }
        public DbSet<Persoon> Personen { get; set; }
        public DbSet<Berekening> Berekeningen { get; set; }
        public DbSet<Analyse> Analyses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Organisatie>(MapOrganisatie);
            builder.Entity<Afdeling>(MapAfdeling);
            builder.Entity<Persoon>(MapPersoon);
            builder.Entity<ArbeidsBemiddelaar>(MapArbeidsBemiddelaar);
            builder.Entity<Analyse>(MapAnalyse);
            builder.Entity<Berekening>(MapBerekening);
           }

        private void MapBerekening(EntityTypeBuilder<Berekening> b)
        {
            b.ToTable("Berekening");
            b.Property(t => t.BerekeningId).ValueGeneratedOnAdd();
            b.Ignore(t => t.Velden);
            b.Ignore(t => t.Lijnen);
            b.HasDiscriminator<String>("Type").HasValue<AanpassingsKost>("AanpassingsKost")
                .HasValue<AanpassingsSubsidie>("AanpassingsSubsidie")
                .HasValue<AdministratieBegeleidingsKost>("AdministratieBegeleidingsKost")
                .HasValue<AndereBesparing>("AndereBesparing")
                .HasValue<AndereKost>("AndereKost")
                .HasValue<LogistiekeBesparing>("LogistiekeBesparing")
                .HasValue<LoonKost>("LoonKost")
                .HasValue<LoonkostSubsidie>("LoonkostSubsidie")
                .HasValue<MedewerkerZelfdeNiveauBesparing>("MedewerkerZelfdeNiveauBesparing")
                .HasValue<MedewerkerHogerNiveauBesparing>("MedewerkerHogerNiveauBesparing")
                .HasValue<OmzetverliesBesparing>("OmzetverliesBesparing")
                .HasValue<OpleidingsKost>("OpleidingsKost")
                .HasValue<OutsourcingBesparing>("OutsourcingBesparing")
                .HasValue<OverurenBesparing>("OverurenBesparing")
                .HasValue<ProductiviteitsWinst>("ProductiviteitsWinst")
                .HasValue<UitzendkrachtenBesparing>("UitzendkrachtenBesparing")
                .HasValue<VoorbereidingsKost>("VoorbereidingsKost")
                .HasValue<WerkkledijKost>("WerkkledijKost");
        }

        private void MapAnalyse(EntityTypeBuilder<Analyse> a)
        {
            a.ToTable("Analyse");
            a.Property(t => t.AnalyseId).ValueGeneratedOnAdd();

            a.Property(t => t.AanmaakDatum).IsRequired();

            a.HasOne(t => t.Organisatie).WithMany().IsRequired().OnDelete(DeleteBehavior.Restrict);
            a.HasMany(t => t.Baten).WithOne().IsRequired().OnDelete(DeleteBehavior.Cascade);
            a.HasMany(t => t.Kosten).WithOne().IsRequired().OnDelete(DeleteBehavior.Cascade);
            }

        private void MapArbeidsBemiddelaar(EntityTypeBuilder<ArbeidsBemiddelaar> a)
        {
            a.HasOne(t => t.EigenOrganisatie).WithMany().IsRequired().OnDelete(DeleteBehavior.Restrict);
            a.HasMany(t => t.Analyses).WithOne().IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        private void MapPersoon(EntityTypeBuilder<Persoon> p)
        {
            p.ToTable("Personen");
            p.Property(t => t.PersoonID).ValueGeneratedOnAdd();

            p.Property(t => t.Naam).IsRequired();
            p.Property(t => t.Voornaam).IsRequired();
            p.Property(t => t.Email).IsRequired();

            p.HasDiscriminator<String>("Type")
                .HasValue<Contactpersoon>("Contactpersoon")
                .HasValue<Administrator>("Administrator")
                .HasValue<ArbeidsBemiddelaar>("Arbeidsbemiddelaar");
        }
        
        private void MapAfdeling(EntityTypeBuilder<Afdeling> a)
        {
            a.ToTable("Afdeling");
            a.Property(t => t.AfdelingId).ValueGeneratedOnAdd();
            a.HasKey(t => new { t.AfdelingId, t.OrganisatieId });

            a.Property(t => t.Naam).IsRequired();
        }

        private static void MapOrganisatie(EntityTypeBuilder<Organisatie> o)
        {
            o.ToTable("Organisatie");
            o.Property(t => t.OrganisatieId).ValueGeneratedOnAdd();

            o.Property(t => t.Naam).IsRequired();
            o.Property(t => t.Straat).IsRequired();
            o.Property(t => t.Huisnummer).IsRequired();
            o.Property(t => t.Postcode).IsRequired();
            o.Property(t => t.Gemeente).IsRequired();
            o.Property(t => t.UrenWerkWeek).IsRequired();
            o.Property(t => t.PatronaleBijdrage).IsRequired();

            o.HasMany(t => t.Afdelingen).WithOne().IsRequired(false).OnDelete(DeleteBehavior.Cascade);
            o.HasOne(t => t.Contactpersoon).WithMany().IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
