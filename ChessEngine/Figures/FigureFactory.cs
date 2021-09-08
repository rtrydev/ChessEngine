using System.Diagnostics;
using ChessEngine.BoardHandle;

namespace ChessEngine.Figures
{
    class FigureFactory
    {
        public static Figure CreateFigure(string name, BoardPoint locationOnBoard, FigureColor color, Board board)
        {
            return name switch
            {
                "p" => new Pawn(color, board, locationOnBoard),
                "k" => new King(color, board, locationOnBoard),
                "b" => new Bishop(color, board, locationOnBoard),
                "n" => new Knight(color, board, locationOnBoard),
                "r" => new Rook(color, board, locationOnBoard),
                "q" => new Queen(color, board, locationOnBoard),
                _ => null
            };
        }

        public static Figure CreateFigureWithState(string name, FigureColor color, Board board, FigureState state)
        {
            return name switch
            {
                "p" => new Pawn(color, board, state),
                "k" => new King(color, board, state),
                "b" => new Bishop(color, board, state),
                "n" => new Knight(color, board, state),
                "r" => new Rook(color, board, state),
                "q" => new Queen(color, board, state),
                _ => null
            };
        }
    }
}