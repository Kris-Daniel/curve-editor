using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
    [SerializeField, HideInInspector] private List<Vector2> points;

    public Path(Vector2 center)
    {
        points = new List<Vector2>()
        {
            center + Vector2.left,
            center + (Vector2.left + Vector2.up) * .5f,
            center + (Vector2.right + Vector2.down) * .5f,
            center + Vector2.right
        };
    }

    public Vector2 this[int i] => points[i];
    public int NumPoints =>  points.Count;
    public int NumSegments => (points.Count - 4) / 3 + 1;

    public void AddSegment(Vector2 anchorPos)
    {
        // сначала правый tangent для последнего anchor, потом левый для нового и новый anchor 
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + anchorPos) * .5f);
        points.Add(anchorPos);
    }

    public Vector2[] GetPointsInSegment(int i)
    {
        // получаем точки сегмента благодоря ряду * 3
        return new Vector2[]
        {
            points[i * 3],
            points[i * 3 + 1],
            points[i * 3 + 2],
            points[i * 3 + 3]
        };
    }

    public void MovePoint(int i, Vector2 pos)
    {
        Vector2 deltaMove = pos - points[i];
        points[i] = pos;
        
        // если anchor
        if (i % 3 == 0)
        {
            // двигаем правый tangent на то же расстояние
            if (i + 1 < points.Count)
            {
                points[i + 1] += deltaMove;
            }
            // двигаем левый tangent на то же расстояние
            if (i - 1 >= 0)
            {
                points[i - 1] += deltaMove;
            }
        }
        else
        {
            bool nextPointIsAnchor = (i + 1) % 3 == 0;
            // если следующий это anchor то соседний тангент через 2, если нет то назад на 2
            int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
            // получает anchor
            int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

            if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count)
            {
                // получаем расстояние соседнего тангента до anchor'a
                float dst = (points[anchorIndex] - points[correspondingControlIndex]).magnitude;
                // получаем вектор направления от текущего тангента до anchor'a
                Vector2 dir = (points[anchorIndex] - pos).normalized;
                // применяем на соседний тангент
                points[correspondingControlIndex] = points[anchorIndex] + dir * dst;
            }
        }
    }
}



































