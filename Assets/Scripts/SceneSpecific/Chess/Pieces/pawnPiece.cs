/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.Chess3D
{
/// <summary>
/// The behaviour controls for the pawn piece in spatial chess.
/// </summary>
	public class pawnPiece : AbstractPiece
    {
#pragma warning disable 0414
		private bool isBlocked = false;
        private bool hasMoved = false;

#pragma warning restore 0414

		private void Start()
        {
            Board = GameObject.Find("chessBoard").GetComponent<chessBoard>();
            PieceType = (chessBoard.PieceType.pawn);
        }
        public override void capture()
        {
            Board.pieceCaptured(this);
        }

        public override bool IsValid(Vector3 attemptedMove)
        {
            List<Vector3> possibleMoves = validMoves();
			return (possibleMoves.Contains(attemptedMove));
		}

        public override List<Vector3> validMoves()
        {
            return allValidMoves.pawnMoves(this.Location, otherTeam(), myTeam(), Team);    
         }
    }
}

