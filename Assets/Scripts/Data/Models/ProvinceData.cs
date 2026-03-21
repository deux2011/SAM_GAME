using System;
using System.Collections.Generic;

namespace SamGame.Data.Models
{
    /// <summary>
    /// 지역(성/군) 데이터 모델
    /// </summary>
    [Serializable]
    public class ProvinceData
    {
        public int Id;
        public string Name;            // 지역명
        public int FactionId;          // 소속 세력 ID
        public int GovernorId;         // 태수 ID

        // 자원
        public int Gold;               // 금
        public int Food;               // 병량
        public int Population;         // 인구
        public int Commerce;           // 상업
        public int Agriculture;        // 농업
        public int Defense;            // 방어도 (성벽)

        // 헥스 그리드 위치 (오프셋 좌표)
        public int HexCol;
        public int HexRow;

        // 도시 규모
        public SamGame.Data.ProvinceSize Size;

        // 인접 지역
        public List<int> AdjacentProvinceIds;

        // 주둔 무장
        public List<int> StationedOfficerIds;
    }
}
