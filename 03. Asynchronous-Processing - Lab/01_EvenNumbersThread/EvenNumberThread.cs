namespace _01_EvenNumbersThread
{
    using System;
    using System.Threading;

    public class EvenNumberThread
    {
        public static void Main(string[] args)
        {
            var startNum = int.Parse(Console.ReadLine());
            var endNum = int.Parse(Console.ReadLine());

            var thread = new Thread(() => PrintEvenNumbers(startNum, endNum));
            thread.Start();
            thread.Join();
            Console.WriteLine("Thread finished work");
        }

        public static void PrintEvenNumbers(int startNum, int endNum)
        {
            for (int i = startNum; i <= endNum; i++)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine(i);
                }
            }
        }
    }
}
