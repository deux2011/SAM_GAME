using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SamGame.Data.Models;
using SamGame.Data.ScriptableObjects;

namespace SamGame.Officer
{
    /// <summary>
    /// 무장 관리 - 검색, 배치, 등용, 해고
    /// </summary>
    public class OfficerManager : MonoBehaviour
    {
        [SerializeField] private OfficerDatabase _database;

        private Dictionary<int, OfficerData> _officers = new();

        public void Initialize()
        {
            foreach (var officer in _database.Officers)
            {
                _officers[officer.Id] = officer;
            }
        }

        public OfficerData GetOfficer(int id)
        {
            return _officers.GetValueOrDefault(id);
        }

        public List<OfficerData> GetOfficersByFaction(int factionId)
        {
            return _officers.Values
                .Where(o => o.FactionId == factionId && o.State == OfficerState.Active)
                .ToList();
        }

        public List<OfficerData> GetOfficersByProvince(int provinceId)
        {
            return _officers.Values
                .Where(o => o.LocationProvinceId == provinceId && o.State == OfficerState.Active)
                .ToList();
        }

        public List<OfficerData> GetFreeOfficers()
        {
            return _officers.Values
                .Where(o => o.State == OfficerState.Free)
                .ToList();
        }

        public bool TryRecruit(int officerId, int factionId, int provinceId)
        {
            var officer = GetOfficer(officerId);
            if (officer == null || officer.State != OfficerState.Free)
                return false;

            officer.FactionId = factionId;
            officer.LocationProvinceId = provinceId;
            officer.State = OfficerState.Active;
            officer.Loyalty = 50;
            return true;
        }

        public void TransferOfficer(int officerId, int targetProvinceId)
        {
            var officer = GetOfficer(officerId);
            if (officer != null)
                officer.LocationProvinceId = targetProvinceId;
        }
    }
}
