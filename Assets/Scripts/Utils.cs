using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Recursively finds a child GameObject by name from a given root GameObject.
    /// </summary>
    /// <param name="root">The root GameObject to search from.</param>
    /// <param name="name">The name of the GameObject to find.</param>
    /// <returns>The found GameObject or null if not found.</returns>
    public static GameObject FindNodeFrom(GameObject root, string name)
    {
        // Check if the current root is the one we're looking for
        if (root.name == name)
        {
            return root;
        }

        // Recursively search through all children
        foreach (Transform child in root.transform)
        {
            GameObject result = FindNodeFrom(child.gameObject, name);
            if (result != null)
            {
                return result;
            }
        }

        // If the node is not found, return null
        return null;
    }

    /// <summary>
    /// Generates a random point inside a unit sphere.
    /// </summary>
    /// <returns>A random point inside a unit sphere as a Vector3.</returns>
    public static Vector3 RandomPointInUnitSphere()
    {
        float theta = Random.Range(0, Mathf.PI * 2);
        float phi = Random.Range(0, Mathf.PI);
        float r = Mathf.Pow(Random.Range(0f, 1f), 1.0f / 3.0f);  // Cube root for uniform distribution

        float x = r * Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = r * Mathf.Sin(phi) * Mathf.Sin(theta);
        float z = r * Mathf.Cos(phi);

        return new Vector3(x, y, z);
    }
}
