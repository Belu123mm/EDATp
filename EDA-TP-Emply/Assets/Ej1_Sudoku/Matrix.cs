using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Matrix<T> : IEnumerable<T> {

    int _width;
    int _height;
    int _capacity;
    T[,] data;
    Matrix<T> auxData;
    public int width { get { return _width; } }

    public int height { get { return _height; } }

    public int Capacity { get { return _capacity; } }

    public Matrix(int width, int height) {
        _width = width;
        _height = height;
        _capacity = _width * _height;
        data = new T[_width, _height];
    }

	public Matrix(T[,] copyFrom) {
        _width = copyFrom.GetLength(0);
        _height = copyFrom.GetLength(1);
        _capacity = _width * _height;
        data = new T[_width, _height];
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                data[x, y] = copyFrom[x, y];
            }
        }
    }

	public Matrix<T> Clone() {
        Matrix<T> aux = new Matrix<T>(width, height);
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                aux.data[x, y] = data[x, y];
            }
        }
        return aux;
    }

	public void SetRangeTo(int x0, int y0, int x1, int y1, T item) {
        for (int y = y0; y < y1; y++)
        {
            for (int x = x0; x < x1; x++)
            {
                data[x, y] = item;
            }
        }
    }

	public List<T> GetRange(int x0, int y0, int x1, int y1) {
        List<T> l = new List<T>();
        for (int y = y0; y < y1; y++)
        {
            for (int x = x0; x < x1; x++)
            {
                l.Add(data[x, y]);
            }
        }
        return l;
	}


    public T this[int x, int y] {
		get {
			return data[x,y];
		}
		set {
            data[x, y] = value;
		}
	}

    

	public IEnumerator<T> GetEnumerator() {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (data[x, y] == null) yield return default(T);
                else yield return data[x, y];
            }
        }
    }

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}
}
