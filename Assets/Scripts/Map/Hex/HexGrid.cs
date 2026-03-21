using System.Collections.Generic;
using UnityEngine;
using SamGame.Data.Models;

namespace SamGame.Map.Hex
{
    /// <summary>
    /// 헥스 그리드 - 전체 맵 데이터 구조
    /// </summary>
    public class HexGrid : MonoBehaviour
    {
        [SerializeField] private int _width = 200;
        [SerializeField] private int _height = 150;
        [SerializeField] private float _hexSize = 0.5f;

        public int Width => _width;
        public int Height => _height;
        public float HexSize => _hexSize;

        private Dictionary<HexCoordinates, HexCell> _cells = new();

        public void Initialize(System.Func<int, int, TerrainType> terrainProvider)
        {
            _cells.Clear();

            for (int col = 0; col < _width; col++)
            {
                for (int row = 0; row < _height; row++)
                {
                    var coords = HexCoordinates.FromOffsetCoordinates(col, row);
                    var terrain = terrainProvider(col, row);
                    var cell = new HexCell(coords, terrain);
                    _cells[coords] = cell;
                }
            }

            Debug.Log($"HexGrid initialized: {_width}x{_height} = {_cells.Count} cells");
        }

        public HexCell GetCell(HexCoordinates coords)
        {
            return _cells.GetValueOrDefault(coords);
        }

        public HexCell GetCellAtWorldPosition(Vector3 worldPos)
        {
            var coords = HexCoordinates.FromWorldPosition(worldPos, _hexSize);
            return GetCell(coords);
        }

        public List<HexCell> GetNeighbors(HexCoordinates coords)
        {
            var result = new List<HexCell>(6);
            foreach (var neighborCoords in coords.GetNeighbors())
            {
                var cell = GetCell(neighborCoords);
                if (cell != null)
                    result.Add(cell);
            }
            return result;
        }

        /// <summary>
        /// 범위 내 모든 헥스 반환 (BFS)
        /// </summary>
        public List<HexCell> GetCellsInRange(HexCoordinates center, int range)
        {
            var result = new List<HexCell>();
            for (int q = -range; q <= range; q++)
            {
                int r1 = Mathf.Max(-range, -q - range);
                int r2 = Mathf.Min(range, -q + range);
                for (int r = r1; r <= r2; r++)
                {
                    var cell = GetCell(new HexCoordinates(center.Q + q, center.R + r));
                    if (cell != null)
                        result.Add(cell);
                }
            }
            return result;
        }

        public IEnumerable<HexCell> AllCells => _cells.Values;

        public bool IsValid(HexCoordinates coords) => _cells.ContainsKey(coords);
    }
}
