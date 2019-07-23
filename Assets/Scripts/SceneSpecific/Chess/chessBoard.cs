/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

 using System;
using System.Collections;
using System.Collections.Generic;
 using Unity.Mathematics;
 using UnityEngine;

namespace IMRE.Chess3D {

/// <summary>
/// The board that keeps track of pieces and moves for spatial chess.
/// This is central to the game.
/// </summary>
	public static class chessBoard : MonoBehaviour{

        public static int boardSize = 9;
		
	public static float3 boardOrigin;
	public static float boardDimensions;
		
        AbstractPiece[,,] board = new AbstractPiece[boardSize, boardSize, boardSize];
#pragma warning disable 0649
    public static List<AbstractPiece> whiteTeam;           //white
    public static List<AbstractPiece> blackTeam;           //black

        List<AbstractPiece> whiteGraveyard;
        List<AbstractPiece> blackGraveyard;
#pragma warning restore 0649

		public static List<AbstractPiece> myTeam(currentTeam team)
        {
            if (team == currentTeam.white)
            {
                return whiteTeam;
            }
            else {
                return blackTeam;
            }
        }

        public static List<AbstractPiece> otherTeam(currentTeam team)
        {
            if (team == currentTeam.black)
            {
                return whiteTeam;
            }
            else
            {
                return blackTeam;
            }
        }

        public enum currentTeam { white, black }
        public enum PieceType{ king, queen, rook, bishop, knight, pawn }

        /// <summary>
        /// returns the piece that was in the place before this move
        /// </summary>
        /// <param name="moveLocation">move that is being made</param>
        /// <returns>piece that was previously in location</returns>
        public static AbstractPiece TestLocation(int3 moveLocation)
        {
            if (board[(int)moveLocation.x,(int)moveLocation.y,(int)moveLocation.z] == null)
                return null;
            else
                return board[(int)moveLocation.x,(int)moveLocation.y,(int)moveLocation.z];
        }

        /// <summary>
        /// Tests if the move attempted puts the opponent in check
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="attemptedMove"></param>
        internal bool Check(AbstractPiece abstractPiece, int3 attemptedMove)
        {
		return abstractPiece.validMoves.Contains(attemptedMove);
	}
        public void pieceCaptured(AbstractPiece pieceDead)
        {
            pieceDead.IsCaptured = true;
            if (whiteTeam.Contains(pieceDead))
            {
                whiteTeam.Remove(pieceDead);
                whiteGraveyard.Add(pieceDead);
            }
            else
            {
                blackTeam.Remove(pieceDead);
                blackGraveyard.Add(pieceDead);
            }


        }
    }
}
