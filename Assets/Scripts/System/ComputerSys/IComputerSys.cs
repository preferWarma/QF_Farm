using System.Collections.Generic;
using Lyf.SaveSystem;
using QFramework;

namespace System.ComputerSys
{
    public interface IComputerSystem : ISystem, ISaveWithJson
    {
        List<ComputerItem> ComputerItems { get; }
        void ResetDefaultData();
    }

    public class ComputerSystem : IComputerSystem
    {
        public List<ComputerItem> ComputerItems { get; } = new();


        public void Init()
        {
            SaveManager.Instance.Register(this, SaveType.Json);
            
            InitComputerItems();
            
        }

        private void InitComputerItems()
        {
            ComputerItems.Clear();
            
            var item1 = Add(new ComputerItem()
                .WithName("制作虚拟猫猫")
                .WithTotalHours(10f)
                .WithPrice(15)
                .WithOnFinish(() =>
                {
                })
                .WithShowCondition(_ => Global.HasComputer)
                .Self(item =>
                {
                    item.IsFinished.RegisterWithInitValue(value =>
                    {
                        if (value)
                        {
                            item.OnFinish();
                        }
                    });
                })
            );
            
            var item2 = Add(new ComputerItem()
                .WithName("制作虚拟偶像")
                .WithTotalHours(100f)
                .WithPrice(150)
                .WithOnFinish(() =>
                {
                }).WithShowCondition(_ => item1.IsFinished)
                .Self(item =>
                {
                    item.IsFinished.RegisterWithInitValue(value =>
                    {
                        if (value)
                        {
                            item.OnFinish();
                        }
                    });
                })
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
            public List<bool> IsFinishedList = new();
            public List<float> CurrentHoursList = new();
            
        }

        public string SAVE_FILE_NAME => "ComputerSystem";
        public void SaveWithJson()
        {
            var saveData = new SaveDataCollection
            {
                IsFinishedList = ComputerItems.ConvertAll(item => item.IsFinished.Value),
                CurrentHoursList = ComputerItems.ConvertAll(item => item.CurrentHours.Value)
            };
            
            SaveManager.SaveWithJson(SAVE_FILE_NAME, saveData);
        }

        public void LoadWithJson()
        {
            var saveData = SaveManager.LoadWithJson<SaveDataCollection>(SAVE_FILE_NAME);
            if (saveData == null) return;
            for (var i = 0; i < saveData.IsFinishedList.Count; i++)
            {
                ComputerItems[i].IsFinished.Value = saveData.IsFinishedList[i];
                ComputerItems[i].CurrentHours.Value = saveData.CurrentHoursList[i];
            }
            
        }

        public void ResetDefaultData()
        {
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