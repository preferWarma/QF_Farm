using Game.Data;
using Lyf.SaveSystem;
using QFramework;
using UnityEngine;

namespace System
{
    public interface ISoilSystem : ISystem, ISaveWithJson
    {
        EasyGrid<SoilData> SoilGrid { get; }
        void LoadDefaultData();
    }

    public class SoilSystem : AbstractSystem, ISoilSystem
    {
        public EasyGrid<SoilData> SoilGrid { get; } = new(5, 4);

        protected override void OnInit()
        {
            SaveManager.Instance.Register(this, SaveType.Json);
        }

        #region 存储相关

        private class SaveDataCollection
        {
            public SoilData[][] SoilDatas;
        }

        public string SAVE_FILE_NAME => "SoilData";

        public void SaveWithJson()
        {
            var saveData = new SaveDataCollection
            {
                SoilDatas = new SoilData[SoilGrid.Width][]
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

            for (var i = 0; i < SoilGrid.Width; i++)
            {
                for (var j = 0; j < SoilGrid.Height; j++)
                {
                    SoilGrid[i, j] = saveData.SoilDatas[i][j];
                }
            }
            
            Debug.Log("加载土地数据成功");
        }
        
        public void LoadDefaultData()
        {
            for (var i = 0; i < SoilGrid.Width; i++)
            {
                for (var j = 0; j < SoilGrid.Height; j++)
                {
                    SoilGrid[i, j] = null;
                }
            }
            SaveWithJson();
        }

        #endregion
    }
}