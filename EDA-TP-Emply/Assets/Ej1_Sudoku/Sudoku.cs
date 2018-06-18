using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Sudoku : MonoBehaviour {
	public Cell prefabCell;
	public Canvas canvas;
	public Text feedback;
	public float stepDuration = 0.05f;
	[Range(1, 82)]public int difficulty = 40;

	Matrix<Cell> _board;
	Matrix<int> _createdMatrix;
    List<int> posibles = new List<int>();
	int _smallSide;
	int _bigSide;
    string memory = "";
    string canSolve = "";
    bool canPlayMusic = false;
    List<int> nums = new List<int>();



    float r = 1.0594f;
    float frequency = 440;
    float gain = 0.5f;
    float increment;
    float phase;
    float samplingF = 48000;


    void Start()
    {
        //Crea un long para sacar el valor de la memoria y calcs 
        long mem = System.GC.GetTotalMemory(true);
        //Cambia el texto de feedback
        feedback.text = string.Format("MEM: {0:f2}MB", mem / (1024f * 1024f));
        //Guarda el texto de feedback a memory
        memory = feedback.text;
        //Guarda el lado pequeño de la grilla (No se pa que esta xd)
        _smallSide = 3;
        //Lo mismo pero con el lado grande (Osea, es re obvio)
        _bigSide = _smallSide * 3;
        //Devuelve el r elevado a la 2 por frecuencia, que por alguna razon es 440
        frequency = frequency * Mathf.Pow(r, 2);
        //Crea el tablero vacio
        CreateEmptyBoard();
        //Limpia el tablero, por si acaso, you know 
        ClearBoard();
    }

    void ClearBoard() {
        //Limpia la lista de matrices
		_createdMatrix = new Matrix<int>(_bigSide, _bigSide);
        //Pone los valores de la board en 0 y valida todas las cells y las desbloquea
		foreach(var cell in _board) {
			cell.number = 0;
			cell.locked = cell.invalid = false;
		}
	}

	void CreateEmptyBoard() {
        //Setea el valor del espaciado entre las celdas
		float spacing = 68f;
        //Valores magicos
		float startX = -spacing * 4f;
		float startY = spacing * 4f;

        //Crea una nueva lista de celulas
		_board = new Matrix<Cell>(_bigSide, _bigSide);
        //Posiciona todas las celulas segun los valores de antes 
		for(int x = 0; x<_board.width; x++) {
			for(int y = 0; y<_board.height; y++) {
                var cell = _board[x, y] = Instantiate(prefabCell);
				cell.transform.SetParent(canvas.transform, false);
				cell.transform.localPosition = new Vector3(startX + x * spacing, startY - y * spacing, 0);
			}
		}
	}
	


    int watchdog = 0;

    //Tal vez modificar esto xd
	bool RecuSolve(Matrix<int> matrixParent, int x, int y, int protectMaxDepth, List<Matrix<int>> solution)
    {
		return false;
	}



    void OnAudioFilterRead(float[] array, int channels)
    {
        if(canPlayMusic)
        {
            increment = frequency * Mathf.PI / samplingF;
            for (int i = 0; i < array.Length; i++)
            {
                phase = phase + increment;
                array[i] = (float)(gain * Mathf.Sin((float)phase));
            }
        }
        
    }
    //Igual esmuy obvio esto xd
    void changeFreq(int num)
    {
        frequency = 440 + num * 80;
    }

	IEnumerator ShowSequence(List<Matrix<int>> seq)
    {
        yield return new WaitForSeconds(0);
    }

    //THIS, ACUERDATE
	void Update () {
		if(Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1))
            SolvedSudoku();
        else if(Input.GetKeyDown(KeyCode.C) || Input.GetMouseButtonDown(0)) 
            CreateSudoku();

	
	}

    void SolvedSudoku()
    {
        StopAllCoroutines();
        nums = new List<int>();
        var solution = new List<Matrix<int>>();
        watchdog = 100000;
        var result =false;//????
        long mem = System.GC.GetTotalMemory(true);
        memory = string.Format("MEM: {0:f2}MB", mem / (1024f * 1024f));
        canSolve = result ? " VALID" : " INVALID";
    }

    void CreateSudoku()
    {
        //Para todas las corutinas
        StopAllCoroutines();
        // Reinicia la lista de nums
        nums = new List<int>();
        //Para los ruiditos
        canPlayMusic = false;
        //Limpia el tablero
        ClearBoard();
        //Crea una nueva lista de matrices llamada l
        List<Matrix<int>> l = new List<Matrix<int>>();
        //Le cambia el valor a watchdog (?)
        watchdog = 100000;
        //Genera una linea valida, 
        GenerateValidLine(_createdMatrix, 0, 0);
        var result =false;//??????
        _createdMatrix = l[0].Clone();//??????
        //Bloquear cells randon (??)
        LockRandomCells();
        //Limpia las desbloqueadas y le pasa una matriz
        ClearUnlocked(_createdMatrix);
        //Pasa todas las variables que no estuvieron borradas por lo anterior xd 
        TranslateAllValues(_createdMatrix);
        //Crea un long :'v con el total de memoria
        long mem = System.GC.GetTotalMemory(true);
        //Calculos :v
        memory = string.Format("MEM: {0:f2}MB", mem / (1024f * 1024f));
        //Pasa el valor de result a una string
        canSolve = result ? " VALID" : " INVALID";
        //Y pasa el feedback a la pantalla 
        feedback.text = "Pasos: " + l.Count + "/" + l.Count + " - " + memory + " - " + canSolve;
    }

    //Genera una linea valida, dandole una matrix y dos valores
	void GenerateValidLine(Matrix<int> mtx, int x, int y)
	{
		int[]aux = new int[9];
		for (int i = 0; i < 9; i++) 
		{
			aux [i] = i + 1;
		}
		int numAux = 0;
		for (int j = 0; j < aux.Length; j++) 
		{
			int r = 1 + Random.Range(j,aux.Length);
			numAux = aux [r-1];
			aux [r-1] = aux [j];
			aux [j] = numAux;
		}
		for (int k = 0; k < aux.Length; k++) 
		{
			mtx [k, 0] = aux [k];
		}
	}


	void ClearUnlocked(Matrix<int> mtx)
	{
		for (int i = 0; i < _board.height; i++) {
			for (int j = 0; j < _board.width; j++) {
				if (!_board [j, i].locked)
					mtx[j,i] = Cell.EMPTY;
			}
		}
	}

	void LockRandomCells()
	{
		List<Vector2> posibles = new List<Vector2> ();
		for (int i = 0; i < _board.height; i++) {
			for (int j = 0; j < _board.width; j++) {
				if (!_board [j, i].locked)
					posibles.Add (new Vector2(j,i));
			}
		}
		for (int k = 0; k < 82-difficulty; k++) {
			int r = Random.Range (0, posibles.Count);
			_board [(int)posibles [r].x, (int)posibles [r].y].locked = true;
			posibles.RemoveAt (r);
		}
	}

    //Pasa todos los valores de la matriz que le pasaste a la board
    void TranslateAllValues(Matrix<int> matrix)
    {
        for (int y = 0; y < _board.height; y++)
        {
            for (int x = 0; x < _board.width; x++)
            {
                _board[x, y].number = matrix[x, y];
            }
        }
    }

    //Pasa el valor especifico a las coordinadas especificas
    void TranslateSpecific(int value, int x, int y)
    {
        _board[x, y].number = value;
    }

    //Pasa todos los valores desde coordenada a a coordenada b de createdmatrix a board
    void TranslateRange(int x0, int y0, int xf, int yf)
    {
        for (int x = x0; x < xf; x++)
        {
            for (int y = y0; y < yf; y++)
            {
                _board[x, y].number = _createdMatrix[x, y];
            }
        }
    }

    //Crea una nueva matriz y la pasa a createdmatrix, luego translada los valores 
    void CreateNew()
    {
        _createdMatrix = new Matrix<int>(Tests.validBoards[1]);
        TranslateAllValues(_createdMatrix);
    }


    //Hace calculos magicos y devuelve si podes meter el valor o no 
    bool CanPlaceValue(Matrix<int> mtx, int value, int x, int y)
    {
        //Pos los nombres lo dicen
        List<int> fila = new List<int>();
        List<int> columna = new List<int>();
        List<int> area = new List<int>();
        List<int> total = new List<int>();
        //:v
        Vector2 cuadrante = Vector2.zero;

        //Por cada fila enla matrix
        for (int cv = 0; cv < mtx.height; cv++)
        {
            //Por cada columna en la matriz
            for (int fv = 0; fv < mtx.width; fv++)
            {
                //Si i es distinto a y y (xd) j es igual a x, agrega en la column 
                if (cv != y && fv == x) columna.Add(mtx[fv, cv]);
                //Y aca al reves xd 
                else if(cv == y && fv != x) fila.Add(mtx[fv,cv]);
                //Osea que agrega todos los valores a la columna x exepto el de la fila y y al reves
                //Y cual es agrega? Ps te respondo
                //Agrega a columna o fila el valor de la matriz en esas coordenadas 
            }
            //En esto, value es el valor en donde qures meetrlo y x e y es la coordenada a donde va a entrar. Entonces vos lo que haces es checkear 
            //Toda la fila y toda la columna para ver si ese valor esta repetido. Haciendo esto guardas los valores para checkearlos luego :3
        }

        //Le contas en que cuadrante (del 3x3) esta el value 
        cuadrante.x = (int)(x / 3); //(Pioruq e esta esto aqui anywys?)

        if (x < 3)
            cuadrante.x = 0;     
        else if (x < 6)
            cuadrante.x = 3;
        else
            cuadrante.x = 6;
        //Lo mismo pero con la y xd 
        if (y < 3)
            cuadrante.y = 0;
        else if (y < 6)
            cuadrante.y = 3;
        else
            cuadrante.y = 6;
        //Da una lista de numeros tal que asi, 00,01,02,10,11,12,20,21,22 suiendo 00 los valores minimos y 22 los maximos (obviamente separados viste xd)
        //Y en este caso da todoslos valores en ese cuadrante ylos guarda en la lsta de area
        area = mtx.GetRange((int)cuadrante.x, (int)cuadrante.y, (int)cuadrante.x + 3, (int)cuadrante.y + 3);
        //Agrega todoslos valores que busco al total
        total.AddRange(fila);
        total.AddRange(columna);
        total.AddRange(area);
        //Saca los ceros de el total
        total = FilterZeros(total);

        //Checkea si en total esta el valor
        if (total.Contains(value))
            //Si esta, ps no podes agregarlo xd
            return false;
        else
            //Y si no, ps podes :V
            return true;
    }

    //Devuelve una lista sin ceros 
    List<int> FilterZeros(List<int> list)
    {
        List<int> aux = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != 0) aux.Add(list[i]);
        }
        return aux;
    }
}
