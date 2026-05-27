using Silk.NET.Maths;

namespace TheAdventure;

public static class GUI
{
    public static bool RenderButton(
        int normalTextureId,
        int hoverTextureId,
        TextureData textureData,
        float x,
        float y,float scale = 1.0f)
    {
        Game g = Game.Instance;

        var mouse = g.input.GetMousePosition();

        bool isHover =
            mouse.X >= x &&
            mouse.X <= x + textureData.Width * scale &&
            mouse.Y >= y &&
            mouse.Y <= y + textureData.Height * scale;

        int textureToDraw = isHover
            ? hoverTextureId
            : normalTextureId;

        g.textures.RenderUISimple(
            textureToDraw,
            textureData,
            (int)x,
            (int)y,
            scale
        );


        // Return true if clicked
        return isHover && g.input.IsMouseLeftClicked();
    }
}