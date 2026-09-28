namespace LunchSplit
{
    // One person at the table.
    public class Attendee
    {
        public string Name { get; }
        public int Weight { get; }
        public bool Included { get; }

        public Attendee(string name, int weight, bool included)
        {
            Name = name;
            Weight = weight;
            Included = included;
        }
    }
}
