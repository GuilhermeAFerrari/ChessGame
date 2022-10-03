using System.Drawing;

namespace Board
{
    public class ChessBoard
    {
        public int Lines { get; set; }
        public int Columns { get; set; }

        private Piece[,] Pieces;

        public ChessBoard(int lines, int columns)
        {
            Lines = lines;
            Columns = columns;
            Pieces = new Piece[Lines, Columns];
        }

        public Piece GetPiece(int line, int column)
        {
            return Pieces[line, column];
        }

        public Piece GetPiece(Position position)
        {
            return Pieces[position.Line, position.Column];
        }

        public void PutPiece(Piece piece, Position position)
        {
            if (ExistsInPosition(position))
                throw new BoardException("There is already a piece in that position");
            
            Pieces[position.Line, position.Column] = piece;
            piece.Position = position;
        }

        public Piece RemovePiece(Position position)
        {
            if (GetPiece(position) == null)
                return null;

            Piece auxPiece = GetPiece(position);
            auxPiece.Position = null;
            Pieces[position.Line, position.Column] = null;
            return auxPiece;
        }

        public bool ExistsInPosition(Position position)
        {
            ValidatePosition(position);
            return GetPiece(position) != null;
        }

        public bool IsPositionValid(Position position)
        {
            if (position.Line < 0 || position.Line >= Lines
                || position.Column < 0 || position.Column >= Columns)
            {
                return false;
            }

            return true;
        }

        public void ValidatePosition(Position position)
        {
            if (!IsPositionValid(position))
                throw new BoardException("Position Invalid");
        }
    }
}
