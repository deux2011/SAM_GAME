using UnityEngine;
using UnityEngine.UI;
using SamGame.Core;
using SamGame.Data.Models;

namespace SamGame.UI.Panels
{
    /// <summary>
    /// 지역 정보 패널 UI
    /// </summary>
    public class ProvinceInfoPanel : MonoBehaviour
    {
        [SerializeField] private Text _provinceName;
        [SerializeField] private Text _factionName;
        [SerializeField] private Text _population;
        [SerializeField] private Text _gold;
        [SerializeField] private Text _food;
        [SerializeField] private Text _commerce;
        [SerializeField] private Text _agriculture;
        [SerializeField] private Text _defense;
        [SerializeField] private Transform _officerListContainer;
        [SerializeField] private GameObject _officerSlotPrefab;

        private void OnEnable()
        {
            EventManager.Subscribe(GameEvents.ProvinceSelected, OnProvinceSelected);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe(GameEvents.ProvinceSelected, OnProvinceSelected);
        }

        private void OnProvinceSelected(object data)
        {
            // int provinceId = (int)data;
            // ProvinceData를 가져와서 UI 업데이트
            gameObject.SetActive(true);
        }

        public void UpdatePanel(ProvinceData province, string factionName)
        {
            _provinceName.text = province.Name;
            _factionName.text = factionName;
            _population.text = $"인구: {province.Population:N0}";
            _gold.text = $"금: {province.Gold:N0}";
            _food.text = $"병량: {province.Food:N0}";
            _commerce.text = $"상업: {province.Commerce}";
            _agriculture.text = $"농업: {province.Agriculture}";
            _defense.text = $"방어: {province.Defense}";
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
