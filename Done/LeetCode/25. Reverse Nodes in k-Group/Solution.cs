namespace _25._Reverse_Nodes_in_k_Group;

public class Solution
{
    public ListNode? ReverseKGroup(ListNode? head, int k)
    {
        if (head == null || k == 1)
        {
            return head;
        }

        // Create a dummy node to simplify edge cases
        var dummy = new ListNode
        {
            next = head
        };

        // Initialize pointers
        var prev = dummy;

        // Count the number of nodes in the list
        var count = 0;
        while (head != null)
        {
            count++;
            head = head.next;
        }

        // Process in groups of k
        while (count >= k)
        {
            var current = prev.next;
            var next = current.next;

            // Reverse k nodes
            for (var i = 1; i < k; i++)
            {
                current.next = next.next;
                next.next = prev.next;
                prev.next = next;
                next = current.next;
            }

            prev = current; // Update prev to the last node in the reversed group
            count -= k;
        }

        return dummy.next;
    }
}