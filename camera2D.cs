using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprite_animado;

public class camera2D
{
    public Matrix Transform { get; private set; }
    public Vector2 Position { get; private set; }
    public float zoom { get; set; } = 1f;
    public float LerpSpeed { get; set; } = 0.1f;

    private Viewport viewport;

    public camera2D (Viewport viewport) {
        this.viewport = viewport;
    }

    public void Follow(Vector2 targetPosition) {
        Position = Vector2.Lerp(Position, targetPosition, LerpSpeed);

        Transform = Matrix.CreateTranslation(new Vector3(-Position, 0 )) *
                    Matrix.CreateScale(zoom) *
                    Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
    }

}
