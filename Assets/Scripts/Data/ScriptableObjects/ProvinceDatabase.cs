using UnityEngine;
using SamGame.Data.Models;

namespace SamGame.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProvinceDatabase", menuName = "SamGame/Province Database")]
    public class ProvinceDatabase : ScriptableObject
    {
        public ProvinceData[] Provinces;
    }
}
