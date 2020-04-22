using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T> 
{
    
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        // Maximum size of Heap Array
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        // Each item should be able to keep track of its own index in the heap
        // Compare two items and see which item has the highest priority to sort it in the heap
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        // Remove item from top of heap
        T firstItem = items[0];
        currentItemCount--;
        // Take item at the end of the heap and put it in the first place and change Index to 0
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        // Sort the new node into its correct position in the heap
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        // Used to change the priority of an item, if the f-cost has reduced through exploration in the algorithm
        // If the priority has been increased call SortUp
        SortUp(item);
    }

    public bool Contains(T item)
    {
        // Check if two items are equal | item in array, index of item being passed in  == item being passed in
        // If it is then true, else false
        return Equals(items[item.HeapIndex], item);
    }

    public int Count
    {
        get { return currentItemCount; }
    }

    void SortDown(T item)
    {
        //Get the indices of the nodes two children nodes
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            // Check if the index has a child on the left / right
            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                {
                    // Which child has a higher priority, set the swap index to that child.
                    // So if the left index has a lower priority then set the swap index to child right.
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                // Now check whether the parent has a lower priority than its higher priority child, in which case
                // swap them
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    // if the parent has higher priority than both children it is in its correct position
                    return;
                }
            }
            else
            {
                // If the parent does not have any children it is in the correct position
                return;
            }
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            // It compares the nodes sorting them upwards, so if the node has a higher priority it return 1,
            // if it has the same priority it returns 0, if it has a lower priority it returns -1
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                // As soon as the item is not a high priority as its parent Index then break out of the loop
                break;
            }

            // Otherwise recalculate
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        // Swap nodes in the heap as well as their heap index values
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        // Remember the index of A before it is swapped with B, then assign B its old index
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
