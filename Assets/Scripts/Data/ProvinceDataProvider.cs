using System.Collections.Generic;
using SamGame.Data.Models;

namespace SamGame.Data
{
    /// <summary>
    /// 삼국지 46개 도시 초기 데이터
    /// 좌표는 200x150 오프셋 그리드 기준 (중국 지도 매핑)
    /// </summary>
    public static class ProvinceDataProvider
    {
        public static List<ProvinceData> GetAllProvinces()
        {
            return new List<ProvinceData>
            {
                // ===== 북부 (화북/하북) =====
                Create(1, "계", 120, 20, ProvinceSize.Standard),           // 유주 (베이징 부근)
                Create(2, "남피", 115, 35, ProvinceSize.Minor),            // 발해
                Create(3, "업", 105, 45, ProvinceSize.Major),              // 위군 (조조 본거지)
                Create(4, "평원", 120, 42, ProvinceSize.Minor),
                Create(5, "북해", 135, 45, ProvinceSize.Standard),         // 공융
                Create(6, "진류", 108, 55, ProvinceSize.Standard),
                Create(7, "복양", 115, 52, ProvinceSize.Standard),         // 동군

                // ===== 서북부 (관중/량주) =====
                Create(8, "장안", 70, 58, ProvinceSize.Major),             // 사례
                Create(9, "천수", 52, 60, ProvinceSize.Standard),          // 량주
                Create(10, "안정", 60, 50, ProvinceSize.Minor),
                Create(11, "무위", 40, 45, ProvinceSize.Minor),            // 하서회랑
                Create(12, "서량", 30, 42, ProvinceSize.Minor),

                // ===== 중원 =====
                Create(13, "낙양", 90, 60, ProvinceSize.Major),            // 수도
                Create(14, "허창", 105, 65, ProvinceSize.Major),           // 조조 수도
                Create(15, "여남", 112, 72, ProvinceSize.Standard),
                Create(16, "남양", 95, 72, ProvinceSize.Standard),         // 완성
                Create(17, "진", 110, 58, ProvinceSize.Standard),

                // ===== 산동 =====
                Create(18, "임치", 138, 48, ProvinceSize.Standard),        // 제남
                Create(19, "낭야", 140, 55, ProvinceSize.Standard),
                Create(20, "서주", 130, 60, ProvinceSize.Standard),         // 도겸
                Create(21, "하비", 128, 65, ProvinceSize.Standard),         // 서주 남부
                Create(22, "소패", 118, 62, ProvinceSize.Standard),         // 패국

                // ===== 형주 (중부) =====
                Create(23, "양양", 92, 80, ProvinceSize.Major),            // 형주 핵심
                Create(24, "강릉", 85, 88, ProvinceSize.Standard),         // 남군
                Create(25, "강하", 105, 82, ProvinceSize.Standard),
                Create(26, "장사", 100, 98, ProvinceSize.Standard),
                Create(27, "무릉", 82, 98, ProvinceSize.Minor),
                Create(28, "계양", 108, 105, ProvinceSize.Minor),
                Create(29, "영릉", 95, 105, ProvinceSize.Minor),

                // ===== 회남/양주 북부 =====
                Create(30, "수춘", 122, 72, ProvinceSize.Standard),        // 원술
                Create(31, "합비", 125, 78, ProvinceSize.Standard),        // 합비 전투
                Create(32, "여강", 118, 80, ProvinceSize.Minor),

                // ===== 강동 (오나라) =====
                Create(33, "건업", 132, 82, ProvinceSize.Major),           // 손권 수도 (난징)
                Create(34, "오", 142, 85, ProvinceSize.Standard),          // 소주
                Create(35, "회계", 148, 90, ProvinceSize.Standard),
                Create(36, "시상", 120, 88, ProvinceSize.Standard),        // 파양호
                Create(37, "노강", 128, 92, ProvinceSize.Minor),

                // ===== 익주 (촉나라) =====
                Create(38, "한중", 65, 72, ProvinceSize.Major),            // 장로 → 유비
                Create(39, "성도", 52, 88, ProvinceSize.Major),            // 유비 수도
                Create(40, "면죽", 55, 82, ProvinceSize.Standard),
                Create(41, "강주", 65, 95, ProvinceSize.Standard),         // 파군
                Create(42, "영안", 75, 90, ProvinceSize.Standard),         // 백제성
                Create(43, "건녕", 50, 108, ProvinceSize.Minor),           // 남만
                Create(44, "운남", 40, 115, ProvinceSize.Minor),

                // ===== 교주 (남부) =====
                Create(45, "번우", 115, 115, ProvinceSize.Standard),       // 광주
                Create(46, "교지", 90, 125, ProvinceSize.Minor),           // 베트남 북부
            };
        }

        private static ProvinceData Create(int id, string name, int col, int row, ProvinceSize size)
        {
            return new ProvinceData
            {
                Id = id,
                Name = name,
                HexCol = col,
                HexRow = row,
                Size = size,
                Population = size switch
                {
                    ProvinceSize.Major => 80000,
                    ProvinceSize.Standard => 50000,
                    ProvinceSize.Minor => 25000,
                    _ => 30000
                },
                Commerce = size == ProvinceSize.Major ? 500 : 300,
                Agriculture = size == ProvinceSize.Major ? 500 : 300,
                Defense = size == ProvinceSize.Major ? 300 : 150,
                Gold = 5000,
                Food = 10000,
                AdjacentProvinceIds = new System.Collections.Generic.List<int>()
            };
        }
    }

    public enum ProvinceSize
    {
        Major,      // 대도시 (낙양, 장안, 성도 등)
        Standard,   // 일반 성
        Minor       // 소 성
    }
}
