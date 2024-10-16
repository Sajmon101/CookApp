# Instalacja CookApp

Poniższa instrukcja pomoże Ci skonfigurować i uruchomić aplikację CookApp na lokalnym serwerze.

## Wymagania wstępne

- **.NET SDK 7.0 lub nowszy**: Upewnij się, że masz zainstalowane .NET SDK, które pozwala na uruchamianie aplikacji WPF i ASP.NET Core Web API.
- **SQL Server**: Aplikacja korzysta z bazy danych SQL Server. Możesz użyć darmowej wersji SQL Server Express do przechowywania danych aplikacji.
- **SQL Server Management Studio (SSMS)**: Zalecane narzędzie do zarządzania SQL Server, które ułatwia konfigurację bazy danych i jej obsługę.


## Krok 1: Klonowanie repozytorium

Najpierw sklonuj repozytorium aplikacji CookApp na swój lokalny komputer:

git clone https://github.com/Sajmon101/CookApp.git


## Krok 2: Przywracanie bazy danych z pliku .bak

1. **Uruchom SSMS** i połącz się z instancją swojego lokalnego serwera SQL Server.
2. W **Object Explorer** kliknij prawym przyciskiem myszy na `Databases` i wybierz **Restore Database...**.
3. W oknie **Source** wybierz **Device** i kliknij przycisk z trzema kropkami (`...`).
4. W oknie **Select backup devices** kliknij **Add** i wskaż plik `CookApp.bak`, który został dostarczony w plikach projektu.
5. Wybierz plik .bak i kliknij **OK**, a następnie ponownie **OK** w oknie **Select backup devices**.
6. W sekcji **Destination** wybierz nazwę bazy danych, np. `CookApp`, i kliknij **OK**, aby przywrócić bazę danych.

Twoja baza danych powinna być teraz gotowa do użycia.

## Krok 3: Konfiguracja plików appsettings.json

### Tworzenie plików appsettings.json

W folderach `CookApp` oraz `CookAppAPI` utwórz pliki `appsettings.json` na podstawie dostarczonych przykładów `appsettings.example.json`.

Przykładowa struktura `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=YourServerName;Database=CookApp;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "ApiSettings": {
    "BaseUrl": "http://localhost:YourPort/"
  }
}
```

**Uwaga**: Dostosuj `Server`, `Database` i `BaseUrl` do swoich lokalnych ustawień.

## Krok 4: Uruchomienie aplikacji

### Uruchomienie WebAPI

1. Przejdź do folderu `CookAppAPI`:

    ```
    cd CookAppAPI
    ```

2. Przywróć zależności projektu:

    ```
    dotnet restore
    ```

3. Uruchom aplikację WebAPI:

    ```
    dotnet run
    ```

WebAPI powinno być teraz dostępne na `http://localhost:36032/`.

### Uruchomienie aplikacji WPF

1. Przejdź do folderu `CookApp`:

    ```
    cd CookApp
    ```

2. Przywróć zależności projektu:

    ```
    dotnet restore
    ```

3. Otwórz rozwiązanie CookApp w Visual Studio lub innym kompatybilnym edytorze i uruchom aplikację WPF.

## Opcjonalnie: Uruchamianie obu projektów jednocześnie

Jeśli chcesz, aby oba projekty były uruchamiane jednocześnie w Visual Studio, wykonaj następujące kroki:

1. Otwórz projekt CookApp (CookApp.sln)
2. W **Solution Explorer** kliknij prawym przyciskiem myszy na nazwę rozwiązania i wybierz **Properties**.
3. W sekcji **Startup Project** wybierz **Multiple startup projects**.
4. Ustaw **Action** na **Start** dla obu projektów: `CookApp` i `CookAppAPI`.
5. Kliknij **OK**, aby zapisać zmiany.

## Krok 5: Testowanie aplikacji

Aplikacja jest teraz gotowa do użytku. Zaloguj się jako kelner lub kucharz i sprawdź proces składania i realizacji zamówień.

## Problemy z uruchomieniem

Jeśli napotkasz problemy:
- Upewnij się, że serwer SQL działa i że baza danych jest poprawnie przywrócona.
- Sprawdź konfigurację `appsettings.json` w obu projektach i upewnij się, że ustawienia połączenia są poprawne.
