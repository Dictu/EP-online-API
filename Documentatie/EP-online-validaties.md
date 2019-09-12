# Businessrules registratiebestand EP-online

**Versiebeheer**

| Datum | Versie  | Auteur  | Wijzigingen
| ------------:  |	:------------:	|	:------------	|	:-----------------------------------------------
| 6 aug 2019	|	0.1	|	Peter Zaal	|	Initiële versie.
|	|	0.2	|	Peter Zaal	|	Foutjes eruit gehaald.
|	9 sep 2019	|	0.3	|	Peter Zaal	|	Bijgewerkt met uitgesplitste SurveyDate rules.
|	12 sep 2019	|	0.4	|	Piet Vredeveld	|	Omgezet naar Markdown formaat, EP-online interne informatie verwijderd.

## Validaties
Het valideren en verwerken van het registratiebestand gebeurd in een aantal stappen. Als er validaties in een stap niet voldoen, wordt de betreffende bijbehorende melding(en) gegeven en niet verder gegaan naar de volgende stap.

## 1. Bepalen versienummer en rekenmethodiek
|	Technische naam	|	Rule(s)
|	:------------------------	| :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|	CalculationType	|	TypeCalculation, Versie, MainBuildingClass en BronGebouwgegevens worden uitgelezen. Aan de hand hiervan wordt bepaald wat de rekenmethodiek is. De rekenmethodiek moet bestaan in EP-online.
|	CalculationTypeVersion	|	De XSD versie bij de rekenmethodiek moet bestaan in EP-online.
|	MonitoringVersion	|	Element *EPMeta.Version* moet bestaan en een geldig getal zijn; dit bepaalt de monitorbestandversie.
|	GenericXmlVersion	|	In de generieke XML moet het veld *Energieprestatie.Versie* gevuld zijn, een geldig getal zijn, en overeenkomen met de generieke XML versie in EP-online.

## 2. XSD validatie
| Technische naam	| Rule(s)
| :---------------------	| :-----------------------------------------------------------------------------------------------------
| MainSchemeValidator	| De XML wordt gevalideerd tegen de monitor XSD (exclusief het element *SurveySourceData* )
| SurveyDataValidator	| Het element *SurveySourceData* moet bestaan en wordt gevalideerd tegen de XSD van het generieke deel.

## 3. Inhoudelijke controles
Onderstaande validaties worden allemaal en in willekeurige volgorde uitgevoerd. Indien er meerdere validaties falen, dan worden alle bijbehorende foutmeldingen geretourneerd.

| Technische naam 	| Rule(s)
| :-------------------------------------------------------------	| :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|  CheckBuildingIdentifiedByBagHasUniqueBagIds  	|  Alle pand-id's (BagBuildingId) mogen slecht één keer voorkomen in het bestand.
|  CheckBuildingObservation  	|  Opname heeft plaatsgevonden in het gebouw (BuildingObservation = Yes).
|  CheckBuildingType  	|  Bij woningbouw: bij elk gebouw moet het gebouwtype (BuildingCategory) ingevuld zijn. 
|	| Bij utiliteitsbouw: bij een gebouw mag zowel het gebouwtype (BuildingCategory) als het subtype (BuildingCategorySupplement) niet ingevuld zijn.
|  CheckBuildingTypeSupplementNta  	|  Alleen voor woningbouw: Voor gebouwtypes (BuildingCategory) 4, 5, 6, 7 moet het subtype (BuildingCategorySupplement) gevuld zijn en een waarde van 1 t/m 8 bevatten. 
|	| Bij andere gebouwtypes mag het subtype (BuildingCategorySupplement) niet gevuld zijn.
|  CheckBuildingUseTypes  	|  Alleen voor utiliteitsbouw: voor alle UseTypes (PrimaryUse en SecondaryUse's) moet het Percentage > 0 zijn en opgeteld tussen 0 en 100 (inclusief) liggen.
|  CheckCalculationType  	|  De rekenmethodiek (TypeCalculation) moet bestaan in EP-online, niet geblokkeerd zijn, en de gebouwklasse moet overeenkomen met de MainBuildingClass in de XML.
|	| NB: voor de NTA_8800 rekenmethodiek wordt deze door code door de webservice aangevuld met de indicatie basis/detailopname en indicatie woningbouw/utiliteitsbouw (in EP-online bestaan er dus 4 NTA-8800 rekenmethodieken).
|  CommercialAndCompoundBuildingsAreIdentyfiedByBagId  	|  Bij  utiliteitsbouw of bij een appartementencomplex (Scope = compound) mag geen enkel gebouw met alleen een adres (TPGIdentification) geïdentificeerd zijn (moet dus met VBO-Id(s) geïdentificeerd zijn).
|  CompoundBuildingHasNoCommercialBuildingClass  	| De combinatie van utiliteitsbouw en een appartementencomplex is niet toegestaan.
|  CheckConstructionAndRenovationYearNta  	| Als de status niet "Aanvraag omgevingsvergunning" is, dan mogen het bouwjaar (ConstructionYear) en het jaar van renovatie (YearOfRenovation) niet in de toekomst liggen.
|  CheckContractor  	|  De contractor (degene die registreert) moet bestaan als gebruiker in EP-online, de rol contracthouder (CH) hebben en gemachtigd zijn voor registraties met de rekenmethodiek (TypeCalculation) in EP-online.
|  CheckDuplicateAddresses  	|  Alle adressen (ZipCode+Number+Extension+BuildingAnnotation uit TPGIdentification) mogen slechts één keer voorkomen in het bestand.
|  CheckDuplicateBagResidenceIds  	|  Alle verblijfsobjecten Id's (BagResidenceId) mogen slecht één keer voorkomen in het bestand.
|  CheckDuplicateImprovement  	|  Alle verbetermaatregelen (AdjustmentId) mogen slecht één keer voorkomen in het bestand.
|  CheckEpcVersion  	|  Het versienummer van het monitorbestand moet geldig zijn op moment van registratie.
|  CheckMainBuildingCertificateByBag  	|  Wanneer bij woningbouw voor het opnamegebouw (MainBuilding) een VBO-Id (BAGIdentification) is opgegeven en er referentiewoningen (ReferenceBuildingList) zijn meegegeven: indien er op de opnamedatum (SurveyDate) al een certificaat is voor het (eerste) VBO-Id van het opnamegebouw, dan moet het versienummer van dit certificaat gelijk of hoger zijn dan die in het registratiebestand.
|  CheckMainBuildingCertificateByTpg  	|  Wanneer bij woningbouw voor het opnamegebouw (MainBuilding) een adres (TPGIdentification) is opgegeven en er referentiewoningen (ReferenceBuildingList) zijn meegegeven: indien er op de opnamedatum (SurveyDate) al een certificaat is  voor het adres van het opnamegebouw, dan moet het versienummer van dit certificaat gelijk of hoger zijn dan die in het registratiebestand.
|  CheckMainBuildingUse  	|  Bij utiliteitsbouw moet het primaire gebruik (MainBuildingUse.PrimaryUse) zijn opgegeven. Bij woningbouw mag het primaire gebruik juist niet zijn opgegeven.
|  CheckNumberOfDwellingsNta  |  Het aantal wooneenheden (NumberOfDwellings) moet bij utiliteitsbouw 0 zijn en bij woningbouw 1 of hoger.
|  CheckObjectLocationIsSpecified  |  Elk gebouw moet minimaal met één BAG-id (BAGIdentification) of adres (TPGIdentification) worden geïdentificeerd.
|  CheckSoftwareTool  |  De naam (VendorSoftwareKey) en versienummer (VendorSoftwareVersionId) van de softwaretool moet ingevuld zijn, bestaan als SoftwareTool in EP-online, en daar geldig (actief) zijn op datum van registratie (huidige datum). Het versienummer van het registratiebestand (Version) moet overeenkomen met de XSD versie van de softwaretool in EP-online. De gebruikte rekenmethodiek (TypeCalculation) moet geldig (aangevinkt) zijn bij de softwaretool in EP-online.
|  CheckSpecificBuildingHasOneBagResidenceId |  Bij een enkel verblijfsobject (Scope = specific) dan moet er ook exact één VBO-Id (BagResidenceId) zijn ingevuld.
|  CheckStatus  	|  Bij utiliteitsbouw mag de status niet "Woningwaarderingsstelsel" zijn.
|  CheckSurveyDateHasValue  	|  De opnamedatum (SurveyDate) moet ingevuld zijn.
|  CheckSurveyDateNotInFuture |  De opnamedatum (SurveyDate) mag niet in de toekomst liggen.
|  CheckSurveyDateNta |  De opnamedatum (SurveyDate) moet binnen de afmeldperiode voor NTA labels liggen.

## 4. BAG controle

Voor elk adres (VBO-id of PHT) wordt gecontroleerd of deze valide is. De validaties zijn afhankelijk of de identificatie d.m.v. een VBO-id en/of Pand-Id is (BAGIdentification is gevuld), d.m.v. een adres is (TPGIdentification is gevuld).

|  Situatie  	|  Rule(s)
|--------------------------------------------------------------------------------------- |:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|  TPGIdentification  |  Het object (op basis van adres) moet gevonden worden in de BAG. Indien er meerdere adressen in de BAG gevonden worden, worden degene met het meest recente bouwjaar genomen. Er moet uiteindelijk precies één resultaat zijn.
|  BAGIdentification	|  De objecten (op basis van VBO-id en evt. alle Pand-Id’s) moeten gevonden worden in de BAG.
|  | Het VBO-Id uit het registratiebestand moet voorkomen in het resultaat van de BAG.
|  | De Pand-Id’s uit het registratiebestand moeten voorkomen in het resultaat van de BAG.

## 5. Controle op recenter certificaat

Voor elk gebouw wordt gecontroleerd dat er niet al een recenter certificaat aanwezig is op basis van 
- het opgegeven adres (in geval van identificatie d.m.v. TPGIdentification), of 
- het gevonden adres van het verblijfsobject via de BAG (in geval van identificatie d.m.v. BAGIdentification).

De controle wordt niet uitgevoerd voor dwangsomregistraties (dit zijn gewoonlijk juist registraties in het verleden door de oude eigenaar waarbij het niet uitmaakt of er al een recenter certificaat, bijvoorbeeld door de nieuw eigenaar, is geregistreerd).

|  Technische naam	|	Rule(s)
|	---------------------	|	:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|	CheckNoneMoreRecent	|	Opnamedatum moet ingevuld zijn.
|	|	Adres moet bekend of gevonden zijn.
|	|	Er mag nog geen geldig certificaat zijn (Geldig t/m is nog actueel) voor het adres met een opnamedatum die ligt na de opnamedatum (SurveyDate) uit het registratiebestand.

## 6. Controle op de actie

Controle of de actie 'Toevoegen', 'Vervangen' of 'Uitbreiden' is toegestaan. Bij Uitbreiden wordt gekeken of de situatie 'Uitbreiden' of 'UitbreidenExtra' betreft en daarop de validaties uitgevoerd.

|  Actie  |  Rule(s)
|------------------|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|  Toevoegen  |  Op de opnamedatum (SurveyDate) mag er niet al een certificaat zijn met dezelfde adressen van het opnamegebouw.
|  Vervangen  |  Op de opnamedatum (SurveyDate) moet er al een certificaat zijn met dezelfde adressen van het opnamegebouw.
|  | De huidige datum moet in de geldigheidsperiode liggen van het bestaande certificaat (tussen Opnamedatum en Geldig tot). 
|  | Het huidige certificaat moet geregistreerd zijn voor de huidige ingelogde gebruiker van EP-online of de webservice. 
|  | De registratiedatum van het bestaande certificaat ligt binnen de 'Vervangperiode' (enkele dagen), of de ingelogde gebruiker heeft tijdelijk het recht gekregen om een certificaat te vervangen. 
|  Uitbreiden  |  Situatie: Op de opnamedatum (SurveyDate) is er al een certificaat met dezelfde adressen van het opnamegebouw.  
|  | De rekenmethodiek van het bestaande certificaat is gelijk aan die van het registratiebestand.
|  |  Er worden **geen** gebouwen toegevoegd die nog niet in het bestaande certificaat zitten, maar waarvoor wel al een ander certificaat bestaat.
|  |  Er zijn gebouwen in het registratiebestand die nog niet bij het bestaande certificaat aanwezig zijn.
|  UitbreidenExtra| **Alleen beschikbaar via EP-online website.**
|  |  Situatie: Op de opnamedatum (SurveyDate) is er al een certificaat met dezelfde adressen van het opnamegebouw.
|  |  De rekenmethodiek van het bestaande certificaat is gelijk aan die van het registratiebestand.
|  |  Er worden **wel** gebouwen toegevoegd die nog niet in het bestaande certificaat zitten, maar waarvoor wel al een ander certificaat bestaat.
