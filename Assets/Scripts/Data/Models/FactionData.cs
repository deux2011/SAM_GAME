using System;
using System.Collections.Generic;
using UnityEngine;

namespace SamGame.Data.Models
{
    /// <summary>
    /// 세력 데이터 모델
    /// </summary>
    [Serializable]
    public class FactionData
    {
        public int Id;
        public string Name;            // 세력명
        public int LeaderId;           // 군주 ID
        public Color FactionColor;     // 세력 색상 (지도 표시용)

        // 소유 지역
        public List<int> OwnedProvinceIds;

        // 외교 관계
        public List<DiplomacyRelation> Relations;

        // 정책
        public FactionPolicy CurrentPolicy;
    }

    [Serializable]
    public class DiplomacyRelation
    {
        public int TargetFactionId;
        public RelationType Type;
        public int Friendliness;       // 우호도 (-100 ~ 100)
        public int TurnsRemaining;     // 동맹/휴전 남은 턴
    }

    public enum RelationType
    {
        Neutral,     // 중립
        Alliance,    // 동맹
        War,         // 교전
        Ceasefire,   // 휴전
        Vassal       // 종속
    }

    public enum FactionPolicy
    {
        Expansion,   // 영토 확장
        Development, // 내정 개발
        Military,    // 군사 강화
        Diplomacy    // 외교 중시
    }
}
