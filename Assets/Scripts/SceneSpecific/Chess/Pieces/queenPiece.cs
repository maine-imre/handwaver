/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

namespace IMRE.Chess3D
{
    /// <summary>
    ///     The behaviour controls for the queen piece in spatial chess.
    /// </summary>
    public class queenPiece : AbstractPiece
    {
        private void Start()
        {
            Board = UnityEngine.GameObject.Find("chessBoard").GetComponent<chessBoard>();
            PieceType = chessBoard.PieceType.queen;
        }

        public override void capture()
        {
            Board.pieceCaptured(this);
        }

        public override bool IsValid(UnityEngine.Vector3 moveToTest)
        {
            UnityEngine.Vector3 tmp = moveToTest - Location;
            if (tmp == UnityEngine.Vector3.zero)
                return false;
            if ((tmp.x == tmp.y) && (tmp.x == tmp.z))
                return true;
            if ((tmp.x == tmp.y) && (tmp.z == 0))
                return true;
            if ((tmp.x == tmp.z) && (tmp.y == 0))
                return true;
            if ((tmp.y == tmp.z) && (tmp.x == 0))
                return true;
            if ((tmp.y == 0) && (tmp.z == 0))
                return true;
            if ((tmp.x == 0) && (tmp.z == 0))
                return true;
            if ((tmp.x == 0) && (tmp.y == 0))
                return true;
            return false;
        }

        public override System.Collections.Generic.List<UnityEngine.Vector3> validMoves()
        {
            throw new System.NotImplementedException();
        }
    }
}