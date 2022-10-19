namespace Lab2;

public class State : IComparable<State>
{
    private int outOfPlace;
    public int F;
    public Board Board { get; set; }
    public State Parent { get; set; }
    public string LastMove { get; set; }
    public int SearchDepth { get; set; }

    public State(Board board,State parent,string lastMove,int searchDepth)
    {
        this.Board = board;
        this.Parent = parent;
        this.LastMove = lastMove;
        this.SearchDepth = searchDepth;
        // this.outOfPlace = OutOfPlace();
        this.F = GetF();
    }

    public int OutOfPlace()
    {
        var board = Board.Matrix;
       
        // if (board[0, 0] != 1) 
        // { 
        //     outOfPlace++;
        // }if (board[0, 1] != 2)
        // { 
        //     outOfPlace++;
        // } 
        // if (board[0, 2] != 3) 
        // { 
        //     outOfPlace++;
        // } 
        // if (board[1, 0] != 4) 
        // { 
        //     outOfPlace++;
        // } 
        // if (board[1, 1] != 5) 
        // { 
        //     outOfPlace++;
        // }
        // if (board[1, 2] != 6)
        // {
        //     outOfPlace++; 
        // }if (board[2, 0] != 7)
        // { 
        //     outOfPlace++;
        // } 
        // if (board[2, 1] != 8) 
        // { 
        //     outOfPlace++;
        // }
        int counter = 1;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (counter == 9) break;
                if (board[i, j] != counter)
                {
                    this.outOfPlace++;
                }

                counter++;
            }
        }
        return this.outOfPlace;
    }

    public int GetF()
    {
        this.outOfPlace = OutOfPlace();
        return this.SearchDepth + this.outOfPlace;
    }
    public State MoveBlankToLeft(int i,int j) 
    {
        if (j == 0)
        {
            return null;
        }

        if (this.LastMove == "right") return null;
        var newState = this.Clone();
        (newState.Board.Matrix[i, j - 1], newState.Board.Matrix[i, j]) = (newState.Board.Matrix[i, j], newState.Board.Matrix[i, j - 1]);
        newState.F = GetF();
        return newState;
    }
    public State MoveBlankToRight(int i,int j) 
    {
        if (j == 2)
        {
            return null;
        }
        if (this.LastMove == "left") return null;
        var newState = this.Clone();
        (newState.Board.Matrix[i, j + 1], newState.Board.Matrix[i, j]) = (newState.Board.Matrix[i, j], newState.Board.Matrix[i, j + 1]);
        newState.F = GetF();
        return newState;
    }
    public State MoveBlankToUp(int i,int j) 
    {
        if (i == 0)
        {
            return null;
        }
        if (this.LastMove == "down") return null;
        var newState = this.Clone();
        (newState.Board.Matrix[i-1, j], newState.Board.Matrix[i, j]) = (newState.Board.Matrix[i, j], newState.Board.Matrix[i-1, j]);
        newState.F = GetF();
        return newState;
    }
    public State MoveBlankToDown(int i,int j) 
    {
        if (i == 2)
        {
            return null;
        }
        if (this.LastMove == "up") return null;
        var newState = this.Clone();
        (newState.Board.Matrix[i+1, j], newState.Board.Matrix[i, j]) = (newState.Board.Matrix[i, j], newState.Board.Matrix[i+1, j]);
        newState.F = GetF();
        return newState;
    }

    public State Clone()
    {
        int[,] newMatrix = new int[3, 3];
        for (int i = 0; i < this.Board.Matrix.GetLength(0); i++)
        {
            for (int j = 0; j < this.Board.Matrix.GetLength(1); j++)
            {
                newMatrix[i, j] = this.Board.Matrix[i, j];
            }
        }

        Board newBoard = new Board(newMatrix);
        return new State(newBoard, this.Parent, this.LastMove, this.SearchDepth);
    }

    public int CompareTo(State obj)
    {
        int current = this.outOfPlace;
        int comparableObj = obj.outOfPlace;
        if (current < comparableObj) return 1;
        if (current > comparableObj) return -1;
        return 0;
    }
    
}