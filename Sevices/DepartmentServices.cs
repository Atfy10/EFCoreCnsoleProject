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
    internal class DepartmentServices : EntityServices<Department>, IEntityServices<Department>
    {
        readonly IBaseRepository<Department> _deptRepository;
        public DepartmentServices()
        {
            _deptRepository = new BaseRepository<Department>(new EduDbContext());
        }

        public override async Task<List<Department>> GetAll()
        {
            return await _deptRepository.GetAllAsync();
        }
        public override async Task<Result<Department>> Add(Department department)
            => await ExecuteOperation(async () =>
            {
                if (!IsValidDepartment(department))
                {
                    return new Result<Department>
                    {
                        IsSuccess = false,
                        Message = "Invalid department data. Please provide a valid department.",
                        Data = department
                    };
                }
                await _deptRepository.AddAsync(department);

                return new Result<Department>
                {
                    IsSuccess = true,
                    Message = "Department added successfully.",
                    Data = department
                };
            });
        public override async Task<Result<Department>> Update(Department newDepartment)
            => await ExecuteOperation(async () =>
            {
                if (!IsValidDepartment(newDepartment))
                {
                    return new Result<Department>
                    {
                        IsSuccess = false,
                        Message = "Invalid department data. Please provide a valid department.",
                        Data = newDepartment
                    };
                }
                var dept = _deptRepository.GetById(newDepartment.Did);
                if (dept is null)
                {
                    return new Result<Department>
                    {
                        IsSuccess = false,
                        Message = "Invalid ID. Please provide a valid ID.",
                        Data = new Department()
                        {
                            Did = newDepartment.Did
                        }
                    };
                }
                dept.Dname = newDepartment.Dname == "" ?
                dept.Dname : newDepartment.Dname;

                dept.Daddress = newDepartment.Daddress == "" ?
                dept.Daddress : newDepartment.Daddress;

                await _deptRepository.UpdateAsync(dept);
                return new Result<Department>
                {
                    IsSuccess = true,
                    Message = $"Department with ID {newDepartment.Did} updated successfully.",
                    Data = dept
                };
            });
        public override async Task<Result<Department>> Delete(int id)
            => await ExecuteOperation(async () =>
            {
                var department = _deptRepository.GetById(id); // Ensure the department exists before deletion
                if (department is null)
                {
                    return new Result<Department>
                    {
                        IsSuccess = false,
                        Message = "Invalid ID. Please provide a valid ID greater than 0.",
                        Data = new Department()
                        {
                            Did = id
                        }
                    };
                }

                await _deptRepository.DeleteAsync(department);

                return new Result<Department>
                {
                    IsSuccess = true,
                    Message = $"Department with ID {id} deleted successfully.",
                    Data = department
                };
            });
        public override async Task<Result<Department>> GetById(int id)
            => await ExecuteOperation(async () =>
            {
                var department = _deptRepository.GetById(id);
                if (department is null)
                {
                    return new Result<Department>
                    {
                        IsSuccess = false,
                        Message = "Invalid ID. Please provide a valid ID greater than 0.",
                        Data = new Department()
                        {
                            Did = id
                        }
                    };
                }
                return new Result<Department>
                {
                    IsSuccess = true,
                    Message = $"Department with ID {id} retrieved successfully.",
                    Data = department
                };
            });


        private bool IsValidDepartment(Department department)
            => IsValidId(department.Did) &&
               IsValidName(department.Dname) &&
               IsValidAddress(department.Daddress);

        private static bool IsValidName(string name) =>
            !string.IsNullOrWhiteSpace(name)
            && name.Length > 3;
        private static bool IsValidAddress(string name) =>
            !string.IsNullOrWhiteSpace(name)
            && name.Length > 4;

        public static bool IsValidId(int id) =>
            id > 0;

    }
}
