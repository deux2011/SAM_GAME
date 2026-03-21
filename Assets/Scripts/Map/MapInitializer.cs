using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SamGame.Data;
using SamGame.Data.Models;
using SamGame.Map.Hex;
using SamGame.Map.Rendering;
using SamGame.Map.Terrain;
using SamGame.Map.Territory;

namespace SamGame.Map
{
    /// <summary>
    /// 맵 초기화 오케스트레이터
    /// GameManager.StartNewGame()에서 호출
    /// </summary>
    public class MapInitializer : MonoBehaviour
    {
        [SerializeField] private HexGrid _hexGrid;
        [SerializeField] private TerritoryManager _territoryManager;
        [SerializeField] private TerrainDataProvider _terrainProvider;
        [SerializeField] private MapColorPalette _colorPalette;
        [SerializeField] private Material _hexMaterial;

        [Header("청크 설정")]
        [SerializeField] private int _chunkSize = 20;
        [SerializeField] private Transform _chunkParent;
        [SerializeField] private Transform _cityParent;
        [SerializeField] private GameObject _cityPrefab;

        private List<HexChunk> _chunks = new();

        public void InitializeMap()
        {
            // 1. 헥스 그리드 생성 (지형 적용)
            _hexGrid.Initialize((col, row) => _terrainProvider.GetTerrainAt(col, row));

            // 2. 도시 데이터 로드 및 배치
            var provinces = ProvinceDataProvider.GetAllProvinces();
            var citySources = new List<(HexCoordinates pos, int cityId)>();

            foreach (var province in provinces)
            {
                var coords = HexCoordinates.FromOffsetCoordinates(province.HexCol, province.HexRow);
                var cell = _hexGrid.GetCell(coords);
                if (cell == null) continue;

                cell.IsCity = true;
                cell.CityId = province.Id;
                cell.Terrain = TerrainType.City;

                citySources.Add((coords, province.Id));
            }

            // 3. 영향권 계산 (다중 소스 다익스트라)
            InfluenceCalculator.CalculateInfluence(_hexGrid, citySources);

            // 4. 초기 세력 배치 → 영토 칠하기
            var cityFactionMap = GetInitialFactionAssignment(provinces);
            _territoryManager.InitializeTerritory(cityFactionMap);

            // 5. 청크 메시 생성
            GenerateChunks();

            // 6. 도시 마커 배치
            SpawnCityMarkers(provinces);

            Debug.Log($"Map initialized: {provinces.Count} cities, {_chunks.Count} chunks");
        }

        private void GenerateChunks()
        {
            var allCells = _hexGrid.AllCells.ToArray();

            // 청크 단위로 셀 분류
            var chunkMap = new Dictionary<(int, int), List<HexCell>>();

            foreach (var cell in allCells)
            {
                var worldPos = cell.Coordinates.ToWorldPosition(_hexGrid.HexSize);
                int cx = Mathf.FloorToInt(worldPos.x / (_chunkSize * _hexGrid.HexSize * 1.5f));
                int cy = Mathf.FloorToInt(worldPos.y / (_chunkSize * _hexGrid.HexSize * Mathf.Sqrt(3f)));
                var key = (cx, cy);

                if (!chunkMap.ContainsKey(key))
                    chunkMap[key] = new List<HexCell>();
                chunkMap[key].Add(cell);
            }

            foreach (var (key, cells) in chunkMap)
            {
                var chunkGO = new GameObject($"Chunk_{key.Item1}_{key.Item2}");
                chunkGO.transform.SetParent(_chunkParent != null ? _chunkParent : transform);
                chunkGO.AddComponent<MeshFilter>();
                chunkGO.AddComponent<MeshRenderer>();

                var chunk = chunkGO.AddComponent<HexChunk>();
                chunk.Initialize(cells.ToArray(), _hexGrid.HexSize, _colorPalette, _hexMaterial);
                _chunks.Add(chunk);
            }
        }

        private void SpawnCityMarkers(List<ProvinceData> provinces)
        {
            foreach (var province in provinces)
            {
                var coords = HexCoordinates.FromOffsetCoordinates(province.HexCol, province.HexRow);
                var worldPos = coords.ToWorldPosition(_hexGrid.HexSize);

                if (_cityPrefab != null)
                {
                    var city = Instantiate(_cityPrefab, worldPos, Quaternion.identity, _cityParent);
                    var view = city.GetComponent<ProvinceView>();
                    if (view != null)
                    {
                        view.Initialize(province.Id, province.Name, _colorPalette.GetFactionColor(province.FactionId));
                    }
                }
            }
        }

        /// <summary>
        /// 190년 시나리오 초기 세력 배치
        /// </summary>
        private Dictionary<int, int> GetInitialFactionAssignment(List<ProvinceData> provinces)
        {
            var map = new Dictionary<int, int>();
            foreach (var p in provinces)
            {
                // 기본 중립 (세력 배치는 시나리오 데이터에서 로드)
                map[p.Id] = p.FactionId;
            }
            return map;
        }

        /// <summary>
        /// 영토 변경 시 해당 청크 리프레시
        /// </summary>
        public void RefreshAllChunks()
        {
            foreach (var chunk in _chunks)
                chunk.SetDirty();
        }
    }
}
