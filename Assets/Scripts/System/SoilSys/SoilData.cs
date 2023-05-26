using Game;

namespace System.SoilSys
{
    public class SoilData
    {
        public bool HasPlant;   // 是否种植了
        public bool Watered;    // 是否浇水了
        public PlantSates PlantSate;   // 植物状态
        public string PlantPrefabName;    // 植物名称
        public int CurrentStateDay; // 当前状态的生长天数
        
    }
}