using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;


public class TitleScreen : IScreen
{
    private int _titleTextureId;
    private TextureData _titleTextureData;

    private int _playBtnTextureId,_playBtnHoverTextureId;
    private TextureData _playBtnTextureData;

    private int _LevelEditorBtnTextureId,_LevelEditorHoverBtnTextureId;
    private TextureData _LevelEditorBtnTextureData;

    private int _QuitBtnTextureId,_QuitBtnHoverTextureId;
    private TextureData _QuitBtnTextureData;


    public void OnEnter()
    {
        _titleTextureId = Game.Instance.textures.LoadTexture(Path.Combine("assets/Menu/Text/", "title.png"), out _titleTextureData);
        
        _playBtnTextureId = Game.Instance.textures.LoadTexture(Path.Combine("assets/Menu/Buttons/", "Play.png"), out _playBtnTextureData);
        _playBtnHoverTextureId = Game.Instance.textures.LoadTexture(Path.Combine("assets/Menu/Buttons/", "PlayHovered.png"), out _playBtnTextureData);
            
        _LevelEditorBtnTextureId = Game.Instance.textures.LoadTexture(Path.Combine("assets/Menu/Buttons/", "LevelEditor.png"), out _LevelEditorBtnTextureData);
        _LevelEditorHoverBtnTextureId = Game.Instance.textures.LoadTexture(Path.Combine("assets/Menu/Buttons/", "LevelEditorHovered.png"), out _LevelEditorBtnTextureData);
        

        _QuitBtnTextureId = Game.Instance.textures.LoadTexture(Path.Combine("assets/Menu/Buttons/", "Quit.png"), out _QuitBtnTextureData);
        _QuitBtnHoverTextureId = Game.Instance.textures.LoadTexture(Path.Combine("assets/Menu/Buttons/", "QuitHovered.png"), out _QuitBtnTextureData);
  

        Game.Instance.LoadLevel("levels/titlescreen");
    }
    

    public   void OnExit()
    {

    }

    public void Update(float dt, InputManager input)
    {
        Game g = Game.Instance;

        if (input.IsKeyPressed(KeyCode.Space))
        {
            g.screens.SetScreen(g.gameScreen);
        }

        if (input.IsKeyPressed(KeyCode.E))
        {
            g.screens.SetScreen(g.levelEditor);
        }

        Game.Instance.background.Update(dt);
        //Game.Instance.entities.Update(dt, input);
    }

    public void Render(IntPtr renderer, Sdl sdl)
    {
        Game g = Game.Instance;

        //g.defaultBlackFont.DrawText("PRESS SPACE TO START", 10,10);
        //g.defaultBlackFont.DrawText("PRESS E TO GO LEVEL EDITOR", 10,30);
        Game.Instance.background.Render();
        Game.Instance.tiles.Render(renderer, sdl);
        //Game.Instance.entities.Render(renderer, sdl);
    

        int y = 40;
        int x = g.baseWidth/2;
        int gapY = 2;

        g.textures.RenderUISimple(
            _titleTextureId,
            _titleTextureData,
            x -  (int)(_titleTextureData.Width * 0.2f),
            y,
            0.4f
        );
        y+= gapY + 3 + (int)(_titleTextureData.Height * 0.5);


        if(GUI.RenderButton(
            _playBtnTextureId,_playBtnHoverTextureId,
            _playBtnTextureData,x -  (int)(_playBtnTextureData.Width * 0.25f),
            y,0.5f
        )){
             g.screens.SetScreen(g.gameScreen);
        }

        y+= gapY + (int)(_playBtnTextureData.Height * 0.5);


        if(GUI.RenderButton(
            _LevelEditorBtnTextureId,_LevelEditorHoverBtnTextureId,
            _LevelEditorBtnTextureData,x -  (int)(_LevelEditorBtnTextureData.Width * 0.25f),
            y,0.5f
        )){
             g.screens.SetScreen(g.levelEditor);
        }

        y+= gapY + (int)(_LevelEditorBtnTextureData.Height * 0.5);

        if(GUI.RenderButton(
            _QuitBtnTextureId,_QuitBtnHoverTextureId,
            _QuitBtnTextureData,x -  (int)(_QuitBtnTextureData.Width * 0.25f),
            y,0.5f
        )){
            g.QuitGame();
        }

    }
}