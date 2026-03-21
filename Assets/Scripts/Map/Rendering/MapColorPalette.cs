using UnityEngine;

namespace SamGame.Map.Rendering
{
    /// <summary>
    /// 맵 색상 설정 (세력 색, 지형 틴트, 하이라이트)
    /// </summary>
    [CreateAssetMenu(fileName = "MapColorPalette", menuName = "SamGame/Map Color Palette")]
    public class MapColorPalette : ScriptableObject
    {
        [Header("세력 색상")]
        public Color NeutralColor = new(0.7f, 0.7f, 0.7f);
        public FactionColor[] FactionColors = new[]
        {
            new FactionColor { Name = "위 (조조)", Color = new Color(0.2f, 0.4f, 0.9f) },     // 파랑
            new FactionColor { Name = "촉 (유비)", Color = new Color(0.1f, 0.8f, 0.2f) },     // 녹색
            new FactionColor { Name = "오 (손권)", Color = new Color(0.9f, 0.2f, 0.2f) },     // 빨강
            new FactionColor { Name = "원소", Color = new Color(0.8f, 0.8f, 0.2f) },          // 노랑
            new FactionColor { Name = "여포", Color = new Color(0.6f, 0.1f, 0.6f) },          // 보라
            new FactionColor { Name = "원술", Color = new Color(0.9f, 0.6f, 0.1f) },          // 주황
            new FactionColor { Name = "유표", Color = new Color(0.4f, 0.7f, 0.7f) },          // 청록
            new FactionColor { Name = "마등", Color = new Color(0.6f, 0.4f, 0.2f) },          // 갈색
        };

        [Header("지형 틴트")]
        public Color PlainsTint = new(0.85f, 0.9f, 0.75f);
        public Color ForestTint = new(0.3f, 0.6f, 0.3f);
        public Color MountainTint = new(0.6f, 0.55f, 0.5f);
        public Color RiverTint = new(0.3f, 0.5f, 0.8f);
        public Color SeaTint = new(0.2f, 0.3f, 0.7f);
        public Color DesertTint = new(0.9f, 0.85f, 0.6f);
        public Color SwampTint = new(0.4f, 0.5f, 0.35f);

        [Header("하이라이트")]
        public Color SelectionHighlight = new(1f, 1f, 0f, 0.5f);
        public Color PathHighlight = new(1f, 1f, 1f, 0.4f);
        public Color AttackHighlight = new(1f, 0f, 0f, 0.4f);

        public Color GetFactionColor(int factionId)
        {
            if (factionId < 0 || factionId >= FactionColors.Length)
                return NeutralColor;
            return FactionColors[factionId].Color;
        }
    }

    [System.Serializable]
    public struct FactionColor
    {
        public string Name;
        public Color Color;
    }
}
