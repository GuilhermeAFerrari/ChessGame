using Board;

namespace Game
{
    public class King : Piece
    {
        private ChessMatch match;

        public King(ChessBoard board, Color color, ChessMatch match) : base(board, color)
        {
            this.match = match;
        }

        private bool TesteRookForRoque(Position position)
        {
            Piece piece = Board.GetPiece(position);
            return piece != null && piece is Rook && piece.Color == Color && NumberOfMoves == 0;
        }

        public override bool[,] PossibleMoves()
        {
            bool[,] matriz = new bool[Board.Lines, Board.Columns];

            Position pos = new(0, 0);

            // above
            pos.SetValuesForPosition(Position.Line - 1, Position.Column);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            // north east
            pos.SetValuesForPosition(Position.Line - 1, Position.Column + 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            // right
            pos.SetValuesForPosition(Position.Line, Position.Column + 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            // southeast
            pos.SetValuesForPosition(Position.Line + 1, Position.Column + 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            // below
            pos.SetValuesForPosition(Position.Line + 1, Position.Column);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            // south-west
            pos.SetValuesForPosition(Position.Line + 1, Position.Column - 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            // left
            pos.SetValuesForPosition(Position.Line, Position.Column - 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            // northwest
            pos.SetValuesForPosition(Position.Line - 1, Position.Column - 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            // #especial move roque
            if (NumberOfMoves == 0 && !match.Xeque)
            {
                // short roque
                Position positionRook = new(Position.Line, Position.Column + 3);

                if (TesteRookForRoque(positionRook))
                {
                    Position positionKing1 = new(Position.Line, Position.Column + 1);
                    Position positionKing2 = new(Position.Line, Position.Column + 2);

                    if (Board.GetPiece(positionKing1) is null &&
                        Board.GetPiece(positionKing2) is null)
                    {
                        matriz[Position.Line, Position.Column + 2] = true;
                    }
                }

                // long roque
                Position positionRook2 = new(Position.Line, Position.Column - 4);

                if (TesteRookForRoque(positionRook2))
                {
                    Position positionKing1 = new(Position.Line, Position.Column - 1);
                    Position positionKing2 = new(Position.Line, Position.Column - 2);
                    Position positionKing3 = new(Position.Line, Position.Column - 3);

                    if (Board.GetPiece(positionKing1) is null &&
                        Board.GetPiece(positionKing2) is null &&
                        Board.GetPiece(positionKing3) is null)
                    {
                        matriz[Position.Line, Position.Column - 2] = true;
                    }
                }
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
            return "K";
        }
    }
}
