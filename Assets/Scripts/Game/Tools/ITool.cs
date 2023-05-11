using Game.Data;
using QFramework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Tools
{
    public interface ITool
    {
        string Name { get; }
        int ToolScope { get; set; }  // 作用范围
        bool Selected();    // 是否被选中
        void Use(ToolNeedData needData);    // 使用
    }

    // 用于传递数据
    public class ToolNeedData
    {
        public EasyGrid<SoilData> ShowGrid;
        public Tilemap Tilemap;
        public Vector3Int CellPos;
        public TileBase Pen;
    }
}