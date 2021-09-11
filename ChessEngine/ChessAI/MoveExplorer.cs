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
            
            var legalMoves = handler.GetLegalMoves().ToList();
            var moveRatings = new float[legalMoves.Count()];

            for (int i = 0; i < legalMoves.Count(); i++)
            {
                moveRatings[i] = GetMinMaxEval(handler, legalMoves[i], depth);
            }

            var bestValue = handler.ColorToPlay == FigureColor.Black ? moveRatings.Min() : moveRatings.Max();
            var bestMoveIndex = Array.IndexOf(moveRatings, bestValue);
            return legalMoves[bestMoveIndex];
        }

        private float GetMinMaxEval(GameHandler handler, string move, int depth)
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
            
            if (depth == 1)
            {
                float eval = GetMax(handler, possibleResponses).eval;
                handler.UndoMove();

                return eval;
            }

            possibleResponses = handler.ColorToPlay == FigureColor.White
                ? possibleResponses.OrderByDescending(x => GetMinMaxEval(handler, x, depth - 1)).ToList()
                : possibleResponses.OrderBy(x => GetMinMaxEval(handler, x, depth - 1)).ToList();

            var toTake = possibleResponses.Count() > 5 ? possibleResponses.Count() / 4 : possibleResponses.Count();
            var responsesAfterElimination = possibleResponses.Take(toTake).ToList();
            var evals = new float[responsesAfterElimination.Count()];
            for (int i = 0; i < responsesAfterElimination.Count(); i++)
            {
                handler.SendMove(responsesAfterElimination[i]);
                evals[i] = GetMinMaxEval(handler, responsesAfterElimination[i], depth - 1);
                handler.UndoMove();
            }

            return handler.ColorToPlay == FigureColor.White ? evals.Min() : evals.Max();


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