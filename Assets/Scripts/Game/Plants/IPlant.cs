using System;
using System.SoilSys;
using UnityEngine;

namespace Game.Plants
{
    public interface IPlant
    {
        int X { get; set; }
        int Y { get; set; }
        GameObject GameObject { get; }
        PlantSates Sate { get; }
        int RipeDay { get; }
        string PlantName { get; }
        void SetState(PlantSates newSate);
        void Grow(SoilData soilData);
    }

    [Serializable]
    public class PlantStateInfo
    {
        [Tooltip("状态")] public PlantSates sate;
        [Tooltip("状态对应的贴图")] public Sprite sprite;
        [Tooltip("状态对应的生长天数")] public int growDay;
        [Tooltip("是否显示地块挖掘状态")] public bool showSoilDig;
    }

    // 为IPlant接口添加扩展方法
    public static class PlantExtensions
    {
        // 清除耕地开垦状态
        public static void ClearSoilDigState(this IPlant plant, GridController gridController)
        {
            gridController.Soil.SetTile(new Vector3Int(plant.X, plant.Y, 0), null);
        }
    }
}