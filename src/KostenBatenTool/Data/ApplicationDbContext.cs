using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KostenBatenTool.Models;
using KostenBatenTool.Models.Domain;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KostenBatenTool.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Organisatie> Organisaties { get; set; }
        public DbSet<Persoon> Personen { get; set; }
        public DbSet<ArbeidsBemiddelaar> ArbeidsBemiddelaars { get; set; }
        public DbSet<Analyse> Analyses { get; set; }
        public DbSet<Bericht> Berichten { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Persoon>(MapPersoon);
            builder.Entity<ArbeidsBemiddelaar>(MapArbeidsBemiddelaar);
            builder.Entity<Analyse>(MapAnalyse);
            builder.Entity<Organisatie>(MapOrganisatie);
            builder.Entity<Berekening>(MapBerekening);
            builder.Entity<LoonkostSubsidie>(MapLoonkostSubsidie);
            builder.Entity<AdministratieBegeleidingsKost>(MapAdministratieBegeleidingsKost);
            builder.Entity<LoonKost>(MapLoonKost);
            builder.Entity<MedewerkerHogerNiveauBesparing>(MapMedewerkerHogerNiveauBesparing);
            builder.Entity<MedewerkerZelfdeNiveauBesparing>(MapMedewerkerZelfdeNiveauBesparing);
            builder.Entity<Veld>(MapVeld);
            builder.Entity<Lijn>(MapLijn);
            builder.Entity<Bericht>(MapBericht);
            builder.Entity<Administrator>(MapAdministrator);
           
        }

        private void MapAdministrator(EntityTypeBuilder<Administrator> a)
        {
            a.Property(ad => ad.Paswoord);
        }

        private void MapBericht(EntityTypeBuilder<Bericht> b)
        {
            b.ToTable("Bericht");
            b.HasKey(br => br.BerichtId);
            b.Property(br => br.BerichtId).ValueGeneratedOnAdd();
            b.Property(br => br.AanmaakDatum);
            b.Property(br => br.Onderwerp);
            b.Property(br => br.Tekst);
        }
        
        private void MapLijn(EntityTypeBuilder<Lijn> b)
        {
            b.ToTable("Lijn");
            b.HasKey(t => t.LijnId);
            b.Property(t => t.LijnId).ValueGeneratedOnAdd();
            b.Ignore(t => t.VeldenDefinitie);
            b.HasMany(t => t.VeldenWaarden).WithOne().OnDelete(DeleteBehavior.Cascade);
        }

        private void MapBerekening(EntityTypeBuilder<Berekening> b)
        {
            b.ToTable("Berekening");
            b.Property(t => t.BerekeningId);
            b.Property(t => t.Resultaat);
            b.HasMany(t => t.Velden).WithOne().OnDelete(DeleteBehavior.Cascade);
            b.HasMany(t => t.Lijnen).WithOne().IsRequired().OnDelete(DeleteBehavior.Cascade);
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

        private void MapVeld(EntityTypeBuilder<Veld> v)
        {
            v.ToTable("Veld");
            v.HasKey(t => t.VeldId);
            v.Property(t => t.VeldId);
            v.Property(t => t.VeldKey);
            v.Ignore(t => t.Value);
            v.Property(t => t.InternalValue);
        }

        private void MapAnalyse(EntityTypeBuilder<Analyse> a)
        {
            a.ToTable("Analyse");
            a.HasKey(t => t.AnalyseId);
            a.Property(t => t.AnalyseId).ValueGeneratedOnAdd();
            a.Property(t => t.AanmaakDatum).IsRequired();
            a.Property(t => t.Resultaat);
            a.HasOne(t => t.Organisatie).WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade); 
            a.HasMany(t => t.Baten).WithOne().OnDelete(DeleteBehavior.Cascade);
            a.HasMany(t => t.Kosten).WithOne().OnDelete(DeleteBehavior.Cascade);
            }

        private void MapArbeidsBemiddelaar(EntityTypeBuilder<ArbeidsBemiddelaar> a)
        {
            a.HasOne(t => t.EigenOrganisatie).WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
            a.HasMany(t => t.Analyses).WithOne().IsRequired(false).OnDelete(DeleteBehavior.Cascade);
        }

        private void MapLoonkostSubsidie(EntityTypeBuilder<LoonkostSubsidie> l)
        {
            l.Ignore(k => k.Loonkost);
        }

        private void MapAdministratieBegeleidingsKost(EntityTypeBuilder<AdministratieBegeleidingsKost> a)
        {
            a.HasOne(t => t.Analyse);
        }

        private void MapLoonKost(EntityTypeBuilder<LoonKost> a)
        {
            a.HasOne(t => t.Analyse);
        }

        private void MapMedewerkerHogerNiveauBesparing(EntityTypeBuilder<MedewerkerHogerNiveauBesparing> a)
        {
            a.HasOne(t => t.Analyse);
        }

        private void MapMedewerkerZelfdeNiveauBesparing(EntityTypeBuilder<MedewerkerZelfdeNiveauBesparing> a)
        {
            a.HasOne(t => t.Analyse);
        }

        private void MapPersoon(EntityTypeBuilder<Persoon> p)
        {
            p.ToTable("Persoon");
            p.HasKey(t => t.PersoonID);
            p.Property(t => t.PersoonID).ValueGeneratedOnAdd();
            p.Property(t => t.Naam).IsRequired();
            p.Property(t => t.Voornaam).IsRequired();
            p.Property(t => t.Email).IsRequired();

            p.HasDiscriminator<String>("Type")
                .HasValue<Contactpersoon>("Contactpersoon")
                .HasValue<Administrator>("Administrator")
                .HasValue<ArbeidsBemiddelaar>("Arbeidsbemiddelaar");
        }
        private static void MapOrganisatie(EntityTypeBuilder<Organisatie> o)
        {
            o.ToTable("Organisatie");
            o.HasKey(t => t.OrganisatieId);
            o.Property(t => t.OrganisatieId).ValueGeneratedOnAdd();
            o.Property(t => t.Naam).IsRequired();
            o.Property(t => t.Straat).IsRequired();
            o.Property(t => t.Huisnummer).IsRequired();
            o.Property(t => t.Postcode).IsRequired();
            o.Property(t => t.Gemeente).IsRequired();
            o.Property(t => t.UrenWerkWeek).IsRequired();
            o.Property(t => t.PatronaleBijdrage).IsRequired();
            o.Property(t => t.Afdeling).IsRequired(false);
            o.HasOne(t => t.Contactpersoon).WithMany().IsRequired(false).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
