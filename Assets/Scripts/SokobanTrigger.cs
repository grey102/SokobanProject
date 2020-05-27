using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanTrigger : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.name.CompareTo("Box") == 0) Sokoban.target++;
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.transform.name.CompareTo("Box") == 0) Sokoban.target--;
	}
}
