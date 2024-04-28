namespace EasySite.DTOs.productDTO.ToReturn
{
    public class CrossSellingToReturnDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public ProductToReturnDto ProductToRedirect { get; set; }
        public int ProductIdToRedirect { get; set; }
        public int ProductId { get; set; }

        public int Sale { get; set; }

        public string BottonTextAgree { get; set; }
        public string BottonColorAgree { get; set; }


        public string ButtonTextReject { get; set; }
        public string ButtonColorReject { get; set; }

        public string Titele { get; set; }
        public string Description { get; set; }
    }
}
