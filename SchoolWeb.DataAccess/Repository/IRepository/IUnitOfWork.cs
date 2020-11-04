using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Student { get; }

        ITeacherRepository Teacher { get; }

        ICourseRepository Course { get; }

        ICourseTeachersRepository CourseTeachers { get; }

        IMarkRepository Mark { get; }

        IClassRepository Class { get; }

        ISectionRepository Section { get; }

        IStudentFeeRepository StudentFee { get; }

        ISchoolFeeRepository SchoolFee { get; }

        IMonthlyPaymentRepository MonthlyPayment { get; }

        INewsRepository News { get; }

        INewsImagesRepository NewsImages { get; }

        IActivityRepository Activity { get; }

        IActivityImagesRepository ActivityImages { get; }

        ISuggestionComplainRepository SuggestionComplain { get; }


        void Save();

    }
}
