using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprite_animado;

public class Animation
{
    private Texture2D spriteSheet;
    public int frameWidth, frameHeight;
    private int frameCount;
    private int currentFrame;
    private double frameTime, elapsedTime;

    public bool isLooping { get; set; } = true;

    public Animation(Texture2D texture, int frameWidth, int frameHeight, int frameCount)
    {
        this.spriteSheet = texture;
        this.frameWidth = frameWidth;
        this.frameHeight = frameHeight;
        this.frameCount = frameCount;
        this.elapsedTime = 0;
        this.currentFrame = 0;
    }

    public void Update(GameTime gameTime)
    {
        elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

        if (elapsedTime >= frameTime) {
            elapsedTime = 0;
            currentFrame++;

            if (currentFrame >= frameCount) {
                currentFrame = isLooping ? 0 : frameCount - 1;
            }
        }
    }

    public void SetFrameTime(double newFrameTime)
    {
        frameTime = newFrameTime;
    }

    public void Reset()
    {
        currentFrame = 0;
        elapsedTime = 0;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effect)
    {
        Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
        spriteBatch.Draw(spriteSheet, position, sourceRect, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
    }

}
