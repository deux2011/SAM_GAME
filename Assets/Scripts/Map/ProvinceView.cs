using UnityEngine;
using SamGame.Map.Hex;

namespace SamGame.Map
{
    /// <summary>
    /// 맵 위의 도시 마커 (영토는 HexChunk 메시가 처리)
    /// 도시 이름 + 아이콘만 표시
    /// </summary>
    public class ProvinceView : MonoBehaviour
    {
        public int ProvinceId;
        public HexCoordinates HexPosition;

        [SerializeField] private SpriteRenderer _cityIcon;
        [SerializeField] private TextMesh _nameLabel;
        [SerializeField] private SpriteRenderer _factionBanner;

        private Color _factionColor;
        private bool _isSelected;

        public void Initialize(int id, string name, Color factionColor)
        {
            ProvinceId = id;
            _factionColor = factionColor;

            if (_cityIcon != null)
                _cityIcon.color = Color.white;

            if (_nameLabel != null)
                _nameLabel.text = name;

            if (_factionBanner != null)
                _factionBanner.color = factionColor;
        }

        public void SetFactionColor(Color color)
        {
            _factionColor = color;
            if (_factionBanner != null)
                _factionBanner.color = color;
        }

        public void SetSelected(bool selected)
        {
            _isSelected = selected;
            if (_cityIcon != null)
                _cityIcon.color = selected ? Color.yellow : Color.white;
        }
    }
}
