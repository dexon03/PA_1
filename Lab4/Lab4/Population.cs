using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    internal class Population
    {
        private BackPack _backPack { get; set; }
        internal List<Chromosome> Chromosomes;

        public Population(BackPack backPack)
        {
            Chromosomes = new List<Chromosome>();
            _backPack = backPack;
        }

        public void GeneratePopulation()
        {
            for (int i = 0; i < 100; i++)
            {
                Chromosomes.Add(new Chromosome());
                Chromosomes[i].Gene[i] = 1;
            }
        }

        //public Chromosome GetBestChromosome()
        //{
        //    int MaxValue = Int32.MinValue;
        //    int MaxIndex = -1;
        //    for (int i = 0; i < Chromosomes.Count; i++)
        //    {
        //        int currentValue = Chromosomes[i].GetValue;
        //        if (MaxValue <= currentValue)
        //        {
        //            MaxValue = currentValue;
        //            MaxIndex = i;
        //        }
        //    }

        //    return Chromosomes[MaxIndex];
        //}
        public Chromosome GetBestChromosome
        {
            get
            {
                int MaxValue = Int32.MinValue;
                int MaxIndex = -1;
                for (int i = 0; i < Chromosomes.Count; i++)
                {
                    int currentValue = Chromosomes[i].GetValue;
                    if (MaxValue <= currentValue)
                    {
                        MaxValue = currentValue;
                        MaxIndex = i;
                    }
                }

                return Chromosomes[MaxIndex];
            }
        }
        

        //public Chromosome GetTheWorstChromosome()
        //{
        //    int MinValue = Int32.MaxValue;
        //    int MinIndex = -1;
        //    for (int i = 0; i < Chromosomes.Count; i++)
        //    {
        //        int currentValue = Chromosomes[i].GetValue;
        //        if (MinValue >= currentValue)
        //        {
        //            MinValue = currentValue;
        //            MinIndex = i;
        //        }
        //    }

        //    return Chromosomes[MinIndex];
        //}

        public Chromosome GetTheWorstChromosome
        {
            get
            {
                int MinValue = Int32.MaxValue;
                int MinIndex = -1;
                for (int i = 0; i < Chromosomes.Count; i++)
                {
                    int currentValue = Chromosomes[i].GetValue;
                    if (MinValue >= currentValue)
                    {
                        MinValue = currentValue;
                        MinIndex = i;
                    }
                }

                return Chromosomes[MinIndex];
            }
        }
        

    }
}
