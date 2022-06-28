using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Projekt_PO.DB
{
    public partial class Obsluga_magazynow_DBContext : DbContext
    {
        public Obsluga_magazynow_DBContext()
        {
        }

        public Obsluga_magazynow_DBContext(DbContextOptions<Obsluga_magazynow_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Adresy> Adresies { get; set; }
        public virtual DbSet<Faktury> Fakturies { get; set; }
        public virtual DbSet<Hurtownie> Hurtownies { get; set; }
        public virtual DbSet<Magazyny> Magazynies { get; set; }
        public virtual DbSet<Pakiety> Pakieties { get; set; }
        public virtual DbSet<Pracownicy> Pracownicies { get; set; }
        public virtual DbSet<Sektory> Sektories { get; set; }
        public virtual DbSet<SektoryMagazynow> SektoryMagazynows { get; set; }
        public virtual DbSet<WartośćFakturMiesiące> WartośćFakturMiesiąces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               optionsBuilder.UseSqlServer("Server=.; Database=Obsluga_magazynow_DB; trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            modelBuilder.Entity<Adresy>(entity =>
            {
                entity.HasKey(e => e.IdAdresu)
                    .HasName("PK__adresy__4826D6147BFB110D");

                entity.ToTable("adresy");

                entity.Property(e => e.IdAdresu).HasColumnName("id_adresu");

                entity.Property(e => e.KodPocztowy)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("kod_pocztowy")
                    .IsFixedLength(true);

                entity.Property(e => e.Miejscowosc)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("miejscowosc");

                entity.Property(e => e.NrDomu)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("nr_domu");

                entity.Property(e => e.NrMieszkania)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("nr_mieszkania");

                entity.Property(e => e.Ulica)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ulica");
            });

            modelBuilder.Entity<Faktury>(entity =>
            {
                entity.HasKey(e => e.IdFaktury)
                    .HasName("PK__faktury__FCBA8FCAF1BB4194");

                entity.ToTable("faktury");

                entity.HasIndex(e => e.NumerFaktury, "UQ__faktury__C4266320E9575943")
                    .IsUnique();

                entity.Property(e => e.IdFaktury).HasColumnName("id_faktury");

                entity.Property(e => e.DataWystawienia)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("data_wystawienia")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.HurtowniaId).HasColumnName("hurtownia_id");

                entity.Property(e => e.NumerFaktury)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("numer_faktury");

                entity.Property(e => e.Opis)
                    .HasMaxLength(50)
                    .HasColumnName("opis");

                entity.Property(e => e.Wartosc).HasColumnName("wartosc");

                entity.Property(e => e.WystawiajacyId).HasColumnName("wystawiajacy_id");

                entity.HasOne(d => d.Hurtownia)
                    .WithMany(p => p.Fakturies)
                    .HasForeignKey(d => d.HurtowniaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_hurtownia_id");

                entity.HasOne(d => d.Wystawiajacy)
                    .WithMany(p => p.Fakturies)
                    .HasForeignKey(d => d.WystawiajacyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_pracownik_id");
            });

            modelBuilder.Entity<Hurtownie>(entity =>
            {
                entity.HasKey(e => e.IdHurtowni)
                    .HasName("PK__hurtowni__737841030FF68947");

                entity.ToTable("hurtownie");

                entity.Property(e => e.IdHurtowni).HasColumnName("id_hurtowni");

                entity.Property(e => e.Nazwa)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("nazwa");

                entity.Property(e => e.Nip)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("nip")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Magazyny>(entity =>
            {
                entity.HasKey(e => e.IdMagazynu)
                    .HasName("PK__magazyny__107A3BF88956D2F3");

                entity.ToTable("magazyny");

                entity.HasIndex(e => e.Adres, "UQ__magazyny__C012CCA4C4A4A7EF")
                    .IsUnique();

                entity.Property(e => e.IdMagazynu).HasColumnName("id_magazynu");

                entity.Property(e => e.Adres)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("adres");

                entity.Property(e => e.CzyAktywny).HasColumnName("czy_aktywny");

                entity.Property(e => e.IloscPracownikow).HasColumnName("ilosc_pracownikow");

                entity.Property(e => e.IloscSektorow).HasColumnName("ilosc_sektorow");

                entity.Property(e => e.Opis)
                    .HasMaxLength(100)
                    .HasColumnName("opis");
            });

            modelBuilder.Entity<Pakiety>(entity =>
            {
                entity.HasKey(e => e.IdPakietu)
                    .HasName("PK__pakiety__64CE9939F70337EB");

                entity.ToTable("pakiety");

                entity.HasIndex(e => e.Kod, "UQ__pakiety__DFD8EB9E4A3DC375")
                    .IsUnique();

                entity.Property(e => e.IdPakietu).HasColumnName("id_pakietu");

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("kod");

                entity.Property(e => e.MagazynId).HasColumnName("magazyn_id");

                entity.Property(e => e.SektorId).HasColumnName("sektor_id");

                entity.HasOne(d => d.SektoryMagazynow)
                    .WithMany(p => p.Pakieties)
                    .HasForeignKey(d => new { d.MagazynId, d.SektorId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_magazyn_sektor_pakietu");
            });

            modelBuilder.Entity<Pracownicy>(entity =>
            {
                entity.HasKey(e => e.IdPracownika)
                    .HasName("PK__pracowni__5D8E25F264278897");

                entity.ToTable("pracownicy");

                entity.HasIndex(e => e.Email, "UQ__pracowni__AB6E616429DECA4E")
                    .IsUnique();

                entity.HasIndex(e => e.NrTel, "UQ__pracowni__D038EEBEAACCB14E")
                    .IsUnique();

                entity.HasIndex(e => e.Pesel, "UQ__pracowni__DC3B1BB8FD81CD32")
                    .IsUnique();

                entity.HasIndex(e => e.AdresId, "UQ__pracowni__F56ACDEAF2723A31")
                    .IsUnique();

                entity.Property(e => e.IdPracownika).HasColumnName("id_pracownika");

                entity.Property(e => e.AdresId).HasColumnName("adres_id");

                entity.Property(e => e.DataUrodzenia)
                    .HasColumnType("date")
                    .HasColumnName("data_urodzenia");

                entity.Property(e => e.DataZatrudnienia)
                    .HasColumnType("date")
                    .HasColumnName("data_zatrudnienia")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("email");

                entity.Property(e => e.Imie)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("imie");

                entity.Property(e => e.MagazynId).HasColumnName("magazyn_id");

                entity.Property(e => e.Nazwisko)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("nazwisko");

                entity.Property(e => e.NrTel)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("nr_tel")
                    .IsFixedLength(true);

                entity.Property(e => e.Pesel)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("pesel")
                    .IsFixedLength(true);

                entity.Property(e => e.Plec)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("plec")
                    .IsFixedLength(true);

                entity.Property(e => e.Stanowisko)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("stanowisko");

                entity.HasOne(d => d.Adres)
                    .WithOne(p => p.Pracownicy)
                    .HasForeignKey<Pracownicy>(d => d.AdresId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_adres_pracownika");

                entity.HasOne(d => d.Magazyn)
                    .WithMany(p => p.Pracownicies)
                    .HasForeignKey(d => d.MagazynId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_magazyn_pracownika");
            });

            modelBuilder.Entity<Sektory>(entity =>
            {
                entity.HasKey(e => e.IdSektoru)
                    .HasName("PK__sektory__5FD5DBCE91301999");

                entity.ToTable("sektory");

                entity.HasIndex(e => e.Oznaczenie, "UQ__sektory__0E9AD17F8CCF591A")
                    .IsUnique();

                entity.Property(e => e.IdSektoru).HasColumnName("id_sektoru");

                entity.Property(e => e.Limit).HasColumnName("limit");

                entity.Property(e => e.Opis)
                    .HasMaxLength(50)
                    .HasColumnName("opis");

                entity.Property(e => e.Oznaczenie)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("oznaczenie")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<SektoryMagazynow>(entity =>
            {
                entity.HasKey(e => new { e.MagazynId, e.SektorId })
                    .HasName("pk");

                entity.ToTable("sektory_magazynow");

                entity.Property(e => e.MagazynId).HasColumnName("magazyn_id");

                entity.Property(e => e.SektorId).HasColumnName("sektor_id");

                entity.HasOne(d => d.Magazyn)
                    .WithMany(p => p.SektoryMagazynows)
                    .HasForeignKey(d => d.MagazynId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_magazyn_id");

                entity.HasOne(d => d.Sektor)
                    .WithMany(p => p.SektoryMagazynows)
                    .HasForeignKey(d => d.SektorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sektor_id");
            });

            modelBuilder.Entity<WartośćFakturMiesiące>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Wartość_faktur_miesiące");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
