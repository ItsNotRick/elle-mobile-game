using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour {

    private bool swipeUp, swipeDown, swipeRight, isDragging = false;
	private Vector2 startTouch, swipeDelta;

	public Vector2 SwipeDelta{ get {return swipeDelta;}}
	public bool SwipeUp{ get { return swipeUp; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeDown{ get { return swipeDown; } }

	private void Reset()
	{
		startTouch = swipeDelta = Vector2.zero;
		isDragging = false;
	}
	
	// Update is called once per frame
	void Update () 
	{

		//PC Inputs
		swipeUp = swipeDown = swipeRight= false;

        #region Standalone Input
        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
			startTouch = Input.mousePosition;
		}
		else if(Input.GetMouseButtonUp(1))
        {
            isDragging = false;
			Reset();
		}

        #endregion

        #region Mobile Input
        if(Input.touches.Length > 0)
			{
				if(Input.touches[0].phase == TouchPhase.Began)
				{
					startTouch = Input.touches[0].position;
				}
				else if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
				{
					Reset();
				}
			}
        #endregion

        swipeDelta = Vector2.zero;
		if(isDragging)
		{
			if(Input.touches.Length > 0)
				swipeDelta = Input.touches[0].position - startTouch;
			else if (Input.GetMouseButton(0)) 
				swipeDelta = (Vector2)Input.mousePosition - startTouch;
		}
        

        if (swipeDelta.magnitude > 125)
		{
			float x = swipeDelta.x;
			float y = swipeDelta.y;
			if(Mathf.Abs(x) >Mathf.Abs(y))
            {
                if (x > 0)
                    swipeRight = true;

            }
            else
            {
                if (y < 0)
                {
                    swipeDown = true;
                }
                else
                {
                    swipeUp = true;
                }
            }
		}
	}
}
