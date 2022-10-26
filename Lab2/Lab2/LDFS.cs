namespace Lab2;

public static class LDFS
{
    public static State? Solve(State state, int limit, out int iterations,out int angles,out int countOfState)
    {
        iterations = 0;
        angles = 0;
        countOfState = 1;
        State? result = RecursiveLDFS(state, limit,ref iterations,ref angles, ref countOfState);
        if (result != null)
        { 
            return result; 
        }
        return null;
    }

    private static State? RecursiveLDFS(State state, int limit, ref int iterations,ref int angles,ref int countOfState)
    {
        iterations++;
        if (state.Board.isEqual(FunctionsAndConstants.goalState))
        {
            return state;
        }
        if (state.SearchDepth == limit)
        {
            var listOfAngles = FunctionsAndConstants.GenerateChildren(state.Parent);
            angles += listOfAngles.Count;
            return null;
        }
    
        
        var children = FunctionsAndConstants.GenerateChildren(state);
        if (children.Count == 0)
        {
            angles++;
        }
        else
        {
            countOfState += children.Count;
        }
        foreach (var child in children)
        {
            // ResultLDFS result = RecursiveLDFS(child, limit);
            State result = RecursiveLDFS(child, limit,ref iterations,ref angles,ref countOfState);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }
}