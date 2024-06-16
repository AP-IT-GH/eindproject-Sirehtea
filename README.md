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
