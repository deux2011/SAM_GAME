using System.Collections.Generic;
using UnityEngine;
using SamGame.Core;
using SamGame.Map.Hex;

namespace SamGame.Map.Territory
{
    /// <summary>
    /// 영토 관리 - 삼국지 14의 핵심: 세력 색으로 헥스 칠하기
    /// </summary>
    public class TerritoryManager : MonoBehaviour
    {
        [SerializeField] private HexGrid _hexGrid;

        // 세력별 소유 헥스 추적
        private Dictionary<int, HashSet<HexCoordinates>> _factionTerritories = new();
        // 도시별 영향권 헥스
        private Dictionary<int, List<HexCoordinates>> _cityInfluenceMap = new();

        /// <summary>
        /// 게임 시작 시 초기 영토 설정
        /// 각 도시의 세력에 따라 영향권 헥스를 세력 색으로 칠함
        /// </summary>
        public void InitializeTerritory(Dictionary<int, int> cityFactionMap)
        {
            _factionTerritories.Clear();

            foreach (var cell in _hexGrid.AllCells)
            {
                int cityId = cell.InfluenceCityId;
                if (cityId < 0) continue;

                // 도시-영향권 매핑 구축
                if (!_cityInfluenceMap.ContainsKey(cityId))
                    _cityInfluenceMap[cityId] = new List<HexCoordinates>();
                _cityInfluenceMap[cityId].Add(cell.Coordinates);

                // 도시 소속 세력으로 헥스 칠하기
                if (cityFactionMap.TryGetValue(cityId, out int factionId))
                {
                    cell.OwnerFactionId = factionId;
                    AddToFactionTerritory(factionId, cell.Coordinates);
                }
            }
        }

        /// <summary>
        /// 헥스 하나를 세력 색으로 칠하기 (군대 이동 시)
        /// </summary>
        public void PaintHex(HexCoordinates hex, int factionId)
        {
            var cell = _hexGrid.GetCell(hex);
            if (cell == null) return;

            int previousOwner = cell.OwnerFactionId;
            if (previousOwner == factionId) return;

            // 이전 세력에서 제거
            if (previousOwner >= 0)
                RemoveFromFactionTerritory(previousOwner, hex);

            // 새 세력에 추가
            cell.OwnerFactionId = factionId;
            AddToFactionTerritory(factionId, hex);

            EventManager.Publish(GameEvents.TerritoryChanged, hex);
        }

        /// <summary>
        /// 경로 전체를 세력 색으로 칠하기
        /// </summary>
        public void PaintPath(List<HexCoordinates> path, int factionId)
        {
            foreach (var hex in path)
                PaintHex(hex, factionId);
        }

        /// <summary>
        /// 도시 점령 시 - 해당 도시 영향권 전체가 새 세력으로 전환
        /// </summary>
        public void CaptureCity(int cityId, int newFactionId)
        {
            if (!_cityInfluenceMap.TryGetValue(cityId, out var hexes))
                return;

            foreach (var hex in hexes)
                PaintHex(hex, newFactionId);

            EventManager.Publish(GameEvents.CityCaptured, cityId);
        }

        public int GetFactionTerritoryCount(int factionId)
        {
            return _factionTerritories.TryGetValue(factionId, out var set) ? set.Count : 0;
        }

        public HashSet<HexCoordinates> GetFactionTerritory(int factionId)
        {
            return _factionTerritories.GetValueOrDefault(factionId);
        }

        private void AddToFactionTerritory(int factionId, HexCoordinates hex)
        {
            if (!_factionTerritories.ContainsKey(factionId))
                _factionTerritories[factionId] = new HashSet<HexCoordinates>();
            _factionTerritories[factionId].Add(hex);
        }

        private void RemoveFromFactionTerritory(int factionId, HexCoordinates hex)
        {
            if (_factionTerritories.TryGetValue(factionId, out var set))
                set.Remove(hex);
        }
    }
}
