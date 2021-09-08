using System.Collections.Generic;
using ChessEngine.BoardHandle;
using ChessEngine.Figures;

namespace ChessEngine.GameHandle
{
    class GameStateAnalyzer
    {
        public GameState GetGameState(Board board, FigureColor colorToPlay)
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
            return GameState.Ongoing;
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