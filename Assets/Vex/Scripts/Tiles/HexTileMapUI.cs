using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public partial class HexTileMap : MonoBehaviour
{
    [SerializeField] LineRenderer separatingLinePrefab;
    [SerializeField] LineRenderer pathLinePrefab;

    public LineRenderer DrawLineSeparating(Tile a, Tile b, LineRenderer existing = null)
    {
        var lr = existing ?? Instantiate(separatingLinePrefab);

        var midpoint = (a.modelTransform.position + b.modelTransform.position) / 2;
        lr.transform.position = midpoint + new Vector3(0f, 0.2f, 0f);

        float s = Vector3.Distance(a.modelTransform.position, b.transform.position) * Mathf.Tan(Mathf.PI / 6f) / 2f;
        Vector3 dir = b.modelTransform.position - a.modelTransform.position;
        Vector3 perpDir = new Vector3(dir.z, 0f, -dir.x).normalized;

        lr.positionCount = 2;
        lr.SetPosition(0,  perpDir * s);
        lr.SetPosition(1, -perpDir * s);

        return lr;
    }

    public List<LineRenderer> IndividualOutline(IEnumerable<Tile> tiles, LineRenderer prefab = null)
    {
        return tiles.SelectMany(x => Outline(x, prefab)).ToList();
    }

    public List<LineRenderer> Outline(Tile tile, LineRenderer prefab = null)
    {
        return DrawBorder(new List<Tile>() { tile }, prefab);
    }

    //TODO: all as one line renderer
    //Challege: ordering points
    public List<LineRenderer> DrawBorder(List<Tile> tiles, LineRenderer prefab = null)
    {
        var lrs = new List<LineRenderer>();

        foreach (Tile t in tiles)
        {
            foreach(var n in t.NeighbouringTiles)
            {
                if(tiles.Contains(n) == false)
                {
                    lrs.Add(DrawLineSeparating(t, n, prefab != null ? Instantiate(prefab) : null));
                }
            }
        }

        return lrs;
    }

    public LineRenderer DrawPath(Tile start, Tile target, LineRenderer existing = null)
    {
        var path = Game.Current.Map.GetPath(start, target)?.ValidPath;

        return DrawPath(path, existing);
    }

    public LineRenderer DrawPath(List<INavigableTile> path, LineRenderer existing = null)
    {
        var pathLine = existing ?? Instantiate(pathLinePrefab);

        if (path != null)
        {
            var tiles = path.ToList();

            pathLine.enabled = true;
            pathLine.positionCount = path.Count;
            pathLine.SetPositions(tiles
                .Cast<Tile>()
                .Select(x => x.modelTransform.position)
                .ToArray());
        }

        return pathLine;
    }

}
