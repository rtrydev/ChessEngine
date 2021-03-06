namespace ChessEngine.ChessAI
{
    public class PieceHeatmaps
    {
        public static int[][] Pawn =
        {
            new[] {0, 0, 0, 0, 0, 0, 0, 0},
            new[] {50, 50, 50, 50, 50, 50, 50, 50},
            new[] {10, 10, 20, 30, 30, 20, 10, 10},
            new[] {5, 5, 10, 25, 25, 10, 5, 5},
            new[] {0, 0, 0, 20, 20, 0, 0, 0},
            new[] {5, -5, -10, 0, 0, -10, -5, 5},
            new[] {5, 10, 10, -20, -20, 10, 10, 5},
            new[] {0, 0, 0, 0, 0, 0, 0, 0}
        };

        public static int[][] Knight =
        {
            new[]{-50, -40, -30, -30, -30, -30, -40, -50},
            new[]{-40, -20, 0, 0, 0, 0, -20, -40},
            new[]{-30, 0, 10, 15, 15, 10, 0, -30},
            new[]{-30, 5, 15, 20, 20, 15, 5, -30},
            new[]{-30, 0, 15, 20, 20, 15, 0, -30},
            new[]{-30, 5, 10, 15, 15, 10, 5, -30},
            new[]{-40, -20, 0, 5, 5, 0, -20, -40},
            new[]{-50, -40, -30, -30, -30, -30, -40, -50},
        };

        public static int[][] Bishop =
        {
            new[] {-20, -10, -10, -10, -10, -10, -10, -20},
            new[] {-10, 0, 0, 0, 0, 0, 0, -10},
            new[] {-10, 0, 5, 10, 10, 5, 0, -10},
            new[] {-10, 5, 5, 10, 10, 5, 5, -10},
            new[] {-10, 0, 10, 10, 10, 10, 0, -10},
            new[] {-10, 10, 10, 10, 10, 10, 10, -10},
            new[] {-10, 5, 0, 0, 0, 0, 5, -10},
            new[] {-20, -10, -10, -10, -10, -10, -10, -20},
        };

        public static int[][] Rook =
        {
            new[] {0, 0, 0, 0, 0, 0, 0, 0},
            new[] {5, 10, 10, 10, 10, 10, 10, 5},
            new[] {-5, 0, 0, 0, 0, 0, 0, -5},
            new[] {-5, 0, 0, 0, 0, 0, 0, -5},
            new[] {-5, 0, 0, 0, 0, 0, 0, -5},
            new[] {-5, 0, 0, 0, 0, 0, 0, -5},
            new[] {-5, 0, 0, 0, 0, 0, 0, -5},
            new[] {0, 0, 0, 5, 5, 0, 0, 0}
        };

        public static int[][] Queen =
        {
            new[] {-20, -10, -10, -5, -5, -10, -10, -20},
            new[] {-10, 0, 0, 0, 0, 0, 0, -10},
            new[] {-10, 0, 5, 5, 5, 5, 0, -10},
            new[] {-5, 0, 5, 5, 5, 5, 0, -5},
            new[] {0, 0, 5, 5, 5, 5, 0, -5},
            new[] {-10, 5, 5, 5, 5, 5, 0, -10},
            new[] {-10, 0, 5, 0, 0, 0, 0, -10},
            new[] {-20, -10, -10, -5, -5, -10, -10, -20}
        };

        public static int[][] WhiteKing =
        {
            new[] {-30, -40, -40, -50, -50, -40, -40, -30},
            new[] {-30, -40, -40, -50, -50, -40, -40, -30},
            new[] {-30, -40, -40, -50, -50, -40, -40, -30},
            new[] {-30, -40, -40, -50, -50, -40, -40, -30},
            new[] {-10, -20, -20, -20, -20, -20, -20, -10},
            new[] {-10, -20, -20, -20, -20, -20, -20, -10},
            new[] {20, 20, 0, 0, 0, 0, 20, 20},
            new[] {20, 30, 10, 0, 0, 10, 30, 20}
        };

        public static int[][] BlackKing =
        {
            new[] {20, 30, 10, 0, 0, 10, 30, 20},
            new[] {20, 20, 0, 0, 0, 0, 20, 20},
            new[] {-10, -20, -20, -20, -20, -20, -20, -10},
            new[] {-10, -20, -20, -20, -20, -20, -20, -10},
            new[] {-30, -40, -40, -50, -50, -40, -40, -30},
            new[] {-30, -40, -40, -50, -50, -40, -40, -30},
            new[] {-30, -40, -40, -50, -50, -40, -40, -30},
            new[] {-30, -40, -40, -50, -50, -40, -40, -30},
        };

        public static int[][] KingEnd =
        {
            new[] {-50, -40, -30, -20, -20, -30, -40, -50},
            new[] {-30, -20, -10, 0, 0, -10, -20, -30},
            new[] {-30, -10, 20, 30, 30, 20, -10, -30},
            new[] {-30, -10, 30, 40, 40, 30, -10, -30},
            new[] {-30, -10, 30, 40, 40, 30, -10, -30},
            new[] {-30, -10, 20, 30, 30, 20, -10, -30},
            new[] {-30, -30, 0, 0, 0, 0, -30, -30},
            new[] {-50, -30, -30, -30, -30, -30, -30, -50}
        };
    }
}