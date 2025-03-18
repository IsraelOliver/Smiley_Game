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
        private int currentFrame;
        private int totalFrames;
        public Vector2 position;
        private float velocityY; //definindo velocidade em Y
        private float velocityX; //definindo velocidade em X
        private bool isMoving; //grante a movimentação
        private bool facingRight = true; //verifica para qual lado ele está virado
        private Gravity gravity;

        float chao = 350f;
       
        public AnimatedSprite(Texture2D textureWalk, Texture2D textureIdle, int rows, int columns, Vector2 startPosition){
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

        }   

        public void Update() {

            KeyboardState state = Keyboard.GetState();
            isMoving = false;

            if(state.IsKeyDown(Keys.W) && position.Y >= chao ) {
                velocityY -= 10f;
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

            gravity.ApplyGravity(ref velocityY);
            position.Y += velocityY;
            position.X += velocityX;

            Console.WriteLine($"velocityY: {velocityY}");

            if(position.Y >= chao) {
                position.Y = chao;
                velocityY = 0;
            }
            Console.WriteLine($"Posição X: {position.X}, Velocidade X: {velocityX}");
        }
        
        public void Draw(SpriteBatch spriteBatch) {

            Texture2D currentTexture = isMoving ? textureWalk : textureIdle;

            int width = currentTexture.Width / Columns;
            int height = currentTexture.Height / Rows;
            int row = currentFrame / Columns;
            int column = currentFrame % Columns;
        
            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

            SpriteEffects spriteEffect = facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        
            spriteBatch.Draw(currentTexture, destinationRectangle, sourceRectangle, Color.White, 0f, Vector2.Zero, spriteEffect, 0f);
        }   
    }
}