/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System.Collections.Generic;
using UnityEngine;

namespace IMRE.Chess3D
{
    /// <summary>
    ///     A list of all of  the valid moves for each piece type in spatial chess.
    ///     This needs to be checked, only works in one direction.
    ///     Maybe we can express these in terms of a basis system?
    /// </summary>
    public static class allValidMoves
    {
        public static List<Vector3> kingMoves(Vector3 position,
            List<AbstractPiece> friendlyPieces)
        {
            var posList =
                new List<Vector3>();
            posList.Add(position + Vector3.up);
            posList.Add(position + Vector3.down);
            posList.Add(position + Vector3.forward);
            posList.Add(position + Vector3.back);
            posList.Add(position + Vector3.right);
            posList.Add(position + Vector3.left);

            posList.Add(position + Vector3.up + Vector3.left);
            posList.Add(position + Vector3.up + Vector3.right);
            posList.Add(position + Vector3.up + Vector3.back);
            posList.Add(position + Vector3.up + Vector3.forward);

            posList.Add(position + Vector3.down + Vector3.left);
            posList.Add(position + Vector3.down + Vector3.right);
            posList.Add(position + Vector3.down + Vector3.back);
            posList.Add(position + Vector3.down + Vector3.forward);

            posList.Add(position + Vector3.left + Vector3.back);
            posList.Add(position + Vector3.left + Vector3.forward);

            posList.Add(position + Vector3.right + Vector3.back);
            posList.Add(position + Vector3.right + Vector3.forward);

            posList.Add(position + Vector3.up + Vector3.left + Vector3.forward);
            posList.Add(position + Vector3.up + Vector3.left + Vector3.back);
            posList.Add(position + Vector3.up + Vector3.right + Vector3.forward);
            posList.Add(position + Vector3.up + Vector3.right + Vector3.back);

            posList.Add(position + Vector3.down + Vector3.left + Vector3.forward);
            posList.Add(position + Vector3.down + Vector3.left + Vector3.back);
            posList.Add(position + Vector3.down + Vector3.right + Vector3.forward);
            posList.Add(position + Vector3.down + Vector3.right + Vector3.back);

            var posListClone = posList;

            foreach (var pos in posListClone)
            foreach (var piece in friendlyPieces)
                if (piece.Location == pos)
                    posList.Remove(pos);

            return posList;
        }

        public static List<Vector3> queenMoves(Vector3 position,
            List<AbstractPiece> enemyPieces,
            List<AbstractPiece> friendlyPieces)
        {
            var posList =
                new List<Vector3>();
            pathMoves(Vector3.up, position, enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.down, position, enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.forward, position, enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.back, position, enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.right, position, enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.left, position, enemyPieces, friendlyPieces, posList);

            pathMoves(Vector3.up + Vector3.left, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.up + Vector3.right, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.up + Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.up + Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(Vector3.down + Vector3.left, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.down + Vector3.right, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.down + Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.down + Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(Vector3.left + Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.left + Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(Vector3.right + Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.right + Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(Vector3.up + Vector3.left + Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.up + Vector3.left + Vector3.back, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.up + Vector3.right + Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.up + Vector3.right + Vector3.back, position,
                enemyPieces, friendlyPieces, posList);

            pathMoves(Vector3.down + Vector3.left + Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.down + Vector3.left + Vector3.back, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.down + Vector3.right + Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.down + Vector3.right + Vector3.back, position,
                enemyPieces, friendlyPieces, posList);

            var posListClone = posList;

            foreach (var pos in posListClone)
            foreach (var piece in friendlyPieces)
                if (piece.Location == pos)
                    posList.Remove(pos);

            return posList;
        }

        private static void pathMoves(Vector3 direction, Vector3 position,
            List<AbstractPiece> enemyPieces,
            List<AbstractPiece> friendlyPieces,
            List<Vector3> posList)
        {
            var newPos = Vector3.zero;
            for (var i = 0; i < 9; i++)
            {
                newPos = position + i * direction;
                if (!(newPos.x > 9 || newPos.y > 9 || newPos.z > 9 || newPos.x < 0 || newPos.y < 0 ||
                      newPos.z < 0))
                {
                    var result = false;
                    foreach (var enemy in enemyPieces)
                        if (enemy.Location == newPos)
                            result = true;

                    if (result)
                    {
                        posList.Add(newPos);
                        break;
                    }

                    foreach (var friendly in friendlyPieces)
                        if (friendly.Location == newPos)
                            result = true;

                    if (result) break;
                }
            }
        }

        public static List<Vector3> knightMoves(Vector3 position,
            List<AbstractPiece> friendlyPieces)
        {
            var posList =
                new List<Vector3>();
            posList.Add(position + 2 * Vector3.right + Vector3.up);
            posList.Add(position + 2 * Vector3.right + Vector3.down);
            posList.Add(position + 2 * Vector3.right + Vector3.forward);
            posList.Add(position + 2 * Vector3.right + Vector3.back);
            posList.Add(position + 2 * Vector3.left + Vector3.up);
            posList.Add(position + 2 * Vector3.left + Vector3.down);
            posList.Add(position + 2 * Vector3.left + Vector3.forward);
            posList.Add(position + 2 * Vector3.left + Vector3.back);

            posList.Add(position + 2 * Vector3.up + Vector3.right);
            posList.Add(position + 2 * Vector3.up + Vector3.left);
            posList.Add(position + 2 * Vector3.up + Vector3.forward);
            posList.Add(position + 2 * Vector3.up + Vector3.back);
            posList.Add(position + 2 * Vector3.down + Vector3.right);
            posList.Add(position + 2 * Vector3.down + Vector3.left);
            posList.Add(position + 2 * Vector3.down + Vector3.forward);
            posList.Add(position + 2 * Vector3.down + Vector3.back);

            posList.Add(position + 2 * Vector3.forward + Vector3.up);
            posList.Add(position + 2 * Vector3.forward + Vector3.down);
            posList.Add(position + 2 * Vector3.forward + Vector3.left);
            posList.Add(position + 2 * Vector3.forward + Vector3.right);
            posList.Add(position + 2 * Vector3.back + Vector3.up);
            posList.Add(position + 2 * Vector3.back + Vector3.down);
            posList.Add(position + 2 * Vector3.back + Vector3.left);
            posList.Add(position + 2 * Vector3.back + Vector3.right);

            var posListClone = posList;

            foreach (var pos in posListClone)
            foreach (var piece in friendlyPieces)
                if (piece.Location == pos)
                    posList.Remove(pos);

            return posList;
        }

        public static List<Vector3> pawnMoves(Vector3 position,
            List<AbstractPiece> enemyPieces,
            List<AbstractPiece> friendlyPieces, chessBoard.currentTeam team)
        {
            if (team == chessBoard.currentTeam.white)
            {
                var posList =
                    new List<Vector3>();

                var blocked = false;

                foreach (var piece in enemyPieces)
                    if (piece.Location == position + Vector3.forward)
                        blocked = true;

                foreach (var piece in friendlyPieces)
                    if (piece.Location == position + Vector3.forward)
                        blocked = true;

                if (!blocked)
                {
                    posList.Add(Vector3.forward + position);

                    if (position.z == 1 || position.z == 8)
                        posList.Add(2 * Vector3.forward + position);
                }
                else
                {
                    foreach (var piece in enemyPieces)
                        if (piece.Location == Vector3.forward + Vector3.up + position)
                            posList.Add(Vector3.forward + Vector3.up + position);
                        else if (piece.Location == Vector3.forward + Vector3.down + position)
                            posList.Add(Vector3.forward + Vector3.down + position);
                        else if (piece.Location == Vector3.forward + Vector3.right + position)
                            posList.Add(Vector3.forward + Vector3.right + position);
                        else if (piece.Location == Vector3.forward + Vector3.left + position)
                            posList.Add(Vector3.forward + Vector3.left + position);
                }

                return posList;
            }

            if (team == chessBoard.currentTeam.black)
            {
                var posList =
                    new List<Vector3>();

                var blocked = false;

                foreach (var piece in enemyPieces)
                    if (piece.Location == position + Vector3.back)
                        blocked = true;

                foreach (var piece in friendlyPieces)
                    if (piece.Location == position + Vector3.back)
                        blocked = true;

                if (!blocked)
                {
                    posList.Add(Vector3.back + position);

                    if (position.z == 1 || position.z == 8) posList.Add(2 * Vector3.back + position);
                }
                else
                {
                    foreach (var piece in enemyPieces)
                        if (piece.Location == Vector3.back + Vector3.up + position)
                            posList.Add(Vector3.back + Vector3.up + position);
                        else if (piece.Location == Vector3.back + Vector3.down + position)
                            posList.Add(Vector3.back + Vector3.down + position);
                        else if (piece.Location == Vector3.back + Vector3.right + position)
                            posList.Add(Vector3.back + Vector3.right + position);
                        else if (piece.Location == Vector3.back + Vector3.left + position)
                            posList.Add(Vector3.back + Vector3.left + position);
                }

                return posList;
            }

            return null;
        }

        public static List<Vector3> bishopMoves(Vector3 position,
            List<AbstractPiece> enemyPieces,
            List<AbstractPiece> friendlyPieces)
        {
            var posList =
                new List<Vector3>();

            pathMoves(Vector3.up + Vector3.left, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.up + Vector3.right, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.up + Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.up + Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(Vector3.down + Vector3.left, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.down + Vector3.right, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.down + Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.down + Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(Vector3.left + Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.left + Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(Vector3.right + Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(Vector3.right + Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(Vector3.up + Vector3.left + Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.up + Vector3.left + Vector3.back, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.up + Vector3.right + Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.up + Vector3.right + Vector3.back, position,
                enemyPieces, friendlyPieces, posList);

            pathMoves(Vector3.down + Vector3.left + Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.down + Vector3.left + Vector3.back, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.down + Vector3.right + Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.down + Vector3.right + Vector3.back, position,
                enemyPieces, friendlyPieces, posList);

            var posListClone = posList;

            foreach (var pos in posListClone)
            foreach (var piece in friendlyPieces)
                if (piece.Location == pos)
                    posList.Remove(pos);

            return posList;
        }

        public static List<Vector3> rookMoves(Vector3 position,
            List<AbstractPiece> enemyPieces,
            List<AbstractPiece> friendlyPieces)
        {
            var posList =
                new List<Vector3>();
            pathMoves(Vector3.up, position, enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.down, position, enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.forward, position, enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.back, position, enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.right, position, enemyPieces, friendlyPieces, posList);
            pathMoves(Vector3.left, position, enemyPieces, friendlyPieces, posList);

            var posListClone = posList;

            foreach (var pos in posListClone)
            foreach (var piece in friendlyPieces)
                if (piece.Location == pos)
                    posList.Remove(pos);

            return posList;
        }
    }
}