using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public Transform brush;
    public Animator brushAnimator;
    public Camera brushCamera;

    public Volume grainVolume;
    //public Transform drawingPlane;
    public Renderer drawingRenderer;
    public CinemachineFreeLook freeLook;

    public bool isDrawing;


    private void Start()
    {
        Cursor.visible = false;

        if (brushCamera != null)
            brushCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        Vector3 temp = Input.mousePosition;
        temp.z = .4f;
        if(isDrawing)
            brush.position = Vector3.Lerp(brush.position,Camera.main.ScreenToWorldPoint(temp),.5f);
        ClampPosition(brush);

        if (Input.GetKeyDown(KeyCode.C))
        {
            SetDrawingMode(!isDrawing);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            SetDrawingMode(!isDrawing);
        }

        if(brushAnimator != null)
        {
            brushAnimator.SetFloat("X", Mathf.Lerp(brushAnimator.GetFloat("X"),Input.GetAxis("Mouse X") * 1,.07f));
            brushAnimator.SetFloat("Y", Mathf.Lerp(brushAnimator.GetFloat("Y"),Input.GetAxis("Mouse Y") * 1,.07f));
            brushAnimator.SetBool("isDrawing", Input.GetMouseButton(0));
        }
    }

    void ClampPosition(Transform obj)
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(obj.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        obj.position = Camera.main.ViewportToWorldPoint(pos);
    }

    public void SetDrawingMode(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;

        if (state == true)
        {
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Default"));
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Interactables"));
            //brush.GetChild(0).localPosition.Set(brush.GetChild(0).localPosition.x, 0.17f, brush.GetChild(0).localPosition.z);
            brush.GetChild(0).DOLocalMoveY(0.17f, .3f).SetUpdate(true).From();
        }
        else
        {
            Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Default");
            Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Interactables");
        }

        isDrawing = state;

        //determine if time is running or not
        Time.timeScale = isDrawing ? 0 : 1;
        //determine if the freelook camera is active
        freeLook.enabled = !state;

        drawingRenderer.enabled = state;
        drawingRenderer.transform.GetChild(0).gameObject.SetActive(state);
        brushCamera.gameObject.SetActive(state);

        //effects
        float effectAmount = isDrawing ? 1 : 0;
        drawingRenderer.transform.DOLocalRotate(new Vector3(isDrawing ? 60 : 90, 180,0), .5f, RotateMode.Fast).SetUpdate(true);
        DOVirtual.Float(grainVolume.weight, effectAmount, .5f, (x) => grainVolume.weight = x).SetUpdate(true);
        drawingRenderer.material.DOFloat(effectAmount, "SepiaAmount", .5f).SetUpdate(true);

        if(state == false)
        {
            FindObjectOfType<Demo>().TryRecognize();
        }
    }
}
