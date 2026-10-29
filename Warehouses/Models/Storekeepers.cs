namespace WarehouseApp.Models;

public class Storekeepers
{
    public int StorekeeperId { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }

    public Storekeepers(int storekeeperId, string fullName, string phone)
    {
        StorekeeperId = storekeeperId;
        FullName = fullName;
        Phone = phone;
    }

    public override string ToString()
    {
        return $"ID: {StorekeeperId} | ФИО: {FullName} | Телефон: {Phone}";
    }
}