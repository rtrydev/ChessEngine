using ChessEngine.Figures;

namespace ChessEngine.BoardHandle
{
    public class BoardInitializer
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
            board.AddFigureOnLocation("n", BoardPoint.FromString("b1"), FigureColor.White);
            board.AddFigureOnLocation("b", BoardPoint.FromString("c1"), FigureColor.White);
            board.AddFigureOnLocation("q", BoardPoint.FromString("d1"), FigureColor.White);
            board.AddFigureOnLocation("k", BoardPoint.FromString("e1"), FigureColor.White);
            board.AddFigureOnLocation("b", BoardPoint.FromString("f1"), FigureColor.White);
            board.AddFigureOnLocation("n", BoardPoint.FromString("g1"), FigureColor.White);
            board.AddFigureOnLocation("r", BoardPoint.FromString("h1"), FigureColor.White);
            
            board.AddFigureOnLocation("p", BoardPoint.FromString("a7"), FigureColor.Black);
            board.AddFigureOnLocation("p", BoardPoint.FromString("b7"), FigureColor.Black);
            board.AddFigureOnLocation("p", BoardPoint.FromString("c7"), FigureColor.Black);
            board.AddFigureOnLocation("p", BoardPoint.FromString("d7"), FigureColor.Black);
            board.AddFigureOnLocation("p", BoardPoint.FromString("e7"), FigureColor.Black);
            board.AddFigureOnLocation("p", BoardPoint.FromString("f7"), FigureColor.Black);
            board.AddFigureOnLocation("p", BoardPoint.FromString("g7"), FigureColor.Black);
            board.AddFigureOnLocation("p", BoardPoint.FromString("h7"), FigureColor.Black);
            
            board.AddFigureOnLocation("r", BoardPoint.FromString("a8"), FigureColor.Black);
            board.AddFigureOnLocation("n", BoardPoint.FromString("b8"), FigureColor.Black);
            board.AddFigureOnLocation("b", BoardPoint.FromString("c8"), FigureColor.Black);
            board.AddFigureOnLocation("q", BoardPoint.FromString("d8"), FigureColor.Black);
            board.AddFigureOnLocation("k", BoardPoint.FromString("e8"), FigureColor.Black);
            board.AddFigureOnLocation("b", BoardPoint.FromString("f8"), FigureColor.Black);
            board.AddFigureOnLocation("n", BoardPoint.FromString("g8"), FigureColor.Black);
            board.AddFigureOnLocation("r", BoardPoint.FromString("h8"), FigureColor.Black);
        }

        public void InitializeFromState(Board board, Board state)
        {
            
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