using Game.Data;
using QFramework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Tools
{
    public interface ITool
    {
        bool Selected();    // 是否被选中
        void Use(EasyGrid<SoilData> easyGrid, Tilemap tilemap, Vector3Int cellPos, TileBase pen);    // 使用
    }
}