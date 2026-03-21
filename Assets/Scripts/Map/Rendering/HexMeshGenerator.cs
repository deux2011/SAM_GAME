using System.Collections.Generic;
using UnityEngine;
using SamGame.Data.Models;
using SamGame.Map.Hex;

namespace SamGame.Map.Rendering
{
    /// <summary>
    /// 헥스 맵 메시 생성기
    /// 30,000개 헥스를 개별 GameObject 대신 청크 단위 결합 메시로 렌더링
    /// </summary>
    public static class HexMeshGenerator
    {
        /// <summary>
        /// 헥스 셀 목록으로 결합 메시 생성
        /// 각 헥스의 세력 색상 + 지형 틴트를 버텍스 컬러로 적용
        /// </summary>
        public static Mesh GenerateChunkMesh(HexCell[] cells, float hexSize, MapColorPalette palette)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var colors = new List<Color>();

            foreach (var cell in cells)
            {
                int vertexStart = vertices.Count;
                var center = cell.Coordinates.ToWorldPosition(hexSize);

                // 헥스 중심
                vertices.Add(center);
                colors.Add(GetCellColor(cell, palette));

                // 6개 꼭짓점 (flat-top)
                for (int i = 0; i < 6; i++)
                {
                    float angle = 60f * i;
                    float rad = Mathf.Deg2Rad * angle;
                    var vertex = center + new Vector3(
                        hexSize * Mathf.Cos(rad),
                        hexSize * Mathf.Sin(rad),
                        0f
                    );
                    vertices.Add(vertex);
                    colors.Add(GetCellColor(cell, palette));
                }

                // 6개 삼각형 (팬 방식)
                for (int i = 0; i < 6; i++)
                {
                    triangles.Add(vertexStart);                      // 중심
                    triangles.Add(vertexStart + 1 + i);              // 현재 꼭짓점
                    triangles.Add(vertexStart + 1 + (i + 1) % 6);   // 다음 꼭짓점
                }
            }

            var mesh = new Mesh();
            if (vertices.Count > 65535)
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.SetColors(colors);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

        private static Color GetCellColor(HexCell cell, MapColorPalette palette)
        {
            Color factionColor = palette.GetFactionColor(cell.OwnerFactionId);
            Color terrainTint = GetTerrainTint(cell.Terrain, palette);

            // 세력 색 70% + 지형 틴트 30% 블렌딩
            return Color.Lerp(factionColor, terrainTint, 0.3f);
        }

        private static Color GetTerrainTint(TerrainType terrain, MapColorPalette palette)
        {
            return terrain switch
            {
                TerrainType.Plains => palette.PlainsTint,
                TerrainType.Forest => palette.ForestTint,
                TerrainType.Mountain => palette.MountainTint,
                TerrainType.River => palette.RiverTint,
                TerrainType.Sea => palette.SeaTint,
                TerrainType.Desert => palette.DesertTint,
                TerrainType.Swamp => palette.SwampTint,
                _ => palette.PlainsTint
            };
        }
    }
}
