using Board;

namespace ChessGame.Game
{
    public class Bishop : Piece
    {
        public Bishop(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool[,] PossibleMoves()
        {
            bool[,] matriz = new bool[Board.Lines, Board.Columns];

            Position pos = new(0, 0);

            // northwest
            pos.SetValuesForPosition(Position.Line - 1, Position.Column - 1);
            while (Board.IsPositionValid(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;

                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                    break;

                pos.SetValuesForPosition(pos.Line - 1, pos.Column - 1);
            }

            // north east
            pos.SetValuesForPosition(Position.Line - 1, Position.Column + 1);
            while (Board.IsPositionValid(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;

                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                    break;

                pos.SetValuesForPosition(pos.Line - 1, pos.Column + 1);
            }

            // southeast
            pos.SetValuesForPosition(Position.Line + 1, Position.Column + 1);
            while (Board.IsPositionValid(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;

                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                    break;

                pos.SetValuesForPosition(pos.Line + 1, pos.Column + 1);
            }

            // south-west
            pos.SetValuesForPosition(Position.Line + 1, Position.Column - 1);
            while (Board.IsPositionValid(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;

                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                    break;

                pos.SetValuesForPosition(pos.Line + 1, pos.Column - 1);
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
            return "B";
        }
    }
}
