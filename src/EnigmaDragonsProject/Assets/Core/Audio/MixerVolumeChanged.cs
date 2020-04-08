
public sealed class MixerVolumeChanged
{
    public string ChannelName { get; }

    public MixerVolumeChanged(string name) => ChannelName = name;
}
