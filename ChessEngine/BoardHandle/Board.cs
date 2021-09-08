using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using ChessEngine.Figures;

namespace ChessEngine.BoardHandle
{
    public class Board
    {
        private Figure[][] _boardState;

        public Figure[][] BoardState => _boardState;

        public Figure GetFigureOnLocation(BoardPoint point)
        {
            if (point is null) return null;
            return _boardState[point.X][point.Y];
        }
        public void AddFigureOnLocation(string figureName, BoardPoint location, FigureColor color)
        {
            _boardState[location.X][location.Y] = FigureFactory.CreateFigure(figureName, location, color, this);
        }

        public void CopyFigureToLocation(Figure figure)
        {
            var state = figure.GetState();
            var figureName = FigureNames.GetFigureName(figure);
            _boardState[state.Location.X][state.Location.Y] =
                FigureFactory.CreateFigureWithState(figureName, figure.Color, this, state);
        }

        private void SetFigureOnLocation(BoardPoint location, Figure figure)
        {
            _boardState[location.X][location.Y] = figure;
            figure?.ChangeLocation(location);
        }

        public bool CheckMoveLegality(BoardPoint fromLocation, BoardPoint toLocation)
        {
            var figure = GetFigureOnLocation(fromLocation);
            if (figure is null) return false;
            var legalByFigure = figure.CheckMoveLegality(toLocation);
            if (legalByFigure)
            {
                var boardCopy = new Board(this);
                if (figure is Pawn)
                {
                    var figureOnLocation = GetFigureOnLocation(toLocation);
                    if (figureOnLocation is null && fromLocation.X != toLocation.X)
                    {
                        boardCopy.MoveFigureToLocation(fromLocation, toLocation, SpecialMove.EnPassaint);
                        if (boardCopy.LookForCheckByColor(figure.Color)) return false;
                    }
                }
                
                boardCopy.MoveFigureToLocation(fromLocation, toLocation, SpecialMove.None);
                if (boardCopy.LookForCheckByColor(figure.Color)) return false;
                
                
            }
            else
            {
                return false;
            }

            return true;
        }

        public void PromotePawn(BoardPoint fromLocation, BoardPoint toLocation, string promotionDestination)
        {
            var pawn = GetFigureOnLocation(fromLocation);
            SetFigureOnLocation(fromLocation, null);
            Figure figure = promotionDestination switch
            {
                "q" => new Queen(pawn.Color, this, toLocation),
                "n" => new Knight(pawn.Color, this, toLocation),
                "b" => new Bishop(pawn.Color, this, toLocation),
                "r" => new Rook(pawn.Color, this, toLocation),
                _ => throw new ArgumentException()
            };
            SetFigureOnLocation(toLocation, figure);
        }

        public void MoveFigureToLocation(BoardPoint fromLocation, BoardPoint toLocation, SpecialMove move)
        {
            var figure = GetFigureOnLocation(fromLocation);
            if(figure is null) return;
            var pawnsOfColor = GetPawnsOfColor(figure.Color);
            foreach (var pawn in pawnsOfColor)
            {
                pawn.EnPassaintable = false;
            }

            switch (move)
            {
                case SpecialMove.None:
                {
                    SetFigureOnLocation(fromLocation, null);
                    SetFigureOnLocation(toLocation, figure);
                    break;
                }
                case SpecialMove.EnPassaint:
                {
                    SetFigureOnLocation(fromLocation, null);
                    var takeLocation = new BoardPoint(toLocation.X, fromLocation.Y);
                    SetFigureOnLocation(takeLocation, null);
                    SetFigureOnLocation(toLocation,figure);
                    break;
                }
                case SpecialMove.Castle:
                {
                    SetFigureOnLocation(fromLocation, null);
                    SetFigureOnLocation(toLocation, figure);
                    var direction = Math.Sign(toLocation.X - fromLocation.X);
                    for (int i = 1; i < 3; i++)
                    {
                        var rookLocation = new BoardPoint(toLocation.X + i * direction, fromLocation.Y);
                        var rook = GetFigureOnLocation(rookLocation);
                        if (rook is not null)
                        {
                            SetFigureOnLocation(rookLocation, null);
                            SetFigureOnLocation(new BoardPoint(toLocation.X - direction, fromLocation.Y), rook);
                            (rook as Rook).CanCastle = false;
                            (figure as King).CanCastle = false;
                            break;
                        }
                    }
                    break;
                }
                case SpecialMove.Promote:
                {
                    break;
                }
                default: break;
            }
            
            
        }

        public BoardPoint FindKingByColor(FigureColor color)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var figure = GetFigureOnLocation(new BoardPoint(i, j));
                    if (figure is null) continue;
                    if (figure.Color == color)
                    {
                        if (figure is King) return figure.Location;
                    }
                }
            }

            return null;
        }

        public bool LookForCheckByColor(FigureColor color)
        {
            var kingLocation = FindKingByColor(color);
            if (kingLocation is null) return false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var figure = GetFigureOnLocation(new BoardPoint(i, j));
                    if (figure is not null)
                    {
                        if (figure.CheckMoveLegality(kingLocation)) return true;
                    }
                }
            }

            return false;
        }

        public bool LookForCheckOnLocation(BoardPoint location, FigureColor color)
        {
            var placedDummy = false;
            if (GetFigureOnLocation(location) is null)
            {
                AddFigureOnLocation("p", location, color);
                placedDummy = true;
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var figure = GetFigureOnLocation(new BoardPoint(i, j));
                    if (figure is not null)
                    {
                        if (figure.Color == color) continue;
                        if (figure.CheckMoveLegality(location))
                        {
                            if(placedDummy) SetFigureOnLocation(location, null);
                            return true;
                        }
                    }
                }
            }
            if(placedDummy) SetFigureOnLocation(location, null);
            return false;
        }

        private IEnumerable<Pawn> GetPawnsOfColor(FigureColor color)
        {
            var pawns = new List<Pawn>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var figure = GetFigureOnLocation(new BoardPoint(i, j));
                    if (figure is null) continue;
                    if (figure.Color == color)
                    {
                        if (figure is Pawn) pawns.Add(figure as Pawn);
                    }
                }
            }

            return pawns;
        }
        
        public Board()
        {
            _boardState = new Figure[8][];
            for (int i = 0; i < 8; i++)
            {
                _boardState[i] = new Figure[8];
            }
        }

        public Board(Board board)
        {
            _boardState = new Figure[8][];
            for (int i = 0; i < 8; i++)
            {
                _boardState[i] = new Figure[8];
                for (int j = 0; j < 8; j++)
                {
                    if (board.BoardState[i][j] is not null)
                    {
                        var figure = board.BoardState[i][j];
                        CopyFigureToLocation(figure);

                    }
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 7; i >= 0; i--)
            {
                sb.Append(i + 1);
                sb.Append(" ");
                for (int j = 0; j < 8; j++)
                {
                    var figure = GetFigureOnLocation(new BoardPoint(j, i));
                    if (figure is not null)
                    {
                        var name = figure.Color == FigureColor.White ? FigureNames.GetFigureName(figure).ToUpper() : FigureNames.GetFigureName(figure);
                        sb.Append(name + "  ");

                    }
                    else
                    {
                        sb.Append("   ");
                    }
                    
                }

                sb.Append(Environment.NewLine);
            }

            sb.Append("  ");
            for (int j = 0; j < 8; j++)
            {
                sb.Append((char) ('a' + j));
                sb.Append("  ");
            }

            return sb.ToString();
        }
    }
}