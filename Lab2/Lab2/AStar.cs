using System.Diagnostics;

namespace Lab2;

public static class AStar
{
    public static State Solve(State state)
    {
        DateTime data1 = DateTime.Now;
        var OpenList = new PriorityQueue<State, int>();
        var ClosedList = new List<State>();
        OpenList.Enqueue(state,state.F);
        while (OpenList.Count != 0)
        {
            var current = OpenList.Dequeue();
            ClosedList.Add(current);
            if (current.Board.isEqual(FunctionsAndConstants.goalState))
            {
                return current;
            }
            
            var children = FunctionsAndConstants.GenerateChildren(current);
            foreach (var child in children)
            {
                if (!ClosedList.Contains(child))
                {
                    OpenList.Enqueue(child,child.F);
                }
            }
            long memoryUsed = Process.GetCurrentProcess().PrivateMemorySize64/1000000000;
            if (memoryUsed > 1)
            {
                Console.WriteLine("Memory used is out of available");
                return null;
            }
            DateTime data2 = DateTime.Now;
            if ((data2 - data1).Minutes > 30)
            {
                Console.WriteLine("Timeout");
                return null;
            }
        }

        return null;
    }
}