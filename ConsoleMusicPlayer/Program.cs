/* Opdracht
In de eerste fase maak je een sterk vereenvoudigde versie van een muziekspeler. 
Probeer hiervoor de volgende functionaliteiten te implementeren: 
• Print een titel voor de applicatie naar de console 
• Vraag de gebruiker om de locatie van het MP3-bestand in te voeren dat hij/zij wenst af te spelen (invoer via Console) 
• Nadat de gebruiker de locatie heeft ingevoerd, wordt het liedje op deze locatie afgespeeld 
• Wanneer de gebruiker op -drukt, wordt de applicatie afgesloten 
*/

using ConsoleMusicPlayer;

MediaPlayer musicPlayer = new MediaPlayer();

bool keepLooping = true;
while (keepLooping)
{
    musicPlayer.GetSongFile();

    if (musicPlayer.MusicFolder == "-")
    {
        keepLooping = false;
    }
    else
    {
        musicPlayer.PlaySong();
    }
}

