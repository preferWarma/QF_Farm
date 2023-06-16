using Game;

namespace System.SoilSys
{
    [Serializable]
    public class SoilData
    {
        public bool HasPlant;   // 是否种植了
        public bool Watered;    // 是否浇水了
        public PlantSates PlantSate;   // 植物状态
        public string PlantPrefabName;    // 植物名称
        public int CurrentStateDay; // 当前状态的生长天数
        public int RipeCount;   // 当前土地采摘时的果实数量
    }
}