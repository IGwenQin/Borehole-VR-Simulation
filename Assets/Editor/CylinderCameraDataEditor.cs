
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


[ CustomEditor( typeof( CylinderCameraData ), true ) ]
public class CylinderCameraDataEditor : Editor {
    void OnEnable() {
        CylinderCameraData data = target as CylinderCameraData;

        if( data.scriptableSceneObject?.GameObject == null ) {
            data.scriptableSceneObject.LoadIdentifierId();
            data.Init();
        }
    }


    public override void OnInspectorGUI() {
        // base.OnInspectorGUI();
        CylinderCameraData data = target as CylinderCameraData;

        serializedObject.Update();


        // display game object
        EditorGUI.BeginChangeCheck();
        data.scriptableSceneObject.GameObject = EditorGUILayout.ObjectField( "Game Object", data.scriptableSceneObject.GameObject, typeof( GameObject ), true ) as GameObject;

        if( EditorGUI.EndChangeCheck() ) {
            // camera objects
            data.Init();
        }

        GUI.enabled = false;
        EditorGUILayout.LongField( "Identifier ID", data.scriptableSceneObject.IdentifierId );
        GUI.enabled = true;


        // select material and texture
        data.cylinderData = EditorGUILayout.ObjectField( "Cylinder Data", data.cylinderData, typeof( CylinderData ), false ) as CylinderData;

        // offset for camera position
        data.positionOffset = EditorGUILayout.FloatField( "Position Offset [mm]", data.positionOffset );

        // capture resolution [s]
        data.samplingResolution = EditorGUILayout.FloatField( "Sampling Resolution [/s]", data.samplingResolution );

        // camera velocity [m/s]
        data.velocity = EditorGUILayout.FloatField( "Camera Velocity [mm/s]", data.velocity );

        // camera angular velocity [euler/s]
        data.rotationVelocity = EditorGUILayout.FloatField( "Camera Rotation Velocity [euler/s]", data.rotationVelocity );

        // render size
        data.renderSize = EditorGUILayout.Vector2IntField( "Render Size", data.renderSize );

        // render path
        data.renderPath = EditorGUILayout.TextField( "Render Path", data.renderPath );

        // update data
        if( GUILayout.Button( "Save" ) ) {
            EditorUtility.SetDirty( target );
            EditorUtility.SetDirty( serializedObject.targetObject );
        }

        if( data.cylinderData == null ) {
            GUI.enabled = false;
        }

        bool isGameObjectNull = data.scriptableSceneObject.GameObject == null;

        // generate cylinder
        if( GUILayout.Button( ( isGameObjectNull ? "Generate" : "Update" ) ) ) {
            data.Generate();

            // for write identifier id of ScriptableSceneObject 
            EditorSceneManager.SaveScene( EditorSceneManager.GetActiveScene() );
            EditorUtility.SetDirty( serializedObject.targetObject );
        }

        if( isGameObjectNull ) {
            GUI.enabled = false;
        }

        // render cylinder
        if( GUILayout.Button( "Render" ) ) {
            data.Render();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
