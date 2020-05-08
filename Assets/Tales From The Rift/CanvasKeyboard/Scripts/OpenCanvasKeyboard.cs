using UnityEngine;
using System.Collections;
using TMPro;

namespace TalesFromTheRift
{
	public class OpenCanvasKeyboard : MonoBehaviour 
	{
		// Canvas to open keyboard under
		public Canvas CanvasKeyboardObject;

		// Optional: Input Object to receive text 
		public TMP_InputField input;

		public void OpenKeyboard() 
		{		
			CanvasKeyboard.Open(CanvasKeyboardObject, input);
		}

		public void CloseKeyboard() 
		{		
			CanvasKeyboard.Close ();
		}
	}
}