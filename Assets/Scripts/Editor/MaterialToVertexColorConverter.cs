/*using UnityEditor;
using UnityEngine;

public class MaterialToVertexColorConverter : AssetPostprocessor
{
    private void OnPostprocessModel(GameObject g)
    {
        var meshFilter = g.GetComponentInChildren<MeshFilter>();
        var meshRenderer = g.GetComponentInChildren<MeshRenderer>();
        var colors = new Color32[meshFilter.sharedMesh.vertices.Length];
        var offset = 0;
        for(var i = 0; i < meshFilter.sharedMesh.subMeshCount; i++)
        {
            var material = meshRenderer.sharedMaterials[i];
            var color = (Color32) material.color.linear;
            var subMesh = meshFilter.sharedMesh.GetSubMesh(i);
            for(var j = 0; j < subMesh.vertexCount; j++)
                colors[offset + j] = color;
            offset += subMesh.vertexCount;
        }
        meshFilter.sharedMesh.colors32 = colors;
        meshFilter.sharedMesh.SetTriangles(meshFilter.sharedMesh.triangles, 0);
        meshFilter.sharedMesh.subMeshCount = 1;
        EditorUtility.SetDirty(g);
    }

    public override int GetPostprocessOrder()
    {
        return 1111111111;
    }
}
*/