using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class VisibilitySystem : MonoBehaviour
{
    [SerializeField]
    private SpiritController Target;

    private List<SpiritController> AllSpirits = new List<SpiritController>();

    bool init = true;

    private void ShowSpirits()
    {
        UpdateVisibility(true);
    }

    private void HideSpirits()
    {
        UpdateVisibility(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshSpiritList();

        Target.PossessionSystem.CharacterPossessed += HideSpirits;
        Target.PossessionSystem.PossessionReleased += ShowSpirits;
    }

    // Update is called once per frame
    void Update()
    {
        if (init)
        {
            init = false;
            if (Target.PossessionSystem.IsPossessing)
            {
                HideSpirits();
            }
            else
            {
                ShowSpirits();
            }
        }
    }

    private void RefreshSpiritList()
    {
        AllSpirits = FindObjectsOfType<SpiritController>().ToList();
    }

    private void UpdateVisibility(bool showSpirits)
    {
        Debug.Log($"Setting visibility: {showSpirits}");
        foreach (var spirit in AllSpirits)
        {
            spirit.SetVisibility(showSpirits);
        }
    }
}
