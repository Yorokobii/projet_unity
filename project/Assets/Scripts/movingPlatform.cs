using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour {
	public Vector3 pointB;
	public float speed;
	private Vector3 pointA;

	IEnumerator Start () {
		pointA = transform.localPosition;

		while (true)
		{
			yield return StartCoroutine(MoveObject(transform, pointA, pointB, 3.0f));
			yield return StartCoroutine(MoveObject(transform, pointB, pointA, 3.0f));
		}
	}

	IEnumerator MoveObject (Transform thisTransform, Vector3 startPos, Vector3 endPos , float time) {
		float i = 0.0f;
		float rate = speed/time;
		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			thisTransform.localPosition = Vector3.Lerp(startPos, endPos, i);
			yield return null; 
		}
	}
		
}