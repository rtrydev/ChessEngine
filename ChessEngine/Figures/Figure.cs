using System.Collections;
using System.Collections.Generic;
using ChessEngine.BoardHandle;

namespace ChessEngine.Figures
{
    public enum FigureColor
    {
        White,
        Black
    }
    
    public abstract class Figure
    {
        public FigureColor Color { get; }
        public Board Board { get; }
        public BoardPoint Location { get; set; }

        public abstract FigureState GetState();

        public Figure(FigureColor color, Board board, BoardPoint location)
        {
            Board = board;
            Color = color;
            Location = new BoardPoint(location.X, location.Y);
        }

        public Figure(FigureColor color, Board board, FigureState state)
        {
            Board = board;
            Color = color;
            Location = new BoardPoint(state.Location.X, state.Location.Y);
        }
        public virtual bool CheckMoveLegality(BoardPoint point)
        {
            var figureOnDestination = Board.GetFigureOnLocation(point);
            if (figureOnDestination is not null)
            {
                if (Color == figureOnDestination.Color) return false;
            }
            if (point == Location) return false;
            
            return true;
        }

        public virtual void ChangeLocation(BoardPoint point)
        {
            Location.X = point.X;
            Location.Y = point.Y;
        }

        public virtual IEnumerable<BoardPoint> GetPotentialTargetSquares()
        {
            return null;
        }
        
    }
}