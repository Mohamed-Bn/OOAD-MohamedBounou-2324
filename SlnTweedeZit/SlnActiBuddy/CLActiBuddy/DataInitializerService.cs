using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CLActiBuddy
{
    public class DataInitializerService
    {
        private static string connString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        // lijst van personen en activiteiten om te vullen in db
        private static List<Persoon> personen = new List<Persoon>
            {
                new Persoon { Voornaam = "Rogier", Achternaam = "van der Linde", Login = "rogier", Paswoord = "1b4f0e9851971998e732078544c96b36c3d01cedf7caa332359d6f1d83567014", Profielfoto = LoadImage("Assets/Profielfotos/m1.jpg"), RegDatum = new DateTime(2020, 12, 23), IsAdmin = true },
                new Persoon { Voornaam = "Frank", Achternaam = "Salliau", Login = "frank", Paswoord = "60303ae22b998861bce3b28f33eec1be758a213c86c93c076dbe9f558c11c752", Profielfoto = LoadImage("Assets/Profielfotos/m2.jpg"), RegDatum = new DateTime(2021, 3, 3), IsAdmin = true },
                new Persoon { Voornaam = "Abdelkarim", Achternaam = "Chouikh", Login = "abdel", Paswoord = "fd61a03af4f77d870fc21e05e7e80678095c92d808cfb3b5c279ee04c74aca13", Profielfoto = LoadImage("Assets/Profielfotos/m3.jpg"), RegDatum = new DateTime(2023, 11, 11), IsAdmin = false },
                new Persoon { Voornaam = "Alexandre", Achternaam = "Kamnerdsiri", Login = "alex", Paswoord = "a4e624d686e03ed2767c0abd85c14426b0b1157d2ce81d27bb4fe4f6f01d688a", Profielfoto = LoadImage("Assets/Profielfotos/m4.jpg"), RegDatum = new DateTime(2023, 6, 17), IsAdmin = false },
                new Persoon { Voornaam = "Ayoub", Achternaam = "Aymen", Login = "ayoub", Paswoord = "a140c0c1eda2def2b830363ba362aa4d7d255c262960544821f556e16661b6ff", Profielfoto = LoadImage("Assets/Profielfotos/m5.jpg"), RegDatum = new DateTime(2022, 12, 12), IsAdmin = false },
                new Persoon { Voornaam = "Brian", Achternaam = "Sutanto", Login = "brian", Paswoord = "ed0cb90bdfa4f93981a7d03cff99213a86aa96a6cbcf89ec5e8889871f088727", Profielfoto = LoadImage("Assets/Profielfotos/m6.jpg"), RegDatum = new DateTime(2023, 9, 17), IsAdmin = false },
                new Persoon { Voornaam = "Marcel", Achternaam = "De Schrijver", Login = "marcel", Paswoord = "bd7c911264aae15b66d4291b6850829aa96986b1d3ead34d1fdbfef27056c112", Profielfoto = LoadImage("Assets/Profielfotos/m7.jpg"), RegDatum = new DateTime(2023, 9, 17), IsAdmin = false },
                new Persoon { Voornaam = "Miranda", Achternaam = "Sousa", Login = "mira", Paswoord = "1f9bfeb15fee8a10c4d0711c7eb0c083962123e1918e461b6a508e7146c189b2", Profielfoto = LoadImage("Assets/Profielfotos/f1.jpg"), RegDatum = new DateTime(2024, 1, 11), IsAdmin = false },
                new Persoon { Voornaam = "Nelly", Achternaam = "Lihmedi", Login = "nelly", Paswoord = "b4451034d3b6590060ce9484a28b88dd332a80a22ae8e39c9c5cb7357ab26c9f", Profielfoto = LoadImage("Assets/Profielfotos/f2.jpg"), RegDatum = new DateTime(2023, 10, 9), IsAdmin = false },
                new Persoon { Voornaam = "Sandra", Achternaam = "Lauer", Login = "sandra", Paswoord = "ec2738feb2bbb0bc783eb4667903391416372ba6ed8b8dddbebbdb37e5102473", Profielfoto = LoadImage("Assets/Profielfotos/f3.jpg"), RegDatum = new DateTime(2024, 2, 13), IsAdmin = false },
                new Persoon { Voornaam = "Soo", Achternaam = "Wang", Login = "soo", Paswoord = "744ea9ec6fa0a83e9764b4e323d5be6b55a5accfc7fe4c08eab6a8de1fca4855", Profielfoto = LoadImage("Assets/Profielfotos/f4.jpg"), RegDatum = new DateTime(2023, 4, 21), IsAdmin = false },
                new Persoon { Voornaam = "Sophie", Achternaam = "Laethem", Login = "sophie", Paswoord = "a98ec5c5044800c88e862f007b98d89815fc40ca155d6ce7909530d792e909ce", Profielfoto = LoadImage("Assets/Profielfotos/f5.jpg"), RegDatum = new DateTime(2023, 8, 8), IsAdmin = false }
            };
        private static List<Activiteit> activiteiten = new List<Activiteit>
            {
                // Sportactiviteiten
                new SportActiviteit
                {
                    Titel = "Zomerse Hardloopwedstrijd",
                    Beschrijving = "Een hardloopwedstrijd van 10 km door het park.",
                    DatumTijd = new DateTime(2024, 8, 1, 9, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sports.png"),
                    Longitude = 4.3517m,
                    Latitude = 50.8467m,
                    MaxPersonen = 100,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 18,
                    Moeilijkheid = ActiviteitMoeilijkheid.Gemiddeld
                },
                new SportActiviteit
                {
                    Titel = "Fietsrit in Brussel",
                    Beschrijving = "Fietsrit van 20 km langs de mooiste plekken in Brussel.",
                    DatumTijd = new DateTime(2024, 8, 5, 10, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sports.png"),
                    Longitude = 4.3488m,
                    Latitude = 50.8467m,
                    MaxPersonen = 50,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 15,
                    Moeilijkheid = ActiviteitMoeilijkheid.Makkelijk
                },
                new SportActiviteit
                {
                    Titel = "Vriendschappelijke Voetbalwedstrijd",
                    Beschrijving = "Een voetbalwedstrijd tussen lokale teams.",
                    DatumTijd = new DateTime(2024, 8, 8, 17, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sports.png"),
                    Longitude = 4.3556m,
                    Latitude = 50.8494m,
                    MaxPersonen = 200,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 12,
                    Moeilijkheid = ActiviteitMoeilijkheid.Gemiddeld
                },
                new SportActiviteit
                {
                    Titel = "Yoga in het Park",
                    Beschrijving = "Gratis yoga sessie in het stadspark.",
                    DatumTijd = new DateTime(2024, 8, 12, 8, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sports.png"),
                    Longitude = 4.3489m,
                    Latitude = 50.8460m,
                    MaxPersonen = 30,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 18,
                    Moeilijkheid = ActiviteitMoeilijkheid.Makkelijk
                },
                new SportActiviteit
                {
                    Titel = "Basketbaltoernooi",
                    Beschrijving = "Toernooi voor basketbalteams van verschillende niveaus.",
                    DatumTijd = new DateTime(2024, 8, 15, 14, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sports.png"),
                    Longitude = 4.3484m,
                    Latitude = 50.8461m,
                    MaxPersonen = 50,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 16,
                    Moeilijkheid = ActiviteitMoeilijkheid.Zwaar
                },

                // Cultuuractiviteiten
                new CultuurActiviteit
                {
                    Titel = "Brusselse Muziekavond",
                    Beschrijving = "Een avond met lokale muzikanten in de stadsschouwburg.",
                    DatumTijd = new DateTime(2024, 8, 2, 19, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3499m,
                    Latitude = 50.8466m,
                    MaxPersonen = 150,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 18,
                    Sector = ActiviteitSector.Muziek
                },
                new CultuurActiviteit
                {
                    Titel = "Theaterfestival",
                    Beschrijving = "Een festival met verschillende toneelstukken in de open lucht.",
                    DatumTijd = new DateTime(2024, 8, 6, 20, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3480m,
                    Latitude = 50.8440m,
                    MaxPersonen = 200,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 12,
                    Sector = ActiviteitSector.Theater
                },
                new CultuurActiviteit
                {
                    Titel = "Dansvoorstelling",
                    Beschrijving = "Professionele dansgroepen tonen hun beste nummers.",
                    DatumTijd = new DateTime(2024, 8, 10, 18, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3556m,
                    Latitude = 50.8494m,
                    MaxPersonen = 100,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 16,
                    Sector = ActiviteitSector.Dans
                },
                new CultuurActiviteit
                {
                    Titel = "Kunstexpositie",
                    Beschrijving = "Expositie van lokale kunstenaars in het stadscentrum.",
                    DatumTijd = new DateTime(2024, 8, 13, 10, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3550m,
                    Latitude = 50.8470m,
                    MaxPersonen = 80,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 12,
                    Sector = ActiviteitSector.Andere
                },
                new CultuurActiviteit
                {
                    Titel = "Filmavond in het Park",
                    Beschrijving = "Buitenfilmvertoning met een klassieker in het stadspark.",
                    DatumTijd = new DateTime(2024, 8, 18, 21, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3545m,
                    Latitude = 50.8480m,
                    MaxPersonen = 300,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 0,
                    Sector = ActiviteitSector.Andere
                },

                // Hobbyactiviteiten
                new HobbyActiviteit
                {
                    Titel = "Schilderworkshop",
                    Beschrijving = "Workshop voor beginners en gevorderden.",
                    DatumTijd = new DateTime(2024, 8, 3, 14, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3491m,
                    Latitude = 50.8468m,
                    MaxPersonen = 25,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 15,
                    Niveau = ActiviteitNiveau.Beginner
                },
                new HobbyActiviteit
                {
                    Titel = "Fotografie Cursus",
                    Beschrijving = "Leer de basisprincipes van fotografie.",
                    DatumTijd = new DateTime(2024, 8, 7, 11, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3502m,
                    Latitude = 50.8471m,
                    MaxPersonen = 20,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 18,
                    Niveau = ActiviteitNiveau.Halfgevorderd
                },
                new HobbyActiviteit
                {
                    Titel = "Tuinaanleg Workshop",
                    Beschrijving = "Leer de basis van tuinonderhoud en -ontwerp.",
                    DatumTijd = new DateTime(2024, 8, 9, 10, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3512m,
                    Latitude = 50.8462m,
                    MaxPersonen = 15,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 20,
                    Niveau = ActiviteitNiveau.Gevorderd
                },
                new HobbyActiviteit
                {
                    Titel = "Creatieve Schrijfavond",
                    Beschrijving = "Workshop voor creatieve schrijvers.",
                    DatumTijd = new DateTime(2024, 8, 11, 18, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3522m,
                    Latitude = 50.8463m,
                    MaxPersonen = 30,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 18,
                    Niveau = ActiviteitNiveau.Halfgevorderd
                },
                new HobbyActiviteit
                {
                    Titel = "Kookcursus",
                    Beschrijving = "Leer de kunst van het koken met een chef-kok.",
                    DatumTijd = new DateTime(2024, 8, 20, 16, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3532m,
                    Latitude = 50.8474m,
                    MaxPersonen = 12,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 21,
                    Niveau = ActiviteitNiveau.Beginner
                },
                new HobbyActiviteit
                {
                    Titel = "Breien voor Beginners",
                    Beschrijving = "Leer breien met eenvoudige patronen.",
                    DatumTijd = new DateTime(2024, 8, 22, 13, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3504m,
                    Latitude = 50.8465m,
                    MaxPersonen = 10,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 15,
                    Niveau = ActiviteitNiveau.Beginner
                },
                new HobbyActiviteit
                {
                    Titel = "Handgemaakte Kaarten",
                    Beschrijving = "Workshop voor het maken van handgemaakte kaarten.",
                    DatumTijd = new DateTime(2024, 8, 25, 11, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3516m,
                    Latitude = 50.8476m,
                    MaxPersonen = 15,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 16,
                    Niveau = ActiviteitNiveau.Halfgevorderd
                },
                new HobbyActiviteit
                {
                    Titel = "Muziekproductie Workshop",
                    Beschrijving = "Leer muziek maken met digitale tools.",
                    DatumTijd = new DateTime(2024, 8, 28, 15, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3520m,
                    Latitude = 50.8469m,
                    MaxPersonen = 20,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 18,
                    Niveau = ActiviteitNiveau.Gevorderd
                },
                new HobbyActiviteit
                {
                    Titel = "Origami Workshop",
                    Beschrijving = "Leer de kunst van origami met diverse ontwerpen.",
                    DatumTijd = new DateTime(2024, 8, 30, 14, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3530m,
                    Latitude = 50.8478m,
                    MaxPersonen = 20,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 12,
                    Niveau = ActiviteitNiveau.Beginner
                },
                new HobbyActiviteit
                {
                    Titel = "Creatief Schrijven",
                    Beschrijving = "Workshop voor het verbeteren van je schrijfvaardigheden.",
                    DatumTijd = new DateTime(2024, 8, 31, 16, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3545m,
                    Latitude = 50.8480m,
                    MaxPersonen = 25,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 18,
                    Niveau = ActiviteitNiveau.Halfgevorderd
                },

                // Meer Sportactiviteiten
                new SportActiviteit
                {
                    Titel = "Zwemwedstrijd",
                    Beschrijving = "Competitie voor verschillende zwemstijlen.",
                    DatumTijd = new DateTime(2024, 8, 4, 11, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sports.png"),
                    Longitude = 4.3495m,
                    Latitude = 50.8473m,
                    MaxPersonen = 80,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 14,
                    Moeilijkheid = ActiviteitMoeilijkheid.Gemiddeld
                },
                new SportActiviteit
                {
                    Titel = "Tafeltennis Toernooi",
                    Beschrijving = "Toernooi voor amateur tafeltennissers.",
                    DatumTijd = new DateTime(2024, 8, 7, 12, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sports.png"),
                    Longitude = 4.3507m,
                    Latitude = 50.8464m,
                    MaxPersonen = 40,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 16,
                    Moeilijkheid = ActiviteitMoeilijkheid.Makkelijk
                },
                new SportActiviteit
                {
                    Titel = "Wielerwedstrijd",
                    Beschrijving = "Een spannende wielerwedstrijd door het centrum van Brussel.",
                    DatumTijd = new DateTime(2024, 8, 11, 9, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sports.png"),
                    Longitude = 4.3489m,
                    Latitude = 50.8462m,
                    MaxPersonen = 60,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 18,
                    Moeilijkheid = ActiviteitMoeilijkheid.Zwaar
                },
                new SportActiviteit
                {
                    Titel = "Klimwedstrijd",
                    Beschrijving = "Klimcompetitie op een klimwand.",
                    DatumTijd = new DateTime(2024, 8, 15, 13, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sports.png"),
                    Longitude = 4.3521m,
                    Latitude = 50.8472m,
                    MaxPersonen = 30,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 16,
                    Moeilijkheid = ActiviteitMoeilijkheid.Zwaar
                },
                new SportActiviteit
                {
                    Titel = "Tennis Toernooi",
                    Beschrijving = "Toernooi voor tennisfanaten.",
                    DatumTijd = new DateTime(2024, 8, 20, 10, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sports.png"),
                    Longitude = 4.3515m,
                    Latitude = 50.8469m,
                    MaxPersonen = 50,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 16,
                    Moeilijkheid = ActiviteitMoeilijkheid.Gemiddeld
                },

                // Meer Cultuuractiviteiten
                new CultuurActiviteit
                {
                    Titel = "Jazz Festival",
                    Beschrijving = "Een festival met jazzmuziek van lokale en internationale artiesten.",
                    DatumTijd = new DateTime(2024, 8, 3, 20, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3550m,
                    Latitude = 50.8471m,
                    MaxPersonen = 150,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 18,
                    Sector = ActiviteitSector.Muziek
                },
                new CultuurActiviteit
                {
                    Titel = "Openlucht Theater",
                    Beschrijving = "Een openlucht theaterstuk in de tuin van het museum.",
                    DatumTijd = new DateTime(2024, 8, 9, 19, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3560m,
                    Latitude = 50.8465m,
                    MaxPersonen = 100,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 12,
                    Sector = ActiviteitSector.Theater
                },
                new CultuurActiviteit
                {
                    Titel = "Moderne Dans Workshop",
                    Beschrijving = "Workshop moderne dans voor alle niveaus.",
                    DatumTijd = new DateTime(2024, 8, 12, 15, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3545m,
                    Latitude = 50.8468m,
                    MaxPersonen = 25,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 16,
                    Sector = ActiviteitSector.Dans
                },
                new CultuurActiviteit
                {
                    Titel = "Kunstmarkt",
                    Beschrijving = "Markt met kunst en ambachten van lokale kunstenaars.",
                    DatumTijd = new DateTime(2024, 8, 18, 10, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3557m,
                    Latitude = 50.8472m,
                    MaxPersonen = 200,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 0,
                    Sector = ActiviteitSector.Andere
                },
                new CultuurActiviteit
                {
                    Titel = "Theaterimprovisatie",
                    Beschrijving = "Improviseer en speel mee in een theaterstuk.",
                    DatumTijd = new DateTime(2024, 8, 25, 17, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3567m,
                    Latitude = 50.8475m,
                    MaxPersonen = 50,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 12,
                    Sector = ActiviteitSector.Theater
                },
                new CultuurActiviteit
                {
                    Titel = "Kunsttentoonstelling",
                    Beschrijving = "Expositie van moderne kunstwerken.",
                    DatumTijd = new DateTime(2024, 8, 1, 11, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3500m,
                    Latitude = 50.8500m,
                    MaxPersonen = 100,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 0,
                    Sector = ActiviteitSector.Muziek
                },
                new CultuurActiviteit
                {
                    Titel = "Straatmuziekfestival",
                    Beschrijving = "Festival met live straatmuzikanten.",
                    DatumTijd = new DateTime(2024, 8, 2, 14, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3485m,
                    Latitude = 50.8490m,
                    MaxPersonen = 300,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 0,
                    Sector = ActiviteitSector.Muziek
                },
                new CultuurActiviteit
                {
                    Titel = "Fotografie Workshop",
                    Beschrijving = "Leer de basis van fotografie in een workshop.",
                    DatumTijd = new DateTime(2024, 8, 3, 09, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3540m,
                    Latitude = 50.8530m,
                    MaxPersonen = 20,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 16,
                    Sector = ActiviteitSector.Andere
                },
                new CultuurActiviteit
                {
                    Titel = "Muzikale Avond",
                    Beschrijving = "Geniet van live muziek van lokale bands.",
                    DatumTijd = new DateTime(2024, 8, 4, 19, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3520m,
                    Latitude = 50.8540m,
                    MaxPersonen = 150,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 0,
                    Sector = ActiviteitSector.Muziek
                },
                new CultuurActiviteit
                {
                    Titel = "Dansworkshop",
                    Beschrijving = "Leer nieuwe dansmoves in een workshop.",
                    DatumTijd = new DateTime(2024, 8, 5, 18, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3570m,
                    Latitude = 50.8520m,
                    MaxPersonen = 30,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 12,
                    Sector = ActiviteitSector.Dans
                },
                new CultuurActiviteit
                {
                    Titel = "Poëzie Lezing",
                    Beschrijving = "Beleef een avond van poëzie door lokale dichters.",
                    DatumTijd = new DateTime(2024, 8, 6, 20, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3550m,
                    Latitude = 50.8510m,
                    MaxPersonen = 80,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 0,
                    Sector = ActiviteitSector.Andere
                },
                new CultuurActiviteit
                {
                    Titel = "Theaterwandeling",
                    Beschrijving = "Een interactieve theaterwandeling door de stad.",
                    DatumTijd = new DateTime(2024, 8, 7, 16, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3560m,
                    Latitude = 50.8505m,
                    MaxPersonen = 40,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 12,
                    Sector = ActiviteitSector.Theater
                },
                new CultuurActiviteit
                {
                    Titel = "Kunstworkshop voor Kinderen",
                    Beschrijving = "Creatieve kunstactiviteiten voor kinderen.",
                    DatumTijd = new DateTime(2024, 8, 8, 10, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/cultuur.png"),
                    Longitude = 4.3580m,
                    Latitude = 50.8525m,
                    MaxPersonen = 25,
                    Soort = ActiviteitSoort.Cultuur,
                    Leeftijdsgroep = 6,
                    Sector = ActiviteitSector.Andere
                },
                new SportActiviteit
                {
                    Titel = "Marathon van Brussel",
                    Beschrijving = "Een marathon door de stad Brussel.",
                    DatumTijd = new DateTime(2024, 8, 9, 07, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sport.png"),
                    Longitude = 4.3550m,
                    Latitude = 50.8500m,
                    MaxPersonen = 1000,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 18,
                    Moeilijkheid = ActiviteitMoeilijkheid.Zwaar
                },
                new SportActiviteit
                {
                    Titel = "Zumba les in het Park",
                    Beschrijving = "Zumba les in een lokaal park.",
                    DatumTijd = new DateTime(2024, 8, 10, 17, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sport.png"),
                    Longitude = 4.3515m,
                    Latitude = 50.8495m,
                    MaxPersonen = 50,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 12,
                    Moeilijkheid = ActiviteitMoeilijkheid.Makkelijk
                },
                new SportActiviteit
                {
                    Titel = "Fietstocht door Brussel",
                    Beschrijving = "Een georganiseerde fietstocht door de stad.",
                    DatumTijd = new DateTime(2024, 8, 11, 09, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sport.png"),
                    Longitude = 4.3525m,
                    Latitude = 50.8505m,
                    MaxPersonen = 60,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 12,
                    Moeilijkheid = ActiviteitMoeilijkheid.Gemiddeld
                },
                new SportActiviteit
                {
                    Titel = "Basketbaltoernooi",
                    Beschrijving = "Toernooi voor lokale basketbalteams.",
                    DatumTijd = new DateTime(2024, 8, 12, 14, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sport.png"),
                    Longitude = 4.3535m,
                    Latitude = 50.8520m,
                    MaxPersonen = 100,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 16,
                    Moeilijkheid = ActiviteitMoeilijkheid.Gemiddeld
                },
                new SportActiviteit
                {
                    Titel = "Voetbalwedstrijd",
                    Beschrijving = "Een spannende voetbalwedstrijd tussen teams.",
                    DatumTijd = new DateTime(2024, 8, 13, 15, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sport.png"),
                    Longitude = 4.3565m,
                    Latitude = 50.8515m,
                    MaxPersonen = 200,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 12,
                    Moeilijkheid = ActiviteitMoeilijkheid.Gemiddeld
                },
                new SportActiviteit
                {
                    Titel = "Yoga in het Park",
                    Beschrijving = "Yoga sessie in het lokale park.",
                    DatumTijd = new DateTime(2024, 8, 14, 08, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sport.png"),
                    Longitude = 4.3555m,
                    Latitude = 50.8525m,
                    MaxPersonen = 30,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 12,
                    Moeilijkheid = ActiviteitMoeilijkheid.Makkelijk
                },
                new SportActiviteit
                {
                    Titel = "Hardloopwedstrijd",
                    Beschrijving = "Wedstrijd voor hardlopers van alle niveaus.",
                    DatumTijd = new DateTime(2024, 8, 15, 10, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sport.png"),
                    Longitude = 4.3575m,
                    Latitude = 50.8530m,
                    MaxPersonen = 150,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 18,
                    Moeilijkheid = ActiviteitMoeilijkheid.Gemiddeld
                },
                new SportActiviteit
                {
                    Titel = "Tafeltennistoernooi",
                    Beschrijving = "Toernooi voor tafeltennis liefhebbers.",
                    DatumTijd = new DateTime(2024, 8, 16, 13, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/sport.png"),
                    Longitude = 4.3545m,
                    Latitude = 50.8520m,
                    MaxPersonen = 40,
                    Soort = ActiviteitSoort.Sport,
                    Leeftijdsgroep = 12,
                    Moeilijkheid = ActiviteitMoeilijkheid.Makkelijk
                },
                new HobbyActiviteit
                {
                    Titel = "Kookworkshop",
                    Beschrijving = "Leer nieuwe kooktechnieken in deze workshop.",
                    DatumTijd = new DateTime(2024, 8, 17, 11, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3555m,
                    Latitude = 50.8515m,
                    MaxPersonen = 20,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 18,
                    Niveau = ActiviteitNiveau.Beginner
                },
                new HobbyActiviteit
                {
                    Titel = "Sieraden Maken",
                    Beschrijving = "Workshop over het maken van je eigen sieraden.",
                    DatumTijd = new DateTime(2024, 8, 18, 14, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3560m,
                    Latitude = 50.8500m,
                    MaxPersonen = 15,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 16,
                    Niveau = ActiviteitNiveau.Halfgevorderd
                },
                new HobbyActiviteit
                {
                    Titel = "Schilderworkshop",
                    Beschrijving = "Leer schildertechnieken in een creatieve workshop.",
                    DatumTijd = new DateTime(2024, 8, 19, 10, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3575m,
                    Latitude = 50.8525m,
                    MaxPersonen = 25,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 18,
                    Niveau = ActiviteitNiveau.Halfgevorderd
                },
                new HobbyActiviteit
                {
                    Titel = "Modelbouwdag",
                    Beschrijving = "Dag om je eigen modelbouwprojecten te maken.",
                    DatumTijd = new DateTime(2024, 8, 20, 13, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3550m,
                    Latitude = 50.8530m,
                    MaxPersonen = 30,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 12,
                    Niveau = ActiviteitNiveau.Beginner
                },
                new HobbyActiviteit
                {
                    Titel = "Lezing over Fotografie",
                    Beschrijving = "Leer meer over de kunst van fotografie.",
                    DatumTijd = new DateTime(2024, 8, 21, 19, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3540m,
                    Latitude = 50.8505m,
                    MaxPersonen = 50,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 0,
                    Niveau = ActiviteitNiveau.Gevorderd
                },
                new HobbyActiviteit
                {
                    Titel = "Creatieve Schrijfworkshop",
                    Beschrijving = "Workshop voor het ontwikkelen van je schrijfvaardigheden.",
                    DatumTijd = new DateTime(2024, 8, 22, 16, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3555m,
                    Latitude = 50.8510m,
                    MaxPersonen = 20,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 16,
                    Niveau = ActiviteitNiveau.Beginner
                },
                new HobbyActiviteit
                {
                    Titel = "Kunst en Ambachten Fair",
                    Beschrijving = "Bezoek een fair met kunst en ambachten.",
                    DatumTijd = new DateTime(2024, 8, 23, 10, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3565m,
                    Latitude = 50.8535m,
                    MaxPersonen = 60,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 0,
                    Niveau = ActiviteitNiveau.Nvt
                },
                new HobbyActiviteit
                {
                    Titel = "Yoga en Meditatie",
                    Beschrijving = "Workshop met yoga en meditatie sessies.",
                    DatumTijd = new DateTime(2024, 8, 24, 09, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3555m,
                    Latitude = 50.8520m,
                    MaxPersonen = 25,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 12,
                    Niveau = ActiviteitNiveau.Halfgevorderd
                },
                new HobbyActiviteit
                {
                    Titel = "Creatieve Fotografie",
                    Beschrijving = "Workshop over creatieve technieken in fotografie.",
                    DatumTijd = new DateTime(2024, 8, 25, 11, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3570m,
                    Latitude = 50.8505m,
                    MaxPersonen = 20,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 16,
                    Niveau = ActiviteitNiveau.Halfgevorderd
                },
                new HobbyActiviteit
                {
                    Titel = "Handwerken en Borduren",
                    Beschrijving = "Leer handwerken en borduren in een gezellige sfeer.",
                    DatumTijd = new DateTime(2024, 8, 26, 14, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3580m,
                    Latitude = 50.8525m,
                    MaxPersonen = 15,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 12,
                    Niveau = ActiviteitNiveau.Beginner
                },
                new HobbyActiviteit
                {
                    Titel = "Bloemschikken Workshop",
                    Beschrijving = "Maak je eigen bloemstukken in een workshop.",
                    DatumTijd = new DateTime(2024, 8, 27, 10, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3550m,
                    Latitude = 50.8510m,
                    MaxPersonen = 20,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 18,
                    Niveau = ActiviteitNiveau.Beginner
                },
                new HobbyActiviteit
                {
                    Titel = "DIY Decoratie",
                    Beschrijving = "Workshop voor het maken van je eigen decoraties.",
                    DatumTijd = new DateTime(2024, 8, 28, 15, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3560m,
                    Latitude = 50.8530m,
                    MaxPersonen = 25,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 12,
                    Niveau = ActiviteitNiveau.Halfgevorderd
                },
                new HobbyActiviteit
                {
                    Titel = "Digitaal Tekenen",
                    Beschrijving = "Leer digitaal tekenen met behulp van speciale software.",
                    DatumTijd = new DateTime(2024, 8, 29, 11, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3575m,
                    Latitude = 50.8520m,
                    MaxPersonen = 15,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 16,
                    Niveau = ActiviteitNiveau.Gevorderd
                },
                new HobbyActiviteit
                {
                    Titel = "Muziek en Geluid Effecten",
                    Beschrijving = "Leer over muziek en geluidseffecten in een interactieve workshop.",
                    DatumTijd = new DateTime(2024, 8, 30, 13, 0, 0),
                    Icoon = LoadImage("Assets/Iconen/hobby.png"),
                    Longitude = 4.3555m,
                    Latitude = 50.8505m,
                    MaxPersonen = 20,
                    Soort = ActiviteitSoort.Hobby,
                    Leeftijdsgroep = 12,
                    Niveau = ActiviteitNiveau.Halfgevorderd
                }
            };

        public static void InitializeData()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                // open connectie
                conn.Open();

                // voer SQL commando uit
                SqlCommand comm = new SqlCommand("SELECT * FROM Persoon", conn);

                SqlDataReader reader = comm.ExecuteReader();

                // hier kijk ik eerst of er al data bestaat. als er niks bestaat enkel dan maken we aan (ervanuitgaand dat als personen leeg is, dat de hele db leeg is)
                if (!reader.Read())
                {
                    FillPersons();
                    FillActiviteiten();
                    FillDeelnamen();
                }
            }
        }

        private static void FillDeelnamen()
        {
            var personen = GetPersonen();
            var activiteiten = GetActiviteiten();

            // Randomizer initialiseren
            Random random = new Random();

            // Houdt bij welke combinaties al zijn toegevoegd (voor het geval je geen dubbele deelnames wilt)
            HashSet<(int persoonId, int activiteitId)> bestaandeDeelnamen = new HashSet<(int, int)>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                for (int i = 0; i < 50; i++) // Aantal deelnames
                {
                    int persoonId;
                    int activiteitId;

                    // Kies willekeurig een persoon en een activiteit
                    do
                    {
                        persoonId = personen[random.Next(personen.Count)].Id;
                        activiteitId = activiteiten[random.Next(activiteiten.Count)].Id;
                    }
                    while (bestaandeDeelnamen.Contains((persoonId, activiteitId))); // Voorkom dubbele deelnames

                    bestaandeDeelnamen.Add((persoonId, activiteitId));

                    // SQL Command voor het toevoegen van een deelname
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Deelname (Persoon_Id, Activiteit_Id) VALUES (@PersoonId, @ActiviteitId)", conn))
                    {
                        cmd.Parameters.AddWithValue("@PersoonId", persoonId);
                        cmd.Parameters.AddWithValue("@ActiviteitId", activiteitId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void FillActiviteiten()
        {
            var personen = GetPersonen();  // dit doe ik om juiste id's te krijgen. moet ook dynamisch zijn
            Random random = new Random();
            foreach (var activiteit in activiteiten)
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string sql = "INSERT INTO Activiteit (Titel, Beschrijving, DatumTijd, Icoon, Longitude, Latitude, MaxPersonen, Soort, Leeftijdsgroep, Organisator_Id, Sector) VALUES (@Titel, @Beschrijving, @DatumTijd, @Icoon, @Longitude, @Latitude, @MaxPersonen, @Soort, @Leeftijdsgroep, @OrganisatorId, @Sector)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Titel", activiteit.Titel);
                        cmd.Parameters.AddWithValue("@Beschrijving", activiteit.Beschrijving);
                        cmd.Parameters.AddWithValue("@DatumTijd", activiteit.DatumTijd);
                        if (activiteit.Icoon == null)
                        {
                            SqlParameter imageParameter = new SqlParameter("@Icoon", SqlDbType.Image);
                            imageParameter.Value = DBNull.Value;
                            cmd.Parameters.Add(imageParameter);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Icoon", activiteit.Icoon);
                        }
                        cmd.Parameters.AddWithValue("@Longitude", activiteit.Longitude);
                        cmd.Parameters.AddWithValue("@Latitude", activiteit.Latitude);
                        cmd.Parameters.AddWithValue("@MaxPersonen", activiteit.MaxPersonen);
                        cmd.Parameters.AddWithValue("@Soort", activiteit.Soort);
                        cmd.Parameters.AddWithValue("@Leeftijdsgroep", activiteit.Leeftijdsgroep);
                        cmd.Parameters.AddWithValue("@OrganisatorId", personen[random.Next(personen.Count)].Id);

                        // Voeg type-specifieke parameters toe
                        if (activiteit is CultuurActiviteit cultuurActiviteit)
                        {
                            cmd.Parameters.AddWithValue("@Sector", cultuurActiviteit.Sector);
                            cmd.Parameters.AddWithValue("@Moeilijkheid", DBNull.Value);
                            cmd.Parameters.AddWithValue("@Niveau", DBNull.Value);
                        }
                        else if (activiteit is SportActiviteit sportActiviteit)
                        {
                            cmd.Parameters.AddWithValue("@Moeilijkheid", sportActiviteit.Moeilijkheid);
                            cmd.Parameters.AddWithValue("@Sector", DBNull.Value);
                            cmd.Parameters.AddWithValue("@Niveau", DBNull.Value);
                        }
                        else if (activiteit is HobbyActiviteit hobbyActiviteit)
                        {
                            cmd.Parameters.AddWithValue("@Niveau", hobbyActiviteit.Niveau);
                            cmd.Parameters.AddWithValue("@Sector", DBNull.Value);
                            cmd.Parameters.AddWithValue("@Moeilijkheid", DBNull.Value);
                        }

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // personen in de db inserten
        private static void FillPersons()
        {
            foreach (var persoon in personen)
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Persoon (voornaam, achternaam, login, paswoord, regdatum, isadmin, profielfoto) VALUES (@Voornaam, @Achternaam, @Login, @Paswoord, @RegDatum, @IsAdmin, @Profielfoto)", conn))
                    {
                        cmd.Parameters.AddWithValue("@Voornaam", persoon.Voornaam);
                        cmd.Parameters.AddWithValue("@Achternaam", persoon.Achternaam);
                        cmd.Parameters.AddWithValue("@Login", persoon.Login);
                        cmd.Parameters.AddWithValue("@Paswoord", persoon.Paswoord);
                        cmd.Parameters.AddWithValue("@RegDatum", persoon.RegDatum);
                        cmd.Parameters.AddWithValue("@IsAdmin", persoon.IsAdmin);
                        cmd.Parameters.AddWithValue("@Profielfoto", persoon.Profielfoto ?? (object)DBNull.Value);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private static List<Persoon> GetPersonen()
        {
            var personen = new List<Persoon>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Id FROM Persoon", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            personen.Add(new Persoon { Id = reader.GetInt32(0) });
                        }
                    }
                }
            }
            return personen;
        }

        private static List<Activiteit> GetActiviteiten()
        {
            var activiteiten = new List<Activiteit>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Activiteit", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ActiviteitSoort soort = (ActiviteitSoort)(int)reader["soort"];
                            if (soort == ActiviteitSoort.Sport)
                            {
                                activiteiten.Add(new SportActiviteit { Id = reader.GetInt32(0) });
                            }
                            else if (soort == ActiviteitSoort.Hobby)
                            {
                                activiteiten.Add(new HobbyActiviteit { Id = reader.GetInt32(0) });
                            }
                            else if (soort == ActiviteitSoort.Cultuur)
                            {
                                activiteiten.Add(new CultuurActiviteit { Id = reader.GetInt32(0) });
                            }
                        }
                    }
                }
            }
            return activiteiten;
        }

        private static byte[] LoadImage(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return File.ReadAllBytes(filePath);
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image from {filePath}: {ex.Message}");
                return null;
            }
        }
    }
}