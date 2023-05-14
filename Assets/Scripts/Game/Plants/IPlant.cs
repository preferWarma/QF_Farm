using Game.Data;
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
        
        void SetState(PlantSates newSate);
        void Grow(SoilData soilData);
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