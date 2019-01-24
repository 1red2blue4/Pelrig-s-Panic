using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void TurnOn(GameObject on)
    {
        on.SetActive(true);
    }

    public void TurnOff(GameObject off)
    {
        off.SetActive(false);
    }

}
