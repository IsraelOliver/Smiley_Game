//Classe para ter uma gravidade global
using Microsoft.Xna.Framework;

namespace sprite_animado;
public class Gravity
{
    public float Intensidade { get; set; }
    public float VelocidadeMaxima { get; set; }

    public Gravity(float intensidade, float velocidadeMaxima)
    {
        Intensidade = intensidade;
        VelocidadeMaxima = velocidadeMaxima;
    }

    public Vector2 AplicarGravidade(Vector2 velocidade, float deltaTime)
    {
        velocidade.Y += Intensidade * deltaTime;

        if (velocidade.Y > VelocidadeMaxima)
        {
            velocidade.Y = VelocidadeMaxima;
        }

        return velocidade;
    }
}