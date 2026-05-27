using Silk.NET.SDL;
using Silk.NET.Maths;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;


namespace TheAdventure;


public class TextureManager {
    private Dictionary<int, IntPtr> _texturePointers = new();
    private Dictionary<int, TextureData> _textureInformation = new();
    private int _index = 0;

    public unsafe int LoadTexture(string fileName, out TextureData textureData)
    {
        using var fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var image = Image.Load<Rgba32>(fStream);
        
        textureData = new TextureData()
        {
            Width = image.Width, 
            Height = image.Height
        };

        var imageRawData = new byte[textureData.Width * textureData.Height * 4];
        image.CopyPixelDataTo(imageRawData.AsSpan());
        Texture* imageTexture;
        
        fixed (byte* data = imageRawData)
        {
            var imageSurface = Game.Instance.sdl.CreateRGBSurfaceWithFormatFrom(
                data, textureData.Width, 
                textureData.Height,
                8, 
                textureData.Width * 4, 
                (uint)PixelFormatEnum.Rgba32);
            imageTexture = Game.Instance.sdl.CreateTextureFromSurface((Renderer*)Game.Instance.renderer, imageSurface);
            Game.Instance.sdl.FreeSurface(imageSurface); // surface is only needed to create the texture, free it immediately
        }

        _texturePointers[_index] = (IntPtr)imageTexture;
        _textureInformation[_index] = textureData;
        return _index++;
    }

    public unsafe void Render(int textureId, Rectangle<int> src, Rectangle<int> dest, RendererFlip flipMode = RendererFlip.None)
    {
        if (_texturePointers.TryGetValue(textureId, out var texture))
        {
            Game g = Game.Instance;
            Camera2D cam = g.mainCamera;

            var screenDest = cam.WorldToScreenRect(dest);

            g.sdl.RenderCopyEx(
                (Renderer*)g.renderer,
                (Texture*)texture,
                in src,
                in screenDest,
                0.0,
                null,
                flipMode
            );
        }
    }

    public unsafe void RenderUI(int textureId, Rectangle<int> src, Rectangle<int> dest, RendererFlip flipMode = RendererFlip.None)
    {
        if (_texturePointers.TryGetValue(textureId, out var texture))
        {
            Game g = Game.Instance;

            g.sdl.RenderCopyEx(
                (Renderer*)g.renderer,
                (Texture*)texture,
                in src,
                in dest,
                0.0,
                null,
                flipMode
            );
        }
    }


    public unsafe void RenderUISimple(int textureId, TextureData textureData,int x,int y,float scale = 1.0f, RendererFlip flipMode = RendererFlip.None)
    {
        if (_texturePointers.TryGetValue(textureId, out var texture))
        {
            Game g = Game.Instance;

            Rectangle<int> src = new Rectangle<int>((int)0,(int)0,(int)textureData.Width,(int)textureData.Height); 
            Rectangle<int> dest = new Rectangle<int>((int)x,(int)y,(int)(textureData.Width* scale) ,(int)(textureData.Height * scale)); 

            g.sdl.RenderCopyEx(
                (Renderer*)g.renderer,
                (Texture*)texture,
                in src,
                in dest,
                0.0,
                null,
                flipMode
            );
        }
    }


}