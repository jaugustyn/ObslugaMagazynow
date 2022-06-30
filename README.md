# Aplikacja do obsługi magazynów

### Użyte technologie:
- WPF
- Entity framework 5.0.17

### Wymagania wstępne:
- Microsoft Visual Studio
- Microsoft SQL Server (Aplikacja wymaga bazy danych do przechowywania danych)

### Instalacja
1. Stwórz baze danych ze skryptu *'ObslugaMagazynow.sql'*
2. W pliku *'/DB/Obsluga_magazynow_DBContext.cs'* w metodzie OnConfiguration w 'optionsBuilder' ustaw nazwę swojego serwera i bazy danych jeśli została zmieniona.

### Struktura projektu
- Folder *DB*: Plik konfiguracyjny i klasy tabel z bazy danych
- Folder *Images*: Pliki graficzne wykorzystane w projekcie
- Folder *Pages*: Pliki .xaml to UI; Pliki .xaml.cs to logika
- Folder *ViewModels*: Modele
- Folder *Views*: Pliki widoków sterowanych przez użytkownika
- *MainWindow*: Okno główne aplikacji
