using System;
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
            handler.InitializeGame();
            handler.StartGame();
        }
    }
}