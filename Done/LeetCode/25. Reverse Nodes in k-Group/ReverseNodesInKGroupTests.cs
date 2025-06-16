namespace _25._Reverse_Nodes_in_k_Group;

public class ReverseNodesInKGroupTests
{
    [Fact]
    public void Test_ReverseKGroup_Example1()
    {
        // Example 1: Input: 1->2->3->4->5, k=2
        // Output: 2->1->4->3->5
        ListNode head = CreateLinkedList(new[] { 1, 2, 3, 4, 5 });
        Solution solution = new Solution();
        
        ListNode result = solution.ReverseKGroup(head, 2);
        
        Assert.Equal(new[] { 2, 1, 4, 3, 5 }, ConvertToArray(result));
    }

    [Fact]
    public void Test_ReverseKGroup_Example2()
    {
        // Example 2: Input: 1->2->3->4->5, k=3
        // Output: 3->2->1->4->5
        ListNode head = CreateLinkedList(new[] { 1, 2, 3, 4, 5 });
        Solution solution = new Solution();
        
        ListNode result = solution.ReverseKGroup(head, 3);
        
        Assert.Equal(new[] { 3, 2, 1, 4, 5 }, ConvertToArray(result));
    }

    [Fact]
    public void Test_ReverseKGroup_K_Equals_1()
    {
        // When k=1, list should remain unchanged
        ListNode head = CreateLinkedList(new[] { 1, 2, 3, 4, 5 });
        Solution solution = new Solution();
        
        ListNode result = solution.ReverseKGroup(head, 1);
        
        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, ConvertToArray(result));
    }

    [Fact]
    public void Test_ReverseKGroup_K_Equals_Length()
    {
        // When k equals the length of the list, the entire list should be reversed
        ListNode head = CreateLinkedList(new[] { 1, 2, 3, 4, 5 });
        Solution solution = new Solution();
        
        ListNode result = solution.ReverseKGroup(head, 5);
        
        Assert.Equal(new[] { 5, 4, 3, 2, 1 }, ConvertToArray(result));
    }

    [Fact]
    public void Test_ReverseKGroup_EmptyList()
    {
        // Empty list should return empty
        ListNode head = null;
        Solution solution = new Solution();
        
        ListNode result = solution.ReverseKGroup(head, 3);
        
        Assert.Null(result);
    }

    [Fact]
    public void Test_ReverseKGroup_SingleNode()
    {
        // Single node with any k should remain unchanged
        ListNode head = new ListNode(1);
        Solution solution = new Solution();
        
        ListNode result = solution.ReverseKGroup(head, 3);
        
        Assert.Equal(new[] { 1 }, ConvertToArray(result));
    }
    
    private ListNode CreateLinkedList(int[] values)
    {
        if (values.Length == 0)
            return null;

        ListNode head = new ListNode(values[0]);
        ListNode current = head;

        for (int i = 1; i < values.Length; i++)
        {
            current.next = new ListNode(values[i]);
            current = current.next;
        }

        return head;
    }

    private int[] ConvertToArray(ListNode head)
    {
        List<int> result = new List<int>();
        while (head != null)
        {
            result.Add(head.val);
            head = head.next;
        }
        return result.ToArray();
    }    
}