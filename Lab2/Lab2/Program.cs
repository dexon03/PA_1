using System.Diagnostics;
using Lab2;

// int[,] test = new int[,] { { 9, 2, 3 }, 
//                            { 1, 4, 5 }, 
//                            { 7, 8, 6 } };
Board board = new Board();
board.GenerateBoard();
board.OutPut();
Console.WriteLine("--------------------------");
var beginState = new State(board,null,null,0);
Console.Write("Choose algorithm 1 - LDFS, 2 - A* :");
int var;
do
{
    var = int.Parse(Console.ReadLine());
    if (var == 1 || var == 2) break;
} while (true);

if (var == 1)
{
    Console.Write("Choose limit for LDFS: ");
    int limit = Int32.Parse(Console.ReadLine());
    
    State? state = LDFS.Solve(beginState,limit);
    if (state != null)
    {
        state.Board.OutPut();
    }
    else
    {
        Console.WriteLine("Cutoff/failure");
    }
}
else
{ 
    State? state = AStar.Solve(beginState);
    if (state != null)
    {
        state.Board.OutPut();
    }
    else
    {
        Console.Write("Not found");
    }
}
// State? state = AStar.Solve(beginState);
// if (state != null)
// {
//     state.Board.OutPut();
// }
//
// Console.WriteLine("null");
// State? state = LDFS.Solve(beginState,22);
// if (state != null)
// {
//     state.Board.OutPut();
// }
//
// else
// {
//     Console.WriteLine("Cutoff/failure");
// }
// if (state.cutoff)
// {
//     Console.WriteLine("Cutoff");
// }else if (state.failure)
// {
//     Console.WriteLine("Failure");
// }
// else
// {
//     state.State.Board.OutPut();
// }

// LDFS.Search(beginState,100);