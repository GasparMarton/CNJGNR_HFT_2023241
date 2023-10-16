using Documentation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.DataAccess.Data
{
    // Model és DbContext osztályok legyeneke publikusak
    // - jelenleg nincs jelentősége, majd akkor lesz, ha osztálykönyvtárba rakjuk… akkor lesz lényeges a láthatóság
    // DbContext osztályok legyenek „partial” osztályok
    // - egy osztály több fájlba feldarabolva
    // - azért hasznos, mert így meg lehet oldani, hogy az osztály egyik felét én, a másik felét a Visual Studio szerkeszti
    /// <summary>
    /// DbContext: Magát az adatbázist reprezentáló osztály. A saját adatbázist a DbContext leszármazott osztálya fogja kezelni.
    /// </summary>
    public partial class DocumentationDbContext : DbContext
    {
        // Minden egyes táblához van egy DbSet<> típusú tulajdonság.
        // - IQueryable<>, azaz LINQ-zható

        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<DocType> DocType { get; set; }
        public virtual DbSet<DocStatus> DocStatus { get; set; }

        // Nulla paraméteres konstruktor mindig kell!
        // Akkor is, ha csinálok nem nulla paraméteres konstruktort!
        public DocumentationDbContext()
        {
            // Arra kérem a keretrendszert, hogy az adatbázist, és a benne lévő összes objektumot hozza létre.
            // Azaz létrehozza a táblákat, és feltölti adattal (ha kell).
            this.Database.EnsureCreated();
        }

        public DocumentationDbContext(DbContextOptions<DocumentationDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        /// <summary>
        /// Feladata a kapcsolat konfigurálása.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) // csak akkor, ha még nem történt meg a konfiguráció
            {
                // Connection string elérése
                // - Solution Explorerben dupla katt a .mdf-re
                // - Server explorerben jobb katt a .mdf-re -> properties -> connection string

                // |DataDirectory|: relatív útvonal
                // integrated security=True: Windowsos bejelentkezéssel
                // MultipleActiveResultSets=True: a lazy load proxyk miatt
                // - Server explorerben jobb katt a .mdf-re -> properties -> Modify connections-ben GUI-val beállítható
                // Utána: Close conneciton!

                optionsBuilder
                    .UseInMemoryDatabase("documentationdb")
                    .UseLazyLoadingProxies(); // lazy load bekapcsolása

                // lazy load bekapcsolása nélkül elszállna kivétellel
                // másik lehetőség: eager loading
                // - Cars.Include(car => car.Brand)

            }
        }

        /// <summary>
        /// Táblák / entitások beállítását végzi.
        /// SQL first megközelítésben akár lehetne üres is, ha SQL first és data annotációs módszerrel dolgozunk.
        /// Esetünkben: Navigation propertyk összekötése, adattal való feltöltés.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* Navigation propertyk összekötése. */

            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasOne(doc => doc.Status) // az autónak van egy Brand navigation property -je
                    .WithMany(DocStatus => DocStatus.) // egy márkához tartozik több autó
                    .HasForeignKey(doc => doc.DocStatusID) // milyen idegen kulcson keresztül kötöttük össze
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // OnDelete()
            // - DeleteBehavior.ClientSetNull: Csak olyan márkára engedi kiadni a törlést, amire nincs hivatkozás. Egyéb esetben, elszállna kivétellel.
            // - DeleteBehavior.Cascade: Ha törölsz egy márkát, akkor automatikusan az összes ilyen márkájú autót is törli.


            /* Adattal való feltöltés.

            Brand bmw = new Brand() { Id = 1, Name = "BMW" };
            Brand citroen = new Brand() { Id = 2, Name = "Citroen" };
            Brand audi = new Brand() { Id = 3, Name = "Audi" };

            Car bmw1 = new Car() { Id = 1, BrandId = bmw.Id, BasePrice = 20000, Model = "BMW 116d" };
            Car bmw2 = new Car() { Id = 2, BrandId = bmw.Id, BasePrice = 30000, Model = "BMW 510" };
            Car citroen1 = new Car() { Id = 3, BrandId = citroen.Id, BasePrice = 10000, Model = "Citroen C1" };
            Car citroen2 = new Car() { Id = 4, BrandId = citroen.Id, BasePrice = 15000, Model = "Citroen C3" };
            Car audi1 = new Car() { Id = 5, BrandId = audi.Id, BasePrice = 20000, Model = "Audi A3" };
            Car audi2 = new Car() { Id = 6, BrandId = audi.Id, BasePrice = 25000, Model = "Audi A4" };
            */


            // Adatok feltöltése a táblába.
            // - vigyázni kell, mert van olyan overloadja, hogy params object[]
            // - fordító nem ellenőrzi, mit adsz hozzá, csak futásidőben kapsz kivételt, ha nem stimmelt valami
            /*
            modelBuilder.Entity<Brand>().HasData(bmw, citroen, audi);
            modelBuilder.Entity<Car>().HasData(bmw1, bmw2, citroen1, citroen2, audi1, audi2);
            */
        }
    }
}
