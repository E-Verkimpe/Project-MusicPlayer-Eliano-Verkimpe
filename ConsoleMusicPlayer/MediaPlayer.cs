using WMPLib;

namespace ConsoleMusicPlayer
{
    public class MediaPlayer
    {
        private WindowsMediaPlayer player = new WindowsMediaPlayer();

        private string musicFolder;
        private MediaPlayerState currentState = MediaPlayerState.Stopped;

        public MediaPlayer()
        {
            player.settings.volume = 10;
            musicFolder = "";
        }

        private void PrintTitle()
        {
            string[] title = new string[8];
            title[0] = @"___  ___         _ _         ______ _";
            title[1] = @"|  \/  |        | (_)        | ___ \ |";
            title[2] = @"| .  . | ___  __| |_  __ _   | |_/ / | __ _ _   _  ___ _ __ ";
            title[3] = @"| |\/| |/ _ \/ _` | |/ _` |  |  __/| |/ _` | | | |/ _ \ '__|";
            title[4] = @"| |  | |  __/ (_| | | (_| |  | |   | | (_| | |_| |  __/ |";
            title[5] = @"\_|  |_/\___|\__,_|_|\__,_|  \_|   |_|\__,_|\__, |\___|_|";
            title[6] = @"                                             __/ |";
            title[7] = @"                                            |___/           ";

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            PrintStringCenter(title);
            Console.ResetColor();
            Console.WriteLine();
        }

        private void PrintMenu()
        {
            string songName;
            string artist;
            string volume;

            if (currentState == MediaPlayerState.Playing)
            {
                if (player.currentMedia.getItemInfo("Title") != null)
                {
                    songName = $"Currently playing: {player.currentMedia.getItemInfo("Title")}";
                }
                else
                {
                    songName = "Unable to retrieve song name";
                }

                if (player.currentMedia.getItemInfo("Artist") != null)
                {
                    artist = $"Artist: {player.currentMedia.getItemInfo("Artist")}";
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

            if (player.settings.mute == true)
            {
                volume = "Media player is muted";
            }
            else
            {
                volume = FetchVolume();
            }

            string[] menuArray = new string[10];
            menuArray[0] = "╔════╦════════════════════╗";
            menuArray[1] = "║ #  ║ Function           ║";
            menuArray[2] = "╠════╬════════════════════╣";
            menuArray[3] = "║ 0  ║ Quit               ║";
            menuArray[4] = $"║ 1  ║ Play/Pause         ║\t\t{songName}";
            menuArray[5] = "║ 2  ║ Change volume      ║";
            menuArray[6] = $"║ 3  ║ Mute/Unmute        ║\t\t{artist}";
            menuArray[7] = "║ 4  ║ Play new song      ║";
            menuArray[8] = $"║ 5  ║ Stop current song  ║\t\t{volume}";
            menuArray[9] = "╚════╩════════════════════╝";

            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintStringCenter(menuArray);
            Console.ResetColor();
        }

        private void PrintStringCenter(string[] text)
        {
            int longestLength = 0;
            for (int i = 0; i < text.Length; i++)
            {
                int lenght = text[i].Length;
                if (lenght > longestLength)
                {
                    longestLength = lenght;
                }
            }

            string leadingSpaces = new string(' ', (Console.WindowWidth - longestLength) / 2);
            for (int i = 0; i < text.Length; i++)
            {
                text[i] = leadingSpaces + text[i];
            }

            var centeredText = string.Join(Environment.NewLine, text);
            Console.WriteLine(centeredText);
        }

        private void PrintInterface()
        {
            PrintTitle();
            PrintMenu();
        }

        private static string GetInput(string requestedInfo)
        {
            Console.WriteLine(requestedInfo);
            string userInput = Console.ReadLine();
            return userInput;
        }

        public void CheckSongFile()
        {
            PrintTitle();
            string filePath = "";

            while (filePath == "")
            {
                string path = GetInput("Please enter a song file including the path or - to quit.");

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
            PrintTitle();
            musicFolder = filePath;
            PlaySong();
        }

        public int CheckUserInput(int lowerBound, int upperBound, string question)
        {
            bool keepLooping = true;
            int input = lowerBound - 1;
            PrintInterface();

            while (keepLooping)
            {
                bool isNumber = int.TryParse(GetInput(question), out input);

                if (isNumber == false)
                {
                    PrintInterface();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: input was not a number.");
                    Console.ResetColor();
                    continue;
                }
                else if ((input > upperBound) || (input < lowerBound))
                {
                    PrintInterface();
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
            player.URL = musicFolder;
            currentState = MediaPlayerState.Playing;
            Thread.Sleep(500);
        }

        public void PlayPause()
        {
            if (currentState == MediaPlayerState.Playing)
            {
                player.controls.pause();
                currentState = MediaPlayerState.Paused;
            }
            else
            {
                player.controls.play();
                currentState = MediaPlayerState.Playing;
            }
        }

        public void ChangeVolume()
        {
            int userVolume = CheckUserInput(0, 100, "Please select a volume level (0-100)");
            player.settings.volume = userVolume;
        }

        private string FetchVolume()
        {
            int currentVolume = player.settings.volume;
            string barFiller = new string('#', currentVolume / 10);
            string barFillerEmpty = new string(' ', 10 - barFiller.Length);
            string volume = $"Current Volume = {currentVolume}% [{barFiller}{barFillerEmpty}]";
            return volume;
        }

        public void MuteUnmute()
        {
            if (player.settings.mute == true)
            {
                player.settings.mute = false;
            }
            else
            {
                player.settings.mute = true;
            }
        }

        public void StopCurrentSong()
        {
            player.controls.stop();
            currentState = MediaPlayerState.Stopped;
        }
    }
}