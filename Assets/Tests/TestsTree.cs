using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestsGraph
{
    [Test]
    public void GraphRootsShouldContainRoots()
    {
        var node = new TreeNode(null, 0);
        var list = new List<TreeNode>{node};
        var tree = new Graph(list);

        Assert.IsTrue(tree.getNodes().Contains(node), "roots should be in graph");
    }

    [Test]
    public void GraphGetNodesTest1()
    {   
        //linear
        var node = new TreeNode(null, 0);
        var node1 = new TreeNode(new List<TreeNode>{node}, 0);
        var node2 = new TreeNode(new List<TreeNode>{node1}, 0);

        node.addChildren(node1);
        node1.addChildren(node2);

        var tree = new Graph(new List<TreeNode>{node});
        var nodes = tree.getNodes();
        Assert.IsTrue(nodes.Contains(node), "node 0 should be in graph");
        Assert.IsTrue(nodes.Contains(node1), "node 1 should be in graph");
        Assert.IsTrue(nodes.Contains(node2), "node 2 should be in graph");
    }

    [Test]
    public void GraphGetNodesTest2()
    {   
        //fork then join
        var node = new TreeNode(null, 0);
        var node1 = new TreeNode(null, 0);
        var node2 = new TreeNode(null, 0);
        var node3 = new TreeNode(null, 0);

        node.addChildren(node1);
        node.addChildren(node2);
        node1.addChildren(node3);
        node2.addChildren(node3);

        var tree = new Graph(new List<TreeNode>{node});
        var nodes = tree.getNodes();
        Assert.IsTrue(nodes.Contains(node), "node 0 should be in graph");
        Assert.IsTrue(nodes.Contains(node1), "node 1 should be in graph");
        Assert.IsTrue(nodes.Contains(node2), "node 2 should be in graph");
        Assert.IsTrue(nodes.Contains(node3), "node 3 should be in graph");
        Assert.IsTrue(nodes.Count == 4 , "Should be of size 4");
    }

    [Test]
    public void GraphGetNodesTest3()
    {   
        //cyclic
        var node = new TreeNode(null, 0);
        var node1 = new TreeNode(null, 0);
        var node2 = new TreeNode(null, 0);

        node.addChildren(node1);
        node1.addChildren(node2);
        node2.addChildren(node);

        var tree = new Graph(new List<TreeNode>{node});
        var nodes = tree.getNodes();
        Assert.IsTrue(nodes.Contains(node), "node 0 should be in graph");
        Assert.IsTrue(nodes.Contains(node1), "node 1 should be in graph");
        Assert.IsTrue(nodes.Contains(node2), "node 2 should be in graph");
    }

    [Test]
    public void GraphGetEdgesTest1()
    {   
        //cyclic
        var node = new TreeNode(null, 0);
        var node1 = new TreeNode(null, 0);
        var node2 = new TreeNode(null, 0);

        node.addChildren(node1);
        node1.addChildren(node2);
        node2.addChildren(node);

        var tree = new Graph(new List<TreeNode>{node});
        var nodes = tree.getNodes();

        var expectedEdge1 = new Tuple<TreeNode, TreeNode>(node, node1);
        var expectedEdge2 = new Tuple<TreeNode, TreeNode>(node1, node2);
        var expectedEdge3 = new Tuple<TreeNode, TreeNode>(node2, node);

        var edges = tree.getEdges();
        Assert.IsTrue(edges.Contains(expectedEdge1), "Missing edge between node and node1");
        Assert.IsTrue(edges.Contains(expectedEdge2), "Missing edge between node and node2");
        Assert.IsTrue(edges.Contains(expectedEdge3), "Missing edge between node and node3");
        Assert.IsTrue(edges.Count == 3 , "There should be 3 edges");
    }

    [Test]
    public void GraphGetEdgesTest2()
    {   
        //linear
        var node = new TreeNode(null, 0);
        var node1 = new TreeNode(new List<TreeNode>{node}, 0);
        var node2 = new TreeNode(new List<TreeNode>{node1}, 0);

        node.addChildren(node1);
        node1.addChildren(node2);

        var tree = new Graph(new List<TreeNode>{node});
        var nodes = tree.getNodes();

        var expectedEdge1 = new Tuple<TreeNode, TreeNode>(node, node1);
        var expectedEdge2 = new Tuple<TreeNode, TreeNode>(node1, node2);

        var edges = tree.getEdges();
        Assert.IsTrue(edges.Contains(expectedEdge1), "Missing edge between node and node1");
        Assert.IsTrue(edges.Contains(expectedEdge2), "Missing edge between node and node2");
        Assert.IsTrue(edges.Count == 2, "There should be 3 edges");
    }   

    [Test]
    public void GraphOrderCheck()
    {   
        //cyclic
        var node = new TreeNode(null, 0);
        var node1 = new TreeNode(null, 1);
        var node2 = new TreeNode(null, 2);

        node.addChildren(node1);
        node1.addChildren(node2);
        node2.addChildren(node);

        var tree = new Graph(new List<TreeNode>{node});

        var order = tree.DFS();
        tree.printOrder(order);
        
        Assert.IsTrue(true, "no test");
    }

    [Test]
    public void GraphOrderCheck2()
    {   
        //linear
        var node = new TreeNode(null, 0);
        var node1 = new TreeNode(new List<TreeNode>{node}, 1);
        var node2 = new TreeNode(new List<TreeNode>{node1}, 2);

        node.addChildren(node1);
        node1.addChildren(node2);

        var tree = new Graph(new List<TreeNode>{node});

        var order = tree.DFS();
        tree.printOrder(order);
        
        Assert.IsTrue(true, "no test");
    }

    [Test]
    public void GraphIsNotCyclic1()
    {   
        //linear
        var node = new TreeNode(null, 0);
        var node1 = new TreeNode(new List<TreeNode>{node}, 1);
        var node2 = new TreeNode(new List<TreeNode>{node1}, 2);

        node.addChildren(node1);
        node1.addChildren(node2);

        var tree = new Graph(new List<TreeNode>{node});
        
        Assert.IsTrue(!tree.isCyclic(), "The graph should not be detected as cyclic");
    }

    [Test]
    public void GraphIsNotCyclic2()
    {   
        //fork then join
        var node = new TreeNode(null, 0);
        var node1 = new TreeNode(null, 0);
        var node2 = new TreeNode(null, 0);
        var node3 = new TreeNode(null, 0);

        node.addChildren(node1);
        node.addChildren(node2);
        node1.addChildren(node3);
        node2.addChildren(node3);

        var tree = new Graph(new List<TreeNode>{node});
        
        Assert.IsTrue(!tree.isCyclic(), "The graph should not be detected as cyclic");
    }


    [Test]
    public void GraphIsCyclic1()
    {   
        //cyclic
        var node = new TreeNode(null, 0);
        var node1 = new TreeNode(null, 1);
        var node2 = new TreeNode(null, 2);

        node.addChildren(node1);
        node1.addChildren(node2);
        node2.addChildren(node);

        var tree = new Graph(new List<TreeNode>{node});
        
        Assert.IsTrue(tree.isCyclic(), "The graph should be detected as cyclic");
    }

    [Test]
    public void GraphIsCyclic2()
    {   
        //cyclic
        var node = new GameObject();
        var node1 = new GameObject();
        var node2 = new GameObject();

        var edge = new List<GameObject>{node, node1};
        var edge1 = new List<GameObject>{node1, node2};
        var edge2 = new List<GameObject>{node2, node};

        var tree = new Graph(new List<List<GameObject>>{edge, edge1, edge2});
        Assert.IsTrue(tree.isCyclic(), "The graph should be detected as cyclic");
    }

    [Test]
    public void getRootWorks()
    {   
        //linear
        var node = new TreeNode(null, 0);
        var node1 = new TreeNode(new List<TreeNode>{node}, 1);
        var node2 = new TreeNode(new List<TreeNode>{node1}, 2);

        node.addChildren(node1);
        node1.addChildren(node2);

        var tree = new Graph(new List<TreeNode>{node});
        var roots = tree.getRoots(new List<TreeNode>{node, node1, node2});
        Debug.Log(roots.Count);
        Assert.IsTrue(roots.Contains(node), "node should be in roots");
        Assert.IsFalse(roots.Contains(node1), "node1 should not be in roots");
        Assert.IsTrue(roots.Count == 1, "There should only be 1 root in this case");
    }

    [Test]
    public void GraphIsCyclic3()
    {   
        //cyclic
        var node = new GameObject();
        var node1 = new GameObject();
        var node2 = new GameObject();
        var node3 = new GameObject();

        var startEdge = new List<GameObject>{node3, node};
        var edge = new List<GameObject>{node, node1};
        var edge1 = new List<GameObject>{node1, node2};
        var edge2 = new List<GameObject>{node2, node};

        var tree = new Graph(new List<List<GameObject>>{startEdge, edge, edge1, edge2});
        Assert.IsTrue(tree.isCyclic(), "The graph should be detected as cyclic");
    }
    

}
