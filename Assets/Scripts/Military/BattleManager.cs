using UnityEngine;
using SamGame.Core;
using SamGame.Data.Models;

namespace SamGame.Military
{
    /// <summary>
    /// 전투 시스템 - 공격/방어 계산, 전투 결과 처리
    /// </summary>
    public class BattleManager : MonoBehaviour
    {
        /// <summary>
        /// 전투 실행
        /// </summary>
        public BattleResult ExecuteBattle(ArmyData attacker, ArmyData defender, OfficerData atkCommander, OfficerData defCommander)
        {
            float atkPower = CalculateAttackPower(attacker, atkCommander);
            float defPower = CalculateDefensePower(defender, defCommander);

            // 기본 전투 계산
            float atkDamage = atkPower * Random.Range(0.8f, 1.2f);
            float defDamage = defPower * Random.Range(0.8f, 1.2f);

            // 병력 손실
            int atkLoss = Mathf.RoundToInt(defDamage * 0.1f);
            int defLoss = Mathf.RoundToInt(atkDamage * 0.1f);

            attacker.SoldierCount = Mathf.Max(0, attacker.SoldierCount - atkLoss);
            defender.SoldierCount = Mathf.Max(0, defender.SoldierCount - defLoss);

            // 사기 변동
            attacker.Morale = Mathf.Clamp(attacker.Morale + (atkDamage > defDamage ? 5 : -10), 0, 100);
            defender.Morale = Mathf.Clamp(defender.Morale + (defDamage > atkDamage ? 5 : -10), 0, 100);

            var result = new BattleResult
            {
                AttackerLoss = atkLoss,
                DefenderLoss = defLoss,
                AttackerWon = attacker.SoldierCount > 0 && (defender.SoldierCount <= 0 || defender.Morale <= 0)
            };

            EventManager.Publish(GameEvents.BattleEnded, result);
            return result;
        }

        private float CalculateAttackPower(ArmyData army, OfficerData commander)
        {
            float basePower = army.SoldierCount * 0.1f;
            float leaderBonus = (commander.Leadership + commander.Warfare) * 0.5f;
            float moraleBonus = army.Morale / 100f;

            return basePower * (1 + leaderBonus / 100f) * moraleBonus;
        }

        private float CalculateDefensePower(ArmyData army, OfficerData commander)
        {
            float basePower = army.SoldierCount * 0.08f;
            float leaderBonus = (commander.Leadership + commander.Intelligence) * 0.5f;
            float moraleBonus = army.Morale / 100f;

            return basePower * (1 + leaderBonus / 100f) * moraleBonus;
        }
    }

    public class BattleResult
    {
        public int AttackerLoss;
        public int DefenderLoss;
        public bool AttackerWon;
    }
}
