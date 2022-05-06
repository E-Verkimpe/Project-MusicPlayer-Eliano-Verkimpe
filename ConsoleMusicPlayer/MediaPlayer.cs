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

        public void FetchMetaData()
        {
            string songName;
            string artist;
            string volume;
            if (currentState == MediaPlayerState.Playing)
            {
                if (_player.currentMedia.getItemInfo("Title") != "")
                {
                    songName = $"Currently playing: {_player.currentMedia.getItemInfo("Title")}";
                }
                else
                {
                    songName = "Unable to retrieve song name";
                }

                if (_player.currentMedia.getItemInfo("Artist") != "")
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

        private string[] ProcessDirectory(string[] songFiles, string path)
        {
            string[] displaySongFiles;

            if (songFiles.Length <= 5)
            {
                displaySongFiles = new string[songFiles.Length];
            }
            else
            {
                displaySongFiles = new string[5];
            }

            for (int i = 0; i < displaySongFiles.Length; i++)
            {
                displaySongFiles[i] = songFiles[i].Substring(path.Length + 1, songFiles[i].Length - path.Length - 5);
            }
            return displaySongFiles;
        }

        public void CheckSongFile()
        {
            _frontend.PrintTitle();
            string path = "";
            bool keepLooping = true;

            while (keepLooping)
            {
                path = _frontend.GetInput("Please enter a song file including the path or - to quit.");

                if (path == "-")
                {
                    Environment.Exit(0);
                }
                else if ((Path.GetExtension(path) == ".mp3") && (File.Exists(path)))
                {
                    musicFolder = path;
                    keepLooping = false;
                }
                else if (Directory.Exists(path))
                {
                    string[] songFiles = Directory.GetFiles(path, "*.mp3");

                    if (songFiles.Length == 0)
                    {
                        _frontend.PrintTitle();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No mp3 files found in the directory.");
                        Console.ResetColor();
                    }
                    else
                    {
                        string[] songNames = ProcessDirectory(songFiles, path);
                        _frontend.PrintSongNames(songNames);
                        int chosenSong = CheckUserInput(0, songNames.Length, $"Pick a song from the list: (0 - {songNames.Length})");

                        if (chosenSong == 0)
                        {
                            Environment.Exit(0);
                        }
                        else
                        {
                            musicFolder = songFiles[chosenSong - 1];
                            keepLooping = false;
                        }
                    }
                }
                else
                {
                    _frontend.PrintTitle();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("File is not an mp3 or directory not found.");
                    Console.ResetColor();
                }
            }

            PlaySong();
        }

        public int CheckUserInput(int lowerBound, int upperBound, string question)
        {
            bool keepLooping = true;
            int input = lowerBound - 1;

            while (keepLooping)
            {
                bool isNumber = int.TryParse(_frontend.GetInput(question), out input);

                if (isNumber == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: input was not a number.");
                    Console.ResetColor();
                }
                else if ((input > upperBound) || (input < lowerBound))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: input was out of bounds, input must be between {lowerBound} and {upperBound}");
                    Console.ResetColor();
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
            Thread.Sleep(500); //wait for the song to load before fetching metadata.
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
            FetchMetaData();
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