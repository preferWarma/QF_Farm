using System;
using System.ComputerSys;
using QFramework;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class UIComputer　: ViewController, IController
    {
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
                        var mItem = item;   // 作缓存, 防止闭包
                        mItem.ItemObj = self.gameObject;
                        var textComponent = self.GetComponentInChildren<Text>();
                        textComponent.text = mItem.Name + $"({mItem.TotalHours}h)";

                        self.onClick.AddListener(() =>
                        {
                            if (mItem.CurrentHours.Value + Global.RestHours.Value >= mItem.TotalHours)    // 当日可完成
                            {
                                Global.RestHours.Value -= mItem.TotalHours - mItem.CurrentHours.Value;
                                mItem.CurrentHours.Value = mItem.TotalHours;
                                mItem.IsFinished.Value = true;
                                UIMessageQueue.Push("[完成]: " + mItem.Name);
                            }
                            else
                            {
                                mItem.CurrentHours.Value += Global.RestHours.Value;
                                Global.RestHours.Value = 0;
                                UIMessageQueue.Push($"[进行时]: {mItem.Name}剩余{mItem.TotalHours - mItem.CurrentHours.Value}h");
                            }
                            AudioController.Instance.Sfx_Trade.Play();
                        });

                        // 用于更新UI
                        mItem.CurrentHours.RegisterWithInitValue(value =>
                        {
                            if (Math.Abs(value - mItem.TotalHours) < 0.05f)
                            {
                                self.interactable = false;
                                textComponent.text = "[完成]: " + mItem.Name;
                            }
                            else
                            {
                                textComponent.text = mItem.Name + $"({mItem.CurrentHours.Value}h/{mItem.TotalHours}h)";
                            }
                        }).UnRegisterWhenGameObjectDestroyed(self.gameObject);
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