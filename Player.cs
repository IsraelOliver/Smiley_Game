using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace sprite_animado
{
    public class Player
    {
        private float walkSpeed = 1.5f;
        private float maxSpeed = 3.5f;
        private float acceleration = 0.2f;
        private float friction = 0.8f;
        private Gravity gravity;
        private bool isOnGround = false;
        private float velocityX;
        private float velocityY;

        public Vector2 Position;

        private Animation walkAnimation;
        private bool facingLeft = true;
        private TileMap tileMap;

        public Player(Texture2D walkTexture, int frameWhidth, int frameHeight, int frameCount, Vector2 startPosition, TileMap tileMap)
        {
            walkAnimation = new Animation(walkTexture, frameWhidth, frameHeight, frameCount);
            Position = startPosition;
            gravity = new Gravity(25f, 100f);
            this.tileMap = tileMap;
        }

        public void MovingPlayer() {
            KeyboardState state = Keyboard.GetState();
            float targetVelocity = 0f;

            // 🔹 Movimento lateral com aceleração
            /*
            if (state.IsKeyDown(Keys.A))
            {
                targetVelocity = -walkSpeed;
                isMoving = true;
                facingRight = false;
                if (state.IsKeyDown(Keys.LeftShift))
                {
                    targetVelocity = -maxSpeed;
                }
            }
            else if (state.IsKeyDown(Keys.D))
            {
                targetVelocity = walkSpeed;
                isMoving = true;
                facingRight = true;
                if (state.IsKeyDown(Keys.LeftShift))
                {
                    targetVelocity = maxSpeed;
                }
            }

            // 🔹 Ajustando velocidade gradualmente
            if (targetVelocity != 0)
            {
                if (velocityX < targetVelocity)
                    velocityX = Math.Min(velocityX + acceleration, targetVelocity);
                else if (velocityX > targetVelocity)
                    velocityX = Math.Max(velocityX - acceleration, targetVelocity);
            }
            else
            {
                // 🔹 Aplicando fricção quando não está pressionando teclas
                velocityX *= friction;
                if (Math.Abs(velocityX) < 0.1f) velocityX = 0;
            }

            */
            if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.A))
            {
                targetVelocity = state.IsKeyDown(Keys.D) ? walkSpeed : -walkSpeed;
                facingLeft = state.IsKeyDown(Keys.A);
            }

            float animationSpeed = state.IsKeyDown(Keys.LeftShift) ? 0.007f : 0.03f;
            walkAnimation.SetFrameTime(animationSpeed);

            if (state.IsKeyDown(Keys.LeftShift))
            {
                targetVelocity = state.IsKeyDown(Keys.D) ? maxSpeed : -maxSpeed;
            }

            //Pular
            if (state.IsKeyDown(Keys.Space) && isOnGround)
            {
                velocityY = -10f;
                isOnGround = false;
            }

            // Suaviza aceleração e frenagem
            if (targetVelocity != 0)
            {
                if (velocityX < targetVelocity)
                    velocityX = Math.Min(velocityX + acceleration, targetVelocity);
                else if (velocityX > targetVelocity)
                    velocityX = Math.Max(velocityX - acceleration, targetVelocity);
            } else {
                velocityX *= friction;
                if (Math.Abs(velocityX) < 0.01f) velocityX = 0;
            } 
        }

        private void ApplyPhysics(float deltaTime)
        {
            Vector2 velocity = gravity.AplicarGravidade(new Vector2(velocityX, velocityY), deltaTime);
            velocityX = velocity.X;
            velocityY = velocity.Y;
        }

        private void ApplyMovement() 
        {
            Position.X += velocityX;
            Position.Y += velocityY;

            if (tileMap.IsSolidTileAtPosition(Position.X, Position.Y + velocityY + tileMap.tileSize))
            {
                isOnGround = true;
                velocityY = 0;
                Position.Y = (float)Math.Round(Position.Y / tileMap.tileSize) * tileMap.tileSize;
            }
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            if (velocityX != 0) {
                walkAnimation.Update(gameTime);
            } else {
                walkAnimation.Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            MovingPlayer();
            ApplyPhysics(deltaTime);
            ApplyMovement();
            UpdateAnimation(gameTime);

            // 🔹 Verifica colisão lateral e salto automático em degraus
            if (tileMap.IsSolidTileAtPosition(Position.X + velocityX, Position.Y))
            {
                if (!tileMap.IsSolidTileAtPosition(Position.X + velocityX, Position.Y - tileMap.tileSize))
                {
                    Position.Y -= tileMap.tileSize;
                }
                else
                {
                    velocityX = 0;
                }
            }

            Console.WriteLine($"Posição X: {Position.X}, Velocidade X: {velocityX}");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = facingLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            walkAnimation.Draw(spriteBatch, Position, spriteEffect);
        }
    }
}