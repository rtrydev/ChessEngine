using System;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using ChessEngine.GameHandle;

namespace ChessEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new GameHandler();
            var lines = File.ReadAllLines("/home/rtry/testperft");
            int iterator = 1;
            for (int i = 0; i < lines.Length; i++)
            {
                var separated = lines[i].Split(",");
                handler = new GameHandler();
                var value = handler.CalculateNodeCount(separated[0], null, 5, false);
                if (value == Convert.ToInt32(separated[5])) Console.WriteLine($"OK {iterator++}");
                else
                {
                    Console.WriteLine($"FAIL {iterator++} {Convert.ToInt32(separated[5])} vs {value}");
                }
            }

        }
    }
}