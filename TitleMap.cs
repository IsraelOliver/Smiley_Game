using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprite_animado {
    public class TileMap {
        private Texture2D grassTileTexture;
        private Texture2D dirtTileTexture;
        private int[,] tiles;
        public int tileSize = 11;
        private int mapWidht = 1000;
        private int mapHeight = 500;
        private FastNoiseLite noise;

        public TileMap(Texture2D grassTexture, Texture2D dirtTexture) {
            grassTileTexture = grassTexture;
            dirtTileTexture = dirtTexture;
            noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.3f);
            GenerateTerrain();
        }

        public void GenerateTerrain()
        {
            tiles = new int[mapHeight, mapWidht];
            int groundLevel = mapHeight / 2;

            noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.08f);

            FastNoiseLite caveNoise = new FastNoiseLite();
            caveNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            caveNoise.SetFrequency(0.05f);
            caveNoise.SetFractalType(FastNoiseLite.FractalType.Ridged);
            caveNoise.SetFractalOctaves(5);

            for (int x = 0; x < mapWidht; x++)
            {
                float noiseValue = noise.GetNoise(x * 0.1f, 0) * 5;
                int currentGroundLevel = groundLevel + (int)noiseValue;
                bool grassPlaced = false; // Para garantir que apenas o primeiro tile seja grama

                for (int y = 0; y < mapHeight; y++)
                {
                    if (y >= currentGroundLevel)
                    {
                        if (!grassPlaced) 
                        {
                            tiles[y, x] = 2; // 2 representa "grass_tile"
                            grassPlaced = true;
                        }
                        else
                        {
                            tiles[y, x] = 1; // 1 representa "dirt_tile"
                        }

                        // Aplicando ruído para cavernas
                        if (y > currentGroundLevel - 8)
                        {
                            float caveValue = caveNoise.GetNoise(x * 0.2f, y * 0.2f);
                            if (caveValue > 0.3f)
                            {
                                tiles[y, x] = 0; // Remove o solo para criar cavernas
                            }
                        }
                    }
                }
            }
        }

        public bool IsSolidTileAtPosition(float x, float y) {
            int tileX = (int)(x / tileSize);
            int tileY = (int)(y / tileSize);

            if (tileX < 0 || tileX >= tiles.GetLength(1) || tileY < 0 || tileY >= tiles.GetLength(0))
                return false; // Fora dos limites do mapa

            return tiles[tileY, tileX] == 1 || tiles[tileY, tileX] == 2; // Retorna true se for um tile sólido
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

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    Texture2D currentTexture = null;

                    if (tiles[y, x] == 1) // Terra
                        currentTexture = dirtTileTexture;
                    else if (tiles[y, x] == 2) // Grama
                        currentTexture = grassTileTexture;

                    if (currentTexture != null)
                    {
                        spriteBatch.Draw(
                            currentTexture,
                            new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize),
                            Color.White
                        );
                    }
                }
            }
        }
    }
}