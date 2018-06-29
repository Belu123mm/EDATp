using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;



public class PipesSim : MonoBehaviour
{
    public enum Content
    {
        Empty,
        Tears,
        Block
    }

    class Node
    {
        public Pipe pipe;
        public Content content = Content.Empty;
        public List<Node> neighbours = new List<Node>();
    };


    public Pipe prefabPipe;
    public int maxWidth, maxHeight;
    [Range(0.0f, 1.0f)]
    public float branchChance;
    [Range(0.0f, 1.0f)]
    public float blockChance;
    public float fillDelaySeconds; //delay de la corrutina....

    public Material materialTears;
    public Material materialEmpty;
    public Material materialBlocked;
    public Material materialError;
    bool _filled = false; //estoparalacorrutina :3


    // Variables agregadas
    private Node _root;

    // Matrix Recargado de nodos visitados(nodos ya puestos en escena)
    private bool[,] visited;

    // Lista de todos los nodos en escena
    private List<Node> pipes = new List<Node>();

    //¿Esta la posicion ocupada?
    bool PositionOccupied(int x, int y)
    {
        if ((x > 0 && x < maxWidth) && (y > 0 && y < maxHeight)) return visited[x, y];
        return true;
    }


    //Creamos un nodo (esta función ya está completa)
    Node CreateNode(int x, int y)
    {

        visited[x, y] = true; // Agregue que el mismo nodo diga en que X e Y se crea.

        var n = new Node();
        n.pipe = Instantiate(prefabPipe);
        n.pipe.transform.parent = transform;
        n.pipe.x = x;
        n.pipe.y = y;

        pipes.Add(n); // Se agrega a la lista de pipes del mapa
        return n;
    }


    //Creamos el sistema de cañerias
    Node RecuBuildDFS(int x, int y, Node prev = null, Direction comingFrom = Direction.Left)
    {
        //ancho y alto max...
        if (x <= maxWidth || y >= maxHeight)
            return prev;
        //un nodo x que tienen una cantidad N de vecinos..lo pense como el laberinto de IA I, por ahora en mi cabeza tiene sentido
        else
        {
            //mi nodito actutal
            Node currentNode = CreateNode(x, y);
            if (prev != null)
                currentNode.pipe.SetConnection(comingFrom, true);
            //el random del current 
            RandomBlock(currentNode);

            int ramitas = RandomBranch(currentNode);
            //recorro las ramitas
            for (int i = 0; i < ramitas; i++)
            {
                //saco los posibles caminos en X e Y
                List<Direction> possibleDirection = GetDireccionesPosibles(x, y);

                //si hay direcciones posibles
                if (possibleDirection.Count > 0)
                {
                    //hago un random entre 0 o la cantidad de direcciones posibles
                    Direction dirToGo = possibleDirection[Random.Range(0, possibleDirection.Count)];

                    //si va para la izq....
                    if (dirToGo == Direction.Left)
                        //agarro el vecito del current y le agrego la bonita recursion que NO FUNCIONA AAAANOENTIENDO
                        //en todas hace lo mismo solo que cambia la direccion :3 
                        currentNode.neighbours.Add(RecuBuildDFS(x - 1, y, currentNode, Direction.Right));
                    else if (dirToGo == Direction.Right)
                        currentNode.neighbours.Add(RecuBuildDFS(x + 1, y, currentNode, Direction.Left));
                    else if (dirToGo == Direction.Bottom)
                        currentNode.neighbours.Add(RecuBuildDFS(x, y - 1, currentNode, Direction.Top));
                    else
                        currentNode.neighbours.Add(RecuBuildDFS(x, y + 1, currentNode, Direction.Bottom));

                    //yyy conecto :3 
                    currentNode.pipe.SetConnection(dirToGo, true);

                }
            }
            return currentNode;
        }
    }
    // Obtiene la cantidad de ramas a dividir el nodo actual
    int RandomBranch(Node current)
    {
        float randomBranch = Random.Range(0, 1);
        //   if (randomBranch < (branchChance / 4))
        //mejor asi 
        return randomBranch < (branchChance / 4) ? 3 : randomBranch < (branchChance / 2) ? 2 : 1;
    }

    List<Direction> GetDireccionesPosibles(int x, int y)
    {
        List<Direction> possibleDir = new List<Direction>();

        //muchos if y me sangran los ojos pero me sangra el cerebro tambien, se deberia hacer menos villa....
        //si las pos no estan ocupadas         
        if (!PositionOccupied(x - 1, y))
            //agrego la posible direccion en el sentido que corresponda :D
            possibleDir.Add(Direction.Right);
        if (!PositionOccupied(x + 1, y))
            possibleDir.Add(Direction.Left);
        if (!PositionOccupied(x, y + 1))
            possibleDir.Add(Direction.Top);
        if (!PositionOccupied(x, y - 1))
            possibleDir.Add(Direction.Bottom);

        return possibleDir;
    }

    //si se bloquea el nodito 
    void RandomBlock(Node node)
    {
        //Y SI MAMITA si lo hiciste con int OBVIAMENTE se te bloquean todas o no se bloquea ninguna. Meodio
        float blockRandom = Random.Range(0f, 1f);
        if (blockRandom < blockChance)
        {
            node.content = Content.Block;
            node.pipe.material = materialBlocked; //le cambio el coloricto asi se que esta bloqueado VIO
        }
    }


    void CheckErrors()
    {
        //eh....nosequehaceraca :3
    }

    void Start()
    {
        //Rotamos una pizca luego de agregar todos los pipes
        //   transform.Rotate(0f, 0f, -5f); no te necesito a ti
        visited = new bool[maxWidth, maxHeight];
    }

    void StartFill()
    {
        //aca va la corrutina
    }
    IEnumerator Fill(Node start)
    {
        Node currentNode = start;
        Queue<Node> queueNode = new Queue<Node>();
        //mientras no este lleno
        while (!_filled)
        {
            //unity hacelo solo
            yield return new WaitForSeconds(fillDelaySeconds);
            //en mi cabeza funciona algo como esto
            //recorro los vecinitos del current node yyy  si esta bloqueado sigo but..no se
            //si termino salgo de aca asi no loopea infinitamente 
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _root = RecuBuildDFS(0, 1);

        }
        else if (Input.GetMouseButtonDown(1))
        {
            //bancala un toque que ya lo voy a sacar a esto
        }
    }
}

