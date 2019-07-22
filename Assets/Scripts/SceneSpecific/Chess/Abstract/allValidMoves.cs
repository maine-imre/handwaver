/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

namespace IMRE.Chess3D
{
    /// <summary>
    ///     A list of all of  the valid moves for each piece type in spatial chess.
    ///     This needs to be checked, only works in one direction.
    ///     Maybe we can express these in terms of a basis system?
    /// </summary>
    public static class allValidMoves
    {
        public static System.Collections.Generic.List<UnityEngine.Vector3> kingMoves(UnityEngine.Vector3 position,
            System.Collections.Generic.List<AbstractPiece> friendlyPieces)
        {
            System.Collections.Generic.List<UnityEngine.Vector3> posList =
                new System.Collections.Generic.List<UnityEngine.Vector3>();
            posList.Add(position + UnityEngine.Vector3.up);
            posList.Add(position + UnityEngine.Vector3.down);
            posList.Add(position + UnityEngine.Vector3.forward);
            posList.Add(position + UnityEngine.Vector3.back);
            posList.Add(position + UnityEngine.Vector3.right);
            posList.Add(position + UnityEngine.Vector3.left);

            posList.Add(position + UnityEngine.Vector3.up + UnityEngine.Vector3.left);
            posList.Add(position + UnityEngine.Vector3.up + UnityEngine.Vector3.right);
            posList.Add(position + UnityEngine.Vector3.up + UnityEngine.Vector3.back);
            posList.Add(position + UnityEngine.Vector3.up + UnityEngine.Vector3.forward);

            posList.Add(position + UnityEngine.Vector3.down + UnityEngine.Vector3.left);
            posList.Add(position + UnityEngine.Vector3.down + UnityEngine.Vector3.right);
            posList.Add(position + UnityEngine.Vector3.down + UnityEngine.Vector3.back);
            posList.Add(position + UnityEngine.Vector3.down + UnityEngine.Vector3.forward);

            posList.Add(position + UnityEngine.Vector3.left + UnityEngine.Vector3.back);
            posList.Add(position + UnityEngine.Vector3.left + UnityEngine.Vector3.forward);

            posList.Add(position + UnityEngine.Vector3.right + UnityEngine.Vector3.back);
            posList.Add(position + UnityEngine.Vector3.right + UnityEngine.Vector3.forward);

            posList.Add(position + UnityEngine.Vector3.up + UnityEngine.Vector3.left + UnityEngine.Vector3.forward);
            posList.Add(position + UnityEngine.Vector3.up + UnityEngine.Vector3.left + UnityEngine.Vector3.back);
            posList.Add(position + UnityEngine.Vector3.up + UnityEngine.Vector3.right + UnityEngine.Vector3.forward);
            posList.Add(position + UnityEngine.Vector3.up + UnityEngine.Vector3.right + UnityEngine.Vector3.back);

            posList.Add(position + UnityEngine.Vector3.down + UnityEngine.Vector3.left + UnityEngine.Vector3.forward);
            posList.Add(position + UnityEngine.Vector3.down + UnityEngine.Vector3.left + UnityEngine.Vector3.back);
            posList.Add(position + UnityEngine.Vector3.down + UnityEngine.Vector3.right + UnityEngine.Vector3.forward);
            posList.Add(position + UnityEngine.Vector3.down + UnityEngine.Vector3.right + UnityEngine.Vector3.back);

            System.Collections.Generic.List<UnityEngine.Vector3> posListClone = posList;

            foreach (UnityEngine.Vector3 pos in posListClone)
            {
                foreach (AbstractPiece piece in friendlyPieces)
                {
                    if (piece.Location == pos) posList.Remove(pos);
                }
            }

            return posList;
        }

        public static System.Collections.Generic.List<UnityEngine.Vector3> queenMoves(UnityEngine.Vector3 position,
            System.Collections.Generic.List<AbstractPiece> enemyPieces,
            System.Collections.Generic.List<AbstractPiece> friendlyPieces)
        {
            System.Collections.Generic.List<UnityEngine.Vector3> posList =
                new System.Collections.Generic.List<UnityEngine.Vector3>();
            pathMoves(UnityEngine.Vector3.up, position, enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.down, position, enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.forward, position, enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.back, position, enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.right, position, enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.left, position, enemyPieces, friendlyPieces, posList);

            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.left, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.right, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.left, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.right, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(UnityEngine.Vector3.left + UnityEngine.Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.left + UnityEngine.Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(UnityEngine.Vector3.right + UnityEngine.Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.right + UnityEngine.Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.left + UnityEngine.Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.left + UnityEngine.Vector3.back, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.right + UnityEngine.Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.right + UnityEngine.Vector3.back, position,
                enemyPieces, friendlyPieces, posList);

            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.left + UnityEngine.Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.left + UnityEngine.Vector3.back, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.right + UnityEngine.Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.right + UnityEngine.Vector3.back, position,
                enemyPieces, friendlyPieces, posList);

            System.Collections.Generic.List<UnityEngine.Vector3> posListClone = posList;

            foreach (UnityEngine.Vector3 pos in posListClone)
            {
                foreach (AbstractPiece piece in friendlyPieces)
                {
                    if (piece.Location == pos) posList.Remove(pos);
                }
            }

            return posList;
        }

        private static void pathMoves(UnityEngine.Vector3 direction, UnityEngine.Vector3 position,
            System.Collections.Generic.List<AbstractPiece> enemyPieces,
            System.Collections.Generic.List<AbstractPiece> friendlyPieces,
            System.Collections.Generic.List<UnityEngine.Vector3> posList)
        {
            UnityEngine.Vector3 newPos = UnityEngine.Vector3.zero;
            for (int i = 0; i < 9; i++)
            {
                newPos = position + i * direction;
                if (!(newPos.x > 9 || newPos.y > 9 || newPos.z > 9 || newPos.x < 0 || newPos.y < 0 || newPos.z < 0))
                {
                    bool result = false;
                    foreach (AbstractPiece enemy in enemyPieces)
                    {
                        if (enemy.Location == newPos) result = true;
                    }

                    if (result)
                    {
                        posList.Add(newPos);
                        break;
                    }

                    foreach (AbstractPiece friendly in friendlyPieces)
                    {
                        if (friendly.Location == newPos) result = true;
                    }

                    if (result) break;
                }
            }
        }

        public static System.Collections.Generic.List<UnityEngine.Vector3> knightMoves(UnityEngine.Vector3 position,
            System.Collections.Generic.List<AbstractPiece> friendlyPieces)
        {
            System.Collections.Generic.List<UnityEngine.Vector3> posList =
                new System.Collections.Generic.List<UnityEngine.Vector3>();
            posList.Add(position + 2 * UnityEngine.Vector3.right + UnityEngine.Vector3.up);
            posList.Add(position + 2 * UnityEngine.Vector3.right + UnityEngine.Vector3.down);
            posList.Add(position + 2 * UnityEngine.Vector3.right + UnityEngine.Vector3.forward);
            posList.Add(position + 2 * UnityEngine.Vector3.right + UnityEngine.Vector3.back);
            posList.Add(position + 2 * UnityEngine.Vector3.left + UnityEngine.Vector3.up);
            posList.Add(position + 2 * UnityEngine.Vector3.left + UnityEngine.Vector3.down);
            posList.Add(position + 2 * UnityEngine.Vector3.left + UnityEngine.Vector3.forward);
            posList.Add(position + 2 * UnityEngine.Vector3.left + UnityEngine.Vector3.back);

            posList.Add(position + 2 * UnityEngine.Vector3.up + UnityEngine.Vector3.right);
            posList.Add(position + 2 * UnityEngine.Vector3.up + UnityEngine.Vector3.left);
            posList.Add(position + 2 * UnityEngine.Vector3.up + UnityEngine.Vector3.forward);
            posList.Add(position + 2 * UnityEngine.Vector3.up + UnityEngine.Vector3.back);
            posList.Add(position + 2 * UnityEngine.Vector3.down + UnityEngine.Vector3.right);
            posList.Add(position + 2 * UnityEngine.Vector3.down + UnityEngine.Vector3.left);
            posList.Add(position + 2 * UnityEngine.Vector3.down + UnityEngine.Vector3.forward);
            posList.Add(position + 2 * UnityEngine.Vector3.down + UnityEngine.Vector3.back);

            posList.Add(position + 2 * UnityEngine.Vector3.forward + UnityEngine.Vector3.up);
            posList.Add(position + 2 * UnityEngine.Vector3.forward + UnityEngine.Vector3.down);
            posList.Add(position + 2 * UnityEngine.Vector3.forward + UnityEngine.Vector3.left);
            posList.Add(position + 2 * UnityEngine.Vector3.forward + UnityEngine.Vector3.right);
            posList.Add(position + 2 * UnityEngine.Vector3.back + UnityEngine.Vector3.up);
            posList.Add(position + 2 * UnityEngine.Vector3.back + UnityEngine.Vector3.down);
            posList.Add(position + 2 * UnityEngine.Vector3.back + UnityEngine.Vector3.left);
            posList.Add(position + 2 * UnityEngine.Vector3.back + UnityEngine.Vector3.right);

            System.Collections.Generic.List<UnityEngine.Vector3> posListClone = posList;

            foreach (UnityEngine.Vector3 pos in posListClone)
            {
                foreach (AbstractPiece piece in friendlyPieces)
                {
                    if (piece.Location == pos) posList.Remove(pos);
                }
            }

            return posList;
        }

        public static System.Collections.Generic.List<UnityEngine.Vector3> pawnMoves(UnityEngine.Vector3 position,
            System.Collections.Generic.List<AbstractPiece> enemyPieces,
            System.Collections.Generic.List<AbstractPiece> friendlyPieces, chessBoard.currentTeam team)
        {
            if (team == chessBoard.currentTeam.white)
            {
                System.Collections.Generic.List<UnityEngine.Vector3> posList =
                    new System.Collections.Generic.List<UnityEngine.Vector3>();

                bool blocked = false;

                foreach (AbstractPiece piece in enemyPieces)
                {
                    if (piece.Location == position + UnityEngine.Vector3.forward)
                        blocked = true;
                }

                foreach (AbstractPiece piece in friendlyPieces)
                {
                    if (piece.Location == position + UnityEngine.Vector3.forward)
                        blocked = true;
                }

                if (!blocked)
                {
                    posList.Add(UnityEngine.Vector3.forward + position);

                    if (position.z == 1 || position.z == 8) posList.Add(2 * UnityEngine.Vector3.forward + position);
                }
                else
                {
                    foreach (AbstractPiece piece in enemyPieces)
                    {
                        if (piece.Location == UnityEngine.Vector3.forward + UnityEngine.Vector3.up + position)
                            posList.Add(UnityEngine.Vector3.forward + UnityEngine.Vector3.up + position);
                        else if (piece.Location == UnityEngine.Vector3.forward + UnityEngine.Vector3.down + position)
                            posList.Add(UnityEngine.Vector3.forward + UnityEngine.Vector3.down + position);
                        else if (piece.Location == UnityEngine.Vector3.forward + UnityEngine.Vector3.right + position)
                            posList.Add(UnityEngine.Vector3.forward + UnityEngine.Vector3.right + position);
                        else if (piece.Location == UnityEngine.Vector3.forward + UnityEngine.Vector3.left + position)
                            posList.Add(UnityEngine.Vector3.forward + UnityEngine.Vector3.left + position);
                    }
                }

                return posList;
            }

            if (team == chessBoard.currentTeam.black)
            {
                System.Collections.Generic.List<UnityEngine.Vector3> posList =
                    new System.Collections.Generic.List<UnityEngine.Vector3>();

                bool blocked = false;

                foreach (AbstractPiece piece in enemyPieces)
                {
                    if (piece.Location == position + UnityEngine.Vector3.back)
                        blocked = true;
                }

                foreach (AbstractPiece piece in friendlyPieces)
                {
                    if (piece.Location == position + UnityEngine.Vector3.back)
                        blocked = true;
                }

                if (!blocked)
                {
                    posList.Add(UnityEngine.Vector3.back + position);

                    if (position.z == 1 || position.z == 8) posList.Add(2 * UnityEngine.Vector3.back + position);
                }
                else
                {
                    foreach (AbstractPiece piece in enemyPieces)
                    {
                        if (piece.Location == UnityEngine.Vector3.back + UnityEngine.Vector3.up + position)
                            posList.Add(UnityEngine.Vector3.back + UnityEngine.Vector3.up + position);
                        else if (piece.Location == UnityEngine.Vector3.back + UnityEngine.Vector3.down + position)
                            posList.Add(UnityEngine.Vector3.back + UnityEngine.Vector3.down + position);
                        else if (piece.Location == UnityEngine.Vector3.back + UnityEngine.Vector3.right + position)
                            posList.Add(UnityEngine.Vector3.back + UnityEngine.Vector3.right + position);
                        else if (piece.Location == UnityEngine.Vector3.back + UnityEngine.Vector3.left + position)
                            posList.Add(UnityEngine.Vector3.back + UnityEngine.Vector3.left + position);
                    }
                }

                return posList;
            }

            return null;
        }

        public static System.Collections.Generic.List<UnityEngine.Vector3> bishopMoves(UnityEngine.Vector3 position,
            System.Collections.Generic.List<AbstractPiece> enemyPieces,
            System.Collections.Generic.List<AbstractPiece> friendlyPieces)
        {
            System.Collections.Generic.List<UnityEngine.Vector3> posList =
                new System.Collections.Generic.List<UnityEngine.Vector3>();

            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.left, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.right, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.left, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.right, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(UnityEngine.Vector3.left + UnityEngine.Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.left + UnityEngine.Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(UnityEngine.Vector3.right + UnityEngine.Vector3.back, position, enemyPieces, friendlyPieces,
                posList);
            pathMoves(UnityEngine.Vector3.right + UnityEngine.Vector3.forward, position, enemyPieces, friendlyPieces,
                posList);

            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.left + UnityEngine.Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.left + UnityEngine.Vector3.back, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.right + UnityEngine.Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.up + UnityEngine.Vector3.right + UnityEngine.Vector3.back, position,
                enemyPieces, friendlyPieces, posList);

            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.left + UnityEngine.Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.left + UnityEngine.Vector3.back, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.right + UnityEngine.Vector3.forward, position,
                enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.down + UnityEngine.Vector3.right + UnityEngine.Vector3.back, position,
                enemyPieces, friendlyPieces, posList);

            System.Collections.Generic.List<UnityEngine.Vector3> posListClone = posList;

            foreach (UnityEngine.Vector3 pos in posListClone)
            {
                foreach (AbstractPiece piece in friendlyPieces)
                {
                    if (piece.Location == pos) posList.Remove(pos);
                }
            }

            return posList;
        }

        public static System.Collections.Generic.List<UnityEngine.Vector3> rookMoves(UnityEngine.Vector3 position,
            System.Collections.Generic.List<AbstractPiece> enemyPieces,
            System.Collections.Generic.List<AbstractPiece> friendlyPieces)
        {
            System.Collections.Generic.List<UnityEngine.Vector3> posList =
                new System.Collections.Generic.List<UnityEngine.Vector3>();
            pathMoves(UnityEngine.Vector3.up, position, enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.down, position, enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.forward, position, enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.back, position, enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.right, position, enemyPieces, friendlyPieces, posList);
            pathMoves(UnityEngine.Vector3.left, position, enemyPieces, friendlyPieces, posList);

            System.Collections.Generic.List<UnityEngine.Vector3> posListClone = posList;

            foreach (UnityEngine.Vector3 pos in posListClone)
            {
                foreach (AbstractPiece piece in friendlyPieces)
                {
                    if (piece.Location == pos) posList.Remove(pos);
                }
            }

            return posList;
        }
    }
}