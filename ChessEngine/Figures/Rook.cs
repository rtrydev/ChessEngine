using System;
using System.Collections.Generic;
using ChessEngine.BoardHandle;

namespace ChessEngine.Figures
{
    public class Rook : Figure
    {
        public bool CanCastle { get; set; }
        public Rook(FigureColor color, Board board, BoardPoint location) : base(color, board, location)
        {
            CanCastle = false;
        }

        public Rook(FigureColor color, Board board, FigureState state) : base(color, board, state)
        {
            CanCastle = state.CanCastle;
        }

        public override bool CheckMoveLegality(BoardPoint point)
        {
            if (point.X - Location.X != 0 && point.Y - Location.Y != 0) return false;
            
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


            return base.CheckMoveLegality(point);
        }

        public override FigureState GetState()
        {
            return new FigureState()
            {
                CanCastle = this.CanCastle,
                Location = new BoardPoint(this.Location.X, this.Location.Y)
            };
        }

        public override void ChangeLocation(BoardPoint point)
        {
            this.CanCastle = false;
            base.ChangeLocation(point);
        }
    }
}