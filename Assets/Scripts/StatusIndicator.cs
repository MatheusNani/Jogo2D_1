using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

    [SerializeField]
    private RectTransform healthBarRect;
    [SerializeField]
    private Text HealthText;

    private void Start()
    {
        if(healthBarRect == null)
        {
            Debug.LogError("STATUS INDICATOR: No health bar object referenced!");
        }
        if (HealthText == null)
        {
            Debug.LogError("STATUS INDICATOR: No health text object referenced!");
        }
    }

    public void SetHealth(int _currentH, int _maxH)
    {
        float _value = (float)_currentH / _maxH;
        healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        HealthText.text = _currentH + "/" + _maxH + "HP";

    }

}
