using System;
using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public enum SmashMoveType
{
    Horizontal,
    Vertical,
    Both
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public enum SmashState
{
    Moving,
    IdlePause,
    HitPause
}

public class SmashHead : Trap
{
    private Animation _blinkAnimation;
    private Animation _topHitAnimation;
    private Animation _bottomHitAnimation;
    private Animation _leftHitAnimation;
    private Animation _rightHitAnimation;

    private SmashMoveType _moveType;
    private float _speed;

    private Direction _initialDirection;

    private SmashState _state;

    private float _stateTimer;
    private float _hitPauseDuration = 2f;

    public SmashHead(
        float x,
        float y,
        int width,
        int height,
        int textureId,
        Animation idleAnimation,
        Animation blinkAnimation,
        Animation topHitAnimation,
        Animation bottomHitAnimation,
        Animation leftHitAnimation,
        Animation rightHitAnimation,
        SmashMoveType moveType,
        Direction initialDirection,
        float speed
    ) : base(x, y, width, height, textureId, idleAnimation)
    {
        _blinkAnimation = blinkAnimation;
        _topHitAnimation = topHitAnimation;
        _bottomHitAnimation = bottomHitAnimation;
        _leftHitAnimation = leftHitAnimation;
        _rightHitAnimation = rightHitAnimation;

        _moveType = moveType;
        _speed = speed;

        isStatic = false;
        hasPhysics = true;

        collider = new Collider(4, 4, _width - 9, _height - 9, ColliderType.Solid);

        //_animator.Add("idle", _idleAnimation);
        _animator.Add("blink", _blinkAnimation);

        _animator.Add("topHit", _topHitAnimation);
        _animator.Add("bottomHit", _bottomHitAnimation);
        _animator.Add("leftHit", _leftHitAnimation);
        _animator.Add("rightHit", _rightHitAnimation);

        _initialDirection = initialDirection;
        SetVelocity(initialDirection);

        _state = SmashState.Moving;
        _animator.Play("blink");
    }



    public override void Update(float dt, InputManager input)
    {
        base.Update(dt, input);

        _stateTimer += dt;

        switch (_state)
        {
            case SmashState.Moving:
                SetVelocity(_initialDirection);
                _animator.Play("blink");
                break;

            case SmashState.IdlePause:
                _animator.Play("idle");

                if (_stateTimer >= 1f)
                {
                    _stateTimer = 0f;
                    _state = SmashState.Moving;
                }
                break;

            case SmashState.HitPause:
                StopMovement();
                HandleHitAnimation();

                if (_stateTimer >= _hitPauseDuration)
                {
                    _stateTimer = 0f;
                    _state = SmashState.Moving;
                    _animator.Play("blink");
                }
                break;
        }

        _animator.Update(dt);
    }

    private void StopMovement()
    {
        _velocity.X = 0;
        _velocity.Y = 0;
    }

    private void SetVelocity(Direction dir)
    {
        _velocity = dir switch
        {
            Direction.Up    => new Vector2D<float>(0, -1 * _speed),
            Direction.Down  => new Vector2D<float>(0,  1 * _speed),
            Direction.Left  => new Vector2D<float>(-1 * _speed, 0),
            Direction.Right => new Vector2D<float>( 1 * _speed, 0),
            _ => new Vector2D<float>(0, 0)
        };
    }

    private void ReverseDirection()
    {
        _speed *= -1;
    }

    private void HandleHitAnimation()
    {
        if (_velocity.X > 0)
            _animator.Play("rightHit");
        else if (_velocity.X < 0)
            _animator.Play("leftHit");
        else if (_velocity.Y > 0)
            _animator.Play("bottomHit");
        else
            _animator.Play("topHit");
    }

    public override void OnCollide(Entity other)
    {
        base.OnCollide(other);

        if (other.id != 1337)
            return;

        ReverseDirection();

        _state = SmashState.HitPause;
        _stateTimer = 0f;
    }

    public override void Render(IntPtr renderer, Sdl sdl)
    {
        base.Render(renderer, sdl);
    }
}