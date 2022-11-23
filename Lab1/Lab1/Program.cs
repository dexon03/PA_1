using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Lab1;
using static System.GC;
using static Lab1.Constants;



// simple sort
var multiwaySort = new SimpleMultiwaySort(Constants.PathTo10MbFolder,Constants.size10MbFile);
multiwaySort.GenerateFile();
int chunk = (int)Math.Log10(Constants.size10MbFile);
multiwaySort.SplitFile(chunk);
DateTime data1 = DateTime.Now;
multiwaySort.Sort(chunk);
DateTime data2 = DateTime.Now;
multiwaySort.OutPut();
Console.WriteLine($"Sorted in: {data2-data1}");




// int n = (int)(size16GbFile / 1000000000 / RAM / Math.Log2(RAM));

// int n = 16;
// var modSort = new ModifiedMultiwaySort(PathTo16GbFolder,size16GbFile);
// // modSort.GenerateFile();
// DateTime data1 = DateTime.Now;
// modSort.Sort(n);
// DateTime date2 =DateTime.Now;
// modSort.OutPut();
// Console.WriteLine($"Sorted in: {date2-data1}");

