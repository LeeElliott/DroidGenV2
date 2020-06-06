using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour
{
	// Use this for initialization
	void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        //Vertices

        Vector3[] vertices = new Vector3[250 * 250];
        Vector2[] uvs = new Vector2[250 * 250];
        int[] triangles = new int[249 * 249 * 6];
        int n = 0;

        for (int j = 0; j < 250; j++)
        {
            for (int i = 0; i < 250; i++)
            {
                vertices[j * 250 + i] = new Vector3(-125 + i, 0, -125 + j);
            }
        }

        for (int j = 0; j < 249; j++)
        {
            for (int i = 0; i < 249; i++)
            {
                triangles[n] = j * 250 + (i + 1);
                triangles[n + 1] = j * 250 + i;
                triangles[n + 2] = (j + 1) * 250 + i;

                triangles[n + 3] = (j + 1) * 250 + i;
                triangles[n + 4] = (j + 1) * 250 + (i + 1);
                triangles[n + 5] = j * 250 + (i + 1);
                n += 6;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        mf.mesh = mesh;

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
