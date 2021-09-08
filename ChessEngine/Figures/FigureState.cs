using ChessEngine.BoardHandle;

namespace ChessEngine.Figures
{
    class FigureState
    {
        public bool FirstMove { get; set; }
        public bool EnPassaintable { get; set; }
        public BoardPoint Location { get; set; }
        public bool CanCastle { get; set; }
    }
}