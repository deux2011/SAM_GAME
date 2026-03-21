using UnityEngine;
using SamGame.Data.Models;

namespace SamGame.Data.ScriptableObjects
{
    /// <summary>
    /// 전체 무장 데이터를 보관하는 ScriptableObject
    /// Unity 에디터에서 데이터 편집 가능
    /// </summary>
    [CreateAssetMenu(fileName = "OfficerDatabase", menuName = "SamGame/Officer Database")]
    public class OfficerDatabase : ScriptableObject
    {
        public OfficerData[] Officers;
    }
}
