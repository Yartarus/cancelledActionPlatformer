using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{

    private Camera camera1;

    private float height, width;

    private void Awake()
    {
        //camera1 = 
    }

    // By  Adrian Lopez (https://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html)
    // Use this for initialization
    void Start()
    {
        SetCameraSize();
    }

    void Update()
    {
        if (height != (float)Screen.height || width != (float)Screen.width)
        {
            SetCameraSize();
        }
    }

    void SetCameraSize()
    {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 4.0f / 3.0f;

        // determine the game window's current aspect ratio
        height = (float)Screen.height;
        width = (float)Screen.width;
        float windowaspect = width / height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Debug.Log("toowide");
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            Debug.Log("toohigh");
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

}
