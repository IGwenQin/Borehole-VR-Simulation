
using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;


[ Serializable ]
public class ScriptableSceneObject {
    GameObject m_gameObject;
    public GameObject GameObject {
        get {
            return m_gameObject;
        }

        set {
            m_gameObject = value;
            SaveIdentiferId();
        }
    }

    [ SerializeField ]
    long m_identifierId = -1;
    public long IdentifierId {
        get {
            return m_identifierId;
        }
    }

    [ SerializeField ]
    bool m_identified = false;
    public bool Identified {
        get {
            return m_identified;
        }
    }


    public void LoadIdentifierId() {
        if( !m_identified ) {
            return;
        }

        foreach( GameObject child in Resources.FindObjectsOfTypeAll< GameObject >() ) {
            if( AssetDatabase.GetAssetOrScenePath( child ).Contains( ".unity" ) && m_identifierId == GetIdentiferId( child ) ) {
                m_gameObject = child;
                break;
            }
        };
    }


    long GetIdentiferId( GameObject obj ) {
        PropertyInfo inspectorModeInfo = typeof( SerializedObject ).GetProperty( "inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance );

        SerializedObject serializedObject = new SerializedObject( obj );
        inspectorModeInfo.SetValue( serializedObject, InspectorMode.Debug, null );

        // note the misspelling of "Identfier"
        SerializedProperty localIdProp = serializedObject.FindProperty( "m_LocalIdentfierInFile" );

        return localIdProp.longValue;
    }


    void SaveIdentiferId() {
        if( m_gameObject == null ) {
            m_identifierId = -1;
            m_identified = false;
            return;
        }

        m_identifierId = GetIdentiferId( m_gameObject );
        m_identified = true;
        return;
    }
}
