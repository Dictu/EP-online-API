# Businessrules registratiebestand EP-online

## 1. Versiebeheer
| Datum 		| Versie| Auteur  			| Wijzigingen
|--------------:|:------:|:-----------------:|:----------------------------------------------------------------------------
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

## 2. Validaties
Het valideren en verwerken van het registratiebestand gebeurt in een aantal stappen. Als er een of meerdere validaties in een stap niet voldoen, worden de betreffende bijbehorende meldingen gegeven en niet verder gegaan naar de volgende stap.

### 2.1. Ophalen en valideren gebruiker (adviseur) en organisatie
Gegevens omtrent de adviseur en organisatie worden opgehaald. Er vinden validaties plaats dat de gegevens gevonden kunnen worden.

### 2.2. Bepalen versienummer en rekenmethodiek
|	Technische naam				|	Rule(s)
|-------------------------------|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|	CalculationType				|	TypeCalculation, Versie, MainBuildingClass en Opnametype worden uitgelezen. Aan de hand hiervan wordt bepaald wat de rekenmethodiek is. De rekenmethodiek moet bestaan in EP-online.
|	CalculationTypeVersion		|	De XSD versie bij de rekenmethodiek moet bestaan in EP-online.
|	MonitoringVersion			|	Element *EPMeta.Version* moet bestaan en een geldig getal zijn; dit bepaalt de monitorbestandversie.
|	GenericXmlVersion			|	In de generieke XML moet het veld *Energieprestatie.Versie* gevuld zijn, een geldig getal zijn, en overeenkomen met de generieke XML versie in EP-online.

### 2.3. XSD validatie
| Technische naam		| Rule(s)
|-----------------------|:-----------------------------------------------------------------------------------------------------
| MainSchemeValidator	| De XML wordt gevalideerd tegen de monitor XSD (exclusief het element *SurveySourceData* )
| SurveyDataValidator	| Het element *SurveySourceData* moet bestaan en wordt gevalideerd tegen de XSD van het generieke deel.

### 2.4. Inhoudelijke controles
Onderstaande validaties worden allemaal en in willekeurige volgorde uitgevoerd. Indien er meerdere validaties falen, dan worden alle bijbehorende foutmeldingen geretourneerd.

| Technische naam 																| Rule(s)
|-------------------------------------------------------------------------------|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|  CheckBagBuildingId															|	Het is verplicht een Verblijfsobject Id op te geven wanneer er een Pand Id is opgegeven.
|  CheckBagIdCombination														|	<ul><li> Standplaats Id en Ligplaats Id mogen niet ingevuld zijn wanneer Verblijfsobject Id en/of Pand Id ingevuld is/zijn.</li><li> Verblijfsobject Id, Pand Id en Standplaats Id mogen niet ingevuld zijn wanneer Ligplaats Id ingevuld is.</li><li> Verblijfsobject Id, Pand Id en Ligplaats Id mogen niet ingevuld zijn wanneer Standplaats Id ingevuld is.</li></ul>
|  CheckBagIdentificationForStatusCompletion									|	Bij Status ‘Oplevering’ en een ingevuld provisional identificatie, moet er een BAG identificatie ingevuld zijn.
|  CheckBagOrProvisionalIdentification											|	Het is verplicht om voor ieder gebouw minimaal een identificatie via BAG of een provisional identificatie op te geven.
|  CheckBuildingIdentifiedByBagHasUniqueBagIds  								|	Alle pand-id's (BagBuildingId) mogen slecht één keer voorkomen in het bestand.
|  CheckBuildingObservation  													|	Opname heeft plaatsgevonden in het gebouw (BuildingObservation = Yes).
|  CheckBuildingType  															|	Bij woningbouw: bij elk gebouw moet het gebouwtype (BuildingCategory) ingevuld zijn. <br/> Bij utiliteitsbouw: bij een gebouw mag zowel het gebouwtype (BuildingCategory) als het subtype (BuildingCategorySupplement) niet ingevuld zijn.
|  CheckBuildingTypeBagBerth													|	Bij gebruik van BagBerthId zijn enkel gebouwtype (BuildingCategory) 13 (Woonboot bestaande ligplaats) en 15 (Woonboot nieuwe ligplaats) toegestaan.
|  CheckBuildingTypeBagPitch													|	Bij gebruik van BagPitchId is enkel gebouwtype (BuildingCategory) 14 (Woonwagen) toegestaan.
|  CheckBuildingTypeSupplement  												|	Alleen voor woningbouw:<br/><br/> <b>Scope is specific:</b><br/>Voor gebouwtype (BuildingCategory) 7 moet het subtype (BuildingCategorySupplement) gevuld zijn en een waarde van 1 t/m 8 bevatten. <br/> Bij andere gebouwtypes mag het subtype (BuildingCategorySupplement) niet gevuld zijn.<br/><br/><b>Scope is compound:</b><br/> Voor gebouwtype (BuildingCategory) 7 moet het subtype (BuildingCategorySupplement) niet gevuld zijn.
|  CheckBuildingUseTypes  														|	Alleen voor utiliteitsbouw: voor alle UseTypes (PrimaryUse en SecondaryUse's) moet het Percentage > 0 zijn en opgeteld tussen 0 en 100 (inclusief) liggen.
|  CheckCalculationType  														|	De rekenmethodiek (TypeCalculation) moet bestaan in EP-online, niet geblokkeerd zijn, en de gebouwklasse moet overeenkomen met de MainBuildingClass in de XML. <br/> <b>NB:</b> voor de NTA_8800 rekenmethodiek wordt deze door code door de webservice aangevuld met de indicatie basis/detailopname en indicatie woningbouw/utiliteitsbouw (in EP-online bestaan er dus 4 NTA-8800 rekenmethodieken).
|  CheckConstructionAndRenovationYear  											|	Als de status niet "Vergunningsaanvraag" is, dan mogen het bouwjaar (ConstructionYear) en het jaar van renovatie (YearOfRenovation) niet in de toekomst liggen.
|  CheckContractor  															|	De contractor (degene die registreert) moet bestaan als gebruiker in EP-online, de rol contracthouder (adviseur) hebben en gemachtigd zijn voor registraties met de rekenmethodiek (TypeCalculation) in EP-online.
|  CheckDetailForStatusOpleveringAndVergunningsAanvraag							|	Bij Status ‘Vergunningsaanvraag’ en bij Status ‘Oplevering’ moet het opnameprotocol 'Detail' zijn.
|  CheckDuplicateAddresses  													|	Alle adressen (ZipCode+Number+Extension+BuildingAnnotation uit TPGIdentification) mogen slechts één keer voorkomen in het bestand.
|  CheckDuplicateBagResidenceIds  												|	Alle verblijfsobjecten Id's (BagResidenceId) mogen slecht één keer voorkomen in het bestand.
|  CheckEpcClassIdResidential													|	Bij een registratie voor woningbouw is de hoogst haalbare labelletter A++++. 
|  CheckEpcVersion  															|	Het versienummer van het monitorbestand moet geldig zijn op moment van registratie.
|  CheckMainBuildingCertificateByBag  											|	Wanneer bij woningbouw voor het opnamegebouw (MainBuilding) een VBO-ID (BAGIdentification) is opgegeven en er referentiewoningen (ReferenceBuildingList) zijn meegegeven: indien er op de opnamedatum (SurveyDate) al een certificaat is voor het (eerste) VBO-Id van het opnamegebouw, dan moet het versienummer van dit certificaat gelijk of hoger zijn dan die in het registratiebestand.
|  CheckMainBuildingUse  														|	Bij utiliteitsbouw moet het primaire gebruik (MainBuildingUse.PrimaryUse) zijn opgegeven. Bij woningbouw mag het primaire gebruik juist niet zijn opgegeven.
|  CheckMultipleBagBuildingIdsWithMultipleBagResidenceIds						|	Bij meerdere Pand-Id’s mogen er niet meerdere VBO-ID’s opgegeven zijn.
|  CheckNotPermitPreNtaStatusCompletionWithBagAndProvisionalIdentification		|	Wanneer de buildingstatus ‘Oplevering’ is, de PermitPreNTA ‘False’ is en zowel de BAGIdentification als de ProvisionalIdentification gevuld zijn, dient er een overeenkomstige registratie met status 'vergunningsaanvraag' met de ProvisionalId te worden gevonden.
|  CheckNotPermitPreNtaStatusCompletionWithBagIdentification					|	Wanneer de buildingstatus ‘Oplevering’ is, de PermitPreNTA ‘False’ is en enkel de BAGIdentification is gevuld, dient er een overeenkomstige registratie met status 'vergunningsaanvraag' met BAGIdentification te worden gevonden.
|  CheckNumberOfDwellings  														|	Het aantal wooneenheden (NumberOfDwellings) moet bij utiliteitsbouw 0 zijn en bij woningbouw 1 of hoger.
|  CheckPermitPreNtaNoProvisionalIdentification  								|	Wanneer de buildingstatus ‘Oplevering’ is, de PermitPreNTA ‘True’ is dan moet de BAGIdentification worden gevuld en mag de ProvisionalIdentification niet zijn toegevoegd.
|  CheckProjectNameAndObjectRequiredForVergunningsAanvraag						|	Wanneer een registratie wordt gedaan waarbij de BAG identificatie en de TPG identificatie leeg zijn, dan moeten de Project Name en Project Object gevuld zijn binnen de Provisional Identification.
|  CheckScopeCompoundEpcClassId													|	Wanneer een registratie wordt gedaan met scope Compound dan moet de Labelklasse niet ingevuld zijn.
|  CheckScopeSpecificMustHaveEpcClassId											|	Wanneer een registratie wordt gedaan met scope Specific dan moet de Labelklasse ingevuld zijn.
|  CheckSoftwareTool  															|	De naam (VendorSoftwareKey) en versienummer (VendorSoftwareVersionId) van de softwaretool moet ingevuld zijn, bestaan als SoftwareTool in EP-online, en daar geldig (actief) zijn op datum van registratie (huidige datum). Het versienummer van het registratiebestand (Version) moet overeenkomen met de XSD versie van de softwaretool in EP-online. De gebruikte rekenmethodiek (TypeCalculation) moet geldig (aangevinkt) zijn bij de softwaretool in EP-online.
|  CheckStatusExistingMustHaveBagIdentifcationAndNotProvisionalIdentification	|	Bij registratie met status 'bestaand' is het verplicht om subelement BAGIdentification op te nemen en mag subelement ProvisionalIdentification niet worden opgenomen.
|  CheckSurveyAndRegistrationNotSameAdvisor										|	Wanneer een registratie wordt gedaan met OpnameEnRegistratieZelfdeAdviseur is 'Yes', dan mag het blok 'Surveyor' niet gevuld zijn.<br/><br/> Wanneer een registratie wordt gedaan met 	OpnameEnRegistratieZelfdeAdviseur is 'No', dan mogen 'Name' en 'ExamNumber' in het blok 'Surveyor' niet leeg zijn.
|  CheckSurveyDate 																|	De opnamedatum (SurveyDate) moet binnen de registratie periode voor NTA labels liggen. Deze periode is anders voor het vervangen ten opzichte van het toevoegen of uitbreiden van labels.
|  CheckSurveyDateHasValue  													|	De opnamedatum (SurveyDate) moet ingevuld zijn.
|  CheckSurveyDateNotInFuture 													|	De opnamedatum (SurveyDate) mag niet in de toekomst liggen.
|  CheckTpgIdentification														|	Een TPG identificatie mag alleen worden opgegeven voor een gebouw als er ook een BAG identificatie is opgegeven.

### 2.5. BAG controle
Voor elk adres (VBO-ID, Ligplaats-Id of Standplaats-Id in BAGIdentification) wordt gecontroleerd aan de hand van de BAG of deze valide is. 

|  Situatie  							|  Rule(s)
|---------------------------------------|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|  BagResultCheckAddressMustExistbyBag  |  De objecten (op basis van Ligplaats-Id, Standplaats-Id of VBO-ID en evt. alle Pand-Id’s) moeten gevonden worden in de BAG.
|  BagResultCheckBagHasMatchingAddress  |  Het ingevulde TPGIdentification adres moet overeenkomen (op basis van adres) met het resultaat uit BAG op basis van de ingevulde BAGIdentification.
|  BagResultCheckBagHasMatchingIds		|  Als het ingevulde VBO-ID niet behoort tot het ingevulde Pand-Id, dan dient de BuildingAnnotation leeg te zijn.
|  BagResultCheckValidateBerthIds		|  Het opgegeven Ligplaats-Id dient overeen te komen met het  Ligplaats-Id  vanuit BAG.
|  BagResultCheckValidateBuildingIds	|  De opgegeven Pand-Id’s dienen overeen te komen met de Pand-Id’s vanuit BAG.
|  BagResultCheckValidatePitchIds		|  Het opgegeven Standplaats-Id dient overeen te komen met de  Standplaats-Id vanuit BAG.
|  BagResultCheckValidateResidenceIds	|  De opgegeven VBO-ID’s dienen overeen te komen met de VBO-ID’s vanuit BAG.

### 2.6. Controle op recenter certificaat
Voor elk gebouw wordt gecontroleerd dat er niet al een recenter certificaat aanwezig is op basis van het gevonden adres van het verblijfsobject via de BAG (in geval van identificatie d.m.v. BAG identificatie) of ProvisionalId in Projectgegevens (in geval van identificatie d.m.v. Provisional identificatie).

|  Technische naam			|	Rule(s)
|---------------------------|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
|	CheckNoneMoreRecent		|	Er mag geen pre-NTA certificaat gevonden worden, op hetzelfde adres, die voldoet aan: <br/><ul><li>‘Geldig tot’ ligt in de toekomst.</li><li>‘Opnamedatum’ ligt na de opnamedatum (SurveyDate) uit het registratiebestand.</li></ul>Er mag geen NTA certificaat gevonden worden, op hetzelfde adres of project, die voldoet aan:<ul><li>‘Geldig tot’ ligt in de toekomst.</li><li> 'Opnamedatum’ ligt na de opnamedatum (SurveyDate) uit het registratiebestand.</li><li>‘Scope’ heeft dezelfde waarde als ‘Scope’ uit het registratiebestand.</li><li>‘Gebouwklasse’ heeft dezelfde waarde als ‘Gebouwklasse’ uit het registratiebestand.</li></ul>

### 2.7. Controle op de actie
Controle of de actie 'Toevoegen', 'Vervangen' of 'Uitbreiden' is toegestaan. Bij Uitbreiden wordt gekeken of de situatie 'Uitbreiden' of 'UitbreidenExtra' betreft en daarop de validaties uitgevoerd.

|  Actie  			|	Status									|  Rule(s)
|------------------	|:-----------------------------------------:|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|	Toevoegen		|	Vergunningsaanvraag						|	Het is verplicht om minimaal subelement BAGIdentification of subelement ProvisionalIdentification op te nemen. Hierbij mag veld ProvisionalID niet zijn gevuld. Dit geldt voor alle gebouwen uit het registratiebestand.
|					|	Vergunningsaanvraag/Oplevering/bestaand	|	Op de opnamedatum (SurveyDate) mag er niet al een certificaat zijn met dezelfde scope en gebouwklasse, waarbij een van de adressen overeenkomt met een adres van het opnamegebouw. 
|	Vervangen		|	Vergunningsaanvraag						|	Op de opnamedatum (SurveyDate) dient er al een certificaat te zijn met dezelfde scope en gebouwklasse, waarbij het ProvisionalId overeenkomt met het ProvisionalId van het opnamegebouw. Mocht er niks gevonden worden op basis van de ProvisionalIdentification dan wordt er gekeken of op basis van een adres een certificaat gevonden kan worden (zie status Oplevering/bestaand).
|					|	Oplevering/bestaand						|	Op de opnamedatum (SurveyDate) dient er al een certificaat te zijn met dezelfde scope en gebouwklasse, waarbij een van de adressen overeenkomt met een adres van het opnamegebouw. 
|					|	Vergunningsaanvraag/Oplevering/bestaand	|	Wanneer er een certificaat gevonden is gelden de volgende regels:<br/><ul><li>De huidige datum moet in de geldigheidperiode liggen van het bestaande certificaat (tussen Opnamedatum en Geldig tot).</li></ul> <b>Aanvullende validaties (gelden niet voor Beheerders): </b><br/> <ul><li> Het huidige certificaat moet geregistreerd zijn door de, huidige ingelogde, gebruiker van de EnergielabelApi.</li><li>De registratiedatum van het bestaande certificaat mag slechts een maximaal aantal dagen in het verleden liggen (deze periode is anders voor vergunningsaanvraag ten opzichte van oplevering of bestaand), of de ingelogde gebruiker heeft de rol ‘Certificaat Vervanger’ en de verloopdatum van deze rol bij de gebruiker is ingevuld en nog niet verstreken.</li></ul>
|	Uitbreiden		|											|	**Situatie met opnamegebouw o.b.v. adres:**	<br/><ul><li> Op de opnamedatum (SurveyDate) dient er al een opnamegebouw te bestaan met dezelfde scope en gebouwklasse, met minimaal dezelfde adressen van het opnamegebouw.</li><li> Op de opnamedatum (SurveyDate) dient er al een certificaat te zijn met dezelfde scope en gebouwklasse, waarbij een adres overeenkomt met een adres van het opnamegebouw. </li></ul> **Situatie met opnamegebouw o.b.v. ProvisionalId:** <br/><ul><li> Op de opnamedatum (SurveyDate) dient er al een opnamegebouw te bestaan met dezelfde scope en gebouwklasse, met minimaal dezelfde adressen van het opnamegebouw.</li><br/><li> Op de opnamedatum (SurveyDate) dient er al een certificaat te zijn met dezelfde scope en gebouwklasse, waarbij een adres overeenkomt met een adres van het opnamegebouw. </li></ul> **Geldt voor alle situaties:** <br/><ul><li> De rekenmethodiek van het bestaande certificaat is gelijk aan die van het registratiebestand. </li><li> Er worden <b>geen</b> gebouwen toegevoegd die nog niet in het bestaande certificaat zitten, maar waarvoor wel al een ander certificaat bestaat. </li><li> Er zijn gebouwen in het registratiebestand die nog niet bij het bestaande certificaat aanwezig zijn. </li></ul>
