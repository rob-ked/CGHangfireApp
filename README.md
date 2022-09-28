# CGHangfireApp

Przykładowa aplikacja

# Przygotowanie

## Wymagania

1. Zainstalowany i działający SQL Server
2. Utwórz (dwie) bazy danych z poziomu interfejsu użytkownika dowolnego managera bazy danych (np. SQL Server Management Studio) lub wykonaj poniższy skrypt. Zmień wartości CG*DB, CG*Usr oraz HASŁA na bardziej Tobie odpowiadające. Pamiętaj, aby zastosować te same wartości w pliku ustawień aplikacji.

```sql
    CREATE DATABASE CGHangfireDB;
    GO 
    
    USE CGHangfireDB;
    GO
    
    CREATE LOGIN CGHangfireUsr WITH PASSWORD = 'jakies_haslo';  
    GO  
    
    CREATE USER CGHangfireUsr FOR LOGIN CGHangfireUsr;  
    GO 
    
    EXEC sp_addrolemember N'db_owner', N'CGHangfireUsr';
    GO

    CREATE DATABASE CGDataDB;
    GO 
    
    USE CGDataDB;
    GO
    
    CREATE LOGIN CGDataUsr WITH PASSWORD = 'jakies_haslo';  
    GO  
    
    CREATE USER CGDataUsr FOR LOGIN CGDataUsr;  
    GO 
    
    EXEC sp_addrolemember N'db_owner', N'CGDataUsr';
    GO
```

## Konfiguracja (Settings.json)

Uzupełnij plik Settings.json dostarczony wraz z aplikacją. Możesz pominąć parametry w sekcjach "Hangfire" i "App"
(poza SQLConnectionString) - aplikacja zastosuje domyślne wartości (jak poniżej). 
Dla sekcji "Jobs" dostępne są jedynie wymienione w konfiguracji zadania. Możesz sterować ich statusem (IsActive - true/false) oraz harmonogramem wykonania (zgodnie z CRON, np. https://crontab.cronhub.io/)
Nie zmianiaj konfiguracji Api - ona tutaj jest, bo jest i tyle :P

```json
{
  "Settings": {
    "Api": {
      "BaseURL": "https://jsonplaceholder.typicode.com",
      "Endpoints": [ "posts", "comments", "photos" ]      
    },
    "App": {
      "SQLConnectionString": "Server=.\\SQLEXPRESS01;Database=CGDataDB;User Id=CGDataUsr;Password=jakies_haslo;",
      "JsonFilesPath": "D:\\CGHangfireAppData\\json"
    },
    "Hangfire": {
      "SQLConnectionString": "Server=.\\SQLEXPRESS01;Database=CGHangfireDB;User Id=CGHangfireUsr;Password=jakies_haslo;",
      "Address": "127.0.0.1",
      "Port": "7777",
      "Path": "hangfire",
      "LogLevel": "Error"
    },
    "Jobs": [
      {
        "Name": "FetchRemoteData",
        "IsActive": true,
        "Schedule": "46 22 * * *"
      },
      {
        "Name": "CopyDataToDB",
        "IsActive": true,
        "Schedule": "10 * * * *"
      },
      {
        "Name": "FetchDataFromDB",
        "IsActive": true,
        "Schedule": "*/5 * * * *"
      }
    ]
  }
}
```
