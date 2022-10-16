
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


[ CustomEditor( typeof( CylinderData ), true ) ]
public class CylinderDataEditor : Editor {
    void OnEnable() {
        CylinderData data = target as CylinderData;

        if( data.scriptableSceneObject?.GameObject == null ) {
            data.scriptableSceneObject?.LoadIdentifierId();
        }
    }


    public override void OnInspectorGUI() {
        // base.OnInspectorGUI();
        CylinderData data = target as CylinderData;

        serializedObject.Update();


        // display game object
        data.scriptableSceneObject.GameObject = EditorGUILayout.ObjectField( "Game Object", data.scriptableSceneObject.GameObject, typeof( GameObject ), true ) as GameObject;

        GUI.enabled = false;
        EditorGUILayout.LongField( "Identifier ID", data.scriptableSceneObject.IdentifierId );
        GUI.enabled = true;


        // select material and texture
        data.material = EditorGUILayout.ObjectField( "Material", data.material, typeof( Material ), false ) as Material;

        // mesh resolution
        data.circleResolution = EditorGUILayout.IntField( "Circle Resolution", data.circleResolution );
        data.heightResolution = EditorGUILayout.IntField( "Height Resolution", data.heightResolution );

        // spatial resolution
        data.spatialResolution = EditorGUILayout.FloatField( "Spatial Resolution [m/vert]", data.spatialResolution );

        // if smooth texture
        data.smoothTexture = EditorGUILayout.Toggle( "Smooth Texture", data.smoothTexture );

        // if smooth normal
        data.smoothNormal = EditorGUILayout.Toggle( "Smooth Normal", data.smoothNormal );

        // if generate zenith
        data.generateZenith = EditorGUILayout.Toggle( "Generate Zenith", data.generateZenith );

        // display parameters
        data.radius = data.circumference / ( 2.0f * Mathf.PI );
        data.circumference = data.circleResolution * data.spatialResolution;
        data.height = data.heightResolution * data.spatialResolution;

        EditorGUILayout.LabelField( "Radius        = " + data.radius + " [m]" );
        EditorGUILayout.LabelField( "Circumference = " + data.circumference + " [m]" );
        EditorGUILayout.LabelField( "Height        = " + data.height        + " [m]" );


        // save data        
        if( GUILayout.Button( "Save" ) ) {
            EditorUtility.SetDirty( target );
            EditorUtility.SetDirty( serializedObject.targetObject );
        }

        if( data.material == null ) {
            GUI.enabled = false;
        }

        // generate cylinder
        if( GUILayout.Button( ( data.scriptableSceneObject.GameObject == null ? "Generate" : "Update" ) ) ) {
            data.Generate();

            // for write identifier id of ScriptableSceneObject 
            EditorSceneManager.SaveScene( EditorSceneManager.GetActiveScene() );
            EditorUtility.SetDirty( serializedObject.targetObject );
        }

        serializedObject.ApplyModifiedProperties();
    }
}
