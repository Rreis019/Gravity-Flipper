using System;
using Silk.NET.SDL;

namespace TheAdventure;

public class InvisibleCollider : Entity
{
    public int width,height;

    public InvisibleCollider(float x, float y,int width_,int height_)
        : base(x, y)
    {
        id = 1337;
        width = width_;
        height = height_;
        isStatic   = true;
        hasPhysics = true;
        collider = new Collider(0,0,width,height,ColliderType.Solid);
    }

    public override void Update(float dt, InputManager input)
    {
    }

    public override void OnCollide(Entity other)
    {
    }


    public override void Render(IntPtr renderer, Sdl sdl)
    {
    }
}