
public static class EnigmaDragonsUI
{
    public static void ToggleUITarget(string targetName) => Message.Publish(new ToggleNamedTarget(targetName));
}
