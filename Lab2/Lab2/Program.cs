using Lab2;

// int[,] test = new int[,] { { 9, 2, 3 }, 
//                            { 1, 4, 5 }, 
//                            { 7, 8, 6 } };
Board board = new Board();
board.GenerateBoard();
board.OutPut();
Console.WriteLine("--------------------------");
var beginState = new State(board,null,null,0);
int iterations;
int angles;
int countOfState;
int stateInMemory;

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
    
    State? state = LDFS.Solve(beginState,limit,out iterations,out angles,out countOfState);
    if (state != null)
    {
        state.Board.OutPut();
        var path = FunctionsAndConstants.FindPath(state);
        Console.WriteLine("Count of states in memory: " + path.Count);
    }
    else
    {
        Console.WriteLine("Cutoff/failure");
    }
    Console.WriteLine("Count of iterations: " + iterations);
    Console.WriteLine("Count of angles: " + angles);
    Console.WriteLine("Count of states: " + countOfState);
}
else
{ 
    State? state = AStar.Solve(beginState,out iterations,out angles,out countOfState,out stateInMemory);
    if (state != null)
    {
        state.Board.OutPut();
    }
    else
    {
        Console.Write("Not found");
    }
    Console.WriteLine("Count of states in memory: " + stateInMemory);
    Console.WriteLine("Count of iterations: " + iterations);
    Console.WriteLine("Count of angles: " + angles);
    Console.WriteLine("Count of states: " + countOfState);
}