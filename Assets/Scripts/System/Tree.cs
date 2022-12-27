using System.Collections.Generic;
using Microsoft.ML.Probabilistic.Models;
using System;
using System.Linq;
using System.Collections;
using UnityEngine;

public class Graph
{   
    private List<TreeNode> roots;

    public Graph(List<TreeNode> roots) 
    {
        this.roots = roots;
    }

    public Graph(List<List<GameObject>> edges) {
        this.roots = getRoots(transformGameObjectToTreeNode(edges));
    }

    public List<Tuple<TreeNode, int>> DFS() {
        var result = new List<Tuple<TreeNode, int>>();
        var visited = new HashSet<TreeNode>();
        var discover = new HashSet<TreeNode>();
        int time = 0;


        foreach(TreeNode node in getNodes()) {
            if(!visited.Contains(node)) {
                DFS_visit(node);
            }
        }

        return result;

        void DFS_visit(TreeNode u) {
            time = time + 1;
            discover.Add(u);

            foreach(TreeNode child in u.getChildren()) {
                if(!visited.Contains(child) && !discover.Contains(child)) {
                    DFS_visit(child);
                }
            }

            visited.Add(u);
            time = time + 1;
            result.Add(new Tuple<TreeNode, int>(u, time));
        }
    } 

    public void printOrder(List<Tuple<TreeNode, int>> order) {
        order.ForEach(tup => Debug.Log("[" + tup.Item1.getValue() + " ," + tup.Item2 + "]"));
    }

    public Boolean isCyclic() {
        //We have no roots
        if(roots.Count == 0) {
            return true;
        }

        var order = DFS();
        var nodes = getNodes();

        foreach(TreeNode node in nodes) { 

            int time1 = order.Where(tuple => tuple.Item1.Equals(node)).First().Item2;
            foreach(TreeNode child in node.getChildren()) {
                int time2 = order.Where(tuple => tuple.Item1.Equals(child)).First().Item2;

                if(time2 > time1) {
                    return true; // Backward edge detected -> cyclic
                }
            }
        }

        return false;
    }

    public HashSet<Tuple<TreeNode, TreeNode>> getEdges()
    {   
        //TODO: getEdges and getNodes share a lot of code -> reuse it in some way
        var edges = new HashSet<Tuple<TreeNode, TreeNode>>();
        var nodes = new HashSet<TreeNode>();
        var toVisit = new HashSet<TreeNode>();

        foreach(TreeNode node in roots) {
            toVisit.Add(node);
        }

        while(toVisit.Any()) {
            var current = toVisit.First();
            toVisit.Remove(current);
            nodes.Add(current);
            var successor = current.getChildren();

            successor.ForEach(node => edges.Add(new Tuple<TreeNode, TreeNode>(current, node)));

            successor
                .Where(node => !nodes.Contains(node) && !toVisit.Contains(node))
                .ToList() // NO foreach method on IEnum type
                .ForEach(node => toVisit.Add(node));
            
        }

        return edges;

    }

    public HashSet<TreeNode> getNodes() {
        var nodes = new HashSet<TreeNode>();
        var toVisit = new HashSet<TreeNode>();

        foreach(TreeNode node in roots) {
            toVisit.Add(node);
        }

        while(toVisit.Any()) {
            var current = toVisit.First();
            toVisit.Remove(current);
            nodes.Add(current);

            var successor = current.getChildren();

            successor
                .Where(node => !nodes.Contains(node) && !toVisit.Contains(node))
                .ToList() // NO foreach method on IEnum type
                .ForEach(node => toVisit.Add(node));
            
        }

        return nodes;
    }

    public List<TreeNode> transformGameObjectToTreeNode(List<List<GameObject>> edges) {
        
        var binding = new Dictionary<GameObject, TreeNode>();
        foreach(var edge in edges) { 
            foreach(var node in edge) {
                if(!binding.ContainsKey(node)) {
                    binding.Add(node, new TreeNode());
                }
            }

            binding[edge[0]].addChildren(binding[edge[1]]);
        }

        return binding.Values.ToList();
    }

    public List<TreeNode> getRoots(List<TreeNode> nodes) {
        var result = new List<TreeNode>();

        foreach(var node in nodes) {
            Boolean rootFlag = true;
            foreach(var otherNode in nodes) {
                if(node != otherNode) {
                    if(otherNode.getChildren().Contains(node)) {
                        rootFlag = false;
                    }
                }
            }

            if(rootFlag) {
                result.Add(node);
            }
        }

        this.roots = result;
        return result;
    }

    public List<TreeNode> getRoots()
    {
        return this.roots;
    }

    public void addNode(TreeNode parent, TreeNode newNode)
    {
        
    }
}

public class TreeNode
{
    private int value;
    List<TreeNode> parents;
    List<TreeNode> children;

    Variable<bool> Generator;

    List<Variable<bool>> transformator;

    public TreeNode(List<TreeNode> parents, int value)
    {
        this.value = value;
        this.parents = parents;
        children = new List<TreeNode>();
    }

    public TreeNode() {
        this.value = 0;
        this.parents = new List<TreeNode>();
        this.children = new List<TreeNode>(); 
        this.Generator = Variable.Bernoulli(0.5);
        this.transformator = new List<Variable<bool>>();
    }

    public void addChildren(TreeNode child)
    {   
        if(child != null){
            this.children.Add(child);
        } 
    }

    public void setValue(int value)
    {
        this.value = value;
    }

    public int getValue() {
        return this.value;
    }

    public List<TreeNode> getChildren() {
        if(this.children == null) {
            return new List<TreeNode>();
        }
        return this.children;
    }

    public void setTransform(Variable<bool> variable)
    {
        this.transformator.Add(variable);
    }

    public void setTransform(List<Variable<bool>> variables)
    {
        variables.ForEach(variable => this.transformator.Add(variable));
    }

    public List<Variable<bool>> getTransform()
    {
        return this.transformator;
    }


}