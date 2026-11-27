# FinCli

A personal finance tracker for the terminal, built with C# and .NET 10.
Add expenses, list them in a table, and see your monthly total. Data is stored as JSON.

![FinCli screenshot](docs/screenshot.png?v=2)

## Features

- Add expenses with amount, category and description
- List all expenses in a styled table
- Show the total and the total of the current month
- Show spending by category as a bar chart
- Data is saved to a JSON file between runs
- Terminal UI built with [Spectre.Console](https://spectreconsole.net/)
- Unit tests with xUnit

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Run

```
git clone https://github.com/recefeb/FinCli.git
cd FinCli
dotnet run --project src/FinCli.App
```

## Test

```
dotnet test
```

## Where is my data?

On Windows, your expenses are saved to `%APPDATA%\FinCli\transactions.json`.
On other systems, .NET chooses the folder (usually `~/.config/FinCli/`).
This file is outside the repository, so it is never committed.

## Project structure

- `src/FinCli.Core` : models, storage and business logic (LINQ totals)
- `src/FinCli.App` : the terminal interface
- `tests/FinCli.Tests` : xUnit tests

---

# FinCli (Türkçe)

C# ve .NET 10 ile yazılmış, terminalde çalışan kişisel finans takip aracı.
Harcama ekleyebilir, tablo halinde listeleyebilir ve aylık toplamı görebilirsin. Veriler JSON olarak saklanır.

## Özellikler

- Tutar, kategori ve açıklama ile harcama ekleme
- Tüm harcamaları şık bir tabloda listeleme
- Toplam ve içinde bulunulan ayın toplamını gösterme
- Harcamaları kategoriye göre bar grafiğinde gösterme
- Veriler çalıştırmalar arasında JSON dosyasında saklanır
- [Spectre.Console](https://spectreconsole.net/) ile terminal arayüzü
- xUnit ile birim testleri

## Gereksinimler

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Çalıştırma

```
git clone https://github.com/recefeb/FinCli.git
cd FinCli
dotnet run --project src/FinCli.App
```

## Test

```
dotnet test
```

## Verilerim nerede?

Windows'ta harcamaların `%APPDATA%\FinCli\transactions.json` dosyasına kaydedilir.
Diğer sistemlerde klasörü .NET seçer (genellikle `~/.config/FinCli/`).
Bu dosya repo dışındadır, bu yüzden commit'e asla girmez.

## Proje yapısı

- `src/FinCli.Core` : modeller, depolama ve iş mantığı (LINQ toplamları)
- `src/FinCli.App` : terminal arayüzü
- `tests/FinCli.Tests` : xUnit testleri
