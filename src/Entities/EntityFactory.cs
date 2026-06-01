using System;
using System.IO;
using Silk.NET.SDL;

namespace TheAdventure
{
    public enum EntityId
    {
        Player = 0,
        
        Apple = 1,
        Cherrie = 2,
        Banana = 3,

        Spike,
        SpikeInverted,
        Saw, //Saw but sometimes is off
        SawNeverStops,
        RockHead,

        MaxEntities
    }

    public static class EntityFactory
    {
        public static Entity Create(EntityId id, float x, float y)
        {
            Animation fruitIdleAnimation = new Animation(
                spriteSheetId: 0,
                frameWidth: 32,
                frameHeight: 32,
                frameCount: 11,
                frameTime: 0.08f,
                loop: true
            );

            switch (id)
            {
                case EntityId.Player:
                {
                    Player p = new Player(x, y);

                    // Temporary bounds
                    p.SetWorldBounds(0, 800 / 2f, 50f, 700f);

                    p.id = (short)EntityId.Player;

                    return p;
                }

                case EntityId.Apple:
                {
                    int tex = LoadTex(Fruit("Apple.png"));

                    var anim = Anim(tex, 32, 32, 11, 0.08f, true);

                    var apple = new Collectible(x, y, 32, 32, tex, anim, 1)
                    {
                        id = (short)EntityId.Apple
                    };

                    return apple;
                }
                case EntityId.Banana:
                {
                    int tex = LoadTex(Fruit("Bananas.png"));

                    var anim = Anim(tex, 32, 32, 11, 0.08f, true);

                    return new Collectible(x, y, 32, 32, tex, anim, 1)
                    {
                        id = (short)EntityId.Banana
                    };
                }
                case EntityId.Cherrie:
                {
                    int tex = LoadTex(Fruit("Cherries.png"));

                    var anim = Anim(tex, 32, 32, 11, 0.08f, true);

                    return new Collectible(x, y, 32, 32, tex, anim, 1)
                    {
                        id = (short)EntityId.Cherrie
                    };
                }
                case EntityId.Spike:
                {
                    int tex = LoadTex(Trap("Spikes/Idle.png"));

                    var anim = Anim(tex, 16, 16, 1, 1f, false);

                    return new Trap(x, y, 16, 16, tex, anim)
                    {
                        id = (short)EntityId.Spike
                    };
                }
                case EntityId.SpikeInverted:
                {
                    TextureData spikeTextureData;
                    // Load texture
                    int spikeTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Traps/Spikes/", "Idle.png"),out spikeTextureData
                    );

                    Animation spikeIdleAnimation = new Animation(
                        spriteSheetId: spikeTextureId,
                        frameWidth: 16,
                        frameHeight: 16,
                        frameCount: 1,
                        frameTime: 1f,
                        loop: false
                    );

                    Trap spike = new Trap(
                        x,
                        y,
                        16,
                        16,
                        spikeTextureId,
                        spikeIdleAnimation
                    );

                    spike.id = (short)EntityId.SpikeInverted;
                    spike.setFlippedVertically();
                    return spike;
                }
                case EntityId.Saw:
                case EntityId.SawNeverStops:
                {
                    int on = LoadTex(Trap("Saw/On (38x38).png"));
                    int off = LoadTex(Trap("Saw/Off.png"));

                    var onAnim = Anim(on, 38, 38, 8, 0.12f, true);
                    var offAnim = Anim(off, 38, 38, 1, 0.08f, false);

                    float speed = id == EntityId.SawNeverStops ? 0f : 2.5f;

                    var saw = new Saw(
                        x, y,
                        38, 38,
                        on,
                        off,
                        onAnim,
                        offAnim,
                        5f,
                        speed
                    );

                    saw.id = (short)id;
                    return saw;
                }
                case EntityId.RockHead:
                {
                    TextureData idleTextureData,blinkTextureData,hitTextureData;

                    int idleTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Traps/Rock Head/", "Idle.png"),out idleTextureData
                    );

                    int blinkTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Traps/Rock Head/", "Blink (42x42).png"),out blinkTextureData
                    );

                    int bottomhitTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Traps/Rock Head/", "Bottom Hit (42x42).png"),out hitTextureData
                    );

                    int tophitTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Traps/Rock Head/", "Top Hit (42x42).png"),out hitTextureData
                    );

                    int lefthitTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Traps/Rock Head/", "Left Hit (42x42).png"),out hitTextureData
                    );

                    int righthitTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Traps/Rock Head/", "Right Hit (42x42).png"),out hitTextureData
                    );



                    Animation headIdleAnimation = new Animation(
                        spriteSheetId: idleTextureId,
                        frameWidth: 42,
                        frameHeight: 42,
                        frameCount: 1,
                        frameTime: 1f,
                        loop: false
                    );

                    Animation headBlinkIdleAnimation = new Animation(
                        spriteSheetId: blinkTextureId,
                        frameWidth: 42,
                        frameHeight: 42,
                        frameCount: 4,
                        frameTime: 0.7f,
                        loop: true
                    );

                    Animation headBottomHitAnimation = new Animation(
                        spriteSheetId: bottomhitTextureId,
                        frameWidth: 42,
                        frameHeight: 42,
                        frameCount: 2,
                        frameTime: 0.3f,
                        loop: false
                    );


                    Animation headTopHitAnimation = new Animation(
                        spriteSheetId: tophitTextureId,
                        frameWidth: 42,
                        frameHeight: 42,
                        frameCount: 2,
                        frameTime: 0.3f,
                        loop: false
                    );

                    Animation headLeftHitAnimation = new Animation(
                        spriteSheetId: lefthitTextureId,
                        frameWidth: 42,
                        frameHeight: 42,
                        frameCount: 2,
                        frameTime: 0.3f,
                        loop: false
                    );

                    Animation headRightHitAnimation = new Animation(
                        spriteSheetId: righthitTextureId,
                        frameWidth: 42,
                        frameHeight: 42,
                        frameCount: 2,
                        frameTime: 0.3f,
                        loop: false
                    );

                    SmashHead smashHead = new SmashHead(
                        x,
                        y,
                        42,
                        42,
                        idleTextureId,
                        headIdleAnimation,
                        headBlinkIdleAnimation,
                        headTopHitAnimation,
                        headBottomHitAnimation,
                        headLeftHitAnimation,
                        headRightHitAnimation,
                        SmashMoveType.Vertical,
                        Direction.Down,
                        100
                    );

                    smashHead.id = (short)EntityId.RockHead;

                    return smashHead;
                }


                default:
                    throw new ArgumentException("Unknown EntityId: " + id);
            }
        }


        private static int LoadTex(string path)
        {
            return Game.Instance.textures.LoadTexture(path, out _);
        }

        private static string Fruit(string file)
        {
            return Path.Combine("assets/Items/Fruits/", file);
        }

        private static string Trap(string file)
        {
            return Path.Combine("assets/Traps/", file);
        }

        private static Animation Anim(int texId, int w, int h, int frames, float time, bool loop)
        {
            return new Animation(texId, w, h, frames, time, loop);
        }
    }
}