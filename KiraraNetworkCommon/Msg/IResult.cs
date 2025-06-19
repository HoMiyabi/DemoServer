namespace Kirara.Network
{
    public interface IResult
    {
        int Code { get; }
        string Msg { get; }
    }
}