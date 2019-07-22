/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

namespace IMRE.Chess3D
{
    /// <summary>
    ///     The behaviour controls for the pawn piece in spatial chess.
    /// </summary>
    public class pawnPiece : AbstractPiece
    {
        private void Start()
        {
            Board = UnityEngine.GameObject.Find("chessBoard").GetComponent<chessBoard>();
            PieceType = chessBoard.PieceType.pawn;
        }

        public override void capture()
        {
            Board.pieceCaptured(this);
        }

        public override bool IsValid(UnityEngine.Vector3 attemptedMove)
        {
            System.Collections.Generic.List<UnityEngine.Vector3> possibleMoves = validMoves();
            return possibleMoves.Contains(attemptedMove);
        }

        public override System.Collections.Generic.List<UnityEngine.Vector3> validMoves()
        {
            return allValidMoves.pawnMoves(Location, otherTeam(), myTeam(), Team);
        }
#pragma warning disable 0414
        private bool isBlocked = false;
        private bool hasMoved = false;

#pragma warning restore 0414
    }
}