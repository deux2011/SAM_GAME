using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SamGame.Data.Models;
using SamGame.Province;
using SamGame.Officer;

namespace SamGame.AI
{
    /// <summary>
    /// AI 세력의 의사결정 시스템
    /// </summary>
    public class AIController : MonoBehaviour
    {
        [SerializeField] private ProvinceManager _provinceManager;
        [SerializeField] private OfficerManager _officerManager;

        public void ProcessAITurn(FactionData faction)
        {
            var provinces = _provinceManager.GetProvincesByFaction(faction.Id);
            var officers = _officerManager.GetOfficersByFaction(faction.Id);

            switch (faction.CurrentPolicy)
            {
                case FactionPolicy.Development:
                    ExecuteDevelopmentStrategy(faction, provinces, officers);
                    break;
                case FactionPolicy.Expansion:
                    ExecuteExpansionStrategy(faction, provinces, officers);
                    break;
                case FactionPolicy.Military:
                    ExecuteMilitaryStrategy(faction, provinces, officers);
                    break;
                case FactionPolicy.Diplomacy:
                    ExecuteDiplomacyStrategy(faction, provinces, officers);
                    break;
            }
        }

        private void ExecuteDevelopmentStrategy(FactionData faction, List<ProvinceData> provinces, List<OfficerData> officers)
        {
            // 정치력 높은 무장을 내정에 배치
            var politicsOfficers = officers.OrderByDescending(o => o.Politics).ToList();
            foreach (var province in provinces)
            {
                var available = politicsOfficers.FirstOrDefault(o => o.LocationProvinceId == province.Id);
                if (available != null)
                {
                    if (province.Commerce < province.Agriculture)
                        _provinceManager.DevelopCommerce(province.Id, available.Politics);
                    else
                        _provinceManager.DevelopAgriculture(province.Id, available.Politics);
                }
            }
        }

        private void ExecuteExpansionStrategy(FactionData faction, List<ProvinceData> provinces, List<OfficerData> officers)
        {
            // TODO: 인접 약한 지역 탐색 후 공격 명령
            Debug.Log($"[AI] {faction.Name}: 영토 확장 전략 실행");
        }

        private void ExecuteMilitaryStrategy(FactionData faction, List<ProvinceData> provinces, List<OfficerData> officers)
        {
            // TODO: 병력 증강, 전선 지역 방어 강화
            Debug.Log($"[AI] {faction.Name}: 군사 강화 전략 실행");
        }

        private void ExecuteDiplomacyStrategy(FactionData faction, List<ProvinceData> provinces, List<OfficerData> officers)
        {
            // TODO: 우호도 높은 세력에 동맹 제안
            Debug.Log($"[AI] {faction.Name}: 외교 전략 실행");
        }
    }
}
