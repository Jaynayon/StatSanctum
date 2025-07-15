using System.Text.Json.Serialization;

namespace StatSanctum.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ItemType
    {
        Armor,
        Belt,
        Boots,
        Cape,
        Gloves,
        Helm,
        Weapon
    }
}
