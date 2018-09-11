Programmet är ett .NET Core program skrivet i C# som är uppdelat i två delar: MusicCollector och testMVC.

MusicCollector har hand om kommunikationen med Spotifys API, och är fristående konsol program.
Spotify clientID och clientSecret anges i filen:  MusicCollector/authKey.json.
Observera att filen HttpConnector.cs som läser av authKey.json filen har sökväg angivet med"/", och inte Windows "\". 

testMVC är ett .NET MVC program som använder sig av MusicCollector. Så som den är inställd så skapar den en webbsida med URLen https://localhost:5001, när den körs.

Webblösningen, testMVC, kan bara söka på artist och låt och får upp förslag på relaterade artister. Dvs uppfyller inte vad uppgiften ska kunna.


// Christer
