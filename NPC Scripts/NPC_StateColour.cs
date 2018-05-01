using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_StateColour : MonoBehaviour
{
    [Header("Attach to stateFlag. Applies the Current color to List of Materials.")]
    public List<Material> matList;
    private MeshRenderer stateFlagMR;
    private Color currColor;

	// Use this for initialization
	void Start ()
    {
        stateFlagMR = GetComponent<MeshRenderer>();
        currColor = stateFlagMR.material.color;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (currColor != stateFlagMR.material.color)
        {
            currColor = stateFlagMR.material.color;
            
            foreach (Material mat in matList)
            {
                mat.color = stateFlagMR.material.color;
            }
        }

	}
}
