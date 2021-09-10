using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessEngine.BoardHandle;
using ChessEngine.ChessAI;
using ChessEngine.Figures;

namespace ChessEngine.GameHandle
{
    public class GameHandler
    {
        private List<Board> _boardHistory;
        private Board _board;
        private GameStateAnalyzer _stateAnalyzer;
        private BoardInitializer _boardInitializer;
        private List<string> _moveList;
        public Board Board => _board;
        public FigureColor ColorToPlay { get; set; }

        public GameState GameState => _stateAnalyzer.GetGameState(_board, ColorToPlay, _moveList);

        public GameHandler()
        {
            _board = new Board();
            _boardHistory = new List<Board>();
            _boardInitializer = new BoardInitializer();
            _stateAnalyzer = new GameStateAnalyzer();
            _moveList = new List<string>();
            ColorToPlay = FigureColor.White;
        }

        public void DrawGameboard()
        {
            Console.Clear();
            Console.WriteLine(_board);
        }
        
        public IEnumerable<string> GetLegalMoves()
        {
            var moves = new List<string>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var currentPoint = new BoardPoint(i, j);
                    var figure = _board.GetFigureOnLocation(currentPoint);
                    if (figure is null) continue;
                    if(figure.Color != ColorToPlay) continue;
                    for (int k = 0; k < 8; k++)
                    {
                        for (int l = 0; l < 8; l++)
                        {
                            var currentDestination = new BoardPoint(k, l);
                            var move = currentPoint.ToString() + currentDestination.ToString();

                            if (_board.CheckMoveLegality(currentPoint, currentDestination))
                            {
                                if (figure is Pawn && (currentDestination.Y == 0 || currentDestination.Y == 7))
                                {
                                    moves.Add(move + "q");
                                    moves.Add(move + "n");
                                    moves.Add(move + "b");
                                    moves.Add(move + "r");
                                }
                                else
                                {
                                    moves.Add(move);
                                }
                            }
                        }
                    }
                }
            }

            return moves;
        }
        public void SendMove(string move)
        {
            if(move is null) return;
            var legalMoves = GetLegalMoves();
            if(!legalMoves.Contains(move)) return;
            
            if (move.Length == 4)
            {
                var from = BoardPoint.FromString(move.Substring(0, 2));
                if (from is null) return;
                var to = BoardPoint.FromString(move.Substring(2, 2));
                if (to is null) return;
                var figure = _board.GetFigureOnLocation(from);
                if (figure is Pawn && (to.Y == 0 || to.Y == 7)) return;
                if (_board.CheckMoveLegality(from, to))
                {
                    _boardHistory.Add(new Board(_board)); 
                    _moveList.Add(move);
                    
                    _board.MoveFigureToLocation(from, to, GetMoveType(_board, from, to));
                    ColorToPlay = ColorToPlay == FigureColor.White ? FigureColor.Black : FigureColor.White;
                }
            }

            if (move.Length == 5)
            {
                var from = BoardPoint.FromString(move.Substring(0, 2));
                if (from is null) return;
                var to = BoardPoint.FromString(move.Substring(2, 2));
                if (to is null) return;
                var desiredFigure = move.Substring(4, 1);
                var figure = _board.GetFigureOnLocation(from);
                if(figure is not Pawn) return;
                if(!(to.Y == 0 || to.Y == 7)) return;
                if (_board.CheckMoveLegality(from, to))
                {
                    _boardHistory.Add(new Board(_board)); 
                    _moveList.Add(move);
                    _board.PromotePawn(from, to, desiredFigure);
                    ColorToPlay = ColorToPlay == FigureColor.White ? FigureColor.Black : FigureColor.White;
                }
            }
        }

        public void UndoMove()
        {
            if (_boardHistory.Count != 0)
            {
                ColorToPlay = ColorToPlay == FigureColor.White ? FigureColor.Black : FigureColor.White;
                _board = new Board(_boardHistory.Last());
                _moveList.RemoveAt(_moveList.Count - 1);
                _boardHistory.RemoveAt(_boardHistory.Count - 1);
            }
            
        }

        public void InitializeGame()
        {
            _boardInitializer.InitializeDefault(_board);
            ColorToPlay = FigureColor.White;
        }

        public void InitializeGame(string fen)
        {
            ColorToPlay = _boardInitializer.InitializeFromFEN(_board,fen);
        }

        public void StartGame()
        {
            DrawGameboard();
            while (_stateAnalyzer.GetGameState(_board, ColorToPlay, _moveList) == GameState.Ongoing)
            {
                var move = Console.ReadLine();
                if(move is null) continue;
                if(move == "q") break;
                if (move.Length == 4)
                {
                    var from = BoardPoint.FromString(move.Substring(0, 2));
                    if (from is null) continue;
                    var to = BoardPoint.FromString(move.Substring(2, 2));
                    if (to is null) continue;
                    var figure = _board.GetFigureOnLocation(from);
                    if(figure.Color != ColorToPlay) continue;
                    if (figure is Pawn && (to.Y == 0 || to.Y == 7)) continue;
                    if (_board.CheckMoveLegality(from, to))
                    {
                        _board.MoveFigureToLocation(from, to, GetMoveType(_board, from, to));
                        ColorToPlay = ColorToPlay == FigureColor.White ? FigureColor.Black : FigureColor.White;
                        DrawGameboard();
                    }
                }

                if (move.Length == 5)
                {
                    var from = BoardPoint.FromString(move.Substring(0, 2));
                    if (from is null) continue;
                    var to = BoardPoint.FromString(move.Substring(2, 2));
                    if (to is null) continue;
                    var desiredFigure = move.Substring(4, 1);
                    var figure = _board.GetFigureOnLocation(from);
                    if(figure.Color != ColorToPlay) continue;
                    if(figure is not Pawn) continue;
                    if(!(to.Y == 0 || to.Y == 7)) continue;
                    if (_board.CheckMoveLegality(from, to))
                    {
                        _board.PromotePawn(from, to, desiredFigure);
                        ColorToPlay = ColorToPlay == FigureColor.White ? FigureColor.Black : FigureColor.White;
                        DrawGameboard();
                    }
                }
            }

            var state = _stateAnalyzer.GetGameState(_board, ColorToPlay, _moveList);
            if(state == GameState.Ongoing) Console.WriteLine("Stopped by user");
            else Console.WriteLine(state);

        }

        public string GetFEN()
        {
            var sb = new StringBuilder();
            for (int i = 7; i >= 0; i--)
            {
                var emptyCount = 0;
                for (int j = 0; j < 8; j++)
                {
                    
                    var figure = _board.GetFigureOnLocation(new BoardPoint(j, i));
                    if (figure is not null)
                    {
                        if (emptyCount != 0) sb.Append(emptyCount);
                        emptyCount = 0;
                        var name = FigureNames.GetFigureName(figure);
                        if (figure.Color == FigureColor.White) name = name.ToUpper();
                        sb.Append(name);
                    }
                    else
                    {
                        emptyCount++;
                    }
                }
                if (emptyCount != 0) sb.Append(emptyCount);
                if (i != 0) sb.Append("/");
            }

            sb.Append(" ");
            if (ColorToPlay == FigureColor.White) sb.Append("w");
            else sb.Append("b");
            sb.Append(" ");

            var castleInfo = "";
            var whiteKingLocation = _board.FindKingByColor(FigureColor.White);
            if (whiteKingLocation is not null)
            {
                var whiteKing = _board.GetFigureOnLocation(whiteKingLocation) as King;
                if (whiteKing.CanCastle)
                {
                    var kingRook = _board.GetFigureOnLocation(BoardPoint.FromString("h1")) as Rook;
                    if (kingRook is not null)
                    {
                        if (kingRook.CanCastle) castleInfo += "K";
                    }
                    var queenRook = _board.GetFigureOnLocation(BoardPoint.FromString("a1")) as Rook;
                    if (queenRook is not null)
                    {
                        if (queenRook.CanCastle) castleInfo += "Q";
                    }
                }
                
            }
            var blackKingLocation = _board.FindKingByColor(FigureColor.Black);
            if (blackKingLocation is not null)
            {
                var blackKing = _board.GetFigureOnLocation(blackKingLocation) as King;
                if (blackKing.CanCastle)
                {
                    var kingRook = _board.GetFigureOnLocation(BoardPoint.FromString("h8")) as Rook;
                    if (kingRook is not null)
                    {
                        if (kingRook.CanCastle) castleInfo += "k";
                    }
                    var queenRook = _board.GetFigureOnLocation(BoardPoint.FromString("a8")) as Rook;
                    if (queenRook is not null)
                    {
                        if (queenRook.CanCastle) castleInfo += "q";
                    }
                }
                
            }

            if (castleInfo != "") sb.Append(castleInfo);
            else sb.Append("-");
            sb.Append(" ");

            var enPassaint = "-";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var location = new BoardPoint(i, j);
                    var pawn = _board.GetFigureOnLocation(location);
                    if (pawn is Pawn)
                    {
                        if ((pawn as Pawn).EnPassaintable)
                        {
                            int direction = pawn.Color == FigureColor.White ? 1 : -1;
                            var boardLocation = new BoardPoint(i, j - direction);
                            enPassaint = boardLocation.ToString();
                            break;
                        }
                    }
                }
                if(enPassaint != "-") break;
            }

            sb.Append(enPassaint);

            return sb.ToString();
        }

        public int CalculateNodeCount(string fen, string moves, int depth, bool showMoves)
        {
            if(fen is null) InitializeGame();
            else InitializeGame(fen);

            if (moves is not null)
            {
                var moveStrings = moves.Split(" ");
                var moveList = moveStrings.ToList();
                foreach (var move in moveList)
                {
                    SendMove(move);
                }
            }
            
            if (showMoves)
            {
                var count = 0;
                var firstMoves = GetLegalMoves();
                foreach (var move in firstMoves)
                {
                    SendMove(move);
                    var currentCount = CalculateNodes(depth - 1);
                    UndoMove();
                    Console.WriteLine(move + ": " + currentCount);
                    count += currentCount;
                }

                return count;
            }
            else
            {
                return CalculateNodes(depth);

            }

        }

        private int CalculateNodes(int depth)
        {
            var movesCount = 0;
            if (depth == 0) return 1;
            
            if (depth == 1)
            {
                return GetLegalMoves().Count();
            }
            
            var moves = GetLegalMoves();
            foreach (var move in moves)
            {
                SendMove(move);
                movesCount += CalculateNodes(depth - 1);
                UndoMove();
            }

            return movesCount;
        }

        public string GetNextMove()
        {
            var legalMoves = GetLegalMoves();
            if (legalMoves.Count() != 0)
            {
                var explorer = new MoveExplorer();
                return explorer.GetBestMove(GetFEN());
            }
            else return "";
        }

        public void GetEvaluation()
        {
            var explorer = new MoveExplorer();
            var moves = GetLegalMoves();
            foreach (var move in moves)
            {
                Console.WriteLine(move +": "+explorer.GetEvaluationAfterMove(GetFEN(), move));
            }

            Console.WriteLine();
        }

        public float GetCurrentEvaluation()
        {
            var eval = new PositionEvaluator();
            return eval.EvaluatePosition(GetFEN());
        }

        private SpecialMove GetMoveType(Board board, BoardPoint from, BoardPoint to)
        {
            var figure = board.GetFigureOnLocation(from);
            if (figure is null) return SpecialMove.None;
            if (figure is King && Math.Abs(to.X - from.X) == 2) return SpecialMove.Castle;
            if (figure is Pawn && (from.X != to.X && from.Y != to.Y) && board.GetFigureOnLocation(to) is null)
                return SpecialMove.EnPassaint;
            return SpecialMove.None;
        }

    }
}