using System.Collections.Generic;

[System.Serializable]
public class ZoneData {
    /* Data storage container for a Zone.
	 * A Zone is simply one piece - or tile - in the grid based world; WorldData will hold a List of all Zones.
     */

    public ZoneType z_ZoneType; //Zone's Type represented by a string (We don't need the full class)
    public int z_ZonePrefabIndex; //The index of the prefab in a ZoneList representing the ZoneType from z_ZoneType.

    #region --- [Private Functionality] ---
    //Stuff here is only needed for world-gen and is not saved in any save-data file.
    private Dictionary<ZoneType, float> conversionPercentages; //During world-gen, this is the conversion progress to the zone specified.

    public bool setConversionPercentage (ZoneType zoneType) {
        /* Sets the conversion percentage of the specified zone type.
         * If progress is >= 1, then the zone is converted here. Also returns true, if so. False otherwise.
         */

        if (conversionPercentages == null) { // If the dictionary doesn't exist (this is the first time the function has been run), we create one:
            conversionPercentages = new Dictionary<ZoneType, float>();
        }

        if (conversionPercentages.ContainsKey(zoneType)) {
            conversionPercentages[zoneType] += zoneType.priority;
        }
        else {
            conversionPercentages.Add(zoneType, zoneType.priority);
        }

        if (conversionPercentages[zoneType] >= 1) {
            z_ZoneType = zoneType;
            return true;
        }
        return false;
    }
    #endregion
}
