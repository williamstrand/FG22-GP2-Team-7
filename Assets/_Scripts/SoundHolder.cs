using UnityEngine;

[CreateAssetMenu(fileName = "SoundHolder", menuName = "ScriptableObjects/SoundHolder")]
public class SoundHolder : ScriptableObject
{
    // Landie
    [field: SerializeField] public AudioClip SandFootsteps { get; private set; }
    [field: SerializeField] public AudioClip RockFootsteps { get; private set; }
    [field: SerializeField] public AudioClip Climbing { get; private set; }

    // Swimmie
    [field: SerializeField] public AudioClip Swimming { get; private set; }
    [field: SerializeField] public AudioClip WaterSquirting { get; private set; }


    // General
    [field: SerializeField] public AudioClip OceanLoop { get; private set; }
    [field: SerializeField] public AudioClip SeagullLoop { get; private set; }
    [field: SerializeField] public AudioClip BgmIntro { get; private set; }
    [field: SerializeField] public AudioClip BgmLoop { get; private set; }


}
