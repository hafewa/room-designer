using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine.EventSystems;

namespace TalesFromTheRift
{
    public class CanvasKeyboard : MonoBehaviour
    {
        #region CanvasKeyboard Instantiation

        public enum CanvasKeyboardType
        {
            ASCIICapable
        }

        public static CanvasKeyboard Open(Canvas canvas, TMP_InputField inputObject = null,
            CanvasKeyboardType keyboardType = CanvasKeyboardType.ASCIICapable)
        {
            // Don't open the keyboard if it is already open for the current input object
            CanvasKeyboard keyboard = GameObject.FindObjectOfType<CanvasKeyboard>();
            if (keyboard == null || (keyboard != null && keyboard.inputObject != inputObject))
            {
                Close();
                keyboard = Instantiate<CanvasKeyboard>(Resources.Load<CanvasKeyboard>("CanvasKeyboard"));
                keyboard.transform.SetParent(canvas.transform, false);
                keyboard.inputObject = inputObject;

                var keyboardAscii = keyboard.GetComponentInChildren<CanvasKeyboardASCII>();

                switch (inputObject.contentType)
                {
                    case TMP_InputField.ContentType.DecimalNumber:
                        var altKey = new GameObject("ALT");
                        keyboardAscii.OnKeyDown(altKey);
                        Destroy(altKey);
                        
                        break;
                    
                    default:
                        break;
                }
            }

            return keyboard;
        }

        public static void Close()
        {
            CanvasKeyboard[] kbs = GameObject.FindObjectsOfType<CanvasKeyboard>();
            foreach (CanvasKeyboard kb in kbs)
            {
                kb.CloseKeyboard();
            }
        }

        public static bool IsOpen
        {
            get { return GameObject.FindObjectsOfType<CanvasKeyboard>().Length != 0; }
        }

        #endregion

        public TMP_InputField inputObject;

        public string text
        {
            get
            {
                if (inputObject == null) return "";

                var tmpField = inputObject.GetComponent<TMP_InputField>();
                if (tmpField == null) return "";

                var prop = tmpField.GetType().GetProperty("text", BindingFlags.Instance | BindingFlags.Public);
                if (prop == null) return "";

                return prop.GetValue(tmpField, null) as string;
            }

            set
            {
                if (inputObject == null) return;

                var tmpField = inputObject.GetComponent<TMP_InputField>();
                if (tmpField == null) return;

                var prop = tmpField.GetType().GetProperty("text", BindingFlags.Instance | BindingFlags.Public);
                if (prop == null) return;
                
                var charValue = value.Length != 0 ? value[value.Length-1] : '0';

                var fieldType = tmpField.contentType;
                switch (fieldType)
                {
                    case TMP_InputField.ContentType.DecimalNumber:
                        if (charValue != '.' && (charValue < '0' || charValue > '9')) return;

                        break;
                    default:
                        break;
                }

                prop.SetValue(tmpField, value, null);
            }
        }

        #region Keyboard Receiving Input

        public void SendKeyString(string keyString)
        {
            if (keyString.Length == 1 && keyString[0] == 8 /*ASCII.Backspace*/)
            {
                if (text.Length > 0)
                {
                    text = text.Remove(text.Length - 1);
                }
            }
            else
            {
                text += keyString;
            }

            // Workaround: Restore focus to input fields (because Unity UI buttons always steal focus)
            // ReactivateInputField(inputObject);
        }

        public void CloseKeyboard()
        {
            Destroy(gameObject);
        }

        #endregion


        #region Steal Focus Workaround

        void ReactivateInputField(TMP_InputField inputField)
        {
            if (inputField != null)
            {
                StartCoroutine(ActivateInputFieldWithoutSelection(inputField));
            }
        }

        IEnumerator ActivateInputFieldWithoutSelection(TMP_InputField inputField)
        {
            inputField.ActivateInputField();

            // wait for the activation to occur in a lateupdate
            yield return new WaitForEndOfFrame();

            // make sure we're still the active ui
            if (EventSystem.current.currentSelectedGameObject == inputField.gameObject)
            {
                // To remove hilight we'll just show the caret at the end of the line
                inputField.MoveTextEnd(false);
            }
        }

        #endregion
    }
}