namespace _42._Trapping_Rain_Water;

public class TrappingRainWaterTests
{
    [Fact]
    public void Example1()
    {
        // Input: height = [0,1,0,2,1,0,1,3,2,1,2,1]
        // Output: 6
        var solution = new Solution();
        int[] height = { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 };

        var result = solution.Trap(height);

        Assert.Equal(6, result);
    }

    [Fact]
    public void Example2()
    {
        // Input: height = [4,2,0,3,2,5]
        // Output: 9
        var solution = new Solution();
        int[] height = { 4, 2, 0, 3, 2, 5 };

        var result = solution.Trap(height);

        Assert.Equal(9, result);
    }

    [Fact]
    public void NoWaterTrapped()
    {
        // No water can be trapped with strictly ascending or descending heights
        var solution = new Solution();
        int[] ascendingHeight = { 0, 1, 2, 3, 4 };
        int[] descendingHeight = { 4, 3, 2, 1, 0 };

        var ascendingResult = solution.Trap(ascendingHeight);
        var descendingResult = solution.Trap(descendingHeight);

        Assert.Equal(0, ascendingResult);
        Assert.Equal(0, descendingResult);
    }

    [Fact]
    public void EmptyOrSmallArray()
    {
        // Edge cases: empty array, single element, or two elements (can't trap water)
        var solution = new Solution();
        int[] emptyArray = { };
        int[] singleElement = { 5 };
        int[] twoElements = { 3, 5 };

        var emptyResult = solution.Trap(emptyArray);
        var singleResult = solution.Trap(singleElement);
        var twoResult = solution.Trap(twoElements);

        Assert.Equal(0, emptyResult);
        Assert.Equal(0, singleResult);
        Assert.Equal(0, twoResult);
    }

    [Fact]
    public void FlatTerrain()
    {
        // All elements are the same height - no water can be trapped
        var solution = new Solution();
        int[] flatArray = { 3, 3, 3, 3, 3 };

        var result = solution.Trap(flatArray);

        Assert.Equal(0, result);
    }

    [Fact]
    public void ComplexPattern()
    {
        // A more complex pattern with multiple peaks and valleys
        var solution = new Solution();
        int[] complexArray = { 5, 2, 1, 2, 1, 5, 0, 3, 4, 1, 2 };

        var result = solution.Trap(complexArray);

        // Expected: 
        // Between first 5 and second 5: (5-2)+(5-1)+(5-2)+(5-1) = 14
        // Between 5 and 4: (4-0)+(4-3) = 5
        // After 4: (2-1) = 1
        // Total: 14 + 5 + 1 = 20
        Assert.Equal(20, result);
    }
}