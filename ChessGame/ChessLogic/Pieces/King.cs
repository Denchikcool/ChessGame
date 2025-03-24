using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override Player Color { get; }

        private static readonly Direction[] directions = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West,
            Direction.NorthEast,
            Direction.SouthEast,
            Direction.NorthWest,
            Direction.SouthWest
        };

        public King(Player color)
        {
            Color = color;
        }

        private static bool IsUnmovedRook(Position position, Board board)
        {
            if (board.IsEmpty(position))
            {
                return false;
            }
            Piece piece = board[position];
            return piece.Type == PieceType.Rook && !piece.HasMoved;
        }

        private static bool AllEmpty(IEnumerable<Position> positions, Board board)
        {
            return positions.All(position =>  board.IsEmpty(position));
        }

        private bool CanCastleKingSide(Position fromPosition, Board board)
        {
            if(HasMoved)
            {
                return false;
            }

            Position rookPosition = new Position(fromPosition.Row, 7);
            Position[] betweenPositions = new Position[] { new(fromPosition.Row, 5), new(fromPosition.Row, 6) };

            return IsUnmovedRook(rookPosition, board) && AllEmpty(betweenPositions, board);
        }

        private bool CanCastleQueenSide(Position fromPosition, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            Position rookPosition = new Position(fromPosition.Row, 0);
            Position[] betweenPositions = new Position[] { new(fromPosition.Row, 1), new(fromPosition.Row, 2), new(fromPosition.Row, 3) };

            return IsUnmovedRook(rookPosition, board) && AllEmpty(betweenPositions, board);
        }

        public override Piece Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private IEnumerable<Position> MovePositions(Position fromPosition, Board board)
        {
            foreach(Direction direction in directions)
            {
                Position to = fromPosition + direction;

                if (!Board.IsInside(to))
                {
                    continue;
                }

                if(board.IsEmpty(to) || board[to].Color != Color)
                {
                    yield return to;
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            foreach(Position to in MovePositions(fromPosition, board))
            {
                yield return new NormalMove(fromPosition, to);
            }

            if(CanCastleKingSide(fromPosition, board))
            {
                yield return new Castle(MoveType.CastleKS, fromPosition);
            }

            if(CanCastleQueenSide(fromPosition, board))
            {
                yield return new Castle(MoveType.CastleQS, fromPosition);
            }
        }

        public override bool CanCaptureOpponentKing(Position fromPosition, Board board)
        {
            return MovePositions(fromPosition, board).Any(to =>
            {
                Piece piece = board[to];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
