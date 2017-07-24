using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{

    public static UnityEngine.Object LoadRandomPrefab(this string resource)
    {
        int variation = 1;
        bool resourceFound = true;
        while (resourceFound == true)
        {
            if (Resources.Load(resource + "_" + variation.ToString()) != null)
            {
                variation++;
            }
            else
            {
                resourceFound = false;
            }
        }
        return Resources.Load(resource + "_" + UnityEngine.Random.Range(1, variation).ToString());
    }

    public static bool CanSeeGameObject(this GameObject go, float fov, GameObject target, Vector3 offset)
    {
        Vector3 toTarget = target.transform.position - go.transform.position;
        float goToTargetAngle = Vector3.Angle(go.transform.forward, toTarget);
        RaycastHit hit;
        if (Physics.Linecast(go.transform.position, target.transform.position + offset, out hit))
        {
            if (goToTargetAngle <= fov && hit.collider.gameObject == target)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public static bool CanSeeGameObject(this GameObject go, float fov, GameObject target)
    {
        return CanSeeGameObject(go, fov, target, Vector3.zero);
    }

    //Breadth-first search
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = child.FindDeepChild(aName);
            if (result != null)
                return result;
        }
        return null;
    }

    //tag search
    public static Transform FindDeepChildWithTag(this Transform aParent, string aTag)
    {
        Transform result = null;
        if (aParent.tag == aTag)
            return aParent;
        foreach (Transform child in aParent)
        {
            result = child.FindDeepChildWithTag(aTag);
            if (result != null)
                return result;
        }
        return result;
    }

    //nearest gameobject
    public static GameObject FindNearestGameObject(this GameObject[] objs, Vector3 pos)
    {
        GameObject nearestObj = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = pos;
        foreach (GameObject o in objs)
        {
            if (o != null)
            {
                float dist = Vector3.Distance(o.transform.position, currentPos);
                if (dist < minDist)
                {
                    nearestObj = o;
                    minDist = dist;
                }
            }
        }
        return nearestObj;
    }

    public static Transform LookAt2D(this Transform transform, Vector2 position)
    {
        float rotZ = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
        return transform;
    }

    public static T CopyComponent<T>(this T original, GameObject destination, bool simpleOnly = false) where T : Component
    {
        Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            if (simpleOnly)
            {
                if (!(field.FieldType.IsPrimitive || field.FieldType.Equals(typeof(string)) || field.FieldType.Equals(typeof(Color))))
                {
                    continue;
                }
            }
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }

    public static void SetLayerRecursively(this GameObject obj, int newLayer )
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

}
