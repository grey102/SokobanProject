using System.Collections;
using UnityEngine;

public class SokobanEditorSetup : MonoBehaviour
{

	[Header("Настройки меню")]
	public Vector2 position = new Vector2(10, 10);
	public float width = 400;
	public float height = 60;
	[Header("Папка с префабами в Resources")]
	public string prefabsPath = "Prefabs";
	[Header("Настройки сетки")]
	public Sprite cellSprite;
	public Color cellColor = Color.white;
	public int cellWidth = 10;
	public int cellHeight = 10;
	public float cellSize = 1;
	[Header("Родительский объект карты")]
	public Transform map;

	// данные переменные используются менюшкой
	[HideInInspector] public Transform[] prefabs;
	[HideInInspector] public string[] prefabsNames;
	[HideInInspector] public int index;
	[HideInInspector] public bool showButton, project2D;
	[HideInInspector] public string tagField;
	[HideInInspector] public LayerMask layerMask;

	void Awake()
	{
		gameObject.SetActive(false);
	}

	public void ClearMap()
	{
		GetClear(map);
	}

	void GetClear(Transform tr)
	{
		GameObject[] obj = new GameObject[tr.childCount];
		for (int i = 0; i < tr.childCount; i++)
		{
			obj[i] = tr.GetChild(i).gameObject;
		}
		foreach (GameObject t in obj)
		{
			DestroyImmediate(t);
		}
	}

	public void Create()
	{
		GetClear(transform);
		Transform sample = new GameObject().transform;
		sample.gameObject.tag = "EditorOnly";
		sample.gameObject.AddComponent<SpriteRenderer>().sprite = cellSprite;
		sample.gameObject.GetComponent<SpriteRenderer>().color = cellColor;
		sample.gameObject.AddComponent<BoxCollider2D>();
		float posX = -cellSize * cellWidth / 2 - cellSize / 2;
		float posY = cellSize * cellHeight / 2 + cellSize / 2;
		float Xreset = posX;
		int z = 0;
		for (int y = 0; y < cellHeight; y++)
		{
			posY -= cellSize;
			for (int x = 0; x < cellWidth; x++)
			{
				posX += cellSize;
				Transform tr = Instantiate(sample) as Transform;
				tr.SetParent(transform);
				tr.localScale = Vector3.one;
				tr.position = new Vector2(posX, posY);
				tr.name = "Cell_" + z;
				z++;
			}
			posX = Xreset;
		}
		DestroyImmediate(sample.gameObject);
	}

	public void SetPrefab(GameObject obj)
	{
		if (prefabs.Length == 0) return;
		Transform clone = Instantiate(prefabs[index], obj.transform.position - Vector3.forward * 0.05f, Quaternion.identity) as Transform;
		clone.gameObject.name = prefabs[index].name;
		clone.parent = map;
	}

	public void LoadResources()
	{
		prefabs = Resources.LoadAll<Transform>(prefabsPath);

		prefabsNames = new string[prefabs.Length];
		for (int i = 0; i < prefabs.Length; i++)
		{
			prefabsNames[i] = prefabs[i].name;
		}

		index = 0;
	}
}