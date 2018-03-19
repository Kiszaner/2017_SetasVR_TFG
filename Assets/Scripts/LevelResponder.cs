using DaydreamElements.Main;
using DaydreamElements.ObjectManipulation;
using DaydreamElements.Teleport;
using UnityEngine.Assertions;

public class LevelResponder : BaseLevelSelectResponder
{
    private GvrTrackedController controller;
    private TeleportController teleport;
    private ObjectManipulationPointer pointer;

    public override void OnMenuOpened()
    {
        SetTeleportModeEnabled(false);
        SetObjectManipulationEnabled(false);
    }

    public override void OnMenuClosed()
    {
        SetTeleportModeEnabled(true);
        SetObjectManipulationEnabled(true);
    }

    void Start()
    {
        controller = SceneHelpers.FindObjectOfType<GvrTrackedController>(true);
        Assert.IsNotNull(controller);
        teleport = SceneHelpers.FindObjectOfType<TeleportController>(true);
        Assert.IsNotNull(teleport);
        pointer = controller.GetComponentInChildren<ObjectManipulationPointer>(true);
        Assert.IsNotNull(pointer);
    }

    void Update()
    {
        if (teleport == null)
        {
            return;
        }

        if (LevelSelectController.Instance == null)
        {
            return;
        }

        bool isTeleporting = teleport.IsSelectingTeleportLocation || teleport.IsTeleporting;
        LevelSelectController.Instance.enabled = !isTeleporting;
    }

    private void SetTeleportModeEnabled(bool enabled)
    {
        controller.gameObject.SetActive(enabled);

        if (teleport != null)
        {
            teleport.gameObject.SetActive(enabled);
        }
    }

    private void SetObjectManipulationEnabled(bool enabled)
    {
        controller.gameObject.SetActive(enabled);
        if (enabled)
        {
            GvrPointerInputModule.Pointer = pointer;
        }
    }
}