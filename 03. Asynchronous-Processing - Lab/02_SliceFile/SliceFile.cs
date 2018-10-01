namespace _02_SliceFile
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class SliceFile
    {
        public static void Main(string[] args)
        {
            var videoPath = Console.ReadLine();
            var destination = Console.ReadLine();
            var pieces = int.Parse(Console.ReadLine());

            SliceAsync(videoPath, destination, pieces);

            Console.WriteLine("Anything else?");
            while (true)
            {
                Console.ReadLine();
            }
        }

        public static void SliceAsync(string sourceFile, string destinationPath, int parts)
        {
            Task.Run(() =>
            {
                Slice(sourceFile, destinationPath, parts);
            });
        }

        public static void Slice(string sourceFile, string destinationPath, int parts)
        {
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            using (var source = new FileStream(sourceFile, FileMode.Open))
            {
                var fileInfo = new FileInfo(sourceFile);

                var partLength = (source.Length / parts) + 1;
                var currentByte = 0;

                for (int currentPart = 1; currentPart <= parts; currentPart++)
                {
                    var filePath = string.Format("{0}/Part-{1}{2}", destinationPath, currentPart, fileInfo.Extension);

                    using (var destination = new FileStream(filePath, FileMode.Create))
                    {
                        byte[] buffer = new byte[partLength];

                        while (currentByte <= partLength * currentPart)
                        {
                            int readBytesCount = source.Read(buffer, 0, buffer.Length);
                            if (readBytesCount == 0)
                            {
                                break;
                            }

                            destination.Write(buffer, 0, readBytesCount);
                            currentByte += readBytesCount;

                            Console.WriteLine("Slice complete.");
                        }
                    }
                }
            }
        }
    }
}
