using WMPLib;

namespace ConsoleMusicPlayer
{
    public class MediaPlayer
    {
        private WindowsMediaPlayer player = new WindowsMediaPlayer();

        public string MusicFolder { get; private set; }

        public MediaPlayer()
        {
            PrintTitle();
            player.settings.volume = 5;
        }

        private void PrintTitle()
        {
            Console.WriteLine(String.Format("{0," + Console.WindowWidth / 2 + "}", "-- Media Player -- "));
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
        }

        public void PlaySong()
        {
            player.URL = MusicFolder;
        }
    }
}