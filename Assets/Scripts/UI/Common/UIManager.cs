using UnityEngine;

namespace SamGame.UI.Common
{
    /// <summary>
    /// UI 전체 관리 - 패널 전환, 팝업 등
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private GameObject _provinceInfoPanel;
        [SerializeField] private GameObject _officerInfoPanel;
        [SerializeField] private GameObject _battlePanel;
        [SerializeField] private GameObject _diplomacyPanel;
        [SerializeField] private GameObject _pauseMenu;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void ShowPanel(GameObject panel)
        {
            HideAllPanels();
            panel.SetActive(true);
        }

        public void HideAllPanels()
        {
            _provinceInfoPanel?.SetActive(false);
            _officerInfoPanel?.SetActive(false);
            _battlePanel?.SetActive(false);
            _diplomacyPanel?.SetActive(false);
        }

        public void TogglePause()
        {
            bool isPaused = _pauseMenu.activeSelf;
            _pauseMenu.SetActive(!isPaused);
            Time.timeScale = isPaused ? 1f : 0f;
        }
    }
}
