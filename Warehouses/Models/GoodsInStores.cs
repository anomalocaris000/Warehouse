namespace WarehouseApp.Models;

public class GoodsInStores
{
    public int GoodsInStoreId { get; set; }
    public int GoodsId { get; set; }
    public int StoreId { get; set; }
    public int Quantity { get; set; }
    public DateTime ArrivalDate { get; set; }

    public GoodsInStores(
        int goodsInStoreId,
        int goodsId,
        int storeId,
        int quantity,
        DateTime arrivalDate)
    {
        GoodsInStoreId = goodsInStoreId;
        GoodsId = goodsId;
        StoreId = storeId;
        Quantity = quantity;
        ArrivalDate = arrivalDate;
    }

    public override string ToString()
    {
        return $"ID поступления: {GoodsInStoreId} | ID товара: {GoodsId} | ID склада: {StoreId} | Количество: {Quantity} | Дата: {ArrivalDate:dd.MM.yyyy}";
    }
}