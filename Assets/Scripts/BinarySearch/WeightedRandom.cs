using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedRandom
{
    public static GameObject GetRandomItem(WeightedItem[] possibleItems)
    {

        // Create an array to hold the cumumative densities - it is parallel to the drop table, so it is the same length!
        float[] CDA = new float[possibleItems.Length];

        // Start with a density of 0
        float cumulativeDensity = 0;

        // Iterate through the drop table
        for (int i = 0; i < possibleItems.Length; i++)
        {
            // Add the density of the next item to the total
            cumulativeDensity += possibleItems[i].weight;

            // Store that in the CDA
            CDA[i] = cumulativeDensity;
        }

        // Choose a random number less than the total density
        float randomValue = Random.Range(0, cumulativeDensity);

        // Find the value in the CDA 
        // Iterate through the CDA.
        for (int i = 0; i < CDA.Length; i++)
        {
            // If the random value is >= the value in the CDA, then we can move on to the next item in the CDA.
            // If the random value is < the CDA value, this is the index we want (because we checked the other lesser values already)
            if (randomValue < CDA[i])
            {
                // So we can return the item from the dropTable at the same index!
                return possibleItems[i].objectToDrop;
            }
        }
        // If we get here, something went wrong - Maybe our table is empty! - so return null
        return null;
    }

    [System.Serializable]
    public class WeightedItem
    {
        public GameObject objectToDrop;
        public float weight;
    }
}