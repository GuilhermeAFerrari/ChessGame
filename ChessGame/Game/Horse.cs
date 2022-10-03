using Board;

namespace ChessGame.Game
{
    public class Horse : Piece
    {
        public Horse(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool[,] PossibleMoves()
        {
            bool[,] matriz = new bool[Board.Lines, Board.Columns];

            Position pos = new(0, 0);

            pos.SetValuesForPosition(Position.Line - 1, Position.Column -2);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            pos.SetValuesForPosition(Position.Line - 2, Position.Column - 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            pos.SetValuesForPosition(Position.Line - 2, Position.Column + 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            pos.SetValuesForPosition(Position.Line - 1, Position.Column + 2);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            pos.SetValuesForPosition(Position.Line + 1, Position.Column + 2);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            pos.SetValuesForPosition(Position.Line + 2, Position.Column + 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            pos.SetValuesForPosition(Position.Line + 2, Position.Column - 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            pos.SetValuesForPosition(Position.Line + 1, Position.Column - 2);
            if (Board.IsPositionValid(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            return matriz;
        }

        private bool CanMove(Position position)
        {
            Piece piece = Board.GetPiece(position);
            return piece == null || piece.Color != Color;
        }

        public override string ToString()
        {
            return "H";
        }
    }
}
