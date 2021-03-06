using System;
using System.Collections.Generic;
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
            if (handler.GameState == GameState.Draw)
                return 0;

            var figures = new List<Figure>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var point = new BoardPoint(i, j);
                    var figure = handler.Board.GetFigureOnLocation(point);
                    
                    if (figure is not null)
                    {
                        figures.Add(figure);
                    }
                }
            }

            var count = figures.Count;

            foreach (var figure in figures)
            {
                if (figure.Color == FigureColor.White)
                {
                    whitePoints += GetPieceValue(figure, figure.Location, count);
                }
                else
                {
                    blackPoints += GetPieceValue(figure, figure.Location, count);
                }
            }

            var legalMovesCount = handler.CalculateNodeCount(position, null, 1, false);
            float multiplier = handler.ColorToPlay == FigureColor.Black ? -0.02f : 0.02f;
            return whitePoints - blackPoints + legalMovesCount * multiplier;
        }

        private float GetPieceValue(Figure figure, BoardPoint location, int figureCount)
        {
            var multiplier = 0.01f;
            if (figure is King)
            {
                if(figureCount > 12)
                    return PieceValues.King +
                       multiplier * PieceHeatmaps.WhiteKing[ColorPosition(figure.Color, location.Y)][location.X];
                else
                {
                    return PieceValues.King +
                        multiplier * PieceHeatmaps.KingEnd[ColorPosition(figure.Color, location.Y)][location.X];
                }
                
            }
            if (figure is Queen) return PieceValues.Queen + multiplier * PieceHeatmaps.Queen[ColorPosition(figure.Color, location.Y)][location.X] ;
            if (figure is Rook) return PieceValues.Rook + multiplier * PieceHeatmaps.Rook[ColorPosition(figure.Color, location.Y)][location.X];
            if (figure is Bishop)
                return PieceValues.Bishop + multiplier * PieceHeatmaps.Bishop[ColorPosition(figure.Color, location.Y)][location.X];
            if (figure is Knight)
                return PieceValues.Knight + (multiplier) * PieceHeatmaps.Knight[ColorPosition(figure.Color, location.Y)][location.X];
            if (figure is Pawn)
            {
                return PieceValues.Pawn + multiplier * PieceHeatmaps.Pawn[ColorPosition(figure.Color, location.Y)][location.X];
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