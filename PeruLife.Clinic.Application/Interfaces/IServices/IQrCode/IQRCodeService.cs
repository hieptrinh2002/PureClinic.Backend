namespace PureLifeClinic.Application.Interfaces.IServices.IQrCode
{
    public interface IQRCodeService
    {
        byte[] GenerateQRCode(string text);
    }
}
