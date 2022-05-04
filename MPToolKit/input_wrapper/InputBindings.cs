using System.Collections.Generic;

public static class InputBindings
{
    public const string UP = "up";
    public const string DOWN = "down";
    public const string LEFT = "left";
    public const string RIGHT = "right";
    public const string JUMP = "jump";
    public const string FULLSCREEN = "fullscreen";

    public static Dictionary<Buttons, string> Bindings = new Dictionary<Buttons, string>
    {
        { Buttons.MOVE_FORWARD, UP},
        { Buttons.MOVE_BACK, DOWN},
        { Buttons.MOVE_RIGHT, RIGHT},
        { Buttons.MOVE_LEFT, LEFT},
        { Buttons.JUMP, JUMP},
    };
}