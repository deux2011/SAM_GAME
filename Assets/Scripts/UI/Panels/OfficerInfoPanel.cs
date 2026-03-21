using UnityEngine;
using UnityEngine.UI;
using SamGame.Data.Models;

namespace SamGame.UI.Panels
{
    /// <summary>
    /// 무장 상세 정보 패널
    /// </summary>
    public class OfficerInfoPanel : MonoBehaviour
    {
        [Header("기본 정보")]
        [SerializeField] private Image _portrait;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _courtesyNameText;

        [Header("능력치")]
        [SerializeField] private Text _leadershipText;
        [SerializeField] private Text _warfareText;
        [SerializeField] private Text _intelligenceText;
        [SerializeField] private Text _politicsText;
        [SerializeField] private Text _charismaText;

        [Header("병과 적성")]
        [SerializeField] private Text _infantryText;
        [SerializeField] private Text _cavalryText;
        [SerializeField] private Text _archerText;
        [SerializeField] private Text _siegeText;

        [Header("상태")]
        [SerializeField] private Text _loyaltyText;
        [SerializeField] private Text _stateText;

        public void Show(OfficerData officer)
        {
            gameObject.SetActive(true);

            _nameText.text = officer.Name;
            _courtesyNameText.text = officer.CourtesyName ?? "";

            _leadershipText.text = $"통솔: {officer.Leadership}";
            _warfareText.text = $"무력: {officer.Warfare}";
            _intelligenceText.text = $"지력: {officer.Intelligence}";
            _politicsText.text = $"정치: {officer.Politics}";
            _charismaText.text = $"매력: {officer.Charisma}";

            _infantryText.text = $"보병: {officer.Infantry}";
            _cavalryText.text = $"기병: {officer.Cavalry}";
            _archerText.text = $"궁병: {officer.Archer}";
            _siegeText.text = $"공성: {officer.Siege}";

            _loyaltyText.text = $"충성: {officer.Loyalty}";
            _stateText.text = officer.State.ToString();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
