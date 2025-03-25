using Xunit;
using System.Linq;
using ChessLogic;
using ChessLogic.Moves;

namespace ChessLogic.Tests
{
    public class ChessPieceTests
    {
        #region Pawn Tests
        [Fact]
        public void Pawn_White_InitialPosition_CanMoveOneOrTwoSquaresForward()
        {
            var pawn = new Pawn(Player.White);
            var board = new Board();
            var initialPos = new Position(7, 1);

            var moves = pawn.GetMoves(initialPos, board).ToList();

            Assert.Equal(2, moves.Count);

            var oneStep = moves.FirstOrDefault(m => m.ToPosition == new Position(6, 1));
            var twoSteps = moves.FirstOrDefault(m => m.ToPosition == new Position(5, 1));

            Assert.NotNull(oneStep);
            Assert.IsType<NormalMove>(oneStep);

            Assert.NotNull(twoSteps);
            Assert.IsType<DoublePawn>(twoSteps);
        }

        [Fact]
        public void Pawn_GeneratesPromotionMoves_WhenOn7thRank()
        {
            var pawn = new Pawn(Player.White);
            var board = new Board();
            var pawnPos = new Position(1, 1);
            board[pawnPos] = pawn;

            var moves = pawn.GetMoves(pawnPos, board)
                           .Where(m => m is PawnPromotion)
                           .ToList();

            Assert.Equal(4, moves.Count);
            Assert.All(moves, m =>
            {
                Assert.IsType<PawnPromotion>(m);
                Assert.Equal(new Position(0, 1), m.ToPosition);
            });
        }

        [Fact]
        public void Pawn_Black_InitialPosition_CanMoveOneOrTwoSquaresForward()
        {
            var pawn = new Pawn(Player.Black);
            var board = new Board();
            var initialPos = new Position(1, 1);
            board[initialPos] = pawn;

            var forwardMoves = pawn.GetMoves(initialPos, board)
                                  .Where(m => m.ToPosition.Column == 1)
                                  .ToList();

            Assert.Equal(2, forwardMoves.Count);
            Assert.Contains(forwardMoves, m => m.ToPosition == new Position(2, 1));
            Assert.Contains(forwardMoves, m => m.ToPosition == new Position(3, 1));
        }

        [Fact]
        public void Pawn_CanCaptureDiagonally()
        {
            var pawn = new Pawn(Player.White);
            var board = new Board();
            var pawnPos = new Position(4, 4);
            board[pawnPos] = pawn;
            board[new Position(3, 3)] = new Pawn(Player.Black);

            var moves = pawn.GetMoves(pawnPos, board).ToList();

            Assert.Contains(moves, m => m is NormalMove && m.ToPosition == new Position(3, 3));
        }

        [Fact]
        public void Pawn_PromotionMoves_GeneratedWhenReachingLastRow()
        {
            var pawn = new Pawn(Player.White);
            var board = new Board();
            var pawnPos = new Position(1, 3);

            var moves = pawn.GetMoves(pawnPos, board).ToList();

            Assert.Equal(4, moves.Count);
            Assert.All(moves, m => Assert.IsType<PawnPromotion>(m));
        }
        #endregion

        #region Knight Tests
        [Fact]
        public void Knight_CentralPosition_Has8PossibleMovesOnEmptyBoard()
        {
            var knight = new Knight(Player.White);
            var board = new Board();
            var centerPos = new Position(3, 3);

            var moves = knight.GetMoves(centerPos, board).ToList();

            Assert.Equal(8, moves.Count);
        }

        [Fact]
        public void Knight_CornerPosition_Has2PossibleMovesOnEmptyBoard()
        {
            var knight = new Knight(Player.White);
            var board = new Board();
            var cornerPos = new Position(0, 0);

            var moves = knight.GetMoves(cornerPos, board).ToList();

            Assert.Equal(2, moves.Count);
        }

        [Fact]
        public void Knight_CannotMoveToSquareOccupiedBySameColor()
        {
            var knight = new Knight(Player.White);
            var board = new Board();
            var knightPos = new Position(3, 3);
            board[new Position(4, 5)] = new Pawn(Player.White);

            var moves = knight.GetMoves(knightPos, board).ToList();

            Assert.DoesNotContain(moves, m => m.ToPosition == new Position(4, 5));
        }
        #endregion

        #region Bishop Tests
        [Fact]
        public void Bishop_CentralPosition_Has13PossibleMovesOnEmptyBoard()
        {
            var bishop = new Bishop(Player.White);
            var board = new Board();
            var centerPos = new Position(3, 3);

            var moves = bishop.GetMoves(centerPos, board).ToList();

            Assert.Equal(13, moves.Count);
        }

        [Fact]
        public void Bishop_CannotJumpOverPieces()
        {
            var bishop = new Bishop(Player.White);
            var board = new Board();
            var bishopPos = new Position(0, 0);
            board[new Position(1, 1)] = new Pawn(Player.White);

            var moves = bishop.GetMoves(bishopPos, board).ToList();

            Assert.Empty(moves);
        }

        [Fact]
        public void Bishop_CanCaptureOpponentPiece()
        {
            var bishop = new Bishop(Player.White);
            var board = new Board();
            var bishopPos = new Position(0, 0);
            board[new Position(3, 3)] = new Pawn(Player.Black);

            var moves = bishop.GetMoves(bishopPos, board).ToList();

            Assert.Contains(moves, m => m.ToPosition == new Position(3, 3));
        }
        #endregion

        #region Rook Tests
        [Fact]
        public void Rook_CentralPosition_Has14PossibleMovesOnEmptyBoard()
        {
            var rook = new Rook(Player.White);
            var board = new Board();
            var centerPos = new Position(3, 3);

            var moves = rook.GetMoves(centerPos, board).ToList();

            Assert.Equal(14, moves.Count);
        }

        [Fact]
        public void Rook_CannotJumpOverPieces()
        {
            var rook = new Rook(Player.White);
            var board = new Board();
            var rookPos = new Position(0, 0);
            board[rookPos] = rook;
            board[new Position(0, 1)] = new Pawn(Player.White);

            var moves = rook.GetMoves(rookPos, board)
                           .Where(m => m.ToPosition.Column > 0)
                           .ToList();

            Assert.Empty(moves);
        }

        [Fact]
        public void Rook_CanCaptureOpponentPiece()
        {
            var rook = new Rook(Player.White);
            var board = new Board();
            var rookPos = new Position(0, 0);
            board[new Position(0, 3)] = new Pawn(Player.Black);

            var moves = rook.GetMoves(rookPos, board).ToList();

            Assert.Contains(moves, m => m.ToPosition == new Position(0, 3));
        }
        #endregion

        #region Queen Tests
        [Fact]
        public void Queen_CentralPosition_Has27PossibleMovesOnEmptyBoard()
        {
            var queen = new Queen(Player.White);
            var board = new Board();
            var centerPos = new Position(3, 3);

            var moves = queen.GetMoves(centerPos, board).ToList();

            Assert.Equal(27, moves.Count);
        }

        [Fact]
        public void Queen_CombinesRookAndBishopMoves()
        {
            var queen = new Queen(Player.White);
            var board = new Board();
            var queenPos = new Position(0, 0);

            var moves = queen.GetMoves(queenPos, board).ToList();

            Assert.Contains(moves, m => m.ToPosition == new Position(0, 1));
            Assert.Contains(moves, m => m.ToPosition == new Position(1, 0));
            Assert.Contains(moves, m => m.ToPosition == new Position(1, 1));
        }
        #endregion

        #region King Tests
        [Fact]
        public void King_CentralPosition_Has8PossibleMovesOnEmptyBoard()
        {
            var king = new King(Player.White);
            var board = new Board();
            var centerPos = new Position(3, 3);

            var moves = king.GetMoves(centerPos, board).ToList();

            Assert.Equal(8, moves.Count);
        }

        [Fact]
        public void King_CannotMoveIntoCheck()
        {
            var king = new King(Player.White);
            var board = new Board();
            var kingPos = new Position(0, 0);
            board[kingPos] = king;
            board[new Position(1, 7)] = new Queen(Player.Black);

            var moves = king.GetMoves(kingPos, board)
                           .Where(m => m.IsLegal(board))
                           .ToList();

            Assert.DoesNotContain(moves, m => m.ToPosition == new Position(1, 1));
        }

        [Fact]
        public void King_CanCastle_WhenConditionsMet()
        {
            var king = new King(Player.White);
            var board = new Board();
            var kingPos = new Position(0, 4);

            board[new Position(0, 0)] = new Rook(Player.White);
            board[new Position(0, 7)] = new Rook(Player.White);

            var moves = king.GetMoves(kingPos, board).ToList();

            Assert.Contains(moves, m => m is Castle);
            Assert.Equal(2, moves.Count(m => m is Castle));
        }
        #endregion
    }

    public class MoveTests
    {
        #region NormalMove Tests
        [Fact]
        public void NormalMove_Execute_MovesPieceCorrectly()
        {
            var board = new Board();
            var piece = new Rook(Player.White);
            var from = new Position(0, 0);
            var to = new Position(0, 1);
            board[from] = piece;

            var move = new NormalMove(from, to);
            bool result = move.Execute(board);

            Assert.Null(board[from]);
            Assert.Equal(piece, board[to]);
            Assert.True(piece.HasMoved);
            Assert.False(result);
        }

        [Fact]
        public void NormalMove_Execute_WithCapture_ReturnsTrue()
        {
            var board = new Board();
            var attacker = new Rook(Player.White);
            var defender = new Pawn(Player.Black);
            var from = new Position(0, 0);
            var to = new Position(0, 1);
            board[from] = attacker;
            board[to] = defender;

            var move = new NormalMove(from, to);
            bool result = move.Execute(board);

            Assert.True(result);
        }

        [Fact]
        public void NormalMove_IsLegal_ReturnsFalse_WhenLeavesKingInCheck()
        {
            var board = new Board();
            var king = new King(Player.White);
            var rook = new Rook(Player.White);
            var enemyQueen = new Queen(Player.Black);

            board[new Position(0, 0)] = king;
            board[new Position(1, 0)] = rook;
            board[new Position(7, 0)] = enemyQueen;

            var move = new NormalMove(new Position(1, 0), new Position(1, 1));
            bool isLegal = move.IsLegal(board);

            Assert.False(isLegal);
        }
        #endregion

        #region DoublePawn Tests
        [Fact]
        public void DoublePawn_Execute_SetsSkippedPosition()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            var from = new Position(1, 0);
            var to = new Position(3, 0);
            board[from] = pawn;

            var move = new DoublePawn(from, to);
            move.Execute(board);

            Assert.Equal(new Position(2, 0), board.GetPawnSkipPosition(Player.White));
        }

        [Fact]
        public void DoublePawn_Execute_MovesPawnTwoSquares()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            var from = new Position(1, 0);
            var to = new Position(3, 0);
            board[from] = pawn;

            var move = new DoublePawn(from, to);
            move.Execute(board);

            Assert.Null(board[from]);
            Assert.Equal(pawn, board[to]);
        }
        #endregion

        #region Castle Tests
        [Fact]
        public void Castle_KingSide_Execute_MovesKingAndRook()
        {
            var board = new Board();
            var king = new King(Player.White);
            var rook = new Rook(Player.White);

            var kingPos = new Position(0, 4);
            var rookPos = new Position(0, 7);

            board[kingPos] = king;
            board[rookPos] = rook;

            var move = new Castle(MoveType.CastleKS, kingPos);
            move.Execute(board);

            Assert.Null(board[kingPos]);
            Assert.Null(board[rookPos]);
            Assert.Equal(king, board[new Position(0, 6)]);
            Assert.Equal(rook, board[new Position(0, 5)]);
        }

        [Fact]
        public void Castle_QueenSide_Execute_MovesKingAndRook()
        {
            var board = new Board();
            var king = new King(Player.White);
            var rook = new Rook(Player.White);

            var kingPos = new Position(0, 4);
            var rookPos = new Position(0, 0);

            board[kingPos] = king;
            board[rookPos] = rook;

            var move = new Castle(MoveType.CastleQS, kingPos);
            move.Execute(board);

            Assert.Null(board[kingPos]);
            Assert.Null(board[rookPos]);
            Assert.Equal(king, board[new Position(0, 2)]);
            Assert.Equal(rook, board[new Position(0, 3)]);
        }

        [Fact]
        public void Castle_IsLegal_ReturnsFalse_WhenKingInCheck()
        {
            var board = new Board();
            var king = new King(Player.White);
            var rook = new Rook(Player.White);
            var enemyRook = new Rook(Player.Black);

            board[new Position(0, 4)] = king;
            board[new Position(0, 7)] = rook;
            board[new Position(7, 4)] = enemyRook;

            var move = new Castle(MoveType.CastleKS, new Position(0, 4));
            bool isLegal = move.IsLegal(board);

            Assert.False(isLegal);
        }
        #endregion

        #region EnPassant Tests
        [Fact]
        public void EnPassant_Execute_CapturesCorrectPawn()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            var enemyPawn = new Pawn(Player.Black);

            var from = new Position(4, 1);
            var to = new Position(5, 0);
            var capturePos = new Position(4, 0);

            board[from] = pawn;
            board[capturePos] = enemyPawn;
            board.SetPawnSkipPosition(Player.Black, new Position(6, 0));

            var move = new EnPassant(from, to);
            move.Execute(board);

            Assert.Null(board[from]);
            Assert.Null(board[capturePos]);
            Assert.Equal(pawn, board[to]);
        }
        #endregion

        #region PawnPromotion Tests
        [Fact]
        public void PawnPromotion_Execute_ReplacesPawnWithPromotionPiece()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            var from = new Position(6, 0);
            var to = new Position(7, 0);
            board[from] = pawn;

            var move = new PawnPromotion(from, to, PieceType.Queen);
            move.Execute(board);

            Assert.Null(board[from]);
            Assert.IsType<Queen>(board[to]);
            Assert.Equal(Player.White, board[to].Color);
        }

        [Fact]
        public void PawnPromotion_CreatePromotionPiece_CreatesCorrectPieceType()
        {
            var knightPromotion = new PawnPromotion(new Position(0, 0), new Position(0, 0), PieceType.Knight);
            var knight = knightPromotion.CreatePromotionPiece(Player.White);
            Assert.IsType<Knight>(knight);

            var bishopPromotion = new PawnPromotion(new Position(0, 0), new Position(0, 0), PieceType.Bishop);
            var bishop = bishopPromotion.CreatePromotionPiece(Player.White);
            Assert.IsType<Bishop>(bishop);

            var rookPromotion = new PawnPromotion(new Position(0, 0), new Position(0, 0), PieceType.Rook);
            var rook = rookPromotion.CreatePromotionPiece(Player.White);
            Assert.IsType<Rook>(rook);

            var queenPromotion = new PawnPromotion(new Position(0, 0), new Position(0, 0), PieceType.Queen);
            var queen = queenPromotion.CreatePromotionPiece(Player.White);
            Assert.IsType<Queen>(queen);
        }
        #endregion
    }

    public class BoardTests
    {
        [Fact]
        public void InitialBoard_HasCorrectPieceSetup()
        {
            var board = Board.Initial();

            Assert.IsType<Rook>(board[0, 0]);
            Assert.IsType<Knight>(board[0, 1]);
            Assert.IsType<Bishop>(board[0, 2]);
            Assert.IsType<Queen>(board[0, 3]);
            Assert.IsType<King>(board[0, 4]);
            Assert.IsType<Bishop>(board[0, 5]);
            Assert.IsType<Knight>(board[0, 6]);
            Assert.IsType<Rook>(board[0, 7]);

            Assert.IsType<Rook>(board[7, 0]);
            Assert.IsType<Knight>(board[7, 1]);
            Assert.IsType<Bishop>(board[7, 2]);
            Assert.IsType<Queen>(board[7, 3]);
            Assert.IsType<King>(board[7, 4]);
            Assert.IsType<Bishop>(board[7, 5]);
            Assert.IsType<Knight>(board[7, 6]);
            Assert.IsType<Rook>(board[7, 7]);

            for (int i = 0; i < 8; i++)
            {
                Assert.IsType<Pawn>(board[1, i]);
                Assert.IsType<Pawn>(board[6, i]);
            }
        }

        [Fact]
        public void IsInside_ReturnsCorrectResults()
        {
            Assert.True(Board.IsInside(new Position(0, 0)));
            Assert.True(Board.IsInside(new Position(7, 7)));
            Assert.False(Board.IsInside(new Position(-1, 0)));
            Assert.False(Board.IsInside(new Position(8, 0)));
        }

        [Fact]
        public void Copy_CreatesExactCopy()
        {
            var original = Board.Initial();
            var copy = original.Copy();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var pos = new Position(row, col);
                    if (original.IsEmpty(pos))
                    {
                        Assert.True(copy.IsEmpty(pos));
                    }
                    else
                    {
                        Assert.Equal(original[pos].Type, copy[pos].Type);
                        Assert.Equal(original[pos].Color, copy[pos].Color);
                    }
                }
            }
        }

        [Fact]
        public void IsInCheck_DetectsCheckCorrectly()
        {
            var board = new Board();
            board[0, 0] = new King(Player.Black);
            board[7, 7] = new King(Player.White);
            board[1, 1] = new Queen(Player.White);

            Assert.True(board.IsInCheck(Player.Black));
            Assert.False(board.IsInCheck(Player.White));
        }
    }

    public class CountingTests
    {
        [Fact]
        public void Increment_CountsPiecesCorrectly()
        {
            var counting = new Counting();

            counting.Increment(Player.White, PieceType.Pawn);
            counting.Increment(Player.White, PieceType.Queen);
            counting.Increment(Player.Black, PieceType.Pawn);

            Assert.Equal(1, counting.White(PieceType.Pawn));
            Assert.Equal(1, counting.White(PieceType.Queen));
            Assert.Equal(1, counting.Black(PieceType.Pawn));
            Assert.Equal(3, counting.TotalCount);
        }
    }

    public class DirectionTests
    {
        [Fact]
        public void OperatorPlus_AddsDirectionsCorrectly()
        {
            var dir1 = new Direction(1, 0);
            var dir2 = new Direction(0, 1);
            var result = dir1 + dir2;

            Assert.Equal(1, result.RowDelta);
            Assert.Equal(1, result.ColumnDelta);
        }
    }

    public class PositionTests
    {
        [Fact]
        public void SquareColor_ReturnsCorrectColor()
        {
            var a1 = new Position(0, 0);
            var a2 = new Position(1, 0);
            var b1 = new Position(0, 1);

            Assert.Equal(Player.White, a1.SquareColor());
            Assert.Equal(Player.Black, a2.SquareColor());
            Assert.Equal(Player.Black, b1.SquareColor());
        }

        [Fact]
        public void OperatorPlus_AddsDirectionToPosition()
        {
            var pos = new Position(3, 3);
            var dir = new Direction(1, -1);
            var result = pos + dir;

            Assert.Equal(4, result.Row);
            Assert.Equal(2, result.Column);
        }
    }

    public class GameStateTests
    {
        [Fact]
        public void MakeMove_ChangesCurrentPlayer()
        {
            var board = new Board();
            board[0, 0] = new King(Player.Black);
            board[7, 7] = new King(Player.White);

            var gameState = new GameState(board, Player.White);
            var move = new NormalMove(new Position(7, 7), new Position(7, 6));

            gameState.MakeMove(move);

            Assert.Equal(Player.Black, gameState.CurrentPlayer);
        }

        [Fact]
        public void IsGameOver_DetectsCheckmate()
        {
            var board = new Board();

            board[0, 0] = new King(Player.Black);
            board[7, 7] = new King(Player.White);
            board[6, 1] = new Queen(Player.White);
            board[7, 1] = new Rook(Player.White);

            var gameState = new GameState(board, Player.White);
            var queenMove = new NormalMove(new Position(6, 1), new Position(1, 1));
            gameState.MakeMove(queenMove);

            Assert.True(gameState.IsGameOver());
            Assert.Equal(Player.White, gameState.Result.Winner);
        }
    }

    public class PlayerExtensionsTests
    {
        [Fact]
        public void Opponent_ReturnsCorrectOpponent()
        {
            Assert.Equal(Player.Black, Player.White.Opponent());
            Assert.Equal(Player.White, Player.Black.Opponent());
            Assert.Equal(Player.None, Player.None.Opponent());
        }
    }
}