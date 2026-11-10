using UnityEngine;

/// <summary>
/// Проигрывает звук удара при столкновении предмета с чем угодно.
/// Громкость зависит от силы удара: скорость сближения в момент контакта
/// раскладывается по шкале minImpactSpeed..maxImpactSpeed и превращается
/// в громкость minVolume..maxVolume.
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class ImpactSound : MonoBehaviour
{
    [Header("Варианты звука для этого материала")]
    public AudioClip[] impactClips;

    [Header("Сила удара, м/с: ниже — тишина, выше — максимум")]
    public float minImpactSpeed = 0.6f;
    public float maxImpactSpeed = 7f;

    [Header("Громкость на границах этого диапазона")]
    [Range(0f, 1f)] public float minVolume = 0.15f;
    [Range(0f, 1f)] public float maxVolume = 1f;

    [Header("Минимальный интервал между звуками, с")]
    public float cooldown = 0.08f;

    AudioSource audioSource;
    float lastPlayTime = -10f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (impactClips == null || impactClips.Length == 0)
            return;

        // защита от треска при долгом контакте (качение, лежание на полу)
        if (Time.time - lastPlayTime < cooldown)
            return;

        float impactSpeed = collision.relativeVelocity.magnitude;
        if (impactSpeed < minImpactSpeed)
            return;

        lastPlayTime = Time.time;

        // 0 — еле стукнули, 1 — сильный удар
        float strength = Mathf.InverseLerp(minImpactSpeed, maxImpactSpeed, impactSpeed);
        float volume = Mathf.Lerp(minVolume, maxVolume, strength);

        // лёгкая случайная высота тона, чтобы повторы не звучали одинаково
        audioSource.pitch = Random.Range(0.93f, 1.07f);
        audioSource.PlayOneShot(impactClips[Random.Range(0, impactClips.Length)], volume);
    }
}
