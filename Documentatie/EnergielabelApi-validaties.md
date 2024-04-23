# Businessrules registratiebestand EP-online

## 1. Versiebeheer
| Datum 			| Versie| Auteur  			| Wijzigingen
|:--------------|:-------|:-----------------|:----------------------------------------------------------------------------
|	6 aug 2019	|	0.1	 |	Peter Zaal		|	Initiële versie.
|				|	0.2	 |	Peter Zaal		|	Foutjes eruit gehaald.
|	9 sep 2019	|	0.3	 |	Peter Zaal		|	Bijgewerkt met uitgesplitste SurveyDate rules.
|	12 sep 2019	|	0.4	 |	Piet Vredeveld	|	Omgezet naar Markdown formaat, EP-online interne informatie verwijderd.
|	24 jan 2020	|	0.5  |	Paul Kamps		|	Nieuwe validaties en checks toegevoegd.
|	07 jul 2020	|	0.6  |	Bas Brouwer  	| 	Geüpdatet aan de hand van de nieuwe XSD versie.
|	10 jul 2020	|	0.7  |	Paul Kamps  	| 	Business Rules geüpdatet en toegevoegd.
|	17 jul 2020	|	0.8  |	Bas Brouwer  	| 	Business Rules geüpdatet en vier business rules toegevoegd.
|	09 sep 2020	|	0.9  |	Paul Kamps  	| 	Standplaats, Ligplaats en Projectgegevens toegevoegd.
|	18 nov 2020	|	1.0  |	Paul Kamps		|	Business Rule geüpdatet.
|	30 nov 2020	|	1.1	 |	Paul Kamps		|	Business Rules voor labelletter toegevoegd.
|	14 dec 2020	|	1.2  |	Robert Boonstra	|	Bag controle voor BuildingAnnotation toegevoegd.
|	13 jan 2021	|	1.3  |	Robert Boonstra	|	Business Rules voor BuildingCategory toegevoegd.
|	13 jan 2021	|	1.4  |	Bas Brouwer		|	Business Rule verwijderd.
|	21 jun 2021	|	1.5  |	Mischa Vreeburg	|	Business Rules aangepast en typo gefixt.
|	09 sep 2021	|	1.6	 |	Paul Kamps		|	Business Rule toegevoegd.
|	17 sep 2021	|	1.7	 |	Paul Kamps		|	Business Rule toegevoegd.
|	25 mrt 2022	|	1.8	 |	Paul Kamps		|	Business Rule toegevoegd.
|	28 apr 2022	|	1.9	 |	Paul Kamps		|	Controle bij vervangen toegevoegd.
|	03 mei 2022	|	1.10 |	Paul Kamps		|	Business Rule verwijderd.
|	07 jun 2022	|	1.11 |	Paul Kamps		|	Business Rule toegevoegd.
|	01 jul 2022	|	1.12 |	Bas Brouwer		|	Business Rule geüpdatet.
|	06 jul 2022 |	1.13 | 	Paul Kamps		|	Business Rule toegevoegd.
|	21 sep 2022	|	1.14 |	Paul Kamps		|	Business Rule verwijderd.
|	15 dec 2022	|	1.15 |	Paul Kamps		|	Business Rule CheckProjectNameAndObjectDoesNotAlreadyExists toegevoegd, Business Rule CheckEpcVersion is verwijderd.
|	23 jan 2023	|	1.16 |	Paul Kamps		|	Business Rules CheckEpcVersion, CheckInterpunction en CheckLinkVergunningsAanvraagToOpleveringSameScope toegevoegd. De naam van Business Rules CheckRegistrationAdvisor en CheckEpClassIdResidential zijn gewijzigd.
|   10 mei 2023 |   1.17 |	Paul Kamps		|	Business Rule CheckBuildingTypeBagPitch bijgewerkt om meer gebouwtypes te ondersteunen.
|	1  aug 2023 |	1.18 |  Paul Kamps		|	Business Rule CheckExpandNotAllowedForProjects toegevoegd. Business rule CheckProjectNameAndProjectObjectCannotChangeWhenExpanding verwijderd.
|	6  sep 2023	|	1.19 |	Paul Kamps		|	Business rules CheckLinkVergunningsAanvraagToOpleveringSameScope, CheckNotPermitPreNtaStatusCompletionWithBagAndProvisionalIdentification, CheckNotPermitPreNtaStatusCompletionWithBagIdentification en CheckPermitPreNtaNoProvisionalIdentification verwijderd.
|	10 apr 2024 |	1.20 |	Paul Kamps		| 	Business Rule CheckSurveyorFieldLengths toegevoegd.
|	23 apr 2024	|	1.21 |	Paul Kamps		|	Business Rule CheckSurveyDate toegevoegd. Hoofdstuk '2.7. Controle op de actie': Herlabelen toegevoegd.

## 2. Validaties
Het valideren en verwerken van het registratiebestand gebeurt in een aantal stappen. Als er een of meerdere validaties in een stap niet voldoen, worden de bijbehorende meldingen terug gegeven en wordt niet verder gegaan naar de volgende stap.

### 2.1. Ophalen en valideren gebruiker (adviseur) en organisatie
Er wordt gevalideerd dat de gegevens gevonden kunnen worden.

### 2.2. Bepalen versienummer en rekenmethodiek
|	Technische naam				|	Rule(s)
|:------------------------------|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|	CalculationType				|	TypeCalculation, Versie, MainBuildingClass en Opnametype worden uitgelezen. Aan de hand hiervan wordt bepaald wat de rekenmethodiek is. De rekenmethodiek moet bestaan in EP-online.
|	CalculationTypeVersion		|	De XSD versie bij de rekenmethodiek moet bestaan in EP-online.
|	MonitoringVersion			|	Element *EPMeta.Version* moet bestaan en een geldig getal zijn; dit bepaalt de monitorbestandversie.
|	GenericXmlVersion			|	In de generieke XML moet het veld *Energieprestatie.Versie* gevuld zijn, een geldig getal zijn, en overeenkomen met de generieke XML versie in EP-online.

### 2.3. XSD validatie
| Technische naam		| Rule(s)
|:----------------------|:-----------------------------------------------------------------------------------------------------
| MainSchemeValidator	| De XML wordt gevalideerd tegen de monitor XSD (exclusief het element *SurveySourceData* )
| SurveyDataValidator	| Het element *SurveySourceData* moet bestaan en wordt gevalideerd tegen de XSD van het generieke deel.

### 2.4. Inhoudelijke controles
Onderstaande validaties worden allemaal en in willekeurige volgorde uitgevoerd. Indien er meerdere validaties falen, dan worden alle bijbehorende foutmeldingen geretourneerd.

| Technische naam 																| Rule(s)
|:---------------------------------------|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|  CheckBagBuildingId															|	Het is verplicht een verblijfsobject id op te geven wanneer er een pand id is opgegeven.
|  CheckBagIdCombination														|	<ul><li> standplaats id en ligplaats id mogen niet ingevuld zijn wanneer verblijfsobject id en/of pand id ingevuld is/zijn.</li><li> verblijfsobject id, pand id en standplaats id mogen niet ingevuld zijn wanneer ligplaats id ingevuld is.</li><li> verblijfsobject id, pand id en ligplaats id mogen niet ingevuld zijn wanneer standplaats id ingevuld is.</li></ul>
|  CheckBagIdentificationForStatusCompletion									|	Bij Status ‘Oplevering’ en een ingevuld provisional identificatie, moet er een BAG identificatie ingevuld zijn.
|  CheckBagOrProvisionalIdentification											|	Het is verplicht om voor ieder gebouw minimaal een identificatie via BAG of een provisional identificatie op te geven.
|  CheckBuildingIdentifiedByBagHasUniqueBagIds  								|	Alle pand id's (BagBuildingId) mogen slechts één keer voorkomen in het bestand.
|  CheckBuildingObservation  													|	Opname heeft plaatsgevonden in het gebouw (BuildingObservation = Yes).
|  CheckBuildingType  															|	Bij woningbouw: bij elk gebouw moet het gebouwtype (BuildingCategory) ingevuld zijn. <br/> Bij utiliteitsbouw: bij een gebouw mag zowel het gebouwtype (BuildingCategory) als het subtype (BuildingCategorySupplement) niet ingevuld zijn.
|  CheckBuildingTypeBagBerth													|	Bij gebruik van BagBerthId zijn enkel gebouwtype (BuildingCategory) 13 (Woonboot bestaande ligplaats) en 15 (Woonboot nieuwe ligplaats) toegestaan.
|  CheckBuildingTypeBagPitch													|	Bij gebruik van BagPitchId zijn enkel gebouwtypes 1 (Vrijstaande woning), 2 (Rijwoning hoek), 3 (Rijwoning tussen), 12 (Twee-onder-één-kap), 14 (Woonwagen) en 16 (Logieswoning) toegestaan.
|  CheckBuildingTypeSupplement  												|	Alleen voor woningbouw:<br/><br/> <b>Scope is specific:</b><br/>Voor gebouwtype (BuildingCategory) 7 moet het subtype (BuildingCategorySupplement) gevuld zijn en een waarde van 1 t/m 8 bevatten. <br/> Bij andere gebouwtypes mag het subtype (BuildingCategorySupplement) niet gevuld zijn.<br/><br/><b>Scope is compound:</b><br/> Voor gebouwtype (BuildingCategory) 7 moet het subtype (BuildingCategorySupplement) niet gevuld zijn.
|  CheckBuildingUseTypes  														|	Alleen voor utiliteitsbouw: voor alle UseTypes (PrimaryUse en SecondaryUse's) moet het Percentage > 0 zijn en opgeteld tussen 0 en 100 (inclusief) liggen.
|  CheckCalculationType  														|	De rekenmethodiek (TypeCalculation) moet bestaan in EP-online, niet geblokkeerd zijn, en de gebouwklasse moet overeenkomen met de MainBuildingClass in de XML. <br/> <b>NB:</b> voor de NTA_8800 rekenmethodiek wordt deze door code door de webservice aangevuld met de indicatie basis/detailopname en indicatie woningbouw/utiliteitsbouw (in EP-online bestaan er dus 4 NTA-8800 rekenmethodieken).
|  CheckConstructionAndRenovationYear  											|	Als de status niet "Vergunningsaanvraag" is, dan mogen het bouwjaar (ConstructionYear) en het jaar van renovatie (YearOfRenovation) niet in de toekomst liggen.
|  CheckRegistrationAdvisor														|	De registratie adviseur moet bestaan als gebruiker in EP-online, de rol adviseur hebben en gemachtigd zijn voor registraties met de rekenmethodiek (TypeCalculation) in EP-online.
|  CheckDetailForStatusOpleveringAndVergunningsAanvraag							|	Bij Status ‘Vergunningsaanvraag’ en bij Status ‘Oplevering’ moet het soort opname 'Detail' zijn.
|  CheckDuplicateAddresses  													|	Alle adressen (ZipCode+Number+Letter+Addition+BuildingAnnotation uit TPGIdentification) mogen slechts één keer voorkomen in het bestand.
|  CheckEpClassIdResidential													|	Bij een registratie voor woningbouw is de hoogst haalbare labelletter A++++. 
|  CheckExpandNotAllowedForProjects												|	Een uitbreiding van Status ‘Vergunningsaanvraag’ is niet toegestaan. 
|  CheckEpcVersion																|	De gebruikte versie moet geldig zijn.
|  CheckInterpunction															|	Voor Projectnaam, ProjectObject en BuildingAnnotation mogen de volgende karakters gebruikt worden: "a-z", "A-Z", "0-9", "\", "-", "_", "'", "`", ",".
|  CheckMainBuildingUse  														|	Bij utiliteitsbouw moet het primaire gebruik (MainBuildingUse.PrimaryUse) zijn opgegeven. Bij woningbouw mag het primaire gebruik juist niet zijn opgegeven.
|  CheckMultipleBagBuildingIdsWithMultipleBagResidenceIds						|	Bij meerdere pand id’s mogen er niet meerdere verblijfsobject id’s opgegeven zijn.
|  CheckNoDuplicateBAGBerthIds													|	Alle ligplaats id's (BagBerthId) mogen slechts één keer voorkomen in het bestand.
|  CheckNoDuplicateBAGPitchIds													|	Alle standplaats id's (BagPitchId) mogen slechts één keer voorkomen in het bestand.
|  CheckNoDuplicateBAGResidenceIds												|	Alle verblijfsobject id's (BagResidenceId) mogen slechts één keer voorkomen in het bestand.
|  CheckNumberOfDwellings  														|	Het aantal wooneenheden (NumberOfDwellings) moet bij utiliteitsbouw 0 zijn en bij woningbouw 1 of hoger.
|  CheckProjectNameAndObjectDoesNotAlreadyExists								|	Er mag geen andere actieve registratie zijn met dezelfde combinatie "ProjectNaam" en "ProjectObject".
|  CheckProjectNameAndObjectRequiredForVergunningsAanvraag						|	Wanneer een registratie wordt gedaan waarbij de BAG identificatie en de TPG identificatie leeg zijn, dan moeten de Project Name en Project Object gevuld zijn binnen de Provisional Identification.
|  CheckResidentialMultipleVboIds												|	Wanneer bij woningbouw een registratie wordt gedaan met meerdere verblijfsobject id's op één en het zelfde opnameadres, dan moet het gebouwtype (BuildingCategory) 'Woongebouw met niet-zelfstandige woonruimte' zijn, of 'Appartement' in combinatie met scope Compound.
|  CheckScopeCompoundEpcClassId													|	Wanneer een registratie wordt gedaan met scope Compound dan moet de Labelklasse niet ingevuld zijn.
|  CheckScopeSpecificMustHaveEpcClassId											|	Wanneer een registratie wordt gedaan met scope Specific dan moet de Labelklasse ingevuld zijn.
|  CheckSoftwareTool  															|	De naam (VendorSoftwareKey) en versienummer (VendorSoftwareVersionId) van de softwaretool moet ingevuld zijn, bestaan als SoftwareTool in EP-online, en daar geldig (actief) zijn op datum van registratie (huidige datum). Het versienummer van het registratiebestand (Version) moet overeenkomen met de XSD versie van de softwaretool in EP-online. De gebruikte rekenmethodiek (TypeCalculation) moet geldig (aangevinkt) zijn bij de softwaretool in EP-online.
|  CheckStatusExistingMustHaveBagIdentifcationAndNotProvisionalIdentification	|	Bij registratie met status 'bestaand' is het verplicht om subelement BAGIdentification op te nemen en mag subelement ProvisionalIdentification niet worden opgenomen.
|  CheckSurveyAndRegistrationNotSameAdvisor										|	Wanneer een registratie wordt gedaan met OpnameEnRegistratieZelfdeAdviseur is 'Yes', dan mag het blok 'Surveyor' niet gevuld zijn.<br/><br/> Wanneer een registratie wordt gedaan met OpnameEnRegistratieZelfdeAdviseur is 'No', dan mogen 'Name' en 'ExamNumber' in het blok 'Surveyor' niet leeg zijn.
|  CheckSurveyorFieldLengths													|	Voor de opnameadviseur mag het veld 'Voorletters' niet meer dan 20, het veld 'Tussenvoegsels' niet meer dan 50 en het veld 'Achternaam' niet meer dan 50 karakters bevatten.
|  CheckSurveyDate 																|	De opnamedatum (SurveyDate) moet binnen de registratie periode voor NTA labels liggen. Deze periode is anders voor het vervangen en herlabelen ten opzichte van het toevoegen of uitbreiden van labels.
|  CheckSurveyDateHasValue  													|	De opnamedatum (SurveyDate) moet ingevuld zijn.
|  CheckSurveyDateNotInFuture 													|	De opnamedatum (SurveyDate) mag niet in de toekomst liggen.
|  CheckTpgIdentification														|	Een TPG identificatie mag alleen worden opgegeven voor een gebouw als er ook een BAG identificatie is opgegeven.

### 2.5. BAG controle
Voor elk adres (verblijfsobject id, ligplaats id of standplaats id in BAGIdentification) wordt gecontroleerd aan de hand van de BAG of deze valide is. 

|  Situatie  							|  Rule(s)
|:--------------------------------------|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|  BagResultCheckBagHasMatchingAddress  |  Het ingevulde TPGIdentification adres moet overeenkomen (op basis van adres) met het resultaat uit BAG op basis van de ingevulde BAGIdentification.
|  BagResultCheckBagHasMatchingIds		|  Als het ingevulde verblijfsobject id niet behoort tot het ingevulde pand id, dan dient de BuildingAnnotation leeg te zijn.
|  BagResultCheckValidateBerthIds		|  Het opgegeven ligplaats id dient overeen te komen met het ligplaats id dat terug is gekomen vanuit de BAG.
|  BagResultCheckValidatePitchIds		|  Het opgegeven standplaats id dient overeen te komen met het standplaats id dat terug is gekomen vanuit BAG.
|  BagResultCheckValidateResidenceIds	|  De opgegeven verblijfsobject id’s dienen overeen te komen met de verblijfsobject id’s die terug zijn gekomen vanuit de BAG.

### 2.6. Controle op recentere registratie
Voor elk gebouw wordt gecontroleerd dat er niet al een recentere registratie aanwezig is op basis van het gevonden adres van het verblijfsobject via de BAG (in geval van identificatie d.m.v. BAG identificatie) of ProvisionalId in Projectgegevens (in geval van identificatie d.m.v. Provisional identificatie).

|  Technische naam						|	Rule(s)
|:--------------------------------------|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|	CheckNoMoreRecentLabelExists		|	Er mag geen pre-NTA registratie gevonden worden, op hetzelfde adres, die voldoet aan: <br/><ul><li>‘Geldig tot’ ligt in de toekomst.</li><li>‘Opnamedatum’ ligt na de opnamedatum (SurveyDate) uit het registratiebestand.</li></ul>Er mag geen NTA registratie gevonden worden, op hetzelfde adres of project, die voldoet aan:<ul><li>‘Geldig tot’ ligt in de toekomst.</li><li> 'Opnamedatum’ ligt na de opnamedatum (SurveyDate) uit het registratiebestand.</li><li>‘Scope’ heeft dezelfde waarde als ‘Scope’ uit het registratiebestand.</li><li>‘Gebouwklasse’ heeft dezelfde waarde als ‘Gebouwklasse’ uit het registratiebestand.</li></ul>

### 2.7. Controle op de actie
Controle of de actie 'Toevoegen', 'Vervangen' of 'Uitbreiden' is toegestaan. Bij Uitbreiden wordt gekeken of de situatie 'Uitbreiden' betreft en daarop de validaties uitgevoerd.

|  Actie  			|	Status									|  Rule(s)
|:------------------|:------------------------------------------|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|	Toevoegen	|	<ul><li>Vergunningsaanvraag</li><li>Oplevering</li><li>Bestaand</li></ul>	|<ul><li> Toevoegen is alleen mogelijk wanneer er op de opnamedatum voor geen enkele van de opnameadressen al een registratie is met dezelfde scope en gebouwklasse.</li><li> Toevoegen voor de status ‘Vergunningsaanvraag’ is alleen mogelijk wanneer één van de blokken ‘BAGIdentification’ of ‘ProvisionalIdentification’ gekozen is. </li><li> Toevoegen voor de status ‘Vergunningsaanvraag’ is niet mogelijk met een ‘provisionalid’. </li><li> Toevoegen voor woningbouw met een detailaanduiding is alleen mogelijk met rechten. Deze kunt u aanvragen in EP-Online. </li></ul>
|	Vervangen		|	<ul><li>Vergunningsaanvraag</li><li>Oplevering</li><li>Bestaand</li></ul>	|	<ul><li>Vervangen is alleen mogelijk wanneer er op de opnamedatum al een registratie is met dezelfde scope en gebouwklasse, waarbij alle adressen overeen komen met de initiële registratie.</li><li>Vervangen is alleen mogelijk wanneer de rekenmethodiek versie overeen komt met de versie die gebruikt is in de initiële registratie.</li><li>Vervangen is niet mogelijk wanneer een of meer van de gebouwen binnen de registratie ingetrokken is.</li><li>Vervangen is alleen mogeljik door de registratieadviseur die de initiële registratie gedaan heeft.</li><li>Vervangen van status ‘Vergunningsaanvraag’ is mogelijk zonder rechten binnen 60 dagen vanaf de originele registratiedatum. Hierna zijn rechten nodig. Deze kunt u aanvragen in EP-Online.</li><li>Vervangen van status ‘Oplevering’ of status ‘Bestaand’ is mogelijk zonder rechten binnen 7 dagen vanaf de originele registratiedatum. Hierna zijn rechten nodig. Deze kunt u aanvragen in EP-Online.</li></ul>
|	Uitbreiden		|	<ul><li>Oplevering</li><li>Bestaand</li></ul>		|	<ul><li>Uitbreiden is alleen mogelijk wanneer er op de opnamedatum al een registratie is met dezelfde scope en gebouwklasse, waarbij alle adressen overeen komen met de initiële registratie.</li><li> Uitbreiden is alleen mogelijk wanneer de meeste recente rekenmethodiek versie wordt gebruikt.</li><li> Uitbreiden is alleen mogelijk voor status ‘Oplevering’ en status ‘Bestaand’. </li><li> Uitbreiden is alleen mogelijk wanneer het adres/de adressen waarmee uitgebreid wordt/worden nog geen recenter(e) registratie heeft/hebben. </li><li> Uitbreiden voor woningbouw met een detailaanduiding is alleen mogelijk met rechten. Deze kunt u aanvragen in EP-Online. </li></ul> 
|	Herlabelen		|	<ul><li>Oplevering</li><li>Bestaand</li></ul>		|	<ul><li>Herlabelen is alleen mogelijk wanneer er op de opnamedatum al een registratie is met dezelfde scope en gebouwklasse, waarbij alle adressen overeen komen met de initiële registratie.</li><li>Herlabelen is niet alleen mogelijk wanneer de opnamedatum afwijkt van de opnamedatum van de initiële registratie.</li><li>Herlabelen is alleen mogelijk wanneer de rekenmethodiek versie overeen komt met de versie die gebruikt is in de initiële registratie.</li><li>Herlabelen is niet mogelijk wanneer een of meer van de gebouwen binnen de registratie ingetrokken is.</li><li>Herlabelen is alleen mogelijk met rechten. Deze kunt u aanvragen in EP-Online.</li><li>Herlabelen is alleen mogelijk binnen dezelfde certificaathouder die gebruikt is in de initiële registratie.</li><li>Herlabelen is alleen mogelijk wanneer de huidige registratieadviseur ‘detailinformatie energielabels inzichtelijk’ aan heeft staan in EP-Online.</li><li>Herlabelen is alleen mogelijk binnen 24 maanden vanaf de opnamedatum.</li></ul> |

