using UnityEngine;

public class FTS : MonoBehaviour
{
    void Start()
    {
        Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero) * 100;

        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(mainCamera.rect.width, mainCamera.rect.height)) * 100;

        Vector3 screenSize = topRight - bottomLeft;

        float screenRatio = screenSize.x / screenSize.y;

        float wantedRatio = transform.localScale.x / transform.localScale.y;

        if (screenRatio > wantedRatio)
        {
            float height = screenSize.y;

            transform.localScale = new Vector3(height * wantedRatio, height);
        }
        else
        {
            float width = screenSize.x;

            transform.localScale = new Vector3(width, width / wantedRatio);
        }
    }
}
