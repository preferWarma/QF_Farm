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
}