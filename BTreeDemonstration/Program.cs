/*
 * Shakeel Khan
 * Spring 2020
 * BIT 265
 * Professor Mike Panitz
 * 
 * NOTE that the code for the B-Tree is simply a port and the original implementation was done
 * in C++ by Akanksha Rai, Shubham Singh and Balasabrmaniam Nagasundaram over at GeeksForGeeks, and can be found here:
 * Insertion Code: https://www.geeksforgeeks.org/insert-operation-in-b-tree/
 * Deletion Code: https://www.geeksforgeeks.org/delete-operation-in-b-tree/?ref=lbp
 */

using System;

namespace BTreeDemonstration
{
    class Program
    {
        static void Main(string[] args)
        {
            runInsertionDemo();

            //runDeletionDemo();
        }

        public static void runInsertionDemo()
        {
            // Max. Degree 6 w/ Preemtive Split & Merge
            BTree myTree = new BTree(3);

            // Populate the tree with values
            // Root is null
            myTree.insert(10);
            Console.WriteLine("Traversal of tree after inserting 10");
            myTree.traverse();
            Console.WriteLine();

            // Inserting into a non-full node
            myTree.insert(20);
            Console.WriteLine("Traversal of tree after inserting 20");
            myTree.traverse();
            Console.WriteLine();

            myTree.insert(30);
            Console.WriteLine("Traversal of tree after inserting 30");
            myTree.traverse();
            Console.WriteLine();

            myTree.insert(40);
            Console.WriteLine("Traversal of tree after inserting 40");
            myTree.traverse();
            Console.WriteLine();

            myTree.insert(50);
            Console.WriteLine("Traversal of tree after inserting 50");
            myTree.traverse();
            Console.WriteLine();

            // Inserting into a full node
            myTree.insert(60);
            Console.WriteLine("Traversal of tree after inserting 60");
            myTree.traverse();
            Console.WriteLine();

            myTree.insert(70);
            Console.WriteLine("Traversal of tree after inserting 70");
            myTree.traverse();
            Console.WriteLine();

            myTree.insert(80);
            Console.WriteLine("Traversal of tree after inserting 80");
            myTree.traverse();
            Console.WriteLine();

            // Split
            myTree.insert(90);
            Console.WriteLine("Traversal of tree after inserting 90");
            myTree.traverse();
            Console.WriteLine();

            // Find the value 40, print the keys of the node returned
            BTreeNode result = myTree.find(40);
            Console.WriteLine("\nHere are the keys of the node returned:");
            result.printKeys();
        }

        public static void runDeletionDemo()
        {
            // Max. Degree 6 w/ Preemtive Split & Merge
            BTree myTree = new BTree(3);

            // Populate the tree with values
            myTree.insert(1);
            myTree.insert(3);
            myTree.insert(7);
            myTree.insert(10);
            myTree.insert(11);
            myTree.insert(13);
            myTree.insert(14);
            myTree.insert(15);
            myTree.insert(18);
            myTree.insert(16);
            myTree.insert(19);
            myTree.insert(24);
            myTree.insert(25);
            myTree.insert(26);
            myTree.insert(21);
            myTree.insert(4);
            myTree.insert(5);
            myTree.insert(20);
            myTree.insert(22);
            myTree.insert(2);
            myTree.insert(17);
            myTree.insert(12);
            myTree.insert(6);

            // Traverse the newly constructed tree before the deletion process
            Console.WriteLine("Traversal of tree constructed is");
            myTree.traverse();
            Console.WriteLine();

            // Begin Deletion Process
            myTree.remove(6);
            Console.WriteLine("Traversal of tree after removing 6");
            myTree.traverse();
            Console.WriteLine();

            myTree.remove(13);
            Console.WriteLine("Traversal of tree after removing 13");
            myTree.traverse();
            Console.WriteLine();

            // Case 2C
            myTree.remove(7);
            Console.WriteLine("Traversal of tree after removing 7");
            myTree.traverse();
            Console.WriteLine();

            // Case 3B (Pre-emptive merge), then case 1A
            myTree.remove(4);
            Console.WriteLine("Traversal of tree after removing 4");
            myTree.traverse();
            Console.WriteLine();

            // Case 1C
            myTree.remove(2);
            Console.WriteLine("Traversal of tree after removing 2");
            myTree.traverse();
            Console.WriteLine();

            // Deleting a key within the root node
            myTree.remove(16);
            Console.WriteLine("Traversal of tree after removing 16");
            myTree.traverse();
            Console.WriteLine();
        }
    }

    class BTreeNode
    {
        public int[] keys; // An array of keys
        public int t; // Minimum degree (defines the range for number of keys)
        public BTreeNode[] children; // Array of child pointers
        public int numOfKeys; // The number of keys this node contains
        public bool leaf; // Whether this node is a leaf (contains no children)

        public BTreeNode(int t, bool leaf)
        {
            // Copy the minimum degree and leaf property
            this.t = t;
            this.leaf = leaf;

            // Allocate memory for the maximum number of possible keys and child pointers
            keys = new int[2 * t - 1];
            children = new BTreeNode[2 * t];

            // Initialize the number of keys as 0
            numOfKeys = 0;
        }

        // This will insert a new key into the subtree rooted within this node.
        // The assumption is the node must be non-full when this function is called.
        public void insertNonFull(int key)
        {
            // Initialize index as index of rightmost element 
            int i = numOfKeys - 1;

            // If this is a leaf node 
            if (leaf == true)
            {
                // The following loop does two things 
                // a) Finds the location of new key to be inserted 
                // b) Moves all greater keys to one place ahead 
                while (i >= 0 && keys[i] > key)
                {
                    keys[i + 1] = keys[i];
                    i--;
                }

                // Insert the new key at found location 
                keys[i + 1] = key;
                numOfKeys = numOfKeys + 1;
            }
            else // If this node is not leaf 
            {
                // Find the child which is going to have the new key 
                while (i >= 0 && keys[i] > key)
                    i--;

                // See if the found child is full 
                if (children[i + 1].numOfKeys == 2 * t - 1)
                {
                    // If the child is full, then split it 
                    splitChild(i + 1, children[i + 1]);

                    // After split, the middle key of C[i] goes up and 
                    // C[i] is splitted into two. See which of the two 
                    // is going to have the new key 
                    if (keys[i + 1] < key)
                        i++;
                }
                children[i + 1].insertNonFull(key);
            }
        }

        // This will split the child y of this node. i is the index of y in the child array
        // children[]. The child y must be full when this function is called.
        public void splitChild(int i, BTreeNode y)
        {
            // Create a new node which is going to store (t-1) keys 
            // of y 
            BTreeNode z = new BTreeNode(y.t, y.leaf);
            z.numOfKeys = t - 1;

            // Copy the last (t-1) keys of y to z 
            for (int j = 0; j < t - 1; j++)
            {
                z.keys[j] = y.keys[j + t];
            }

            // Copy the last t children of y to z 
            if (y.leaf == false)
            {
                for (int j = 0; j < t; j++)
                {
                    z.children[j] = y.children[j + t];
                }
            }

            // Reduce the number of keys in y 
            y.numOfKeys = t - 1;

            // Since this node is going to have a new child, 
            // create space of new child 
            for (int j = numOfKeys; j >= i + 1; j--)
            {
                children[j + 1] = children[j];
            }

            // Link the new child to this node 
            children[i + 1] = z;

            // A key of y will move to this node. Find the location of 
            // new key and move all greater keys one space ahead 
            for (int j = numOfKeys - 1; j >= i; j--)
                keys[j + 1] = keys[j];

            // Copy the middle key of y to this node 
            keys[i] = y.keys[t - 1];

            // Increment count of keys in this node 
            numOfKeys = numOfKeys + 1;
        }

        // A wrapper function to remove the key k in subtree rooted with 
        // this node. 
        public void remove(int key)
        {
            int idx = findKey(key);

            // The key to be removed is present in this node 
            if (idx < numOfKeys && keys[idx] == key)
            {

                // If the node is a leaf node - removeFromLeaf is called 
                // Otherwise, removeFromNonLeaf function is called 
                if (leaf)
                    removeFromLeaf(idx);
                else
                    removeFromNonLeaf(idx);
            }
            else
            {

                // If this node is a leaf node, then the key is not present in tree 
                if (leaf)
                {
                    Console.WriteLine("The key {0} is does not exist in the tree", key);
                    return;
                }

                // The key to be removed is present in the sub-tree rooted with this node 
                // The flag indicates whether the key is present in the sub-tree rooted 
                // with the last child of this node 
                bool flag = ((idx == numOfKeys) ? true : false);

                // If the child where the key is supposed to exist has less that t keys, 
                // we fill that child 
                if (children[idx].numOfKeys < t)
                    fill(idx);

                // If the last child has been merged, it must have merged with the previous 
                // child and so we recurse on the (idx-1)th child. Else, we recurse on the 
                // (idx)th child which now has atleast t keys 
                if (flag && idx > numOfKeys)
                {
                    children[idx - 1].remove(key);
                }
                else
                {
                    children[idx].remove(key);
                }
            }
        }

        // A function to remove the key present in idx-th position in 
        // this node which is a leaf 
        public void removeFromLeaf(int idx)
        {
            // Move all the keys after the idx-th pos one place backward 
            for (int i = idx + 1; i < numOfKeys; ++i)
            {
                keys[i - 1] = keys[i];
            }

            // Reduce the count of keys 
            numOfKeys--;
        }

        // A function to remove the key present in idx-th position in 
        // this node which is a non-leaf node 
        public void removeFromNonLeaf(int idx)
        {
            int key = keys[idx];

            // If the child that precedes k (C[idx]) has atleast t keys, 
            // find the predecessor 'pred' of k in the subtree rooted at 
            // C[idx]. Replace k by pred. Recursively delete pred 
            // in C[idx] 
            if (children[idx].numOfKeys >= t)
            {
                int pred = getPred(idx);
                keys[idx] = pred;
                children[idx].remove(pred);
            }

            // If the child C[idx] has less that t keys, examine C[idx+1]. 
            // If C[idx+1] has atleast t keys, find the successor 'succ' of k in 
            // the subtree rooted at C[idx+1] 
            // Replace k by succ 
            // Recursively delete succ in C[idx+1] 
            else if (children[idx + 1].numOfKeys >= t)
            {
                int succ = getSucc(idx);
                keys[idx] = succ;
                children[idx + 1].remove(succ);
            }

            // If both C[idx] and C[idx+1] has less that t keys,merge k and all of C[idx+1] 
            // into C[idx] 
            // Now C[idx] contains 2t-1 keys 
            // Free C[idx+1] and recursively delete k from C[idx] 
            else
            {
                merge(idx);
                children[idx].remove(key);
            }
        }

        // A function to get the predecessor of the key- where the key 
        // is present in the idx-th position in the node 
        public int getPred(int idx)
        {
            // Keep moving to the right most node until we reach a leaf 
            BTreeNode cur = children[idx];
            while (!cur.leaf)
            {
                cur = cur.children[cur.numOfKeys];
            }

            // Return the last key of the leaf 
            return cur.keys[cur.numOfKeys - 1];
        }

        // A function to get the successor of the key- where the key 
        // is present in the idx-th position in the node 
        public int getSucc(int idx)
        {
            // Keep moving the left most node starting from C[idx+1] until we reach a leaf 
            BTreeNode cur = children[idx + 1];
            while (!cur.leaf)
            {
                cur = cur.children[0];
            }

            // Return the first key of the leaf 
            return cur.keys[0];
        }

        // A function to fill up the child node present in the idx-th 
        // position in the C[] array if that child has less than t-1 keys 
        public void fill(int idx)
        {
            // If the previous child(C[idx-1]) has more than t-1 keys, borrow a key 
            // from that child 
            if (idx != 0 && children[idx - 1].numOfKeys >= t)
            {
                borrowFromPrev(idx);
            }

            // If the next child(C[idx+1]) has more than t-1 keys, borrow a key 
            // from that child 
            else if (idx != numOfKeys && children[idx + 1].numOfKeys >= t)
            {
                borrowFromNext(idx);
            }

            // Merge C[idx] with its sibling 
            // If C[idx] is the last child, merge it with with its previous sibling 
            // Otherwise merge it with its next sibling 
            else
            {
                if (idx != numOfKeys)
                {
                    merge(idx);
                }
                else
                {
                    merge(idx - 1);
                }
            }
        }

        // A function to borrow a key from the C[idx-1]-th node and place 
        // it in C[idx]th node 
        public void borrowFromPrev(int idx)
        {
            BTreeNode child = children[idx];
            BTreeNode sibling = children[idx - 1];

            // The last key from C[idx-1] goes up to the parent and key[idx-1] 
            // from parent is inserted as the first key in C[idx]. Thus, the loses 
            // sibling one key and child gains one key 

            // Moving all key in C[idx] one step ahead 
            for (int i = child.numOfKeys - 1; i >= 0; --i)
            {
                child.keys[i + 1] = child.keys[i];
            }

            // If C[idx] is not a leaf, move all its child pointers one step ahead 
            if (!child.leaf)
            {
                for (int i = child.numOfKeys; i >= 0; --i)
                    child.children[i + 1] = child.children[i];
            }

            // Setting child's first key equal to keys[idx-1] from the current node 
            child.keys[0] = keys[idx - 1];

            // Moving sibling's last child as C[idx]'s first child 
            if (!child.leaf)
            {
                child.children[0] = sibling.children[sibling.numOfKeys];
            }

            // Moving the key from the sibling to the parent 
            // This reduces the number of keys in the sibling 
            keys[idx - 1] = sibling.keys[sibling.numOfKeys - 1];

            child.numOfKeys += 1;
            sibling.numOfKeys -= 1;
        }

        // A function to borrow a key from the C[idx+1]-th node and place it 
        // in C[idx]th node 
        public void borrowFromNext(int idx)
        {
            BTreeNode child = children[idx];
            BTreeNode sibling = children[idx + 1];

            // keys[idx] is inserted as the last key in C[idx] 
            child.keys[(child.numOfKeys)] = keys[idx];

            // Sibling's first child is inserted as the last child 
            // into C[idx] 
            if (!child.leaf)
            {
                child.children[(child.numOfKeys) + 1] = sibling.children[0];
            }

            //The first key from sibling is inserted into keys[idx] 
            keys[idx] = sibling.keys[0];

            // Moving all keys in sibling one step behind 
            for (int i = 1; i < sibling.numOfKeys; ++i)
            {
                sibling.keys[i - 1] = sibling.keys[i];
            }

            // Moving the child pointers one step behind 
            if (!sibling.leaf)
            {
                for (int i = 1; i <= sibling.numOfKeys; ++i)
                    sibling.children[i - 1] = sibling.children[i];
            }

            // Increasing and decreasing the key count of C[idx] and C[idx+1] 
            // respectively 
            child.numOfKeys += 1;
            sibling.numOfKeys -= 1;
        }

        // A function to merge idx-th child of the node with (idx+1)th child of 
        // the node 
        public void merge(int idx)
        {
            BTreeNode child = children[idx];
            BTreeNode sibling = children[idx + 1];

            // Pulling a key from the current node and inserting it into (t-1)th 
            // position of C[idx] 
            child.keys[t - 1] = keys[idx];

            // Copying the keys from C[idx+1] to C[idx] at the end 
            for (int i = 0; i < sibling.numOfKeys; ++i)
            {
                child.keys[i + t] = sibling.keys[i];
            }

            // Copying the child pointers from C[idx+1] to C[idx] 
            if (!child.leaf)
            {
                for (int i = 0; i <= sibling.numOfKeys; ++i)
                {
                    child.children[i + t] = sibling.children[i];
                }
            }

            // Moving all keys after idx in the current node one step before - 
            // to fill the gap created by moving keys[idx] to C[idx] 
            for (int i = idx + 1; i < numOfKeys; ++i)
            {
                keys[i - 1] = keys[i];
            }

            // Moving the child pointers after (idx+1) in the current node one 
            // step before 
            for (int i = idx + 2; i <= numOfKeys; ++i)
            {
                children[i - 1] = children[i];
            }

            // Updating the key count of child and the current node 
            child.numOfKeys += sibling.numOfKeys + 1;
            numOfKeys--;
        }

        // This will traverse all the nodes in a subtree rooted within this node
        public void traverse()
        {
            // There are n keys and n+1 children, traverse through n keys
            // and first n children
            int i;
            for (i = 0; i < numOfKeys; i++)
            {
                // If this is not a leaf (it has children) then before we print keys[i]
                // call traverse on the child first
                if (leaf == false)
                {
                    children[i].traverse();
                }
                Console.Write("{0} ", keys[i]);
            }

            // Print the subtree rooted with last child
            if (leaf == false)
            {
                children[i].traverse();
            }
        }

        // This will search for a specified key in subtree rooted with this node
        public BTreeNode find(int key)
        {
            // Find the first key greater than or equal to k
            int i = 0;
            while (i < numOfKeys && key > keys[i])
            {
                i++;
            }

            // If the found key is equal to the target, return this node
            if (keys[i] == key)
            {
                return this;
            }

            // If key is not found here and this is a leaf node
            if (leaf == true)
            {
                // Because if this is a leaf node that means there are no more children to search
                return null;
            }

            // Go to the appropiate child
            return children[i].find(key);
        }

        // A function that returns the index of the first key that is greater 
        // or equal to k 
        public int findKey(int key)
        {
            int idx = 0;
            while (idx < numOfKeys && keys[idx] < key)
            {
                ++idx;
            }
            return idx;
        }

        public void printKeys()
        {
            for (int i = 0; i < numOfKeys; i++)
            {
                Console.Write("{0} ", keys[i]);
            }
        }
    }

    class BTree
    {

        BTreeNode root;
        int t; // The minimum degree

        public BTree(int t)
        {
            // Store the minimum degree
            this.t = t;
            // Set the root to null
            root = null;
        }

        public void traverse()
        {
            if (root != null)
            {
                root.traverse();
            }
        }

        public BTreeNode find(int key)
        {
            if (root == null)
            {
                return null;
            }
            else
            {
                return root.find(key);
            }
        }

        public void insert(int key)
        {
            // If tree is empty 
            if (root == null)
            {
                // Allocate memory for root 
                root = new BTreeNode(t, true);
                root.keys[0] = key; // Insert key 
                root.numOfKeys = 1; // Update number of keys in root
            }
            else // If tree is not empty 
            {
                // If root is full, then tree grows in height 
                if (root.numOfKeys == 2 * t - 1)
                {
                    // Allocate memory for new root 
                    BTreeNode s = new BTreeNode(t, false);

                    // Make old root as child of new root 
                    s.children[0] = root;

                    // Split the old root and move 1 key to the new root
                    s.splitChild(0, root);

                    // New root has two children now. Decide which of the 
                    // two children is going to have new key
                    int i = 0;
                    if (s.keys[0] < key)
                    {
                        i++;
                    }
                    s.children[i].insertNonFull(key);

                    // Change root
                    root = s;
                }
                else // If root is not full, call insertNonFull for root 
                {
                    root.insertNonFull(key);
                }
            }
        }

        public void remove(int key)
        {
            if (root == null)
            {
                Console.WriteLine("The tree is empty");
                return;
            }

            // Call the remove function for root 
            root.remove(key);

            // If the root node has 0 keys, make its first child as the new root 
            // if it has a child, otherwise set root as NULL 
            if (root.numOfKeys == 0)
            {
                BTreeNode tmp = root;
                if (root.leaf)
                {
                    root = null;
                }
                else
                {
                    root = root.children[0];
                }
            }
        }
    }
}
