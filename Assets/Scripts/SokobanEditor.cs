#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SokobanEditorSetup))]
public class SokobanEditor : Editor
{

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		SokobanEditorSetup t = (SokobanEditorSetup)target;
		GUILayout.Label("Управление:", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Создать / Обновить сетку")) t.Create();
		if (GUILayout.Button("Очистить карту")) t.ClearMap();
		GUILayout.EndHorizontal();
	}

	void OnSceneGUI()
	{
		SokobanEditorSetup t = (SokobanEditorSetup)target;

		HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive)); // отмена выбора объекта ЛКМ в окне редактора

		if (Event.current.button == 0 && Event.current.type == EventType.MouseDown || Event.current.button == 0 && Event.current.type == EventType.MouseDrag)
		{
			RaycastHit2D hit = Physics2D.Raycast(SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(new Vector2(Event.current.mousePosition.x,
				SceneView.currentDrawingSceneView.camera.pixelHeight - Event.current.mousePosition.y)), Vector2.zero);

			if (hit.collider != null)
			{
				if (!Event.current.shift)
				{
					if (hit.collider.name.CompareTo(t.prefabsNames[t.index]) != 0) t.SetPrefab(hit.transform.gameObject);
				}
				else
				{
					if (hit.collider.tag.CompareTo("EditorOnly") != 0) DestroyImmediate(hit.transform.gameObject);
				}
			}
		}

		Handles.BeginGUI();
		GUILayout.BeginArea(new Rect(t.position.x, t.position.y, t.width, t.height), EditorStyles.helpBox);

		if (GUILayout.Button("Загрузить / Обновить префабы")) t.LoadResources();

		GUILayout.TextArea("Справка: установить выбранный объект ЛКМ, убрать объект Shift+ЛКМ.");

		GUILayout.BeginHorizontal();
		GUILayout.TextField("Выбор префаба: ");
		t.index = EditorGUILayout.Popup(t.index, t.prefabsNames);
		GUILayout.EndHorizontal();

		GUILayout.EndArea();
		Handles.EndGUI();
	}
}
#endif