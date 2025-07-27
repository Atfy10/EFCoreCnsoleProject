using EjadaEFProject.Data;
using EjadaEFProject.Interfaces;
using EjadaEFProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjadaEFProject.Sevices
{
    internal class StdCrsServices : EntityServices<StdCr>
    {
        private readonly IBaseRepository<StdCr> _stdCrrepository;
        public StdCrsServices()
        {
            _stdCrrepository = new BaseRepository<StdCr>(new EduDbContext());
        }

        public override async Task<List<StdCr>> GetAll()
        {
            return await _stdCrrepository.GetAllAsync();
        }
        public override async Task<Result<StdCr>> GetById(int[] ids)
            => await ExecuteOperation(async () =>
            {
                if (ids.Length != 2)
                {
                    throw new Exception("Invalid IDs. Please provide exactly two elements: [studentId, courseId].");
                }
                var stdCrs = _stdCrrepository.GetById(ids);
                if (stdCrs is null)
                {
                    return new Result<StdCr>
                    {
                        IsSuccess = false,
                        Message = "Invalid ID. Please provide a valid ID greater than 0.",
                        Data = new StdCr
                        {
                            StudentId = ids[0],
                            CourseId = ids[1]
                        }
                    };
                }
                return new Result<StdCr>
                {
                    IsSuccess = true,
                    Message = $"Student-Course relationship with ID {ids[0]}, {ids[1]} retrieved successfully.",
                    Data = stdCrs
                };
            });
        public override async Task<Result<StdCr>> Add(StdCr stdCr)
            => await ExecuteOperation(async () =>
            {
                if (!IsValidStdCrs(stdCr))
                {
                    return new Result<StdCr>
                    {
                        IsSuccess = false,
                        Message = "One from Student/Course ID or both not found.",
                        Data = stdCr
                    };
                }

                await _stdCrrepository.AddAsync(stdCr);

                return new Result<StdCr>
                {
                    IsSuccess = true,
                    Message = "Student-Course relationship added successfully.",
                    Data = stdCr
                };
            });
        public override async Task<Result<StdCr>> Update(StdCr stdCr)
            => await ExecuteOperation(async () =>
            {
                if(!IsValidStdCrs(stdCr))
                {
                    return new Result<StdCr>
                    {
                        IsSuccess = false,
                        Message = "One from Student/Course ID or both not found.",
                        Data = stdCr
                    };
                }

                var existingStdCrs = _stdCrrepository.GetById( stdCr.StudentId, stdCr.CourseId);
                if (existingStdCrs is null)
                {
                    return new Result<StdCr>
                    {
                        IsSuccess = false,
                        Message = $"No student-course relationship found with " +
                        $"Student ID {stdCr.StudentId} and Course ID {stdCr.CourseId}.",
                        Data = stdCr
                    };
                }

                existingStdCrs.StudentId = stdCr.StudentId;
                existingStdCrs.CourseId = stdCr.CourseId ;
                existingStdCrs.Grade = stdCr.Grade;

                await _stdCrrepository.UpdateAsync(existingStdCrs);
                return new Result<StdCr>
                {
                    IsSuccess = true,
                    Message = $"Student-Course relationship with Student ID {stdCr.StudentId} and " +
                    $"Course ID {stdCr.CourseId} updated successfully.",
                    Data = existingStdCrs
                };
            });
        public override async Task<Result<StdCr>> Delete(int[] ids)
            => await ExecuteOperation(async () =>
            {
                if (ids.Length != 2)
                {
                    throw new Exception("Invalid IDs. Please provide exactly two elements: [studentId, courseId].");
                }

                var stdCrs = _stdCrrepository.GetById(ids);
                if (stdCrs is null)
                {
                    return new Result<StdCr>
                    {
                        IsSuccess = false,
                        Message = "Invalid IDs. Please provide a valid IDs.",
                        Data = new StdCr
                        {
                            StudentId = ids[0],
                            CourseId = ids[1]
                        }
                    };
                }

                await _stdCrrepository.DeleteAsync(stdCrs);
                return new Result<StdCr>
                {
                    IsSuccess = true,
                    Message = $"Student-Course relationship with Student ID {ids[0]} and " +
                    $"Course ID {ids[1]} deleted successfully.",
                    Data = stdCrs
                };
            });


        private bool IsValidStdCrs(StdCr stdCr)
        {
            using var context = new EduDbContext();
            return stdCr.Grade >= 0 
                && stdCr.Grade <= 100
                && context.Students.Any(s => s.Id == stdCr.StudentId) 
                && context.Courses.Any(c => c.CourseId == stdCr.CourseId);
        }
    }
}
