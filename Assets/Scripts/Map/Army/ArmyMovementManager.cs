using System.Collections.Generic;
using UnityEngine;
using SamGame.Core;
using SamGame.Data.Models;
using SamGame.Map.Hex;
using SamGame.Map.Pathfinding;
using SamGame.Map.Territory;

namespace SamGame.Map.Army
{
    /// <summary>
    /// 군대 헥스 이동 + 영토 칠하기
    /// 군대가 지나간 헥스를 소속 세력 색으로 칠함 (삼국지 14 핵심 메커니즘)
    /// </summary>
    public class ArmyMovementManager : MonoBehaviour
    {
        [SerializeField] private HexGrid _hexGrid;
        [SerializeField] private TerritoryManager _territoryManager;

        /// <summary>
        /// 군대에 이동 명령
        /// </summary>
        public bool OrderMove(ArmyData army, HexCoordinates destination)
        {
            var result = HexPathfinder.FindPath(_hexGrid, army.CurrentHex, destination, army.Type);
            if (!result.Found)
            {
                Debug.Log("경로를 찾을 수 없습니다.");
                return false;
            }

            army.PlannedPath = result.Path;
            army.PathIndex = 0;
            army.RemainingMovementPoints = army.MovementPointsPerTurn;
            return true;
        }

        /// <summary>
        /// 턴 진행 시 군대 이동 처리
        /// </summary>
        public void ProcessTurnMovement(List<ArmyData> armies)
        {
            foreach (var army in armies)
            {
                if (army.PlannedPath == null || army.PathIndex >= army.PlannedPath.Count - 1)
                    continue;

                army.RemainingMovementPoints = army.MovementPointsPerTurn;
                MoveAlongPath(army);
            }
        }

        private void MoveAlongPath(ArmyData army)
        {
            while (army.PathIndex < army.PlannedPath.Count - 1)
            {
                var nextHex = army.PlannedPath[army.PathIndex + 1];
                var nextCell = _hexGrid.GetCell(nextHex);
                if (nextCell == null) break;

                float cost = nextCell.GetMovementCost(army.Type);
                if (cost >= float.MaxValue || army.RemainingMovementPoints < cost)
                    break;

                army.RemainingMovementPoints -= cost;
                army.PathIndex++;
                army.CurrentHex = nextHex;

                // 지나간 헥스를 세력 색으로 칠하기
                _territoryManager.PaintHex(nextHex, army.FactionId);

                // 도착 여부
                if (army.PathIndex >= army.PlannedPath.Count - 1)
                {
                    army.PlannedPath = null;
                    EventManager.Publish(GameEvents.ArmyArrived, army.Id);
                    break;
                }
            }

            EventManager.Publish(GameEvents.ArmyMoved, army.Id);
        }

        /// <summary>
        /// 이동 예상 턴 수
        /// </summary>
        public int GetEstimatedTurns(ArmyData army, HexCoordinates destination)
        {
            var result = HexPathfinder.FindPath(_hexGrid, army.CurrentHex, destination, army.Type);
            return result.EstimatedTurns(army.MovementPointsPerTurn);
        }
    }
}
