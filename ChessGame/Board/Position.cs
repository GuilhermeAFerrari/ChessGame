using System.Data.Common;

namespace Board
{
    public class Position
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public Position(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public void SetValuesForPosition(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return Line + ", " + Column;
        }
    }
}
