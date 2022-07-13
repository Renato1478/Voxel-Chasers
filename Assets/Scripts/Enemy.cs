using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform target; //the enemy's target
    private float moveSpeed = 3f; //move speed
    private float rotationSpeed = 3f; //speed of turning
    private float range = 100f; //Range within target will be detected
    private float stop = 0;
    private Transform myTransform; //current transform data of this enemy
    public List<Vertex> path = new List<Vertex>();

    private bool canMove = true;

    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform; //target the player
        myTransform = transform; //cache transform data for easy access/preformance
        moveSpeed = Random.Range(moveSpeed, moveSpeed + 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Debug.DrawRay(myTransform.position, target.position - myTransform.position, Color.green);
            FollowTarget(target.position);
        } else
        {
            Debug.DrawRay(myTransform.position, target.position - myTransform.position, Color.red);
            // se raycasts (entre inimigo e player) colidirem com obstaculo
            // vertices nao gerados ainda.
            // gera-se os vertices para perseguicao evitando obstaculos com dijkstra
            // vertices ja gerados?
            // avanca para o proximo vertice
            // se raycasts nao colidirem mais com obstaculo
            // canMove = true

            // Detecta colisao com obstaculo
            RaycastHit hit;
            if (Physics.Raycast(myTransform.position + new Vector3(0, 0.1f, -0.15f), target.position - myTransform.position, out hit, 100f))
            {
                if (hit.collider.gameObject.tag == "Hazards")
                {
                    /**
                    if(path.Count.Equals(0))
                    {
                        GeneratePath();
                    } else
                    {
                        Debug.Log("Goto" + path[path.Count - 1]);
                        FollowTarget(path[path.Count - 1].position);
                        path.RemoveAt(path.Count - 1);
                    }*/
                } else
                {
                    canMove = true;
                    path.Clear();
                    Debug.Log("Can Move!!!");
                }
            }
        }
    }

    private void FollowTarget(Vector3 pos)
    {
        var distance = Vector3.Distance(myTransform.position, pos);
        if (distance <= range)
        {
            //look
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
            Quaternion.LookRotation(pos - myTransform.position), rotationSpeed * Time.deltaTime);
            //move
            if (distance > stop)
            {
                myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
            }
        }
    }

    private void GeneratePath()
    {
        List<Vertex> graph = new List<Vertex>();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Vertex newVertex = new Vertex(new Vector3((i - 5) + myTransform.position.x, myTransform.position.y, (j - 5) + myTransform.position.z));
                graph.Add(newVertex);
            }
        }

        for (int v = 0; v < graph.Count; v++)
        {
            graph[v].addNeighbor(GetVertex(graph, v + 1));
            graph[v].addNeighbor(GetVertex(graph, v - 1));
            graph[v].addNeighbor(GetVertex(graph, v + 9)); // diagonal superior esq.
            graph[v].addNeighbor(GetVertex(graph, v + 10)); 
            graph[v].addNeighbor(GetVertex(graph, v + 11)); // diagonal superior dir.
            graph[v].addNeighbor(GetVertex(graph, v - 9)); // diagonal inferior esq.
            graph[v].addNeighbor(GetVertex(graph, v - 10));
            graph[v].addNeighbor(GetVertex(graph, v - 11)); // diagonal inferior dir.
        }

        Vertex origin = GetVertex(graph, graph.Count / 2); // vertice do meio
        List<Vertex> neighs = origin.GetNeighbors();

        RaycastHit hit;
        Physics.Raycast(origin.position, target.position - origin.position, out hit, 100f);
        float currentDistance = hit.distance;
        if(hit.collider.gameObject.tag == "Hazards")
        {
            path.Add(origin);
            return;
        }

        while (hit.collider.gameObject.tag == "Hazards") {
            foreach (Vertex neighbor in neighs)
            {
                Physics.Raycast(neighbor.position, target.position - neighbor.position, out hit, 100f);
                float neighborDistance = hit.distance;
                if (hit.collider.gameObject.tag == "Hazards")
                {
                    path.Add(neighbor);
                    return;
                }
                if (neighborDistance < currentDistance)
                {
                    path.Add(neighbor);
                }
            }
            origin = path[path.Count - 1];
            neighs = origin.GetNeighbors();
        }
    }

    private Vertex GetVertex(List<Vertex> listVertex, int index)
    {
        if(index <= 0 || index >= listVertex.Count)
        {
            return null;
        } else
        {
            return listVertex[index];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hazards")
        {
            canMove = false;
        }
    }

    /** 
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        foreach(Vertex vertex in graph) {
            Gizmos.DrawSphere(vertex.position, 0.5f);
        }
    }*/
}
