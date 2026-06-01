using Silk.NET.Maths;
using Silk.NET.SDL;

namespace TheAdventure;

public abstract class Entity
{
    public short id;

    protected Vector2D<float> _position;
    protected Vector2D<float> _velocity;

    public Collider collider = null!;

    public bool isActive = true;
    public bool isStatic = false;
    public bool hasPhysics = false;

    protected Entity(float x, float y)
    {
        _position = new Vector2D<float>(x, y);
        _velocity = new Vector2D<float>(0, 0);
    }

    public Vector2D<float> position
    {
        get => _position;
        set => _position = value;
    }

    public float X
    {
        get => _position.X;
        set => _position.X = value;
    }

    public float Y
    {
        get => _position.Y;
        set => _position.Y = value;
    }

    public Vector2D<float> velocity => _velocity;

    protected void SetVelocity(Vector2D<float> v)
    {
        _velocity = v;
    }

    public void SetVelocityX(float x)
    {
        _velocity.X = x;
    }

    public void SetVelocityY(float y)
    {
        _velocity.Y = y;
    }

    public abstract void Update(float deltaTime, InputManager input);
    
    public abstract void Render(IntPtr renderer, Sdl sdl);

    public virtual void OnCollide(Entity other) { }
}