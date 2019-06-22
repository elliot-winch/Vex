using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Tiles
{
    public class Tile : MonoBehaviour, INavigableTile
    {
        public float moveCost = 1f;

        [HideInInspector]
        public HexTileMap map;
        public Vector2Int axialCoordinate;

        private LockDown passable = new LockDown();

        public bool IsUnlocked => passable.IsUnlocked;

        public float MoveIntoCost => moveCost;

        public Vector3 Position => HexTileMap.AxialToCube(axialCoordinate);

        public IEnumerable<INavigableTile> Neighbours { get; set; }
    }
}
