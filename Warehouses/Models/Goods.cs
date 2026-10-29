namespace WarehouseApp.Models;

public class Goods
{
    public int GoodsId { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }

    public Goods(int goodsId, string name, string category, decimal price)
    {
        GoodsId = goodsId;
        Name = name;
        Category = category;
        Price = price;
    }

    public override string ToString()
    {
        return $"ID: {GoodsId} | Название: {Name} | Категория: {Category} | Цена: {Price:N2} руб.";
    }
}