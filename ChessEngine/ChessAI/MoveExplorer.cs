using System;
using ChessEngine.Figures;
using ChessEngine.GameHandle;

namespace ChessEngine.ChessAI
{
    class MoveExplorer
    {
        public float GetEvaluationAfterMove(string positionFEN, string move)
        {
            var handler = new GameHandler();
            handler.InitializeGame(positionFEN);
            handler.SendMove(move);
            return handler.GetCurrentEvaluation();
        }

        public string GetBestMove(string positionFEN)
        {
            var handler = new GameHandler();
            handler.InitializeGame(positionFEN);
            var moves = handler.GetLegalMoves();
            var bestMove = "";
            var bestEval = handler.ColorToPlay == FigureColor.White ? -1000000f : 1000000f;
            foreach (var move in moves)
            {
                var eval = GetEvaluationAfterMove(positionFEN, move);
                if (handler.ColorToPlay == FigureColor.Black)
                {
                    if (eval < bestEval)
                    {
                        bestEval = eval;
                        bestMove = move;
                    }
                }
                else
                {
                    if (eval > bestEval)
                    {
                        bestEval = eval;
                        bestMove = move;
                    }
                }
            }

            return bestMove;
        }
    }
}