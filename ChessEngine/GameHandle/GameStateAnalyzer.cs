using System.Collections.Generic;
using System.Linq;
using ChessEngine.BoardHandle;
using ChessEngine.Figures;

namespace ChessEngine.GameHandle
{
    class GameStateAnalyzer
    {

        public GameState GetGameState(Board board, FigureColor colorToPlay, List<string> moves)
        {
            if (board.LookForCheckByColor(colorToPlay))
            {
                var hasLegalMoves = HasLegalMoves(board, colorToPlay);
                if (hasLegalMoves) return GameState.Ongoing;
                
                if (colorToPlay == FigureColor.Black) return GameState.BlackKingMated;
                return GameState.WhiteKingMated;
                
            }
            else
            {
                var hasLegalMoves = HasLegalMoves(board, colorToPlay);
                if (!hasLegalMoves) return GameState.Draw;
            }

            if (moves.Count() > 159) return GameState.Draw;

            var figureCount = GetFigureCount(board);
            if (figureCount == 2) return GameState.Draw;
            if (figureCount == 3)
            {
                if (FindKnight(board)) return GameState.Draw;
                if (FindKBishop(board)) return GameState.Draw;
            }
            
            
            if (moves.Count >= 10)
            {
                var count = moves.Count;
                int c = 0;
                if (moves[count - 1] == moves[count - 5] &&
                    moves[count - 9] == moves[count - 5]) c++;
                if (moves[count - 2] == moves[count - 6] &&
                    moves[count - 10] == moves[count - 6]) c++;
                if (moves[count - 4] == moves[count - 8]) c++;
                if (moves[count - 7] == moves[count - 3]) c++;
                if (c == 4) return GameState.Draw;
            }

            return GameState.Ongoing;
        }

        private int GetFigureCount(Board board)
        {
            int count = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board.GetFigureOnLocation(new BoardPoint(i, j)) is not null) count++;
                }
            }

            return count;
        }

        private bool FindKnight(Board board)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board.GetFigureOnLocation(new BoardPoint(i, j)) is Knight) return true;
                }
            }

            return false;
        }
        
        private bool FindKBishop(Board board)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board.GetFigureOnLocation(new BoardPoint(i, j)) is Bishop) return true;
                }
            }

            return false;
        }

        private bool FindBishopInsufficient(Board board)
        {
            var bishops = new List<Figure>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var figure = board.GetFigureOnLocation(new BoardPoint(i, j));
                    if (figure is Bishop) bishops.Add(figure);
                }   
            }

            if (bishops.Count == 2)
            {
                if (bishops[0].Color == bishops[1].Color) return false;
                var location1 = bishops[0].Location;
                var location2 = bishops[1].Location;
                var color1 = location1.X % 2 == location1.Y % 2 ? FigureColor.Black : FigureColor.White;
                var color2 = location2.X % 2 == location2.Y % 2 ? FigureColor.Black : FigureColor.White;
                if (color1 == color2) return true;
            }

            return false;
        }
        
        private bool HasLegalMoves(Board board, FigureColor color)
        {
            int legalMovesCount = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var location = new BoardPoint(i, j);
                    var figure = board.GetFigureOnLocation(location);
                    if (figure is null) continue;
                    if (figure.Color != color) continue;
                    for (int k = 0; k < 8; k++)
                    {
                        for (int l = 0; l < 8; l++)
                        {
                            var locationToMove = new BoardPoint(k, l);
                            if (board.CheckMoveLegality(location, locationToMove)) legalMovesCount++;
                        }
                    }
                }
            }

            return legalMovesCount > 0;

        }
    }
}