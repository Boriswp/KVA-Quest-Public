using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static IEnumerator ChangeColorIfHit(Color changeColor, float time, SpriteRenderer sprite)
    {
        sprite.color = changeColor;
        yield return new WaitForSeconds(time);
        sprite.color = Color.white;
    }

    public static void SetActiveAllObjects(GameObject[] gameObjects, bool isActive)
    {
        foreach (var item in gameObjects)
        {
            item.SetActive(isActive);
        }
    }

    public static bool IsKarma(float karmaChance)
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= karmaChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static string FormatTime(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60); // Получаем количество минут
        int seconds = Mathf.FloorToInt(totalSeconds % 60); // Оставшиеся секунды

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public static bool IsPointInRectangle(Vector2[] rectanglePoints, Vector2 point)
    {
        // Найти минимальные и максимальные координаты по осям X и Y
        float xMin = Mathf.Min(rectanglePoints[0].x, rectanglePoints[1].x, rectanglePoints[2].x, rectanglePoints[3].x);
        float xMax = Mathf.Max(rectanglePoints[0].x, rectanglePoints[1].x, rectanglePoints[2].x, rectanglePoints[3].x);
        float yMin = Mathf.Min(rectanglePoints[0].y, rectanglePoints[1].y, rectanglePoints[2].y, rectanglePoints[3].y);
        float yMax = Mathf.Max(rectanglePoints[0].y, rectanglePoints[1].y, rectanglePoints[2].y, rectanglePoints[3].y);

        // Проверить, находится ли точка внутри прямоугольника
        return (xMin <= point.x && point.x <= xMax && yMin <= point.y && point.y <= yMax);
    }

}
