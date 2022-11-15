namespace UnitTests;
using Lab2;

public class Tests
{

    //LimitDepth = 22;
    [Test]
    public void TestNegativeResultLDFS()
    {
        // Arrange
        int[,] arr = new int[,]
        {
            {5, 1, 4},
            {7, 9, 8},
            {2, 3, 6},
        };
        Board board = new Board(arr);
        var beginState = new State(board,null,null,0);
        int iterations;
        int angles;
        int countOfState;
        int stateInMemory;
        //Action
        State? state = LDFS.Solve(beginState,22,out iterations,out angles,out countOfState);
        //Assert
        Assert.AreEqual(state, null);
    }
    //LimitDepth = 30;
    [Test]
    public void TestPositiveResultLDFS()
    {
        // Arrange
        int[,] arr = new int[,]
         {
            {3, 6, 8},
            {5, 4, 1},
            {9, 2, 7},
         };
        Board board = new Board(arr);
        var beginState = new State(board, null, null, 0);
        int iterations;
        int angles;
        int countOfState;
        int stateInMemory;
        //Action
        State? state = LDFS.Solve(beginState, 30, out iterations, out angles, out countOfState);
        //Assert
        Assert.AreEqual(state.Board.Matrix, FunctionsAndConstants.goalState);
    }
    [Test]
    public void TestNegativeResultAStar()
    {
        // Arrange
        int[,] arr = new int[,]
        {
            {7, 2, 5},
            {9, 1, 4},
            {6, 8, 3},
        };
        Board board = new Board(arr);
        var beginState = new State(board, null, null, 0);
        int iterations;
        int angles;
        int countOfState;
        int stateInMemory;
        //Action
        State? state = AStar.Solve(beginState, out iterations, out angles, out countOfState,out stateInMemory);
        //Assert
        Assert.AreEqual(state, null);
    }
    [Test]
    public void TestPositiveResultAStar()
    {
        //Arrange 
        int[,] arr = new int[,]
        {
            {5, 1, 3},
            {6, 9, 8},
            {2, 4, 7},
        };
        Board board = new Board(arr);
        //board.GenerateBoard();
        var beginState = new State(board, null, null, 0);
        int iterations;
        int angles;
        int countOfState;
        int stateInMemory;
        //Action
        State? state = AStar.Solve(beginState, out iterations, out angles, out countOfState, out stateInMemory);
        //Assert
        Assert.AreEqual(state.Board.Matrix, FunctionsAndConstants.goalState);

    }
}