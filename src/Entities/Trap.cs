using System;
using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public class Trap : Entity
{
    protected Animator _animator;
    protected Animation _idleAnimation;
    protected int _width,_height;
    private int _textureId;
    public RendererFlip flip = RendererFlip.None;

    public Trap(float x, float y,int width,int height,int textureId, Animation defaultAnimation)
        : base(x, y)
    {
        _animator = new Animator();

        _textureId = textureId;
        _idleAnimation = defaultAnimation;

        _width = width;
        _height = height;

        _animator.Add("idle", defaultAnimation);
        _animator.Play("idle");

        isStatic   = true;
        hasPhysics = true;

        collider = new Collider(0,8,_width,8,ColliderType.Trigger);
    }

    public void setFlippedVertically()
    {
        collider = new Collider(0,0,_width,8,ColliderType.Trigger);
        flip = RendererFlip.Vertical;
    }



    public override void Update(float dt, InputManager input)
    {
        _animator.Update(dt);
    }


    public override void OnCollide(Entity other)
    {
        Game.Instance.RestartLevel();
    }


    public override void Render(IntPtr renderer, Sdl sdl)
    {
        unsafe
        {
            var r = (Renderer*)renderer;
            Rectangle<int> dest = new Rectangle<int>((int)_position.X,(int)_position.Y,(int)_width,(int)_height); 
            var src = _animator.GetFrame();
            int textureId = _animator.GetTextureId();
            Game.Instance.textures.Render(textureId,src,dest,flip);
        }
    }
}