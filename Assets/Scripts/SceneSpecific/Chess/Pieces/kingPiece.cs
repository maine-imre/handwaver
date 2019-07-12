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
/// The behaviour controls for the king piece in spatial chess.
/// </summary>
	public class kingPiece : AbstractPiece
    {
        private void Start()
        {
            Board = GameObject.Find("chessBoard").GetComponent<chessBoard>();
            PieceType = (chessBoard.PieceType.king);
        }

        public override void capture()
        {
            Debug.Log("Game Ends! "+Team+" lost the game.");
        }

        public override bool IsValid(Vector3 moveToTest)
        {
            Vector3 tmp = moveToTest - Location;
            if (tmp == Vector3.zero)
            {
                return false;
            }else if(Board.TestLocation(moveToTest) != null){
                if (Board.TestLocation(moveToTest).Team == this.Team)
                {return false;}
            }

            if (Mathf.Abs(Vector3.Dot(tmp,Vector3.right)) > 1)
            {
                return false;
            } else if (Mathf.Abs(Vector3.Dot(tmp, Vector3.up))>1)
            {
                return false;
            }else if (Mathf.Abs(Vector3.Dot(tmp, Vector3.forward))> 1){
                return false;
            }
            else {
                if(Board.placeSelfInCheck(this, moveToTest))
                {
                    return false;
                }
                return true;
            }
            //check if in check
        }

        public override List<Vector3> validMoves()
        {
            return allValidMoves.kingMoves(Location, Board.myTeam(Team));
        }
    }
}