namespace WarehouseApp.Models;

public class Stores
{
    public int StoreId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int? StorekeeperId { get; set; }

    public Stores(int storeId, string name, string address)
    {
        StoreId = storeId;
        Name = name;
        Address = address;
        StorekeeperId = null;
    }

    public override string ToString()
    {
        string storekeeper = StorekeeperId.HasValue
            ? StorekeeperId.Value.ToString()
            : "не назначен";

        return $"ID: {StoreId} | Склад: {Name} | Адрес: {Address} | ID кладовщика: {storekeeper}";
    }
}