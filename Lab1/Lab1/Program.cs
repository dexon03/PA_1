using Lab1;
using Microsoft.VisualBasic;

// var sizeFirst = Constants.size10MbFile;
// var secondSize = Constants.size16GbFile;
//
// Random random = new Random();
//
// List<long> secondArray = new List<long>();
//
// for (int i = 0; i < secondSize; i++)
// {
//     secondArray.Add(random.NextInt64(1,Int64.MaxValue));
// }
//
// using (StreamWriter streamWriter = new StreamWriter(Constants.path16GbFile, false))
// {
//     foreach (var item in secondArray)
//     {
//         streamWriter.WriteLine(item);
//     }
// }
//
// FileInfo fi = new FileInfo(Constants.path16GbFile);
// Console.WriteLine($"{fi.Length/1000000}");

List<long> array = new List<long>();

// read array from file

string path = @"10mbFolder\";
using (StreamReader streamReader = new StreamReader(path + "A.txt"))
{
    string? current;
    while ((current = streamReader.ReadLine()) != null)
    {
        array.Add(Int64.Parse(current));
    }
}
// distribution of series 
int n = 4;
List<List<long>> distribArray = Distribution(array, n);
CreateFiles(distribArray,n,path);





List<List<long>> Distribution(List<long> arr,int n)
{
    List<List<long>> result = new List<List<long>>(n);
    for (int j = 0; j < n; j++)
    {
        result.Add(new List<long>());
    }
    int i = 1;
    foreach (var elem  in arr)
    {
        result[i-1].Add(elem);
        i = i % n + 1;
    }

    return result;
}

void CreateFiles(List<List<long>> arr, int n, string path)
{
    for (int i = 0; i < n; i++)
    {
        using (StreamWriter streamWriter = new StreamWriter(path + $"B{i + 1}.txt"))
        {
            streamWriter.Write(String.Join("\n",arr[i]));
        }
    }
}

