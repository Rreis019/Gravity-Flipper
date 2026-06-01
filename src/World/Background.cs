using Silk.NET.Maths;
using Silk.NET.SDL;

namespace TheAdventure;

public class BackgroundManager
{
    private int _textureId;
    private TextureData _textureData;
    private float _scrollY;

    public BackgroundManager()
    {
        _textureId = Game.Instance.textures.LoadTexture(Path.Combine("assets/Background/", "Yellow.png"), out _textureData);

    }

    public void Update(float dt)
    {
        _scrollY += 30.0f * dt;
    }

    public void Render()
    {
        Game g = Game.Instance;

        int minX = g.tiles.GetMinX();
        int minY = g.tiles.GetMinY();

        int maxX = g.tiles.GetMaxX();
        int maxY = g.tiles.GetMaxY();

        int texW = _textureData.Width;
        int texH = _textureData.Height;

        for (int y = minY - texH; y < maxY + texH; y += texH)
        {
            for (int x = minX - texW; x < maxX + texW; x += texW)
            {
                int drawY = (int)(y + (_scrollY % texH));

                Rectangle<int> src = new(
                    0,
                    0,
                    texW,
                    texH
                );

                Rectangle<int> dest = new(
                    x,
                    drawY,
                    texW,
                    texH
                );

                g.textures.Render(_textureId, src, dest);
            }
        }
    }
}