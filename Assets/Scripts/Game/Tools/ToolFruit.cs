﻿namespace Game.Tools
{
    // 植物果实工具,仅用于防止空引用异常
    public class ToolFruit : ITool
    {
        public string Name => "Fruit";
        public int ToolScope => 1;
        public float CostHours => 0f;
        public float CdTime { get; set; } = 0f;
        public float InitCdTime => 0f;

        public bool Selected()
        {
            return Global.CurrentTool.Value.Name == Name;
        }

        public void Use(ToolNeedData needData)
        {
        }
    }
}