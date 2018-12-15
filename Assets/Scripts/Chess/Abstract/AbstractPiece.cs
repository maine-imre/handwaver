/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

namespace IMRE.Chess3D
{
    [RequireComponent(typeof(InteractionBehaviour))]

/// <summary>
/// An abstracted version of the chess piece.
/// </summary>
	public abstract class AbstractPiece : MonoBehaviour
    {
        private chessBoard board;

        private Vector3 location;
        private chessBoard.currentTeam team;
        private chessBoard.PieceType pieceType;
        private bool isCaptured = false;

        public List<AbstractPiece> myTeam()
        {
            return board.myTeam(team);
        }
        public List<AbstractPiece> otherTeam()
        {
            return board.otherTeam(team);
        }


        /// <summary>
        /// Tests requested move compared to current location 
        /// </summary>
        /// <param name="attemptedMove">the attempted move on this current piece</param>
        /// <returns>true if the move requested is valid</returns>
        public abstract bool IsValid(Vector3 attemptedMove);

        public abstract List<Vector3> validMoves();



        /// <summary>
        /// Called when the peice is captured
        /// </summary>
        public abstract void capture();



        /// <summary>
        /// Moves the piece after testing if it is a valid move
        /// </summary>
        /// <param name="attemptedMove">the attempted move on this current piece</param>
        public void move(Vector3 attemptedMove)
        {
            if (IsValid(attemptedMove))
            {
                AbstractPiece pieceInSpot = board.TestLocation(attemptedMove);
                if (pieceInSpot != null)
                {
                    pieceInSpot.capture();
                }
                this.Location.Set(attemptedMove.x, attemptedMove.y, attemptedMove.z);
                board.Check(this, attemptedMove);
            }

        }

        #region GettersSetters
        /// <summary>
        /// Current location of the piece
        /// </summary>
        public Vector3 Location
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

        public chessBoard Board
        {
            get
            {
                return board;
            }

            set
            {
                board = value;
            }
        }

        #endregion

    }
}