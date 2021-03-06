# Warehouse Manager

## Člana ekipe:

63190314 Gregor Volčanjk  
63200104 Timotej Gregorič

## Nekaj slik iz Spletne in Mobilne Aplikacije
![screencapture-warehouse-is-azurewebsites-net-Item-2022-01-13-18_55_15](https://user-images.githubusercontent.com/65243657/149383540-60b00e3b-f003-4689-8bbb-97173733d60a.png)

![screencapture-warehouse-is-azurewebsites-net-Item-Create-2022-01-13-18_56_10](https://user-images.githubusercontent.com/65243657/149383601-0370468b-15b8-4530-9478-eaa46dc74b7c.png)


![image](https://user-images.githubusercontent.com/65243657/149384388-be403380-141c-45a3-83ee-fc9c4eb1df6e.png)

![image](https://user-images.githubusercontent.com/65243657/149384475-11a29420-10cb-4eea-8b59-196207f2138a.png)

![image](https://user-images.githubusercontent.com/65243657/149384572-423c9ea0-4620-473a-91ee-19cba6682fca.png)



## Opis teme

Z informacijskim sistemom Warehouse Manager bodo podjetja in uporabniki le-tega imeli enostavnejši vpogled nad vsemi informacijami in operacijami skladišč, ki si jih lastijo, hkrati pa jim to omogoča tudi veliko lažje vodenje skladišč.
Izdelek ni mišljen za stranke podjetja, temveč za določene delavce podjetja. To pomeni pogled nad izdelki v vsakem izmed skladišč, dobavne čase različnih dobaviteljev in naročil strank in celotno evidenco vseh preteklih dobav in izvozov produktov.

V informacijski sistem se bodo torej prijavljali le določeni zaposleni v podjetju, ki jim je dodeljena ta pravica. Po prijavi bodo ti lahko v sistem vnašali določene podatke (nova pošiljka v evidenco, sprememba števila nekega izdelka v določenem skladišču, dodajanje izdelkov v skladišče...), ali pa brskali po že vnešenih podatkih.


## Entities:

(main)
Warehouse  
Item  
Evidence  
Distributor  
Customer

### Warehouse:

- WarehouseID (int) PK
- Address (string)
- ZIP (int)
- City (string)
- Country (string)

### Item:

- ItemID(int) PK
- Name (string)
- Description (string)
- Quantity (int)
- Active (bool)
- WarehouseID (int) FK
- CustomerID (int) FK

### Evidence:

- EvidenceID (int) PK
- itemID (int) FK
- WarehouseID (int) FK
- CustomerID (int) FK
- Out (DateTIme)

### Distributor:

- DistributorID (int) PK
- Name (string)
- Address (string)
- ZIP (int)
- City (string)
- Country (string)

### Customer

- CustomerID (int) PK
- FirstName (string)
- LastName (string)
- Address (string)
- ZIP (int)
- City (string)
- Country (string)

### UserRole:

- UserID (int) PK
- UserRoleID (int) FK
- Password (hash)
- Email (string)

### Roles:

- Staff - ima pravico pregleda nad podatki skladišč in evidenc, vendar jih
  jih nima pravice urejati, brisati ali ustvarjati, razen vnašanje novih izdelkov
  in beleženje novih pošiljk v evidence.
- Manager - za razliko od Staff (delavcev) lahko Manager spreminja vse v skladiščih, razen ustvarja profile za nove zaposlene.
- Administrator - vse pravice, kar pomeni, da lahko ustvarja tudi profile za nove zaposlene.

### Opis delovanja

Za uporabo spletne aplikacije mora uporabnik imeti ustvarjen uporabniški račun.
Tega lahko ustvari le administrator aplikacije. Ko se uporabnik prijavi v aplikacijo, lahko v meniju izbira, katere informacije bo pregledoval in potencialno tudi urejal. Izbira lahko med Strankami, Distributerji, Izdelki, Skladišči in pa Evidenco. Administrator pa ima na izbiro še Ustvarjanje novega uporabnika.

- Vsaka vsaka izmed teh strani ima izpis teh entitet, po katerih lahko uporabnik brska z iskanjem nizov, straneh, ali pa jih sortira glede na lastnosti. Ogleda si lahko tudi detajle, in glede na pravice tudi kreira, ureja ali briše le te.

Mobilno aplikacija je na voljo vsem zaposlenim. Z njo pa lahko v bazo vnašajo nove izdelke, jih skenirajo in izvažajo. Aplikacija prav tako omogoča pregled nad vsemi izdelki, ali v posamezenm skladišču, skladisšči ter strankami.

- Skeniranje izdelkov poteka s skeniranjem QR kod, ki so nič drugega kot pa samo številka izdelka.

### Delitev dela

#### Timotej:
- Zasnova delovanja spletne aplikacije
- Izdelovanje modelov, controllerjev in view-ov
- Kreiranje PB, migracij in začetne populacije baze
- Implementacija funkcionalnosti (npr. Export)
- Postavitev aplikacije v oblaku (https://warehouse-is.azurewebsites.net/)
- Mobilna aplikacija

#### Gregor:
- Dodajanje funkcionalnosti v controllerju (strani, search, filter,...)
- UI Spletna Aplikacija
- UI Mobilna Aplikacija
- Dokumentacija
- Povezovanje controller funkcionalnosti z viewi (strani, search, filter,...)

## Diagram podatkovne baze
  ![image](https://user-images.githubusercontent.com/65243657/149382999-9f2af741-9cb7-4c04-82aa-156de021775b.png)



## Pomembno

Ob pullih ne pozabi na [DB info](https://github.com/GreTimotej/warehouse/blob/62686aee7fdde8f007f60f228761ee8d681aefa1/web/appsettings.json#L10).
