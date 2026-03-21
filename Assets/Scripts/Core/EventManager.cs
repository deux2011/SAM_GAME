using System;
using System.Collections.Generic;

namespace SamGame.Core
{
    /// <summary>
    /// 이벤트 기반 시스템 간 통신 매니저
    /// </summary>
    public static class EventManager
    {
        private static readonly Dictionary<string, Action<object>> _events = new();

        public static void Subscribe(string eventName, Action<object> handler)
        {
            if (!_events.ContainsKey(eventName))
                _events[eventName] = null;
            _events[eventName] += handler;
        }

        public static void Unsubscribe(string eventName, Action<object> handler)
        {
            if (_events.ContainsKey(eventName))
                _events[eventName] -= handler;
        }

        public static void Publish(string eventName, object data = null)
        {
            if (_events.ContainsKey(eventName))
                _events[eventName]?.Invoke(data);
        }

        public static void Clear()
        {
            _events.Clear();
        }
    }

    // 이벤트 이름 상수
    public static class GameEvents
    {
        public const string TurnAdvanced = "TurnAdvanced";
        public const string ProvinceSelected = "ProvinceSelected";
        public const string OfficerAssigned = "OfficerAssigned";
        public const string BattleStarted = "BattleStarted";
        public const string BattleEnded = "BattleEnded";
        public const string DiplomacyChanged = "DiplomacyChanged";
        public const string ResourceChanged = "ResourceChanged";

        // 맵 시스템
        public const string HexSelected = "HexSelected";
        public const string TerritoryChanged = "TerritoryChanged";
        public const string ArmyMoved = "ArmyMoved";
        public const string ArmyArrived = "ArmyArrived";
        public const string CityCaptured = "CityCaptured";
        public const string PathRequested = "PathRequested";
    }
}
