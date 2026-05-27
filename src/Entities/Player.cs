using System;
using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public class Player : Entity
{
    private Vector2D<float> _previousVelocity;
    private int _width,_height;

    public float Speed = 100f;

    // true = gravity goes down, false = gravity goes up
    public bool GravityDown = true;


    private WorldBounds _bounds = new WorldBounds(0,0,0,0);


    //Animations
    private int _runTextureId;
    private TextureData _runTextureData;
    
    private  int _idleTextureId;
    private  TextureData _idleTextureData;
    
    
    private  int _fallTextureId;
    private  TextureData _fallTextureData;
 
    private  bool flipSpriteHorizontal = false;

    Animation _runAnimation;    
    Animation _idleAnimation;
    Animation _fallAnimation;
    Animator _animator;


    void setNormalCollider(){
        collider = new Collider(4,5,_width - 8,_height - 5,ColliderType.Solid);
    }

    void setFlippedCollider(){
        collider = new Collider(4,0,_width - 8,_height - 6,ColliderType.Solid);
    }


    public Player(float x, float y) : base(x, y)
    {
        LoadTextures();

        _animator = new Animator();

        _idleAnimation = new Animation(
            spriteSheetId: _idleTextureId,
            frameWidth: 32,
            frameHeight: 32,
            frameCount : 11,
            frameTime: 0.08f,
            loop: true
        );

        _runAnimation = new Animation(
            spriteSheetId: _runTextureId,
            frameWidth: 32,
            frameHeight: 32,
            frameCount : 11,
            frameTime: 0.08f,
            loop: true
        );

        _fallAnimation = new Animation(
            spriteSheetId: _fallTextureId,
            frameWidth: 32,
            frameHeight: 32,
            frameCount : 1,
            frameTime: 0.08f,
            loop: true
        );

        _width = _height = 32;

        isStatic = false;
        hasPhysics = true;

        setNormalCollider();

        _animator.Add("idle", _idleAnimation);
        _animator.Add("run", _runAnimation);
        _animator.Add("fall", _fallAnimation);
        _animator.Play("idle");
    }

    private void LoadTextures(){
        Game g = Game.Instance;
        _runTextureId = g.textures.LoadTexture(Path.Combine("assets/Main Characters/Ninja Frog/", "Run (32x32).png"), out _runTextureData);
        _idleTextureId = g.textures.LoadTexture(Path.Combine("assets/Main Characters/Ninja Frog/", "Idle (32x32).png"), out _idleTextureData);
        _fallTextureId = g.textures.LoadTexture(Path.Combine("assets/Main Characters/Ninja Frog/", "Fall (32x32).png"), out _fallTextureData);
    }

    public override void Update(float dt,InputManager input)
    {
        //Animation
        if(_velocity.Y != 0)
        {
            _animator.Play("fall");
        }
        else if(_previousVelocity.X != _velocity.X || _previousVelocity.Y != _velocity.Y)
        {
            if(_velocity.X == 0){
                _animator.Play("idle");
            }

            if(_velocity.X != 0){
                _animator.Play("run");
            }
        }
        
        
        _animator.Update(dt);


        if (input.IsKeyPressed(KeyCode.Space))
        {
            FlipGravity();
        }

        _velocity.X = 0;

        if (input.IsKeyDown(KeyCode.A))
        {
            _velocity.X = -Speed;
            flipSpriteHorizontal = true;
        }
        if (input.IsKeyDown(KeyCode.D))
        {
            _velocity.X = Speed;
            flipSpriteHorizontal = false;
        }

        // Apply custom gravity
        float gravity = GravityDown ? 50f : -50f;
        _velocity.Y += gravity;

        
        _previousVelocity = _velocity;
    }

    public override void Render(IntPtr renderer,Sdl sdl)
    {
        unsafe
        {
            RendererFlip flip = RendererFlip.None;

            if(flipSpriteHorizontal)
                flip |= RendererFlip.Horizontal;

            if (GravityDown == false)
                flip |= RendererFlip.Vertical;


            var r = (Renderer*)renderer;
            Rectangle<int> dest = new Rectangle<int>((int)_position.X,(int)_position.Y,(int)_width,(int)_height); 
            var src = _animator.GetFrame();
            int textureId = _animator.GetTextureId();
            Game.Instance.textures.Render(textureId,src,dest,flip);
        }
    }

    public override void OnCollide(Entity other)
    {

    }


    public void SetWorldBounds(float left,float right, float top, float bottom)
    {
        _bounds = new WorldBounds(left,right,top, bottom);
    }

    public void FlipGravity()
    {
        // Invert gravity direction
        GravityDown = !GravityDown;

        // Reset vertical velocity to avoid unwanted bounce effect
        _velocity.Y = 0;

        if(GravityDown == false){
            setFlippedCollider();
        }else{
            setNormalCollider();
        }
    }
}