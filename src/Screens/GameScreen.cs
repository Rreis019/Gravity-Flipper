using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;


public class GameScreen : IScreen
{
    private int _backBtnTextureId,_RestartBtnTextureId;
    private TextureData _backBtnTextureData;

    public void OnEnter()
    {
        Game g = Game.Instance;
        _backBtnTextureId = Game.Instance.textures.LoadTexture(Path.Combine("assets/Menu/Buttons/", "Back.png"), out _backBtnTextureData);
        _RestartBtnTextureId = Game.Instance.textures.LoadTexture(Path.Combine("assets/Menu/Buttons/", "Restart.png"), out _backBtnTextureData);

        g.LoadCurrentLevel();
        Collider.debugRender = false;
    }
    

    public   void OnExit()
    {

    }

    public void Update(float dt, InputManager input)
    {
        Game g = Game.Instance;
        if(!g.isGameWon){
            g.entities.Update(dt, input);
        }else{
            if(input.IsKeyPressed(KeyCode.Return))
            {
                g.currentLevel = 0;
                g.isGameWon = false;
                g.screens.SetScreen(Game.Instance.titleScreen);
            }
        }

        g.background.Update(dt);
    }

    public void Render(IntPtr renderer, Sdl sdl)
    {
        Game g = Game.Instance;

        g.background.Render();
        g.tiles.Render(renderer, sdl);
        g.entities.Render(renderer, sdl);
        
        if(g.isGameWon){
            ShowWinMessage(renderer,sdl);
        }

        if(GUI.RenderButton(
            _backBtnTextureId,_backBtnTextureId,
            _backBtnTextureData,5,5,0.5f
        )){
             g.screens.SetScreen(g.titleScreen);
        }

        if(GUI.RenderButton(
            _RestartBtnTextureId,_RestartBtnTextureId,
            _backBtnTextureData,17,5,0.5f
        )){
             g.RestartLevel();
        }
    }

    private void ShowWinMessage(IntPtr renderer, Sdl sdl)
    {
        unsafe
        {
            Game g = Game.Instance;

            int x = g.baseWidth / 2;
            int y = g.baseHeight / 2;

            float scale = 1.0f;

            int charWidth = (int)(8 * scale);

            // Linha mais longa:
            // "PRESS ENTER TO RETURN TO THE MAIN MENU."
            int longestLineChars = 38;

            int textWidth = longestLineChars * charWidth;
            int textHeight = 3 * 20;

            int padding = 16;

            int boxX = x - textWidth / 2 - padding;
            int boxY = y - padding;

            int boxWidth = textWidth + padding * 2;
            int boxHeight = textHeight + padding * 2;

            var r = (Renderer*)renderer;

            // Borda branca (retângulo exterior)
            sdl.SetRenderDrawColor(r, 255, 255, 255, 255);

            var borderRect = new Rectangle<int>(
                boxX - 2,
                boxY - 2,
                boxWidth + 4,
                boxHeight + 4
            );

            sdl.RenderFillRect(r, ref borderRect);

            // Fundo escuro (33,31,48)
            sdl.SetRenderDrawColor(r, 33, 31, 48, 255);

            var fillRect = new Rectangle<int>(
                boxX,
                boxY,
                boxWidth,
                boxHeight
            );

            sdl.RenderFillRect(r, ref fillRect);

            // Texto
            int textY = y;

            g.defaultWhiteFont.DrawText(
                "ALL LEVELS COMPLETED!",
                x - 8 * 21 / 2,
                textY,
                scale
            );

            textY += 20;

            g.defaultWhiteFont.DrawText(
                "THANK YOU FOR PLAYING.",
                x - 8 * 22 / 2,
                textY,
                scale
            );

            textY += 20;

            g.defaultWhiteFont.DrawText(
                "PRESS ENTER TO RETURN TO THE MAIN MENU.",
                x - 8 * 38 / 2,
                textY,
                scale
            );
        }
    }

}