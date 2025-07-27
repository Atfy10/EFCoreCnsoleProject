using EjadaEFProject.Data;
using EjadaEFProject.Interfaces;
using EjadaEFProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjadaEFProject.Sevices
{
    internal class CourseServices : EntityServices<Course>
    {
        readonly IBaseRepository<Course> _crsRepository;
        public CourseServices()
        {
            _crsRepository = new BaseRepository<Course>(new EduDbContext());
        }
        public override async Task<List<Course>> GetAll()
        {
            return await _crsRepository.GetAllAsync();
        }
        public override async Task<Result<Course>> GetById(int id)
            => await ExecuteOperation(async () =>
            {
                var course = _crsRepository.GetById(id);
                if (course is null)
                {
                    return new Result<Course>
                    {
                        IsSuccess = false,
                        Message = "Invalid ID. Please provide a valid ID greater than 0.",
                        Data = new Course()
                        {
                            CourseId = id
                        }
                    };
                }
                return new Result<Course>
                {
                    IsSuccess = true,
                    Message = $"Course with ID {id} retrieved successfully.",
                    Data = course
                };
            });
        public override async Task<Result<Course>> Add(Course course)
            => await ExecuteOperation(async () =>
            {
                // Validate the course data
                if (!IsValidCourse(course))
                {
                    return new Result<Course>
                    {
                        IsSuccess = false,
                        Message = "Invalid course data. Please provide a valid course.",
                        Data = course
                    };
                }

                await _crsRepository.AddAsync(course);
                return new Result<Course>
                {
                    IsSuccess = true,
                    Message = "Course added successfully.",
                    Data = course
                };
            });
        public override async Task<Result<Course>> Update(Course newCourse)
            => await ExecuteOperation(async () =>
            {
                if (!IsValidCourse(newCourse))
                {
                    return new Result<Course>
                    {
                        IsSuccess = false,
                        Message = "Invalid course data. Please provide a valid course.",
                        Data = newCourse
                    };
                }

                var course = _crsRepository.GetById(newCourse.CourseId);
                if (course is null)
                {
                    return new Result<Course>
                    {
                        IsSuccess = false,
                        Message = $"No course found with ID {newCourse.CourseId}.",
                        Data = newCourse
                    };
                }

                course.CourseName = newCourse.CourseName == ""
                ? course.CourseName : newCourse.CourseName;
                await _crsRepository.UpdateAsync(course);

                return new Result<Course>
                {
                    IsSuccess = true,
                    Message = $"Course with ID {newCourse.CourseId} updated successfully.",
                    Data = course
                };

            });
        public override async Task<Result<Course>> Delete(int id)
            => await ExecuteOperation(async () =>
            {
                var course = _crsRepository.GetById(id);
                if (course is null)
                {
                    return new Result<Course>
                    {
                        IsSuccess = false,
                        Message = "Invalid ID. Please provide a valid ID.",
                        Data = new Course()
                        {
                            CourseId = id
                        }
                    };
                }

                await _crsRepository.DeleteAsync(course);
                return new Result<Course>
                {
                    IsSuccess = true,
                    Message = $"Course with ID {id} deleted successfully.",
                    Data = course
                };
            });


        private bool IsValidCourse(Course course)
            => IsValidName(course.CourseName);

        private bool IsValidName(string courseName)
            => !string.IsNullOrWhiteSpace(courseName)
            && courseName.Length >= 3;
    }
}
