using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public enum ColliderType
{
    None,
    Solid,
    Trigger
}

public class Collider
{
    public float offsetX;
    public float offsetY;

    public float width;
    public float height;

    public ColliderType type;

    public static bool debugRender = false;

    public Collider(float offsetX_,float offsetY_,float width, float height, ColliderType type = ColliderType.Solid)
    {
        this.offsetX = offsetX_;
        this.offsetY = offsetY_;
        this.width = width;
        this.height = height;
        this.type = type;
    }

    // ---------------- WORLD BOUNDS ----------------

    public float Left(Entity e)   => e.X + offsetX;
    public float Right(Entity e)  => e.X + offsetX + width;
    public float Top(Entity e)    => e.Y + offsetY;
    public float Bottom(Entity e) => e.Y + offsetY + height;

    // ---------------- COLLISION ----------------

     public static bool Intersects(Entity a, Entity b, float skin = 0.01f)
    {
        var ac = a.collider;
        var bc = b.collider;

        if (ac == null || bc == null)
            return false;

        float aLeft = a.X + ac.offsetX + skin;
        float aRight = a.X + ac.offsetX + ac.width - skin;
        float aTop = a.Y + ac.offsetY + skin;
        float aBottom = a.Y + ac.offsetY + ac.height - skin;

        float bLeft = b.X + bc.offsetX + skin;
        float bRight = b.X + bc.offsetX + bc.width - skin;
        float bTop = b.Y + bc.offsetY + skin;
        float bBottom = b.Y + bc.offsetY + bc.height - skin;

        return !(aRight < bLeft ||
                 aLeft > bRight ||
                 aBottom < bTop ||
                 aTop > bBottom);
    }

   // ---------------- DEBUG RENDER ----------------

    public void Render(IntPtr renderer, Sdl sdl, Entity entity)
    {
        if(!Collider.debugRender){return;}

        unsafe
        {
            if(type == ColliderType.Solid) {sdl.SetRenderDrawColor((Renderer*)renderer, 255,0,0,255);}
            else if(type == ColliderType.Trigger){sdl.SetRenderDrawColor((Renderer*)renderer, 0,255,0,255);}
            else{return;}

            Game g = Game.Instance;
            Camera2D cam = g.mainCamera;

            var rect = new Rectangle<int>((int)Left(entity), (int)Top(entity), (int)width,(int)height);

            var screenDest = cam.WorldToScreenRect(rect);

            // draw outline 
            sdl.RenderDrawRect((Renderer*)renderer, ref screenDest);
        }
    }
}