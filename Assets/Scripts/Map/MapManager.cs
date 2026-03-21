using UnityEngine;
using SamGame.Core;
using SamGame.Map.Hex;

namespace SamGame.Map
{
    /// <summary>
    /// 전략 맵 관리 - 헥스 기반 선택, 카메라 이동, 호버 하이라이트
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Camera _mapCamera;
        [SerializeField] private HexGrid _hexGrid;

        [Header("카메라 설정")]
        [SerializeField] private float _zoomSpeed = 5f;
        [SerializeField] private float _panSpeed = 10f;
        [SerializeField] private float _minZoom = 5f;
        [SerializeField] private float _maxZoom = 60f;
        [SerializeField] private float _edgePanSpeed = 15f;
        [SerializeField] private float _edgePanThreshold = 20f; // 화면 가장자리 픽셀

        [Header("선택")]
        [SerializeField] private GameObject _hexHighlight;       // 헥스 하이라이트 표시용

        private HexCoordinates? _hoveredHex;
        private HexCoordinates? _selectedHex;

        private void Update()
        {
            HandleCameraInput();
            HandleEdgePan();
            HandleHexHover();
            HandleHexSelection();
        }

        private void HandleCameraInput()
        {
            // 줌
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                float newSize = _mapCamera.orthographicSize - scroll * _zoomSpeed;
                _mapCamera.orthographicSize = Mathf.Clamp(newSize, _minZoom, _maxZoom);
            }

            // 드래그 이동 (중간 버튼)
            if (Input.GetMouseButton(2))
            {
                float h = -Input.GetAxis("Mouse X") * _panSpeed * Time.deltaTime;
                float v = -Input.GetAxis("Mouse Y") * _panSpeed * Time.deltaTime;
                _mapCamera.transform.Translate(h, v, 0);
            }

            // WASD 이동
            float moveH = Input.GetAxis("Horizontal") * _panSpeed * Time.deltaTime;
            float moveV = Input.GetAxis("Vertical") * _panSpeed * Time.deltaTime;
            if (moveH != 0 || moveV != 0)
                _mapCamera.transform.Translate(moveH, moveV, 0);
        }

        /// <summary>
        /// 화면 가장자리에 마우스 두면 카메라 이동
        /// </summary>
        private void HandleEdgePan()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 move = Vector3.zero;

            if (mousePos.x < _edgePanThreshold) move.x = -1;
            else if (mousePos.x > Screen.width - _edgePanThreshold) move.x = 1;

            if (mousePos.y < _edgePanThreshold) move.y = -1;
            else if (mousePos.y > Screen.height - _edgePanThreshold) move.y = 1;

            if (move != Vector3.zero)
                _mapCamera.transform.Translate(move * _edgePanSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 마우스 호버 시 헥스 하이라이트
        /// </summary>
        private void HandleHexHover()
        {
            if (_hexGrid == null) return;

            Vector3 worldPos = _mapCamera.ScreenToWorldPoint(Input.mousePosition);
            var cell = _hexGrid.GetCellAtWorldPosition(worldPos);

            if (cell != null)
            {
                var coords = cell.Coordinates;
                if (_hoveredHex == null || _hoveredHex.Value != coords)
                {
                    _hoveredHex = coords;
                    if (_hexHighlight != null)
                    {
                        _hexHighlight.SetActive(true);
                        _hexHighlight.transform.position = coords.ToWorldPosition(_hexGrid.HexSize);
                    }
                }
            }
            else
            {
                _hoveredHex = null;
                if (_hexHighlight != null)
                    _hexHighlight.SetActive(false);
            }
        }

        /// <summary>
        /// 좌클릭 → 헥스/도시 선택
        /// </summary>
        private void HandleHexSelection()
        {
            if (!Input.GetMouseButtonDown(0) || _hexGrid == null) return;

            Vector3 worldPos = _mapCamera.ScreenToWorldPoint(Input.mousePosition);
            var cell = _hexGrid.GetCellAtWorldPosition(worldPos);
            if (cell == null) return;

            _selectedHex = cell.Coordinates;

            if (cell.IsCity)
            {
                EventManager.Publish(GameEvents.ProvinceSelected, cell.CityId);
            }
            else
            {
                EventManager.Publish(GameEvents.HexSelected, cell.Coordinates);
            }
        }

        /// <summary>
        /// 특정 도시로 카메라 이동
        /// </summary>
        public void FocusOnCity(HexCoordinates cityCoords)
        {
            var worldPos = cityCoords.ToWorldPosition(_hexGrid.HexSize);
            _mapCamera.transform.position = new Vector3(worldPos.x, worldPos.y, _mapCamera.transform.position.z);
        }
    }
}
