using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprite_animado {
    public class TileMap {
        private Texture2D tileTexture;
        private int[,] tiles; // Matriz para armazenar os blocos do mapa
        public int tileSize = 32; // Tamanho de cada tile (em pixels)

        public TileMap(Texture2D texture) {
            tileTexture = texture;
            LoadTiles();
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
                return new Vector2(x * tileSize, (y * tileSize) - tileSize); // Coloca o personagem acima do tile
            }
        }
    }
    return new Vector2(0, 0); // Caso não encontre um tile sólido
}

        private void LoadTiles() {
            // Exemplo: um mapa simples (0 = vazio, 1 = bloco de chão)
            tiles = new int[,] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            };
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