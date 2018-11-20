using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AtlasManager))]
[RequireComponent(typeof(PanelManager))]


public class DialogueManager : MonoBehaviour
{
    private List<DialogueStateManager> managerList = new List<DialogueStateManager>();

    public static AtlasManager atlasManager { get; private set; }

    public static PanelManager panelManeger { get; private set; }
    void Awake()
    {
        atlasManager = GetComponent<AtlasManager>();
        

        panelManeger = GetComponent<PanelManager>();

        managerList.Add(atlasManager);
        managerList.Add(panelManeger);
        StartCoroutine(BootAllManagers());
    }

    private IEnumerator BootAllManagers()
    {
        foreach (DialogueStateManager manager in managerList)
        {
            manager.BootSequence();
        }
        yield return null;
    }
}
