namespace ProjectService.API.Domain.Contracts
{
    public class TagesDtos
    {
        public class TagResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class CreateTagRequest
        {
            public string Name { get; set; }
        }

        public class UpdateTagRequest
        {
            public string Name { get; set; }
        }



    }
}
