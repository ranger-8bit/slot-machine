

/*using System.Collections;
using UnityEngine;

public class ReelController : MonoBehaviour
{
    [Header("Symbol Settings")]
    [SerializeField] private SpriteRenderer[] symbolRenderers;
    [SerializeField] private Sprite[] symbols;

    [Header("Spin Settings")]
    [SerializeField] private float symbolSpacing = 1.2f;
    [SerializeField] private float spinSpeed = 12f;

    public int SymbolCount => symbols == null ? 0 : symbols.Length;

    private bool isSpinning;

    private void Start()
    {
        SetupSymbols();
    }

    private void SetupSymbols()
    {
        if (symbolRenderers == null || symbolRenderers.Length == 0)
        {
            Debug.LogError("Assign symbol renderers on " + name);
            return;
        }

        for (int i = 0; i < symbolRenderers.Length; i++)
        {
            symbolRenderers[i].transform.localPosition =
                new Vector3(0, (2 - i) * symbolSpacing, 0);

            symbolRenderers[i].sprite =
                symbols[Random.Range(0, symbols.Length)];
        }
    }

    public IEnumerator SpinTo(int finalIndex, float duration)
    {
        if (isSpinning || symbols == null || symbols.Length == 0)
            yield break;

        if (symbolRenderers == null || symbolRenderers.Length < 5)
        {
            Debug.LogError("Each reel needs at least 5 symbol objects!");
            yield break;
        }

        isSpinning = true;

        float elapsed = 0f;
        float totalDistance = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / duration);

            // Gradually reduce speed
            float currentSpeed = Mathf.Lerp(
                spinSpeed,
                2f,
                progress * progress
            );

            float movement = currentSpeed * Time.deltaTime;
            totalDistance += movement;

            MoveSymbols(movement);

            yield return null;
        }

        // Snap the final centre symbol into place
        SetFinalSymbol(finalIndex);

        isSpinning = false;
    }

    private void MoveSymbols(float movement)
    {
        float resetPosition = -2f * symbolSpacing;
        float newPosition = 3f * symbolSpacing;

        foreach (SpriteRenderer renderer in symbolRenderers)
        {
            Vector3 position = renderer.transform.localPosition;
            position.y -= movement;

            if (position.y < resetPosition)
            {
                position.y = newPosition;

                int randomIndex = Random.Range(0, symbols.Length);
                renderer.sprite = symbols[randomIndex];
            }

            renderer.transform.localPosition = position;
        }
    }

    private void SetFinalSymbol(int finalIndex)
    {
        // Centre symbol stops on the selected result
        symbolRenderers[2].sprite = symbols[finalIndex];
        symbolRenderers[2].transform.localPosition =
            Vector3.zero;

        symbolRenderers[1].transform.localPosition =
            new Vector3(0, symbolSpacing, 0);

        symbolRenderers[0].transform.localPosition =
            new Vector3(0, symbolSpacing * 2, 0);

        symbolRenderers[3].transform.localPosition =
            new Vector3(0, -symbolSpacing, 0);

        symbolRenderers[4].transform.localPosition =
            new Vector3(0, -symbolSpacing * 2, 0);
    }
}*/

using System.Collections;
using UnityEngine;

public class ReelController : MonoBehaviour
{
    [Header("Symbol Settings")]
    [SerializeField] private SpriteRenderer[] symbolRenderers;
    [SerializeField] private Sprite[] symbols;

    [Header("Spin Settings")]
    [SerializeField] private float symbolSpacing = 1f;
    [SerializeField] private float spinSpeed = 12f;

    public int SymbolCount => symbols == null ? 0 : symbols.Length;

    private bool isSpinning;

    private void Start()
    {
        SetupSymbols();
    }

    private void SetupSymbols()
    {
        if (symbolRenderers == null || symbolRenderers.Length != 5)
        {
            Debug.LogError("Each reel needs exactly 5 symbol objects!");
            return;
        }

        for (int i = 0; i < symbolRenderers.Length; i++)
        {
            symbolRenderers[i].transform.localPosition =
                new Vector3(0, (2 - i) * symbolSpacing, 0);

            symbolRenderers[i].sprite =
                symbols[Random.Range(0, symbols.Length)];
        }
    }

    public IEnumerator SpinTo(int finalIndex, float duration)
    {
        if (isSpinning || symbols == null || symbols.Length == 0)
            yield break;

        if (symbolRenderers == null || symbolRenderers.Length != 5)
        {
            Debug.LogError("Assign 5 symbol renderers to " + name);
            yield break;
        }

        isSpinning = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / duration);

            float currentSpeed = Mathf.Lerp(
                spinSpeed, 2f, progress * progress
            );

            MoveSymbols(currentSpeed * Time.deltaTime);

            yield return null;
        }

        SetFinalSymbol(finalIndex);
        isSpinning = false;
    }

    private void MoveSymbols(float movement)
    {
        foreach (SpriteRenderer renderer in symbolRenderers)
        {
            Vector3 position = renderer.transform.localPosition;
            position.y -= movement;

            if (position.y < -2f * symbolSpacing)
            {
                position.y = 3f * symbolSpacing;
                renderer.sprite = symbols[Random.Range(0, symbols.Length)];
            }

            renderer.transform.localPosition = position;
        }
    }

    private void SetFinalSymbol(int finalIndex)
    {
        symbolRenderers[2].sprite = symbols[finalIndex];

        symbolRenderers[0].transform.localPosition =
            new Vector3(0, 2f * symbolSpacing, 0);

        symbolRenderers[1].transform.localPosition =
            new Vector3(0, symbolSpacing, 0);

        symbolRenderers[2].transform.localPosition = Vector3.zero;

        symbolRenderers[3].transform.localPosition =
            new Vector3(0, -symbolSpacing, 0);

        symbolRenderers[4].transform.localPosition =
            new Vector3(0, -2f * symbolSpacing, 0);
    }
}


