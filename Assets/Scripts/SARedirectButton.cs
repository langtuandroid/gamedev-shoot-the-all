using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SARedirectButton : MonoBehaviour
{
    [SerializeField] private string _link;
    [SerializeField] private List<Button> _buttonsToDisable;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            foreach (var button in _buttonsToDisable) button.interactable = false;
            Application.OpenURL(_link);
            StartCoroutine(EnableButtons());
        });
    }

    private IEnumerator EnableButtons()
    {
        yield return new WaitForSeconds(2);
        foreach (var button in _buttonsToDisable) button.interactable = true;
    }
}