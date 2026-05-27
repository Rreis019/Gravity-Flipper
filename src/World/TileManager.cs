using System.Collections.Generic;
using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public class TileManager
{
    private readonly List<Tile> _tiles = new();

    private TileSet _tileSet;

    public TileManager(TileSet tileSet)
    {
        _tileSet = tileSet;
    }

    public void Add(Tile tile)
    {
        _tiles.Add(tile);
    }

    public void RemoveAtPosition(Vector2D<float> pos)
    {
        _tiles.RemoveAll(t =>
            pos.X >= t.x && pos.X < t.x + 16 &&
            pos.Y >= t.y && pos.Y < t.y + 16
        );
    }

    public void RenderTile(int index,float x,float y)
    {
        var src = _tileSet.GetSource(index);

        var dest = new Rectangle<int>(
            (int)x,
            (int)y,
            16,
            16
        );

        Game.Instance.textures.Render(
            _tileSet.textureId,
            src,
            dest
        );
    }

    public void Render(IntPtr renderer, Sdl sdl)
    {
        foreach (var tile in _tiles)
        {
            RenderTile(tile.index,tile.x,tile.y);
        }
    }

    public void SaveTilesFile(string path)
    {
        using StreamWriter writer = new StreamWriter(path);

        foreach (var tile in _tiles)
        {
            writer.WriteLine(
                $"{tile.index} {tile.x} {tile.y}"
            );
        }
    }

    public void LoadTilesFile(string path)
    {
        if (!File.Exists(path))
            return;

        _tiles.Clear();

        using StreamReader reader = new StreamReader(path);

        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split(' ');

            if (parts.Length < 3)
                continue;

            int index = int.Parse(parts[0]);
            int x = int.Parse(parts[1]);
            int y = int.Parse(parts[2]);

            Add(new Tile(index, x, y));
        }
    }
    
    public void Clear()
    {
        _tiles.Clear();
    }



    public int GetMinX()
    {
        if (_tiles.Count == 0)
            return 0;

        int min = _tiles[0].x;

        foreach (var tile in _tiles)
        {
            if (tile.x < min)
                min = tile.x;
        }

        return min;
    }

    public int GetMinY()
    {
        if (_tiles.Count == 0)
            return 0;

        int min = _tiles[0].y;

        foreach (var tile in _tiles)
        {
            if (tile.y < min)
                min = tile.y;
        }

        return min;
    }

    public int GetMaxX()
    {
        if (_tiles.Count == 0)
            return 0;

        int max = _tiles[0].x + 16;

        foreach (var tile in _tiles)
        {
            int right = tile.x + 16;

            if (right > max)
                max = right;
        }

        return max;
    }

    public int GetMaxY()
    {
        if (_tiles.Count == 0)
            return 0;

        int max = _tiles[0].y + 16;

        foreach (var tile in _tiles)
        {
            int bottom = tile.y + 16;

            if (bottom > max)
                max = bottom;
        }

        return max;
    }

}