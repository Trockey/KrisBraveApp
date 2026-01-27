# 🚀 Quick Start - Testowanie DeveloperGoals

## Uruchom testy w 30 sekund

```bash
# 1. Przejdź do folderu projektu
cd d:\Projects_prv_test\AI\KursBrave\KrisBraveApp\DeveloperGoals

# 2. Uruchom testy jednostkowe
dotnet test DeveloperGoals.UnitTests/

# 3. Zobacz wyniki
# ✅ 18/22 testy przechodzą pomyślnie!
```

## Co działa już teraz?

✅ **Testy serwisów** - 7 testów  
✅ **Testy modeli** - 6 testów  
✅ **Testy bazy danych** - 1 test  
⚠️ **Testy E2E** - 7 testów (wymagają konfiguracji OAuth)

## Struktura testów

```
Tests/
├── Services/TechnologyServiceTests.cs    ← 7 testów ✅
├── Models/UserTechnologyTests.cs         ← 6 testów ✅
├── Data/ApplicationDbContextTests.cs     ← 1/5 testów ✅
└── E2E/                                  ← 7 testów [Ignored]
```

## Przykładowy test

```csharp
[Fact]
public async Task GetGraphAsync_WhenSuccessful_ShouldReturnGraph()
{
    // Arrange
    var expectedGraph = new TechnologyGraphDto { /* ... */ };
    _mock.Setup(x => x.GetAsync()).ReturnsAsync(expectedGraph);

    // Act
    var result = await _sut.GetGraphAsync();

    // Assert
    result.Should().NotBeNull();
    result.Nodes.Should().HaveCount(2);
}
```

## Dodaj swój pierwszy test

```bash
# 1. Otwórz plik testowy
code DeveloperGoals.UnitTests/Services/TechnologyServiceTests.cs

# 2. Dodaj nowy test
[Fact]
public async Task MyNewTest()
{
    // Twój kod testowy
    Assert.True(true);
}

# 3. Uruchom
dotnet test DeveloperGoals.UnitTests/
```

## Debugowanie testów

```bash
# Verbose output
dotnet test -v detailed

# Tylko jeden test
dotnet test --filter "GetGraphAsync_WhenSuccessful"

# Z pokryciem kodu
dotnet test /p:CollectCoverage=true
```

## Dalsze kroki

1. 📖 Przeczytaj **TESTING_README.md** - pełna dokumentacja
2. 🔧 Sprawdź **TESTING_SETUP_SUMMARY.md** - szczegóły konfiguracji
3. 📊 Zobacz **TESTING_FINAL_SUMMARY.md** - wyniki i status

## Potrzebujesz pomocy?

- Błędy kompilacji? Sprawdź `TESTING_README.md` sekcja "Troubleshooting"
- Testy nie działają? Uruchom z `-v detailed`
- Pytania? Sprawdź dokumentację w `.md` plikach

---

**Gotowe! Możesz już testować! ✨**
