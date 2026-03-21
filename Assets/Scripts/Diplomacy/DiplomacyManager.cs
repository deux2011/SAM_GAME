using System.Linq;
using UnityEngine;
using SamGame.Core;
using SamGame.Data.Models;

namespace SamGame.Diplomacy
{
    /// <summary>
    /// 외교 시스템 - 동맹, 선전포고, 휴전, 우호도 관리
    /// </summary>
    public class DiplomacyManager : MonoBehaviour
    {
        public bool ProposeAlliance(FactionData proposer, FactionData target)
        {
            var relation = GetRelation(proposer, target.Id);
            if (relation == null || relation.Type == RelationType.War)
                return false;

            // 우호도 기반 수락 확률
            float acceptChance = Mathf.Clamp01((relation.Friendliness + 100) / 200f);
            if (Random.value > acceptChance)
                return false;

            relation.Type = RelationType.Alliance;
            relation.TurnsRemaining = 12; // 3년

            // 상대측도 동기화
            var reverseRelation = GetRelation(target, proposer.Id);
            if (reverseRelation != null)
            {
                reverseRelation.Type = RelationType.Alliance;
                reverseRelation.TurnsRemaining = 12;
            }

            EventManager.Publish(GameEvents.DiplomacyChanged);
            return true;
        }

        public void DeclareWar(FactionData attacker, FactionData target)
        {
            var relation = GetRelation(attacker, target.Id);
            if (relation != null)
            {
                relation.Type = RelationType.War;
                relation.TurnsRemaining = 0;
            }

            var reverseRelation = GetRelation(target, attacker.Id);
            if (reverseRelation != null)
            {
                reverseRelation.Type = RelationType.War;
                reverseRelation.TurnsRemaining = 0;
            }

            EventManager.Publish(GameEvents.DiplomacyChanged);
        }

        public void ProcessTurnDiplomacy(FactionData[] factions)
        {
            foreach (var faction in factions)
            {
                foreach (var relation in faction.Relations)
                {
                    if (relation.TurnsRemaining > 0)
                    {
                        relation.TurnsRemaining--;
                        if (relation.TurnsRemaining <= 0 && relation.Type == RelationType.Alliance)
                        {
                            relation.Type = RelationType.Neutral;
                        }
                    }
                }
            }
        }

        private DiplomacyRelation GetRelation(FactionData faction, int targetId)
        {
            return faction.Relations?.FirstOrDefault(r => r.TargetFactionId == targetId);
        }
    }
}
