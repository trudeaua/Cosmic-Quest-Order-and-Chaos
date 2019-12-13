using System.Collections;
using UnityEngine;

public class VfxHelper : MonoBehaviour
{
    public static IEnumerator CreateVFX(GameObject vfxPrefab, Vector3 position, Quaternion rotation, float delay = 0f)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        GameObject vfx = Instantiate(vfxPrefab, position, rotation);

        var ps = GetFirstPS(vfx);

        Destroy(vfx, ps.main.duration + ps.main.startLifetime.constantMax + 1);
    }
    
    public static IEnumerator CreateVFX(GameObject vfxPrefab, Vector3 position, Quaternion rotation, Color vfxColour, float delay = 0f)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        GameObject vfx = Instantiate(vfxPrefab, position, rotation);

        ParticleSystem[] particleSystems = vfx.GetComponentsInChildren<ParticleSystem>();
        // Convert the hue of the particle effect to the hue of the character colour
        foreach (ParticleSystem p in particleSystems)
        {
            ParticleSystem.MainModule main = p.main;
            float h1, s1, v1, h2, s2, v2, h3, s3, v3;
            Color min = vfxColour;
            Color max = vfxColour;
            float minA = main.startColor.colorMin.a;
            float maxA = main.startColor.colorMax.a;
            if (vfxColour != Color.white && vfxColour != Color.black && vfxColour != Color.gray)
            {
                // need to maintain alpha values, they are forgotten in the RGB to HSV to RGB conversion
                // need to get saturation and brightness value
                Color.RGBToHSV(main.startColor.colorMin, out h1, out s1, out v1);
                Color.RGBToHSV(main.startColor.colorMax, out h2, out s2, out v2);
                Color.RGBToHSV(vfxColour, out h3, out s3, out v3);
                min = Color.HSVToRGB(h3, s1, v1);
                max = Color.HSVToRGB(h3, s2, v2);
            }
            main.startColor = new ParticleSystem.MinMaxGradient(new Color(min.r, min.g, min.b, minA), new Color(max.r, max.g, max.b, maxA));
        }

        ParticleSystem ps = GetFirstPS(vfx);

        Destroy(vfx, ps.main.duration + ps.main.startLifetime.constantMax);
    }

    private static ParticleSystem GetFirstPS(GameObject vfx)
    {
        var ps = vfx.GetComponentInChildren<ParticleSystem>();
        if (ps is null && vfx.transform.childCount > 0)
        {
            foreach (Transform t in vfx.transform)
            {
                ps = t.GetComponent<ParticleSystem>();
                if (ps)
                    return ps;
            }
        }
        
        return ps;
    }
}