namespace Kirara.Network
{
    public interface IRsp : IMsg
    {
        IResult Result { get; }
    }
}