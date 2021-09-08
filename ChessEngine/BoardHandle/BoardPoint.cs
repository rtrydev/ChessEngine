using System;
using System.Linq;

namespace ChessEngine.BoardHandle
{
    class BoardPoint
    {
        public int X;
        public int Y;
        private static readonly String[] legalX = {"a", "b", "c", "d", "e", "f", "g", "h"};
        private static readonly String[] legalY = {"1", "2", "3", "4", "5", "6", "7", "8"};

        public BoardPoint(int x, int y)
        {
            if (x > 7 || x < 0) throw new ArgumentException();
            if (y > 7 || y < 0) throw new ArgumentException();
            X = x;
            Y = y;
        }

        public static BoardPoint FromString(String str)
        {
            if (str.Length != 2) return null;
            if (!legalX.Contains(str[0].ToString())) return null;
            if (!legalY.Contains(str[1].ToString())) return null;
            var x = Array.IndexOf(legalX, str[0].ToString());
            var y = Array.IndexOf(legalY, str[1].ToString());
            return new BoardPoint(x, y);
        }

        public override string ToString()
        {
            return $"{legalX[X]}{legalY[Y]}";
        }

        public static bool operator ==(BoardPoint a, BoardPoint b)
        {
            if (a.X != b.X) return false;
            if (a.Y != b.Y) return false;
            return true;
        }

        public static bool operator !=(BoardPoint a, BoardPoint b)
        {
            return !(a == b);
        }
    }
}