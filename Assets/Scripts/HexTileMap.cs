using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Tiles
{
    public enum Direction
    {
        X,
        Y,
        Z
    }

    public class HexTileMap : MonoBehaviour
    {

        //Inspector
        public Tile tilePrefab;
        public Vector2Int mapDimensions;
        public float tileSize;

        //Private variables
        private Tile[,] tiles; //stored in axial co-ordinates

        private List<Tile> allTiles;
        public List<Tile> AllTiles
        {
            get
            {
                if (allTiles == null)
                {
                    allTiles = new List<Tile>();

                    int width = tiles.GetLength(0);
                    int height = tiles.GetLength(1);

                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            allTiles.Add(tiles[i, j]);
                        }
                    }
                }

                return allTiles;
            }
        }

        public Tile TileAt(Vector3Int cubeCoord)
        {
            return TileAt(CubeToAxial(cubeCoord));
        }

        public Tile TileAt(Vector2Int axialCoord)
        {
            return TileAt(axialCoord.x, axialCoord.y);
        }

        public Tile TileAt(int axialX, int axialY)
        {
            if (axialX < 0 || axialX >= mapDimensions.x || axialY < 0 || axialY >= mapDimensions.y)
            {
                return null;
            }

            return tiles[axialX, axialY];
        }

        public AStarPath GetPath(Tile start, Tile end)
        {
            return new AStarPath(AllTiles.Cast<INavigableTile>().ToList(), start, end);
        }

        #region Creation
        public void Create()
        {
            tiles = new Tile[mapDimensions.x, mapDimensions.y];

            //Create tiles
            for (int x = 0; x < mapDimensions.x; x++)
            {
                for (int y = 0; y < mapDimensions.y; y++)
                {
                    tiles[x, y] = CreateTile(x, y);
                }
            }

            //Link Neighbours
            //Create tiles
            for (int x = 0; x < mapDimensions.x; x++)
            {
                for (int y = 0; y < mapDimensions.y; y++)
                {
                    var t = tiles[x, y];
                    var cubeCoord = AxialToCube(x, y);

                    var n = new List<Vector3Int>()
                    {
                        Step(cubeCoord,  1, Direction.X),
                        Step(cubeCoord, -1, Direction.X),
                        Step(cubeCoord,  1, Direction.Y),
                        Step(cubeCoord, -1, Direction.Y),
                        Step(cubeCoord,  1, Direction.Z),
                        Step(cubeCoord, -1, Direction.Z),
                    }
                    .Select(nCoord => TileAt(nCoord))
                    .NonNull()
                    .ToList();

                    t.Neighbours = n;
                }
            }
        }

        private Tile CreateTile(int x, int y)
        {
            //TODO: load form source

            Tile t = Instantiate(tilePrefab);

            t.map = this;
            t.axialCoordinate = new Vector2Int(x, y);
            
            return t;
        }
        #endregion

        public Vector3Int Step(Vector3Int from, int by, Direction towards)
        {
            switch (towards)
            {
                case Direction.X:
                    return new Vector3Int(from.x, from.y + by, from.z - by);
                case Direction.Y:
                    return new Vector3Int(from.x + by, from.y, from.z - by);
                case Direction.Z:
                    return new Vector3Int(from.x + by, from.y - by, from.z);
                default:
                    return from;
            }
        }

        public Vector2 PositionToWorld(Vector3Int position)
        {
            Vector2Int axial = CubeToAxial(position);

            float sqrtThree = Mathf.Sqrt(3);
            float x = tileSize * (sqrtThree * axial.x + sqrtThree/2 * axial.y);
            float y = tileSize * (                      3f / 2      * axial.y);

            return new Vector2(x, y);
        }

        #region Conversions
        public static Vector2Int CubeToAxial(Vector3Int cube)
        {
            return new Vector2Int(cube.x, cube.y);
        }

        public static Vector3Int AxialToCube(Vector2Int axial)
        {
            return AxialToCube(axial.x, axial.y);
        }

        public static Vector3Int AxialToCube(int x, int y)
        {
            return new Vector3Int(x, y, -x-y);
        }
        #endregion
    }
}
