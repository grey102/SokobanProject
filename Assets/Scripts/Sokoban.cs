using System.Collections;
using UnityEngine;

public class Sokoban : MonoBehaviour
{

	[SerializeField] private int targetCount = 1; // сколько точек для ящиков на карте
	[SerializeField] private float zOffset = -1; // смещение, чтобы персонаж и ящики, были выше остальных объектов
	[SerializeField] private float step = 1; // шаг движения, должен быть такой же, как и размер клетки
	[SerializeField] private float speed = 0.05f; // скорость движения
	private Transform player, moved;
	public static int target { get; set; }
	private Vector3 direction, targetPos;
	private bool isMove;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		target = 0;
		if (player != null)
		{
			player.position = new Vector3(player.position.x, player.position.y, zOffset);
			targetPos = player.position;
		}
	}

	void Complete()
	{
		Debug.Log("! You Win !");
	}

	void Update()
	{
		if (player != null)
		{
			Control();
		}
	}

	Transform GetTransform(Vector2 point)
	{
		RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);

		if (hit.collider != null)
		{
			return hit.transform;
		}

		return null;
	}

	bool CanMove()
	{
		moved = null;

		// поиск объектов в направлении движения, на два хода вперед
		Transform t1 = GetTransform(new Vector2(player.position.x + step * direction.x, player.position.y + step * direction.y));
		Transform t2 = GetTransform(new Vector2(player.position.x + step * direction.x * 2f, player.position.y + step * direction.y * 2f));

		// ищем ящик
		if (t1 != null && t1.name.CompareTo("Box") == 0) moved = t1;

		// условия при которых, движение невозможно
		if (t1 == null || t1.name.CompareTo("Wall") == 0 || moved != null && t2 != null && t2.name.CompareTo("Box") == 0 ||
			moved != null && t2 != null && t2.name.CompareTo("Wall") == 0) return false;

		isMove = true;
		if (moved != null) moved.position = new Vector3(moved.position.x, moved.position.y, zOffset);

		return true;
	}

	void Move()
	{
		if (!CanMove()) return;

		// определяем точку назначения
		targetPos = new Vector3(player.position.x + step * direction.x, player.position.y + step * direction.y, player.position.z);
	}

	Vector3 GetRoundPos(Vector3 val) // обрезаем хвосты
	{
		val.x = Mathf.Round(val.x * 100f) / 100f;
		val.y = Mathf.Round(val.y * 100f) / 100f;
		val.z = Mathf.Round(val.z * 100f) / 100f;
		return val;
	}

	void Control()
	{
		if (isMove)
		{
			// движение персонажа и ящика, если он есть
			player.position = Vector3.MoveTowards(player.position, targetPos, speed);
			if (moved != null) moved.position = Vector3.MoveTowards(moved.position, targetPos + direction * step, speed);

			if (targetPos == GetRoundPos(player.position))
			{
				isMove = false;

				// выравнивание позиции
				player.position = GetRoundPos(player.position);
				if (moved != null) moved.position = GetRoundPos(moved.position);

				if (target == targetCount)
				{
					Complete();
					enabled = false;
				}
			}

			return;
		}

		if (Input.GetKey(KeyCode.D))
		{
			direction = Vector3.right;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			direction = Vector3.left;
		}
		else if (Input.GetKey(KeyCode.W))
		{
			direction = Vector3.up;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			direction = Vector3.down;
		}
		else
		{
			direction = Vector3.zero;
		}

		if (direction.magnitude != 0)
		{
			Move();
		}
	}
}