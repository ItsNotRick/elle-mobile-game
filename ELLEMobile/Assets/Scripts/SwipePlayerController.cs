using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipePlayerController : MonoBehaviour {

	public Swipe swipeControls;
	public Transform player;
	private Vector3 desiredPosition = Vector3.zero;
	
	// Update is called once per frame
	void Update () {

		if (swipeControls.SwipeUp)
			desiredPosition += Vector3.up;
		if (swipeControls.SwipeDown)
			desiredPosition += Vector3.down;
		if (swipeControls.SwipeRight)
			desiredPosition += Vector3.right;

		player.transform.position = Vector3.MoveTowards (player.transform.position, desiredPosition, 3f * Time.deltaTime);

		
	}
}
