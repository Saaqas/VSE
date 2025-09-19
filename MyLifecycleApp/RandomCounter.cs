public class RandomCounter : ICounter
{
    private static readonly Random rnd = new Random();
    private readonly int _value;

    public RandomCounter()
    {
        _value = rnd.Next(0, 1000000);
    }

    public int Value => _value;
}
