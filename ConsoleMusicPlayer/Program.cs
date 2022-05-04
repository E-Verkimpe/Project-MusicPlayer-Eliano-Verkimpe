using ConsoleMusicPlayer;

MediaPlayer musicPlayer = new MediaPlayer();
musicPlayer.CheckSongFile();

bool keepLooping = true;
int userInput;
PossibleChoices userChoice;

while (keepLooping)
{
    userInput = musicPlayer.CheckUserInput(0, 5, "Please make a choice from the menu (0-5)");
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

        default:
            throw new Exception("Error, something has gone seriously wrong.");
    }
}