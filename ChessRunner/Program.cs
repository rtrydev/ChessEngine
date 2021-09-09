using System;
using ChessEngine.GameHandle;

namespace ChessRunner
{
    class Program
    {
        static void Main(string[] args)
        {

            var white = 0;
            var black = 0;
            var draw = 0;


            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(i);
                var handler = new GameHandler();
                handler.InitializeGame();
                while (handler.GameState == GameState.Ongoing)
                {
                    handler.SendMove(handler.GetNextMove());
                }

                if (handler.GameState == GameState.Draw) draw++;
                if (handler.GameState == GameState.BlackKingMated) white++;
                if (handler.GameState == GameState.WhiteKingMated) black++;
            }

            Console.WriteLine("white: " + white);            
            Console.WriteLine("black: " + black);            
            Console.WriteLine("draw: " + draw);            
        }
    }
}