using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class OptionsUI : MonoBehaviour
{
    private SoundManager soundManager;
    private UIDocument uIDocument;
    private Slider master;
    private Slider music;
    private Slider sound_fx;
    private Slider environment;
    private Slider ui;

    void OnEnable()
    {
        soundManager = SoundManager.instance;
        uIDocument = GetComponent<UIDocument>();
        master = uIDocument.rootVisualElement.Q<Slider>("Master");
        music = uIDocument.rootVisualElement.Q<Slider>("Music");
        sound_fx = uIDocument.rootVisualElement.Q<Slider>("SoundFx");
        environment = uIDocument.rootVisualElement.Q<Slider>("Environment");
        ui = uIDocument.rootVisualElement.Q<Slider>("UI");

        master.RegisterValueChangedCallback(x => soundManager.SetVolume(SoundManager.SoundType.MASTER, x.newValue));
        music.RegisterValueChangedCallback(x => soundManager.SetVolume(SoundManager.SoundType.MUSIC, x.newValue));
        sound_fx.RegisterValueChangedCallback(x => soundManager.SetVolume(SoundManager.SoundType.SFX, x.newValue));
        environment.RegisterValueChangedCallback(x => soundManager.SetVolume(SoundManager.SoundType.ENVIRONMENT, x.newValue));
        ui.RegisterValueChangedCallback(x => soundManager.SetVolume(SoundManager.SoundType.UI, x.newValue));
    }
}
