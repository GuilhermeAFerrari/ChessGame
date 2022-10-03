using Board;
using Game;

namespace ChessGame.Game
{
    public class Pawn : Piece
    {
        private ChessMatch Match;

        public Pawn(ChessBoard board, Color color, ChessMatch match) : base(board, color)
        {
            Match = match;
        }

        private bool ThereIsEnemy(Position position)
        {
            Piece piece = Board.GetPiece(position);
            return piece != null && piece.Color != Color;
        }

        private bool Free(Position position)
        {
            return Board.GetPiece(position) == null;
        }

        public override bool[,] PossibleMoves()
        {
            bool[,] matriz = new bool[Board.Lines, Board.Columns];

            Position pos = new(0, 0);

            if (Color == Color.White)
            {
                pos.SetValuesForPosition(Position.Line - 1, Position.Column);
                if (Board.IsPositionValid(pos) && Free(pos))
                {
                    matriz[pos.Line, pos.Column] = true;
                }

                pos.SetValuesForPosition(Position.Line - 2, Position.Column);
                Position position2 = new(Position.Line - 1, Position.Column);
                if (Board.IsPositionValid(position2) && Free(position2) &&
                    Board.IsPositionValid(pos) && Free(pos) &&
                    NumberOfMoves == 0)
                {
                    matriz[pos.Line, pos.Column] = true;
                }

                pos.SetValuesForPosition(Position.Line - 1, Position.Column - 1);
                if (Board.IsPositionValid(pos) && ThereIsEnemy(pos))
                {
                    matriz[pos.Line, pos.Column] = true;
                }

                pos.SetValuesForPosition(Position.Line - 1, Position.Column + 1);
                if (Board.IsPositionValid(pos) && ThereIsEnemy(pos))
                {
                    matriz[pos.Line, pos.Column] = true;
                }

                // #especial move en passant
                if (Position.Line == 3)
                {
                    Position leftPosition = new(Position.Line, Position.Column - 1);

                    if (Board.IsPositionValid(leftPosition) && ThereIsEnemy(leftPosition) &&
                        Board.GetPiece(leftPosition) == Match.PossibleEnPassant)
                    {
                        matriz[leftPosition.Line -1, leftPosition.Column] = true;
                    }

                    Position rightPosition = new(Position.Line, Position.Column + 1);

                    if (Board.IsPositionValid(rightPosition) && ThereIsEnemy(rightPosition) &&
                        Board.GetPiece(rightPosition) == Match.PossibleEnPassant)
                    {
                        matriz[rightPosition.Line -1, rightPosition.Column] = true;
                    }
                }
            }
            else
            {
                pos.SetValuesForPosition(Position.Line + 1, Position.Column);
                if (Board.IsPositionValid(pos) && Free(pos))
                {
                    matriz[pos.Line, pos.Column] = true;
                }

                pos.SetValuesForPosition(Position.Line + 2, Position.Column);
                Position position2 = new Position(Position.Line + 1, Position.Column);
                if (Board.IsPositionValid(position2) && Free(position2) &&
                    Board.IsPositionValid(pos) && Free(pos) &&
                    NumberOfMoves == 0)
                {
                    matriz[pos.Line, pos.Column] = true;
                }

                pos.SetValuesForPosition(Position.Line + 1, Position.Column - 1);
                if (Board.IsPositionValid(pos) && ThereIsEnemy(pos))
                {
                    matriz[pos.Line, pos.Column] = true;
                }

                pos.SetValuesForPosition(Position.Line + 1, Position.Column + 1);
                if (Board.IsPositionValid(pos) && ThereIsEnemy(pos))
                {
                    matriz[pos.Line, pos.Column] = true;
                }

                // #especial move en passant
                if (Position.Line == 4)
                {
                    Position leftPosition = new(Position.Line, Position.Column - 1);

                    if (Board.IsPositionValid(leftPosition) && ThereIsEnemy(leftPosition) &&
                        Board.GetPiece(leftPosition) == Match.PossibleEnPassant)
                    {
                        matriz[leftPosition.Line + 1, leftPosition.Column] = true;
                    }

                    Position rightPosition = new(Position.Line, Position.Column + 1);

                    if (Board.IsPositionValid(rightPosition) && ThereIsEnemy(rightPosition) &&
                        Board.GetPiece(rightPosition) == Match.PossibleEnPassant)
                    {
                        matriz[rightPosition.Line + 1, rightPosition.Column] = true;
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
            return "P";
        }
    }
}
