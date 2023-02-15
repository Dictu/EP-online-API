# Businessrules registreren maatwerkadvies in EP-online

## 1. Versiebeheer
|   Datum 		| Versie |  Auteur  		 |  Wijzigingen
|--------------:|:------:|:-----------------:|:----------------------------------------------------------------------------
|   15 feb 2023	|  0.1	 |	Paul Kamps		 |	Initiële versie.

## 2. Validaties
Het valideren en verwerken van het maatwerkadvies gebeurt in een aantal stappen. Als er een of meerdere validaties in een stap niet voldoen, worden de bijbehorende meldingen terug gegeven en wordt niet verder gegaan naar de volgende stap.

### 2.1. Ophalen en valideren gebruiker (adviseur) en organisatie
Er wordt gevalideerd dat de gegevens gevonden kunnen worden.

### 2.2. Bepalen versienummer
|	Rule(s)
|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|	Element *TAMeta.Version* moet bestaan en een geldig getal zijn; de versie moet bekend zijn binnen EP-Online als maatwerkadvies versie.

### 2.3. XSD validatie
| Rule(s)
|:-----------------------------------------------------------------------------------------------------
| De XML wordt gevalideerd tegen de corresponderende XSD versie

### 2.4. Inhoudelijke controles
Onderstaande validaties worden allemaal en in willekeurige volgorde uitgevoerd. Indien er meerdere validaties falen, dan worden alle bijbehorende foutmeldingen geretourneerd.

| Technische naam 																| Rule(s)
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

|  Situatie  							|  Rule(s)
|---------------------------------------|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|  BagResultCheckBagHasMatchingIds		|  Als het ingevulde verblijfsobject id niet behoort tot het ingevulde pand id, dan dient de BuildingAnnotation leeg te zijn.
|  BagResultCheckValidateBerthIds		|  Het opgegeven ligplaats id dient overeen te komen met het ligplaats id dat terug is gekomen vanuit de BAG.
|  BagResultCheckValidateBuildingIds	|  De opgegeven pand id’s dienen overeen te komen met de pand id’s die terug zijn gekomen vanuit de BAG.
|  BagResultCheckValidatePitchIds		|  Het opgegeven standplaats id dient overeen te komen met het standplaats id dat terug is gekomen vanuit BAG.
|  BagResultCheckValidateResidenceIds	|  De opgegeven verblijfsobject id’s dienen overeen te komen met de verblijfsobject id’s die terug zijn gekomen vanuit de BAG.

### 2.6. Controle op recenter maatwerkadvies
Voor elk adres wordt gecontroleerd dat er niet al een recenter maatwerkadvies aanwezig is op basis van het gevonden adres van het verblijfsobject via de BAG.

|	Rule(s)
|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|	Er mag geen maatwerkadvies registratie worden gevonden op hetzelfde adres met dezelfde gebouwklasse (woningbouw of utiliteitsbouw) met een recentere adviesdatum.

### 2.7. Controle op de actie
Controle of de actie 'Toevoegen' betreft en daarop de validaties uitgevoerd.

|  Actie  			|  	Rule(s)
|------------------	|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|	Toevoegen		|	Op de adviesdatum (AdviceDate) mag er niet al een maatwerkadvies zijn met dezelfde gebouwklasse, waarbij een van de adressen overeenkomt met een adres van uit het registratiebestand. 