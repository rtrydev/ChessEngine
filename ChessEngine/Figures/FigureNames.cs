using System;
using System.Runtime.CompilerServices;

namespace ChessEngine.Figures
{
    public class FigureNames
    {
        public static string GetFigureName(Figure figure)
        {
            var name = figure switch
            {
                Pawn => "p",
                King => "k",
                Bishop => "b",
                Knight => "n",
                Rook => "r",
                Queen => "q",
                _ => " "
            };
            

            return name;
        }
    }
}