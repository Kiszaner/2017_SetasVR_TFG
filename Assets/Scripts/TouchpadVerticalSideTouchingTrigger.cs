using UnityEngine;

/// Trigger that fires if user is touching the side of the controller.
public class TouchpadVerticalSideTouchingTrigger : BaseActionTrigger
{
    /// Side of the controller.
    public enum Side
    {
        Top,
        Bottom
    }
    /// Side of the controller to trigger on.
    [Tooltip("Side of the controller to trigger on")]
    public Side side;

    private float verticalSideWidth = .2f;

    public override bool TriggerActive()
    {
        if (GvrControllerInput.IsTouching == false)
        {
            return false;
        }

        float yPos = GvrControllerInput.TouchPos.y;

        if (yPos <= verticalSideWidth)
        {
            return side == Side.Top;
        }

        // Check for right side active.
        if (yPos >= (1 - verticalSideWidth))
        {
            return side == Side.Bottom;
        }

        return false;
    }
}
