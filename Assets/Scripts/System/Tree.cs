using System.Collections.Generic;
using Microsoft.ML.Probabilistic.Models;
using System;
using System.Linq;

class Tree
{   
    private List<TreeNode> roots;
    public Tree(List<TreeNode> roots) 
    {
        this.roots = roots;
    }

    //Perform topological sort
    //source : https://gist.github.com/Sup3rc4l1fr4g1l1571c3xp14l1d0c10u5/3341dba6a53d7171fe3397d13d00ee3f
    public bool checkCyclic(HashSet<Tuple<TreeNode, TreeNode>> edges)
    {
        // Empty list that will contain the sorted elements
        var L = new List<TreeNode>();

        // Set of all nodes with no incoming edges
        var S = new HashSet<TreeNode>(roots);

        while (S.Any()) {

            var n = S.First();
            S.Remove(n);
            L.Add(n);
            // for each node m with an edge e from n to m do
            foreach (var e in edges.Where(e => e.Item1.Equals(n)).ToList()) {
                var m = e.Item2;
                edges.Remove(e);

                // if m has no other incoming edges then
                if (edges.All(me => me.Item2.Equals(m) == false)) {
                    S.Add(m);
                }
            }
        }

        if (edges.Any()) {
            //(graph has at least one cycle)
            return false;
        } else {
            return true;
        }
    }

    private HashSet<Tuple<TreeNode, TreeNode>> edges(List<TreeNode> nodes)
    {   
        HashSet<Tuple<TreeNode, TreeNode>> edgesAcc(List<TreeNode> nodes, HashSet<Tuple<TreeNode, TreeNode>> acc, HashSet<TreeNode> visited) {
            if(nodes.Count == 0) {
                return acc;
            } else { 
                HashSet<TreeNode> moreVisited = new HashSet<TreeNode>();
                var toBeVisited = new List<TreeNode>();
                foreach(TreeNode node in nodes) {
                    node.getChildren().ForEach(child => {
                        acc.Add(new Tuple<TreeNode, TreeNode>(node, child)); 
                        if(!visited.Contains(child)){
                            moreVisited.Add(child);
                            toBeVisited.Add(child);
                            }
                        });
                }

                return edgesAcc(toBeVisited, acc, new HashSet<TreeNode>(visited.Union(moreVisited)));
            }
        }
        return edgesAcc(nodes, new HashSet<Tuple<TreeNode, TreeNode>>(), new HashSet<TreeNode>());
    }
}

class TreeNode
{
    private int value;
    List<TreeNode> parents;
    List<TreeNode> children;

    public TreeNode(List<TreeNode> parents, int value) 
    { 
        this.value = value;
    }

    public void addChildren(TreeNode child)
    {   
        if(child != null) this.children.Add(child);
    }

    public void setValue(int value)
    {
        this.value = value;
    }

    public List<TreeNode> getChildren() {return this.children;}

}