namespace _84._Largest_Rectangle_in_Histogram;

public class Solution
{
    public int LargestRectangleArea(int[] heights)
    {
        if (heights == null || heights.Length == 0)
            return 0;
            
        // For a single bar, the largest rectangle is just its height
        if (heights.Length == 1)
            return heights[0];
            
        // Add a sentinel 0 at the end to process all remaining heights in the stack
        int[] extendedHeights = new int[heights.Length + 1];
        Array.Copy(heights, extendedHeights, heights.Length);
        // Last element is already 0 by default
        
        Stack<int> stack = new Stack<int>();
        int maxArea = 0;
        
        for (int i = 0; i < extendedHeights.Length; i++)
        {
            // When we find a bar shorter than the stack top,
            // we calculate area for all taller bars
            while (stack.Count > 0 && extendedHeights[i] < extendedHeights[stack.Peek()])
            {
                int heightIndex = stack.Pop();
                int height = extendedHeights[heightIndex];
                
                // Calculate width based on whether stack is empty
                int width = stack.Count == 0 ? i : i - stack.Peek() - 1;
                
                // Calculate area and update maxArea
                int area = height * width;
                maxArea = Math.Max(maxArea, area);
            }
            
            // Push current index to stack
            stack.Push(i);
        }
        
        // Handle the special case where all bars have the same height
        // This is already covered by our algorithm with the sentinel 0 at the end
        
        return maxArea;
    }
}