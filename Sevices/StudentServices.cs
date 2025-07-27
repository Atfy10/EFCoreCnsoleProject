using EjadaEFProject.Data;
using EjadaEFProject.Interfaces;
using EjadaEFProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EjadaEFProject.Sevices
{
    internal class StudentServices : EntityServices<Student>
    {
        readonly IBaseRepository<Student> _studRepository;
        public StudentServices()
        {
            _studRepository = new BaseRepository<Student>(new EduDbContext());
        }
        public override async Task<List<Student>> GetAll()
        {
            return await _studRepository.GetAllAsync();
        }
        public override async Task<Result<Student>> GetById(int id)
            => await ExecuteOperation(async () =>
            {
                var student = _studRepository.GetById(id);
                if (student is null)
                {
                    return new Result<Student>
                    {
                        IsSuccess = false,
                        Message = "Invalid ID. Please provide a valid ID greater than 0.",
                        Data = new Student()
                        {
                            Id = id
                        }
                    };
                }
                return new Result<Student>
                {
                    IsSuccess = true,
                    Message = $"Student with ID {id} retrieved successfully.",
                    Data = student
                };
            });
        public override async Task<Result<Student>> Add(Student student)
            => await ExecuteOperation(async () =>
            {
                //  Validate the student data
                if (!IsValidStudent(student))
                {
                    return new Result<Student>
                    {
                        IsSuccess = false,
                        Message = "Invalid student data. Please provide a valid student.",
                        Data = student
                    };

                }

                // Add the student to the db
                await _studRepository.AddAsync(student);

                return new Result<Student>
                {
                    IsSuccess = true,
                    Message = "Student added successfully.",
                    Data = student
                };
            });
        public override async Task<Result<Student>> Update(Student newStudent)
            => await ExecuteOperation(async () =>
            {
                //  Validate the new student data
                if (!IsValidStudent(newStudent))
                {
                    return new Result<Student>
                    {
                        IsSuccess = false,
                        Message = "Invalid student data. Please provide a valid student.",
                        Data = newStudent
                    };
                }

                //  Check if the student exists in the db
                var existingStudent = _studRepository.GetById(newStudent.Id);
                if (existingStudent is null)
                {
                    return new Result<Student>
                    {
                        IsSuccess = false,
                        Message = $"No student found with ID {newStudent.Id}.",
                        Data = newStudent
                    };
                }

                //  Update the existing student with new data
                existingStudent.Name = newStudent.Name;
                existingStudent.Age = newStudent.Age;

                await _studRepository.UpdateAsync(existingStudent);

                return new Result<Student>
                {
                    IsSuccess = true,
                    Message = $"Student with ID {newStudent.Id} updated successfully.",
                    Data = existingStudent
                };
            });
        public override async Task<Result<Student>> Delete(int id)
            => await ExecuteOperation(async () =>
            {
                //  Check if the student exists in the db
                var student = _studRepository.GetById(id);
                if (student is null)
                {
                    return new Result<Student>
                    {
                        IsSuccess = false,
                        Message = "Invalid ID. Please provide a valid ID greater than 0.",
                        Data = new Student()
                        {
                            Id = id
                        }
                    };
                }

                //  Delete the student from the db
                await _studRepository.DeleteAsync(student);

                return new Result<Student>
                {
                    IsSuccess = true,
                    Message = $"Student with ID {id} deleted successfully.",
                    Data = student
                };
            });


        private static bool IsValidStudent(Student student)
            => student != null
            && IsValidStudentName(student.Name)
            && IsValidStudentAge(student.Age)
            && IsValidDepartmentNumber(student.DeptId);
        //&& IsValidStudentId(student.Id);
        private static bool IsValidStudentName(string name)
            => !string.IsNullOrWhiteSpace(name)
            && name.Length >= 3
            && name.Length <= 50
            && name == name?.Trim();
        private static bool IsValidStudentAge(int age)
            => age >= 9
            && age <= 26;
        private static bool IsValidStudentId(int id)
            => id > 0;
        private static bool IsValidDepartmentNumber(int deptId)
        {
            using var context = new EduDbContext();
            return context.Departments.Any(d => d.Did == deptId);
        }

    }
}
