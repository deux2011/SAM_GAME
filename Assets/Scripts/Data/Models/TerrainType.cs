namespace SamGame.Data.Models
{
    public enum TerrainType
    {
        Plains,      // 평지
        Forest,      // 숲
        Mountain,    // 산
        River,       // 강
        Swamp,       // 습지
        Desert,      // 사막
        Sea,         // 바다
        City,        // 성
        Pass         // 관문 (함곡관, 동관 등)
    }

    /// <summary>
    /// 지형별 이동 비용, 방어 보너스 계산
    /// </summary>
    public static class TerrainHelper
    {
        /// <summary>
        /// 병종별 지형 이동 비용 (float.MaxValue = 통행 불가)
        /// </summary>
        public static float GetMovementCost(TerrainType terrain, UnitType unitType)
        {
            return terrain switch
            {
                TerrainType.Plains => 1.0f,
                TerrainType.City => 1.0f,
                TerrainType.Forest => unitType == UnitType.Cavalry ? 2.0f : 1.5f,
                TerrainType.Mountain => unitType == UnitType.Cavalry ? float.MaxValue : 3.0f,
                TerrainType.River => unitType == UnitType.Navy ? 1.0f : 2.0f,
                TerrainType.Swamp => unitType == UnitType.Cavalry ? 3.0f : 2.0f,
                TerrainType.Desert => 2.0f,
                TerrainType.Pass => 1.5f,
                TerrainType.Sea => unitType == UnitType.Navy ? 1.0f : float.MaxValue,
                _ => 1.0f
            };
        }

        /// <summary>
        /// 지형별 방어 보너스 배율
        /// </summary>
        public static float GetDefenseBonus(TerrainType terrain)
        {
            return terrain switch
            {
                TerrainType.Mountain => 1.5f,
                TerrainType.Forest => 1.2f,
                TerrainType.Pass => 1.8f,
                TerrainType.City => 2.0f,
                TerrainType.River => 1.3f,
                TerrainType.Swamp => 1.1f,
                _ => 1.0f
            };
        }
    }
}
