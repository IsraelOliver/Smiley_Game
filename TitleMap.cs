using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprite_animado {
    public class TileMap {
        private Texture2D tileTexture;
        private int[,] tiles;
        public int tileSize = 11;
        private int mapWidht = 1000;
        private int mapHeight = 500;
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

            // Configuração do ruído para o terreno
            noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.08f); // Frequência ajustada para suavizar o terreno

            // Criando o gerador de ruído para cavernas
            FastNoiseLite caveNoise = new FastNoiseLite();
            caveNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            caveNoise.SetFrequency(0.15f); // Frequência ajustada para cavernas
            caveNoise.SetFractalType(FastNoiseLite.FractalType.Ridged);
            caveNoise.SetFractalOctaves(4); // Mais octaves deixam as cavernas mais detalhadas

            for (int x = 0; x < mapWidht; x++) {
                float noiseValue = noise.GetNoise(x * 0.1f, 0) * 5;
                int currentGroundLevel = groundLevel + (int)noiseValue;

                for (int y = 0; y < mapHeight; y++) {
                    tiles[y, x] = (y >= currentGroundLevel) ? 1 : 0; // Define o solo

                    // Aplicando ruído para cavernas SOMENTE se o tile for solo
                    if (y > currentGroundLevel - 8) { // Permite cavernas abaixo da superfície
                        float caveValue = caveNoise.GetNoise(x * 0.2f, y * 0.2f); // Pequena variação
                        if (caveValue > 0.3f) { // Reduzindo a restrição
                            tiles[y, x] = 0; // Remove o solo para criar a caverna
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
                            new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), // 🔹 Ajuste correto do tamanho
                            new Rectangle(0, 0, tileTexture.Width, tileTexture.Height), // 🔹 Usa a textura completa corretamente
                            Color.White
                        );
                    }
                }
            }
        }
    }
}