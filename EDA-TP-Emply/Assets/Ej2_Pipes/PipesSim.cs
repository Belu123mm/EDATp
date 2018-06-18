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
	public int maxWidth , maxHeight;
	[Range(0.0f, 1.0f)] public float branchChance;
	[Range(0.0f, 1.0f)] public float blockChance;
	public float fillDelaySeconds ;

	public Material materialTears;
	public Material materialEmpty;
	public Material materialBlocked;
	public Material materialError;
    bool _filled = false;


    // Variables agregadas
    private Node _root;

    // Matrix Recargado de nodos visitados(nodos ya puestos en escena)
    private bool[,] visited;

    // Lista de todos los nodos en escena
	private List<Node> pipes = new List<Node>();

    //¿Esta la posicion ocupada?
    bool PositionOccupied(int x, int y)
    {
        if((x > 0 && x < maxWidth) && (y > 0 && y < maxHeight)) return visited[x, y];
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

		pipes.Add (n); // Se agrega a la lista de pipes del mapa
        return n;
	}


    //Creamos el sistema de cañerias
    Node RecuBuildDFS(int x, int y, Node prev = null, Direction comingFrom = Direction.Left)
    { 
        return null;
	}

    // Obtiene la cantidad de ramas a dividir el nodo actual
    int RandomBranch()
    {
            return 0;
    }

    List<Direction> GetDireccionesPosibles(int x, int y)
    {
        return null;
    }


    void RandomBlock(Node node)
    {

       
    }


    void CheckErrors() {
		
	}

    void Start()
    {
		//Rotamos una pizca luego de agregar todos los pipes
		transform.Rotate(0f, 0f, -5f);
	}

	void StartFill()
    {
		
	}
	


	void Update ()
    {
		if(Input.GetMouseButtonDown(0))
        {
           
        }
		else if(Input.GetMouseButtonDown(1))
        {
		
		}
	}
}
