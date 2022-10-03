using Board;
using ChessGame.Game;
using System.Net.Http.Headers;

namespace Game
{
    public class ChessMatch
    {
        public ChessBoard Board { get; private set; }
        public int Turn { get; private set; }
        public Color CurrentPlayer { get; private set; }
        public bool GameOver { get; private set; }

        private HashSet<Piece> Pieces;

        private HashSet<Piece> CapturedPieces;

        public Piece PossibleEnPassant { get; private set; }

        public bool Xeque { get; private set; }

        public ChessMatch()
        {
            Board = new ChessBoard(8, 8);
            Turn = 1;
            CurrentPlayer = Color.White;
            GameOver = false;
            Pieces = new HashSet<Piece>();
            CapturedPieces = new HashSet<Piece>();
            PossibleEnPassant = null;
            Xeque = false;
            PutPieces();
        }

        public Piece ExecuteMove(Position source, Position target)
        {
            Piece piece = Board.RemovePiece(source);
            piece.IncrementMoves();
            Piece capturedPiece = Board.RemovePiece(target);
            Board.PutPiece(piece, target);

            if (capturedPiece != null)
                CapturedPieces.Add(capturedPiece);

            // #especial move short roque
            if (piece is King && target.Column == source.Column + 2)
            {
                Position sourceRook = new Position(source.Line, source.Column + 3);
                Position targetRook = new Position(source.Line, source.Column + 1);

                Piece rook = Board.RemovePiece(sourceRook);
                rook.IncrementMoves();
                Board.PutPiece(rook, targetRook);
            }

            // #especial move long roque
            if (piece is King && target.Column == source.Column - 2)
            {
                Position sourceRook = new Position(source.Line, source.Column - 4);
                Position targetRook = new Position(source.Line, source.Column - 1);

                Piece rook = Board.RemovePiece(sourceRook);
                rook.IncrementMoves();
                Board.PutPiece(rook, targetRook);
            }

            // #especial move en passant
            if (piece is Pawn)
            {
                if (source.Column != target.Column && capturedPiece == null)
                {
                    Position positionPawn;
                    if (piece.Color == Color.White)
                    {
                        positionPawn = new Position(target.Line + 1, target.Column);
                    }
                    else
                    {
                        positionPawn = new Position(target.Line - 1, target.Column);
                    }

                    capturedPiece = Board.RemovePiece(positionPawn);
                    CapturedPieces.Add(capturedPiece);
                }
            }

            return capturedPiece;
        }

        public void MakesMove(Position source, Position target)
        {
            Piece capturedPiece = ExecuteMove(source, target);

            if (IsXeque(CurrentPlayer))
            {
                UndoMove(source, target, capturedPiece);
                throw new BoardException("You can't put yourself in check");
            }

            Piece piece = Board.GetPiece(target);

            // #especial move promotion
            if (piece is Pawn)
            {
                if ((piece.Color == Color.White && target.Line == 0) ||
                    (piece.Color == Color.Black && target.Line == 7))
                {
                    piece = Board.RemovePiece(target);
                    Pieces.Remove(piece);
                    Piece queen = new Queen(Board, piece.Color);
                    Board.PutPiece(queen, target);
                    Pieces.Add(queen);
                }
            }

            if (IsXeque(Adversary(CurrentPlayer)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }

            if (TestXequeMate(Adversary(CurrentPlayer)))
            {
                GameOver = true;
            }
            else
            {
                Turn++;
                ChangePlayer();
            }

            // #especial move en passant
            if (piece is Pawn && (target.Line == source.Line - 2 ||
                target.Line == source.Line + 2))
            {
                PossibleEnPassant = piece;
            }
            else
            {
                PossibleEnPassant = null;
            }
        }

        public void UndoMove(Position source, Position target, Piece captured)
        {
            Piece piece = Board.RemovePiece(target);
            piece.DecrementMoves();

            if (captured != null)
            {
                Board.PutPiece(captured, target);
                CapturedPieces.Remove(captured);
            }

            Board.PutPiece(piece, source);

            // #especial move short roque
            if (piece is King && target.Column == source.Column + 2)
            {
                Position sourceRook = new Position(source.Line, source.Column + 3);
                Position targetRook = new Position(source.Line, source.Column + 1);

                Piece rook = Board.RemovePiece(targetRook);
                rook.DecrementMoves();
                Board.PutPiece(rook, sourceRook);
            }

            // #especial move long roque
            if (piece is King && target.Column == source.Column - 2)
            {
                Position sourceRook = new Position(source.Line, source.Column - 4);
                Position targetRook = new Position(source.Line, source.Column - 1);

                Piece rook = Board.RemovePiece(targetRook);
                rook.DecrementMoves();
                Board.PutPiece(rook, sourceRook);
            }

            // #especial move en passant
            if (piece is Pawn)
            {
                if (source.Column != target.Column && captured == PossibleEnPassant)
                {
                    Piece pawn = Board.RemovePiece(target);
                    Position positionPawn;
                    if (piece.Color == Color.White)
                    {
                        positionPawn = new Position(3, target.Column);
                    }
                    else
                    {
                        positionPawn = new Position(4, target.Column);
                    }

                    Board.PutPiece(pawn, positionPawn);
                }
            }
        }

        public void ValidateSourcePosition(Position position)
        {
            if (Board.GetPiece(position) is null)
                throw new BoardException("Not exists piece in this position");

            if (CurrentPlayer != Board.GetPiece(position).Color)
                throw new BoardException("The chosen piece is not yours");

            if (!Board.GetPiece(position).ThereIsPossibleMoves())
                throw new BoardException("There is not possible moves for this piece");
        }

        public void ValidateTargetPostion(Position origin, Position target)
        {
            if (!Board.GetPiece(origin).CanMoveForPosition(target))
            {
                throw new BoardException("Target position is invalid");
            }
        }

        private void ChangePlayer()
        {
            if (CurrentPlayer == Color.White)
            {
                CurrentPlayer = Color.Black;
            }
            else
            {
                CurrentPlayer = Color.White;
            }
        }

        public HashSet<Piece> AllPiecesCapturedPerColor(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (var item in CapturedPieces)
            {
                if (item.Color == color)
                {
                    aux.Add(item);
                }
            }

            return aux;
        }

        public HashSet<Piece> PiecesAtGame(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (var item in Pieces)
            {
                if (item.Color == color)
                {
                    aux.Add(item);
                }
            }

            aux.ExceptWith(AllPiecesCapturedPerColor(color));
            return aux;            
        }

        public void PutNewPiece(char column, int line, Piece piece)
        {
            Board.PutPiece(piece, new ChessPosition(column, line).ToBoardPosition());
            Pieces.Add(piece);
        }

        private Color Adversary(Color color)
        {
            if (color == Color.White)
            {
                return Color.Black;
            }
            else
            {
                return Color.White;
            }
        }

        private Piece KingFromColor(Color color)
        {
            foreach (var item in PiecesAtGame(color))
            {
                if (item is King)
                    return item;
            }

            return null;
        }

        public bool IsXeque(Color color)
        {
            Piece king = KingFromColor(color);

            if (king is null)
                throw new BoardException($"There is no King in that color: {color}");

            foreach (var item in PiecesAtGame(Adversary(color)))
            {
                bool[,] matriz = item.PossibleMoves();
                if (matriz[king.Position.Line, king.Position.Column])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TestXequeMate(Color color)
        {
            if (!IsXeque(color))
                return false;

            foreach (var item in PiecesAtGame(color))
            {
                bool[,] matriz = item.PossibleMoves();
                for(int i = 0; i < Board.Lines; i++)
                {
                    for(int j = 0; j < Board.Columns; j++)
                    {
                        if (matriz[i, j])
                        {
                            Position source = item.Position;
                            Position target = new Position(i, j);
                            Piece caputredPiece = ExecuteMove(source, target);
                            bool isXeque = IsXeque(color);
                            UndoMove(source, target, caputredPiece);

                            if (!Xeque)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        private void PutPieces()
        {
            PutNewPiece('a', 1, new Rook(Board, Color.White));
            PutNewPiece('h', 1, new Rook(Board, Color.White));
            PutNewPiece('b', 1, new Horse(Board, Color.White));
            PutNewPiece('g', 1, new Horse(Board, Color.White));
            PutNewPiece('c', 1, new Bishop(Board, Color.White));
            PutNewPiece('f', 1, new Bishop(Board, Color.White));
            PutNewPiece('d', 1, new Queen(Board, Color.White));
            PutNewPiece('e', 1, new King(Board, Color.White, this));
            PutNewPiece('a', 2, new Pawn(Board, Color.White, this));
            PutNewPiece('b', 2, new Pawn(Board, Color.White, this));
            PutNewPiece('c', 2, new Pawn(Board, Color.White, this));
            PutNewPiece('d', 2, new Pawn(Board, Color.White, this));
            PutNewPiece('e', 2, new Pawn(Board, Color.White, this));
            PutNewPiece('f', 2, new Pawn(Board, Color.White, this));
            PutNewPiece('g', 2, new Pawn(Board, Color.White, this));
            PutNewPiece('h', 2, new Pawn(Board, Color.White, this));

            PutNewPiece('a', 8, new Rook(Board, Color.Black));
            PutNewPiece('h', 8, new Rook(Board, Color.Black));
            PutNewPiece('b', 8, new Horse(Board, Color.Black));
            PutNewPiece('g', 8, new Horse(Board, Color.Black));
            PutNewPiece('c', 8, new Bishop(Board, Color.Black));
            PutNewPiece('f', 8, new Bishop(Board, Color.Black));
            PutNewPiece('d', 8, new Queen(Board, Color.Black));
            PutNewPiece('e', 8, new King(Board, Color.Black, this));
            PutNewPiece('a', 7, new Pawn(Board, Color.Black, this));
            PutNewPiece('b', 7, new Pawn(Board, Color.Black, this));
            PutNewPiece('c', 7, new Pawn(Board, Color.Black, this));
            PutNewPiece('d', 7, new Pawn(Board, Color.Black, this));
            PutNewPiece('e', 7, new Pawn(Board, Color.Black, this));
            PutNewPiece('f', 7, new Pawn(Board, Color.Black, this));
            PutNewPiece('g', 7, new Pawn(Board, Color.Black, this));
            PutNewPiece('h', 7, new Pawn(Board, Color.Black, this));
        }
    }
}
