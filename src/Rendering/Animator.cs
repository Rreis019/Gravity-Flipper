using Silk.NET.Maths;

namespace TheAdventure;

public class Animator
{
    private readonly Dictionary<string, Animation> _animations = new();

    private Animation _current = null!;
    private string _currentName = "";

    public void Add(string name, Animation animation)
    {
        _animations[name] = animation;
    }

    public void Play(string name)
    {
        if (!_animations.ContainsKey(name))
            return;

        if (_current == _animations[name])
            return;

        _current = _animations[name];
        _currentName = name;
    }


    public string GetCurrentAnimationName()
    {
        return _currentName;
    }

    public void Update(float dt)
    {
        _current?.Update(dt);
    }

    public Rectangle<int> GetFrame()
    {
        return _current?.GetCurrentFrame() 
               ?? new Rectangle<int>(0, 0, 0, 0);
    }

    public int GetTextureId()
    {
        return _current?.textureId ?? 0; // ou expõe property (melhor)
    }
}