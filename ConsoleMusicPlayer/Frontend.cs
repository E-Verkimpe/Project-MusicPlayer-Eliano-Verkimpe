namespace ConsoleMusicPlayer
{
    internal class Frontend
    {
        public void PrintTitle()
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

        private void PrintMenu(string songName, string artist, string volume)
        {
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

        public void PrintSongNames(string[] songNames)
        {
            string[] songPaths = new string[songNames.Length + 3];
            songPaths[0] = "#\tSong";
            songPaths[1] = "=============================";
            songPaths[2] = "0\tQuit";
            for (int i = 0; i < songNames.Length; i++)
            {
                songPaths[i + 3] = $"{i + 1}\t{songNames[i]}";
            }

            Console.Clear();
            PrintTitle();
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintStringCenter(songPaths);
            Console.ResetColor();
        }

        public void PrintInterface(string songName, string artist, string volume)
        {
            PrintTitle();
            PrintMenu(songName, artist, volume);
        }

        public string GetInput(string requestedInfo)
        {
            Console.WriteLine(requestedInfo);
            string userInput = Console.ReadLine();
            return userInput;
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
    }
}