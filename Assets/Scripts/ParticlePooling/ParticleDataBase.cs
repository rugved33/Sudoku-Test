using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ParticleDatabase", menuName = "ParticleDatabase")]
public class ParticleDataBase :ScriptableObject
{
    public List<PoolObject> Particles;
}
