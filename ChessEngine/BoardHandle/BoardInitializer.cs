using ChessEngine.Figures;

namespace ChessEngine.BoardHandle
{
    class BoardInitializer
    {
        public void InitializeDefault(Board board)
        {
            board.AddFigureOnLocation("p", BoardPoint.FromString("a2"), FigureColor.White);
            board.AddFigureOnLocation("p", BoardPoint.FromString("b2"), FigureColor.White);
            board.AddFigureOnLocation("p", BoardPoint.FromString("c2"), FigureColor.White);
            board.AddFigureOnLocation("p", BoardPoint.FromString("d2"), FigureColor.White);
            board.AddFigureOnLocation("p", BoardPoint.FromString("e2"), FigureColor.White);
            board.AddFigureOnLocation("p", BoardPoint.FromString("f2"), FigureColor.White);
            board.AddFigureOnLocation("p", BoardPoint.FromString("g2"), FigureColor.White);
            board.AddFigureOnLocation("p", BoardPoint.FromString("h2"), FigureColor.White);
            
            board.AddFigureOnLocation("r", BoardPoint.FromString("a1"), FigureColor.White);
            (board.GetFigureOnLocation(BoardPoint.FromString("a1")) as Rook).CanCastle = true;
            board.AddFigureOnLocation("n", BoardPoint.FromString("b1"), FigureColor.White);
            board.AddFigureOnLocation("b", BoardPoint.FromString("c1"), FigureColor.White);
            board.AddFigureOnLocation("q", BoardPoint.FromString("d1"), FigureColor.White);
            board.AddFigureOnLocation("k", BoardPoint.FromString("e1"), FigureColor.White);
            board.AddFigureOnLocation("b", BoardPoint.FromString("f1"), FigureColor.White);
            board.AddFigureOnLocation("n", BoardPoint.FromString("g1"), FigureColor.White);
            board.AddFigureOnLocation("r", BoardPoint.FromString("h1"), FigureColor.White);
            (board.GetFigureOnLocation(BoardPoint.FromString("h1")) as Rook).CanCastle = true;
            
            board.AddFigureOnLocation("p", BoardPoint.FromString("a7"), FigureColor.Black);
            board.AddFigureOnLocation("p", BoardPoint.FromString("b7"), FigureColor.Black);
            board.AddFigureOnLocation("p", BoardPoint.FromString("c7"), FigureColor.Black);
            board.AddFigureOnLocation("p", BoardPoint.FromString("d7"), FigureColor.Black);
            board.AddFigureOnLocation("p", BoardPoint.FromString("e7"), FigureColor.Black);
            board.AddFigureOnLocation("p", BoardPoint.FromString("f7"), FigureColor.Black);
            board.AddFigureOnLocation("p", BoardPoint.FromString("g7"), FigureColor.Black);
            board.AddFigureOnLocation("p", BoardPoint.FromString("h7"), FigureColor.Black);
            
            board.AddFigureOnLocation("r", BoardPoint.FromString("a8"), FigureColor.Black);
            (board.GetFigureOnLocation(BoardPoint.FromString("a8")) as Rook).CanCastle = true;
            board.AddFigureOnLocation("n", BoardPoint.FromString("b8"), FigureColor.Black);
            board.AddFigureOnLocation("b", BoardPoint.FromString("c8"), FigureColor.Black);
            board.AddFigureOnLocation("q", BoardPoint.FromString("d8"), FigureColor.Black);
            board.AddFigureOnLocation("k", BoardPoint.FromString("e8"), FigureColor.Black);
            board.AddFigureOnLocation("b", BoardPoint.FromString("f8"), FigureColor.Black);
            board.AddFigureOnLocation("n", BoardPoint.FromString("g8"), FigureColor.Black);
            board.AddFigureOnLocation("r", BoardPoint.FromString("h8"), FigureColor.Black);
            (board.GetFigureOnLocation(BoardPoint.FromString("h8")) as Rook).CanCastle = true;
        }

        public void InitializeFromState(Board board, Board state)
        {
            
        }

        public FigureColor InitializeFromFEN(Board board, string fen)
        {
            var segments = fen.Split(" ");
            var ranks = segments[0].Split("/");
            for (int i = 0; i < 8; i++)
            {
                int iterator = 0;
                for (int j = 0; j < ranks[i].Length; j++)
                {
                    if (ranks[i][j] > '0' && ranks[i][j] < '9')
                    {
                        iterator += ranks[i][j] - '0';
                    }
                    else
                    {
                        switch (ranks[i][j])
                        {
                            case 'p':
                            {
                                var location = new BoardPoint(iterator++, 7 - i);
                                board.AddFigureOnLocation("p",location, FigureColor.Black);
                                if (7 - i != 6) (board.GetFigureOnLocation(location) as Pawn).FirstMove = false;
                                break;
                            }
                            case 'k':
                            {
                                board.AddFigureOnLocation("k",new BoardPoint(iterator++, 7 - i), FigureColor.Black);
                                break;
                            }
                            case 'q':
                            {
                                board.AddFigureOnLocation("q",new BoardPoint(iterator++, 7 - i), FigureColor.Black);
                                break;
                            }
                            case 'n':
                            {
                                board.AddFigureOnLocation("n",new BoardPoint(iterator++, 7 - i), FigureColor.Black);
                                break;
                            }
                            case 'b':
                            {
                                board.AddFigureOnLocation("b",new BoardPoint(iterator++, 7 - i), FigureColor.Black);
                                break;
                            }
                            case 'r':
                            {
                                board.AddFigureOnLocation("r",new BoardPoint(iterator++, 7 - i), FigureColor.Black);
                                break;
                            }
                            case 'P':
                            {
                                var location = new BoardPoint(iterator++, 7 - i);
                                board.AddFigureOnLocation("p",location, FigureColor.White);
                                if (7 - i != 1) (board.GetFigureOnLocation(location) as Pawn).FirstMove = false;
                                break;
                            }
                            case 'K':
                            {
                                board.AddFigureOnLocation("k",new BoardPoint(iterator++, 7 - i), FigureColor.White);
                                break;
                            }
                            case 'Q':
                            {
                                board.AddFigureOnLocation("q",new BoardPoint(iterator++, 7 - i), FigureColor.White);
                                break;
                            }
                            case 'N':
                            {
                                board.AddFigureOnLocation("n",new BoardPoint(iterator++, 7 - i), FigureColor.White);
                                break;
                            }
                            case 'B':
                            {
                                board.AddFigureOnLocation("b",new BoardPoint(iterator++, 7 - i), FigureColor.White);
                                break;
                            }
                            case 'R':
                            {
                                board.AddFigureOnLocation("r",new BoardPoint(iterator++, 7 - i), FigureColor.White);
                                break;
                            }
                        }
                    }
                }
            }

            if (segments[3] != "-")
            {
                var location = BoardPoint.FromString(segments[3]);
                location.Y = location.Y == 2 ? location.Y + 1 : location.Y - 1;
                var pawn = board.GetFigureOnLocation(location) as Pawn;
                if(pawn is not null)
                    pawn.EnPassaintable = true;
            }

            var castles = segments[2];
            if (castles.Contains('K'))
                (board.GetFigureOnLocation(BoardPoint.FromString("h1")) as Rook).CanCastle = true;
            if (castles.Contains('Q'))
                (board.GetFigureOnLocation(BoardPoint.FromString("a1")) as Rook).CanCastle = true;
            if (!castles.Contains('K') && !castles.Contains('Q'))
            {
                (board.GetFigureOnLocation(board.FindKingByColor(FigureColor.White)) as King).CanCastle = false;
            }
            if (castles.Contains('k'))
                (board.GetFigureOnLocation(BoardPoint.FromString("h8")) as Rook).CanCastle = true;
            if (castles.Contains('q'))
                (board.GetFigureOnLocation(BoardPoint.FromString("a8")) as Rook).CanCastle = true;
            if (!castles.Contains('k') && !castles.Contains('q'))
            {
                (board.GetFigureOnLocation(board.FindKingByColor(FigureColor.Black)) as King).CanCastle = false;
            }
                
            return segments[1] == "w" ? FigureColor.White : FigureColor.Black;
        }

        public void InitializeCustom(Board board)
        {
            board.AddFigureOnLocation("k", BoardPoint.FromString("e1"), FigureColor.White);
            board.AddFigureOnLocation("r", BoardPoint.FromString("a2"), FigureColor.Black);
            board.AddFigureOnLocation("q", BoardPoint.FromString("a1"), FigureColor.Black);
            board.AddFigureOnLocation("b", BoardPoint.FromString("c3"), FigureColor.White);
        }
    }
}