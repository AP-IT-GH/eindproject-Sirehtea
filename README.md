# Projectopdracht VR-Experience
*door: Turgay Yasar, Haitam Baqloul, Maximilian Duda, Jens Galle*

# Tutorial AI-powered VR: PRISON BREAK

## Inleiding

Welkom bij de tutorial voor ons project. Dit project is een VR-game ontwikkeld in Unity, waarin de speler in de rol van een gevangene moet proberen te ontsnappen uit een gevangenis. De gevangenis is opgezet als een doolhof en wordt bewaakt door bewakers die gebruik maken van machine learning om de speler te detecteren en te achtervolgen. De speler moet een keycard vinden om de uitgang te ontgrendelen en te ontsnappen.

## Korte Samenvatting

In deze tutorial zullen we je door het proces leiden van het verkrijgen en reproduceren van ons project. Je zult leren over de verschillende objecten die in ons Unity-project zijn gebruikt. Daarnaast krijg je een overzicht van de training van de machine learning-agents, inclusief grafieken van TensorBoard. Aan het einde van deze tutorial heb je een goed begrip van hoe je een AI-gestuurde VR-game kunt ontwikkelen in Unity.

## Methoden

### Installatie

Unity versie: 2022.3.20f1
Anaconda versie: 2024.02
ML Agents: 2.0.1
Oculus XR Plugin: 4.1.2
OpenXR Plugin: 1.10.0
XR Core Utilities: 2.3.0
XR Interaction Toolkit: 2.5.4
XR Legacy Input Helpers: 2.1.10
XR Plugin Management: 4.4.1

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
