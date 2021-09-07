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
        private FigureColor ColorToPlay;

        public GameHandler()
        {
            _board = new Board();
            _boardInitializer = new BoardInitializer();
            _stateAnalyzer = new GameStateAnalyzer();
            ColorToPlay = FigureColor.White;
        }

        public void StartGame()
        {
            _boardInitializer.InitializeDefault(_board);
            Console.Clear();
            Console.WriteLine(_board);

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
                        Console.Clear();
                        Console.WriteLine(_board);
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
                        Console.Clear();
                        Console.WriteLine(_board);
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