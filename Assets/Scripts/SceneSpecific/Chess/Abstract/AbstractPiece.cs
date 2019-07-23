/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using Unity.Mathematics;

namespace IMRE.Chess3D
{
/// <summary>
/// An abstracted version of the chess piece.
/// </summary>
	public abstract class AbstractPiece
    {
        private int3 location;
        private chessBoard.currentTeam team;
        private chessBoard.PieceType pieceType;
        private bool isCaptured = false;

        public List<AbstractPiece> myTeam()
        {
            return chessBoard.myTeam(team);
        }
        public List<AbstractPiece> otherTeam()
        {
            return chessBoard.otherTeam(team);
        }
        
        public int3[] validMoves => allValidMoves.validMoves(this);

        public bool IsValid(int3 newLocation)
        {
            return ((IList) validMoves).Contains(newLocation);
        }



        /// <summary>
        /// Called when the peice is captured
        /// </summary>
        public abstract void capture();



        /// <summary>
        /// Moves the piece after testing if it is a valid move
        /// </summary>
        /// <param name="attemptedMove">the attempted move on this current piece</param>
        public void move(int3 attemptedMove)
        {
            if (IsValid(attemptedMove))
            {
                AbstractPiece pieceInSpot = chessBoard.TestLocation(attemptedMove);
                if (pieceInSpot != null && pieceInSpot.Team != team)
                {
                    pieceInSpot.capture();
                }

                this.Location = attemptedMove;
                chessBoard.Check(this, attemptedMove);
            }
		//TODO remove preview of piece.

        }

        #region GettersSetters
        /// <summary>
        /// Current location of the piece
        /// </summary>
        public int3 Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }

        /// <summary>
        /// Is piece captured by opponent
        /// </summary>
        public bool IsCaptured
        {
            get
            {
                return isCaptured;
            }

            set
            {
                this.isCaptured = value;
                //move piece
            }
        }

        public chessBoard.PieceType PieceType
        {
            get
            {
                return pieceType;
            }

            set
            {
                pieceType = value;
            }
        }
        public chessBoard.currentTeam Team
        {
            get
            {
                return team;
            }

            set
            {
                team = value;
            }
        }

        #endregion
	public void highlightValidCells(){
		foreach(int3 validMove in validMoves){
			//TODO higlihgt validMove;
			Debug.Log(validMove);
		}
	}
		
	public void movePreview(){
		//TODO make preview to move
	}
    }
}

