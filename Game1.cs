using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using sprite_animado;

namespace sprite_animado;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private AnimatedSprite animatedSprite;

    Texture2D background;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D textureIdle = Content.Load<Texture2D> ("Smiley");
        Texture2D textureWalk = Content.Load<Texture2D> ("SmileyWalk");
        
        animatedSprite = new AnimatedSprite(textureWalk, textureIdle, 7, 2, new Vector2(100, 200));

        background = Content.Load<Texture2D> ("background");

    } //lol

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        animatedSprite.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
        animatedSprite.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
