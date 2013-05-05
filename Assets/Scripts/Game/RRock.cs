/* Rocks - Unity/Futile AI Turret
 * © 2013 Adam K Dean / Imdsm
 * http://www.adamkdean.co.uk 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RRock : FSprite
{
    public RRock(int rockSize = 6, float startX = 0, float startY = 0)
        : base(string.Format("rock{0}.png", rockSize))
    {
        Direction = new Vector2(RXRandom.Range(-0.8f, 0.8f), RXRandom.Range(-0.8f, 0.8f));
        Speed = RXRandom.Range(10f, 100f);
        Spin = RXRandom.Range(10f, 100f);
        Size = rockSize;

        if (startX != 0 && startY != 0)
        {
            x = startX;
            y = startY;
        }
        else
        {
            // randomly select a side for the rock to come from
            if (RXRandom.Range(0, 1) == 0)
            {
                if (Direction.y > 0f)
                {
                    // going up
                    x = RXRandom.Range(-Futile.screen.halfWidth, Futile.screen.halfWidth);
                    y = -Futile.screen.halfHeight - height;
                }
                else
                {
                    // going down
                    x = RXRandom.Range(-Futile.screen.halfWidth, Futile.screen.halfWidth);
                    y = Futile.screen.halfHeight + height;
                }
            }
            else
            {
                if (Direction.x > 0f)
                {
                    // going right
                    x = -Futile.screen.halfWidth - width;
                    y = RXRandom.Range(-Futile.screen.halfHeight, Futile.screen.halfHeight);
                }
                else
                {
                    // going left
                    x = Futile.screen.halfWidth + width;
                    y = RXRandom.Range(-Futile.screen.halfHeight, Futile.screen.halfHeight);
                }
            }
        }
    }

    public void Update(float dt)
    {
        x += (Direction.x * Speed) * dt;
        y += (Direction.y * Speed) * dt;
        rotation += Spin * dt;

        // see if it's off the screen
        if (MathEx.IsOffScreen(x, y, width, height))
        {
            RGameScreen.Instance.Rocks.Remove(this);
            RGameScreen.Instance.RemoveChild(this);
        }

        // check if it's been hit by a bullet        
        for (int i = 0; i < RGameScreen.Instance.Bullets.Count; i++)
        {
            var bullet = RGameScreen.Instance.Bullets[i];
            var bulletPos = this.GlobalToLocal(new Vector2(bullet.x, bullet.y));

            if (this.textureRect.Contains(bulletPos))
            {
                // remove this rock & the bullet that hit it
                RGameScreen.Instance.Rocks.Remove(this);
                RGameScreen.Instance.RemoveChild(this);
                RGameScreen.Instance.Bullets.Remove(bullet);
                RGameScreen.Instance.RemoveChild(bullet);

                SoundEx.PlaySound(SoundEffect.Explosion);

                // create fragments
                if (Size > 1)
                {
                    while (Size > 0)
                    {
                        int s = (Size > 1) ? (int)RXRandom.Range(1, Size) : 1;
                        Size -= s;
                        RRock rock = new RRock(s, this.x, this.y);
                        RGameScreen.Instance.Rocks.Add(rock);
                        RGameScreen.Instance.AddChild(rock);
                    }
                }

                break;
            }
        }
    }

    public Vector2 Direction { get; set; }
    public float Speed { get; set; }
    public float Spin { get; set; }
    public int Size { get; set; }
}