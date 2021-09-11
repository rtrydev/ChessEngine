using System;
using System.Collections.Generic;
using ChessEngine.BoardHandle;

namespace ChessEngine.Figures
{
    class Knight : Figure
    {
        public Knight(FigureColor color, Board board, BoardPoint location) : base(color, board, location)
        {
        }

        public Knight(FigureColor color, Board board, FigureState state) : base(color, board, state)
        {
        }

        public override FigureState GetState()
        {
            return new FigureState()
            {
                Location = new BoardPoint(this.Location.X, this.Location.Y)
            };
        }
        
        public override IEnumerable<BoardPoint> GetPotentialTargetSquares()
        {
            var points = new List<BoardPoint>();
            int startX = Location.X > 1 ? Location.X - 2 : 0;
            int endX = Location.X > 5 ? 7 : Location.X + 2;
            int startY = Location.Y > 1 ? Location.Y - 2 : 0;
            int endY = Location.Y > 5 ? 7 : Location.Y + 2;
            
            for (int j = startX; j <= endX; j++)
            {
                for (int i = startY; i <= endY; i++)
                {
                    points.Add(new BoardPoint(j ,i));
                    
                }
            }

            return points;
        }

        public override bool CheckMoveLegality(BoardPoint point)
        {
            var changeX = Math.Abs(point.X - Location.X);
            var changeY = Math.Abs(point.Y - Location.Y);

            if (changeX != 1 && changeY != 1) return false;

            if (changeX != 2 * changeY && changeY != 2 * changeX) return false;
            
            return base.CheckMoveLegality(point);
        }
    }
}