using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Test_NoteButtonController : MonoBehaviour
{

	[SerializeField] private bool moveRight;
	[SerializeField] private float dragDelay;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float deleteDelay;

	private Button button_Component;

	private float x, y;
	public int buttonIndx;
	private bool isDrag;
	private float touchDownTime = 0;

	private Vector2 touchDownPos;
	private Vector2 touchUpPos;

	public void OnTouchDown ()
	{
		touchDownPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		touchDownTime = Time.time;
	}

	public void OnTouchUp ()
	{
		touchUpPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		if (Time.time - touchDownTime < dragDelay)
		{
			if (!isDrag)
			{
				ButtonManager.Instance.currOpenedButton = gameObject;
				ButtonManager.Instance.OpenNote (gameObject);
			}
		}

		else
		{
			if (touchDownPos.x < touchUpPos.x)
				moveRight = true;

			else
				moveRight = false;

			transform.SetParent (ButtonManager.Instance.centerPanel.transform);
			isDrag = true;
			StartCoroutine (DeleteWithDelay ());
		}
	}

	private void FixedUpdate ()
	{
		MoveButton ();
	}

	private void MoveButton ()
	{
		if (isDrag)
		{
			if (moveRight)
				GetComponent<RectTransform> ().position = new Vector2 (GetComponent<RectTransform> ().position.x + moveSpeed * Time.deltaTime, GetComponent<RectTransform> ().position.y);
			else
				GetComponent<RectTransform> ().position = new Vector2 (GetComponent<RectTransform> ().position.x - moveSpeed * Time.deltaTime, GetComponent<RectTransform> ().position.y);
		}
	}

	private IEnumerator DeleteWithDelay ()
	{
		yield return new WaitForSeconds (deleteDelay);
		ButtonManager.Instance.DeleteNote (gameObject);

	}
}