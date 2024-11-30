using MarketplaceApp.Data.Entities;
using MarketplaceApp.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data
{
    public static class Seed
    {
        public static readonly List<PromoCode> PromoCodes = new List<PromoCode>()
        {
            new PromoCode("POTPUNO10", 10, DateTime.Now.AddMonths(1)),
            new PromoCode("ELEKTRONIKA15", 15, DateTime.Now.AddMonths(2)),
            new PromoCode("KNJIGE20", 20, DateTime.Now.AddMonths(3)),
            new PromoCode("NAMJESTAJ25", 25, DateTime.Now.AddMonths(2))
        };

        public static readonly List<Category> Categories = new List<Category>()
        {
            new Category("Elektronika", new List<Product>(), new List<PromoCode> { PromoCodes[1] }),
            new Category("Odjeca", new List<Product>(), new List<PromoCode> { PromoCodes[0] }),
            new Category("Knjige", new List<Product>(), new List<PromoCode> { PromoCodes[0], PromoCodes[2] }),
            new Category("Hrana i pice", new List<Product>(), new List<PromoCode>()),
            new Category("Namjestaj", new List<Product>(), new List<PromoCode> { PromoCodes[3] })
        };

        public static readonly List<Product> Products = new List<Product>()
        {
            // Produkti za kategoriju Elektronika
            new Product("Laptop", "Visoko kvalitetni laptop sa 16GB RAM-a i 512GB SSD-om.", 700m, ProductStatusEnum.OnSale, Categories[0].Id), // 0
            new Product("Pametni telefon", "Pametni telefon sa 128GB memorije i 6GB RAM-a.", 499.99m, ProductStatusEnum.Sold, Categories[0].Id), // 1
            new Product("Bluetooth slušalice", "Slušalice sa dugim trajanjem baterije i kvalitetnim zvukom.", 89.99m, ProductStatusEnum.OnSale, Categories[0].Id), // 2

            // Produkti za kategoriju Odjeca
            new Product("Traperice za žene", "Moderne traperice s visokim strukom.", 49.99m, ProductStatusEnum.OnSale, Categories[1].Id), // 3
            new Product("Muška majica", "Jednostavna, ali elegantna majica za svakodnevno nošenje.", 19.99m, ProductStatusEnum.Sold, Categories[1].Id), // 4
            new Product("Ženska haljina", "Lagana haljina idealna za ljeto.", 29.99m, ProductStatusEnum.Sold, Categories[1].Id), // 5

            // Produkti za kategoriju Knjige
            new Product("Harry Potter i Kamen mudraca", "Prva knjiga iz serijala o Harryju Potteru, mladom čarobnjaku.", 19.99m, ProductStatusEnum.Sold, Categories[2].Id), // 6
            new Product("1984", "Dystopijski roman Georgea Orwella koji opisuje totalitarni režim.", 14.99m, ProductStatusEnum.OnSale, Categories[2].Id), // 7
            new Product("Ubiti pticu rugalicu", "Klasik Harper Lee koji istražuje rasizam i pravdu u malom američkom gradu.", 12.99m, ProductStatusEnum.OnSale, Categories[2].Id), // 8

            // Produkti za kategoriju Hrana i pice
            new Product("Čokolada", "Tamna čokolada sa 70% kakaa, bogat okus i visoka kvaliteta.", 2.49m, ProductStatusEnum.OnSale, Categories[3].Id), // 9
            new Product("Jabuke", "Svježe i sočne jabuke, odlične za užinu.", 1.99m, ProductStatusEnum.OnSale, Categories[3].Id), // 10
            new Product("Kava", "Svježe mljevena kava s bogatim i aromatičnim okusom.", 4.99m, ProductStatusEnum.Sold, Categories[3].Id), // 11

            // Produkti za kategoriju Namjestaj
            new Product("Kauč", "Udoban trosjed sa modernim dizajnom, pogodan za dnevni boravak.", 299.99m, ProductStatusEnum.Sold, Categories[4].Id), // 12
            new Product("Stol za dnevni boravak", "Elegantni drveni stol za dnevni boravak s prostorom za pohranu.", 199.99m, ProductStatusEnum.OnSale, Categories[4].Id), // 13
            new Product("Komoda", "Komoda s 6 ladica, idealna za pohranu odjeće ili drugih predmeta.", 149.99m, ProductStatusEnum.OnSale, Categories[4].Id) // 14
        };
        
        public static readonly List<User> Users = new List<User>()
        {
            // Kupci
            new Buyer("Ana", "ana@gmail.com", 150.00m, new List<Guid> { Products[1].Id }, new List<Guid> { Products[0].Id, Products[7].Id }), // 0
            new Buyer("Marko", "marko@gmail.com", 200.00m, new List<Guid> { Products[4].Id, Products[5].Id }, new List<Guid> { Products[3].Id, Products[7].Id, Products[13].Id }), // 1
            new Buyer("Ivana", "ivana@gmail.com", 300.00m, new List<Guid> { Products[6].Id, Products[11].Id, Products[12].Id }, new List<Guid> { Products[13].Id, Products[14].Id }), // 2

            // Prodavaci
            new Seller("Ivo", "ivo@gmail.com", new List<Guid> { Products[12].Id, Products[0].Id, Products[4].Id }, 319.98m), // 3
            new Seller("Jelena", "jelena@gmail.com", new List<Guid> { Products[1].Id, Products[3].Id, Products[5].Id, Products[7].Id }, 499.99m + 29.99m), // 4
            new Seller("Mirko", "mirko@gmail.com", new List<Guid> { Products[2].Id, Products[6].Id, Products[8].Id, Products[9].Id }, 19.99m), // 5
            new Seller("Stipe", "stipe@gmail.com", new List<Guid> { Products[10].Id, Products[11].Id, Products[13].Id, Products[14].Id }, 4.99m), // 6
        };

        public static readonly List<Transaction> Transactions = new List<Transaction>()
        {
            // Transakcije 
            new Transaction(Products[1].Id, Users[0].Id, Users[4].Id, DateTime.Parse("08.11.2024"), -Products[1].Price),
            new Transaction(Products[4].Id, Users[1].Id, Users[3].Id, DateTime.Parse("12.11.2024"), -Products[4].Price),
            new Transaction(Products[5].Id, Users[1].Id, Users[4].Id, DateTime.Parse("15.11.2024"), -Products[5].Price),
            new Transaction(Products[6].Id, Users[2].Id, Users[5].Id, DateTime.Parse("20.11.2024"), -Products[6].Price),
            new Transaction(Products[11].Id, Users[2].Id, Users[3].Id, DateTime.Parse("22.11.2024"), -Products[11].Price),
            new Transaction(Products[12].Id, Users[2].Id, Users[6].Id, DateTime.Parse("27.11.2024"), -Products[12].Price)
        };
    }
}
