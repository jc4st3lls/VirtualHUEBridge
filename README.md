## Emulate a PHILIPS HUE Bridge V3

**Emulador d'un Bridge HUE V3 de Philips compatible amb Alexa**

Aquest projecte és una prova de concepte (POC). Fa poc vaig comprar un Echo Alexa i vaig començar a jugar amb el dispositiu i investigar una mica com connectar llums i altres components compatibles, per poder interactuar amb ells enviant comandes de veu al'Echo.

En resum, la majoria de fabricants de dispositius IOT, et fan descarregar una App pròpia, on configures el dispositiu, connectes la App al seu núvol, descarregues un skill del fabricant a l'Echo, la connectes al núvol, i aquest li presenta els teus dispositius de casa. I que vol dir això? Doncs que una bombeta que està a la mateixa xarxa que l'Echo, aquest l'engega connectant-se al núvol del fabricant de la bombeta, i aquest l'encén a casa teva. Si tens deu bombetes de deu fabricants diferents, deu fabricants accediran a casa teva per gestionar cada un la seva bombeta.

Aleshores, vaig començar a investigar si tot era així. Vaig trobar informació i algun projecte que parlava de que les bombetes Philips Hue es podien connectar a Alexa sense passar pel nuvol. També alguna altre marca, però d'aquestes les Philips, vaig trobar més informació. Philips té un bridge que dona la possibilitat de connectar-hi moltes bombetes i altres dispositius IOT. Aquest bridge si es troba en la mateixa xarxa que l'Echo, es capaç de mostrar-se com a dispositiu connectable a l'Echo, sense connexió al núvol.

**Hacking Alexa**

Per saber com cerca dispositius l'Echo, vaig captura tots els paquets de xarxa d'aquest, quan aquest cerca dispositius. El que envia per descubrir dispositius com era d’esperar, són cerques

 SSDP [https://es.wikipedia.org/wiki/SSDP](https://es.wikipedia.org/wiki/SSDP), concretament:

    SEARCH * HTTP/1.1
    HOST: 239.255.255.250:1900
    ST: ssdp:all
    MAN: "ssdp:discover"
    MX: 3

    SEARCH * HTTP/1.1
    HOST: 239.255.255.250:1900
    ST: upnp:rootdevice
    MAN: "ssdp:discover"
    MX: 3

    M-SEARCH * HTTP/1.1
    HOST: 239.255.255.250:1900
    ST: ssdp:all
    MAN: "ssdp:discover"
    MX: 3

Tot seguit vaig adquirir un bridge i una bombeta. Ho vaig connectar a la xarxa i seguint les instruccions (cal prémer un botó per que es pugui detectar), vaig connectar-lo a l’Echo (prèviament havia connectat la bombeta a l’Echo). Ho vaig treure tot, i vaig repetir la operació (més d’un cop) per capturar la conversa que tenien ambdós per connectar-se. El resultat es pot explicar en dues fases. La primera és la connexió, el moment en que l’Echo i el bridge es connecten. I la segona fase, quan és connecta, com l’Echo interroga al bridge per saber com “funciona” i quins dispositius controla.

Per saber com ho fa, podeu investigar el codi que deixo aquí o seguir el passos que he seguit jo. Només dir que, la documentació de la api dels dispositius de Philips es pot trobar a [https://developers.meethue.com](https://developers.meethue.com).

**Solució**

En aquest repositori trobem una solució amb dos projectes desenvolupats amb Net Core 3.1. Cada projecte es correspon a una de les fases, la de connexió (cal executar el primer cop, quan l’Echo cerca, detecta i configura) i la api que emula el bridge (només bombetes), que cal sempre. A la api hi he afegit unes bombetes virtuals a través d'una plana html.

**Funcionamet**

Primer de tot cal modificar alguns paràmetres de configuració. Al projecte que permet en "Discovery", cal modificar dins la classe AppSet.cs, les propietats:

FrontendIP=[IP del front end de la API]

EchoIP=[IP del Echo]

Al projecte de la API, cal modificar el fitxer de configuració appsettings.json:


    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft": "Warning",
          "Microsoft.Hosting.Lifetime": "Information"
        }
      },
      "AllowedHosts": "*",
      "UPNP": {
        "FrontendIP": "**[IP del front end de la API]**",
        "FrontendPort": "80"
      },
      "ApiPort": "**[Port accés a la API]**"
    }

FrontendIP=[IP del front end de la API]

ApiPort=[Port accés a la API]

La Api ara està programada perque corri en el port 80. Però si al host tenim un nginx, un apache o un IIS corrent (o altres), tindrant el port ocupat i no funcionarà. Aleshores hem de canviar el port de la api, i fer un reverse proxy des del nginx, apache o IIS cap al port de la API. També podem crear un contenidor de la Api i fer el mateix.

Un cop fet això, engeguem primer de tot la API. S'obrirà una plana html molt simple, amb unes llums virtuals apagades.

Tot seguit engeguem el projecte de "discovery". Aquest es manté a l'espera fins que rep solicituts del l'Echo.

Un cop els dos estan funcionan, li diem al nostre Echo:
**"Alexa, descubre dispositivos"**

Si tot va bé, trobarà 5 bombetes, que es poden engegar i apagar. Si observeu la plana de les llums virtuals, veure-ho el resultat.

A partir d'aquí, imaginació. 

Si volem controlar dispositius reals on/off, només cal fixar-nos en la interfície **INotificationService**, la implementació per a les bombetes virtuals **NotificationSignalRService** i el componet que actua quan es produeixen els esdeveniments **LightsMessageCenter**.

**Donacions**
BTC = bc1q33wmuc0lcwh4krchfmxrt4jak3v628z658f85j
TRX = TRdxjgLv4nr8GYwsdXkUv4ZwCCTvfMqgnS
ETH = 0x3582cf65c158b23bbbaee3e3a3158ce4b5d99978

:_)
