using System;
using System.Linq;
using System.Runtime.CompilerServices;
using ChessEngine.BoardHandle;
using ChessEngine.Figures;
using ChessEngine.GameHandle;

namespace ChessEngine.ChessAI
{
    class PositionEvaluator
    {
        public float EvaluatePosition(string position)
        {
            var handler = new GameHandler();
            handler.InitializeGame(position);
            var whitePoints = 0f;
            var blackPoints = 0f;

            if (handler.GameState == GameState.BlackKingMated)
                whitePoints += 10000;
            if (handler.GameState == GameState.WhiteKingMated)
                blackPoints += 10000;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var point = new BoardPoint(i, j);
                    var figure = handler.Board.GetFigureOnLocation(point);
                    if (figure is not null)
                    {
                        if (figure.Color == FigureColor.White)
                        {
                            whitePoints += GetPieceValue(figure, point);
                        }
                        else
                        {
                            blackPoints += GetPieceValue(figure, point);
                        }
                    }
                }
            }

            return whitePoints - blackPoints;
        }

        private float GetPieceValue(Figure figure, BoardPoint location)
        {
            var multiplier = 0.05f;
            if (figure is King)
            {
                return PieceValues.King +
                       multiplier * PieceHeatmaps.WhiteKing[ColorPosition(figure.Color, location.Y)][location.X];
                
            }
            if (figure is Queen) return PieceValues.Queen + multiplier * PieceHeatmaps.Queen[ColorPosition(figure.Color, location.Y)][location.X] ;
            if (figure is Rook) return PieceValues.Rook + multiplier * PieceHeatmaps.Rook[ColorPosition(figure.Color, location.Y)][location.X];
            if (figure is Bishop)
                return PieceValues.Bishop + multiplier * PieceHeatmaps.Bishop[ColorPosition(figure.Color, location.Y)][location.X];
            if (figure is Knight)
                return PieceValues.Knight + multiplier * PieceHeatmaps.Knight[ColorPosition(figure.Color, location.Y)][location.X];
            if (figure is Pawn)
            {
                return PieceValues.Pawn + multiplier * 2 * PieceHeatmaps.Pawn[ColorPosition(figure.Color, location.Y)][location.X];
            }

            return 0;
        }

        private int ColorPosition(FigureColor color, int y)
        {
            if (color == FigureColor.Black) return y;
            return 7 - y;
        }
    }
}