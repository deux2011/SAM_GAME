using UnityEngine;
using SamGame.Data.Models;

namespace SamGame.Map.Terrain
{
    /// <summary>
    /// 지형 데이터 제공 - 텍스처 기반
    /// 200x150 텍스처의 픽셀 색상 → 지형 타입 변환
    /// 텍스처가 없으면 절차적 생성 (강/산맥 배치)
    /// </summary>
    public class TerrainDataProvider : MonoBehaviour
    {
        [SerializeField] private Texture2D _terrainMap;

        // 텍스처 색상 → 지형 매핑
        private static readonly Color32 ColorPlains = new(144, 238, 144, 255);    // 연두
        private static readonly Color32 ColorForest = new(34, 139, 34, 255);      // 짙은 녹색
        private static readonly Color32 ColorMountain = new(139, 119, 101, 255);  // 갈색
        private static readonly Color32 ColorRiver = new(65, 105, 225, 255);      // 파랑
        private static readonly Color32 ColorSea = new(0, 0, 139, 255);           // 진파랑
        private static readonly Color32 ColorDesert = new(238, 221, 130, 255);    // 모래색
        private static readonly Color32 ColorSwamp = new(85, 107, 47, 255);       // 올리브
        private static readonly Color32 ColorPass = new(169, 169, 169, 255);      // 회색

        public TerrainType GetTerrainAt(int col, int row)
        {
            if (_terrainMap != null)
                return GetTerrainFromTexture(col, row);

            return GenerateProceduralTerrain(col, row);
        }

        private TerrainType GetTerrainFromTexture(int col, int row)
        {
            if (col >= _terrainMap.width || row >= _terrainMap.height)
                return TerrainType.Sea;

            Color32 pixel = _terrainMap.GetPixel(col, _terrainMap.height - 1 - row);
            return MatchColor(pixel);
        }

        /// <summary>
        /// 절차적 지형 생성 (텍스처 없을 때)
        /// 중국 지리를 대략적으로 반영
        /// </summary>
        private TerrainType GenerateProceduralTerrain(int col, int row)
        {
            // 맵 외곽은 바다
            if (col <= 2 || row <= 2 || col >= 197 || row >= 147)
                return TerrainType.Sea;

            // 동쪽 해안선 (대략적)
            if (col > 150 && row > 60)
                return TerrainType.Sea;
            if (col > 160 && row > 40)
                return TerrainType.Sea;

            // 남쪽 해안선
            if (row > 130 && col > 80)
                return TerrainType.Sea;

            // 황하 (黃河) - 대략 row 45~55, col 40~140
            if (row >= 45 && row <= 47 && col >= 40 && col <= 140)
                return TerrainType.River;

            // 장강 (長江) - 대략 row 85~87, col 50~145
            if (row >= 85 && row <= 87 && col >= 50 && col <= 145)
                return TerrainType.River;

            // 진령산맥 (秦嶺) - col 55~95, row 65~68
            if (row >= 65 && row <= 68 && col >= 55 && col <= 95)
                return TerrainType.Mountain;

            // 태항산맥 (太行山脈) - col 95~100, row 30~60
            if (col >= 95 && col <= 98 && row >= 30 && row <= 60)
                return TerrainType.Mountain;

            // 촉 지역 산악 (사천분지 주변)
            if (col >= 40 && col <= 65 && row >= 75 && row <= 95)
            {
                if (col <= 45 || col >= 60 || row <= 78 || row >= 92)
                    return TerrainType.Mountain;
            }

            // 운남 남만 지역 - 숲/습지
            if (row > 105 && col < 60)
                return Mathf.PerlinNoise(col * 0.1f, row * 0.1f) > 0.5f
                    ? TerrainType.Forest : TerrainType.Swamp;

            // 서북 사막
            if (col < 35 && row < 50)
                return TerrainType.Desert;

            // 숲 (펄린 노이즈로 자연스럽게 배치)
            float noise = Mathf.PerlinNoise(col * 0.08f, row * 0.08f);
            if (noise > 0.65f)
                return TerrainType.Forest;

            return TerrainType.Plains;
        }

        private TerrainType MatchColor(Color32 pixel)
        {
            float minDist = float.MaxValue;
            TerrainType best = TerrainType.Plains;

            CheckColor(pixel, ColorPlains, TerrainType.Plains, ref minDist, ref best);
            CheckColor(pixel, ColorForest, TerrainType.Forest, ref minDist, ref best);
            CheckColor(pixel, ColorMountain, TerrainType.Mountain, ref minDist, ref best);
            CheckColor(pixel, ColorRiver, TerrainType.River, ref minDist, ref best);
            CheckColor(pixel, ColorSea, TerrainType.Sea, ref minDist, ref best);
            CheckColor(pixel, ColorDesert, TerrainType.Desert, ref minDist, ref best);
            CheckColor(pixel, ColorSwamp, TerrainType.Swamp, ref minDist, ref best);
            CheckColor(pixel, ColorPass, TerrainType.Pass, ref minDist, ref best);

            return best;
        }

        private void CheckColor(Color32 pixel, Color32 target, TerrainType type, ref float minDist, ref TerrainType best)
        {
            float dist = Mathf.Abs(pixel.r - target.r) + Mathf.Abs(pixel.g - target.g) + Mathf.Abs(pixel.b - target.b);
            if (dist < minDist)
            {
                minDist = dist;
                best = type;
            }
        }
    }
}
