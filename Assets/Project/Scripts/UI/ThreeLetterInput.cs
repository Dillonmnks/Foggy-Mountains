using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThreeLetterInput : MonoBehaviour, IPointerClickHandler
{
    public TMP_InputField field;

    private void Awake()
    {
        field.characterLimit = 3;
        field.onValidateInput += ValidateChar;

        field.onSubmit.AddListener(_ => RemoveClipboard());
        field.onValueChanged.AddListener(_ => RemoveClipboard());
    }

    private char ValidateChar(string text, int charIndex, char addedChar)
    {
        if (addedChar == ' ')
            return '\0';

        if (!char.IsLetter(addedChar))
            return '\0';

        return char.ToUpperInvariant(addedChar);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RemoveClipboard();
    }

    private void RemoveClipboard()
    {
        GUIUtility.systemCopyBuffer = "";
    }
}
