using UnityEngine;
using System.Collections.Generic;

public class SequentialSoundPlayer : MonoBehaviour
{
    [Header("Sound Sequence")]
    [SerializeField] private List<SoundSlot> soundSlots = new List<SoundSlot>();
    [SerializeField] private bool playOnCollision = true;
    [SerializeField] private bool playOnArenaCollision = true;
    [SerializeField] private bool playOnBallCollision = true;

    private AudioSource audioSource;
    private int currentSlotIndex = 0;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayNextSound()
    {
        if (soundSlots == null || soundSlots.Count == 0)
        {
            Debug.LogWarning("No sound slots configured!");
            return;
        }

        // Get current sound slot
        SoundSlot currentSlot = soundSlots[currentSlotIndex];

        // Play the sound with customized settings
        if (currentSlot.sound != null)
        {
            audioSource.pitch = currentSlot.pitch;
            audioSource.PlayOneShot(currentSlot.sound, currentSlot.volume);
        }

        // Move to next slot (with wrap-around)
        currentSlotIndex = (currentSlotIndex + 1) % soundSlots.Count;
    }

    // Call this from your collision handler
    public void HandleCollision(Collision2D collision)
    {
        if (!playOnCollision) return;

        bool isArenaCollision = collision.gameObject.name == "ArenaCollider";
        bool isBallCollision = collision.gameObject.CompareTag("ball");

        if ((isArenaCollision && playOnArenaCollision) ||
            (isBallCollision && playOnBallCollision))
        {
            PlayNextSound();
        }
    }
}
