using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprite_animado {
    public class TileMap {
        private Texture2D grassTileTexture;
        private Texture2D dirtTileTexture;
        private Texture2D sandTileTexture;
        private int[,] tiles;
        public int tileSize = 11;
        private int mapWidht = 500;
        private int mapHeight = 250;
        private FastNoiseLite noise;

        public TileMap(Texture2D grassTexture, Texture2D dirtTexture, Texture2D sandTexture) {
            grassTileTexture = grassTexture;
            dirtTileTexture = dirtTexture;
            sandTileTexture = sandTexture;
            noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.1f);
            GenerateTerrain();
        }

        public void GenerateTerrain()
        {
            tiles = new int[mapHeight, mapWidht];
            int groundLevel = mapHeight / 2;

            noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.10f);

            FastNoiseLite caveNoise = new FastNoiseLite();
            caveNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            caveNoise.SetFrequency(0.2f);
            caveNoise.SetFractalType(FastNoiseLite.FractalType.Ridged);
            caveNoise.SetFractalOctaves(5);

            // Passo 1: Criar terreno normal (terra e grama)
            for (int x = 0; x < mapWidht; x++)
            {
                float noiseValue = noise.GetNoise(x * 0.1f, 0) * 5;
                int currentGroundLevel = groundLevel + (int)noiseValue;
                bool grassPlaced = false;

                for (int y = 0; y < mapHeight; y++)
                {
                    if (y >= currentGroundLevel)
                    {
                        if (!grassPlaced)
                        {
                            tiles[y, x] = 2; // Grass
                            grassPlaced = true;
                        }
                        else
                        {
                            tiles[y, x] = 1; // Dirt
                        }

                        // Criar cavernas
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

            // Passo 2: Substituir parte do terreno por areia (Criar bioma de deserto)
            int centerX = mapWidht / 2;  // Centro do mapa na horizontal
            int centerY = mapHeight / 2; // Meio do mapa na vertical
            int a = mapWidht / 6;        // Largura do deserto
            int b = mapHeight / 5;       // Altura do deserto

            for (int x = 0; x < mapWidht; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    // Verifica se o ponto está dentro da elipse
                    double ellipse = Math.Pow(x - centerX, 2) / Math.Pow(a, 2) +
                                    Math.Pow(y - centerY, 2) / Math.Pow(b, 2);

                    if (ellipse <= 1 && tiles[y, x] == 1)  // Apenas substitui terra por areia
                    {
                        tiles[y, x] = 3; // Define o tile como "sand_tile"
                    }
                }
            }
            for (int x = 0; x < mapWidht; x++)
            {
                for (int y = 1; y < mapHeight; y++) // Começa de y=1 para evitar acessar índice -1
                {
                    if (tiles[y, x] == 3 && tiles[y - 1, x] == 2) // Se for areia e houver grama acima
                    {
                        tiles[y - 1, x] = 3; // Substitui a grama por areia
                    }
                }
            }

        }
        public bool IsSolidTileAtPosition(float x, float y) {
            int tileX = (int)(x / tileSize);
            int tileY = (int)(y / tileSize);

            if (tileX < 0 || tileX >= tiles.GetLength(1) || tileY < 0 || tileY >= tiles.GetLength(0))
                return false; // Fora dos limites do mapa

            return tiles[tileY, tileX] == 1 || tiles[tileY, tileX] == 2 || tiles[tileY, tileX] == 3; // Retorna true se for um tile sólido
        }

        public Vector2 GetSpawnPosition()
        {
            for (int y = 0; y < tiles.GetLength(0); y++) {
                for (int x = 0; x < tiles.GetLength(1); x++) {
                    if (tiles[y, x] == 2) { // Primeiro tile sólido encontrado
                        return new Vector2(x * tileSize, y * tileSize); // Coloca o personagem acima do tile
                    }
                }
            }
            return new Vector2(0, 0);
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
                    else if (tiles[y, x] == 3) // Areia
                        currentTexture = sandTileTexture;

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