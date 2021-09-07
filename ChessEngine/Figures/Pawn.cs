using System;
using ChessEngine.BoardHandle;

namespace ChessEngine.Figures
{
    public class Pawn : Figure
    {
        public bool EnPassaintable { get; set; }
        public bool FirstMove { get; set; }
        
        public Pawn(FigureColor color, Board board, BoardPoint point) : base(color, board, point)
        {
            EnPassaintable = false;
            FirstMove = true;
        }

        public Pawn(FigureColor color, Board board, FigureState state) : base(color, board, state)
        {
            EnPassaintable = state.EnPassaintable;
            FirstMove = state.FirstMove;
        }

        public override FigureState GetState()
        {
            return new FigureState()
            {
                EnPassaintable = this.EnPassaintable,
                FirstMove = this.FirstMove,
                Location = new BoardPoint(this.Location.X, this.Location.Y)
            };
        }

        public override bool CheckMoveLegality(BoardPoint point)
        {
            var legalDirection = Color == FigureColor.White ? 1 : -1;

            if (!FirstMove)
            {
                if (point.Y - Location.Y != legalDirection) return false;
            }
            else
            {
                if (point.Y - Location.Y != 2 * legalDirection && point.Y - Location.Y != legalDirection) return false;
            }
            

            var figureOnDestination = Board.GetFigureOnLocation(point);
            var enPassaintLegality = false;
            if (figureOnDestination is not null)
            {
                if (Math.Abs(point.X - Location.X) != 1 || (point.Y - Location.Y) * legalDirection != 1) return false;
            }
            else
            {
                if (point.X - Location.X != 0)
                {
                    enPassaintLegality = CheckEnPassaint(point, legalDirection);
                    if (!enPassaintLegality) return false;
                }
                
            }
            return base.CheckMoveLegality(point);
        }

        private bool CheckEnPassaint(BoardPoint point, int direction)
        {
            var enPassaintPosition = new BoardPoint(point.X, point.Y - direction);
            var figureOnEnPassaint = Board.GetFigureOnLocation(enPassaintPosition);
            if (figureOnEnPassaint is Pawn)
            {
                var pawn = figureOnEnPassaint as Pawn;
                if (pawn.EnPassaintable)
                {
                    return true;
                }
            }

            return false;
        }

        public override void ChangeLocation(BoardPoint point)
        {
            if (FirstMove)
            {
                FirstMove = false;
                var moveLength = Math.Abs(point.Y - Location.Y);
                EnPassaintable = moveLength == 2;
            }

            base.ChangeLocation(point);
        }
    }
}