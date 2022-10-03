using Board;

namespace Game
{
    public class Rook : Piece
    {
        public Rook(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool[,] PossibleMoves()
        {
            bool[,] matriz = new bool[Board.Lines, Board.Columns];

            Position pos = new(0, 0);

            // above
            pos.SetValuesForPosition(Position.Line - 1, Position.Column);
            while (Board.IsPositionValid(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;

                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                    break;

                pos.Line--;
            }

            // below
            pos.SetValuesForPosition(Position.Line + 1, Position.Column);
            while (Board.IsPositionValid(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;

                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                    break;

                pos.Line++;
            }

            // right
            pos.SetValuesForPosition(Position.Line, Position.Column + 1);
            while (Board.IsPositionValid(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;

                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                    break;

                pos.Column++;
            }

            // left
            pos.SetValuesForPosition(Position.Line, Position.Column - 1);
            while (Board.IsPositionValid(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;

                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                    break;

                pos.Column--;
            }

            return matriz;
        }

        private bool CanMove(Position position)
        {
            Piece piece = Board.GetPiece(position);
            return piece == null || piece.Color != Color;
        }
    
        public override string ToString()
        {
            return "R";
        }
    }
}
