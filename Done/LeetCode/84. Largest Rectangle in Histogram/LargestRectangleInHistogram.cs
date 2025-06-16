namespace _84._Largest_Rectangle_in_Histogram;

public class LargestRectangleInHistogram
{
    [Fact]
    public void Example1_ShouldReturn10()
    {
        // Arrange
        var solution = new Solution();
        int[] heights = { 2, 1, 5, 6, 2, 3 };
        
        // Act
        int result = solution.LargestRectangleArea(heights);
        
        // Assert
        Assert.Equal(10, result);
    }
    
    [Fact]
    public void Example2_ShouldReturn4()
    {
        // Arrange
        var solution = new Solution();
        int[] heights = { 2, 4 };
        
        // Act
        int result = solution.LargestRectangleArea(heights);
        
        // Assert
        Assert.Equal(4, result);
    }
    
    [Fact]
    public void SingleBar_ShouldReturnItsHeight()
    {
        // Arrange
        var solution = new Solution();
        int[] heights = { 5 };
        
        // Act
        int result = solution.LargestRectangleArea(heights);
        
        // Assert
        Assert.Equal(5, result);
    }
    
    [Fact]
    public void AllSameHeight_ShouldReturnHeightTimesLength()
    {
        // Arrange
        var solution = new Solution();
        int[] heights = { 3, 3, 3, 3 };
        
        // Act
        int result = solution.LargestRectangleArea(heights);
        
        // Assert
        Assert.Equal(12, result); // 3 height * 4 width
    }
    
    [Fact]
    public void AscendingHeights_ShouldFindCorrectArea()
    {
        // Arrange
        var solution = new Solution();
        int[] heights = { 1, 2, 3, 4, 5 };
        
        // Act
        int result = solution.LargestRectangleArea(heights);
        
        // Assert
        Assert.Equal(9, result); // max is 3 height * 3 width (bars 3,4,5)
    }
    
    [Fact]
    public void DescendingHeights_ShouldFindCorrectArea()
    {
        // Arrange
        var solution = new Solution();
        int[] heights = { 5, 4, 3, 2, 1 };
        
        // Act
        int result = solution.LargestRectangleArea(heights);
        
        // Assert
        Assert.Equal(9, result); // max is 3 height * 3 width (bars 1,2,3)
    }
    
    [Fact]
    public void ZeroHeightBars_ShouldHandleCorrectly()
    {
        // Arrange
        var solution = new Solution();
        int[] heights = { 2, 0, 3, 4, 0, 1 };
        
        // Act
        int result = solution.LargestRectangleArea(heights);
        
        // Assert
        Assert.Equal(6, result); // max is either 3*2 or 4*1, which is 6
    }
    
    [Fact]
    public void ComplexPattern_ShouldFindMaxRectangle()
    {
        // Arrange
        var solution = new Solution();
        int[] heights = { 2, 1, 2, 3, 1 };
        
        // Act
        int result = solution.LargestRectangleArea(heights);
        
        // Assert
        Assert.Equal(5, result); // max is height 1 across all 5 positions
    }
}