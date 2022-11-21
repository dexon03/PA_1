using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    internal static class Solver
    {
        public static BackPack backPack = new BackPack();
        static Population population = new Population(backPack);
        static Chromosome Record;

        public static void Solve()
        {
            population.GeneratePopulation();
            Record = population.GetTheWorstChromosome;
            int iterations = 0;
            while (iterations <= 1000)
            {
                Chromosome FirstParent = population.GetBestChromosome;
                Chromosome SecondParent;
                while (true)
                {
                    Random random  = new Random();
                    int index = random.Next(0, 100);
                    if (population.Chromosomes[index] != FirstParent)
                    {
                        SecondParent = population.Chromosomes[index];
                        break;
                    }
                }
                Chromosome Offspring = CrossOver(FirstParent, SecondParent);
                Chromosome mutation = Mutation(Offspring);
                Chromosome improved = LocalImprovement(mutation);
                if(improved.GetValue > Record.GetValue)
                {
                    Record= improved;
                }
                Chromosome worstChromo = population.GetTheWorstChromosome;
                if (improved.GetWeight <= backPack.Capacity)
                {
                    
                    population.Chromosomes.Remove(worstChromo);
                    population.Chromosomes.Add(improved);
                }
                else
                {
                    if(Offspring.GetWeight <= backPack.Capacity)
                    {
                        population.Chromosomes.Remove(worstChromo);
                        population.Chromosomes.Add(Offspring);
                    }
                    else
                    {
                        continue;
                    }
                }
                if(iterations%20==0)
                {
                    WriteRecordToFile(iterations, Record.GetValue);
                }
                iterations++;
            }
            for (int i = 0; i < population.Chromosomes.Count; i++)
            {
                Console.WriteLine("Chomosome: " + i + "  Weight: " + population.Chromosomes[i].GetWeight + "  Value: " + population.Chromosomes[i].GetValue);
            }
        }

        private static Chromosome CrossOver(Chromosome FirstParent,Chromosome SecondParent) 
        {
            List<int> mergedList = new List<int>(100);
            int flag = 1;
            for (int i = 0; i < 100; i++)
            {
                if (i % 25 == 0 && i!= 0) flag *= -1;
                if(flag > 0)
                {
                    mergedList.Add(FirstParent.Gene[i]);
                }
                else
                {
                    mergedList.Add(SecondParent.Gene[i]);
                }

            }
            Chromosome Offspring = new Chromosome(mergedList);
            return Offspring;
        }

        private static Chromosome Mutation(Chromosome chromosome)
        {
            Random random = new Random();
            int chance = random.Next(1, 101);
            if(chance <= 5)
            {
                int index_1 = random.Next(0, chromosome.Gene.Count);
                int index_2;
                while (true)
                {
                    index_2 = random.Next(0, chromosome.Gene.Count);
                    if (index_2 != index_1) break;
                }
                (chromosome.Gene[index_1], chromosome.Gene[index_2]) = (chromosome.Gene[index_2], chromosome.Gene[index_1]);
            }
            return chromosome;
        }

        private static Chromosome LocalImprovement(Chromosome chromosome)
        {
            Random random = new Random();
            while (true)
            {
                int index = random.Next(0,chromosome.Gene.Count);
                if (chromosome.Gene[index] == 0)
                {
                    chromosome.Gene[index] = 1;
                    break;
                }
            }
            return chromosome;
        }
        private static void WriteRecordToFile(int iteration,int value)
        {
            using(var stream = new StreamWriter("records.txt", true))
            {
                stream.WriteLine(value);
            }
        }
    }
}
