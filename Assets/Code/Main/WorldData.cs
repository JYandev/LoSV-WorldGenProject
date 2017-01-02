using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData {
    /* WorldData is a container that represents a world.
     * World state, name, zone information, etc. are all stored in this class.
     * In the most basic implementation, this class is to be serialized to JSON
     * - for storage in GameData - Our save data file.
     */

    public List<ZoneData> w_Zones; //World Zone List

    public void Generate (ZoneTypesList zoneTypes, int length = 10, int width = 10) {
        /* Generates a new world and fills the w_Zones property.
         */

        // Create a 2D Array of blank zones:
        ZoneData[,] newWorldZones = new ZoneData[length, width];
        for (int yIndex = 0; yIndex < width; yIndex++) {
            for (int xIndex = 0; xIndex < length; xIndex++) {
                newWorldZones[yIndex, xIndex] = new ZoneData();
                newWorldZones[yIndex, xIndex].z_ZoneType = new ZoneType(); //This is automatically set to unspecified.
            }
        }
        // ---
        // Pick random starting points for each zone type.
        foreach (ZoneType currentType in zoneTypes.zoneTypes) {
            Vector2 chosenPoint = pickRandomUnchosenPoint(currentType.name, newWorldZones);
            newWorldZones[(int)chosenPoint.x, (int)chosenPoint.y].z_ZoneType = currentType;
        }
    }

    #region --- [World-Gen Helpers] ---
    private Vector2 pickRandomUnchosenPoint (string zoneType, ZoneData[,] zones) {
        var chosen = false;
        Vector2 randomVector = Vector2.zero;
        while (chosen == false) {
            randomVector = new Vector2((int)Random.Range(0, zones.GetLength(0)), (int)Random.Range(0, zones.GetLength(1)));
            if (zones[(int)randomVector.x, (int)randomVector.y].z_ZoneType.name != zoneType) {
                chosen = true;
            }
        }
        return randomVector;
    }
    #endregion
}
