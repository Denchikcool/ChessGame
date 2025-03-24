using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;
        public override Player Color { get; }
        public Knight(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Knight copy = new Knight(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private static IEnumerable<Position> PotentialToPosition(Position fromPosition)
        {
            foreach(Direction verticalDirection in new Direction[] { Direction.North, Direction.South})
            {
                foreach(Direction horizontalDirection in new Direction[] {Direction.West, Direction.East })
                {
                    yield return fromPosition + 2 * verticalDirection + horizontalDirection;
                    yield return fromPosition + 2 * horizontalDirection + verticalDirection;
                }
            }
        }
        
        private IEnumerable<Position> MovePositions(Position fromPosition, Board board)
        {
            return PotentialToPosition(fromPosition).Where(pos => Board.IsInside(pos) 
                && (board.IsEmpty(pos) || board[pos].Color != Color));
        }

        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            return MovePositions(fromPosition, board).Select(to => new NormalMove(fromPosition, to));
        }
    }
}
