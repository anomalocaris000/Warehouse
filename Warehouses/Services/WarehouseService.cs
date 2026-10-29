using WarehouseApp.Models;

namespace WarehouseApp.Services;

public class WarehouseService
{
    private readonly Dictionary<int, Goods> goods = new();
    private readonly Dictionary<int, Stores> stores = new();
    private readonly Dictionary<int, Storekeepers> storekeepers = new();
    private readonly Dictionary<int, GoodsInStores> goodsInStores = new();

    public IReadOnlyCollection<Goods> Goods => goods.Values;
    public IReadOnlyCollection<Stores> Stores => stores.Values;
    public IReadOnlyCollection<Storekeepers> Storekeepers => storekeepers.Values;
    public IReadOnlyCollection<GoodsInStores> GoodsInStores => goodsInStores.Values;

    public Goods AddGoods(string name, string category, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("Название товара не может быть пустым.");

        if (price < 0)
            throw new InvalidOperationException("Цена не может быть отрицательной.");

        int id = goods.Count == 0 ? 1 : goods.Keys.Max() + 1;
        Goods item = new Goods(id, name, category, price);
        goods.Add(id, item);

        return item;
    }

    public void EditGoods(int id, string name, string category, decimal price)
    {
        if (!goods.TryGetValue(id, out Goods? item))
            throw new InvalidOperationException("Товар не найден.");

        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("Название товара не может быть пустым.");

        if (price < 0)
            throw new InvalidOperationException("Цена не может быть отрицательной.");

        item.Name = name;
        item.Category = category;
        item.Price = price;
    }

    public Stores AddStore(string name, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("Название склада не может быть пустым.");

        int id = stores.Count == 0 ? 1 : stores.Keys.Max() + 1;
        Stores store = new Stores(id, name, address);
        stores.Add(id, store);

        return store;
    }

    public Storekeepers AddStorekeeper(string fullName, string phone)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new InvalidOperationException("ФИО кладовщика не может быть пустым.");

        int id = storekeepers.Count == 0 ? 1 : storekeepers.Keys.Max() + 1;
        Storekeepers storekeeper = new Storekeepers(id, fullName, phone);
        storekeepers.Add(id, storekeeper);

        return storekeeper;
    }

    public void AssignStorekeeper(int storeId, int storekeeperId)
    {
        if (!stores.TryGetValue(storeId, out Stores? store))
            throw new InvalidOperationException("Склад не найден.");

        if (!storekeepers.ContainsKey(storekeeperId))
            throw new InvalidOperationException("Кладовщик не найден.");

        store.StorekeeperId = storekeeperId;
    }

    public GoodsInStores AddGoodsInStore(
        int goodsId,
        int storeId,
        int quantity)
    {
        if (!goods.ContainsKey(goodsId))
            throw new InvalidOperationException("Товар не найден.");

        if (!stores.ContainsKey(storeId))
            throw new InvalidOperationException("Склад не найден.");

        if (quantity <= 0)
            throw new InvalidOperationException("Количество должно быть больше нуля.");

        int id = goodsInStores.Count == 0 ? 1 : goodsInStores.Keys.Max() + 1;

        GoodsInStores receipt = new GoodsInStores(
            id,
            goodsId,
            storeId,
            quantity,
            DateTime.Now
        );

        goodsInStores.Add(id, receipt);

        return receipt;
    }

    public int GetStock(int goodsId, int storeId)
    {
        return goodsInStores.Values
            .Where(x => x.GoodsId == goodsId && x.StoreId == storeId)
            .Sum(x => x.Quantity);
    }

    public Goods? FindGoods(int id)
    {
        return goods.GetValueOrDefault(id);
    }

    public Stores? FindStore(int id)
    {
        return stores.GetValueOrDefault(id);
    }

    public Storekeepers? FindStorekeeper(int id)
    {
        return storekeepers.GetValueOrDefault(id);
    }

    public string GetGoodsName(int id)
    {
        return goods.TryGetValue(id, out Goods? item)
            ? item.Name
            : "Неизвестный товар";
    }

    public string GetStoreName(int id)
    {
        return stores.TryGetValue(id, out Stores? store)
            ? store.Name
            : "Неизвестный склад";
    }

    public string GetStorekeeperName(int? id)
    {
        if (!id.HasValue)
            return "Не назначен";

        return storekeepers.TryGetValue(id.Value, out Storekeepers? storekeeper)
            ? storekeeper.FullName
            : "Не назначен";
    }
}