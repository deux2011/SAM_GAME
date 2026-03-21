using SamGame.Data.Models;

namespace SamGame.Map.Hex
{
    /// <summary>
    /// 개별 헥스 셀 데이터 (MonoBehaviour X - 30,000개 이상 생성되므로 순수 클래스)
    /// </summary>
    public class HexCell
    {
        public HexCoordinates Coordinates { get; }
        public TerrainType Terrain { get; set; }
        public int OwnerFactionId { get; set; } = -1;       // 소유 세력 (-1 = 중립)
        public int InfluenceCityId { get; set; } = -1;       // 영향권 도시 ID
        public bool IsCity { get; set; }
        public int CityId { get; set; } = -1;

        public float MovementCost { get; private set; }

        public HexCell(HexCoordinates coordinates, TerrainType terrain)
        {
            Coordinates = coordinates;
            Terrain = terrain;
            CacheMovementCost();
        }

        public void CacheMovementCost()
        {
            MovementCost = TerrainHelper.GetMovementCost(Terrain, UnitType.Infantry);
        }

        public float GetMovementCost(UnitType unitType)
        {
            return TerrainHelper.GetMovementCost(Terrain, unitType);
        }

        public float GetDefenseBonus()
        {
            return TerrainHelper.GetDefenseBonus(Terrain);
        }
    }
}
