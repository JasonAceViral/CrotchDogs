using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class AlignAndDistribute {

    [MenuItem( "Ace Viral/Align and Distribute/Align Left" )]
    static void AlignLeft () {
        if(Selection.gameObjects.Any())
        {
            UpdateX(Selection.gameObjects.Min(go => go.transform.position.x));
        }
    }

    [MenuItem( "Ace Viral/Align and Distribute/Align Center" )]
    static void AlignCenter () {
        if(Selection.gameObjects.Any())
        {
            UpdateX(Selection.gameObjects.Average(go => go.transform.position.x));
        }
    }

    [MenuItem( "Ace Viral/Align and Distribute/Align Right" )]
    static void AlignRight () {
        if(Selection.gameObjects.Any())
        {
            UpdateX(Selection.gameObjects.Max(go => go.transform.position.x));
        }
    }

    [MenuItem("Ace Viral/Align and Distribute/Distribute Vertical")]
    static void DistributeVertical () {
        if(Selection.gameObjects.Length >= 3)
        {
            var top = Selection.gameObjects.Max(go => go.transform.position.y);
            var bottom = Selection.gameObjects.Min(go => go.transform.position.y);
            List<GameObject> blah = Selection.gameObjects.ToList();
            blah.Sort((g1,g2) => (g1.transform.position.y >= g2.transform.position.y) ? 1 : -1);
            var i = 0f;
            var t = blah.Count*1f-1;
            foreach (var go in blah)
            {
                var pos = go.transform.position;
                pos.y = top*i/t + bottom*(1-i/t);
                go.transform.position = pos;
                i++;
            }
        }
    }

    [MenuItem("Ace Viral/Align and Distribute/Distribute Horizontal")]
    static void DistributeHorizontal () {
        if(Selection.gameObjects.Length >= 3)
        {
            var top = Selection.gameObjects.Max(go => go.transform.position.x);
            var bottom = Selection.gameObjects.Min(go => go.transform.position.x);
            List<GameObject> blah = Selection.gameObjects.ToList();
            blah.Sort((g1,g2) => (g1.transform.position.x >= g2.transform.position.x) ? 1 : -1);
            var i = 0f;
            var t = blah.Count*1f-1;
            foreach (var go in blah)
            {
                var pos = go.transform.position;
                pos.x = top*i/t + bottom*(1-i/t);
                go.transform.position = pos;
                i++;
            }
        }
    }


    static void UpdateX (float val)
    {
        foreach(var t in Selection.gameObjects)
        {
            var pos = t.transform.position;
            pos.x = val;
            t.transform.position = pos;
        }
    }

}