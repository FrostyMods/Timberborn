#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MissingReferenceFinder : MonoBehaviour
{
    private static List<GameObject> GetObjects(bool onlyInScene)
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            var includeAll = true;

            if (onlyInScene) {
                includeAll = !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave);
            }

            if (EditorUtility.IsPersistent(go.transform.root.gameObject) && includeAll)
                objectsInScene.Add(go);
        }

        return objectsInScene;
    }

    [MenuItem("Tools/Find Missing References in Scene")]
    public static void FindMissingReferences()
    {
        var objects = GetObjects(true);
    
        foreach (var go in objects)
        {
            var components = go.GetComponents(typeof(Component));
    
            foreach (var c in components)
            {
                SerializedObject so = new SerializedObject(c);
                var sp = so.GetIterator();
    
                while (sp.NextVisible(true))
                {
                    if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (sp.objectReferenceValue == null && sp.objectReferenceInstanceIDValue != 0)
                        {
                            ShowError(FullObjectPath(go), sp.name);
                        }
                    }
                }
            }
        }
    }

    private static void ShowError (string objectName, string propertyName)
    {
        Debug.LogError("Missing reference found in: " + objectName + ", Property : " + propertyName);
    }
    
    private static string FullObjectPath(GameObject go)
    {
        return go.transform.parent == null ? go.name : FullObjectPath(go.transform.parent.gameObject) + "/" + go.name;
    }
}
#endif