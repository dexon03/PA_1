namespace Lab1;

public static class Functions
{
    public static void Generate10MbFile(string path, int size)
    {
        Random random = new Random();
        Queue<long> firstArray = new Queue<long>();
        for (int i = 0; i < size; i++)
        {
            firstArray.Enqueue(random.NextInt64(1,1000000));
        }

        using BinaryWriter binaryWriter = new BinaryWriter(File.Open(@"10mbFolder\" + Constants.path10MbFile, FileMode.Create));
        foreach (var ele in firstArray)
        {
            binaryWriter.Write(ele);
        }
    }

    public static void Generate16GbFile(string path, long size)
    {
        Random random = new Random();
        Queue<long> firstArray = new Queue<long>();
        for (long i = 0; i < size; i++)
        {
            firstArray.Enqueue(random.NextInt64(1,Int64.MaxValue));
        }

        using BinaryWriter binaryWriter = new BinaryWriter(File.Open(@"16GbFolder\" + Constants.path16GbFile, FileMode.Create));
        foreach (var ele in firstArray)
        {
            binaryWriter.Write(ele);
        }
        firstArray.Clear();
    }

    public static Queue<long> ReadFromInputFile(string path)
    {
        Queue<long> array = new Queue<long>();

        using (BinaryReader binaryReader = new BinaryReader(File.Open(@"10mbFolder\" + Constants.path10MbFile, FileMode.Open)) )
        {
            while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
            {
                array.Enqueue(binaryReader.ReadInt64());
            }
        }

        return array;
    }
    
    public static List<Queue<long>> Distribution(Queue<long> arr,int n)
    {
        List<Queue<long>> result = new List<Queue<long>>(n);
        for (int j = 0; j < n; j++)
        {
            result.Add(new Queue<long>());
        }
        int i = 1;
        foreach (var elem  in arr)
        {
            result[i-1].Enqueue(elem);
            i = i % n + 1;
        }

        return result;
    }
    public static List<string> CreateFiles(List<Queue<long>> arr, int n, string path)
    {
        List<string> fileNames = new List<string>();
        for (int i = 0; i < n; i++)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(path + $"B{i+1}.bin", FileMode.Create)) )
            {
                foreach (var ele in arr[i])
                {
                    binaryWriter.Write(ele);
                }
                fileNames.Add($"B{i+1}.bin");
            }
        }

        for (int i = 0; i < n; i++)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(path + $"C{i+1}.bin", FileMode.Create)) )
            {
                fileNames.Add($"C{i+1}.bin");
            }
        }

        return fileNames;
    }

    public static  void ClearFiles(int check,int n, string path)
    {
        string fileName;
        if (check > 0)
        {
            fileName = "B";
        }
        else
        {
            fileName = "C";
        } 
        for (int i = 0; i < n; i++)
        {
            FileStream fileStream = File.Open(path + $"{fileName}{i+1}.bin", FileMode.Open);
            fileStream.SetLength(0);
            fileStream.Close();
        }
    }
}