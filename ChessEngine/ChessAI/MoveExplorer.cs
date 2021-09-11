using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        public string GetBestMove(string positionFEN, int depth)
        {
            var handler = new GameHandler();
            handler.InitializeGame(positionFEN);
            var bestEval = handler.ColorToPlay == FigureColor.White ? -1000000f : 1000000f;
            var baseEval = handler.GetCurrentEvaluation();
            var legalMoves = handler.GetLegalMoves().ToList();
            var moveRatings = new float[legalMoves.Count()];

            for (int i = 0; i < legalMoves.Count(); i++)
            {
                moveRatings[i] = GetMinMaxEval(handler, legalMoves[i]);
            }

            var bestValue = handler.ColorToPlay == FigureColor.Black ? moveRatings.Min() : moveRatings.Max();
            var bestMoveIndex = Array.IndexOf(moveRatings, bestValue);
            return legalMoves[bestMoveIndex];
        }

        private float GetMinMaxEval(GameHandler handler, string move)
        {
            handler.SendMove(move);
            var possibleResponses = handler.GetLegalMoves().ToList();
            if (!possibleResponses.Any())
            {
                if (handler.GameState == GameState.Draw)
                {
                    handler.UndoMove();
                    return 0;
                }
                if (handler.GameState == GameState.BlackKingMated)
                {
                    handler.UndoMove();
                    return 100000f;
                }
                if (handler.GameState == GameState.WhiteKingMated)
                {
                    handler.UndoMove();
                    return -100000f;
                }
            }

            float eval = GetMax(handler, possibleResponses).eval;
            handler.UndoMove();

            return eval;
            
        }

        private (string move, float eval) GetMax(GameHandler handler, List<string> moves)
        {
            var evals = new float[moves.Count()];
            for (int i = 0; i < moves.Count(); i++)
            {
                handler.SendMove(moves[i]);
                evals[i] = handler.GetCurrentEvaluation();
                handler.UndoMove();
            }

            var bestEval = handler.ColorToPlay == FigureColor.White ? evals.Max() : evals.Min();
            var bestEvalIndex = Array.IndexOf(evals, bestEval);

            return (moves[bestEvalIndex], bestEval);
        }
        
    }
}