using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;


public class GameScreen : IScreen
{
    private int _backgroundTextureId;
    private TextureData _backgroundTextureData;
    private float _backgroundScrollY = 0.0f;



    private void RenderBackground()
    {
        Game g = Game.Instance;

        int minX = g.tiles.GetMinX();
        int minY = g.tiles.GetMinY();

        int maxX = g.tiles.GetMaxX();
        int maxY = g.tiles.GetMaxY();

        int texW = _backgroundTextureData.Width;
        int texH = _backgroundTextureData.Height;

        for (int y = minY - texH; y < maxY + texH; y += texH)
        {
            for (int x = minX - texW; x < maxX + texW; x += texW)
            {
                int drawY = (int)(y + (_backgroundScrollY % texH));


                Rectangle<int> src = new Rectangle<int>(
                    0,
                    0,
                    texW,
                    texH
                );

                Rectangle<int> dest = new Rectangle<int>(
                    x,
                    drawY,
                    texW,
                    texH
                );

                Game.Instance.textures.Render(_backgroundTextureId,src,dest);
            }
        }
    }


    public   void OnEnter()
    {

        Game g = Game.Instance;

        _backgroundTextureId = g.textures.LoadTexture(Path.Combine("assets/Background/", "Yellow.png"), out _backgroundTextureData);



        //TODO : Just testing the entitites remove after and use levels
        /*
        Entity p = EntityFactory.Create(EntityId.Player,150,100);
        Entity apple = EntityFactory.Create(EntityId.Apple,50,100);

        Entity wall = (Entity)new InvisibleCollider(0,200,250,10);
        Entity wall2 = (Entity)new InvisibleCollider(0,0,250,10);


        Entity saw = EntityFactory.Create(EntityId.Saw,50,100);

        Entity smashHead = EntityFactory.Create(EntityId.RockHead,90,100);

        Entity spike = EntityFactory.Create(EntityId.Spike,90,100);



        g.entities.Add(p);
        g.entities.Add(apple);
        g.entities.Add(wall);
        g.entities.Add(wall2);

        g.entities.Add(saw);
        g.entities.Add(smashHead);

        g.entities.Add(spike);
        
        g.tiles.Add(new Tile(0,0,0));
        g.tiles.Add(new Tile(1,32,0));
        */
        g.LoadLevel("levels/level2");
    }
    

    public   void OnExit()
    {

    }


    public void Update(float dt, InputManager input)
    {
        _backgroundScrollY += 30.0f * dt;
        Game.Instance.entities.Update(dt, input);
    }

    public void Render(IntPtr renderer, Sdl sdl)
    {
        RenderBackground();

        Game.Instance.tiles.Render(renderer, sdl);
        Game.Instance.entities.Render(renderer, sdl);
    }
}