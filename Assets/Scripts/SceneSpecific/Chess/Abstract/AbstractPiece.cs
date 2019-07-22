/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

namespace IMRE.Chess3D
{
    [UnityEngine.RequireComponent(typeof(Leap.Unity.Interaction.InteractionBehaviour))]
    /// <summary>
    /// An abstracted version of the chess piece.
    /// </summary>
    public abstract class AbstractPiece : UnityEngine.MonoBehaviour
    {
        public System.Collections.Generic.List<AbstractPiece> myTeam()
        {
            return Board.myTeam(Team);
        }

        public System.Collections.Generic.List<AbstractPiece> otherTeam()
        {
            return Board.otherTeam(Team);
        }


        /// <summary>
        ///     Tests requested move compared to current location
        /// </summary>
        /// <param name="attemptedMove">the attempted move on this current piece</param>
        /// <returns>true if the move requested is valid</returns>
        public abstract bool IsValid(UnityEngine.Vector3 attemptedMove);

        public abstract System.Collections.Generic.List<UnityEngine.Vector3> validMoves();


        /// <summary>
        ///     Called when the peice is captured
        /// </summary>
        public abstract void capture();


        /// <summary>
        ///     Moves the piece after testing if it is a valid move
        /// </summary>
        /// <param name="attemptedMove">the attempted move on this current piece</param>
        public void move(UnityEngine.Vector3 attemptedMove)
        {
            if (IsValid(attemptedMove))
            {
                AbstractPiece pieceInSpot = Board.TestLocation(attemptedMove);
                if (pieceInSpot != null) pieceInSpot.capture();
                Location.Set(attemptedMove.x, attemptedMove.y, attemptedMove.z);
                Board.Check(this, attemptedMove);
            }
        }

        #region GettersSetters

        /// <summary>
        ///     Current location of the piece
        /// </summary>
        public UnityEngine.Vector3 Location { get; set; }

        /// <summary>
        ///     Is piece captured by opponent
        /// </summary>
        public bool IsCaptured { get; set; } = false;

        public chessBoard.PieceType PieceType { get; set; }

        public chessBoard.currentTeam Team { get; set; }

        public chessBoard Board { get; set; }

        #endregion
    }
}