using TMPro;
using UIScripts;
using UnityEngine;

public class EnemyHPViewer : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _text.text = $"HP: {GameUserInterface.instance.GetLockTargetHP()}";
    }
}