using System.Collections.Generic;
using UnityEngine;
using SamGame.Map.Hex;
using SamGame.Data.Models;

namespace SamGame.Map.Pathfinding
{
    /// <summary>
    /// 헥스 그리드 A* 경로 탐색
    /// </summary>
    public static class HexPathfinder
    {
        public static PathResult FindPath(HexGrid grid, HexCoordinates start, HexCoordinates goal, UnitType unitType)
        {
            var result = new PathResult();

            var startCell = grid.GetCell(start);
            var goalCell = grid.GetCell(goal);
            if (startCell == null || goalCell == null)
                return result;

            var openSet = new PriorityQueue<HexCoordinates, float>();
            var cameFrom = new Dictionary<HexCoordinates, HexCoordinates>();
            var gScore = new Dictionary<HexCoordinates, float>();

            gScore[start] = 0;
            openSet.Enqueue(start, 0);

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                if (current == goal)
                {
                    result.Found = true;
                    result.TotalCost = gScore[goal];
                    result.Path = ReconstructPath(cameFrom, current);
                    return result;
                }

                foreach (var neighbor in grid.GetNeighbors(current))
                {
                    float moveCost = neighbor.GetMovementCost(unitType);
                    if (moveCost >= float.MaxValue)
                        continue; // 통행 불가

                    float tentativeG = gScore[current] + moveCost;
                    var neighborCoords = neighbor.Coordinates;

                    if (!gScore.ContainsKey(neighborCoords) || tentativeG < gScore[neighborCoords])
                    {
                        gScore[neighborCoords] = tentativeG;
                        cameFrom[neighborCoords] = current;

                        float h = HexCoordinates.Distance(neighborCoords, goal) * 1.0f; // 최소 이동비용 휴리스틱
                        openSet.Enqueue(neighborCoords, tentativeG + h);
                    }
                }
            }

            return result; // 경로 없음
        }

        private static List<HexCoordinates> ReconstructPath(
            Dictionary<HexCoordinates, HexCoordinates> cameFrom, HexCoordinates current)
        {
            var path = new List<HexCoordinates> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            path.Reverse();
            return path;
        }
    }
}
