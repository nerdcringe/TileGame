
using UnityEngine;
using UnityEngine.UI;
public class ParticleAnimator : MonoBehaviour
{
    public Toggle snowToggle;

    ParticleSystem pSystem;
    int defMaxParticles;

    public void Start()
    {
        pSystem = GetComponent<ParticleSystem>();
        defMaxParticles = pSystem.maxParticles;
    }


    public void Update()
    {
        bool doParticles = true;

        if (snowToggle != null)
        {
            doParticles = snowToggle.isOn;
        }

        if (doParticles)
        {
            pSystem.maxParticles = defMaxParticles;
        }
        else
        {
            pSystem.maxParticles = 0;
        }

        if (!pSystem.IsAlive() && !pSystem.main.loop)
        {
            Destroy(gameObject);
        }
    }

}