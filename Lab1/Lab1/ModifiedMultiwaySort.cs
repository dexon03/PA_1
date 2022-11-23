using System.IO.MemoryMappedFiles;

namespace Lab1;

public class ModifiedMultiwaySort
{
    public string PathToAFolder { get; }
    public long Size { get; set; }
    public List<string> Filelist { get; set; }
    
    public ModifiedMultiwaySort(string path, long size)
    {
        PathToAFolder = path;
        Size = size;
        Filelist = new List<string>();
    }
    public void GenerateFile()
    {
        Random random = new Random();
        using BinaryWriter binaryWriter = new BinaryWriter(File.Open(PathToAFolder + "A.bin" , FileMode.Create));
        for (int i = 0; i < Size/8; i++)
        {
            binaryWriter.Write(random.NextInt64(1,Int64.MaxValue));
        }
    }

    private void CreateFilesForMerge(int n)
    {
        for (int i = 0; i < n; i++)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(PathToAFolder + $"B{i + 1}.bin", FileMode.Create)))
            {
            }
            Filelist.Add($"B{i + 1}.bin");
        }
        // for (int i = 0; i < n; i++)
        // {
        //     using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(PathToAFolder + $"С{i + 1}.bin", FileMode.Create)))
        //     {
        //     }
        //     Filelist.Add($"С{i + 1}.bin");
        // }
        using (BinaryWriter binaryWriter = new BinaryWriter(File.Create(PathToAFolder + "C.bin")))
        {
        }
    }

    public void Sort(int n)
    {
        CreateFilesForMerge(n);
        SortSets(n);
        int check = 1;
        BinaryReader[] readers = new BinaryReader[n];
        // while (!isSorted(n))
        // {
        //     
            for (int i = 0; i < n; i++)
            {
                readers[i] = new BinaryReader(File.Open(PathToAFolder + Filelist[i], FileMode.Open));
            }

        //     check *= -1;
        // }
        Merge(n,readers);
        
    }

    public void SortSets(int n)
    {
        long start = 0;
        long setSize = Size / n;
        using (var mmf = MemoryMappedFile.CreateFromFile(PathToAFolder + "A.bin"))
        {
            int j = 1;
            while (start<Size)
            {
                setSize = Math.Min(setSize, Size - start);
                using (var ms = mmf.CreateViewAccessor(start,setSize,MemoryMappedFileAccess.ReadWrite))
                {
                    long[] list = new long[(int)setSize / 8];
                    ms.ReadArray(0, list, 0, list.Length);
                    // DateTime data1 = DateTime.Now;
                    Array.Sort(list);
                    // DateTime data2 = DateTime.Now;
                    // Console.WriteLine($"{j} sorted in {data2 - data1}");
                    // data1 = DateTime.Now;
                    using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(PathToAFolder + $"B{j}.bin",FileMode.Append)))
                    {
                        foreach (var ele in list)
                        {
                            binaryWriter.Write(ele);
                        }
                    }
                    // data2 = DateTime.Now;
                    // Console.WriteLine($"Arr j written ing file in: {data2-data1}");
                }

                j = j % n + 1;
                start += setSize;
            }
        }
    }

    private void Merge(int n, BinaryReader[] readers)
    {
        using BinaryWriter writer = new BinaryWriter(File.Open(PathToAFolder + "C.bin", FileMode.Append));
        long[] position = new long[n];
        int move = 8;
        long maxPosition = Size/n;
        // long lastElement;
        // int tmpIndex;
        // (lastElement, tmpIndex) = FirstMinimum(n);
        // position[tmpIndex]++;
        List<long> set = new List<long>();
        while (!isMerged(position,maxPosition))
        {
            if (set.Count >= Size / n / 8)
            {
                long tmpValue = set.Last();
                foreach (var value in set)
                {
                    writer.Write(value);
                }
                set.Clear();
                set.Add(tmpValue);
            }
            // using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(PathToAFolder + "C.bin", FileMode.Append)))
            // {
            //     long minValue = long.MaxValue;
            //     int? minIndex = null;
            //     for (int i = 0; i < n; i++)
            //     {
            //         if(position[i]*move >= maxPosition) continue;
            //         // using (BinaryReader binaryReader = new BinaryReader(File.Open(PathToAFolder + Filelist[i],FileMode.Open)))
            //         // {
            //             readers[i].BaseStream.Position = move * position[i];
            //             var value = readers[i].ReadInt64();
            //             if (value <= minValue && value >= lastElement)
            //             {
            //                 minValue = value;
            //                 minIndex = i;
            //             }
            //
            //         // }
            //     }
            //
            //     binaryWriter.Write(minValue);
            //     lastElement = minValue;
            //     position[(int)minIndex]++; 
            // }
            int minIndex = FindMinimum(readers, position, set);
            set.Add(readers[minIndex].ReadInt64());
            position[minIndex]++;

        }
        foreach (var value in set)
        {
            writer.Write(value);
        }
        set.Clear();

        for (int i = 0; i < n; i++)
        {
            readers[i].Close();
        }
        
    }

    private int FindMinimum(BinaryReader[] readers,long[] positions,List<long> set)
    {
        // long minValue = long.MaxValue;
        // int? minIndex = null;
        // for (int i = 0; i < n; i++)
        // {
        //     using (BinaryReader binaryReader = new BinaryReader(File.Open(PathToAFolder + Filelist[i], FileMode.Open)))
        //     {
        //         var value = binaryReader.ReadInt64();
        //         if (value <= minValue)
        //         {
        //             minValue = value;
        //             minIndex = i;
        //         }
        //     }
        // }
        //
        // using (BinaryWriter binaryWriter = new BinaryWriter(File.Create(PathToAFolder + "C.bin")))
        // {
        //     binaryWriter.Write(minValue);
        // }
        //
        // return (minValue, (int)minIndex);
        long minValue = Int64.MaxValue;
        int? minIndex = null;
        for (int i = 0; i < readers.Length; i++)
        {
            if (positions[i] < readers[i].BaseStream.Length/8)
            {
                long tmpValue = readers[i].ReadInt64();
                if (set.Count == 0 || tmpValue >= set.Last())
                {
                    if (tmpValue <= minValue)
                    {
                        minValue = tmpValue;
                        minIndex = i;
                    }
                }
            }
        }

        for (int i = 0; i < readers.Length; i++)
        {
            readers[i].BaseStream.Position -= 8;
        }
        return (int)minIndex;
    }
    private bool isSorted(int n)
    {
        FileInfo[] files = {
            new FileInfo(PathToAFolder + Filelist[0]),
            new FileInfo(PathToAFolder + Filelist[n])
        };
        // ReSharper disable once ComplexConditionExpression
        if (Size == files[0].Length || Size == files[1].Length) return true;
        // FileInfo fi = new FileInfo(PathToAFolder + "C.bin");
        // if (fi.Length == Size) return true;
        return false;
    }

    private bool isMerged(long[] positions, long maxPosition)
    {
        long count = 0;
        foreach (var ele in positions)
        {
            if (ele >= maxPosition / 8) count++;
        }

        if (count == positions.Length) return true;
        return false;
    }

    public void OutPut()
    {
        using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(PathToAFolder + "C.bin")))
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(binaryReader.ReadInt64());
            }

            binaryReader.BaseStream.Position = binaryReader.BaseStream.Length - 8000;
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(binaryReader.ReadInt64());
            }
        }
    }
}