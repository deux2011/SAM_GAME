using UnityEngine;
using SamGame.Map.Hex;

namespace SamGame.Map.Army
{
    /// <summary>
    /// 맵 위의 군대 시각 표현
    /// </summary>
    public class ArmyView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _unitIcon;
        [SerializeField] private SpriteRenderer _banner;
        [SerializeField] private TextMesh _commanderName;
        [SerializeField] private TextMesh _soldierCount;
        [SerializeField] private float _moveAnimSpeed = 5f;

        public int ArmyId { get; private set; }

        private Vector3 _targetPosition;
        private bool _isMoving;

        public void Initialize(int armyId, string commanderName, int soldiers, Color factionColor, Vector3 startPos)
        {
            ArmyId = armyId;
            _commanderName.text = commanderName;
            _soldierCount.text = soldiers.ToString("N0");
            _banner.color = factionColor;
            transform.position = startPos;
            _targetPosition = startPos;
        }

        public void MoveTo(Vector3 worldPosition)
        {
            _targetPosition = worldPosition;
            _isMoving = true;
        }

        public void UpdateSoldierCount(int count)
        {
            _soldierCount.text = count.ToString("N0");
        }

        private void Update()
        {
            if (!_isMoving) return;

            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _moveAnimSpeed);

            if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
            {
                transform.position = _targetPosition;
                _isMoving = false;
            }
        }
    }
}
