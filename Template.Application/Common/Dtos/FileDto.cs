namespace Template.Application.Common.Dtos;

public class FileDto
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public byte[] Bytes { get; set; }
}