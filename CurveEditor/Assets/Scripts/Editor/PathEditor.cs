using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator creator;
    Path path;
    
    // когда объект доступен
    void OnEnable()
    {
        creator = (PathCreator) target;
        if (creator.path == null)
        {
            creator.CreatePath();
        }

        path = creator.path;
    }
    
    // когда мы нажали на объект
    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            // сохраняем объект в его текущем состоянии для CTRL + Z
            Undo.RecordObject(creator, "Add Segment");
            // добавляем новый точкуна сплайне
            path.AddSegment(mousePos);
        }
    }

    void Draw()
    {
        for (int i = 0; i < path.NumSegments; i++)
        {
            Vector2[] points = path.GetPointsInSegment(i);
            Handles.color = Color.black;
            // рисуем линии тангентов
            Handles.DrawLine(points[1], points[0]);
            Handles.DrawLine(points[2], points[3]);
            // рисуем кривые через метод из юнити
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2);
            
        }
        
        Handles.color = Color.red;
        for (int i = 0; i < path.NumPoints; i++)
        {
            // создаём красные точки и возможностью двигаться после клика на них, получаем изменённую позицию
            Vector2 newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, .1f, Vector3.zero, Handles.CylinderHandleCap);
            if (path[i] != newPos)
            {
                // сохраняем объект в его текущем состоянии для CTRL + Z
                Undo.RecordObject(creator, "Move Point");
                // сохраняем изменённый path
                path.MovePoint(i, newPos);
            }
        }
    }
}










