using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class DoublePawn : Move
    {
        public override MoveType Type => MoveType.DoublePawn;
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }

        private readonly Position skippedPosition;

        public DoublePawn(Position fromPosition, Position toPosition)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition;
            skippedPosition = new Position((fromPosition.Row + toPosition.Row) / 2, fromPosition.Column);
        }

        public override bool Execute(Board board)
        {
            Player player = board[FromPosition].Color;
            board.SetPawnSkipPosition(player, skippedPosition);
            new NormalMove(FromPosition, ToPosition).Execute(board);

            return true;
        }
    }
}
