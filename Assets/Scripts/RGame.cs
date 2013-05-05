/* Rocks - Unity/Futile AI Turret
 * © 2013 Adam K Dean / Imdsm
 * http://www.adamkdean.co.uk 
 */

using UnityEngine;
using System.Collections;

public class RGame
{
    public RGame()
    {
        Instance = this;

        gameScreen = new RGameScreen();
        Futile.stage.AddChild(gameScreen);
    }

    public void Update(float dt)
    {
        gameScreen.Update(dt);
    }

    public static RGame Instance { get; private set; }
    private RGameScreen gameScreen;
}