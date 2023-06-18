using System.Collections.Generic;
using Game.UI;
using Lyf.SaveSystem;
using QFramework;

namespace System.ComputerSys
{
    public interface IComputerSystem : ISystem, ISaveWithJson
    {
        void ResetDefaultData();
    }

    public class ComputerSystem : IComputerSystem
    {
        public List<ComputerItem> ComputerItems = new();


        public void Init()
        {
            SaveManager.Instance.Register(this, SaveType.Json);
            
            InitComputerItems();
            
        }

        private void InitComputerItems()
        {
            var item1 = Add(new ComputerItem()
                .WithName("制作虚拟猫猫")
                .WithTotalHours(100f)
                .WithOnFinish(() =>
                {
                    UIComputer.FirstComputerItemIsFinished.Value = true;
                }).WithShowCondition(_ => Global.HasComputer)
            );
            
            var item2 = Add(new ComputerItem()
                .WithName("制作虚拟偶像")
                .WithTotalHours(100f)
                .WithOnFinish(() =>
                {
                    UIComputer.SecondComputerItemIsFinished.Value = true;
                }).WithShowCondition(_ => item1.IsFinished)
            );
        }

        private ComputerItem Add(ComputerItem item)
        {
            ComputerItems.Add(item);
            return item;
        }

        #region 存储相关

        private class SaveDataCollection
        {
            
        }

        public string SAVE_FILE_NAME => "ComputerSystem";
        public void SaveWithJson()
        {
            var saveData = new SaveDataCollection
            {

            };
            SaveManager.SaveWithJson(SAVE_FILE_NAME, saveData);
        }

        public void LoadWithJson()
        {
            var saveData = SaveManager.LoadWithJson<SaveDataCollection>(SAVE_FILE_NAME);
            if (saveData == null) return;
            
        }

        public void ResetDefaultData()
        {
            ComputerItems = new List<ComputerItem>();
            InitComputerItems();
        }

        #endregion
        
        public IArchitecture GetArchitecture()
        {
            return Global.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
        }
    }
}