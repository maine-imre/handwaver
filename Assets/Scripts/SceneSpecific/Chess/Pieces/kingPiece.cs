/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

namespace IMRE.Chess3D
{
    /// <summary>
    ///     The behaviour controls for the king piece in spatial chess.
    /// </summary>
    public class kingPiece : AbstractPiece
    {
        private void Start()
        {
            Board = UnityEngine.GameObject.Find("chessBoard").GetComponent<chessBoard>();
            PieceType = chessBoard.PieceType.king;
        }

        public override void capture()
        {
            UnityEngine.Debug.Log("Game Ends! " + Team + " lost the game.");
        }

        public override bool IsValid(UnityEngine.Vector3 moveToTest)
        {
            UnityEngine.Vector3 tmp = moveToTest - Location;
            if (tmp == UnityEngine.Vector3.zero)
                return false;
            if (Board.TestLocation(moveToTest) != null)
                if (Board.TestLocation(moveToTest).Team == Team)
                    return false;

            if (UnityEngine.Mathf.Abs(UnityEngine.Vector3.Dot(tmp, UnityEngine.Vector3.right)) > 1) return false;

            if (UnityEngine.Mathf.Abs(UnityEngine.Vector3.Dot(tmp, UnityEngine.Vector3.up)) > 1) return false;
            if (UnityEngine.Mathf.Abs(UnityEngine.Vector3.Dot(tmp, UnityEngine.Vector3.forward)) > 1) return false;

            if (Board.placeSelfInCheck(this, moveToTest)) return false;
            return true;
            //check if in check
        }

        public override System.Collections.Generic.List<UnityEngine.Vector3> validMoves()
        {
            return allValidMoves.kingMoves(Location, Board.myTeam(Team));
        }
    }
}