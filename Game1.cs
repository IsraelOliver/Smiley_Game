using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace sprite_animado;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Player player;
    private Texture2D spriteSheet;
    private camera2D camera;
    private TileMap tileMap;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        camera = new camera2D(GraphicsDevice.Viewport);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        Texture2D grassTileTexture = Content.Load<Texture2D>("grass_tile");
        Texture2D dirtTileTexture = Content.Load<Texture2D>("dirt_tile");
        Texture2D sandTileTexture = Content.Load<Texture2D>("sand_tile");
        tileMap = new TileMap(grassTileTexture, dirtTileTexture, sandTileTexture);

        spriteSheet = Content.Load<Texture2D>("smileyAnimation");
        Vector2 startPosition = tileMap.GetSpawnPosition();
        player = new Player(spriteSheet, 17, 23, 16, startPosition, tileMap);


    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardState state = Keyboard.GetState();

        //atualiza o animatedSprite
        player.Update(gameTime);

        //faz a camera seguir o animatedSprite
        camera.Follow(player.Position);

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
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(transformMatrix: camera.Transform);

        tileMap.Draw(_spriteBatch);
        player.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
