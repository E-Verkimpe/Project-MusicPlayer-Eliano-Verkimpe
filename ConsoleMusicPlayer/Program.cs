using ConsoleMusicPlayer;

MediaPlayer mediaplayer = new MediaPlayer();
mediaplayer.CheckSongFile();

bool keepLooping = true;
int userInput;
PossibleChoices userChoice;

while (keepLooping)
{
    mediaplayer.FetchMetaData();
    userInput = mediaplayer.CheckUserInput(0, 5, "Please make a choice from the menu (0-5)");
    userChoice = (PossibleChoices)userInput;

    switch (userChoice)
    {
        case PossibleChoices.Quit:
            keepLooping = false;
            break;

        case PossibleChoices.PlayPause:
            mediaplayer.PlayPause();
            break;

        case PossibleChoices.ChangeVolume:
            mediaplayer.ChangeVolume();
            break;

        case PossibleChoices.MuteUnmute:
            mediaplayer.MuteUnmute();
            break;

        case PossibleChoices.PlayNewSong:
            mediaplayer.CheckSongFile();
            break;

        case PossibleChoices.Stop:
            mediaplayer.StopCurrentSong();
            break;

        default:
            throw new Exception("Error, something has gone seriously wrong.");
    }
}