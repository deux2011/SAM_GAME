using System;

namespace SamGame.Data.Models
{
    /// <summary>
    /// 무장(장수) 데이터 모델
    /// </summary>
    [Serializable]
    public class OfficerData
    {
        public int Id;
        public string Name;        // 이름
        public string CourtesyName; // 자 (字)
        public int BirthYear;
        public int DeathYear;
        public Gender Gender;
        public int FactionId;       // 소속 세력 ID

        // 기본 능력치 (1~100)
        public int Leadership;      // 통솔
        public int Warfare;         // 무력
        public int Intelligence;    // 지력
        public int Politics;        // 정치
        public int Charisma;        // 매력

        // 병과 적성 (S/A/B/C)
        public UnitAptitude Infantry;    // 보병
        public UnitAptitude Cavalry;     // 기병
        public UnitAptitude Archer;      // 궁병
        public UnitAptitude Siege;       // 공성

        // 특기
        public string[] Skills;

        // 상태
        public OfficerState State;
        public int LocationProvinceId;
        public int Loyalty;         // 충성도 (0~100)
    }

    public enum Gender { Male, Female }

    public enum UnitAptitude { S, A, B, C }

    public enum OfficerState
    {
        Active,      // 활동 중
        Captured,    // 포로
        Free,        // 재야
        Dead         // 사망
    }
}
