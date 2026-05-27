using System;
using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public class Saw : Trap
{
    private Animation _onAnimation;

    private bool _isOn;
    private float _timer;

    private float _onSeconds;
    private float _offSeconds;

    public Saw(
        float x,
        float y,
        int width,
        int height,
        int offTextureId,
        int onTextureId,
        Animation onAnimation,
        Animation offAnimation,
        float onSeconds,
        float offSeconds
    ) : base(x, y, width, height, offTextureId, offAnimation)
    {
        _onAnimation = onAnimation;

        _onSeconds = onSeconds;
        _offSeconds = offSeconds;

        _animator.Add("on", _onAnimation);
        _animator.Add("idle", offAnimation);

        _animator.Play("idle");

        _isOn = false;
        _timer = 0f;


        collider = new Collider(0,0,_width,height,ColliderType.Trigger);
        collider.type = ColliderType.None;
    }

    public override void Update(float dt, InputManager input)
    {
        base.Update(dt, input);

        _timer += dt;

        float currentLimit = _isOn ? _onSeconds : _offSeconds;

        if (_timer >= currentLimit)
        {
            _timer = 0f;
            Toggle();
        }
    }

    private void Toggle()
    {
        _isOn = !_isOn;

        if (_isOn)
        {
            _animator.Play("on");
            collider.type = ColliderType.Trigger;
        }
        else
        {
            _animator.Play("idle");
            collider.type = ColliderType.None;
        }
    }

    public override void OnCollide(Entity other)
    {
        if (!_isOn)
            return;

        base.OnCollide(other);
    }

    public override void Render(IntPtr renderer, Sdl sdl)
    {
        base.Render(renderer, sdl);
    }
}