namespace ArdRehber.Dtos
{
    public class TitleDto
    {
        public int Id { get; set; }

        public string TitleName { get; set; }

        public int Order { get; set; }

        public List<int> DepartmentIdlist { get; set; }
    }
}
