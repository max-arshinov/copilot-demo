namespace _4._Median_of_Two_Sorted_Arrays;

public class MedianOfTwoSortedArraysTests
{
    [Fact]
    public void Test_SingleElementArrays()
    {
        var sol = new Solution();
        Assert.Equal(2.0, sol.FindMedianSortedArrays(new[] { 1, 3 }, new[] { 2 }));
    }

    [Fact]
    public void Test_EvenTotalLength()
    {
        var sol = new Solution();
        Assert.Equal(2.5, sol.FindMedianSortedArrays(new[] { 1, 2 }, new[] { 3, 4 }));
    }

    [Fact]
    public void Test_OneEmptyArray()
    {
        var sol = new Solution();
        Assert.Equal(1.0, sol.FindMedianSortedArrays(new[] { 1 }, new int[] { }));
        Assert.Equal(2.0, sol.FindMedianSortedArrays(new int[] { }, new[] { 2 }));
    }

    [Fact]
    public void Test_DifferentLengths()
    {
        var sol = new Solution();
        Assert.Equal(4.5, sol.FindMedianSortedArrays(new[] { 1, 2, 3, 4, 5 }, new[] { 6, 7, 8 }));
    }

    [Fact]
    public void Test_NegativeNumbers()
    {
        var sol = new Solution();
        Assert.Equal(0.0, sol.FindMedianSortedArrays(new[] { -2, 0, 2 }, new[] { -1, 1 }));
    }

    [Fact]
    public void Test_Duplicates()
    {
        var sol = new Solution();
        Assert.Equal(2.0, sol.FindMedianSortedArrays(new[] { 1, 2, 2 }, new[] { 2, 3 }));
    }
}