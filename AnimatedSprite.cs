using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace sprite_animado{
    
    public class AnimatedSprite {
        public Texture2D textureIdle {get; set;}
        public Texture2D textureWalk { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public Vector2 position;

        private bool isOnGround = false;
        private int currentFrame;
        private int totalFrames;
        private TileMap tileMap;
        private float velocityY; //definindo velocidade em Y
        private float velocityX; //definindo velocidade em X
        private bool isMoving; //grante a movimentação
        private bool facingRight = true; //verifica para qual lado ele está virado
        private Gravity gravity;
       
        public AnimatedSprite(Texture2D textureWalk, Texture2D textureIdle, int rows, int columns, Vector2 startPosition, TileMap tileMap){
            this.textureWalk = textureWalk;
            this.textureIdle = textureIdle;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            position = startPosition; // posição inicial
            velocityY = 0f;
            velocityX = 0f;
            isMoving = false;
            gravity = new Gravity();
            this.tileMap = tileMap;

        }   

        public void Update() {

            KeyboardState state = Keyboard.GetState();
            isMoving = false;

            if (state.IsKeyDown(Keys.W) && isOnGround) {
                velocityY = -15f;
                isOnGround = false; // 🔹 Evita que o personagem pule novamente no ar
            }

            if(state.IsKeyDown(Keys.A)) {
                velocityX = -1.5f;
                isMoving = true;
                facingRight = true;
                if (state.IsKeyDown(Keys.LeftShift)) {
                    velocityX = -3.5f;
                }
            } else if(state.IsKeyDown(Keys.D)) {
                velocityX = 1.5f;
                isMoving = true;
                facingRight = false;
                if (state.IsKeyDown(Keys.LeftShift)) {
                    velocityX = 3.5f;
                }
            } else {
                velocityX = 0;
            }

            if(isMoving) {
                currentFrame++;
                if (currentFrame == totalFrames)
                    currentFrame = 0;
            } else {
                currentFrame = 0;
            }
            

            if (tileMap.IsSolidTileAtPosition(position.X, position.Y + velocityY + tileMap.tileSize)) {
                isOnGround = true;
                velocityY = 0;
                
                // Ajuste para alinhar ao tile || Round arredonda para cima, e Floor arredonda para baixo.
                position.Y = (float)Math.Round(position.Y / tileMap.tileSize) * tileMap.tileSize;
            } else {
                isOnGround = false;
                gravity.ApplyGravity(ref velocityY);
            }

            if (tileMap.IsSolidTileAtPosition(position.X + velocityX, position.Y)) {
                velocityX = 0; // Se houver um tile sólido à esquerda/direita, ele para
            }

            // 🔹 Impedir que o personagem entre nos tiles por baixo (batendo a cabeça)
            if (velocityY < 0 && tileMap.IsSolidTileAtPosition(position.X, position.Y + velocityY)) {
                velocityY = 0; // Se houver um tile acima, cancela a subida
            }

//            gravity.ApplyGravity(ref velocityY);
            position.Y += velocityY;
            position.X += velocityX;

            Console.WriteLine($"velocityY: {velocityY}");

            Console.WriteLine($"Posição X: {position.X}, Velocidade X: {velocityX}");
        }
        
        public void Draw(SpriteBatch spriteBatch) {

            Texture2D currentTexture = isMoving ? textureWalk : textureIdle;

            int width = currentTexture.Width / Columns;
            int height = currentTexture.Height / Rows;
            int row = currentFrame / Columns;
            int column = currentFrame % Columns;
        
            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);

            SpriteEffects spriteEffect = facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Vector2 origin =  new Vector2(width / 2, height / 2);
            Vector2 adjustedPosition = new Vector2(position.X, position.Y);

            spriteBatch.Draw(currentTexture, adjustedPosition, sourceRectangle, Color.White, 0f, origin, 1f, spriteEffect, 0f);
        }   
    }
}