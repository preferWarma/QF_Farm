using Game.Data;
using UnityEngine;

namespace Game
{
    public interface IPlant
    {
        int X { get; set; }
        int Y { get; set; }
        GameObject GameObject { get; }
        PlantSates Sate { get; }
        void SetState(PlantSates newSate);
        int RipeDay { get; }
        void Grow(SoilData soilData);
    }
}