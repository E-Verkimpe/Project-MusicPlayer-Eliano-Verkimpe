using WMPLib;

namespace ConsoleMusicPlayer
{
    public class MediaPlayer
    {
        private WindowsMediaPlayer _player;
        private Frontend _frontend;

        private string musicFolder;
        private MediaPlayerState currentState;

        public MediaPlayer()
        {
            //this way is fine.
            _player = new WindowsMediaPlayer();
            _frontend = new Frontend();

            _player.settings.volume = 10;
            musicFolder = "";
            currentState = MediaPlayerState.Stopped;
        }

        private void FetchMetaData()
        {
            string songName;
            string artist;
            string volume;

            if (currentState == MediaPlayerState.Playing)
            {
                if (_player.currentMedia.getItemInfo("Title") != null)
                {
                    songName = $"Currently playing: {_player.currentMedia.getItemInfo("Title")}";
                }
                else
                {
                    songName = "Unable to retrieve song name";
                }

                if (_player.currentMedia.getItemInfo("Artist") != null)
                {
                    artist = $"Artist: {_player.currentMedia.getItemInfo("Artist")}";
                }
                else
                {
                    artist = "Unable to retrieve Artist name";
                }
            }
            else if (currentState == MediaPlayerState.Paused)
            {
                songName = "Media player is paused";
                artist = "";
            }
            else
            {
                songName = "Media player stopped";
                artist = "";
            }

            if (_player.settings.mute == true)
            {
                volume = "Media player is muted";
            }
            else
            {
                volume = FetchVolume();
            }

            _frontend.PrintInterface(songName, artist, volume);
        }

        private string FetchVolume()
        {
            int currentVolume = _player.settings.volume;
            string barFiller = new string('#', currentVolume / 10);
            string barFillerEmpty = new string(' ', 10 - barFiller.Length);

            string volume = $"Current Volume = {currentVolume}% [{barFiller}{barFillerEmpty}]";
            return volume;
        }

        public void CheckSongFile()
        {
            _frontend.PrintTitle();
            string filePath = "";

            while (filePath == "")
            {
                string path = _frontend.GetInput("Please enter a song file including the path or - to quit.");

                if (path == "-")
                {
                    Environment.Exit(0);
                }
                else if (Path.GetExtension(path) != ".mp3")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("File is not an mp3");
                    Console.ResetColor();
                }
                else if (!File.Exists(path))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect filepath entered or the song does not exist");
                    Console.ResetColor();
                }
                else
                {
                    filePath = path;
                }
            }

            Console.Clear();
            _frontend.PrintTitle();
            musicFolder = filePath;
            PlaySong();
        }

        public int CheckUserInput(int lowerBound, int upperBound, string question)
        {
            bool keepLooping = true;
            int input = lowerBound - 1;
            FetchMetaData();

            while (keepLooping)
            {
                bool isNumber = int.TryParse(_frontend.GetInput(question), out input);

                if (isNumber == false)
                {
                    FetchMetaData();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: input was not a number.");
                    Console.ResetColor();
                    continue;
                }
                else if ((input > upperBound) || (input < lowerBound))
                {
                    FetchMetaData();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: input was out of bounds, input must be between {lowerBound} and {upperBound}");
                    Console.ResetColor();
                    continue;
                }
                else
                {
                    keepLooping = false;
                }
            }

            Console.WriteLine("Processing...");
            return input;
        }

        private void PlaySong()
        {
            _player.URL = musicFolder;
            currentState = MediaPlayerState.Playing;
            Thread.Sleep(500);
        }

        public void PlayPause()
        {
            if (currentState == MediaPlayerState.Playing)
            {
                _player.controls.pause();
                currentState = MediaPlayerState.Paused;
            }
            else
            {
                _player.controls.play();
                currentState = MediaPlayerState.Playing;
            }
        }

        public void ChangeVolume()
        {
            int userVolume = CheckUserInput(0, 100, "Please select a volume level (0-100)");
            _player.settings.volume = userVolume;
        }

        public void MuteUnmute()
        {
            if (_player.settings.mute == true)
            {
                _player.settings.mute = false;
            }
            else
            {
                _player.settings.mute = true;
            }
        }

        public void StopCurrentSong()
        {
            _player.controls.stop();
            currentState = MediaPlayerState.Stopped;
        }
    }
}