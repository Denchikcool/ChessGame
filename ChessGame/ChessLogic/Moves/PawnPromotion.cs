using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.Moves
{
    public class PawnPromotion : Move
    {
        public override MoveType Type => MoveType.PawnPromotion;
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }
        private readonly PieceType newType;

        public PawnPromotion(Position fromPosition, Position toPosition, PieceType newType)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition;
            this.newType = newType;
        }

        private Piece CreatePromotionPiece(Player color)
        {
            return newType switch
            {
                PieceType.Knight => new Knight(color),
                PieceType.Bishop => new Bishop(color),
                PieceType.Rook => new Rook(color),
                _ => new Queen(color)
            };
        }

        public override bool Execute(Board board)
        {
            Piece pawn = board[FromPosition];
            board[FromPosition] = null;

            Piece promotionPiece = CreatePromotionPiece(pawn.Color);
            promotionPiece.HasMoved = true;
            board[ToPosition] = promotionPiece;

            return true;
        }
    }
}
