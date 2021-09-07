using System;
using ChessEngine.BoardHandle;

namespace ChessEngine.Figures
{
    public class Knight : Figure
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