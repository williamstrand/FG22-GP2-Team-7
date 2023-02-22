using UnityEngine;

[CreateAssetMenu(fileName = "SoundHolder", menuName = "ScriptableObjects/SoundHolder")]
public class SoundHolder : ScriptableObject
{
    [field: Header("Landie")]
    [field: SerializeField] public AudioClip SandFootsteps { get; private set; }
    [field: SerializeField] public AudioClip RockFootsteps { get; private set; }
    [field: SerializeField] public AudioClip Climbing { get; private set; }
    [field: SerializeField] public AudioClip LandieJump { get; private set; }

    [field: Header("Swimmie")]
    [field: SerializeField] public AudioClip Swimming { get; private set; }
    [field: SerializeField] public AudioClip WaterSquirting { get; private set; }
    [field: SerializeField] public AudioClip SwimmieJump { get; private set; }

    [field: Header("General")]
    [field: SerializeField] public AudioClip OceanLoop { get; private set; }
    [field: SerializeField] public AudioClip SeagullLoop { get; private set; }
    [field: SerializeField] public AudioClip BgmIntro { get; private set; }
    [field: SerializeField] public AudioClip BgmLoop { get; private set; }
    [field: SerializeField] public AudioClip CatapultFire { get; private set; }
    [field: SerializeField] public AudioClip CatapultLoad { get; private set; }


}
