# warehouse

## Člana ekipe:
63190314 Gregor Volčanjk
63200104 Timotej Gregorič


## Opis teme
Z informacijskim sistemom Storage Manager bodo podjetja in uporabniki le tega imeli enostavnejši vpogled na vse informacije in operacije skladišč, ki si jih lastijo, hkrati pa jim to omogoča tudi veliko lažje vodenje skladišč.
Izdelek ni mišljen za stranke podjetja, temveč za določene delavce podjetja. To pomeni pogled na vse od izdelkov v vsakem izmed skladišč, čase dobave različnih dobaviteljev in naročil strank in celotno evidenco vseh preteklih dobav in izvozov.
 
V informacijski sistem se bodo torej prijavljali le določeni zaposleni v podjetju, ki jim je dodeljena ta pravica. Po prijavi bodo ti lahko v sistem vnašali določene podatke (nova pošiljka v evidenco, sprememba števila nekega izdelka v določenem skladišču,...), ali pa brskali po že vnešenih podatkih.
Ob nizki vsoti pomembnih izdelkov bi se informacije o izdelku prikazale tudi na določenem prostoru v sistemu, kjer bi uporabnika opomnilo, da je treba naročiti novo zalogo tega izdelka. Opozorila bi se prikazala tudi, če bi bilo samo skladišče prepolno. To bi posledično prispevalo tudi k bolj uravnovešenimi zalogami med skladišči.
Predvidene entitete: uporabniki, skladišča, izdelki, stranke, dobavitelji, evidenca.

## Entities:

(main)
Warehouse
Item
Evidence
Distributor
Customer

### Warehouse:
WarehouseID (int) PK
Address (string)
ZIP (int)
City (string)
Country (string)

### Item: 
ItemID(int) PK
Name (string)
Description (string)
Quantity (int)
Active (bool)
WarehouseID (int) FK
CustomerID (int) FK

### Distributor: 
DistributorID (int) PK
Name (string)
Address (string)
ZIP (int)
City (string)
Country (string)

### Evidence:
EvidenceID (int) PK
itemID (int) FK
WarehouseID (int) FK
CustomerID (int) FK
Out (DateTIme)

### UserRole:
UserID (int) PK
UserRoleID (int) FK
Password (hash)
Email (string)

### Roles:
Worker - gleda lahko samo informacije o skladišču v katerem je? spreminja lahko le kvantiteto izdelkov
Manager - spreminja lahko vse v trenutnem skladišču
Administrator - vse pravice?


## Pomembno
Ob pullih ne pozabi na [DB info](https://github.com/GreTimotej/warehouse/blob/62686aee7fdde8f007f60f228761ee8d681aefa1/web/appsettings.json#L10).
