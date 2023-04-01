using UnityEngine;
using UnityEngine.UIElements;

public class UISingleton : MonoBehaviour
{
    private static UIDocument _instance = null;
    public static UIDocument Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UISingleton>().GetComponent<UIDocument>();
            }
            
            return _instance;
        }
    }
    
    public static T Q<T>(string name) where T : VisualElement => Instance.rootVisualElement.Q<T>(name);
}
