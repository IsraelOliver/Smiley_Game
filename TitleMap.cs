using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprite_animado {
    public class TileMap {
        private Texture2D tileTexture;
        private int[,] tiles;
        public int tileSize = 32;
        private int mapWidht = 100;
        private int mapHeight = 30;
        private FastNoiseLite noise;

        public TileMap(Texture2D texture) {
            tileTexture = texture;
            noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.3f);
            GenerateTerrain();
        }

        public void GenerateTerrain() {
            tiles = new int[mapHeight, mapWidht];
            int groundLevel = mapHeight / 2;

            for (int x = 0; x < mapWidht; x++) {
                float noiseValue = noise.GetNoise(x * 0.1f, 0) * 5;
                int currentGroundLevel = groundLevel + (int)noiseValue;

                for (int y = 0; y < mapHeight; y++) {
                    tiles[y, x] = (y >= currentGroundLevel) ? 1 : 0;
                }
            }
        }

        public bool IsSolidTileAtPosition(float x, float y) {
            int tileX = (int)(x / tileSize);
            int tileY = (int)(y / tileSize);

            if (tileX < 0 || tileX >= tiles.GetLength(1) || tileY < 0 || tileY >= tiles.GetLength(0))
                return false; // Fora dos limites do mapa

            return tiles[tileY, tileX] == 1; // Retorna true se for um tile sólido
        }

public Vector2 GetSpawnPosition()
{
    for (int y = 0; y < tiles.GetLength(0); y++) {
        for (int x = 0; x < tiles.GetLength(1); x++) {
            if (tiles[y, x] == 1) { // Primeiro tile sólido encontrado
                return new Vector2(x * tileSize, y * tileSize); // Coloca o personagem acima do tile
            }
        }
    }
    return new Vector2(0, 0); // Caso não encontre um tile sólido
}

        public void Draw(SpriteBatch spriteBatch) {
            for (int y = 0; y < tiles.GetLength(0); y++) {
                for (int x = 0; x < tiles.GetLength(1); x++) {
                    if (tiles[y, x] == 1) { // Se for um tile válido
                        spriteBatch.Draw(
                            tileTexture, 
                            new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), // 🔹 Redimensiona para 32x32
                            Color.White
                        );
                    }
                }
            }
        }
    }
}