using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NINESOFT.TUTORIAL_SYSTEM
{

    public class TutorialModule : MonoBehaviour
    {

        protected bool IsActive;

      
        protected void OnEnable()
        {
            IsActive = true;
            ResetProperties();
            ActiveTheModule();
        }
        protected void OnDisable()
        {
            IsActive = false;
        }

        protected virtual void ResetProperties()
        {
            
        }

        public void ActiveTheModule() => StartCoroutine(ActiveTheModuleEnum());

        public virtual IEnumerator ActiveTheModuleEnum()
        {
            yield return new WaitForSeconds(0.01f);
        }

    }
}