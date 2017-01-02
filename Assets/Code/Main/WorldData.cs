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

    public void Generate (ZoneTypesList zoneTypes, int length = 10, int width = 10) { //Make this an IEnumerator later --- (To Add)
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

        //Grow each zone type until no more moves can be made.
        int zonesFinished = 0;
        Vector2[] adjacentArray = new Vector2[8] { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2 (-1, -1) };
        while (zonesFinished < length*width) {
            for (int yIndex = 0; yIndex < width; yIndex++) {
                for (int xIndex = 0; xIndex < length; xIndex++) {
                    //For every zone in our newWorldZones array.
                    if (newWorldZones[yIndex, xIndex].z_ZoneType.name != "Unspecified") { //If the zonetype has been set.
                        //Check all adjacent tiles and spread to them with priority zoneType.Priority if possible.
                        foreach (Vector2 adjacentTilePos in adjacentArray) {
                            try {
                                if (newWorldZones[(int)yIndex + (int)adjacentTilePos.x, (int)xIndex + (int)adjacentTilePos.y].setConversionPercentage(newWorldZones[yIndex, xIndex].z_ZoneType)) {
                                    zonesFinished += 1;
                                }
                            }
                            catch (System.IndexOutOfRangeException) {
                                continue;
                            }
                        }
                    }
                }
            }
        }

        //Apply a randomized cutting of zones at the edges of the world to create a continental look:
        Debug.Log("Unimplemented");

        //Assign MandatoryZones in their respective ZoneTypes, and if they do not exist, create them randomly.
        Debug.Log("Unimplemented");

        //Fill in the rest of the Zones with FillerZones or UniqueZones based on their ZoneType. Also assign their prefabIndex.
        Debug.Log("Unimplemented");

        //Finally, convert to a 1D list and update our own w_Zones:
        List<ZoneData> processedZoneList = new List<ZoneData>();
        foreach(ZoneData z in newWorldZones) {
            processedZoneList.Add(z); //Perhaps check for empty, deleted zones? --- (To Add/Review)
        }
        w_Zones = processedZoneList;
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
