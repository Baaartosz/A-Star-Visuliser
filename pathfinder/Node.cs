namespace pathfinder
{
    public class Node
    {
        public int x, y;
        public int hCost, gCost;
        public Node parent;

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int fCost => gCost + hCost;
    }
}