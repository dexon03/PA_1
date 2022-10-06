using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Lab1;
using static System.GC;

//
// string path10Mb = @"10mbFolder\";
// string path16Gb = @"16GbFolder\";
// // Generate10MbFile(path10Mb,Constants.size10MbFile);
// Generate16GbFile(path16Gb);
// // show size of file
// FileInfo fi = new FileInfo( path16Gb + Constants.path16GbFile);
// Console.WriteLine($"File size: {fi.Length/1000000}mb");
// //
// //
// // // read array from A file
// // Queue<long> inputArr = ReadFromInputFile(path10Mb + Constants.path10MbFile);
// // // distribution of series 
// // int n = (int)Math.Log10(inputArr.Count); // count of series
// //
// //
// // List<Queue<long>> distributeArray = Distribution(inputArr, n); 
// // List<string> fileNames = CreateFiles(distributeArray,n,path10Mb);
// // DateTime data1 = DateTime.Now;
// // SimpleMultiwayMergeSort(fileNames,n,path10Mb);
// // DateTime data2 = DateTime.Now;
//
// // Read values from final file and show count of elements
// /*int count = 0;
// foreach (var file in fileNames)
// {
//     int countOfElements = 0;
//     using (BinaryReader binaryReader = new BinaryReader(File.Open(path10Mb + file, FileMode.Open)) )
//     {
//         if (binaryReader.BaseStream.Length != 0)
//         {
//             count++;
//             Console.WriteLine("number:" + count);
//             while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
//             {
//                 countOfElements++;
//                 Console.WriteLine(binaryReader.ReadInt64());
//             }
//
//             Console.WriteLine("Count of elements:" + countOfElements);
//         }
//     }
// }*/
// // Console.WriteLine("Sorted:" + (data2 - data1));
//
//
//
// void SimpleMultiwayMergeSort(List<string> fileList, int n, string path)
// {
//     int check = 1;
//     while (true)
//     {
//         FileInfo[] files = {
//             new FileInfo(path + fileList[0]),
//             new FileInfo(path + fileList[n])
//         };
//         if (fi.Length == files[0].Length || fi.Length == files[1].Length) break;
//
//         List<Queue<long>> listsFromFiles = readFile(check, n,fileList);
//         // if (inputArr.Count == listsFromFiles[0].Count) break;
//         
//         Merge(check,n,path,listsFromFiles);
//         check *= -1;
//     }
// }
//
// void Merge(int check,int n,string path, List<Queue<long>> list)
// {
//     List<long> series = new List<long>();
//     string fileName;
//     if (check > 0)
//     {
//         fileName = "C";
//     }
//     else fileName = "B";
//     
//     int fileNumber = 1;
//     while (!isEmpty(list))
//     {
//         // foreach (var queue in list)
//         // {
//         //     if (queue.Count != 0)
//         //     {
//         //         long temp = queue.Dequeue();
//         //         series.Add(temp);
//         //         while(queue.Count!=0 && queue.Peek()>series[series.Count-1]) series.Add(queue.Dequeue());
//         //     }
//         // }
//         // using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(path +$"{fileName}{fileNumber}.bin",FileMode.Append)) )
//         // {
//         //         foreach (var ele in series)
//         //         {
//         //             binaryWriter.Write(ele);
//         //         }
//         // }
//         // fileNumber = fileNumber % n + 1;
//         // series.Clear();
//         var minValue = long.MaxValue;
//         int? minIndex = null;
//         //searching minimal element from series
//         for (int i = 0; i < list.Count; i++)
//         {
//             if (list[i].Count != 0)
//             {
//                 long tmp = list[i].Peek();
//                 if (series.Count == 0 || tmp >= series.Last()) // check if series is empty, then first minimal will be added to series else check if current element is bigger than last elements in series and lower than current minimal
//                 {
//                     if (tmp <= minValue)
//                     {
//                         minValue = tmp;
//                         minIndex = i;
//                     }
//                 }
//             }   
//         }
//         //if minimal isn't found, than write series to file and clear it
//         if (minIndex == null)
//         {
//             using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(path +$"{fileName}{fileNumber}.bin",FileMode.Append)) )
//             {
//                 foreach (var ele in series)
//                 { 
//                     binaryWriter.Write(ele);
//                 }
//             }
//             fileNumber = fileNumber % n + 1;
//             series.Clear();
//         }
//         //else add minimum fo series
//         else
//         {
//             series.Add(list[(int)minIndex].Dequeue());
//         }
//     }
//     // add to file last found series
//     using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(path +$"{fileName}{fileNumber}.bin",FileMode.Append)) )
//     {
//         foreach (var ele in series)
//         { 
//             binaryWriter.Write(ele);
//         }
//     }
//     //clear input files
//     ClearFiles(check,n,path);
// }
//
//
// List<Queue<long>> readFile(int check, int n, List<string> fileList)
// {
//     List<Queue<long>> listsFromFiles = new List<Queue<long>>(n);
//     if (check>0)
//     {
//         for (int i = 0; i < n; i++)
//         {
//             using (BinaryReader binaryReader = new BinaryReader(File.Open(path10Mb + $"{fileList[i]}", FileMode.Open)) )
//             {
//                 if (binaryReader.BaseStream.Length != 0)
//                 {
//                     var tmpQueue = new Queue<long>();
//                     while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
//                     {
//                         tmpQueue.Enqueue(binaryReader.ReadInt64());
//                     }
//                     listsFromFiles.Add(tmpQueue);
//                 }
//             }
//         }
//     }
//     else
//     {
//         for (int i = n; i < 2*n; i++)
//         {
//             using (BinaryReader binaryReader = new BinaryReader(File.Open(path10Mb + $"{fileList[i]}", FileMode.Open)) )
//             {
//                 if (binaryReader.BaseStream.Length != 0)
//                 {
//                     var tmpQueue = new Queue<long>();
//                     while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
//                     {
//                         tmpQueue.Enqueue(binaryReader.ReadInt64());
//                     }
//                     listsFromFiles.Add(tmpQueue);
//                 }
//             }
//         }
//     }
//     return listsFromFiles;
// }
//
//
// // bool IsSorted(BinaryReader fileA,BinaryReader fileB, BinaryReader fileC)
// // {
// //     if (fileA.BaseStream.Length == fileB.BaseStream.Length || fileA.BaseStream.Length == fileC.BaseStream.Length)
// //     {
// //         return true;
// //     }
// //
// //     return false;
// // }
//
// bool isEmpty(List<Queue<long>> list)
// {
//     int count = 0;
//     foreach (var queue in list)
//     {
//         count += queue.Count;
//     }
//     if (count != 0)
//     {
//         return false;
//     }
//     return true;
// }


// simple sort
// var multiwaySort = new SimpleMultiwaySort(Constants.PathTo10MbFodler,Constants.size10MbFile);
// multiwaySort.GenerateFile();
// int chunk = (int)Math.Log10(Constants.size10MbFile);
// multiwaySort.SplitFile(chunk);
// DateTime data1 = DateTime.Now;
// multiwaySort.Sort(chunk);
// DateTime data2 = DateTime.Now;
// multiwaySort.OutPut();
// Console.WriteLine($"Sorted in: {data2-data1}");

