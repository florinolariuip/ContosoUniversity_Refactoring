namespace Application.Students.Queries.GetStudentsOverview
{
    public class StudentsOverviewVM
    {
        public IList<StudentVM> Students { get; }

        public string CurrentSort { get; set; }

        public string NameSortParm { get; set; }

        public string DateSortParm { get; set; }

        public string CurrentFilter { get; set; }

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }

        public StudentsOverviewVM()
        {
            Students = new List<StudentVM>();
            PageNumber = 1;
        }

        public StudentsOverviewVM(IList<StudentVM> students)
        {
            if (students != null)
                Students = students;
            else
                Students = new List<StudentVM>();

            PageNumber = 1;
        }

        public void AddStudents(List<StudentVM> students)
        {
            foreach (var student in students)
            {
                Students.Add(student);
            }
        }
    }
}
