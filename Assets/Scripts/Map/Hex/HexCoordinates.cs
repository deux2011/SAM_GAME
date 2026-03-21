using System;
using UnityEngine;

namespace SamGame.Map.Hex
{
    /// <summary>
    /// 큐브 좌표계 기반 헥스 좌표 (q + r + s = 0)
    /// Flat-top 헥스 레이아웃 사용
    /// </summary>
    [Serializable]
    public readonly struct HexCoordinates : IEquatable<HexCoordinates>
    {
        public readonly int Q;
        public readonly int R;
        public int S => -Q - R;

        public HexCoordinates(int q, int r)
        {
            Q = q;
            R = r;
        }

        // 6방향 이웃 (flat-top 기준)
        private static readonly HexCoordinates[] Directions = new HexCoordinates[]
        {
            new(1, 0), new(1, -1), new(0, -1),
            new(-1, 0), new(-1, 1), new(0, 1)
        };

        /// <summary>
        /// 6방향 인접 헥스 반환
        /// </summary>
        public HexCoordinates[] GetNeighbors()
        {
            var neighbors = new HexCoordinates[6];
            for (int i = 0; i < 6; i++)
            {
                neighbors[i] = new HexCoordinates(Q + Directions[i].Q, R + Directions[i].R);
            }
            return neighbors;
        }

        public HexCoordinates GetNeighbor(int direction)
        {
            var d = Directions[direction % 6];
            return new HexCoordinates(Q + d.Q, R + d.R);
        }

        /// <summary>
        /// 두 헥스 간 거리
        /// </summary>
        public static int Distance(HexCoordinates a, HexCoordinates b)
        {
            return (Mathf.Abs(a.Q - b.Q) + Mathf.Abs(a.R - b.R) + Mathf.Abs(a.S - b.S)) / 2;
        }

        /// <summary>
        /// 큐브 좌표 → Unity 월드 좌표 (flat-top)
        /// </summary>
        public Vector3 ToWorldPosition(float hexSize)
        {
            float x = hexSize * (1.5f * Q);
            float y = hexSize * (Mathf.Sqrt(3f) / 2f * Q + Mathf.Sqrt(3f) * R);
            return new Vector3(x, y, 0f);
        }

        /// <summary>
        /// Unity 월드 좌표 → 큐브 좌표 (가장 가까운 헥스)
        /// </summary>
        public static HexCoordinates FromWorldPosition(Vector3 worldPos, float hexSize)
        {
            float q = worldPos.x / (hexSize * 1.5f);
            float r = (worldPos.y - Mathf.Sqrt(3f) / 2f * hexSize * q) / (Mathf.Sqrt(3f) * hexSize);
            return CubeRound(q, r);
        }

        /// <summary>
        /// 오프셋 좌표(col, row) → 큐브 좌표
        /// Even-q offset 방식
        /// </summary>
        public static HexCoordinates FromOffsetCoordinates(int col, int row)
        {
            int q = col;
            int r = row - (col + (col & 1)) / 2;
            return new HexCoordinates(q, r);
        }

        /// <summary>
        /// 실수 큐브 좌표를 가장 가까운 정수 큐브 좌표로 반올림
        /// </summary>
        private static HexCoordinates CubeRound(float q, float r)
        {
            float s = -q - r;
            int rq = Mathf.RoundToInt(q);
            int rr = Mathf.RoundToInt(r);
            int rs = Mathf.RoundToInt(s);

            float qDiff = Mathf.Abs(rq - q);
            float rDiff = Mathf.Abs(rr - r);
            float sDiff = Mathf.Abs(rs - s);

            if (qDiff > rDiff && qDiff > sDiff)
                rq = -rr - rs;
            else if (rDiff > sDiff)
                rr = -rq - rs;

            return new HexCoordinates(rq, rr);
        }

        public bool Equals(HexCoordinates other) => Q == other.Q && R == other.R;
        public override bool Equals(object obj) => obj is HexCoordinates other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Q, R);
        public override string ToString() => $"({Q}, {R}, {S})";

        public static bool operator ==(HexCoordinates a, HexCoordinates b) => a.Equals(b);
        public static bool operator !=(HexCoordinates a, HexCoordinates b) => !a.Equals(b);
    }
}
