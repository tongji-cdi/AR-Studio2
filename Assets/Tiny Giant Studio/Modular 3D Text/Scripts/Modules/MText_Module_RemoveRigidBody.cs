using System.Collections;
using UnityEngine;

namespace MText
{
    [CreateAssetMenu(menuName = "Modular 3d Text/Modules/Remove Rigidbody")]
    public class MText_Module_RemoveRigidBody : MText_Module
    {
        public override IEnumerator ModuleRoutine(GameObject obj, float delay)
        {
            if (obj)
            {
                if (obj.GetComponent<BoxCollider>())
                {
                    Destroy(obj.GetComponent<Rigidbody>());
                }
                if (obj.GetComponent<Rigidbody>())
                {
                    Destroy(obj.GetComponent<Rigidbody>());
                }
            }
            yield return null;
        }
    }
}
