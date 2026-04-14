# Businessrules registreren maatwerkadvies in EP-online

## 1. Versiebeheer
|   Datum 		| Versie |  Auteur  		 |  Wijzigingen
|--------------:|:------:|:-----------------:|:----------------------------------------------------------------------------
|   15 feb 2023	|  0.1	 |	Paul Kamps		 |	Initiële versie.
|   14 apr 2026	|  1.0	 |	Paul Kamps  	 |	Document bijgewerkt voor Maatwerkadvies xsd 2.0; Toegevoegd: validaties voor Vervangen actie en CRT controle.

## 2. Validaties
Het valideren en verwerken van het maatwerkadvies gebeurt in een aantal stappen. Als er een of meerdere validaties in een stap niet voldoen, worden de bijbehorende meldingen terug gegeven en wordt niet verder gegaan naar de volgende stap.

**Ondersteunde acties:**
- **Toevoegen**: Een nieuw maatwerkadvies registreren
- **Vervangen**: Een bestaand maatwerkadvies vervangen door een nieuwe versie

### 2.1. Ophalen en valideren gebruiker (adviseur) en organisatie
Er wordt gevalideerd dat de gegevens gevonden kunnen worden.

### 2.2. Bepalen versienummer
|	Regels
|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|	Element *TAMeta.Version* moet bestaan en een geldig getal zijn; de versie moet bekend zijn binnen EP-Online als maatwerkadvies versie.

### 2.3. XSD validatie
| Regels
|:-----------------------------------------------------------------------------------------------------
| De XML wordt gevalideerd tegen de corresponderende XSD versie

### 2.4. Inhoudelijke controles
Onderstaande validaties worden allemaal en in willekeurige volgorde uitgevoerd. Indien er meerdere validaties falen, dan worden alle bijbehorende foutmeldingen geretourneerd.

| Technische naam 																| Regels
|-------------------------------------------------------------------------------|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|  CheckAdviceDateNotInFuture													|	De opgegeven adviesdatum mag niet in de toekomst liggen.
|  CheckBagBuildingIdMustHaveBagResidenceId										|	Wanneer een pand id is opgegeven, dan is het verplicht om een verblijfsobject id op te geven.
|  CheckBuildingIdentificationForIncorrectCharacters							|	Wanneer een BuildingAnnotation is opgegeven dan mag deze naast letters en cijfers alleen de volgende karakters bevatten: "a-z", "A-Z", "0-9", "\", "-", "_", "'", "`", ",".
|  CheckNoDuplicateBAGBerthIds													|	Alle ligplaats id's (BagBerthId) mogen slechts één keer voorkomen in het bestand.
|  CheckNoDuplicateBAGPitchIds													|	Alle standplaats id's (BagPitchId) mogen slechts één keer voorkomen in het bestand.
|  CheckNoDuplicateBAGResidenceIds												|	Alle verblijfsobject id's (BagResidenceId) mogen slechts één keer voorkomen in het bestand.
|  CheckRegistrationAdvisorHasCorrectCalculationType							|	De registratieadviseur dient de correcte bevoegdheid (rekenmethodiek) in EP-Online te hebben om een maatwerkadvies te mogen registreren.
|  CheckSoftwareToolAndCalculationTypeAreValid  								|	De opgegeven softwaretool (in de XML) dient bekend te zijn binnen EP-Online en te corresponderen met de correcte rekenmethodiek. Zowel de softwaretool als de rekenmethodiek dienen actief te zijn.
|  CheckSurveyAndRegistrationNotSameAdvisor										|	Wanneer een registratie wordt gedaan met OpnameEnRegistratieZelfdeAdviseur is 'true', dan mag het blok 'Surveyor' niet gevuld zijn.<br/><br/> Wanneer een registratie wordt gedaan met OpnameEnRegistratieZelfdeAdviseur is 'false', dan mogen 'Name' en 'ExamNumber' in het blok 'Surveyor' niet leeg zijn.

### 2.5. BAG controle
Voor elk adres (verblijfsobject id, ligplaats id of standplaats id in BAGIdentification) wordt gecontroleerd aan de hand van de BAG of deze valide is. 

|  Situatie  							|  Regels
|---------------------------------------|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|  BagResultCheckBagHasMatchingIds		|  Als het ingevulde verblijfsobject id niet behoort tot het ingevulde pand id, dan dient de BuildingAnnotation leeg te zijn.
|  BagResultCheckValidateBerthIds		|  Het opgegeven ligplaats id dient overeen te komen met het ligplaats id dat terug is gekomen vanuit de BAG.
|  BagResultCheckValidateBuildingIds	|  De opgegeven pand id’s dienen overeen te komen met de pand id’s die terug zijn gekomen vanuit de BAG.
|  BagResultCheckValidatePitchIds		|  Het opgegeven standplaats id dient overeen te komen met het standplaats id dat terug is gekomen vanuit BAG.
|  BagResultCheckValidateResidenceIds	|  De opgegeven verblijfsobject id’s dienen overeen te komen met de verblijfsobject id’s die terug zijn gekomen vanuit de BAG.

### 2.6. Controle op recenter maatwerkadvies
Voor elk adres wordt gecontroleerd dat er niet al een recenter maatwerkadvies aanwezig is op basis van het gevonden adres van het verblijfsobject via de BAG.

|	Regels
|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|	Er mag geen maatwerkadvies registratie worden gevonden op hetzelfde adres met dezelfde gebouwklasse (woningbouw of utiliteitsbouw) met een recentere adviesdatum.

### 2.7. Controle op de actie
Controle of de actie 'Toevoegen' of 'Vervangen' betreft en daarop de validaties uitgevoerd.

#### 2.7.1. Actie: Toevoegen

|  Regels
|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| <ul><li>Bij woningbouw registraties: wanneer een detailaanduiding is opgegeven, dient de registratieadviseur over voldoende gebouwidentificatie credits te beschikken. Het aantal benodigde credits is gelijk aan het aantal ObjectLocations waarbij de BuildingAnnotation is ingevuld.</li><li>Alle adressen in het bestand (op basis van postcode, huisnummer, huisletter, huisnummertoevoeging en detailaanduiding) mogen slechts één keer voorkomen.</li><li>Op de adviesdatum (AdviceDate) mag er niet al een maatwerkadvies zijn met dezelfde gebouwklasse, waarbij een van de adressen overeenkomt met een adres uit het registratiebestand.</li></ul>

#### 2.7.2. Actie: Vervangen

|  Regels
|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| <ul><li>Alle adressen in het bestand (op basis van postcode, huisnummer, huisletter, huisnummertoevoeging en detailaanduiding) mogen slechts één keer voorkomen.</li><li>Er moet een bestaande maatwerkadvies registratie gevonden worden met dezelfde gebouwklasse en waarbij een van de adressen overeenkomt met een adres uit het registratiebestand.</li><li>De adressen in het registratiebestand moeten exact overeenkomen met de adressen van de initiële registratie. Het aantal adressen moet gelijk zijn en alle adressen moeten voorkomen in de initiële registratie.</li><li>De bestaande registratie moet voldoen aan de volgende voorwaarden:<ul><li>De uitvoerende registratieadviseur moet gelijk zijn aan de laatste registratieadviseur van de bestaande registratie</li><li>De rekenmethodiek (CalculationType) moet gelijk zijn aan de rekenmethodiek van de bestaande registratie</li><li>De bestaande registratie mag niet ingetrokken zijn</li><li>De adviesdatum van de bestaande registratie mag niet in de toekomst liggen</li></ul></li><li>De vervangingsperiode moet geldig zijn volgens de volgende regels:<ul><li>Als de reguliere vervangingsperiode (gerekend vanaf de originele adviesdatum) nog niet verstreken is, is vervangen toegestaan</li><li>Als de reguliere vervangingsperiode verstreken is, moeten er vervangrechten zijn toegekend</li><li>Als er vervangrechten zijn toegekend, moet de einddatum van deze rechten nog niet verstreken zijn</li></ul></li></ul>