using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class TextureProcessor
{
    public static Rectangle GetBoundingBox(Texture2D texture)
    {
        Color[] pixels = new Color[texture.Width * texture.Height];
        texture.GetData(pixels);

        int minX = texture.Width, maxX = 0;
        int minY = texture.Height, maxY = 0;

        bool hasVisiblePixel = false;

        for (int y = 0; y < texture.Height; y++)
        {
            for (int x = 0; x < texture.Width; x++)
            {
                if (pixels[y * texture.Width + x].A > 0) // Verifica transparência
                {
                    hasVisiblePixel = true;
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            }
        }

        if (!hasVisiblePixel)
            return new Rectangle(0, 0, texture.Width, texture.Height); // Caso seja 100% transparente

        return new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
    }
}