using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants {
    /* GameConstants holds manually configurable stats for the game.
     * Most importantly to this project is the ability to modify how many ZoneTypes there are.
     */

    public static Dictionary<string, float> ZoneType = new Dictionary<string, float> {
        {"Grasslands", 1.0f},
        {"Desert", 1.0f},
        {"Snowy Hills", 1.0f}
    }; //Modify this to add more ZoneTypes. Set the float value to the zone's growth priority.
}
