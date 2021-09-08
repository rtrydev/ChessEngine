using System;
using System.Collections.Generic;
using ChessEngine.BoardHandle;

namespace ChessEngine.Figures
{
    public class King : Figure
    {
        public bool CanCastle { get; set; }
        public King(FigureColor color, Board board, BoardPoint location) : base(color, board, location)
        {
            CanCastle = true;
        }
        
        public King(FigureColor color, Board board, FigureState state) : base(color, board, state)
        {
            CanCastle = state.CanCastle;
        }
        
        public override void ChangeLocation(BoardPoint point)
        {
            CanCastle = false;
            base.ChangeLocation(point);
        }

        public override FigureState GetState()
        {
            return new FigureState()
            {
                CanCastle = this.CanCastle,
                Location = new BoardPoint(this.Location.X, this.Location.Y)
            };
        }

        public override bool CheckMoveLegality(BoardPoint point)
        {
            if (Math.Abs(point.Y - Location.Y) > 1) return false;
            if (!CanCastle || Board.LookForCheckOnLocation(Location, Color))
            {
                if (Math.Abs(point.X - Location.X) > 1) return false;
            }
            else
            {
                var lenght = Math.Abs(point.X - Location.X);
                if (lenght > 2) return false;
                if (lenght == 2)
                {
                    if (point.Y != Location.Y) return false;
                    var direction = Math.Sign(point.X - Location.X);
                    for (int i = lenght; i < 5; i++)
                    {
                        var searchX = Location.X + i * direction;
                        if (searchX > 7 || searchX < 0)
                        {
                            return false;
                        }

                        var rook = Board.GetFigureOnLocation(new BoardPoint(searchX, Location.Y));
                        if (rook is null) continue;
                        if (rook is not Rook) return false;
                        if (rook.Color != Color) return false;
                        if (!(rook as Rook).CanCastle) return false;
                        break;
                    }
                    var figure = Board.GetFigureOnLocation(new BoardPoint(Location.X + direction, Location.Y));
                    if (figure is not null)
                    {
                        return false;
                    }

                    var pointsBetween = new List<BoardPoint>();
                    for (int i = 1; i < lenght; i++)
                    {
                        pointsBetween.Add(new BoardPoint(Location.X + i * direction, Location.Y));
                    }
                    
                    foreach (var location in pointsBetween)
                    {
                        if (Board.LookForCheckOnLocation(location, Color)) return false;
                    }
                }

            }
            
            return base.CheckMoveLegality(point);
        }
    }
}