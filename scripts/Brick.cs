using Godot;
using System;

public class Brick : Node2D
{
    public const int DIMENSION = 4;

    public int x = 0;
    public int y = 0;
    public bool[,] collisionMap;
    public Tile[] tiles;
    public PackedScene tileScene = (PackedScene)GD.Load("res://Tile.tscn");

    public override void _Ready()
    {
        collisionMap = new bool[DIMENSION, DIMENSION];

        tiles = new Tile[1] { MakeTile(0, 0) };

        foreach (Tile tile in tiles)
        {
            AddChild(tile);
        }
    }

    public Tile MakeTile(int x, int y)
    {
        Tile tile = (Tile)tileScene.Instance();
        tile.x = x;
        tile.y = y;
        collisionMap[x, y] = true;
        return tile;
    }

    public void Rotate()
    {

    }

    public void Move(int direction)
    {
        direction = Math.Sign(direction);

        if (!CanMoveToward(direction, 0))
        {
            return;
        }

        foreach (Tile tile in tiles)
        {
            tile.Move(direction, 0);
        }

        x += direction;
    }

    public void Drop()
    {
        if (!CanMoveToward(0, 1))
        {
            return;
        }

        foreach (Tile tile in tiles)
        {
            tile.Move(0, 1);
        }

        y += 1;
    }

    private bool CanMoveToward(int x, int y)
    {
        for (int i = 0; i < DIMENSION; i++)
        {
            for (int j = 0; j < DIMENSION; j++)
            {
                if (!collisionMap[i, j])
                {
                    continue;
                }

                int horizontal = j + x + this.x;
                int verticle = i + y + this.y;

                if (horizontal < 0 || horizontal >= Board.BOARD_WIDTH || verticle < 0 || verticle >= Board.BOARD_HEIGHT || Board.WillLocationCollide(horizontal, i + this.y) || Board.WillLocationCollide(j + this.x, verticle))
                {
                    return false;
                }
            }
        }

        return true;
    }
}