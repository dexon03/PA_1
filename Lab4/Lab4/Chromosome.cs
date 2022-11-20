namespace Lab4
{
    internal class Chromosome
    {
        public List<int> Gene { get; set; }

        public Chromosome()
        {
            Gene = new List<int>(100);
            for (int i = 0; i < Gene.Capacity; i++)
            {
                Gene.Add(0);
            }
        }

        public Chromosome(List<int> gene)
        {
            Gene = gene;
        }

        public int GetWeight { get
            {
                int result = 0;
                for (int i = 0; i < Gene.Count; i++)
                {
                    if (Gene[i] == 1)
                    {
                        result += Solver.backPack.Items[i].Weight;
                    }
                }
                return result;
            }
        }


        public int GetValue
        {
            get
            {
                int result = 0;
                for (int i = 0; i < Gene.Count; i++)
                {
                    if (Gene[i] == 1)
                    {
                        result += Solver.backPack.Items[i].Value;
                    }
                }
                return result;
            }
        }

    }
}
