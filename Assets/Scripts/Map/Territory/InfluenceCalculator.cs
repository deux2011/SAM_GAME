using System.Collections.Generic;
using SamGame.Map.Hex;
using SamGame.Map.Pathfinding;

namespace SamGame.Map.Territory
{
    /// <summary>
    /// 다중 소스 다익스트라로 각 헥스의 영향권 도시 계산
    /// 지형 비용을 반영하여 자연스러운 보로노이 분할 생성
    /// (산/강이 자연 국경 역할)
    /// </summary>
    public static class InfluenceCalculator
    {
        public static void CalculateInfluence(HexGrid grid, List<(HexCoordinates pos, int cityId)> citySources)
        {
            var dist = new Dictionary<HexCoordinates, float>();
            var openSet = new PriorityQueue<HexCoordinates, float>();

            // 모든 도시를 시작점으로 (거리 0)
            foreach (var (pos, cityId) in citySources)
            {
                var cell = grid.GetCell(pos);
                if (cell == null) continue;

                cell.InfluenceCityId = cityId;
                dist[pos] = 0;
                openSet.Enqueue(pos, 0);
            }

            // 다익스트라 확장
            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();
                var currentCell = grid.GetCell(current);
                if (currentCell == null) continue;

                float currentDist = dist[current];

                foreach (var neighbor in grid.GetNeighbors(current))
                {
                    float moveCost = neighbor.MovementCost;
                    if (moveCost >= float.MaxValue)
                        continue;

                    float newDist = currentDist + moveCost;
                    var neighborCoords = neighbor.Coordinates;

                    if (!dist.ContainsKey(neighborCoords) || newDist < dist[neighborCoords])
                    {
                        dist[neighborCoords] = newDist;
                        neighbor.InfluenceCityId = currentCell.InfluenceCityId;
                        openSet.Enqueue(neighborCoords, newDist);
                    }
                }
            }
        }
    }
}
