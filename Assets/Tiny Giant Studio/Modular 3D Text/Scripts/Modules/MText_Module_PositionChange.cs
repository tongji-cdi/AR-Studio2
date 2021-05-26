/// <summary>
/// This module puts a single frame delay to texts
/// Circular texts reposition characters after they are created instead of positing them correctly when spawning like every other layout
/// So, putting a single frame delay for that to happen first and not mess up positioning
/// I acknowledge this is a band-aid fix. Will look into this later. 
/// 
/// Requires reposition old chars disabled to work correctly
/// 
/// - Ferdowsur Asif
/// </summary>

using System.Collections;
using UnityEngine;

namespace MText
{
    [CreateAssetMenu(menuName = "Modular 3d Text/Modules/Change Position")]
    public class MText_Module_PositionChange : MText_Module
    {
        [Header("This Module always put a single frame delay")]
        [Space]
        [SerializeField] float delayBeforeStarting = 0;
        [SerializeField] bool useLocalPosition = false;
        [Space]
        [Space]


        [Tooltip("If start From default is set to false 'Start From' Value will be used")]
        [SerializeField] bool startFromDefaultPosition = true;
        [Tooltip("If Grow From default is also set to false Grow From Value will be used")]
        [SerializeField] Vector3 startFrom = Vector3.zero;

        [Space]
        [Space]
        [Tooltip("Move to will be used only if Move To Original is false")]
        [SerializeField] bool moveToOriginal = false;
        [Tooltip("If set to true, move to will be used to add position instead of literally moving to new position")]
        [SerializeField] bool addMoveToValue = false;
        [Tooltip("Move to will be used only if Move To Original is false")]
        [SerializeField] Vector3 moveTo = Vector3.zero;
        [Space]
        [Space]
        [Header("Don't forget to assign a animation curve")]
        [SerializeField] AnimationCurve animationCurve = null;

        public override IEnumerator ModuleRoutine(GameObject obj, float duration)
        {
            obj.SetActive(false);
            yield return null;
            if (obj)
            {
                Transform tr = obj.transform;

                Vector3 startPosition = startFrom;
                if (startFromDefaultPosition)
                {
                    if (useLocalPosition)
                        startPosition = tr.localPosition;
                    else
                        startPosition = tr.position;
                }

                Vector3 targetPosition = moveTo;
                if (moveToOriginal)
                {
                    if (useLocalPosition)
                        targetPosition = tr.localPosition;
                    else
                        targetPosition = tr.position;
                }
                else if (addMoveToValue)
                {
                    if (useLocalPosition)
                        targetPosition += tr.localPosition;
                    else
                        targetPosition += tr.position;
                }

                yield return new WaitForSeconds(delayBeforeStarting);
                obj.SetActive(true);

                float timer = 0;
                while (timer < duration)
                {
                    if (!tr)
                        break;

                    float perc = timer / duration;
                    if (useLocalPosition)
                        tr.localPosition = Vector3.Lerp(startPosition, targetPosition, animationCurve.Evaluate(perc));
                    else
                        tr.position = Vector3.Lerp(startPosition, targetPosition, animationCurve.Evaluate(perc));
                    timer += Time.deltaTime;

                    yield return null;
                }

                if (tr)
                {
                    if (useLocalPosition)
                        tr.localPosition = targetPosition;
                    else
                        tr.position = targetPosition;
                }
            }
        }
    }
}