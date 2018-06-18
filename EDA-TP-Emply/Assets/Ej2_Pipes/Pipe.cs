using UnityEngine;
using System.Collections;



//IMPORTANTE: Esta clase esta completa, NO LA DEBE MODIFICAR -- USELA ASI
public class Pipe : MonoBehaviour {
	public GameObject[] parts;

	Material _material;
	int _x, _y;
	
	//Cambia la posicion x de la grilla y ajustando el objeto
	public int x {
		get { return _x; }
		set { 
			_x = value;
			RefreshPos();
		}
	}

	//Cambia la posicion y de la grilla y ajustando el objeto
	public int y {
		get { return _y; }
		set { 
			_y = value;
			RefreshPos();
		}
	}

	void RefreshPos() {
		gameObject.transform.localPosition = new Vector3(_x*3f, -_y*3f, 0f);
	}

	//Asigna un material al modelo
	public Material material
    {
		get { return _material; }
		set {
			_material = value;
			foreach(var p in parts)
				p.GetComponent<Renderer>().material = _material;
		}
	}

	//Hace visible o invisible una conexion (representacion grafica unicamente)
	//Ejemplo: SetConnection(Direction.Left, true);
	public void SetConnection(Direction which, bool visible) {
		parts[(int)which].SetActive(visible);
	}
	
	void Awake () {
		foreach(var p in parts) {
			p.SetActive (false);
		}
		SetConnection(Direction.Center, true);
	}
}
