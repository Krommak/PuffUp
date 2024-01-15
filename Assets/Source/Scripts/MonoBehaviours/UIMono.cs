using Game.Data;
using Game.Signals;
using Game.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMono : MonoBehaviour
{
    public TMP_Text GoldCounter;
    public TMP_Text MovesCounter;
    public Button SoundButton;

    public Image RewardBackground;
    public TMP_Text RewardText;

    public RectTransform WinPanel;
    public RectTransform LosePanel;
    public Renderer RewardPanel;

    public RectTransform[] AnimatedElementsTransform;

    public void Start()
    {
        TriggerListenerSystem.Trigger(new UILoaded()
        {
            UIMono = this
        });
    }

    public void InitWinLosePanels(RuntimeData runtimeData, StaticData staticData)
    {

    }
}
