//Classe para ter uma gravidade global

using System;

namespace sprite_animado
{
    public class Gravity
    {
    public float GravityForce { get; set; }
    public float TerminalVelocity { get; set; }

        public Gravity(float gravityForce = 9.8f, float terminalVelocity = 50f)
        {
            GravityForce = gravityForce;
            TerminalVelocity = terminalVelocity;
        }

        public float GetGravityEffect(float currentVelocity) {
            float newVelocity = currentVelocity + GravityForce * 0.1f;
            return MathF.Min(newVelocity, TerminalVelocity);
        }
    }
}
