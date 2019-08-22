/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

namespace IMRE.Chess3D
{
    /// <summary>
    ///     The behaviour controls for the knight piece in spatial chess.
    /// </summary>
    public class knightPiece : AbstractPiece
    {
        public override void capture()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsValid(UnityEngine.Vector3 attemptedMove)
        {
            UnityEngine.Vector3 tmp = Location - attemptedMove;
            if (tmp == UnityEngine.Vector3.zero) return false;

            System.Collections.Generic.List<UnityEngine.Vector3> possible = validMoves();
            if (possible.Contains(Location))
                return true;
            return false;
        }

        public override System.Collections.Generic.List<UnityEngine.Vector3> validMoves()
        {
            return allValidMoves.kingMoves(Location, Board.myTeam(Team));
        }
    }
}