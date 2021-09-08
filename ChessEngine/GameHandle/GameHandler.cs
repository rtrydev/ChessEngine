using System;
using System.Collections.Generic;
using ChessEngine.BoardHandle;
using ChessEngine.Figures;

namespace ChessEngine.GameHandle
{
    public class GameHandler
    {
        private Board _board;
        private GameStateAnalyzer _stateAnalyzer;
        private BoardInitializer _boardInitializer;
        public FigureColor ColorToPlay { get; set; }

        public GameState GameState => _stateAnalyzer.GetGameState(_board, ColorToPlay);

        public GameHandler()
        {
            _board = new Board();
            _boardInitializer = new BoardInitializer();
            _stateAnalyzer = new GameStateAnalyzer();
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
                    _board.PromotePawn(from, to, desiredFigure);
                    ColorToPlay = ColorToPlay == FigureColor.White ? FigureColor.Black : FigureColor.White;
                }
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
            while (_stateAnalyzer.GetGameState(_board, ColorToPlay) == GameState.Ongoing)
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

            var state = _stateAnalyzer.GetGameState(_board, ColorToPlay);
            if(state == GameState.Ongoing) Console.WriteLine("Stopped by user");
            else Console.WriteLine(state);

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