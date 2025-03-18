using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace sprite_animado;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private AnimatedSprite animatedSprite;
    private camera2D camera;
    private RenderTarget2D _renderTarget;
    private Vector2 _scale;
    private int baseWidth = 1920, baseHeight = 1080;

    Texture2D background;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _renderTarget = new RenderTarget2D(GraphicsDevice, baseWidth, baseHeight);

        float scaleX = (float)_graphics.PreferredBackBufferWidth / baseWidth;
        float scaleY = (float)_graphics.PreferredBackBufferHeight / baseHeight;

        camera = new camera2D(GraphicsDevice.Viewport);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D textureIdle = Content.Load<Texture2D> ("Smiley");
        Texture2D textureWalk = Content.Load<Texture2D> ("SmileyWalk");
        
        animatedSprite = new AnimatedSprite(textureWalk, textureIdle, 7, 2, new Vector2(100, 200));

        background = Content.Load<Texture2D> ("background");

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardState state = Keyboard.GetState();

        //atualiza o animatedSprite
        animatedSprite.Update();

        //faz a camera seguir o animatedSprite
        camera.Follow(animatedSprite.position);

        //controla a camera
        if(state.IsKeyDown(Keys.OemPlus)) {
            camera.zoom += 0.01f;
        }

        if(state.IsKeyDown(Keys.OemMinus)) {
            camera.zoom -= 0.01f;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(transformMatrix: camera.Transform);

        _spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
        animatedSprite.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
