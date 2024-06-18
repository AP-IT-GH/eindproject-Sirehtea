# Projectopdracht VR-Experience
*door: Turgay Yasar, Haitam Baqloul, Maximilian Duda, Jens Galle*

# Tutorial AI-powered VR: PRISON BREAK

## Inleiding

Welkom bij de tutorial voor ons project. Dit project is een VR-game ontwikkeld in Unity, waarin de speler in de rol van een gevangene moet proberen te ontsnappen uit een gevangenis. De gevangenis is opgezet als een doolhof en wordt bewaakt door bewakers die gebruik maken van machine learning om de speler te detecteren en te achtervolgen. De speler moet een keycard vinden om de uitgang te ontgrendelen en te ontsnappen.

## Korte Samenvatting

In deze tutorial zullen we je door het proces leiden van het verkrijgen en reproduceren van ons project. Je zult leren over de verschillende objecten die in ons Unity-project zijn gebruikt. Daarnaast krijg je een overzicht van de training van de machine learning-agents, inclusief grafieken van TensorBoard. Aan het einde van deze tutorial heb je een goed begrip van hoe je een AI-gestuurde VR-game kunt ontwikkelen in Unity.

## Methoden

### Installatie

- Unity versie: 2022.3.20f1
- Anaconda versie: 2024.02
- ML Agents: 2.0.1
- Oculus XR Plugin: 4.1.2
- OpenXR Plugin: 1.10.0
- XR Core Utilities: 2.3.0
- XR Interaction Toolkit: 2.5.4
- XR Legacy Input Helpers: 2.1.10
- XR Plugin Management: 4.4.1

Om het unity project te ontvangen moet je "git clone https://github.com/AP-IT-GH/eindproject-Sirehtea.git"

### Trainen van de agent

#### Benodigdheden:

- anaconda environment
- laatste versie van unity
- in je unity folderstructuur (.../assets/config) maak je een config file aan met extensie .yaml:

#### Te installeren (anaconda prompt)

- pip3 install torch~=1.7.1 -f
- python -m pip install mlagents==0.30.0

#### AI trainen

```
mlagents-learn config/[naam].yaml --run-id=[te-kiezen]
```

Nadat je deze commando uitvoert op je anaconda prompt zal je gevraagd worden om in unity de play knop in te drukken.

#### Bekijken van de resulataten via TensorBoard

```
tensorboard --logdir results --port 6006
```

Nadat je dit commando uitgevoerd hebt zal je een link krijgen die je redirzct naar je TensorBoard.

### Verloop van het Spel

In ons spel neemt de speler de rol van een gevangene aan die moet ontsnappen uit een doolhofachtige gevangenis. De bewakers, die door machine learning zijn getraind, patrouilleren door de gevangenis en proberen de speler te vangen. De speler moet een keycard vinden die verspreid is in de gevangenis om de uitgang te ontgrendelen en te ontsnappen. De speler ervaart het spel in VR met behulp van de XR Rig Simulator.

### Observaties, Acties en Beloningen

#### Observaties

De ML-agent (bewaker) verzamelt verschillende observaties om zijn omgeving en de positie van de speler te begrijpen.

- **Positie van de bewaker**: De lokale positie van de bewaker binnen de gevangenis.

#### Mogelijke Acties

De ML-agent kan verschillende acties uitvoeren om de speler te detecteren en te achtervolgen. Deze acties zijn:

- **Roteren (Links/Rechts)**: De agent kan draaien om zijn zicht en richting aan te passen.
- **Vooruit Bewegen**: De agent kan vooruit bewegen in de richting waarin hij kijkt.

#### Beloningen

De beloningen zijn ontworpen om het gedrag van de ML-agent te sturen naar het succesvol voltooien van zijn taken en het vermijden van ongewenst gedrag:

- **Vinden van de speler**: Wanneer de agent de speler (met de tag "Agent") vangt, krijgt hij een positieve beloning.
- **Botsen met muren**: Wanneer de agent tegen een muur botst (met de tag "Wall"), krijgt hij een negatieve beloning om dit gedrag te ontmoedigen.

### Beschrijving van de Objecten

- **Gevangene (speler)**: De hoofdpersoon die probeert te ontsnappen uit de gevangenis. De speler beweegt zich vrij door de gevangenis, zoekt voor een keycard en gebruikt deze om de uitgang te ontgrendelen terwijl hij de bewakers probeerd te ontwijken.
- **Bewakers**: ML-agents die zijn getraind om de speler te detecteren en te achtervolgen. Ze patrouilleren door de gevangenis en proberen de speler te vangen.
- **Keycard**: Een essentieel object dat nodig is om de uitgang te ontgrendelen.
- **Uitgang**: De uitgang wordt ontgrendeld met een keycard en leidt naar de vrijheid.

### Scènes

Er zijn in totaal zes scènes in het spel:

1. **BeginScene**: De VR-speler begint in een gevangeniscel. Deze scène bevat een gebruikersinterface om het spel te starten en het volume aan te passen.
2. **Lobby**: De speler bevindt zich in een gevangenislobby waar hij met de XR rig interactie kan hebben door een van de drie knoppen te pakken. De groene knop brengt de speler naar de makkelijkste level, de oranje knop naar matige level en de rode knop naar de moeillijskte level.
3. **Map1, Map2, Map3**: De verschillende maps bepalen het aantal bewakers in de map, wat de moeilijkheidsgraad van het level beïnvloedt. In deze gevangenisdoolhoven moet de speler een keycard vinden om de uitgang te ontgrendelen en door een gebroken ventilatieschacht te ontsnappen. Terwijl de speler de keycard zoekt, proberen de bewakers de speler te vinden en te vangen.
4. **EndScene**: Dit is een statische scène waar de gevangene wordt getoond terwijl hij ontsnapt en de eindcredits worden afgespeeld.

### Assets

- **LowPolyJail**: Bevat de modellen voor de gevangene, bewakers, muren, kamers, props.
- **AllSkyFree**: Bevat de skyboxes voor de scènes.
- **ADG_Textures**: Bevat de grondtexturen.
- **TextureHaven**: Bevat de vloertexturen.
- **WhiteCity**: Bevat de stad die als achtergrond wordt gebruikt.

### One-pager Informatie

Onze game is gebaseerd op het "PRISON BREAK" concept waarbij de speler in VR moet ontsnappen uit een doolhofachtige gevangenis terwijl hij wordt achtervolgd door AI-gestuurde bewakers. In dit spel neemt de speler de rol aan van een gevangene die door verschillende levels moet navigeren om een keycard te vinden en de uitgang te bereiken. De bewakers, die met machine learning zijn getraind, patrouilleren door de gevangenis en proberen de speler te vangen. Elk level varieert in moeilijkheidsgraad, afhankelijk van het aantal bewakers.

### Afwijkingen van de One-pager

Onze eerste one-pager beschreef een iets ander concept voor het spel. Hieronder beschrijven we de oorspronkelijke plannen en de aanpassingen die we hebben doorgevoerd, evenals de redenen voor deze veranderingen.

#### Oorspronkelijke Plannen

De oorspronkelijke one-pager bevatte de volgende elementen:

- **Interacties**:
  - **Scene 1**: De speler moest drie keycards verzamelen, verspreid over drie kamers met een bewaker in elke kamer. Na het verzamelen van de keycards ontgrendelde de speler een taser en ging naar het gevangenisplein.
  - **Scene 2**: Op het gevangenisplein moest de speler een geavanceerde Robocop vier keer raken met een taser, waarbij de Robocop steeds slimmer werd met elke hit.

#### Redenen voor de Aanpassingen

1. **Complexiteit Verminderen**: De oorspronkelijke scènes met de Robocop en meerdere keycards waren complex en vereisten veel resources. Door de levels te vereenvoudigen en de focus te leggen op het vinden van één keycard, konden we de ontwikkeltijd verkorten en de gameplay verbeteren.
2. **Gameplay Verbeteren**: Door de levels te variëren in moeilijkheidsgraad op basis van het aantal bewakers, hebben we de spelervaring uitdagender en gevarieerder gemaakt. Dit zorgt voor een betere leercurve voor de spelers.
3. **Gebruik van VR**: Door de speler in een VR-omgeving te plaatsen, hebben we de immersie en spanning verhoogd. De speler kan vrij rondkijken en interageren met de omgeving, wat een meeslepende ervaring biedt.

### Samenvatting van de Aanpassingen

- **Oorspronkelijke Plan**: Drie keycards verzamelen, taser gebruiken op een Robocop.
- **Huidige Implementatie**: Een keycard in de maze, variërende moeilijkheidsgraad, focus op ontsnappen uit de gevangenis.
- **Redenen**: Vereenvoudiging van de ontwikkeling, verbetering van de gameplay.

Deze aanpassingen hebben geleid tot een meer gestroomlijnde en boeiende spelervaring, waarbij de kracht van AI en VR optimaal wordt benut om een dynamisch en meeslepend spel te creëren.

## Resultaten

### Resultaten van de Training

Hier presenteren we de resultaten van de training van de ML-agents met behulp van TensorBoard. De grafiek hieronder toont de cumulatieve beloning van twee controllers: de Agent Controller (blauwe lijn) en de Hunter Controller (roze lijn).

### Beschrijving van de TensorBoard Grafieken

![image](https://github.com/AP-IT-GH/eindproject-Sirehtea/assets/145667096/b9dee6e5-fd2e-41f2-87da-68d5ea489f1c)


- **Trainingsevolutie**: Deze grafiek toont de vooruitgang van de bewakers (Hunter Controller) bij het leren om de speler te detecteren en te achtervolgen, en de agent (Agent Controller) bij het vinden van drie objecten in het doolhof.
- **Beloningsgrafiek**: Deze grafiek toont de gemiddelde beloning die de ML-agents ontvangen tijdens de training. De y-as vertegenwoordigt de cumulatieve beloning, terwijl de x-as de tijd (in stappen) weergeeft.

### Opvallende Waarnemingen tijdens het Trainen

Tijdens het trainen van de ML-agents hebben we de volgende observaties gedaan:

1. **Beginfase van de Training**:

   - De Hunter Controller (roze lijn) begon met een aanzienlijk lagere beloning, wat aangeeft dat de agent in het begin moeite had met het efficiënt detecteren en achtervolgen van de speler.
   - De Agent Controller (blauwe lijn) had een relatief stabiele beloning in het begin, wat aangeeft dat de agent aanvankelijk effectief was in het vinden van objecten in het doolhof.

2. **Middenfase van de Training**:

   - Tussen 1M en 4M stappen is er een duidelijke verbetering in de prestaties van de Hunter Controller. De cumulatieve beloning stijgt, wat aangeeft dat de bewakers beter werden in het vinden en achtervolgen van de speler.
   - De Hunter Controller maakte gebruik van curiosity-driven exploration om nieuwe plekken in het doolhof te verkennen, wat bijdroeg aan de verbetering van hun prestaties.
   - De Agent Controller behoudt een relatief constante prestatie gedurende deze fase, met een lichte fluctuatie rond de gemiddelde beloning.

3. **Eindfase van de Training**:
   - Na 4M stappen lijken beide controllers een stabiel patroon te vertonen, met de Hunter Controller die een licht hogere cumulatieve beloning bereikt dan de Agent Controller.
   - De stabilisatie in beloningen suggereert dat de ML-agents een zekere mate van bekwaamheid hebben bereikt in hun respectieve taken.

## Conclusie

In dit project hebben we een AI-gestuurde VR-game ontwikkeld waarin de speler moet ontsnappen uit een doolhofachtige gevangenis.

De resultaten van onze training laten zien dat de ML-agents effectief zijn getraind in hun respectieve taken: de Hunter Controller is beter geworden in het detecteren en achtervolgen van de speler, terwijl de Agent Controller succesvol objecten in de doolhof heeft gevonden. Het gebruik van curiosity-driven exploration heeft de prestaties van de Hunter Controller aanzienlijk verbeterd, wat wijst op een adaptieve leeromgeving.

Deze resultaten tonen aan dat het integreren van AI in VR-games niet alleen de complexiteit en uitdaging van het spel kan verhogen, maar ook een dynamische en unieke speelervaring kan bieden. Door AI-gestuurde bewakers toe te voegen, wordt het spel onvoorspelbaarder en spannender, wat bijdraagt aan de herspeelbaarheid. Voor toekomstige verbeteringen kunnen we overwegen om meer geavanceerde gedragingen en interacties toe te voegen, zoals verschillende soorten bewakers of complexe ontsnappingsroutes voor de speler.

## Bronvermelding

- [Tutorial - 4](https://youtu.be/KHgSDFB9nTE?si=xjAkrKDIVXOaUfIg)
- [Tutorial - 3](https://youtu.be/Xwec9-Do22Y?si=yDPKnKF_ncFgnBZH)
- [Tutorial - 2](https://youtu.be/QKk_scTdabQ?si=K2OB9iRLkP8y5oG0)
- [Tutorial - 1](https://youtu.be/Z87xPYl7g4o?si=NaIQmBdNQqZG32-s)


