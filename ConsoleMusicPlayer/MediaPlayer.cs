using WMPLib;

namespace ConsoleMusicPlayer
{
    public class MediaPlayer
    {
        private WindowsMediaPlayer player = new WindowsMediaPlayer();

        public string MusicFolder { get; private set; }
        private bool isPlaying = false;

        public MediaPlayer()
        {
            PrintTitle();
            player.settings.volume = 5;
            MusicFolder = "";
        }

        private void PrintTitle()
        {
            string title = @"
            ___  ___         _ _         ______ _                       
            |  \/  |        | (_)        | ___ \ |                      
            | .  . | ___  __| |_  __ _   | |_/ / | __ _ _   _  ___ _ __ 
            | |\/| |/ _ \/ _` | |/ _` |  |  __/| |/ _` | | | |/ _ \ '__|
            | |  | |  __/ (_| | | (_| |  | |   | | (_| | |_| |  __/ |   
            \_|  |_/\___|\__,_|_|\__,_|  \_|   |_|\__,_|\__, |\___|_|   
                                                         __/ |          
                                                        |___/           ";
            //Console.WriteLine(String.Format("{0," + Console.WindowWidth / 2 + "}", title));
            Console.Write("\t");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(title);
            Console.WriteLine();
            Console.ResetColor();
        }

        private void PrintMenu()
        {
            Console.WriteLine("0) Quit");
            Console.WriteLine("1) Pause/Play");
            Console.WriteLine("2) Change volume");
            Console.WriteLine("3) Mute/Unmute");
            Console.WriteLine("4) Play song");
            Console.WriteLine("5) Stop current song");
        }

        private void PrintInterface()
        {
            Console.Clear();
            PrintTitle();
            PrintMenu();
        }

        private string GetInput(string requestedInfo)
        {
            Console.WriteLine(requestedInfo);
            string userInput = Console.ReadLine();
            return userInput;
        }

        public void GetSongFile()
        {
            string filePath = "";

            while (filePath == "")
            {
                string path = GetInput("Please enter a song file including the path.");

                if (path == "-")
                {
                    filePath = path;
                }
                else if (Path.GetExtension(path) != ".mp3")
                {
                    Console.WriteLine("File is not an mp3");
                }
                else if (!File.Exists(path))
                {
                    Console.WriteLine("Incorrect filepath entered");
                }
                else
                {
                    filePath = path;
                }
            }

            Console.Clear();
            PrintTitle();
            MusicFolder = filePath;
            PlaySong();
        }

        public int GetUserChoice(int lowerBound, int upperBound)
        {
            PrintInterface();
            bool keepLooping = true;
            int input = lowerBound -1;

            while (keepLooping)
            {
                bool isNumber = int.TryParse(GetInput($"Please make a choice ({lowerBound}-{upperBound})"), out input);

                if (isNumber == false)
                {
                    Console.WriteLine("Error: input was not a number.");
                    continue;
                }
                else if ((input > upperBound) || (input < lowerBound))
                {
                    Console.WriteLine($"Error: input was out of bounds ({lowerBound}-{upperBound})");
                    continue;
                }
                else
                {
                    keepLooping = false;
                }
            }
            return input;
        }

        private void PlaySong()
        {
            player.URL = MusicFolder;
            isPlaying = true;
        }

        public void PlayPause()
        {
            if (isPlaying == true)
            {
                player.controls.pause();
                isPlaying = false;
            }
            else
            {
                player.controls.play();
                isPlaying = true;
            }
        }

        public void ChangeVolume()
        {
            int userVolume = GetUserChoice(0, 100);
            player.settings.volume = userVolume;
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
        }
    }
}