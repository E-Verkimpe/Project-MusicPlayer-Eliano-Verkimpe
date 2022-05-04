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
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            PrintStringCenter(title);
            Console.ResetColor();
            Console.WriteLine();
        }

        private void PrintMenu()
        {
            string menu = @"
╔════╦════════════════════╗
║ #  ║ Function           ║
╠════╬════════════════════╣
║ 0  ║ Quit               ║
║ 1  ║ Play/Pause         ║
║ 2  ║ Change volume      ║
║ 3  ║ Mute/Unmute        ║
║ 4  ║ Play new song      ║
║ 5  ║ Stop current song  ║
╚════╩════════════════════╝";
            Console.ForegroundColor= ConsoleColor.Cyan;
            PrintStringCenter(menu);
            Console.ResetColor();
        }

        private void PrintStringCenter(string text)
        {
            using (StringReader reader = new StringReader(text))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - line.Length) / 2, Console.CursorTop);
                        Console.WriteLine(line);
                    }
                } while (line != null);
            }
        }

        public void PrintInterface()
        {
            PrintTitle();
            PrintMenu();
        }

        private string GetInput(string requestedInfo)
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
                string path = GetInput("Please enter a song file including the path.");

                if (path == "-")
                {
                    filePath = path;
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
            MusicFolder = filePath;
            PlaySong();
        }

        public int CheckUserInput(int lowerBound, int upperBound, string question)
        {
            bool keepLooping = true;
            int input = lowerBound - 1;

            while (keepLooping)
            {
                bool isNumber = int.TryParse(GetInput(question), out input);

                if (isNumber == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: input was not a number.");
                    Console.ResetColor();
                    continue;
                }
                else if ((input > upperBound) || (input < lowerBound))
                {
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
            PrintTitle();
            Console.WriteLine($"current volume level: {player.settings.volume}");
            int userVolume = CheckUserInput(0, 100, "Please select a volume level (0-100)");
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
            isPlaying = false;
        }
    }
}