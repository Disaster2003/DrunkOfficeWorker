using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    [SerializeField] Vector3 position_Goal;   // –Ú•W’n“_‚ÌxÀ•W

    [Header("—h‚ê‚Ì•A•p“x")]
    private float shakeAmplitude = 5.0f; // —h‚ê‚ÌU•
    private float shakeFrequency = 5.0f; // —h‚ê‚Ì•p“x
    private float shakeTime = 0f; // —h‚ê‚ÌŠÔ‚ğŠÇ—‚·‚é

    void Update()
    {
        if (position_Goal == Vector3.zero)
        {
            return;
        }

        //// ¬‚İ‚É—h‚ê‚é“®‚«‚ğPerlinNoise‚Å‰Á‚¦‚é
        //shakeTime += Time.deltaTime * shakeFrequency;
        //float shakeOffset = Mathf.PerlinNoise(shakeTime, 0) * shakeAmplitude * 2f - shakeAmplitude; // -shakeAmplitude ‚Å—h‚ê‚ª-U•~+U•‚Ì”ÍˆÍ‚Å¬‚İ‚É“®‚­
        //transform.position += new Vector3(0, shakeOffset);

        // V‚µ‚¢ˆÊ’u‚É—h‚ê‚ÌƒIƒtƒZƒbƒg‚ğ‰Á‚¦‚é
        Vector3 positionTarget = new Vector3(-position_Goal.x + SpawnerPlay.GetCount * 100, transform.localPosition.y/* + shakeOffset*/);

        // Œ»İ‚ÌˆÊ’u‚ğüŒ`•âŠÔ‚ÅŒvZ
        transform.localPosition = Vector3.Lerp(transform.localPosition, positionTarget, Time.deltaTime);
    }
}
