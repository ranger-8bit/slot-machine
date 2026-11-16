

using System.Collections;
using UnityEngine;
using TMPro;

public class SlotMachineManager : MonoBehaviour
{
    [Header("Reels")]
    [SerializeField] private ReelController[] reels;

    [Header("UI")]
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text jackpotText;
    [SerializeField] private TMP_Text gameOverText;

    [Header("Game Settings")]
    [SerializeField] private int startingBalance = 500;
    [SerializeField] private int spinCost = 5;

    // Seven, Cherries, Bell, BAR
    [SerializeField] private int[] payouts = { 100, 50, 75, 25 };

    private int balance;
    private bool isPlaying;

    private void Start()
    {
        balance = startingBalance;

        jackpotText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);

        UpdateBalance();
        resultText.text = "GOOD LUCK!";
    }

    public bool TrySpin()
    {
        if (isPlaying)
            return false;

        if (balance < spinCost)
        {
            gameOverText.gameObject.SetActive(true);
            resultText.text = "NOT ENOUGH COINS!";
            return false;
        }

        Spin();
        return isPlaying;
    }

    public void Spin()
    {
        if (isPlaying || balance < spinCost)
            return;

        if (reels == null || reels.Length != 3)
        {
            Debug.LogError("Assign exactly three reels!");
            return;
        }

        foreach (ReelController reel in reels)
        {
            if (reel == null || reel.SymbolCount != payouts.Length)
            {
                Debug.LogError("Check reel references and symbol arrays.");
                return;
            }
        }

        StartCoroutine(PlaySpin());
    }

    private IEnumerator PlaySpin()
    {
        isPlaying = true;

        jackpotText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);

        balance -= spinCost;
        UpdateBalance();

        resultText.text = "SPINNING...";

        int[] results = new int[3];

        for (int i = 0; i < 3; i++)
            results[i] = Random.Range(0, payouts.Length);

        float[] durations = { 2.5f, 3.2f, 3.9f };

        for (int i = 0; i < 3; i++)
            StartCoroutine(reels[i].SpinTo(results[i], durations[i]));

        yield return new WaitForSeconds(4f);

        CheckResult(results);

        isPlaying = false;
    }

    private void CheckResult(int[] results)
    {
        if (results[0] == results[1] && results[1] == results[2])
        {
            int reward = payouts[results[0]];
            balance += reward;

            resultText.text = "YOU WON " + reward + " COINS!";
            jackpotText.gameObject.SetActive(true);

            StartCoroutine(PopJackpot());
        }
        else
        {
            resultText.text = "NO MATCH! TRY AGAIN!";
        }

        UpdateBalance();

        if (balance < spinCost)
        {
            gameOverText.gameObject.SetActive(true);
        }
    }

    private IEnumerator PopJackpot()
    {
        Transform jackpotTransform = jackpotText.transform;

        jackpotTransform.localScale = Vector3.zero;

        float elapsed = 0f;
        float duration = 0.4f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float scale = Mathf.Lerp(
                0f, 1.2f, elapsed / duration
            );

            jackpotTransform.localScale = Vector3.one * scale;

            yield return null;
        }

        jackpotTransform.localScale = Vector3.one;
    }

    private void UpdateBalance()
    {
        balanceText.text = "" + balance;
    }

    public void ResetGame()
    {
        if (isPlaying)
            return;

        balance = startingBalance;

        jackpotText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);

        UpdateBalance();
        resultText.text = "GAME RESET!";
    }
}
