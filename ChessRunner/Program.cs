using System;
using ChessEngine.GameHandle;

namespace ChessRunner
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