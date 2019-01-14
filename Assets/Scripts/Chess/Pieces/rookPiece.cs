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
/// The behaviour controls for the rook piece in spatial chess.
/// </summary>
	public class rookPiece : AbstractPiece
    {
        private void Start()
        {
            Board = GameObject.Find("chessBoard").GetComponent<chessBoard>();
            PieceType = (chessBoard.PieceType.rook);
        }

        public override void capture()
        {
            Board.pieceCaptured(this);
        }

        public override bool IsValid(Vector3 moveToTest)
        {
            Vector3 tmp = moveToTest - Location;
            if (tmp == Vector3.zero)
            {
                return false;
            }
            else if (tmp.y == 0 && tmp.z == 0)
            {
                return true;
            }
            else if (tmp.x == 0 && tmp.z == 0)
            {
                return true;
            }
            else if (tmp.x == 0 && tmp.y == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override List<Vector3> validMoves()
        {
            throw new NotImplementedException();
        }
    }
}
