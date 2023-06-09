﻿using System;
using Game.Tools;
using QFramework;
using UnityEngine;

namespace Game.Inventory
{
    // 背包物品类
    [Serializable]
    public class Item
    {
        [Header("基本属性")]
        public string name; // 名称
        public string iconName; // 图标名(最后会使用动态加载的方式加载图标)
        public BindableProperty<int> Count; // 数量
        public bool canStack; // 是否可堆叠

        [Tooltip("该物品对应的背包槽")]
        public UISlot slot;
        
        [Header("工具属性")]
        public ITool Tool; // 该物品对应的工具
        public bool isPlant; // 是否是植物
        public string plantPrefabName; // 植物预制体名字(最后会使用动态加载的方式加载植物预制体)
    }
}