using System;
using System.Collections.Generic;
using ChessEngine.BoardHandle;

namespace ChessEngine.Figures
{
    class Queen : Figure
    {
        public Queen(FigureColor color, Board board, BoardPoint location) : base(color, board, location)
        {
        }

        public Queen(FigureColor color, Board board, FigureState state) : base(color, board, state)
        {
        }

        public override FigureState GetState()
        {
            return new FigureState()
            {
                Location = new BoardPoint(this.Location.X, this.Location.Y)
            };
        }

        public override bool CheckMoveLegality(BoardPoint point)
        {
            var changeX = Math.Abs(point.X - Location.X);
            var changeY = Math.Abs(point.Y - Location.Y);
            
            if(changeX != 0 && changeY != 0)
                if (changeX != changeY)
                    return false;
            if (changeX == changeY)
            {
                var multiplierX = Math.Sign(point.X - Location.X);
                var multiplierY = Math.Sign(point.Y - Location.Y);

                var upper = Math.Abs(point.X - Location.X);
                var pointsBetween = new List<BoardPoint>();

                for (int i = 1; i < upper; i++)
                {
                    pointsBetween.Add(new BoardPoint(Location.X + i * multiplierX, Location.Y + i * multiplierY));
                }

                foreach (var location in pointsBetween)
                {
                    if (Board.GetFigureOnLocation(location) is not null) return false;
                }
            }
            else
            {
                int multiplierX = 0;
                int multiplierY = 0;
                if (point.X - Location.X != 0) multiplierX = Math.Sign(point.X - Location.X);
                if (point.Y - Location.Y != 0) multiplierY = Math.Sign(point.Y - Location.Y);
            
                var pointsBetween = new List<BoardPoint>();
                var upper = (multiplierX != 0) ? Math.Abs(point.X - Location.X) : Math.Abs(point.Y - Location.Y);
            
                for (int i = 1; i < upper; i++)
                {
                    pointsBetween.Add(new BoardPoint(Location.X + i * multiplierX, Location.Y + i * multiplierY));
                }

                foreach (var location in pointsBetween)
                {
                    if (Board.GetFigureOnLocation(location) is not null) return false;
                }
            }
            
            return base.CheckMoveLegality(point);
        }
    }
}