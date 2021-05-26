using UnityEngine;
using System.Collections;

namespace MText
{
    [CreateAssetMenu(menuName = "Modular 3d Text/Modules/Play Particle")]

    public class MText_Module_PlayParticles : MText_Module
    {
        [SerializeField] GameObject particlePrefab = null;
        [SerializeField] int destroyParticleAfter = 6;

        public override IEnumerator ModuleRoutine(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (obj)
            {
                if (particlePrefab)
                {
                    GameObject spawnedParticle = Instantiate(particlePrefab);
                    spawnedParticle.transform.position = obj.transform.position;
                    spawnedParticle.transform.rotation = obj.transform.rotation;

                    if(spawnedParticle.GetComponent<ParticleSystem>())
                        spawnedParticle.GetComponent<ParticleSystem>().Play();

                    yield return new WaitForSeconds(destroyParticleAfter);
                    if (spawnedParticle)
                        Destroy(spawnedParticle);
                }
            }
        }
    }
}