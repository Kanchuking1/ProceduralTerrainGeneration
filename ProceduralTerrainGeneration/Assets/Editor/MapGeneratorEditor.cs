using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator MG = (MapGenerator)target;
        if (DrawDefaultInspector())
        {
            if (MG.autoUpdate)
            {
                MG.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            MG.GenerateMap();
        }
    }
}
