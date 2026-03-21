using System.Collections.Generic;
using SamGame.Map.Hex;

namespace SamGame.Map.Pathfinding
{
    /// <summary>
    /// A* 경로 탐색 결과
    /// </summary>
    public class PathResult
    {
        public List<HexCoordinates> Path { get; set; } = new();
        public float TotalCost { get; set; }
        public bool Found { get; set; }

        /// <summary>
        /// 이동에 필요한 예상 턴 수
        /// </summary>
        public int EstimatedTurns(float movementPerTurn)
        {
            if (!Found || movementPerTurn <= 0) return -1;
            return UnityEngine.Mathf.CeilToInt(TotalCost / movementPerTurn);
        }
    }
}
