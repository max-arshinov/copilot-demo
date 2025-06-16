namespace _42._Trapping_Rain_Water;

public class Solution
{
    public int Trap(int[] height)
    {
        if (height == null || height.Length < 3)
            return 0;

        var left = 0;
        var right = height.Length - 1;
        var leftMax = 0;
        var rightMax = 0;
        var trappedWater = 0;

        while (left < right)
            // If the left height is lower than the right height
            if (height[left] < height[right])
            {
                // Update the max height seen from the left
                if (height[left] >= leftMax)
                    leftMax = height[left];
                else
                    // Calculate water trapped at this position
                    trappedWater += leftMax - height[left];
                left++;
            }
            // If the right height is lower or equal to the left height
            else
            {
                // Update the max height seen from the right
                if (height[right] >= rightMax)
                    rightMax = height[right];
                else
                    // Calculate water trapped at this position
                    trappedWater += rightMax - height[right];
                right--;
            }

        return trappedWater;
    }
}