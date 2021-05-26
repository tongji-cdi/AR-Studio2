/// Created by Ferdowsur Asif @ Tiny Giant Studios


using System.Collections;
using UnityEngine;

namespace MText
{
    [CreateAssetMenu(menuName = "Modular 3d Text/Modules/Add Rigidbody")]
    public class MText_Module_AddRigidBody : MText_Module
    {
        [SerializeField] bool enableGravity = false;

        [SerializeField] bool addRandomForce = false;
        [SerializeField] float horizontalForcePower = 1;
        [SerializeField] float verticalForcePower = 1;
        [SerializeField] Vector3 forceDirectionMinimum = Vector3.zero;
        [SerializeField] Vector3 forceDirectionMaximum = new Vector3(360,360,360);

        [SerializeField] PhysicMaterial physicMaterial = null;

        public override IEnumerator ModuleRoutine(GameObject obj, float delay)
        {

            if (obj.GetComponent<MeshFilter>())
            {
                if (obj.GetComponent<Rigidbody>())
                    obj.GetComponent<Rigidbody>().isKinematic = true;

                yield return new WaitForSeconds(delay);

                if (obj)
                {
                    if (!obj.GetComponent<BoxCollider>())
                        obj.AddComponent<BoxCollider>();

                    if (physicMaterial)
                        obj.GetComponent<BoxCollider>().material = physicMaterial;

                    if (!obj.GetComponent<Rigidbody>())
                        obj.AddComponent<Rigidbody>();

                    obj.GetComponent<Rigidbody>().isKinematic = false;
                    obj.GetComponent<Rigidbody>().useGravity = enableGravity;

                    if (addRandomForce)
                        obj.GetComponent<Rigidbody>().AddForce(new Vector3(horizontalForcePower * Random.Range(forceDirectionMinimum.x, forceDirectionMaximum.x), verticalForcePower * Random.Range(forceDirectionMinimum.y, forceDirectionMaximum.y), horizontalForcePower * Random.Range(forceDirectionMinimum.z, forceDirectionMaximum.z)));
                    //obj.GetComponent<Rigidbody>().AddForce(Random.insideUnitCircle.normalized*forcePower);
                }
            }
        }
    }
}

