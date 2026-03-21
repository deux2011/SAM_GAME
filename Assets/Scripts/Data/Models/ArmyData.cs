using System;
using System.Collections.Generic;
using SamGame.Map.Hex;

namespace SamGame.Data.Models
{
    /// <summary>
    /// 군단(부대) 데이터 모델
    /// </summary>
    [Serializable]
    public class ArmyData
    {
        public int Id;
        public int CommanderId;        // 지휘관 ID
        public int FactionId;          // 소속 세력
        public int SoldierCount;       // 병력 수
        public UnitType Type;          // 병종
        public float Morale;           // 사기 (0~100)

        // 헥스 기반 이동
        public HexCoordinates CurrentHex;
        public List<HexCoordinates> PlannedPath;
        public int PathIndex;
        public float MovementPointsPerTurn;    // 병종별: 기병=8, 보병=5, 공성=3
        public float RemainingMovementPoints;

        // 거점 소속
        public int CurrentProvinceId;

        // 전투 관련
        public float AttackPower;
        public float DefensePower;
    }

    public enum UnitType
    {
        Infantry,    // 보병
        Cavalry,     // 기병
        Archer,      // 궁병
        Siege,       // 공성병
        Navy         // 수군
    }
}
