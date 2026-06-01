using System;
using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public class Collectible : Entity
{
    private int _score;
    private int _width,_height;
    private int _textureId;
    Animation _idleAnimation;
    private Animator _animator;

    public Collectible(float x, float y,int width,int height,int textureId, Animation idleAnimation,int score)
        : base(x, y)
    {
        _animator = new Animator();

        _textureId = textureId;
        _idleAnimation = idleAnimation;
        _score = score;

        _width = width;
        _height = height;

        _animator.Add("idle", _idleAnimation);
        _animator.Play("idle");

        isStatic   = true;
        hasPhysics = true;
        collider = new Collider(8,8,16,16,ColliderType.Trigger);
    }

    public override void Update(float dt, InputManager input)
    {
        _animator.Update(dt);
    }


    public override void OnCollide(Entity other)
    {
        //Destroy the object
        this.isActive = false;

        //TODO : Spawn Particle
        //...

        Game g = Game.Instance; 

        if(!g.levelEditor.isPlaying)
        {
            g.entities.fruitCount--;

            if(g.entities.fruitCount == 0){
                g.NextLevel();
            }
        }

    }


    public override void Render(IntPtr renderer, Sdl sdl)
    {
        unsafe
        {
            var r = (Renderer*)renderer;
            Rectangle<int> dest = new Rectangle<int>((int)_position.X,(int)_position.Y,(int)_width,(int)_height); 
            var src = _animator.GetFrame();
            int textureId = _animator.GetTextureId();
            Game.Instance.textures.Render(textureId,src,dest);
        }
    }
}