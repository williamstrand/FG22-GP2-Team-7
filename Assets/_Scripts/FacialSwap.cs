using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialSwap : MonoBehaviour
{
    // Written by a game artist, don't blame our programmers for my poor tomfoolery
    // Also yes, for some reason Unity don't like material changes unless you insert a whole array.
    // For future projects I'll look into blendshapes on UV maps, that's supposedly a thing.
    [SerializeField] private List<Material> faces;
    [SerializeField] private SkinnedMeshRenderer target;
    [SerializeField] private List<Material> originalMaterials;

    public void Mischevious()
    {
        Material[] materials = new Material[target.sharedMaterials.Length];
        materials[0] = faces[0];
        materials[1] = originalMaterials[1];
        target.sharedMaterials = materials;
    }
    public void Gape()
    {
        Material[] materials = new Material[target.sharedMaterials.Length];
        materials[0] = faces[1];
        materials[1] = originalMaterials[1];
        target.sharedMaterials = materials;
    }
    public void Shock()
    {
        Material[] materials = new Material[target.sharedMaterials.Length];
        materials[0] = faces[2];
        materials[1] = originalMaterials[1];
        target.sharedMaterials = materials;
    }
    public void Smile()
    {
        Material[] materials = new Material[target.sharedMaterials.Length];
        materials[0] = faces[3];
        materials[1] = originalMaterials[1];
        target.sharedMaterials = materials;
    }
    public void Spit()
    {
        Material[] materials = new Material[target.sharedMaterials.Length];
        materials[0] = faces[4];
        materials[1] = originalMaterials[1];
        target.sharedMaterials = materials;
    }

}


