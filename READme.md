# WorkshopManager

# System zarzadzania warsztatem samochodowym 2.0

Aplikacja webowa umożliwia: dodawanie klientów, pojazdów, zleceń serwisowych oraz generowanie raportów.

---

Rejestracja i logowanie użytkowników (ASP.NET Identity).<br>
Role: Admin, Mechanik, Recepcjonista.

---

## Admin

- Zarządzanie rolami użytkowników
- Zarządzanie zleceniami serwisowymi
- Zarządzanie częściami pojazdu
- Filtrowanie zleceń serwisowych
+ Generowanie miesięcznego raportu PDF napraw

## Mechanik

- Zarządzanie zleceniami serwisowymi (zarządzanie częściami, kosztem, ...)
- Zarządzanie komentarzami
- Zarządzanie zadaniami do zleceń serwisowych

## Recepcjonista

- Zarządzanie klientami
- Zrządzanie pojazdami 
- Zarządzanie częsciami
- Zarządzanie zleceniami (przypisanie mechanika, ...)
- Generowanie raportu (PDF) o użytkowniku, pojazdach i zleceniach

---

## Technologie

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core (Code First)
- Razor Pages 
- SQL Server (LocalDB)
- QuestPDF (raporty PDF)
- C#

---

## Struktura katalogów

WorkshopManager/<br>
│<br>
├── /<br>
├── wwwroot/<br>
│   └── uploads/   # zdjęcia pojazdów<br>
├── Controllers/<br>
├── Models/<br>
├── Services/<br>
├── Views/<br>
├── Migrations/
├── Data/          # ApplicationDbContext, SeedData<br>
└── Program.cs<br>
