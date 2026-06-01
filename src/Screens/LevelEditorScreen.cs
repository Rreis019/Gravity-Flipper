using Silk.NET.SDL;
using Silk.NET.Maths;

namespace TheAdventure;

public class LevelEditorScreen : IScreen
{

    private enum EditMode
    {
        Collider,
        Entity,
        Tile
    }

    private bool _isPlaying = false;
    public bool isPlaying => _isPlaying;
    private const string _editorLevelName = "EditorLevel";

    private EditMode _mode = EditMode.Collider;

    private Camera2D Camera => Game.Instance.mainCamera;

    private Vector2D<float> _mouseWorld;

    private int _indexEntity = 0;
    private int _indexTile = 0;

    // drag collider
    private bool _isDragging;
    private Vector2D<float> _dragStart;
    private Vector2D<float> _dragEnd;

    public void OnEnter() { 
        Game.Instance.entities.Clear();
        Game.Instance.tiles.Clear();
        Game.Instance.SetWhiteBackgroundColor();
        Collider.debugRender = true;
    }
    public void OnExit() { 
        Game.Instance.SetDefaultBackgroundColor();
        Collider.debugRender = false;
    }

    public void Update(float dt, InputManager input)
    {

        if (input.IsKeyPressed(KeyCode.P))
        {
            if (!_isPlaying)
            {
                Game.Instance.SaveLevel(_editorLevelName);
                _isPlaying = true;
            }
        }

        if (input.IsKeyPressed(KeyCode.Escape))
        {
            if (_isPlaying)
            {
                _isPlaying = false;
                Game.Instance.LoadLevel(_editorLevelName);
            }
        }

        if (_isPlaying)
        {
            Game g = Game.Instance;

            g.background.Update(dt);
            g.entities.Update(dt, input);

            return; 
        }


        HandleModeSwitch(input);
        HandleCamera(input, dt);
        HandleSwitchObject(input);

        _mouseWorld = GetMouseWorld(input);



        // ---------------- COLLIDER DRAG ----------------
        if (_mode == EditMode.Collider)
        {
            if (input.IsMouseLeftClicked())
            {
                _isDragging = true;
                _dragStart = SnapVec(_mouseWorld);
            }

            if (_isDragging){
                _dragEnd = SnapVec(_mouseWorld);
            }

            if (input.IsMouseLeftReleased() && _isDragging)
            {
                _isDragging = false;
                SpawnCollider(_dragStart, _dragEnd);
            }
        }
        else
        {
            if (input.IsMouseLeftClicked())
                Spawn(_mouseWorld);

            if (input.IsMouseRightClicked())
                Delete(_mouseWorld);
        }

        if(input.IsKeyPressed(KeyCode.J))
        {
            Game.Instance.SaveLevel(_editorLevelName);
        }

        if(input.IsKeyPressed(KeyCode.K))
        {
            Game.Instance.LoadLevel(_editorLevelName);
        }

        if(input.IsKeyPressed(KeyCode.M))
        {
            Game.Instance.screens.SetScreen(Game.Instance.titleScreen);
        }


        Game.Instance.entities.RemoveInactivesEntities();
    }

    public void Render(IntPtr renderer, Sdl sdl)
    {
        Game.Instance.tiles.Render(renderer, sdl);
        Game.Instance.entities.Render(renderer, sdl);

        if (!_isPlaying)
        {
            DrawCursorPreview(renderer, sdl);
            DrawUI();
        }else{
            Game.Instance.defaultBlackFont.DrawText("ESC = BACK TO EDITOR", 5, 5,1);
        }
    }

    // -----------------Draw UI ----------------

    private void DrawUI()
    {
        Game g = Game.Instance;

        string mode = _mode switch
        {
            EditMode.Collider => "COLLIDER",
            EditMode.Entity => "ENTITY",
            EditMode.Tile => "TILE",
            _ => "UNKNOWN"
        };

        float scale = 0.5f;
        int gapY = 10;
        int startY = 5;


        g.defaultBlackFont.DrawText($"MODE: {mode}", 5, startY,scale); startY+= gapY;
        g.defaultBlackFont.DrawText("C = COLLIDER MODE", 5, startY,scale); startY+= gapY;
        g.defaultBlackFont.DrawText("O = ENTITY MODE", 5, startY,scale); startY+= gapY;
        g.defaultBlackFont.DrawText("T = TILE MODE", 5, startY,scale); startY+= gapY;

        g.defaultBlackFont.DrawText("WASD = MOVE CAMERA", 5, startY,scale); startY+= gapY;

        g.defaultBlackFont.DrawText("LEFT CLICK = PLACE", 5, startY,scale); startY+= gapY;
        g.defaultBlackFont.DrawText("RIGHT CLICK = DELETE", 5, startY,scale); startY+= gapY;

        g.defaultBlackFont.DrawText("Q OR E = CHANGE OBJECT", 5, startY,scale); startY+= gapY;

        g.defaultBlackFont.DrawText("J = SAVE LEVEL", 5, startY,scale); startY+= gapY;
        g.defaultBlackFont.DrawText("K = LOAD LEVEL", 5, startY,scale); startY+= gapY;
        g.defaultBlackFont.DrawText("P = PLAY LEVEL", 5, startY,scale); startY+= gapY;
        g.defaultBlackFont.DrawText("M = GO TO TITLESCREEN", 5, startY,scale); startY+= gapY;
    }


    // ---------------- PREVIEW ----------------

    private void DrawCursorPreview(IntPtr renderer, Sdl sdl)
    {
        switch (_mode)
        {
            case EditMode.Collider:
                if (_isDragging)
                    DrawRect(renderer, SnapVec(_dragStart), SnapVec(_dragEnd));
                else
                    DrawRect(renderer, _mouseWorld, _mouseWorld);
                break;

            case EditMode.Entity:
                DrawEntityPreview(renderer, sdl, _mouseWorld);
                break;

            case EditMode.Tile:
                DrawTile(renderer, SnapVec(_mouseWorld));
                break;
        }
    }

    // ---------------- CAMERA ----------------

    private void HandleCamera(InputManager input, float dt)
    {
        float speed = 200f * dt;

        if (input.IsKeyDown(KeyCode.W))
            Camera.position.Y -= speed;

        if (input.IsKeyDown(KeyCode.S))
            Camera.position.Y += speed;

        if (input.IsKeyDown(KeyCode.A))
            Camera.position.X -= speed;

        if (input.IsKeyDown(KeyCode.D))
            Camera.position.X += speed;
    }

    // ---------------- MODE SWITCH ----------------

    private void HandleModeSwitch(InputManager input)
    {
        if (input.IsKeyPressed(KeyCode.C))
            _mode = EditMode.Collider;

        if (input.IsKeyPressed(KeyCode.O))
            _mode = EditMode.Entity;

        if (input.IsKeyPressed(KeyCode.T))
            _mode = EditMode.Tile;
    }

    // ---------------- SWITCH OBJECT ----------------

    private void HandleSwitchObject(InputManager input)
    {
        if (_mode == EditMode.Entity)
        {
            if (input.IsKeyPressed(KeyCode.Q)) _indexEntity--;
            if (input.IsKeyPressed(KeyCode.E)) _indexEntity++;

            _indexEntity = Math.Clamp(_indexEntity, 0, (int)EntityId.MaxEntities - 1);
        }

        if (_mode == EditMode.Tile)
        {
            if (input.IsKeyPressed(KeyCode.Q)) _indexTile--;
            if (input.IsKeyPressed(KeyCode.E)) _indexTile++;

            _indexTile = Math.Clamp(_indexTile, 0, Game.Instance.tileset.tileCount - 1);
        }
    }

    // ---------------- SPAWN ----------------

    private void Spawn(Vector2D<float> pos)
    {
        switch (_mode)
        {
            case EditMode.Entity:
                Game.Instance.entities.Add(
                    EntityFactory.Create((EntityId)_indexEntity, pos.X, pos.Y)
                );
                break;

            case EditMode.Tile:
                Game.Instance.tiles.Add(
                    new Tile(_indexTile, Snap(pos.X), Snap(pos.Y))
                );
                break;
        }
    }

    private void SpawnCollider(Vector2D<float> a, Vector2D<float> b)
    {
        float x = MathF.Min(a.X, b.X);
        float y = MathF.Min(a.Y, b.Y);

        float w = MathF.Abs(a.X - b.X);
        float h = MathF.Abs(a.Y - b.Y);

        if (w < 4 || h < 4) return;

        Game.Instance.entities.Add(
            new InvisibleCollider(x, y, (int)w, (int)h)
        );
    }

    // ---------------- DELETE ----------------

    private void Delete(Vector2D<float> pos)
    {
        switch (_mode)
        {
            case EditMode.Collider:
                Game.Instance.entities.RemoveAtPositionSameType<InvisibleCollider>(pos);
                break;
            case EditMode.Entity:
                Game.Instance.entities.RemoveAtPosition(pos);
                break;
            case EditMode.Tile:
                Game.Instance.tiles.RemoveAtPosition(SnapVec(pos));
                break;        
        }
    }

    // ---------------- MOUSE WORLD ----------------

    private Vector2D<float> GetMouseWorld(InputManager input)
    {
        var cam = Game.Instance.mainCamera;
        var mouse = input.GetMousePosition();

        return new Vector2D<float>(
            mouse.X / cam.zoom + cam.position.X,
            mouse.Y / cam.zoom + cam.position.Y
        );
    }

    // ---------------- DRAW RECT ----------------

    private unsafe void DrawRect(IntPtr renderer, Vector2D<float> a, Vector2D<float> b)
    {
        var g = Game.Instance;
        var sdl = g.sdl;
        var cam = g.mainCamera;

        float x = MathF.Min(a.X, b.X);
        float y = MathF.Min(a.Y, b.Y);
        float w = MathF.Abs(a.X - b.X);
        float h = MathF.Abs(a.Y - b.Y);

        // camera transform
        x = (x - cam.position.X) * cam.zoom;
        y = (y - cam.position.Y) * cam.zoom;

        w *= cam.zoom;
        h *= cam.zoom;

        var r = (Renderer*)renderer;

        var rect = new Rectangle<int>(
            (int)x,
            (int)y,
            (int)w,
            (int)h
        );

        sdl.SetRenderDrawColor(r, 255, 0, 0, 255);
        sdl.RenderDrawRect(r, in rect);
    }
    private void DrawTile(IntPtr renderer, Vector2D<float> pos) { 
        Game.Instance.tiles.RenderTile(_indexTile,Snap(pos.X),Snap(pos.Y));
    }


    private void DrawEntityPreview(IntPtr renderer, Sdl sdl, Vector2D<float> pos)
    {
        Entity e = EntityFactory.Create((EntityId)_indexEntity,pos.X,pos.Y);
        e.Render(renderer,sdl);
    }



    // ---------------- UTIL ----------------

    private Vector2D<float> SnapVec(Vector2D<float> v)
        => new Vector2D<float>(Snap(v.X), Snap(v.Y));

private int Snap(float v)
    => (int)(v / 16) * 16;

}