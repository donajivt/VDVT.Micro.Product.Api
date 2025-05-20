namespace VDVT.Micro.Product.Api.Models.Dto
{
    public class ResponsDto
    {
        public object Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
