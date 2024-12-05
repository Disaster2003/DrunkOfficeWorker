using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    [SerializeField] Vector3 position_Goal;   // –Ú•W’n“_‚ÌxÀ•W

    [Header("—h‚ê‚Ì•A•p“x")]
    public float shakeAmplitude = 5.0f; // —h‚ê‚ÌU•
    public float shakeFrequency = 5.0f; // —h‚ê‚Ì•p“x
    private float shakeTime = 0f; // —h‚ê‚ÌŠÔ‚ğŠÇ—‚·‚é

    void Update()
    {
        if (position_Goal == Vector3.zero)
        {
            return;
        }

        Vector3 positionNew =
            Vector3.Lerp
            (
                transform.localPosition,
                new Vector3(-position_Goal.x + SpawnerPlay.GetCount * 100, transform.localPosition.y),
                Time.deltaTime
            );

        //// ¬‚İ‚É—h‚ê‚é“®‚«‚ğPerlinNoise‚Å‰Á‚¦‚é
        //shakeTime += Time.deltaTime * shakeFrequency;
        //float shakeOffset = Mathf.PerlinNoise(shakeTime, 0) * shakeAmplitude * 2f - shakeAmplitude; // -shakeAmplitude ‚Å—h‚ê‚ª-U•~+U•‚Ì”ÍˆÍ‚Å¬‚İ‚É“®‚­
        //positionNew.y += shakeOffset;

        //// V‚µ‚¢ˆÊ’u‚É—h‚ê‚ÌƒIƒtƒZƒbƒg‚ğ‰Á‚¦‚é
        //positionNew.y += shakeOffset;

        // Œ»İ‚ÌˆÊ’u‚ğüŒ`•âŠÔ‚ÅŒvZ
        transform.localPosition = positionNew;         
    }
}
