# Warehouse Manager

## Člana ekipe:

63190314 Gregor Volčanjk  
63200104 Timotej Gregorič

## Opis teme

Z informacijskim sistemom Warehouse Manager bodo podjetja in uporabniki le tega imeli enostavnejši vpogled na vse informacije in operacije skladišč, ki si jih lastijo, hkrati pa jim to omogoča tudi veliko lažje vodenje skladišč.
Izdelek ni mišljen za stranke podjetja, temveč za določene delavce podjetja. To pomeni pogled na vse od izdelkov v vsakem izmed skladišč, čase dobave različnih dobaviteljev in naročil strank in celotno evidenco vseh preteklih dobav in izvozov produktov.

V informacijski sistem se bodo torej prijavljali le določeni zaposleni v podjetju, ki jim je dodeljena ta pravica. Po prijavi bodo ti lahko v sistem vnašali določene podatke (nova pošiljka v evidenco, sprememba števila nekega izdelka v določenem skladišču, dodajanje izdelkov v skladišče...), ali pa brskali po že vnešenih podatkih.
Predvidene entitete: uporabniki, skladišča, izdelki, stranke, dobavitelji, evidenca.

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

Mobilno aplikacija je na voljo vsem zaposlenim, z njo pa lahko vnašajo v bazo nove izdelke, ter enako funkcijo ogledovanja kot v spletni aplikaciji.

- Mobilna aplikacija ima tudi možnost vnosa izdelkov s skeniranjem določenih QR kod, da delavcem olajša vnos informacij o izdelkih.

## Pomembno

Ob pullih ne pozabi na [DB info](https://github.com/GreTimotej/warehouse/blob/62686aee7fdde8f007f60f228761ee8d681aefa1/web/appsettings.json#L10).
