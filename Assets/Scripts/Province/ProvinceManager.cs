using System.Collections.Generic;
using UnityEngine;
using SamGame.Data.Models;
using SamGame.Data.ScriptableObjects;

namespace SamGame.Province
{
    /// <summary>
    /// 지역 관리 - 내정, 개발, 자원 생산
    /// </summary>
    public class ProvinceManager : MonoBehaviour
    {
        [SerializeField] private ProvinceDatabase _database;

        private Dictionary<int, ProvinceData> _provinces = new();

        public void Initialize()
        {
            foreach (var province in _database.Provinces)
            {
                _provinces[province.Id] = province;
            }
        }

        public ProvinceData GetProvince(int id)
        {
            return _provinces.GetValueOrDefault(id);
        }

        public List<ProvinceData> GetProvincesByFaction(int factionId)
        {
            var result = new List<ProvinceData>();
            foreach (var p in _provinces.Values)
            {
                if (p.FactionId == factionId)
                    result.Add(p);
            }
            return result;
        }

        /// <summary>
        /// 턴 종료 시 자원 생산
        /// </summary>
        public void ProcessTurnProduction()
        {
            foreach (var province in _provinces.Values)
            {
                province.Gold += province.Commerce / 10;
                province.Food += province.Agriculture / 10;
                province.Population += Mathf.Max(0, (province.Population / 100) - 1);
            }
        }

        /// <summary>
        /// 내정 - 상업 개발
        /// </summary>
        public void DevelopCommerce(int provinceId, int officerPolitics)
        {
            var province = GetProvince(provinceId);
            if (province == null) return;

            int increase = Mathf.Max(1, officerPolitics / 20);
            province.Commerce = Mathf.Min(999, province.Commerce + increase);
        }

        /// <summary>
        /// 내정 - 농업 개발
        /// </summary>
        public void DevelopAgriculture(int provinceId, int officerPolitics)
        {
            var province = GetProvince(provinceId);
            if (province == null) return;

            int increase = Mathf.Max(1, officerPolitics / 20);
            province.Agriculture = Mathf.Min(999, province.Agriculture + increase);
        }

        /// <summary>
        /// 성벽 강화
        /// </summary>
        public void FortifyDefense(int provinceId, int officerPolitics)
        {
            var province = GetProvince(provinceId);
            if (province == null) return;

            int increase = Mathf.Max(1, officerPolitics / 25);
            province.Defense = Mathf.Min(999, province.Defense + increase);
        }
    }
}
