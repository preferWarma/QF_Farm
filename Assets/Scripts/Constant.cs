// 配置物品名称的集合类
public static class ItemNameCollections
{
    // 工具类
    public const string Hand = "Hand";
    public const string Shovel = "Shovel";
    public const string WateringCan = "WateringCan";
    public const string Seed = "Seed";
    
    // 种子类
    public const string SeedPumpkin = "SeedPumpkin";
    public const string SeedRadish = "SeedRadish";
    public const string SeedPotato = "SeedPotato";
    public const string SeedTomato = "SeedTomato";
    public const string SeedBean = "SeedBean";
        
    // 物品类
    public const string Pumpkin = "Pumpkin";
    public const string Radish = "Radish";
    public const string Potato = "Potato";
    public const string Tomato = "Tomato";
    public const string Bean = "Bean";
    public const string Computer = "Computer";
    
    // 土地强化类
    public const string Soil5X5 = "Soil5X5";
    public const string Soil6X6 = "Soil6X6";
    public const string Soil7X7 = "Soil7X7";
    public const string Soil8X8 = "Soil8X8";
    
    public static string GetSoilNameBySize(int size)
    {
        return size switch
        {
            5 => Soil5X5,
            6 => Soil6X6,
            7 => Soil7X7,
            8 => Soil8X8,
            _ => null
        };
    }
}