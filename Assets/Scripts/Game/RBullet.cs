/* Rocks - Unity/Futile AI Turret
 * © 2013 Adam K Dean / Imdsm
 * http://www.adamkdean.co.uk 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RBullet : FSprite
{
    public RBullet(Vector2 direction)
        : base("bullet.png")
    {
        this.direction = direction;
        x += direction.x * startOffset;
        y += direction.y * startOffset;
    }

    public void Update(float dt)
    {
        x += (direction.x * speed) * dt;
        y += (direction.y * speed) * dt;

        if (MathEx.IsOffScreen(x, y, width, height, 1.1f))
        {
            RGameScreen.Instance.Bullets.Remove(this);
            RGameScreen.Instance.RemoveChild(this);
        }
    }

    private Vector2 direction;
    private float startOffset = 30f; // at the end of the turret
    private float speed = 1000f;
}