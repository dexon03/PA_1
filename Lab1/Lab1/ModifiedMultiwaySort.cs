namespace Lab1;

public class ModifiedMultiwaySort
{
    public string PathToAFolder { get; }
    public int Size { get; set; }
    public List<string> Filelist { get; set; }
    
    public ModifiedMultiwaySort(string path, int size)
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
            binaryWriter.Write(random.NextInt64(1,Int64.MaxValue));
        }
    }

    public void Sort()
    {
        
    }
}