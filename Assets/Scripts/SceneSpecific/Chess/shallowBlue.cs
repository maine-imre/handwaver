/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.Chess3D.AI
{
	/// <summary>
	/// An attempt at an AI for spatial chess.
	/// </summary>
	///public static class shallowBlue
	//{
	//    public class AIBoard
	//    {
	//        public List<piece> pieces;                                  //List of pieces on the board
	//        public List<AIBoard> children = new List<shallowBlue.AIBoard>();   //List of possible next states
	//        public int heuristicOutput;                                        //The output of the heuristic state of the board
	//        public void AIBoard(List<piece> Pieces)                     //Initialization function
	//        {
	//            pieces = Pieces;
	//        }
	//    }
	//    public class piece                                              //The piece used by the AI system
	//    {
	//        public string type;                                         //Type of the piece i.e. rook, king, etc.
	//        public ArrayList location;                                  //location of the piece
	//        public string team;                                         //The team that the piece is on
	//        public MonoBehaviour pieceReference;                        //The in-game reference to the piece

	//        public void piece(string Type, ArrayList Location, string Team, MonoBehaviour PieceReference)//initialization function
	//        {
	//            type = Type;
	//            location = Location;
	//            pieceReference = PieceReference;
	//            team = Team;
	//        }
	//        public List<ArrayList> validMoves(AIBoard board){return null;}//should return the valid moves for the piece
	//    }

	//    public static void generateChildren(AIBoard startBoard, string team)//function to generate all next states
	//    {
	//        int numberOfPieces = startBoard.pieces.Count;
	//        for (int i = 0; i < numberOfPieces; i++)                    //Loop through all of the pieces on the board
	//        {
	//            if (startBoard.peices[i].team.equals(team))             //if it's on the right team
	//            {                                                       //then create a new board state for it
	//                List<ArrayList> nextMoves = startBoard.peices[i].validMoves(startBoard);//get the valid moves for the peice
	//                for (int r = 0; r < nextMoves.Count; r++)           //STICKY KEYS WAS TURNED OFF AT THIS POINT
	//                {                                                   //generate the new board
	//                    AIBoard newBoard = new AIBoard();
	//                    for (int x = 0; x < numberOfPieces; x++)        //loop through all of the peices adding them to the new board
	//                    {
	//                        if (x == i)                                 //if this node is the one being changed, then use the new coordinates for the node
	//                        {
	//                            piece newPiece = new piece();
	//                            newPiece.type = startBoard.piece[i].type;
	//                            newPiece.location = nextMoves[r];
	//                            newPiece.team = startBoard.piece[i].team;
	//                            newPiece.pieceReference = startBoard.piece[i].pieceReference;
	//                            newBoard.pieces.Add(newPiece);
	//                            //CHECK HERE IF NEW PIECE REMOVES A OPPOSING PEACE
	//                        } else {
	//                            newBoard.pieces.Add(startBoard.pieces[x]);
	//                        }
	//                    }
	//                    newBoard.heuristicOutput = heuristicFunction(newBoard);
	//                    startBoard.children.Add(newBoard);
	//                    if (newBoard.heuristicOutput > startBoard.heuristicOutput)
	//                    {
	//                        startBoard.heuristicOutput = newBoard.heuristicOutput;
	//                    }
	//                }
	//            }
	//        }
	//    }

	//    public static int heuristicFunction(AIBoard board)
	//    {
	//        //heuristic function

	//    }
	//}
}