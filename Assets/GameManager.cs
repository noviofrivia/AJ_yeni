using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Ýstediðin resimleri buradan koy")]

    [Tooltip("Soldaki takýmýn logosu (image)")]
    [SerializeField]private Sprite TeamLogo1;
    [Tooltip("Soldaki takýmýn topunun logosu (image)")]
    [SerializeField] private Sprite BallLogo1;
    [SerializeField] private Sprite Ball1_Border;

    [Tooltip("Saðdaki takýmýn logosu (image)")]
    [SerializeField]private Sprite TeamLogo2;
    [Tooltip("Saðdaki takýmýn topunun logosu (image)")]
    [SerializeField] private Sprite BallLogo2;
    [SerializeField] private Sprite Ball2_Border;

    [Tooltip("Background (image)")]
    [SerializeField]private Sprite CanvasBackground;
    [Tooltip("Aþaðý kýsýmdaki taraftarlarýn görüntüsü (image)")]
    [SerializeField]private Sprite AudienceSprite;
    [SerializeField]private Sprite GoalSprite;
    [SerializeField] private Sprite ArenaChamber;
    [SerializeField] private Sprite ScoreboardPanel;


    [Header("Gol olunca kaleye giren top ile birlikte resetlenecekler")]
    [Tooltip("Kutucuk iþaretlenirse gol olduðunda kale de resetlenir")]
    [SerializeField] private bool kaleyi_resetle = false;
    [Tooltip("Kutucuk iþaretlenirse gol olduðunda golü atan top da resetlenir")]
    [SerializeField] private bool gol_atan_topu_resetle = false;
    [Range(0f, 5f)] [SerializeField] private float reset_timer;

    [Header("Simülasyon süresi çarpaný")]
    [Tooltip("1 puan 90 saniye demek. Kaç sn istiyosan ona göre çarp. Örneðin 60sn için 1.5")]
    [SerializeField] private float gameTimeMultiplier = 1.5f;
    [SerializeField] private float simulasyon_baslama_suresi;

    [Header("Gol Sesleri")]
    [SerializeField] private AudioClip default_gol_sesi; // Sound when any team scores
    [SerializeField] private AudioClip birinci_takým_gol_sesi; // Sound when team 1 scores
    [SerializeField] private AudioClip ikinci_takým_gol_sesi; // Sound when team 2 scores
    [SerializeField] private AudioClip ambiyans_sesi; // New field for background music
    [Range(0f, 1f)][SerializeField] private float default_gol_sesi_yüksekliði = 1f;
    [Range(0f, 1f)][SerializeField] private float gol_sesi_yüksekliði = 1f;
    [Range(0f, 1f)][SerializeField] private float ambiyans_sesi_yüksekliði = 0.5f;
    [Range(0f, 1f)][SerializeField] private float top_carpisma_sesi_yüksekliði = 0.5f;


    [Header("Süre bitince resetlenecekler")]
    [Tooltip("If checked, Ball 1 will be disabled when the timer reaches 90 seconds.")]
    [SerializeField] private bool disableBall1At90 = true;

    [Tooltip("If checked, Ball 2 will be disabled when the timer reaches 90 seconds.")]
    [SerializeField] private bool disableBall2At90 = true;

    [Tooltip("If checked, the Goal will be disabled when the timer reaches 90 seconds.")]
    [SerializeField] private bool disableGoalAt90 = true;


    [Space(50)]
    [Header("Code-Related")]
    [SerializeField] private GameObject StartSimulationButton;
    [SerializeField] private GameObject GoalText_gameobject;
    [SerializeField] private GameObject ball_1;
    [SerializeField] private Transform ball_1_SpawnPosition;
    [SerializeField] private GameObject ball_2;
    [SerializeField] private Transform ball_2_SpawnPosition;
    [SerializeField] private GameObject goal;
    [SerializeField] private TextMeshProUGUI team1_score_text;
    [SerializeField] private TextMeshProUGUI team2_score_text;
    [SerializeField] private ShakeExample arena_shake_ref;
    [SerializeField] private GameObject firework_prefab;
    [SerializeField] private Transform goal_transform;
    [SerializeField] private AudioSource ball1AudioSource;
    [SerializeField] private AudioSource ball2AudioSource;

    private int team_1_score = 0;
    private int team_2_score = 0;


    [Header("Assigning Sprites")]
    [SerializeField]private GameObject TeamLogo1_gameobject;
    [SerializeField] private GameObject BallLogo1_gameobject;
    [SerializeField] private GameObject BallBorder1_gameobject;
    [SerializeField]private GameObject TeamLogo2_gameobject;
    [SerializeField] private GameObject BallLogo2_gameobject;
    [SerializeField] private GameObject BallBorder2_gameobject;
    [SerializeField]private GameObject Audience_gameobject;
    [SerializeField]private GameObject CanvasBackground_gameobject;
    [SerializeField] private GameObject GoalSprite_gameobject;
    [SerializeField] private GameObject ArenaChamber_gameobject;
    [SerializeField] private GameObject ScoreboardPanel_gameobject;

    [Header("Assigning Sprites")]
    [SerializeField]private TextMeshProUGUI timer;
    private float currentTime = 0f;
    private bool isRunning; // Controls if the timer is active
    private AudioSource audioSource;
    private AudioSource musicSource; // Separate AudioSource for music

    void Start()
    {
        if(TeamLogo1 != null) { TeamLogo1_gameobject.GetComponent<Image>().sprite = TeamLogo1; }
        if(BallLogo1 != null) { BallLogo1_gameobject.GetComponent<SpriteRenderer>().sprite = BallLogo1; }
        if (Ball1_Border != null) { BallBorder1_gameobject.GetComponent<SpriteRenderer>().sprite = Ball1_Border; }

        if (TeamLogo2 != null) { TeamLogo2_gameobject.GetComponent<Image>().sprite = TeamLogo2; }
        if (BallLogo2 != null) { BallLogo2_gameobject.GetComponent<SpriteRenderer>().sprite = BallLogo2; }
        if (Ball2_Border != null) { BallBorder2_gameobject.GetComponent<SpriteRenderer>().sprite = Ball2_Border; }

        if (CanvasBackground != null) { CanvasBackground_gameobject.GetComponent<SpriteRenderer>().sprite = CanvasBackground; }
        if(AudienceSprite != null) { Audience_gameobject.GetComponent<Image>().sprite = AudienceSprite; }
        if(GoalSprite != null) { GoalSprite_gameobject.GetComponent<SpriteRenderer>().sprite = GoalSprite; }
        if (ArenaChamber != null) { ArenaChamber_gameobject.GetComponent<SpriteRenderer>().sprite = ArenaChamber; }
        if (ScoreboardPanel != null) { ScoreboardPanel_gameobject.GetComponent<Image>().sprite = ScoreboardPanel; }

        team1_score_text.text = team_1_score.ToString();
        team2_score_text.text = team_2_score.ToString();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set up separate AudioSource for music
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true; // Important for background music
        musicSource.volume = ambiyans_sesi_yüksekliði;
        ball1AudioSource.volume = top_carpisma_sesi_yüksekliði;
        ball2AudioSource.volume = top_carpisma_sesi_yüksekliði;
    }


    void Update()
    {
        if (!isRunning) return; // Stop if timer is paused

        currentTime += Time.deltaTime * gameTimeMultiplier;

        // Stop at 90 seconds
        if (currentTime >= 90f)
        {
            currentTime = 90f;
            isRunning = false; // Optional: Stop timer at 90

            if (disableBall1At90 && ball_1 != null)
            {
                ball_1.SetActive(false);
            }
            if (disableBall2At90 && ball_2 != null)
            {
                ball_2.SetActive(false);
            }
            if (disableGoalAt90 && goal != null)
            {
                goal.SetActive(false);
            }
        }

        UpdateTimerDisplay();
    }

    void StartSimulation()
    {
        StartCoroutine(StartSimulation_Coroutine());
        UndisplayStartSimulationButton();
    }

    IEnumerator StartSimulation_Coroutine()
    {
        yield return new WaitForSeconds(simulasyon_baslama_suresi);

        ball_1.SetActive(true);
        ball_2.SetActive(true);
        goal.SetActive(true);
        
        StartTimer();

        PlayBackgroundMusic();
    }

    void PlayBackgroundMusic()
    {
        if (ambiyans_sesi != null && musicSource != null)
        {
            musicSource.clip = ambiyans_sesi;
            musicSource.Play();
        }
    }

    public void Ball_1_Goal()
    {
        StartCoroutine(Ball_1_Goal_Coroutine());
    }

    IEnumerator Ball_1_Goal_Coroutine()
    {
        arena_shake_ref.ShakeObject();
        team_1_score++;
        team1_score_text.text = team_1_score.ToString();
        GoalText_gameobject.SetActive(true);

        // Play default goal sound first
        if (default_gol_sesi != null && audioSource != null)
        {
            audioSource.PlayOneShot(default_gol_sesi, default_gol_sesi_yüksekliði);
        }

        // Play goal sound #1 (team 2 scores)
        if (birinci_takým_gol_sesi != null && audioSource != null)
        {
            audioSource.PlayOneShot(birinci_takým_gol_sesi, gol_sesi_yüksekliði);
        }

        // Spawn fireworks at goal position
        GameObject instantiated_firework_prefab = Instantiate(firework_prefab, goal_transform.position, Quaternion.identity);


        if (kaleyi_resetle) { goal.SetActive(false);}
        if(gol_atan_topu_resetle) { ball_2.SetActive(false); }
        ball_1.SetActive(false);
        yield return new WaitForSeconds(reset_timer);

        Destroy(instantiated_firework_prefab);

        GoalText_gameobject.SetActive(false);
        if (kaleyi_resetle) { goal.SetActive(true); }
        if (gol_atan_topu_resetle) { ball_2.SetActive(true); ball_2.transform.position = ball_2_SpawnPosition.position; }
        ball_1.SetActive(true);
        ball_1.transform.position = ball_1_SpawnPosition.position;
    }


    public void Ball_2_Goal()
    {
        StartCoroutine(Ball_2_Goal_Coroutine());
    }

    IEnumerator Ball_2_Goal_Coroutine()
    {
        arena_shake_ref.ShakeObject();
        team_2_score++;
        team2_score_text.text = team_2_score.ToString();
        GoalText_gameobject.SetActive(true);

        // Play default goal sound first
        if (default_gol_sesi != null && audioSource != null)
        {
            audioSource.PlayOneShot(default_gol_sesi, default_gol_sesi_yüksekliði);
        }

        // Play goal sound #2 (team 1 scores)
        if (ikinci_takým_gol_sesi != null && audioSource != null)
        {
            audioSource.PlayOneShot(ikinci_takým_gol_sesi, gol_sesi_yüksekliði);
        }

        GameObject instantiated_firework_prefab = Instantiate(firework_prefab, goal_transform.position, Quaternion.identity);

        if (kaleyi_resetle) { goal.SetActive(false); }
        if (gol_atan_topu_resetle) { ball_1.SetActive(false); }
        ball_2.SetActive(false);
        yield return new WaitForSeconds(reset_timer);

        Destroy(instantiated_firework_prefab);

        GoalText_gameobject.SetActive(false);
        if (kaleyi_resetle) { goal.SetActive(true); }
        if (gol_atan_topu_resetle) { ball_1.SetActive(true); ball_1.transform.position = ball_1_SpawnPosition.position; }
        ball_2.SetActive(true);
        ball_2.transform.position = ball_2_SpawnPosition.position;
    }


    void DisplayStartSimulationButton()
    {
        StartSimulationButton.SetActive(true);
    }

    void UndisplayStartSimulationButton()
    {
        StartSimulationButton.SetActive(false);
    }

    private void UpdateTimerDisplay()
    {
        // Format as "90" (no decimals)
        timer.text = Mathf.Floor(currentTime).ToString("0") + "'";
    }

    // === Timer Control Methods ===
    public void StartTimer()
    {
        isRunning = true; // Starts/resumes counting
        
    }

    public void PauseTimer()
    {
        isRunning = false; // Freezes the timer
    }

    public void ResetTimer()
    {
        currentTime = 0f;
        UpdateTimerDisplay(); // Immediately update UI
    }

    public void ResetAndStartTimer()
    {
        ResetTimer();
        StartTimer();
    }
}
