var items = new Items();

public partial class Items
{
    public partial int Capacity { get; set; }
}

public partial class Items
{
    public partial int Capacity
    {
        get => _items.Count;
        set
        {
            if (value != _items.Count && value >= 0)
            {
                _items.Capacity = value;
            }
        }
    }

    private readonly List<int> _items = Enumerable.Range(0, 10).ToList();
}