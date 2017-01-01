using System.Collections.Generic;

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

        ZoneData[,] newWorldZones = new ZoneData[length, width]; //Create a 2D Array of blank zones.
        for (int yIndex = 0; yIndex < width; yIndex++) {
            for (int xIndex = 0; xIndex < length; xIndex++) {
                newWorldZones[yIndex, xIndex] = new ZoneData();
                newWorldZones[yIndex, xIndex].z_ZoneType = new ZoneType(); //This is automatically set to unspecified.
            }
        }

    }
}
