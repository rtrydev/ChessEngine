using System;
using ChessEngine.GameHandle;

namespace ChessEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new GameHandler();
            handler.StartGame();
        }
    }
}