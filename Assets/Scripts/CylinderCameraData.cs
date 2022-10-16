
using System.IO;
using System.Collections.Generic;
using UnityEngine;


[ CreateAssetMenu() ]
public class CylinderCameraData : ScriptableObject {
    // scriptable scene object for hierarchy
    public ScriptableSceneObject scriptableSceneObject;
    public List< GameObject > children;
    public int digit;

    // target cylinder data
    public CylinderData cylinderData;

    // offset for camera position
    public float positionOffset = -3.0f;

    // capture resolution [s]
    public float samplingResolution = 1.0f;

    // camera velocity [mm/s]
    public float velocity = 0.5f;

    // camera angular velocity [euler/s]
    public float rotationVelocity = 30.0f;

    // render size
    public Vector2Int renderSize = new Vector2Int( 1024, 1024 );

    // render path
    public string renderPath;


    protected enum EncodeType {
        PNG,
        JPG,
        EXR,
        TGA,
    }


    private void OnEnable() {
        renderPath = Path.GetFullPath( Application.dataPath + "/../output/" );
    }


    public void Init() {
        if( scriptableSceneObject.GameObject == null ) {
            return;
        }

        Transform rootChildren = scriptableSceneObject.GameObject.GetComponentInChildren< Transform >();
        int childCount = rootChildren.childCount;

        children = new List< GameObject >( childCount );

        foreach( Transform child in rootChildren ) {
            children.Add( child.gameObject );
        }

        digit = ( int )Mathf.Log10( childCount - 1 ) + 1;
    }


    public void Generate() {
        Transform cylinderTransform = cylinderData.scriptableSceneObject.GameObject.transform;
        Vector3 cylinderObjPosition = cylinderTransform.position;
        Vector3 cylinderObjRotation = cylinderTransform.rotation.eulerAngles;
        float height = cylinderData.height;


        // generate root object
        if( scriptableSceneObject.GameObject == null ) {
            scriptableSceneObject.GameObject = new GameObject( name );
        }


        float fixedHeight = height + 2 * positionOffset;
        float fixedTime = fixedHeight / velocity;
        int newCameraSize = ( int )( fixedTime * samplingResolution );


        // check deleting game object
        for( int i = children.Count - 1; i >= 0; i-- ) {
            if( children[i] == null ) {
                children.RemoveAt( i );
            }
        }

        int currentCameraSize = children.Count;
        int newDigit = ( int )Mathf.Log10( newCameraSize - 1 ) + 1;


        // add or remove game object
        if( currentCameraSize < newCameraSize ) {
            // rename current game objects
            if( digit != newDigit ) {
                digit = newDigit;

                for( int i = 0; i < currentCameraSize; i++ ) {
                    children[i].name = "CylinderCamera" + i.ToString().PadLeft( digit, '0' );
                }
            }

            children.Capacity = newCameraSize;

            // generate new game objects
            for( int i = currentCameraSize; i < newCameraSize; i++ ) {
                children.Add( new GameObject( "CylinderCamera" + i.ToString().PadLeft( digit, '0' ), typeof( Camera ) ) );
                children[i].transform.parent = scriptableSceneObject.GameObject.transform;
            }

        } else {
            // destroy exceed objects
            for( int i = currentCameraSize - 1; i >= newCameraSize; i-- ) {
                DestroyImmediate( children[i] );
                children.RemoveAt( i );
            }

            children.Capacity = newCameraSize;

            // rename new game objects
            if( digit != newDigit ) {
                digit = newDigit;

                for( int i = 0; i < newCameraSize; i++ ) {
                    children[i].name = "CylinderCamera" + i.ToString().PadLeft( digit, '0' );
                }
            }
        }


        float initialHeight = height / 2.0f + positionOffset;
        float cameraHeightResolution = velocity / samplingResolution;
        float cameraRotationResolution = rotationVelocity / samplingResolution;


        for( int i = 0; i < newCameraSize; i++ ) {
            Transform cameraTransform = children[i].transform;

            cameraTransform.position = cylinderObjPosition + new Vector3( 0, initialHeight - i * cameraHeightResolution, 0 );
            cameraTransform.rotation = Quaternion.Euler( cylinderObjRotation + new Vector3( 0, 45, 0 ) );
        }
    }


    public void Render() {
        Directory.CreateDirectory( renderPath );

        foreach( GameObject child in children ) {
            Camera camera = child.GetComponent< Camera >();

            RenderTexture renderTexture = new RenderTexture( renderSize.x, renderSize.y, 0 );
            camera.targetTexture = renderTexture;
            camera.Render();

            RenderTexture activeRenderTexture = RenderTexture.active;
            RenderTexture.active = renderTexture;
            SaveTextureAsImage( renderTexture, renderPath + child.name + ".jpg", TextureFormat.RGB24, EncodeType.PNG );
            RenderTexture.active = activeRenderTexture;

            camera.targetTexture = null;
        }
    }



    protected void SaveTextureAsImage( Texture2D texture, string filename, EncodeType type ) {
        switch( type ) {
            case EncodeType.PNG: {
                byte[] bytes = texture.EncodeToPNG();
                DestroyImmediate( texture );
                File.WriteAllBytes( filename, bytes );
                break;
            }

            case EncodeType.JPG: {
                byte[] bytes = texture.EncodeToJPG();
                DestroyImmediate( texture );
                File.WriteAllBytes( filename, bytes );
                break;
            }

            case EncodeType.EXR: {
                byte[] bytes = texture.EncodeToEXR();
                DestroyImmediate( texture );
                File.WriteAllBytes( filename, bytes );
                break;
            }

            case EncodeType.TGA: {
                byte[] bytes = texture.EncodeToTGA();
                DestroyImmediate( texture );
                File.WriteAllBytes( filename, bytes );
                break;
            }
        }
    }


    protected void SaveTextureAsImage( RenderTexture renderTexture, string filename, TextureFormat format, EncodeType type ) {
        Texture2D texture = new Texture2D( renderTexture.width, renderTexture.height, format, false, true );
        RenderTexture.active = renderTexture;
        texture.ReadPixels( new Rect( 0, 0, renderTexture.width, renderTexture.height ), 0, 0 );
        texture.Apply();
        SaveTextureAsImage( texture, filename, type );
    }

}
