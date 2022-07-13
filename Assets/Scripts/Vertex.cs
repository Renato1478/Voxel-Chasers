using UnityEngine;
using System.Collections.Generic;
using System; //This allows the IComparable Interface

//This is the class you will be storing
//in the different collections. In order to use
//a collection's Sort() method, this class needs to
//implement the IComparable interface.
public class Vertex
{
    public Vector3 position;
    public List<Vertex> neighbors;

    public Vertex(Vector3 newPosition)
    {
        position = newPosition;
        neighbors = new List<Vertex>();
    }

    //This method is required by the IComparable
    //interface. 
    public void addNeighbor(Vertex other)
    {
        neighbors.Add(other);
    }

    public List<Vertex> GetNeighbors()
    {
        return neighbors;
    }
}