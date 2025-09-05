using System.Text.Json.Serialization;

namespace API.Models
{
    public class AllItems
    {
        public string Type { get; set; }

        public string Version { get; set; }
        public BasicItem Basic { get; set; }
        public Dictionary<string, Item> Data { get; set; }

    }

    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Colloq { get; set; }
        public string Plaintext { get; set; }
        public bool? Consumed { get; set; }
        public int? Stacks { get; set; }
        public int? Depth { get; set; }
        public bool? ConsumeOnFull { get; set; }
        public List<string> From { get; set; }
        public List<string> Into { get; set; }
        public int? SpecialRecipe { get; set; }
        public bool? InStore { get; set; }
        public bool? HideFromAll { get; set; }
        public string RequiredChampion { get; set; }
        public string RequiredAlly { get; set; }
        public ItemImage Image { get; set; }
        public Gold Gold { get; set; }
        public List<string> Tags { get; set; }
        public Dictionary<string, bool> Maps { get; set; }
        public Stats Stats { get; set; }
        public Dictionary<string, string> Effect { get; set; }
    }

    public class BasicItem
    {
        public string Name { get; set; }
        public Rune Rune { get; set; }
        public Gold Gold { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public string Colloq { get; set; }
        public string Plaintext { get; set; }
        public bool Consumed { get; set; }
        public int Stacks { get; set; }
        public int Depth { get; set; }
        public bool ConsumeOnFull { get; set; }
        public List<string> From { get; set; }
        public List<string> Into { get; set; }
        public int SpecialRecipe { get; set; }
        public bool InStore { get; set; }
        public bool HideFromAll { get; set; }
        public string RequiredChampion { get; set; }
        public string RequiredAlly { get; set; }
        public Stats Stats { get; set; }
        public List<string> Tags { get; set; }
        public Dictionary<string, bool> Maps { get; set; }
    }

    public class Rune
    {
        public bool IsRune { get; set; }

        public int Tier { get; set; }

        public string Type { get; set; }
    }

    public class Gold
    {
        public int Base { get; set; }
        public int Total { get; set; }
        public int Sell { get; set; }
        public bool Purchasable { get; set; }
    }

    public class ItemImage
    {
        public string Full { get; set; }
        public string Sprite { get; set; }
        public string Group { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
    }

    public class Stats
    {
        public double? FlatHPPoolMod { get; set; }
        public double? RFlatHPModPerLevel { get; set; }
        public double? FlatMPPoolMod { get; set; }
        public double? RFlatMPModPerLevel { get; set; }
        public double? PercentHPPoolMod { get; set; }
        public double? PercentMPPoolMod { get; set; }
        public double? FlatHPRegenMod { get; set; }
        public double? RFlatHPRegenModPerLevel { get; set; }
        public double? PercentHPRegenMod { get; set; }
        public double? FlatMPRegenMod { get; set; }
        public double? RFlatMPRegenModPerLevel { get; set; }
        public double? PercentMPRegenMod { get; set; }
        public double? FlatArmorMod { get; set; }
        public double? RFlatArmorModPerLevel { get; set; }
        public double? PercentArmorMod { get; set; }
        public double? RFlatArmorPenetrationMod { get; set; }
        public double? RFlatArmorPenetrationModPerLevel { get; set; }
        public double? RPercentArmorPenetrationMod { get; set; }
        public double? RPercentArmorPenetrationModPerLevel { get; set; }
        public double? FlatPhysicalDamageMod { get; set; }
        public double? RFlatPhysicalDamageModPerLevel { get; set; }
        public double? PercentPhysicalDamageMod { get; set; }
        public double? FlatMagicDamageMod { get; set; }
        public double? RFlatMagicDamageModPerLevel { get; set; }
        public double? PercentMagicDamageMod { get; set; }
        public double? FlatMovementSpeedMod { get; set; }
        public double? RFlatMovementSpeedModPerLevel { get; set; }
        public double? PercentMovementSpeedMod { get; set; }
        public double? RPercentMovementSpeedModPerLevel { get; set; }
        public double? FlatAttackSpeedMod { get; set; }
        public double? PercentAttackSpeedMod { get; set; }
        public double? RPercentAttackSpeedModPerLevel { get; set; }
        public double? RFlatDodgeMod { get; set; }
        public double? RFlatDodgeModPerLevel { get; set; }
        public double? PercentDodgeMod { get; set; }
        public double? FlatCritChanceMod { get; set; }
        public double? RFlatCritChanceModPerLevel { get; set; }
        public double? PercentCritChanceMod { get; set; }
        public double? FlatCritDamageMod { get; set; }
        public double? RFlatCritDamageModPerLevel { get; set; }
        public double? PercentCritDamageMod { get; set; }
        public double? FlatBlockMod { get; set; }
        public double? PercentBlockMod { get; set; }
        public double? FlatSpellBlockMod { get; set; }
        public double? RFlatSpellBlockModPerLevel { get; set; }
        public double? PercentSpellBlockMod { get; set; }
        public double? FlatEXPBonus { get; set; }
        public double? PercentEXPBonus { get; set; }
        public double? RPercentCooldownMod { get; set; }
        public double? RPercentCooldownModPerLevel { get; set; }
        public double? RFlatTimeDeadMod { get; set; }
        public double? RFlatTimeDeadModPerLevel { get; set; }
        public double? RPercentTimeDeadMod { get; set; }
        public double? RPercentTimeDeadModPerLevel { get; set; }
        public double? RFlatGoldPer10Mod { get; set; }
        public double? RFlatMagicPenetrationMod { get; set; }
        public double? RFlatMagicPenetrationModPerLevel { get; set; }
        public double? RPercentMagicPenetrationMod { get; set; }
        public double? RPercentMagicPenetrationModPerLevel { get; set; }
        public double? FlatEnergyRegenMod { get; set; }
        public double? RFlatEnergyRegenModPerLevel { get; set; }
        public double? FlatEnergyPoolMod { get; set; }
        public double? RFlatEnergyModPerLevel { get; set; }
        public double? PercentLifeStealMod { get; set; }
        public double? PercentSpellVampMod { get; set; }
    }
}
