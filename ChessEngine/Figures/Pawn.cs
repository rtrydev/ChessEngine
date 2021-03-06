using System;
using System.Collections.Generic;
using ChessEngine.BoardHandle;
using Microsoft.VisualBasic;

namespace ChessEngine.Figures
{
    class Pawn : Figure
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
        
        public override IEnumerable<BoardPoint> GetPotentialTargetSquares()
        {
            var points = new List<BoardPoint>();
            
            int startX = Location.X > 0 ? Location.X - 1 : 0;
            int endX = Location.X > 6 ? 7 : Location.X + 1;
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

            if (Math.Abs(point.X - Location.X) > 1) return false;
            
            if (!FirstMove)
            {
                if (point.Y - Location.Y != legalDirection) return false;
            }
            else
            {
                if (point.Y - Location.Y != 2 * legalDirection && point.Y - Location.Y != legalDirection) return false;
                if (Board.GetFigureOnLocation(new BoardPoint(Location.X, Location.Y + legalDirection)) is not null)
                    if(Math.Abs(point.Y - Location.Y) != 1 && Math.Abs(point.X - Location.X) != 1)
                        return false;
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
                    enPassaintLegality = CheckEnPassaint(point, legalDirection, Color);
                    if (!enPassaintLegality) return false;
                }
                
            }
            return base.CheckMoveLegality(point);
        }

        private bool CheckEnPassaint(BoardPoint point, int direction, FigureColor color)
        {
            var enPassaintPosition = new BoardPoint(point.X, point.Y - direction);
            var figureOnEnPassaint = Board.GetFigureOnLocation(enPassaintPosition);
            if (figureOnEnPassaint is Pawn)
            {
                if (figureOnEnPassaint.Color == color) return false;
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