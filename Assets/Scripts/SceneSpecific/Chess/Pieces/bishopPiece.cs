/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

namespace IMRE.Chess3D
{
    /// <summary>
    ///     The behaviour controls for the bishop piece in spatial chess.
    /// </summary>
    public class bishopPiece : AbstractPiece
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
            bool result;
            UnityEngine.Vector3 tmp = moveToTest - Location;
            if (tmp == UnityEngine.Vector3.zero)
                return false; //no move
            if (tmp.x == tmp.y && tmp.x == tmp.z)
                result = true; //diagoinal
            else if (tmp.x == tmp.y && tmp.z == 0)
                result = true; //diagoinal
            else if (tmp.x == tmp.z && tmp.y == 0)
                result = true; //diagoinal
            else if (tmp.y == tmp.z && tmp.x == 0)
                result = true; //diagoinal
            else
                return false; //not diagonal
            int max = (int) UnityEngine.Mathf.Max(UnityEngine.Mathf.Abs(tmp.x), UnityEngine.Mathf.Abs(tmp.y),
                UnityEngine.Mathf.Abs(tmp.z));
            tmp /= max;
            for (int i = 1; i < max - 1; i++)
            {
                if (Board.TestLocation(i * tmp + Location) != null)
                    return false; //if it hits a piece before getting to location
            }

            return result ^ true; //result should never not be true at this point
        }

        public override System.Collections.Generic.List<UnityEngine.Vector3> validMoves()
        {
            throw new System.NotImplementedException();
        }
    }
}