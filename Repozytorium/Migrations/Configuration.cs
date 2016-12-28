namespace Repozytorium.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Repozytorium.Models.OglContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Repozytorium.Models.OglContext context)
        {
            //  This method will be called after migrating to the latest version.
            //Debugowanie metody seed

            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();

            SeedRoles(context);
            SeedUsers(context);
            SeedOgloszenia(context);
            SeedKategorie(context);
            SeedOgloszenieKategoria(context);
        }

        private void SeedRoles(OglContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());

            if(!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
        }

        private void SeedUsers(OglContext context)
        {
            var manager = new UserManager<Uzytkownik>(new UserStore<Uzytkownik>(context));
            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var passwordHash = new PasswordHasher();
                string password = passwordHash.HashPassword("admin");
                var user = new Uzytkownik
                {
                    UserName = "admin",
                    Wiek = 12,
                    PasswordHash = password
                };
                var adminResult = manager.Create(user);

                if (adminResult.Succeeded)
                    manager.AddToRole(user.Id, "Admin");
            }
        }

        private void SeedOgloszenia(OglContext context)
        {
            var idUzytkownika = context.Set<Uzytkownik>().Where(u => u.UserName == "admin").FirstOrDefault().Id;
            for(int i=1; i<=10; i++)
            {
                var ogl = new Ogloszenie()
                {
                    Id = i,
                    UzytkownikId = idUzytkownika,
                    Tresc = "Treœæ og³oszenia: " + i.ToString(),
                    Tytul = "Tytu³ og³oszenia: " + i.ToString(),
                    DataDodania = DateTime.Now.AddDays(-i)
                };
                context.Set<Ogloszenie>().AddOrUpdate(ogl);
            }
            context.SaveChanges();
        }

        private void SeedKategorie(OglContext context)
        {
            for(int i = 1; i<=10; i++)
            {
                var kat = new Kategoria()
                {
                    Id = i,
                    Nazwa = "Nazwa kategorii: " + i.ToString(),
                    Tresc = "Treœæ og³oszenia: " + i.ToString(),
                    MetaTytul = "Tytu³ kategorii: " + i.ToString(),
                    MetaOpis = "Opis kategorii: " + i.ToString(),
                    MetaSlowa = "S³owa kluczowe do kategorii: " + i.ToString(),
                    ParentId = i
                };
                context.Set<Kategoria>().AddOrUpdate(kat);
            }
            context.SaveChanges();
        }

        private void SeedOgloszenieKategoria(OglContext context)
        {
            for(int i = 1; i<10; i++)
            {
                var okat = new OgloszenieKategoria()
                {
                    ID = i,
                    OgloszenieID = i / 2 + 1,
                    KategoriaID = i / 2 + 2
                };
                context.Set<OgloszenieKategoria>().AddOrUpdate(okat);
            }
            context.SaveChanges();
        }
    }
}
