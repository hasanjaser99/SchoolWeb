
using SchoolWeb.Data;

namespace SchoolWeb.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Student = new StudentRepository(db);
            Teacher = new TeacherRepository(db);
            Course = new CourseRepository(db);
            CourseTeachers = new CourseTeachersRepository(db);
            Mark = new MarkRepository(db);
            Class = new ClassRepository(db);
            Section = new SectionRepository(db);
            StudentFee = new StudentFeeRepository(db);
            SchoolFee = new SchoolFeeRepository(db);
            MonthlyPayment = new MonthlyPaymentRepository(db);
            News = new NewsRepository(db);
            NewsImages = new NewsImagesRepository(db);
            Activity = new ActivityRepository(db);
            ActivityImages = new ActivitiyImagesRepository(db);
            SuggestionComplain = new SuggestionComplainRepository(db);


        }
        public IStudentRepository Student { get; private set; }

        public ITeacherRepository Teacher { get; private set; }

        public ICourseRepository Course { get; private set; }

        public ICourseTeachersRepository CourseTeachers { get; private set; }

        public IMarkRepository Mark { get; private set; }

        public IClassRepository Class { get; private set; }

        public ISectionRepository Section { get; private set; }

        public IStudentFeeRepository StudentFee { get; private set; }

        public ISchoolFeeRepository SchoolFee { get; private set; }

        public IMonthlyPaymentRepository MonthlyPayment { get; private set; }

        public INewsRepository News { get; private set; }

        public INewsImagesRepository NewsImages { get; private set; }

        public IActivityRepository Activity { get; private set; }

        public IActivityImagesRepository ActivityImages { get; private set; }

        public ISuggestionComplainRepository SuggestionComplain { get; private set; }


        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
