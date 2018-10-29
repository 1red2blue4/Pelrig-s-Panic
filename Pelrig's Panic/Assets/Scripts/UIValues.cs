using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIValues : MonoBehaviour {

    private int value;
    [SerializeField] public int initialValue;
    [SerializeField] private GridPositioner gridPositioner;
    private TextMesh textValue;

	// Use this for initialization
	void Start()
    {
        textValue = gameObject.GetComponent<TextMesh>();
        value = initialValue;
        textValue.text = value.ToString();
        gameObject.GetComponent<MeshRenderer>().sortingOrder = 3;
        gridPositioner.mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        gridPositioner.AdjustToCamera();
        gridPositioner.gameObject.transform.Rotate(new Vector3(90.0f, 0.0f, 0.0f));
    }

    void Update()
    {
        gridPositioner.AdjustToCamera();
        gridPositioner.gameObject.transform.Rotate(new Vector3(90.0f, 0.0f, 0.0f));
        textValue.text = value.ToString();
    }

    public void SetValue(int vl)
    {
        value = vl;
    }

    public int GetValue()
    {
        return value;
    }
}
