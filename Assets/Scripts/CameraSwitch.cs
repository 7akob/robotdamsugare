using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    void Start()
    {
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            bool isCamera1Active = camera1.gameObject.activeSelf;

            camera1.gameObject.SetActive(!isCamera1Active);
            camera2.gameObject.SetActive(isCamera1Active);
        }
    }
}