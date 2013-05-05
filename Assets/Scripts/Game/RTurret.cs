/* Rocks - Unity/Futile AI Turret
 * © 2013 Adam K Dean / Imdsm
 * http://www.adamkdean.co.uk 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RTurret : FSprite
{
	public RTurret()
		: base("turret.png")
	{		
	}
	
	public void Update(float dt)
	{
		bulletFreqCurrent += dt;

        // if we have no target, or the last ones been removed, find a new one
		if (currentTarget == null || !RGameScreen.Instance.Rocks.Contains(currentTarget)) 		
			currentTarget = FindTarget();
		
		if (currentTarget != null)
		{
            // reset target/find new target
            if (Input.GetKeyUp(KeyCode.R))
            {
                currentTarget.color = Color.white;
                currentTarget = FindTarget();
            }

            // track the target
            float dx = x - currentTarget.x;
            float dy = y - currentTarget.y;
			float targetAngle = MathEx.RadiansToDegrees(Mathf.Atan2(dx, dy)) + 180;
            float currentAngle = MathEx.ClampAngle(rotation);
            float diffCW = MathEx.ClampAngle(targetAngle - currentAngle);
            float diffCCW = MathEx.ClampAngle(360 - diffCW);

            // turn either CW/CCW depending on efficiency
            float turnAmount = 0f;
            if (diffCW <= diffCCW) turnAmount = (diffCW < fireProximity) ? diffCW : turnSpeed;
            else turnAmount = (diffCCW < fireProximity) ? diffCCW * -1 : turnSpeed * -1;
            rotation += turnAmount * dt;

            // fire at the target if it's close
            if (Mathf.Abs(diffCW) < fireProximity || Mathf.Abs(diffCCW) < fireProximity) Fire(dt);

            // paint the target for debug
			currentTarget.color = Color.red;
		}		
	}
	
	public RRock FindTarget()
	{
        if (RGameScreen.Instance.Rocks.Count > 0)
        {
            // get the biggest target that is the closest
            var target = RGameScreen.Instance.Rocks
                .OrderByDescending(rock => rock.Size)                
                .ThenBy(rock => Mathf.Abs(rock.x) + Mathf.Abs(rock.y))                
                .First();
            return target;
        }
        else return null;
	}
	
	public void Fire(float dt)
	{
		if (bulletFreqCurrent >= bulletFrequency)
		{
			Vector2 direction = MathEx.DegreesToXY(this.rotation);		
			RBullet bullet = new RBullet(direction);		
			RGameScreen.Instance.Bullets.Add(bullet);
			RGameScreen.Instance.AddChild(bullet);

            SoundEx.PlaySound(SoundEffect.Turret, 0.4f);            
			
			bulletFreqCurrent = 0f;
		}		
	}

    private float bulletFreqCurrent = 0f;
    private float bulletFrequency = 0.4f; // shots per second	
    private float fireProximity = 2f;     // how close before firing?
	private float turnSpeed = 200f;       // tracking speed    
	
	private RRock currentTarget;
}