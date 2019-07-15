using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    X,
    Y,
    Z
}

public partial class HexTileMap : MonoBehaviour
{
    public enum MapShape
    {
        Sqaure,
        Hexagon
    }

    //Inspector
    [SerializeField] Tile tilePrefab;
    public Vector3Int mapDimensions;
    public float tileSize;
    public MapShape mapShape;

    //Private variables
    private Dictionary<Vector2Int, Tile> tiles; //stored in axial co-ordinates

    private List<Tile> allTiles;
    public List<Tile> AllTiles
    {
        get
        {
            return tiles.Values.ToList();
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
        var key = tiles
            .Keys
            .Where(a => a.x == axialX && a.y == axialY)
            .ToList();

        if(key.Count > 0)
        {
            return tiles[key.First()];
        }

        return null;
    }

    #region Creation
    public void Create()
    {
        tiles = new Dictionary<Vector2Int, Tile>();

        switch (mapShape)
        {
            case MapShape.Sqaure:
                for (int x = 0; x < mapDimensions.x; x++)
                {
                    for (int y = 0; y < mapDimensions.y; y++)
                    {
                        tiles[new Vector2Int(x, y)] = CreateTile(x, y);
                    }
                }
                break;

            case MapShape.Hexagon:
                GetAxialCoordinatesAround(Vector2Int.zero, mapDimensions).ForEach(coord =>
                {
                    tiles[coord] = CreateTile(coord.x, coord.y);
                });
                break;
        }


        //Link Neighbours
        //Create tiles
        foreach(Tile t in AllTiles)
        {
            var cubeCoord = AxialToCube(t.axialCoordinate);

            var n = StepAll(cubeCoord, 1)
            .Select(nCoord => TileAt(nCoord))
            .NonNull()
            .ToList();

            t.Neighbours = n;
        }
    }

    private Tile CreateTile(int x, int y)
    {
        //TODO: load form source

        Tile t = Instantiate(tilePrefab);

        t.map = this;
        t.axialCoordinate = new Vector2Int(x, y);

        t.transform.position = AxialToWorld(t.axialCoordinate);
        t.transform.SetParent(this.transform);    

        return t;
    }

    #endregion

    #region Navigation
    public AStarPath GetPath(Tile start, Tile end)
    {
        return new AStarPath(AllTiles.Cast<INavigableTile>().ToList(), start, end);
    }

    public List<Vector3Int> StepAll(Vector3Int from, int by)
    {
        return new List<Vector3Int>()
            {
                Step(from,  by, Direction.X),
                Step(from, -by, Direction.X),
                Step(from,  by, Direction.Y),
                Step(from, -by, Direction.Y),
                Step(from,  by, Direction.Z),
                Step(from, -by, Direction.Z),
            };
    }

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

    public static int Distance(Tile a, Tile b)
    {
        var ca = AxialToCube(a.axialCoordinate);
        var cb = AxialToCube(b.axialCoordinate);

        return Math.Max(Math.Max(Math.Abs(ca.x - cb.x), Math.Abs(ca.y - cb.y)), Math.Abs(ca.z - cb.z));
    }

    public List<Vector2Int> GetAxialCoordinatesAround(Vector2Int origin, int distance)
    {
        return GetAxialCoordinatesAround(origin, new Vector3Int(distance, distance, distance));
    }

    public List<Vector2Int> GetAxialCoordinatesAround(Vector2Int origin, Vector3Int dimensions)
    {
        var coords = new List<Vector2Int>();

        for (int x = -dimensions.x / 2; x <= dimensions.x / 2; x++)
        {
            for (int y = -dimensions.y / 2; y <= dimensions.y / 2; y++)
            {
                var cc = AxialToCube(x, y);

                if (cc.z <= dimensions.z / 2 && cc.z >= -dimensions.z / 2)
                {
                    coords.Add(new Vector2Int(x, y));
                }
            }
        }

        return coords;
    }
    #endregion

    public List<Tile> FloodFill(Tile origin, int distance, bool includeUnlocked)
    {
        if(distance <= 0)
        {
            return new List<Tile>();
        }

        var visited = new List<Tile>() { origin };
        var prevFront = new Tile[] { origin };
        var newFront = new List<Tile>();

        for(int i = 1; i <= distance; i++)
        {
            foreach(var t in prevFront)
            {
                var n = t.Neighbours
                    .Where(x =>
                    {
                        return visited.Contains(x) == false && (x.IsUnlocked || includeUnlocked);
                    })
                    .Cast<Tile>()
                    .ToList();

                n.ForEach(x =>
                {
                    visited.Add(x);

                    if(x.IsUnlocked)
                    {
                        newFront.Add(x);
                    }
                });
            }

            prevFront = new Tile[newFront.Count];
            newFront.CopyTo(prevFront);
        }

        return visited;
    }

    //Given a tile set, find the tiles on the frontier of the group
    public List<Tile> BorderTiles(List<Tile> tiles)
    {
        var border = new List<Tile>();

        foreach(Tile t in tiles)
        {
            if(t.Neighbours.Any(x => tiles.Contains(x) == false))
            {
                border.Add(t);
            }
        }

        return border;
    }

    public List<Tile> Line(Tile a, Tile b)
    {
        if(a == b)
        {
            return new List<Tile>() { a };
        }

        float dist = Distance(a, b);
        Vector3Int ca = AxialToCube(a.axialCoordinate);
        Vector3Int cb = AxialToCube(b.axialCoordinate);

        List<Tile> line = new List<Tile>();

        for(int i = 0; i <= dist; i++)
        {
            //Sampling the map by lerping between the two points
            line.Add(tiles[CubeToAxial(FloatingToCube(new Vector3()
            {
                x = Mathf.Lerp(ca.x, cb.x, i / dist),
                y = Mathf.Lerp(ca.y, cb.y, i / dist),
                z = Mathf.Lerp(ca.z, cb.z, i / dist),
            }))]);
        }

        return line;
    }

    public bool InLineOfSight(Tile a, Tile b)
    {
        var line = Game.Current.Map.Line(a, b);

        if (line == null)
        {
            return false;
        }

        if(line.Count <= 1)
        {
            return true;
        }

        //Ignore that the current tile and end tile are occupied -> pieces we want to throw to are there
        line.RemoveAt(0);
        line.RemoveAt(line.Count - 1);

        return line.Count <= 0 || line.All(l => l?.IsUnlocked == true);
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

    public Vector3 AxialToWorld(Vector2Int axial)
    {
        float sqrtThree = Mathf.Sqrt(3);
        float x = tileSize * (sqrtThree * axial.x + sqrtThree / 2 * axial.y);
        float y = tileSize * (3f / 2 * axial.y);

        return new Vector3(x, 0f, y);
    }

    //Used when sampling the hex map returns a floating point co-ordinate to get a tile
    private Vector3Int FloatingToCube(Vector3 cube)
    {
        int rx = (int)Mathf.Round(cube.x);
        int ry = (int)Mathf.Round(cube.y);
        int rz = (int)Mathf.Round(cube.z);

        float x_diff = Mathf.Abs(rx - cube.x);
        float y_diff = Mathf.Abs(ry - cube.y);
        float z_diff = Mathf.Abs(rz - cube.z);

        if(x_diff > y_diff && x_diff > z_diff)
        {
            rx = -ry - rz;
        }
        else if(y_diff > z_diff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }

        return new Vector3Int(rx, ry, rz);
    }
    #endregion
}

