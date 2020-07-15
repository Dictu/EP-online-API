# Businessrules registratiebestand EP-online

**1. Versiebeheer**

| Datum 		| Versie| Auteur  			| Wijzigingen
| ------------: |	:------------:			|	:------------	|	:-----------------------------------------------
|	6 aug 2019	|	0.1	|	Peter Zaal		|	Initiële versie.
|				|	0.2	|	Peter Zaal		|	Foutjes eruit gehaald.
|	9 sep 2019	|	0.3	|	Peter Zaal		|	Bijgewerkt met uitgesplitste SurveyDate rules.
|	12 sep 2019	|	0.4	|	Piet Vredeveld	|	Omgezet naar Markdown formaat, EP-online interne informatie verwijderd.
|	24 jan 2020	|	0.5 |	Paul Kamps		|	Nieuwe validaties en checks toegevoegd.
|	07 jul 2020 |	0.6 |	Bas Brouwer  	| Geüpdatet aan de hand van de nieuwe XSD versie.
|	10 jul 2020 |	0.6 |	Paul Kamps  	| Business Rules geüpdatet en toegevoegd.

## 2. Validaties
Het valideren en verwerken van het registratiebestand gebeurt in een aantal stappen. Als er een of meerdere validaties in een stap niet voldoen, worden de betreffende bijbehorende meldingen gegeven en niet verder gegaan naar de volgende stap.

## 2.1. Bepalen versienummer en rekenmethodiek
|	Technische naam				|	Rule(s)
|	:------------------------	| :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|	CalculationType				|	TypeCalculation, Versie, MainBuildingClass en Opnametype worden uitgelezen. Aan de hand hiervan wordt bepaald wat de rekenmethodiek is. De rekenmethodiek moet bestaan in EP-online.
|	CalculationTypeVersion		|	De XSD versie bij de rekenmethodiek moet bestaan in EP-online.
|	MonitoringVersion			|	Element *EPMeta.Version* moet bestaan en een geldig getal zijn; dit bepaalt de monitorbestandversie.
|	GenericXmlVersion			|	In de generieke XML moet het veld *Energieprestatie.Versie* gevuld zijn, een geldig getal zijn, en overeenkomen met de generieke XML versie in EP-online.

## 2.2 XSD validatie
| Technische naam		| Rule(s)
| :---------------------	| :-----------------------------------------------------------------------------------------------------
| MainSchemeValidator	| De XML wordt gevalideerd tegen de monitor XSD (exclusief het element *SurveySourceData* )
| SurveyDataValidator	| Het element *SurveySourceData* moet bestaan en wordt gevalideerd tegen de XSD van het generieke deel.

## 3. Inhoudelijke controles
Onderstaande validaties worden allemaal en in willekeurige volgorde uitgevoerd. Indien er meerdere validaties falen, dan worden alle bijbehorende foutmeldingen geretourneerd.

| Technische naam 	| Rule(s)
| :-------------------------------------------------------------	| :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|  CheckBagBerthId	|	BagResidenceId, BagBuildingId en BagPitchId mogen niet ingevuld zijn wanneer BagBerthId ingevuld is.
|  CheckBagBuildingIdAndOrResidenceId	|	BagPitchId en BagBerthId mogen niet ingevuld zijn wanneer BagResidenceId en/of BagBuildingId ingevuld is/zijn.
|  CheckBagPitchId	|	BagResidenceId, BagBuildingId en BagBerthId mogen niet ingevuld zijn wanneer BagPitchId ingevuld is.	
|  CheckBuildingIdentifiedByBagHasUniqueBagIds  	|  Alle pand-id's (BagBuildingId) mogen slecht één keer voorkomen in het bestand.
|  CheckBuildingObservation  	|  Opname heeft plaatsgevonden in het gebouw (BuildingObservation = Yes).
|  CheckBuildingType  	|  Bij woningbouw: bij elk gebouw moet het gebouwtype (BuildingCategory) ingevuld zijn. 
|	| Bij utiliteitsbouw: bij een gebouw mag zowel het gebouwtype (BuildingCategory) als het subtype (BuildingCategorySupplement) niet ingevuld zijn.
|  CheckBuildingTypeSupplement  	|  Alleen voor woningbouw: Voor gebouwtype (BuildingCategory) 7 moet het subtype (BuildingCategorySupplement) gevuld zijn en een waarde van 1 t/m 8 bevatten. 
|	| Bij andere gebouwtypes mag het subtype (BuildingCategorySupplement) niet gevuld zijn.
|  CheckBuildingUseTypes  	|  Alleen voor utiliteitsbouw: voor alle UseTypes (PrimaryUse en SecondaryUse's) moet het Percentage > 0 zijn en opgeteld tussen 0 en 100 (inclusief) liggen.
|  CheckCalculationType  	|  De rekenmethodiek (TypeCalculation) moet bestaan in EP-online, niet geblokkeerd zijn, en de gebouwklasse moet overeenkomen met de MainBuildingClass in de XML.
|	| NB: voor de NTA_8800 rekenmethodiek wordt deze door code door de webservice aangevuld met de indicatie basis/detailopname en indicatie woningbouw/utiliteitsbouw (in EP-online bestaan er dus 4 NTA-8800 rekenmethodieken).
|  CommercialAndCompoundBuildingsAreIdentyfiedByBagId  	|  Bij  utiliteitsbouw of bij een appartementencomplex (Scope = compound) mag geen enkel gebouw met alleen een adres (TPGIdentification) geïdentificeerd zijn (moet dus met VBO-Id(s) geïdentificeerd zijn).
|  CheckConstructionAndRenovationYear  	| Als de status niet "Aanvraag omgevingsvergunning" is, dan mogen het bouwjaar (ConstructionYear) en het jaar van renovatie (YearOfRenovation) niet in de toekomst liggen.
|  CheckContractor  	|  De contractor (degene die registreert) moet bestaan als gebruiker in EP-online, de rol contracthouder (CH) hebben en gemachtigd zijn voor registraties met de rekenmethodiek (TypeCalculation) in EP-online.
|  CheckDuplicateAddresses  	|  Alle adressen (ZipCode+Number+Extension+BuildingAnnotation uit TPGIdentification) mogen slechts één keer voorkomen in het bestand.
|  CheckDuplicateBagResidenceIds  	|  Alle verblijfsobjecten Id's (BagResidenceId) mogen slecht één keer voorkomen in het bestand.
|  CheckEpcVersion  	|  Het versienummer van het monitorbestand moet geldig zijn op moment van registratie.
|  CheckMainBuildingCertificateByBag  	|  Wanneer bij woningbouw voor het opnamegebouw (MainBuilding) een VBO-Id (BAGIdentification) is opgegeven en er referentiewoningen (ReferenceBuildingList) zijn meegegeven: indien er op de opnamedatum (SurveyDate) al een certificaat is voor het (eerste) VBO-Id van het opnamegebouw, dan moet het versienummer van dit certificaat gelijk of hoger zijn dan die in het registratiebestand.
|  CheckMainBuildingCertificateByTpg  	|  Wanneer bij woningbouw voor het opnamegebouw (MainBuilding) een adres (TPGIdentification) is opgegeven en er referentiewoningen (ReferenceBuildingList) zijn meegegeven: indien er op de opnamedatum (SurveyDate) al een certificaat is  voor het adres van het opnamegebouw, dan moet het versienummer van dit certificaat gelijk of hoger zijn dan die in het registratiebestand.
|  CheckMainBuildingUse  	|  Bij utiliteitsbouw moet het primaire gebruik (MainBuildingUse.PrimaryUse) zijn opgegeven. Bij woningbouw mag het primaire gebruik juist niet zijn opgegeven.
|  CheckMultipleBagBuildingIdsWithMultipleBagResidenceIds	| Bij meerdere Pand-Id’s mogen er niet meerdere VBO-Id’s opgegeven zijn.
|  CheckNotPermitPreNtaStatusCompletionWithBagAndProvisionalIdentification	|	Wanneer de buildingstatus ‘Oplevering’ is, de PermitPreNTA ‘False’ is en enkel de BAGIdentification is gevuld, dient er een overeenkomstige registratie met status 'vergunningsaanvraag' met BAGIdentification te worden gevonden.
|  CheckNotPermitPreNtaStatusCompletionWithBagIdentification	|	Wanneer de buildingstatus ‘Oplevering’ is, de PermitPreNTA ‘False’ is en zowel de BAGIdentification als de ProvisionalIdentification gevuld zijn, dient er een overeenkomstige registratie met status 'vergunningsaanvraag' met de ProvisionalId te worden gevonden.
|  CheckNumberOfDwellings  |  Het aantal wooneenheden (NumberOfDwellings) moet bij utiliteitsbouw 0 zijn en bij woningbouw 1 of hoger.
|  CheckPermitPreNta  |  Wanneer de buildingstatus ‘Oplevering’ is, de PermitPreNTA ‘True’ is dan moet de BAGIdentification worden gevuld en mag de ProvisionalIdentification niet zijn toegevoegd.
|  CheckSoftwareTool  |  De naam (VendorSoftwareKey) en versienummer (VendorSoftwareVersionId) van de softwaretool moet ingevuld zijn, bestaan als SoftwareTool in EP-online, en daar geldig (actief) zijn op datum van registratie (huidige datum). Het versienummer van het registratiebestand (Version) moet overeenkomen met de XSD versie van de softwaretool in EP-online. De gebruikte rekenmethodiek (TypeCalculation) moet geldig (aangevinkt) zijn bij de softwaretool in EP-online.
|  CheckStatusExistingMustHaveBagIdentifcationAndNotProvisionalIdentification |  Bij registratie met status 'bestaand' is het verplicht om subelement BAGIdentification op te nemen en mag subelement ProvisionalIdentification niet worden opgenomen.
|  CheckSurveyDate |  De opnamedatum (SurveyDate) moet binnen de registratie periode voor NTA labels liggen.
|  CheckSurveyDateHasValue  	|  De opnamedatum (SurveyDate) moet ingevuld zijn.
|  CheckSurveyDateNotInFuture |  De opnamedatum (SurveyDate) mag niet in de toekomst liggen.

## 4. BAG controle

Voor elk adres (VBO-id of PHT) wordt gecontroleerd of deze valide is. De validaties zijn afhankelijk of de identificatie d.m.v. een VBO-id en/of Pand-Id is (BAGIdentification is gevuld), d.m.v. een adres is (TPGIdentification is gevuld).

|  Situatie  	|  Rule(s)
|--------------------------------------------------------------------------------------- |:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|  BagResultCheckAddressMustExistbyBag  |  De objecten (op basis van VBO-ID en evt. alle Pand-Id’s) moeten gevonden worden in de BAG.
|  BagResultCheckBagHasMatchingAddress  |  Het ingevulde TPGIdentification adres moet overeenkomen (op basis van adres) met het resultaat uit BAG op basis van de ingevulde BAGIdentification.
|  BagResultCheckValidateBuildingIds	|  De opgegeven Pand-Id’s dienen overeen te komen met de Pand-Id’s vanuit BAG.
|  BagResultCheckValidateResidenceIds	|  De opgegeven VBO-Id’s dienen overeen te komen met de VBO-Id’s vanuit BAG.

## 5. Controle op recenter certificaat

Voor elk gebouw wordt gecontroleerd dat er niet al een recenter certificaat aanwezig is op basis van 
- het opgegeven adres (in geval van identificatie d.m.v. TPGIdentification), of 
- het gevonden adres van het verblijfsobject via de BAG (in geval van identificatie d.m.v. BAGIdentification).

De controle wordt niet uitgevoerd voor dwangsomregistraties (dit zijn gewoonlijk juist registraties in het verleden door de oude eigenaar waarbij het niet uitmaakt of er al een recenter certificaat, bijvoorbeeld door de nieuw eigenaar, is geregistreerd).

|  Technische naam			|	Rule(s)
|	---------------------	|	:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|	CheckNoneMoreRecent		|	Er mag geen PreNTA certificaat gevonden worden, op hetzelfde adres, die voldoet aan: <br/><ul><li>‘Geldig tot’ ligt in de toekomst.</li><br/><li>‘Opnamedatum’ ligt na de opnamedatum (SurveyDate) uit het registratiebestand.</li></ul>
|							|	Er mag geen NTACompliant certificaat gevonden worden, op hetzelfde adres, die voldoet aan: <br/><ul><li>‘Geldig tot’ ligt in de toekomst.</li><br/><li> 'Opnamedatum’ ligt na de opnamedatum (SurveyDate) uit het registratiebestand.</li><br/><li>‘Scope’ heeft dezelfde waarde als ‘Scope’ uit het registratiebestand.</li><br/><li>‘Gebouwklasse’ heeft dezelfde waarde als ‘Gebouwklasse’ uit het registratiebestand.</li></ul>

## 6. Controle op de actie

Controle of de actie 'Toevoegen', 'Vervangen' of 'Uitbreiden' is toegestaan. Bij Uitbreiden wordt gekeken of de situatie 'Uitbreiden' of 'UitbreidenExtra' betreft en daarop de validaties uitgevoerd.

|  Actie  			|  Rule(s)
|------------------	|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|	Toevoegen
|	|	Vergunningsaanvraag
|	|	|	Het is verplicht om minimaal subelement BAGIdentification of subelement ProvisionalIdentification op te nemen. Hierbij mag veld ProvisionalID niet zijn gevuld. Dit geldt voor alle gebouwen uit het registratiebestand.
|	|	Vergunningsaanvraag/Oplevering/bestaand
|	|	|	Op de opnamedatum (SurveyDate) mag er niet al een certificaat zijn met dezelfde scope en gebouwklasse, waarbij een adres overeenkomt met een adres van het opnamegebouw. 
|	Vervangen
|	|	Vergunningsaanvraag
|	|	|	Op de opnamedatum (SurveyDate) dient er al een certificaat te zijn met dezelfde scope en gebouwklasse, waarbij het ProvisionalId overeenkomt met het ProvisionalId van het opnamegebouw. 
Mocht er niks gevonden worden op basis van de ProvisionalIdentification dan wordt er gekeken of op basis van een adres een certificaat gevonden kan worden (zie status Oplevering/bestaand).
|	|	Oplevering/bestaand
|	|	|	Op de opnamedatum (SurveyDate) dient er al een certificaat te zijn met dezelfde scope en gebouwklasse, waarbij een adres overeenkomt met een adres van het opnamegebouw. 
|	|	Vergunningsaanvraag/Oplevering/bestaand
|	|	|	Wanneer er een certificaat gevonden is gelden de volgende regels:
De huidige datum moet in de geldigheidperiode liggen van het bestaande certificaat (tussen Opnamedatum en Geldig tot).
Aanvulende validaties (gelden niet voor Beheerders):
Het huidige certificaat moet geregistreerd zijn door de, huidige ingelogde, gebruiker van de EnergielabelApi.
De registratiedatum van het bestaande certificaat mag slechts een maximaal aantal dagen in het verleden liggen (a.d.h.v. EP-online stamgegevens Registreren - Vervangperiode in dagen), of de ingelogde gebruiker heeft de rol ‘Certificaat Vervanger’ en de verloopdatum van deze rol bij de gebruiker is ingevuld en nog niet verstreken.
|	Uitbreiden
|	|	Situatie met opnamegebouw o.b.v. adres:
|	|	|	Op de opnamedatum (SurveyDate) dient er al een opnamegebouw te bestaan met dezelfde scope en gebouwklasse, met minimaal dezelfde adressen van het opnamegebouw.
Op de opnamedatum (SurveyDate) dient er al een certificaat te zijn met dezelfde scope en gebouwklasse, waarbij een adres overeenkomt met een adres van het opnamegebouw. 
|	|	Situatie met opnamegebouw o.b.v. ProvisionalId:
|	|	|	Op de opnamedatum (SurveyDate) dient er al een opnamegebouw te bestaan met dezelfde scope en gebouwklasse, met minimaal dezelfde adressen van het opnamegebouw.
Op de opnamedatum (SurveyDate) dient er al een certificaat te zijn met dezelfde scope en gebouwklasse, waarbij een adres overeenkomt met een adres van het opnamegebouw. 
|	|	Geldt voor alle situaties:
|	|	|	De rekenmethodiek van het bestaande certificaat is gelijk aan die van het registratiebestand.
Er worden geen gebouwen toegevoegd die nog niet in het bestaande certificaat zitten, maar waarvoor wel al een ander certificaat bestaat.
Er zijn gebouwen in het registratiebestand die nog niet bij het bestaande certificaat aanwezig zijn.
|	UitbreidenExtra| **Dit is momenteel niet mogelijk vanuit de EnergielabelApi**
|	|	Situatie met opnamegebouw o.b.v. adres:
|	|	|	Op de opnamedatum (SurveyDate) dient er al een opnamegebouw te bestaan met dezelfde scope en gebouwklasse, met minimaal dezelfde adressen van het opnamegebouw.

Op de opnamedatum (SurveyDate) dient er al een certificaat te zijn met dezelfde scope en gebouwklasse, waarbij een adres overeenkomt met een adres van het opnamegebouw. 
|	|	Situatie met opnamegebouw o.b.v. ProvisionalId:
|	|	|	Op de opnamedatum (SurveyDate) dient er al een opnamegebouw te bestaan met dezelfde scope en gebouwklasse, met minimaal dezelfde adressen van het opnamegebouw.
Op de opnamedatum (SurveyDate) dient er al een certificaat te zijn met dezelfde scope en gebouwklasse, waarbij een adres overeenkomt met een adres van het opnamegebouw. 
|	|	Geldt voor alle situaties:
|	|	|	De rekenmethodiek van het bestaande certificaat is gelijk aan die van het registratiebestand.
Er worden wel gebouwen toegevoegd die nog niet in het bestaande certificaat zitten, maar waarvoor wel al een ander certificaat bestaat.
|  |  Situatie: Op de opnamedatum (SurveyDate) is er al een certificaat met dezelfde adressen van het opnamegebouw.