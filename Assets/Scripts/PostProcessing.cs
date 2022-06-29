using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour
{

    public PlayerScript player;

    public PostProcessVolume poprovol;
    Vignette pVignette;
    Grain pGrain;
    ColorGrading pColorGrading;
    LensDistortion pLensDistortion;
    ChromaticAberration pChromaticAberration;

    // Start is called before the first frame update
    void Start()
    {
        pVignette = ScriptableObject.CreateInstance<Vignette>();
        pVignette.enabled.Override(true);
        pVignette.intensity.Override(0f);
        pVignette.smoothness.Override(1f);
        poprovol = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, pVignette);

        pGrain = ScriptableObject.CreateInstance<Grain>();
        pGrain.enabled.Override(true);
        pGrain.intensity.Override(0.3f);
        poprovol = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, pGrain);

        pColorGrading = ScriptableObject.CreateInstance<ColorGrading>();
        pColorGrading.enabled.Override(true);
        poprovol = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, pColorGrading);

        pLensDistortion = ScriptableObject.CreateInstance<LensDistortion>();
        pLensDistortion.enabled.Override(true);
        poprovol = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, pLensDistortion);

        pChromaticAberration = ScriptableObject.CreateInstance<ChromaticAberration>();
        pChromaticAberration.enabled.Override(true);
        poprovol = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, pChromaticAberration);
    }

    // Update is called once per frame
    void Update()
    {
        if(player.health < player.maxHealth / 2f){
            pVignette.intensity.Override((player.maxHealth / 2f - player.health) / (player.maxHealth / 2));
        }
        //pChromaticAberration.intensity.Override(1f - 1f / (player.fightIntensity + 1f));
        //pColorGrading.lift.red.Override(100 + Mathf.Sin(DateTime.Now));
        //pColorGrading.lift.green.Override(0);
        //pColorGrading.lift.blue.Override(0);
    }
}
