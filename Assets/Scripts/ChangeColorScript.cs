using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeColorScript : MonoBehaviour
{
	public InputField inputNote;
	// Use this for initialization
	public void ChangeNoteColor ()
	{
		Color clr = new Color ();
		clr = gameObject.GetComponent<Image> ().color;
		inputNote.GetComponent<Image> ().color = clr;
	}
}