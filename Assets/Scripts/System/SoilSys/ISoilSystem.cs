using Game;
using Game.Plants;
using Lyf.SaveSystem;
using QFramework;
using UnityEngine;

namespace System.SoilSys
{
    public interface ISoilSystem : ISystem, ISaveWithJson
    {
        EasyGrid<SoilData> SoilGrid { get; }
        void ResetDefaultData();
    }

    public class SoilSystem : AbstractSystem, ISoilSystem
    {
        public EasyGrid<SoilData> SoilGrid { get; private set; } = new(Config.InitSoilWidth, Config.InitSoilHeight);

        protected override void OnInit()
        {
            SaveManager.Instance.Register(this, SaveType.Json);
        }

        #region 存储相关

        private class SaveDataCollection
        {
            public SoilData[][] SoilDatas;
            public int Width;
            public int Height;
        }

        public string SAVE_FILE_NAME => "SoilData";

        public void SaveWithJson()
        {
            var saveData = new SaveDataCollection
            {
                SoilDatas = new SoilData[SoilGrid.Width][],
                Width = SoilGrid.Width,
                Height = SoilGrid.Height
            };
            for (var i = 0; i < SoilGrid.Width; i++)
            {
                saveData.SoilDatas[i] = new SoilData[SoilGrid.Height];
                for (var j = 0; j < SoilGrid.Height; j++)
                {
                    saveData.SoilDatas[i][j] = SoilGrid[i, j];
                }
            }

            SaveManager.SaveWithJson(SAVE_FILE_NAME, saveData);
        }

        public void LoadWithJson()
        {
            var saveData = SaveManager.LoadWithJson<SaveDataCollection>(SAVE_FILE_NAME);
            if (saveData == null)
            {
                Debug.LogError("加载土地数据失败");
                return;
            }
            SoilGrid.Resize(saveData.Width, saveData.Height,(_,_)=>null);
            PlantController.Instance.PlantGrid.Resize(saveData.Width, saveData.Height,(_,_)=>null);
            
            for (var i = 0; i < SoilGrid.Width; i++)
            {
                for (var j = 0; j < SoilGrid.Height; j++)
                {
                    SoilGrid[i, j] = saveData.SoilDatas[i][j];
                }
            }
        }
        
        public void ResetDefaultData()
        {
            SoilGrid = new EasyGrid<SoilData>(Config.InitSoilWidth, Config.InitSoilHeight);
            PlantController.Instance.PlantGrid = new EasyGrid<IPlant>(Config.InitSoilWidth, Config.InitSoilHeight);
        }

        #endregion
    }
}