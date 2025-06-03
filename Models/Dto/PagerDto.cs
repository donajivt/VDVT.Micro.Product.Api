namespace VDVT.Micro.Product.Api.Models.Dto
{
    public record PagerDto(int Page = 1, int RecordsPerPage = 10)
    {
        private const int MaxRecordPerPage = 50;
        public int Page { get; set; } = Math.Max(1,Page);
        /// <sumary>
        /// clamp me permite identificar un valor entre 1 y el valor maximo por pagina
        /// </sumary>
        public int RecordsPerPage { get; set; } = Math.Clamp(RecordsPerPage, 1, MaxRecordPerPage);
    }
}
