
/*using System.Collections;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    [Header("Lever Sprites")]
    [SerializeField] private GameObject leverUp;
    [SerializeField] private GameObject leverDown;

    [Header("Game Manager")]
    [SerializeField] private SlotMachineManager gameManager;

    [Header("Animation")]
    [SerializeField] private float pullDuration = 0.2f;
    [SerializeField] private float holdDuration = 0.15f;

    private bool isAnimating;

    private void Start()
    {
        leverUp.SetActive(true);
        leverDown.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (isAnimating || gameManager == null)
            return;

        if (gameManager.TrySpin())
        {
            StartCoroutine(PullLever());
        }
    }

    private IEnumerator PullLever()
    {
        isAnimating = true;

        leverUp.SetActive(false);
        leverDown.SetActive(true);

        yield return new WaitForSeconds(pullDuration);

        yield return new WaitForSeconds(holdDuration);

        leverDown.SetActive(false);
        leverUp.SetActive(true);

        isAnimating = false;
    }
}*/

/*using UnityEngine;

public class LeverController : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("LEVER CLICK WORKS!");
    }
}*/

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeverController : MonoBehaviour
{
    [Header("Lever Sprites")]
    [SerializeField] private GameObject leverUp;
    [SerializeField] private GameObject leverDown;

    [Header("Game Manager")]
    [SerializeField] private SlotMachineManager gameManager;

    private Collider2D leverCollider;
    private Camera mainCamera;
    private bool isAnimating;

    private void Start()
    {
        leverCollider = GetComponent<Collider2D>();
        mainCamera = Camera.main;

        leverUp.SetActive(true);
        leverDown.SetActive(false);

        if (leverCollider == null)
            Debug.LogError("Lever needs a Collider2D!");

        if (mainCamera == null)
            Debug.LogError("Main Camera not found!");
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(
                Mouse.current.position.ReadValue()
            );

            if (leverCollider != null &&
                leverCollider.OverlapPoint(mousePosition))
            {
                Debug.Log("LEVER CLICK DETECTED!");

                if (!isAnimating && gameManager != null)
                {
                    if (gameManager.TrySpin())
                        StartCoroutine(PullLever());
                }
            }
        }
    }

    private IEnumerator PullLever()
    {
        isAnimating = true;

        leverUp.SetActive(false);
        leverDown.SetActive(true);

        yield return new WaitForSeconds(0.35f);

        leverDown.SetActive(false);
        leverUp.SetActive(true);

        isAnimating = false;
    }
}



