using System.ComputerSys;
using QFramework;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class UIComputer　: ViewController, IController
    {
        public static readonly BindableProperty<bool> FirstComputerItemIsFinished = new();
        public static readonly BindableProperty<bool> SecondComputerItemIsFinished = new();
        
        private IComputerSystem _computerSystem;

        private void Awake()
        {
            _computerSystem = this.GetSystem<IComputerSystem>();
        }

        private void Start()
        {
            var items = _computerSystem.ComputerItems;
            foreach (var item in items)
            {
                ComputerItemTemplate.InstantiateWithParent(transform)
                    .Self(self =>
                    {
                        item.ItemObj = self.gameObject;
                        var textComponent = self.GetComponentInChildren<Text>();
                        textComponent.text = item.Name + $"({item.TotalHours - item.RestHours}h/{item.TotalHours}h)";
                        self.onClick.AddListener(() =>
                        {
                            if (item.RestHours < Global.RestHours.Value)    // 当日可完成
                            {
                                Global.RestHours.Value -= item.RestHours;
                                item.RestHours = 0;
                                item.IsFinished.Value = true;
                            }
                            else
                            {
                                item.RestHours -= Global.RestHours.Value;
                                Global.RestHours.Value = 0;
                            }
                            textComponent.text = item.Name + $"({item.TotalHours - item.RestHours}h/{item.TotalHours}h)";
                        });
                    });
            }
        }

        private void Update()
        {
            foreach (var item in _computerSystem.ComputerItems)
            {
                item.ItemObj.SetActive(item.ShowCondition());
            }
        }

        public IArchitecture GetArchitecture()
        {
            return Global.Interface;
        }
    }
}