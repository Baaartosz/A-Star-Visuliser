using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pathfinder
{
    /*
     * Using A* Pathfinding
     */
    public class Finder
    {
        private Grid grid;
        private int startX, startY;
        private int goalX, goalY;

        private int maxSize;
        
        private List<Node> openList, closedList, finalPath;

        private Node currentNode;
        
        public bool iterate = false;

        public Finder(Grid grid)
        {
            this.grid = grid;
            maxSize = grid.GetArraySize();
            Reset();
        }

        public void Setup()
        {
            grid.GetStartLocation(out startX, out startY);
            grid.GetGoalLocation(out goalX, out goalY);

            Reset();
            
            openList.Add(new Node(startX, startY));
            Console.WriteLine(openList);
        }

        public void Reset()
        {
            openList = new List<Node>();
            closedList = new List<Node>();
            finalPath = null;
        }

        private List<Node> GetNeighbours(Node n)
        {
            List<Node> nodes = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    
                    int checkX = n.x + x;
                    int checkY = n.y + y;

                    if (grid.WithinArrayBounds(checkX,checkY))
                    {
                        var newNode = new Node(checkX,checkY);
                        nodes.Add(newNode);
                    }
                }
            }
            return nodes;
        }

        private int Heuristic(Node node, Node other)
        {
            return Heuristic(node.x, node.y, other.x, other.y);
        }
        
        private int Heuristic(int x1, int y1, int x2, int y2)
        {
            int dstX = Math.Abs(x1 - x2);
            int dstY = Math.Abs(y1 - y2);
            
            if (dstX > dstY)  return 14 * dstY + 10 * (dstX-dstY);
            return 14*dstX + 10 * (dstY-dstX);
            // return (int)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2,2));
        }

        private bool Step() // Returns true is goal is found.
        {
            currentNode = openList.ElementAt(0);
            
            for (int i = 1; i < openList.Count; i ++) {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost) {
                    if (openList[i].hCost < currentNode.hCost)
                        currentNode = openList[i];
                }
            }
            
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.x == goalX && currentNode.y == goalY)
            {
                finalPath = FindPath(currentNode);
                return true;
            }

            foreach (var neighbour in GetNeighbours(currentNode))
            {
                if (grid.GetCellValue(neighbour.x, neighbour.y) == 3 || 
                    closedList.Any(node => node.x == neighbour.x && node.y == neighbour.y)) continue;
            
                int newCostToNeighbour = currentNode.gCost + Heuristic(currentNode, neighbour);
                
                // If new cost is lower then neighbour and is not in openlist.
                if (newCostToNeighbour < neighbour.gCost || 
                    !openList.Any(node => node.x == neighbour.x && node.y == neighbour.y))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = Heuristic(neighbour.x, neighbour.y, goalX, goalY);
                    neighbour.parent = currentNode;
            
                    if (!openList.Any(node => node.x == neighbour.x && node.y == neighbour.y))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
            return false;
        }

        private List<Node> FindPath(Node node)
        {
            List<Node> path = new List<Node>();

            Node current = node;

            while (current != null)
            {
                path.Add(current);
                current = current.parent;
            }

            return path;
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {
            if (iterate)
            {
                foreach (var open in openList)
                {
                    grid.SetCell(open.x, open.y, 4);
                }
                foreach (var closed in closedList)
                {
                    grid.SetCell(closed.x, closed.y, 5);
                }
                if(currentNode != null)  grid.SetCell(currentNode.x, currentNode.y, 1);
                
            }
            spriteBatch.DrawString(Game1.pixelFont, $"Open <{openList.Count}>", new Vector2(450, 300), Color.LimeGreen);
            spriteBatch.DrawString(Game1.pixelFont, $"Closed <{closedList.Count}>", new Vector2(450, 350), Color.Red);
            spriteBatch.DrawString(Game1.pixelFont, $"Max Size <{maxSize}>", new Vector2(450, 400), Color.White);

            if (finalPath == null) return;
            foreach (var node in finalPath)
            {
                grid.SetCell(node.x,node.y, 6);
            }

        }

        public void Update(GameTime gameTime)
        {
            if (iterate)
                if (Step())
                {
                    iterate = false;
                    Console.WriteLine("GOAL FOUND");
                    
                }
        }
    }
}