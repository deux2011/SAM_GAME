using UnityEngine;
using SamGame.Map.Hex;

namespace SamGame.Map.Rendering
{
    /// <summary>
    /// 맵 청크 - 20x20 헥스 단위 메시 렌더링
    /// 영토 변경 시 해당 청크만 메시 재생성 (성능 최적화)
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class HexChunk : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private MeshCollider _meshCollider;

        public HexCell[] Cells { get; private set; }
        public float HexSize { get; private set; }
        public MapColorPalette Palette { get; private set; }

        private bool _isDirty;

        public void Initialize(HexCell[] cells, float hexSize, MapColorPalette palette, Material material)
        {
            Cells = cells;
            HexSize = hexSize;
            Palette = palette;

            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshCollider = GetComponent<MeshCollider>();
            if (_meshCollider == null)
                _meshCollider = gameObject.AddComponent<MeshCollider>();

            _meshRenderer.material = material;

            Refresh();
        }

        public void SetDirty()
        {
            _isDirty = true;
        }

        private void LateUpdate()
        {
            if (_isDirty)
            {
                Refresh();
                _isDirty = false;
            }
        }

        public void Refresh()
        {
            if (Cells == null || Cells.Length == 0) return;

            var mesh = HexMeshGenerator.GenerateChunkMesh(Cells, HexSize, Palette);
            _meshFilter.mesh = mesh;
            _meshCollider.sharedMesh = mesh;
        }
    }
}
