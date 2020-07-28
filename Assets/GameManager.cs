using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public Volume grainVolume;
    public Transform drawingPlane;
    public Renderer drawingRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            float amount = grainVolume.weight == 1 ? 0 : 1;

            drawingPlane.gameObject.SetActive(!drawingPlane.gameObject.activeSelf);
            DOVirtual.Float(grainVolume.weight, amount, .5f, (x) => grainVolume.weight = x);
            drawingRenderer.material.DOFloat(amount, "SepiaAmount", .5f);
        }
    }
}
