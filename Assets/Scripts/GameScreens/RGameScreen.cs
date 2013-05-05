/* Rocks - Unity/Futile AI Turret
 * © 2013 Adam K Dean / Imdsm
 * http://www.adamkdean.co.uk 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RGameScreen : FContainer
{
    public RGameScreen()
    {
        Instance = this;

        bgSprite = new FSprite("bg.png");
        Turret = new RTurret();
        Rocks = new List<RRock>();
        Bullets = new List<RBullet>();

        AddChild(bgSprite);
        AddChild(Turret);
    }

    public void Update(float dt)
    {
        Turret.Update(dt);

        // we modify the list so we can't use a foreach
        for (int i = Bullets.Count - 1; i >= 0; i--) Bullets[i].Update(dt);
        for (int i = Rocks.Count - 1; i >= 0; i--) Rocks[i].Update(dt);        
        
        // check to see if we should add another rock
        rockFreqCurrent += dt;
        if (rockFreqCurrent >= rockFrequency)
        {
            RRock rock = new RRock();
            Rocks.Add(rock);
            AddChild(rock);

            //rockFrequency *= 0.99f;			
            rockFreqCurrent = 0f;
        }
    }

    public static RGameScreen Instance { get; private set; }
    public List<RRock> Rocks { get; set; }
    public List<RBullet> Bullets { get; set; }
    public RTurret Turret { get; set; }

    private FSprite bgSprite;
    private float rockFrequency = 0.5f;
    private float rockFreqCurrent = 0f;
}