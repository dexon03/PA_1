namespace Lab2;

public static class LDFS
{
    public static State? Solve(State state, int limit)
    {
        // try
        // {
            State? result = RecursiveLDFS(state, limit);
            if (result != null)
            {
                return result; 
            }
            
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e.Message);
        // }

        return null;
        // return state;
        // ResultLDFS result = RecursiveLDFS(state, limit);
        // return result;
    }

    // public static bool Search(State state, int limit)
    // {
    //     Stack<State> stateStack = new Stack<State>();
    //     var visited = new HashSet<Board>();
    //     stateStack.Push(state);
    //     int depth = 0;
    //     while (stateStack.Count != 0)
    //     {
    //         if (depth <= limit)
    //         {
    //             var current = stateStack.Pop();
    //             if (current.Board.isEqual(FunctionsAndConstants.goalState))
    //             {
    //                 while (current.Parent != null)
    //                 {
    //                     current.Board.OutPut();
    //                     current = current.Parent;
    //                 }
    //     
    //                 Console.WriteLine("Goal was found");
    //                 return true;
    //             }
    //             else
    //             {
    //                 visited.Add(current.Board);
    //                 var children = FunctionsAndConstants.GenerateChildren(current);
    //                 foreach (var child in children)
    //                 {
    //                     if (!visited.Contains(child.Board))
    //                     {
    //                         stateStack.Push(child);
    //                     }
    //                 }
    //                 depth++;
    //             }
    //     
    //         }
    //         else
    //         {
    //             var current = stateStack.Pop();
    //             while (current.Parent != null)
    //             {
    //                 current.Board.OutPut();
    //                 Console.WriteLine("--------------------------");
    //                 current = current.Parent;
    //             }
    //             Console.WriteLine("Goal state not found within depth limit");
    //             return false;
    //         }
    //     }
    //     
    //     Console.WriteLine("Goal state not found");
    //     return false;
    // }
    private static State? RecursiveLDFS(State state, int limit)
    {
        if (state.Board.isEqual(FunctionsAndConstants.goalState))
        {
            // return new ResultLDFS(state,false,false);
            return state;
        }
        if (state.SearchDepth > limit)
        {
            // return new ResultLDFS(state, false, true);
            return null;
        }
    
        
        var children = FunctionsAndConstants.GenerateChildren(state);
        foreach (var child in children)
        {
            // ResultLDFS result = RecursiveLDFS(child, limit);
            State result = RecursiveLDFS(child, limit);
            if (result != null)
            {
                return result;
            }
        }
        // return new ResultLDFS(null,true,false);
        return null;
    }
}