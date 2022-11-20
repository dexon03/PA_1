namespace Lab4
{
    internal class BackPack
    {
        public int Capacity { get;} = 250;

        public List<Item> Items { get; set; }

        public BackPack()
        {
            Items = new List<Item>(100);
            for (int i = 0; i < Items.Capacity; i++)
            {
                Items.Add(new Item());
            }
        }
    }
}
