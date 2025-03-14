
//Classe para ter uma gravidade global

namespace sprite_animado
{
    public class Gravity
    {
        private float gravityForce;
        private float terminalVelocity;

        public Gravity(float gravityForce = 9.8f, float terminalVelocity = 50f)
        {
            this.gravityForce = gravityForce;
            this.terminalVelocity = terminalVelocity;
        }

        public void ApplyGravity(ref float velocityY)
        {
            // Aplica a gravidade, limitando a velocidade máxima
            velocityY += gravityForce * 0.1f; // Multiplicamos por um fator pequeno para ajustar a escala do jogo
            if (velocityY > terminalVelocity)
                velocityY = terminalVelocity;
        }
    }
}
