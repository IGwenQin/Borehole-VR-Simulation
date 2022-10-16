
using System.Collections.Generic;
using UnityEngine;


[ CreateAssetMenu() ]
public class CylinderData : ScriptableObject {
    // scriptable scene object for hierarchy
    public ScriptableSceneObject scriptableSceneObject;

    // material
    public Material material;

    // mesh resolution
    public int circleResolution = 32;
    public int heightResolution = 128;

    // spatial resolution for mesh
    public float spatialResolution = 0.5f;

    public float radius;
    public float circumference;
    public float height;

    // if smooth texture
    public bool smoothTexture = true;

    // if smooth normal
    public bool smoothNormal = true;

    // if generete zenith
    public bool generateZenith = false;


    public void Generate() {
        List< Vector3 > vertices = new List< Vector3 >();
        List< Vector2 > uvs = new List< Vector2 >();
        List< int > triangles = new List< int >();

        GenerateMeshes( ref vertices, ref uvs, ref triangles );
        GenerateGameObject( vertices, uvs, triangles );
    }


    void GenerateMeshes( ref List< Vector3 > vertices, ref List< Vector2 > uvs, ref List< int > triangles ) {
        int fixedCircleResolution = circleResolution + ( smoothTexture ? 1 : 0 );
        int fixedHeightResolution = heightResolution + 1;
        int vertexOffset = ( generateZenith ? 1 : 0 );


        // circumference * ( height + 1 ) + 2
        int vertexSize = fixedCircleResolution * fixedHeightResolution + 2;

        // 3 * ( 2 * circumference * height + 2 * circumference )
        int triangleSize = 6 * circleResolution * fixedHeightResolution;

        vertices = new List< Vector3 >( vertexSize );
        uvs = new List< Vector2 >( vertexSize );
        triangles = new List< int >( triangleSize );


        // zenith
        if( generateZenith ) {
            vertices.Add( new Vector3( 0, height / 2.0f, 0 ) );
            uvs.Add( new Vector2( 0, 1 ) );
        }

        for( int i = 0; i < fixedHeightResolution; i++ ) {
            float v = 1 - ( float )i / heightResolution;
            float y = height * ( v - 0.5f );

            for( int j = 0; j < fixedCircleResolution; j++ ) {
                float angle = 2 * Mathf.PI * j / circleResolution;

                float u = 1 - ( float )j / circleResolution;
                float x = radius * Mathf.Cos( angle );
                float z = radius * Mathf.Sin( angle );

                vertices.Add( new Vector3( x, y, z ) );
                uvs.Add( new Vector2( u, v ) );

                if( j == circleResolution ) {
                    continue;
                }

                // calculate triangles
                int index = i * fixedCircleResolution + j + vertexOffset;

                int right = ( smoothTexture ? j + 1 : ( j + 1 ) % circleResolution );
                int rightIndex = i * fixedCircleResolution + right + vertexOffset;

                if( generateZenith || i != 0 ) {
                    int top = i - 1;
                    int topIndex = ( top < 0 ? 0 : top * fixedCircleResolution + j + vertexOffset );

                    triangles.Add( topIndex );
                    triangles.Add( index );
                    triangles.Add( rightIndex );
                }

                int bottom = i + 1;
                int bottomRightIndex = ( bottom >= fixedHeightResolution ? fixedCircleResolution * fixedHeightResolution + vertexOffset : bottom * fixedCircleResolution + right + vertexOffset );

                triangles.Add( rightIndex );
                triangles.Add( index );
                triangles.Add( bottomRightIndex );
            }
        }

        // nadir
        vertices.Add( new Vector3( 0, -height / 2.0f, 0 ) );
        uvs.Add( new Vector2( 0, 0 ) );
    }


    void GenerateGameObject( List< Vector3 > vertices, List< Vector2 > uvs, List< int > triangles ) {
        if( scriptableSceneObject.GameObject == null ) {
            scriptableSceneObject.GameObject = new GameObject( name );

            scriptableSceneObject.GameObject.AddComponent< MeshRenderer >();
            scriptableSceneObject.GameObject.AddComponent< MeshFilter >();
            // scriptableSceneObject.GameObject.AddComponent< MeshCollider >();
        }

        MeshRenderer meshRenderer = scriptableSceneObject.GameObject.GetComponent< MeshRenderer >();
        MeshFilter meshFilter = scriptableSceneObject.GameObject.GetComponent< MeshFilter >();

        meshRenderer.sharedMaterial = new Material( material );
        meshRenderer.sharedMaterial.SetTexture( "_MainTex", material.mainTexture );


        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        // smooth normals
        if( smoothNormal ) {
            List< Vector3 > normals = new List< Vector3 >( mesh.normals );

            int fixedCircleResolution = circleResolution + ( smoothTexture ? 1 : 0 );
            int fixedHeightResolution = heightResolution + 1;
            int vertexOffset = ( generateZenith ? 1 : 0 );

            for( int i = 0; i < fixedHeightResolution; i++ ) {
                int startEdgeIndex = i * fixedCircleResolution + vertexOffset;
                int endEdgeIndex = startEdgeIndex + circleResolution;

                Vector3 averageNormal = ( normals[ startEdgeIndex ] + normals[ endEdgeIndex ] ) / 2.0f;
                normals[ startEdgeIndex ] = averageNormal;
                normals[ endEdgeIndex ] = averageNormal;
            }

            mesh.normals = normals.ToArray();
        }


        meshFilter.sharedMesh = mesh;
        // meshCollider.sharedMesh = mesh;
    }
}
