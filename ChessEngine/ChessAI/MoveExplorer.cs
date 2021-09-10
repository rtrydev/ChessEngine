using System;
using System.Linq;
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
            var baseEval = handler.GetCurrentEvaluation();
            var moves = handler.GetLegalMoves().ToList();
            var movesEvalChange = new float[moves.Count()];
            var bestMove = "";
            //var bestEval = handler.ColorToPlay == FigureColor.White ? -1000000f : 1000000f;
            for (int i = 0; i < moves.Count(); i++)
            {
                var eval = GetEvaluationAfterMove(positionFEN, moves[i]);
                movesEvalChange[i] = eval - baseEval;
                handler.SendMove(moves[i]);
                var secondMoves = handler.GetLegalMoves().ToList();
                var movesEvalChange2 = new float[secondMoves.Count()];
                for (int j = 0; j < secondMoves.Count(); j++)
                {
                    var eval2 = GetEvaluationAfterMove(handler.GetFEN(), secondMoves[j]);
                    movesEvalChange2[j] = eval2 - baseEval;
                    
                }

                float change = 0;
                if(movesEvalChange2.Length != 0)
                    change = handler.ColorToPlay == FigureColor.Black ? movesEvalChange2.Min() : movesEvalChange2.Max();
                movesEvalChange[i] += change;
                handler.UndoMove();
            }

            var bestEval = handler.ColorToPlay == FigureColor.Black ? movesEvalChange.Min() : movesEvalChange.Max();
            var index = Array.IndexOf(movesEvalChange, bestEval);
            return moves[index];


            return bestMove;
        }
    }
}