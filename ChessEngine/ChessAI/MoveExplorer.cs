using System;
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

        public string GetBestMove(string positionFEN, bool goDeeper)
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

                var indexOfBestResponse = -1;
                if (movesEvalChange2.Length != 0)
                {
                    indexOfBestResponse = handler.ColorToPlay == FigureColor.Black
                        ? Array.IndexOf(movesEvalChange2, movesEvalChange2.Min())
                        : Array.IndexOf(movesEvalChange2, movesEvalChange2.Max());
                }
                handler.SendMove(secondMoves[indexOfBestResponse]);
                var thirdMoves = handler.GetLegalMoves().ToList();
                var baseEval3 = handler.GetCurrentEvaluation();
                var movesEvalChange3 = new float[thirdMoves.Count()];
                float change2 = 0;
                for (int j = 0; j < thirdMoves.Count(); j++)
                {
                    var eval3 = GetEvaluationAfterMove(handler.GetFEN(),thirdMoves[j]);
                    movesEvalChange3[j] = eval3 - baseEval;
                    handler.SendMove(thirdMoves[j]);
                    var fourthMoves = handler.GetLegalMoves().ToList();
                    var movesEvalChange4 = new float[fourthMoves.Count()];
                    for (int k = 0; k < fourthMoves.Count(); k++)
                    {
                        var eval4 = GetEvaluationAfterMove(handler.GetFEN(), fourthMoves[k]);
                        movesEvalChange4[k] = eval4 - baseEval;
                    
                    }

                    
                    if(movesEvalChange4.Length != 0)
                        change2 = handler.ColorToPlay == FigureColor.Black ? movesEvalChange4.Min() : movesEvalChange4.Max();
                }
                
                float change = 0;
                if(movesEvalChange2.Length != 0)
                    change = handler.ColorToPlay == FigureColor.Black ? movesEvalChange2.Min() : movesEvalChange2.Max();
                movesEvalChange[i] += change;
                movesEvalChange[i] += change2;
                handler.UndoMove();
            }

            var bestEval = handler.ColorToPlay == FigureColor.Black ? movesEvalChange.Min() : movesEvalChange.Max();
            var index = Array.IndexOf(movesEvalChange, bestEval);
            return moves[index];


            return bestMove;
        }
    }
}