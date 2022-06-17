namespace $ext_safeprojectname$.Application.Common.Dtos;

public class ResponseDto<T>
{
    public string ErrorMessage { get; set; }
    public T Value { get; set; }
}