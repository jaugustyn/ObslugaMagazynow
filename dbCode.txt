CREATE DATABASE Obsluga_magazynow_DB
GO
USE Obsluga_magazynow_DB
GO
CREATE TABLE adresy(
id_adresu Int Primary Key Identity(1,1)
,miejscowosc nVarChar(30) NOT NULL
,ulica nVarChar(20) NOT NULL
,nr_domu Varchar(3) NULL
,nr_mieszkania Varchar(6) NULL
,kod_pocztowy Char(6) NOT NULL
)
CREATE TABLE magazyny(
id_magazynu Int Primary Key Identity(1,1)
,adres nVarChar(50) UNIQUE NOT NULL
,ilosc_sektorow Tinyint NULL
,ilosc_pracownikow Tinyint NULL
,czy_aktywny Bit DEFAULT(0) NOT NULL
,opis nVarChar(100) NULL
)
CREATE TABLE hurtownie(
id_hurtowni Int Primary Key Identity(1,1)
,nazwa nVarChar(30) NOT NULL
,nip Char(10) NOT NULL
)
CREATE TABLE pracownicy(
id_pracownika Int Primary Key Identity(1,1)
,adres_id Int UNIQUE NOT NULL
,magazyn_id Int NOT NULL
,stanowisko nVarChar(50) CHECK(stanowisko IN ('Logistyk', 'Magazynier', 'Praktykant', 'Informatyk', 'Kierownik ds. Logistyki i Dystrybucji')) NOT NULL
,imie nVarChar(20) NOT NULL
,nazwisko nVarChar(30) NOT NULL
,nr_tel Char(11) UNIQUE NOT NULL
,email nVarChar(30) UNIQUE NOT NULL
,data_urodzenia Date CHECK(data_urodzenia <= GetDate()) NOT NULL
,pesel Char(11) UNIQUE NOT NULL
,plec Char (1) CHECK(plec IN ('m', 'k')) NOT NULL
,data_zatrudnienia Date DEFAULT(GetDate()) NOT NULL
,CONSTRAINT fk_adres_pracownika Foreign key(adres_id) REFERENCES adresy(id_adresu) ON DELETE NO ACTION
,CONSTRAINT fk_magazyn_pracownika Foreign key(magazyn_id) REFERENCES magazyny(id_magazynu) ON DELETE NO ACTION
)
CREATE TABLE sektory(
id_sektoru Int Primary Key Identity(1,1)
,oznaczenie Char(5) UNIQUE NOT NULL
,opis nVarChar(50) NULL
,limit Tinyint NOT NULL
)
CREATE TABLE sektory_magazynow(
magazyn_id Int NOT NULL
,sektor_id Int NOT NULL
,CONSTRAINT pk Primary Key(magazyn_id, sektor_id)
,CONSTRAINT fk_magazyn_id Foreign key(magazyn_id) REFERENCES magazyny(id_magazynu) ON DELETE NO ACTION
,CONSTRAINT fk_sektor_id Foreign key(sektor_id) REFERENCES sektory(id_sektoru) ON DELETE NO ACTION
)
CREATE TABLE pakiety(
id_pakietu Int Primary Key Identity(1,1)
,magazyn_id Int NOT NULL
,sektor_id Int NOT NULL
,kod nVarChar(30) UNIQUE NOT NULL
,CONSTRAINT fk_magazyn_sektor_pakietu Foreign key(magazyn_id, sektor_id) REFERENCES sektory_magazynow(magazyn_id, sektor_id) ON DELETE NO ACTION
)
CREATE TABLE faktury(
id_faktury Int Primary Key Identity(1,1)
,wystawiajacy_id Int NOT NULL
,hurtownia_id Int NOT NULL
,numer_faktury nVarChar(30) UNIQUE NOT NULL
,data_wystawienia SmallDateTime DEFAULT(GetDate()) CHECK(data_wystawienia <= GETDATE()) NOT NULL
,wartosc Real CHECK(wartosc > 0) NOT NULL
,opis nVarChar(50) NULL
,CONSTRAINT fk_pracownik_id Foreign key(wystawiajacy_id) REFERENCES pracownicy(id_pracownika) ON DELETE NO ACTION
,CONSTRAINT fk_hurtownia_id Foreign key(hurtownia_id) REFERENCES hurtownie(id_hurtowni) ON DELETE NO ACTION
)
GO
INSERT INTO adresy(miejscowosc, ulica, nr_domu, nr_mieszkania, kod_pocztowy) VALUES
('Kraków', 'Słowicza', null, '19/7', '31-320')
,('Kraków', 'Mozarta Wolfganga', null, '12C/3', '31-232')
,('Kraków', 'Słowicza', '200', null, '31-320')
,('Niepołomice', 'Kolejowa', '171', null, '32-005')
,('Niepołomice', 'Portowa', null, '2B/4', '32-005')
,('Bochnia', '3 Maja', null, '11/2', '32-015')
,('Stanisławice', 'Senatorska', '115', null, '32-015')
INSERT INTO magazyny(adres, czy_aktywny, opis) VALUES
('Kraków ul. Słona 12', 1, 'Magazyn główny, wysokiego składowania'),
('Niepołomice ul. kolejowa 55', 1, 'Magazyn średniego składowania'),
('Stanisławice ul. Polna 167', 1, 'Magazyn zamknięty, niskiego i średniego składowania'),
('Bochnia ul. 3 Maja 44', 0, 'W budowie')
INSERT INTO hurtownie (nazwa, nip) VALUES
('Hurtownia Misio', '1232343456')
,('Hurtownia M&Ms Partners s.c.', '5557772498')
,('Hurtownia Denetet', '8787871234')
INSERT INTO pracownicy(adres_id, magazyn_id, stanowisko, imie, nazwisko, nr_tel, email, data_urodzenia, pesel, plec) VALUES
(1, 2, 'Logistyk', 'Amir', 'Kozłowski', '795-674-141', 'amirkozlowski@gmail.com', '1990.01.01', '03252148199', 'm')
,(2, 3, 'Logistyk', 'Roksana', 'Malinowska', '457-379-822', 'roksanamalinowska@wp.pl', '2000.11.11', '63091799183', 'k')
,(3, 1, 'Informatyk', 'Andrzej', 'Kalinowski', '793-285-813', 'andrzejkalinowska@o2.pl', '1982.10.23', '61051677432', 'm')
,(4, 1, 'Praktykant', 'Oskar', 'Przybylski', '885-556-580', 'oskarprzbylski@gmail.com', '2003.03.15', '89021195418', 'm')
,(5, 2, 'Magazynier', 'Amadeusz', 'Piotrowski', '605-551-101', 'amadeuszpiotrkowski@wp.pl', '1965.04.29', '92073185197', 'm')
,(6, 3, 'Magazynier', 'Amelia', 'Kamińska', '795-555-768', 'ameliakaminska@onet.pl', '1984.03.21', '74082284719', 'k')
,(7, 1, 'Kierownik ds. Logistyki i Dystrybucji', 'Borys', 'Stępień', '735-550-419', 'borysstepien@gmail.com', '1972.09.13', '96122216856', 'm')
INSERT INTO sektory(oznaczenie, limit, opis) VALUES
('A-1', 10, 'Sektor A, skład niski')
,('B-1', 15, 'Sektor B, skład średni')
,('C-1', 15, 'Sektor C, skład średni')
,('A-2', 20, 'Sektor A2, skład wysoki')
,('B-2', 1, 'Sektor B2, skład średni')
INSERT INTO sektory_magazynow(magazyn_id, sektor_id) VALUES
(1, 1)
,(1, 2)
,(1, 5)
,(2, 1)
,(2, 4)
,(3, 1)
,(3, 2)
,(3, 3)
INSERT INTO pakiety(kod, magazyn_id, sektor_id) VALUES
('A1B4C7D6', 1, 1)
,('A1B4C7D7', 1, 1)
,('A1B5C2D1', 1, 5)
,('A2B6C2D7', 2, 1)
,('A3B12C2D1',2, 4)
,('A3B9C0D7', 3, 1)
,('A3B6C8D1', 3, 2)
INSERT INTO faktury(wystawiajacy_id, hurtownia_id, numer_faktury, data_wystawienia, wartosc, opis) VALUES
(5, 1, '1/D/2022', '2022.02.01 08:47:22', 74000, 'Misie')
,(4, 2, '5/AK/2022', '2022.02.01 11:03:37', 145000, 'Karty graficzne')
,(1, 2, '6/AK/2021', '2021.12.05 15:00:40', 20000, 'Obudowy komputerowe')
,(5, 3, '96/BLU/2022', '2022.01.20 10:33:14', 34500, 'Monitory')
,(5, 3, '106/BLU/2022', '2022.01.28 10:25:21', 99000, 'Procesory')
GO
CREATE VIEW [Wartość_faktur_miesiące] AS
(
SELECT YEAR(data_wystawienia) AS [Rok], MONTH(data_wystawienia) AS [Miesiąc],SUM(wartosc) AS [Suma] from faktury GROUP BY MONTH(data_wystawienia), YEAR(data_wystawienia)
)
GO
CREATE TRIGGER [Maks_pakietów] ON pakiety
INSTEAD OF INSERT
AS
IF (SELECT count(*) FROM pakiety as P, inserted as I WHERE p.magazyn_id = i.magazyn_id AND p.sektor_id = i.sektor_id) < (SELECT limit FROM sektory AS s INNER JOIN inserted AS i ON s.id_sektoru = i.sektor_id)
BEGIN
INSERT INTO pakiety
SELECT magazyn_id, sektor_id, kod FROM inserted
END
ELSE
BEGIN
Print 'Sektor już osiągnął swój limit. Nie można dodać kolejnego pakietu.'
END
GO
CREATE PROCEDURE [Aktualizuj_ilosc_sektorow] @id_magazynu int
AS
UPDATE magazyny SET ilosc_sektorow = (SELECT count(*) from sektory_magazynow WHERE magazyn_id = @id_magazynu) WHERE id_magazynu = @id_magazynu
GO
EXEC Aktualizuj_ilosc_sektorow @id_magazynu = 1
EXEC Aktualizuj_ilosc_sektorow @id_magazynu = 2
EXEC Aktualizuj_ilosc_sektorow @id_magazynu = 3
EXEC Aktualizuj_ilosc_sektorow @id_magazynu = 4
GO
CREATE PROCEDURE [Aktualizuj_ilosc_pracownikow] @id_magazynu int
AS
UPDATE magazyny SET ilosc_pracownikow = (SELECT count(*) from pracownicy WHERE magazyn_id = @id_magazynu) WHERE id_magazynu = @id_magazynu
GO
EXEC [Aktualizuj_ilosc_pracownikow] @id_magazynu = 1
EXEC [Aktualizuj_ilosc_pracownikow] @id_magazynu = 2
EXEC [Aktualizuj_ilosc_pracownikow] @id_magazynu = 3
EXEC [Aktualizuj_ilosc_pracownikow] @id_magazynu = 4
GO
