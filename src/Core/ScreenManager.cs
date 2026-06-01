using System;
using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public class ScreenManager
{
    private IScreen _currentScreen = null!;

    public void SetScreen(IScreen newScreen)
    {
        // Leaving the current one
        _currentScreen?.OnExit();

        // Change
        _currentScreen = newScreen;

        // Entering on new screen
        _currentScreen?.OnEnter();
    }

    public void Update(float dt, InputManager input)
    {
        _currentScreen?.Update(dt, input);
    }

    public void Render(IntPtr renderer, Sdl sdl)
    {
        _currentScreen?.Render(renderer, sdl);
    }
}