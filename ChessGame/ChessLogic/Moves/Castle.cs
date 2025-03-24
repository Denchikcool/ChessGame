using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Castle : Move
    {
        public override MoveType Type { get; }
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }

        private readonly Direction kingMoveDirection;
        private readonly Position rookFromPosition;
        private readonly Position rookToPosition;

        public Castle(MoveType type, Position kingPosition)
        {
            Type = type;
            FromPosition = kingPosition;

            if(type == MoveType.CastleKS)
            {
                kingMoveDirection = Direction.East;
                ToPosition = new Position(kingPosition.Row, 6);
                rookFromPosition = new Position(kingPosition.Row, 7);
                rookToPosition = new Position(kingPosition.Row, 5);
            }
            else if(type == MoveType.CastleQS) 
            {
                kingMoveDirection= Direction.West;
                ToPosition = new Position(kingPosition.Row, 2);
                rookFromPosition = new Position(kingPosition.Row, 0);
                rookToPosition = new Position(kingPosition.Row, 3);
            }
        }

        public override bool Execute(Board board)
        {
            new NormalMove(FromPosition, ToPosition).Execute(board);
            new NormalMove(rookFromPosition, rookToPosition).Execute(board);

            return false;
        }

        public override bool IsLegal(Board board)
        {
            Player player = board[FromPosition].Color;

            if (board.IsInCheck(player))
            {
                return false;
            }

            Board copy = board.Copy();
            Position kingPositionInCopy = FromPosition;

            for(int i = 0; i < 2; i++)
            {
                new NormalMove(kingPositionInCopy, kingPositionInCopy + kingMoveDirection).Execute(copy);
                kingPositionInCopy += kingMoveDirection;

                if (copy.IsInCheck(player))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
