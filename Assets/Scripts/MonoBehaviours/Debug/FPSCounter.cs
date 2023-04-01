using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class FPSCounter : MonoBehaviour
{
    private string _prefix;
    private Label _fpsLabel;

    private void Awake()
    {
        _fpsLabel = GetComponent<UIDocument>().rootVisualElement.Q<Label>("fps");
        _prefix = _fpsLabel.text;
    }

    private void Update()
    {
        _fpsLabel.text = _prefix + (1 / Time.deltaTime).ToString("N0");
    }
}
