using System.Text;
using WarehouseApp.Models;
using WarehouseApp.Services;

namespace WarehouseApp;

public class Program
{
    private static readonly WarehouseService service = new();

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("\nУЧЁТ ТОВАРОВ НА СКЛАДАХ");
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Просмотреть товары");
            Console.WriteLine("3. Редактировать товар");
            Console.WriteLine("4. Добавить склад");
            Console.WriteLine("5. Просмотреть склады");
            Console.WriteLine("6. Добавить кладовщика");
            Console.WriteLine("7. Просмотреть кладовщиков");
            Console.WriteLine("8. Назначить кладовщика складу");
            Console.WriteLine("9. Зарегистрировать поступление товара");
            Console.WriteLine("10. Просмотреть поступления");
            Console.WriteLine("11. Посмотреть остатки на складе");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine() ?? "";

            try
            {
                switch (choice)
                {
                    case "1":
                        AddGoods();
                        break;
                    case "2":
                        ShowGoods();
                        break;
                    case "3":
                        EditGoods();
                        break;
                    case "4":
                        AddStore();
                        break;
                    case "5":
                        ShowStores();
                        break;
                    case "6":
                        AddStorekeeper();
                        break;
                    case "7":
                        ShowStorekeepers();
                        break;
                    case "8":
                        AssignStorekeeper();
                        break;
                    case "9":
                        AddGoodsInStore();
                        break;
                    case "10":
                        ShowGoodsInStores();
                        break;
                    case "11":
                        ShowStock();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine("\nНажмите Enter, чтобы продолжить.");
            Console.ReadLine();
        }
    }

    private static void AddGoods()
    {
        Console.WriteLine("\n--- Добавление товара ---");

        string name = ReadText("Название товара: ");
        string category = ReadText("Категория: ");
        decimal price = ReadDecimal("Цена: ");

        Goods item = service.AddGoods(name, category, price);

        Console.WriteLine($"Товар добавлен. ID: {item.GoodsId}");
    }

    private static void ShowGoods()
    {
        Console.WriteLine("\n--- Список товаров ---");

        if (!service.Goods.Any())
        {
            Console.WriteLine("Товаров пока нет.");
            return;
        }

        foreach (Goods item in service.Goods)
            Console.WriteLine(item);
    }

    private static void EditGoods()
    {
        Console.WriteLine("\n--- Редактирование товара ---");

        ShowGoods();

        int id = ReadInt("Введите ID товара: ");
        Goods? item = service.FindGoods(id);

        if (item == null)
        {
            Console.WriteLine("Товар не найден.");
            return;
        }

        Console.WriteLine($"Текущее название: {item.Name}");
        string name = ReadText("Новое название: ");

        Console.WriteLine($"Текущая категория: {item.Category}");
        string category = ReadText("Новая категория: ");

        Console.WriteLine($"Текущая цена: {item.Price}");
        decimal price = ReadDecimal("Новая цена: ");

        service.EditGoods(id, name, category, price);

        Console.WriteLine("Данные товара обновлены.");
    }

    private static void AddStore()
    {
        Console.WriteLine("\n--- Добавление склада ---");

        string name = ReadText("Название склада: ");
        string address = ReadText("Адрес склада: ");

        Stores store = service.AddStore(name, address);

        Console.WriteLine($"Склад добавлен. ID: {store.StoreId}");
    }

    private static void ShowStores()
    {
        Console.WriteLine("\n--- Список складов ---");

        if (!service.Stores.Any())
        {
            Console.WriteLine("Складов пока нет.");
            return;
        }

        foreach (Stores store in service.Stores)
        {
            Console.WriteLine(
                $"{store} | Кладовщик: {service.GetStorekeeperName(store.StorekeeperId)}"
            );
        }
    }

    private static void AddStorekeeper()
    {
        Console.WriteLine("\n--- Добавление кладовщика ---");

        string fullName = ReadText("ФИО кладовщика: ");
        string phone = ReadText("Телефон: ");

        Storekeepers storekeeper = service.AddStorekeeper(fullName, phone);

        Console.WriteLine($"Кладовщик добавлен. ID: {storekeeper.StorekeeperId}");
    }

    private static void ShowStorekeepers()
    {
        Console.WriteLine("\n--- Список кладовщиков ---");

        if (!service.Storekeepers.Any())
        {
            Console.WriteLine("Кладовщиков пока нет.");
            return;
        }

        foreach (Storekeepers storekeeper in service.Storekeepers)
            Console.WriteLine(storekeeper);
    }

    private static void AssignStorekeeper()
    {
        Console.WriteLine("\n--- Назначение кладовщика ---");

        ShowStores();
        ShowStorekeepers();

        int storeId = ReadInt("Введите ID склада: ");
        int storekeeperId = ReadInt("Введите ID кладовщика: ");

        service.AssignStorekeeper(storeId, storekeeperId);

        Console.WriteLine("Кладовщик назначен складу.");
    }

    private static void AddGoodsInStore()
    {
        Console.WriteLine("\n--- Регистрация поступления ---");

        ShowGoods();
        ShowStores();

        int goodsId = ReadInt("Введите ID товара: ");
        int storeId = ReadInt("Введите ID склада: ");
        int quantity = ReadInt("Количество поступивших единиц: ");

        GoodsInStores receipt = service.AddGoodsInStore(
            goodsId,
            storeId,
            quantity
        );

        Console.WriteLine($"Поступление зарегистрировано. ID: {receipt.GoodsInStoreId}");
    }

    private static void ShowGoodsInStores()
    {
        Console.WriteLine("\n--- История поступлений ---");

        if (!service.GoodsInStores.Any())
        {
            Console.WriteLine("Поступлений пока нет.");
            return;
        }

        foreach (GoodsInStores receipt in service.GoodsInStores)
        {
            Console.WriteLine(
                $"ID: {receipt.GoodsInStoreId} | " +
                $"Товар: {service.GetGoodsName(receipt.GoodsId)} | " +
                $"Склад: {service.GetStoreName(receipt.StoreId)} | " +
                $"Количество: {receipt.Quantity} | " +
                $"Дата: {receipt.ArrivalDate:dd.MM.yyyy HH:mm}"
            );
        }
    }

    private static void ShowStock()
    {
        Console.WriteLine("\n--- Остатки на складе ---");

        ShowStores();

        int storeId = ReadInt("Введите ID склада: ");
        Stores? store = service.FindStore(storeId);

        if (store == null)
        {
            Console.WriteLine("Склад не найден.");
            return;
        }

        Console.WriteLine($"Остатки склада «{store.Name}»:");

        foreach (Goods item in service.Goods)
        {
            int quantity = service.GetStock(item.GoodsId, storeId);

            Console.WriteLine(
                $"Товар: {item.Name} | Количество: {quantity} | " +
                $"Стоимость: {item.Price * quantity:N2} руб."
            );
        }
    }

    private static string ReadText(string message)
    {
        while (true)
        {
            Console.Write(message);
            string value = Console.ReadLine() ?? "";

            if (!string.IsNullOrWhiteSpace(value))
                return value.Trim();

            Console.WriteLine("Поле не должно быть пустым.");
        }
    }

    private static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
                return value;

            Console.WriteLine("Введите целое положительное число.");
        }
    }

    private static decimal ReadDecimal(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (decimal.TryParse(Console.ReadLine(), out decimal value) && value >= 0)
                return value;

            Console.WriteLine("Введите неотрицательное число.");
        }
    }
}