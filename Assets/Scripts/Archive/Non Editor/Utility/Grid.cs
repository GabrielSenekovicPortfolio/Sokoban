using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Grid<T>: object
{
    T[] array;
    int width;
    int height;

    int Width
    {
        get { return width; }
        set { width = Mathf.Clamp(value, 1, 100); }
    }
    int Height
    {
        get { return height; }
        set { height = Mathf.Clamp(value, 1, 100); }
    }

    public Grid(int width, int height)
    {
        Width = width;
        Height = width;
        array = new T[height * width];
    }

    public T this[int x, int y]
    {
        get 
        {
            x = Mathf.Clamp(x, 0, width);
            y = Mathf.Clamp(y, 0, height);
            int i = x + width * y;
            return array[i];
        }
        set 
        {
            x = Mathf.Clamp(x, 0, width);
            y = Mathf.Clamp(y, 0, height);
            int i = x + width * y;
            array[i] = value;
        }
    }
    public int GetWidth() => width;
    public int GetHeight() => height;
    public Vector2Int GetDimensions() => new Vector2Int(width, height);
    public bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
    public T[] GetArray() => array;
}
