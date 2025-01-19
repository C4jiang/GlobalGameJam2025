using System;
using System.Collections.Generic;
using UnityEngine;

public static class PolygonSimilarity
{
    public static float CalculateSimilarity(List<Vector2> polygon1, List<Vector2> polygon2)
    {
        // Step 1: Normalize polygons
        // List<Vector2> normalizedPolygon1 = NormalizePolygon(polygon1);
        // List<Vector2> normalizedPolygon2 = NormalizePolygon(polygon2);

        // Step 2: Calculate deviation arrays
        float[] deviations1 = CalculateDeviations(polygon1);
        float[] deviations2 = CalculateDeviations(polygon2);

        // Step 3: Calculate standard deviation of differences
        float similarity = CalculateStandardDeviation(deviations1, deviations2);

        return similarity;
    }

    private static List<Vector2> NormalizePolygon(List<Vector2> polygon)
    {
        // Calculate centroid
        Vector2 centroid = Vector2.zero;
        foreach (var point in polygon)
        {
            centroid += point;
        }
        centroid /= polygon.Count;

        // Calculate max distance to centroid (radius of the circumcircle)
        float maxDistance = 0f;
        foreach (var point in polygon)
        {
            float distance = Vector2.Distance(point, centroid);
            if (distance > maxDistance)
            {
                maxDistance = distance;
            }
        }

        // Normalize points
        List<Vector2> normalizedPolygon = new List<Vector2>();
        foreach (var point in polygon)
        {
            normalizedPolygon.Add((point - centroid) / maxDistance);
        }

        return normalizedPolygon;
    }

    private static float[] CalculateDeviations(List<Vector2> polygon)
    {
        float[] deviations = new float[360];
        for (int i = 0; i < 360; i++)
        {
            float angle = i * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            float maxDistance = 0f;

            foreach (var point in polygon)
            {
                float distance = Vector2.Dot(point, direction);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
            }

            deviations[i] = maxDistance;
        }

        return deviations;
    }

    private static float CalculateStandardDeviation(float[] deviations1, float[] deviations2)
    {
        float sum = 0f;
        for (int i = 0; i < deviations1.Length; i++)
        {
            float difference = deviations1[i] - deviations2[i];
            sum += difference * difference;
        }

        return Mathf.Sqrt(sum / deviations1.Length);
    }
}