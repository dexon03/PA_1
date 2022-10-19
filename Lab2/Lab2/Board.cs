namespace Lab2;

public class Board
{
    public int[,] Matrix { get; set; }

    public Board()
    {
        Matrix = new int[3,3];
    }

    public Board(int[,] matrix)
    {
        this.Matrix = matrix;
    }

    public void GenerateBoard()
    {
        Random random = new Random();
        int[] arr = new int[9];
        int number;
        for (int i = 0; i < arr.Length; i++)
        {
            do
            {
                number = random.Next(1, 10);
            } while (arr.Contains(number));

            arr[i] = number;
        }

        int indexArr = 0;
        for (int i = 0; i < Matrix.GetLength(0); i++)
        {
            for (int j = 0; j < Matrix.GetLength(1); j++)
            {
                Matrix[i, j] = arr[indexArr];
                indexArr++;
            }
        }
    }

    public bool isEqual(int[,] goalMatrix)
    {
        for (int i = 0; i < this.Matrix.GetLength(0); i++)
        {
            for (int j = 0; j < this.Matrix.GetLength(1); j++)
            {
                if (this.Matrix[i, j] != goalMatrix[i, j]) return false;
            }
        }

        return true;
    }

    public (int, int) IndexOfBlank()
    {
        for (int i = 0; i < this.Matrix.GetLength(0); i++)
        {
            for (int j = 0; j < this.Matrix.GetLength(1); j++)
            {
                if (this.Matrix[i, j].Equals(9)) return (i, j);
            }
        }

        return (-1, -1);
    }

    public void OutPut()
    {
        for (int i = 0; i < Matrix.GetLength(0); i++)
        {
            for (int j = 0; j < Matrix.GetLength(1); j++)
            {
                if (Matrix[i, j].Equals(9))
                {
                    Console.Write("* ");
                }
                else
                {
                    Console.Write(Matrix[i,j] + " ");
                }
            }
            Console.WriteLine();
        }
    }
}