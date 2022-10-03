namespace Board
{
    public abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get;  protected set; }
        public int NumberOfMoves { get; protected set; }
        public ChessBoard Board { get; protected set; }

        public Piece(ChessBoard board, Color color)
        {
            Position = null;
            Board = board;
            Color = color;
            NumberOfMoves = 0;
        }

        public void IncrementMoves()
        {
            NumberOfMoves ++;
        }

        public void DecrementMoves()
        {
            NumberOfMoves--;
        }

        public bool ThereIsPossibleMoves()
        {
            bool[,] matriz = PossibleMoves();
            for (int i = 0; i < Board.Lines; i++)
            {
                for (int j = 0; j < Board.Columns; j++)
                {
                    if (matriz[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CanMoveForPosition(Position position)
        {
            return PossibleMoves()[position.Line, position.Column];
        }

        public abstract bool[,] PossibleMoves();
    }
}
