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
                    TextureData fruitTextureData;
                    // Load texture
                    int appleTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Items/Fruits/", "Apple.png"),out fruitTextureData
                    );

                    fruitIdleAnimation.textureId = appleTextureId;

                    Collectible apple = new Collectible(
                        x,
                        y,
                        32,
                        32,
                        appleTextureId,
                        fruitIdleAnimation,
                        1 // value / score
                    );

                    apple.id = (short)EntityId.Apple;

                    return apple;
                }
                case EntityId.Banana:
                {
                    TextureData bananasTextureData;
                    // Load texture
                    int bananasTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Items/Fruits/", "Bananas.png"),out bananasTextureData
                    );

                    fruitIdleAnimation.textureId = bananasTextureId;

                    Collectible banana = new Collectible(
                        x,
                        y,
                        32,
                        32,
                        bananasTextureId,
                        fruitIdleAnimation,
                        1 // value / score
                    );

                    banana.id = (short)EntityId.Banana;

                    return banana;
                }
                case EntityId.Cherrie:
                {
                    TextureData cherrieTextureData;
                    // Load texture
                    int cherrieTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Items/Fruits/", "Cherries.png"),out cherrieTextureData
                    );

                    fruitIdleAnimation.textureId = cherrieTextureId;

                    Collectible cherrie = new Collectible(
                        x,
                        y,
                        32,
                        32,
                        cherrieTextureId,
                        fruitIdleAnimation,
                        1 // value / score
                    );

                    cherrie.id = (short)EntityId.Cherrie;

                    return cherrie;
                }
                case EntityId.Spike:
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

                    spike.id = (short)EntityId.Spike;

                    return spike;
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
                {
                    TextureData sawOnTextureData,sawOffTextureData;
                    // Load texture
                    int sawOnTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Traps/Saw/", "On (38x38).png"),out sawOnTextureData
                    );

                    int sawOffTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Traps/Saw/", "Off.png"),out sawOffTextureData
                    );

                    Animation sawOnAnimation = new Animation(
                        spriteSheetId: sawOnTextureId,
                        frameWidth: 38,
                        frameHeight: 38,
                        frameCount: 8,
                        frameTime: 0.12f,
                        loop: true
                    );

                    Animation sawOffAnimation = new Animation(
                        spriteSheetId: sawOffTextureId,
                        frameWidth: 38,
                        frameHeight: 38,
                        frameCount: 1,
                        frameTime: 0.08f,
                        loop: false
                    );

                    Saw saw = new Saw(
                        x,
                        y,
                        38,
                        38,
                        sawOnTextureId,
                        sawOffTextureId,
                        sawOnAnimation,
                        sawOffAnimation,
                        5f,
                        2.5f
                    );

                    saw.id = (short)EntityId.Saw;

                    return saw;
                }
                case EntityId.SawNeverStops:
                {
                    TextureData sawOnTextureData,sawOffTextureData;
                    // Load texture
                    int sawOnTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Traps/Saw/", "On (38x38).png"),out sawOnTextureData
                    );

                    int sawOffTextureId = Game.Instance.textures.LoadTexture(
                        Path.Combine("assets/Traps/Saw/", "Off.png"),out sawOffTextureData
                    );

                    Animation sawOnAnimation = new Animation(
                        spriteSheetId: sawOnTextureId,
                        frameWidth: 38,
                        frameHeight: 38,
                        frameCount: 8,
                        frameTime: 0.12f,
                        loop: true
                    );

                    Animation sawOffAnimation = new Animation(
                        spriteSheetId: sawOffTextureId,
                        frameWidth: 38,
                        frameHeight: 38,
                        frameCount: 1,
                        frameTime: 0.08f,
                        loop: false
                    );

                    Saw saw = new Saw(
                        x,
                        y,
                        38,
                        38,
                        sawOnTextureId,
                        sawOffTextureId,
                        sawOnAnimation,
                        sawOffAnimation,
                        5f,
                        0f
                    );

                    saw.id = (short)EntityId.SawNeverStops;

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
    }
}