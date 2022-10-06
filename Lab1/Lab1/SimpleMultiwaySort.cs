namespace Lab1;

public class SimpleMultiwaySort
{
    public string PathToAFolder { get; }
    public int Size { get; set; }
    public List<string> Filelist { get; set; }
    

    public SimpleMultiwaySort(string path, int size)
    {
        PathToAFolder = path;
        Size = size;
        Filelist = new List<string>();
    }
    public void GenerateFile()
    {
        Random random = new Random();
        using BinaryWriter binaryWriter = new BinaryWriter(File.Open(PathToAFolder + "A.bin" , FileMode.Create));
        for (int i = 0; i < Size; i++)
        {
            binaryWriter.Write(random.NextInt64(1,1000000));
        }
    }

    private Queue<long> ReadFromInputFile()
    {
        Queue<long> arrFromInputFile = new Queue<long>();
        using (BinaryReader binaryReader = new BinaryReader(File.Open(PathToAFolder + "A.bin", FileMode.Open)))
        {
            while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
            {
                arrFromInputFile.Enqueue(binaryReader.ReadInt64());
            }
        }

        return arrFromInputFile;
    }
    public void SplitFile(int n)
    {
        Queue<long> arrFromInputFile = ReadFromInputFile();
        List<Queue<long>> listInFiles = new List<Queue<long>>(n);
        for (int i = 0; i < n; i++)
        {
            listInFiles.Add(new Queue<long>());
        }
        int j = 1;
        foreach (var elem  in arrFromInputFile)
        {
            listInFiles[j-1].Enqueue(elem);
            j = j % n + 1;
        }
        WriteInOutputFiles(listInFiles,n);
        
    }

    private void WriteInOutputFiles(List<Queue<long>> arr, int n)
    {
        for (int i = 0; i < n; i++)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(PathToAFolder + $"B{i+1}.bin", FileMode.Create)) )
            {
                foreach (var ele in arr[i])
                {
                    binaryWriter.Write(ele);
                }
                Filelist.Add($"B{i+1}.bin");
            }
        }

        for (int i = 0; i < n; i++)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(PathToAFolder + $"C{i+1}.bin", FileMode.Create)) )
            {
                Filelist.Add($"C{i+1}.bin");
            }
        }
    }
    
    private List<Queue<long>> ReadFiles(int check,int n)
    {
        List<Queue<long>> listsFromFiles = new List<Queue<long>>(n);
        if (check>0)
        {
            for (int i = 0; i < n; i++)
            {
                using (BinaryReader binaryReader = new BinaryReader(File.Open(PathToAFolder + $"{Filelist[i]}", FileMode.Open)) )
                {
                    if (binaryReader.BaseStream.Length != 0)
                    {
                        var tmpQueue = new Queue<long>();
                        while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                        {
                            tmpQueue.Enqueue(binaryReader.ReadInt64());
                        }
                        listsFromFiles.Add(tmpQueue);
                    }
                }
            }
        }
        else
        {
            for (int i = n; i < 2*n; i++)
            {
                using (BinaryReader binaryReader = new BinaryReader(File.Open(PathToAFolder + $"{Filelist[i]}", FileMode.Open)) )
                {
                    if (binaryReader.BaseStream.Length != 0)
                    {
                        var tmpQueue = new Queue<long>();
                        while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                        {
                            tmpQueue.Enqueue(binaryReader.ReadInt64());
                        }
                        listsFromFiles.Add(tmpQueue);
                    }
                }
            }
        }
        return listsFromFiles;
    }

    public void Sort(int n)
    {
        int check = 1;
        while (true)
        {
            FileInfo[] files = {
                new FileInfo(PathToAFolder + Filelist[0]),
                new FileInfo(PathToAFolder + Filelist[n])
            };
            // ReSharper disable once ComplexConditionExpression
            if (Size*8 == files[0].Length || Size*8 == files[1].Length) break;

            List<Queue<long>> listsFromFiles = ReadFiles(check,n);
            // if (inputArr.Count == listsFromFiles[0].Count) break;
        
            Merge(check,n,listsFromFiles);
            check *= -1;
        }
    }

    private void Merge(int check,int n, List<Queue<long>> list)
    {
        List<long> series = new List<long>();
        string fileName;
        if (check > 0)
        {
            fileName = "C";
        }
        else fileName = "B";
        int fileNumber = 1;
        while (!isEmpty(list))
        {
            var minValue = long.MaxValue;
            int? minIndex = null;
            //searching minimal element from series
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Count != 0)
                {
                    long tmp = list[i].Peek();
                    if (series.Count == 0 || tmp >= series.Last()) // check if series is empty, then first minimal will be added to series else check if current element is bigger than last elements in series and lower than current minimal
                    {
                        if (tmp <= minValue)
                        {
                            minValue = tmp;
                            minIndex = i;
                        }
                    }
                }   
            }
            //if minimal isn't found, than write series to file and clear it
            if (minIndex == null)
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(PathToAFolder +$"{fileName}{fileNumber}.bin",FileMode.Append)) )
                {
                    foreach (var ele in series)
                    { 
                        binaryWriter.Write(ele);
                    }
                }
                fileNumber = fileNumber % n + 1;
                series.Clear();
            }
            //else add minimum to series
            else
            {
                series.Add(list[(int)minIndex].Dequeue());
            }
        }
        // add to file last found series
        using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(PathToAFolder +$"{fileName}{fileNumber}.bin",FileMode.Append)) )
        {
            foreach (var ele in series)
            { 
                binaryWriter.Write(ele);
            }
        }
        //clear input files
        ClearFiles(check,n);
    }
    private void ClearFiles(int check,int n)
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
            FileStream fileStream = File.Open(PathToAFolder + $"{fileName}{i+1}.bin", FileMode.Open);
            fileStream.SetLength(0);
            fileStream.Close();
        }
    }
    private bool isEmpty(List<Queue<long>> list)
    {
        int count = 0;
        foreach (var queue in list)
        {
            count += queue.Count;
        }
        if (count != 0)
        {
            return false;
        }
        return true;
    }

    public void OutPut()
    {
        foreach (var file in Filelist)
        {
            int countOfElements = 0;
            using (BinaryReader binaryReader = new BinaryReader(File.Open(PathToAFolder + file, FileMode.Open)))
            {
                if (binaryReader.BaseStream.Length != 0)
                {
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        countOfElements++;
                        Console.WriteLine(binaryReader.ReadInt64());
                    }

                    Console.WriteLine("Count of elements:" + countOfElements);
                }
            }

        }
    }
}