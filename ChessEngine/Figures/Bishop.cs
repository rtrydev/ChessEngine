using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using ChessEngine.BoardHandle;

namespace ChessEngine.Figures
{
    class Bishop : Figure
    {
        public Bishop(FigureColor color, Board board, BoardPoint location) : base(color, board, location)
        {
        }

        public Bishop(FigureColor color, Board board, FigureState state) : base(color, board, state)
        {
        }

        public override IEnumerable<BoardPoint> GetPotentialTargetSquares()
        {
            var points = new List<BoardPoint>();
            var x = Location.X;
            var y = Location.Y;

            var tempX = x;
            var tempY = y;

            while (tempX < 8 && tempY < 8)
            {
                points.Add(new BoardPoint(tempX++, tempY++));
            }
            tempX = x;
            tempY = y;
            
            while (tempX < 8 && tempY >= 0)
            {
                points.Add(new BoardPoint(tempX++, tempY--));
            } 
            tempX = x;
            tempY = y;
            while (tempX >= 0 && tempY >= 0)
            {
                points.Add(new BoardPoint(tempX--, tempY--));
            }
            tempX = x;
            tempY = y;
            while (tempX >= 0 && tempY < 8)
            {
                points.Add(new BoardPoint(tempX--, tempY++));
            }   
    

            return points;
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
            if (Math.Abs(point.X - Location.X) != Math.Abs(point.Y - Location.Y)) return false;
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
            
            return base.CheckMoveLegality(point);
        }
    }
}