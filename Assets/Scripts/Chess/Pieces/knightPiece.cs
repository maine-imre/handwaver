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
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class knightPiece : AbstractPiece
    {
        public override void capture()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid(Vector3 attemptedMove)
        {
            Vector3 tmp = this.Location - attemptedMove;
            if (tmp == Vector3.zero)
            {
                //piece did not move
                return false;
            }
            else
            {
                List<Vector3> possible = validMoves();
                if (possible.Contains(this.Location))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override List<Vector3> validMoves()
        {
            return allValidMoves.kingMoves(Location, Board.myTeam(Team));
        }
    }
}
