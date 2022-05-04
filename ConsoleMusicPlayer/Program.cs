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
musicPlayer.CheckSongFile();

bool keepLooping = true;
int userInput = 6;
PossibleChoices userChoice = PossibleChoices.Default;
while (keepLooping)
{
    musicPlayer.PrintInterface();
    userInput = musicPlayer.CheckUserInput(0,5, "Please make a choice from the menu (0-5)");
    userChoice = (PossibleChoices)userInput;

    switch (userChoice)
    {
        case PossibleChoices.Quit:
            keepLooping = false;
            break;
        case PossibleChoices.PlayPause:
            musicPlayer.PlayPause();
            break;
        case PossibleChoices.ChangeVolume:
            musicPlayer.ChangeVolume();
            break;
        case PossibleChoices.MuteUnmute:
            musicPlayer.MuteUnmute();
            break;
        case PossibleChoices.PlayNewSong:
            musicPlayer.CheckSongFile();
            break;
        case PossibleChoices.Stop:
            musicPlayer.StopCurrentSong();
            break;
        case PossibleChoices.Default:
            break;
        default:
            break;
    }
}

