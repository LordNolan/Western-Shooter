using UnityEngine;
using System.Collections;

public class Tile
{
    private Vector2 _position;
	
    public Tile(Vector2 position)
    {
        _position = position;
    }
	
    public Vector2 getPosition()
    {
        return _position;
    }
    
    public void setPosition(Vector2 position)
    {
        _position = position;
    }
}
