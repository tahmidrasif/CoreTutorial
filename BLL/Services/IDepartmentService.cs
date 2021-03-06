﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Model;
using DLL.Repository;
using DLL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Utility;

namespace BLL.Services
{
    public interface IDepartmentService
    {
        Task<Department> AddDepartmentAsync(DepartInsertRequest request);
        Task<List<Department>> GetAllDepartmentAsync();
        Task<Department> FindADepartmentAsync(string code);

        Task<Boolean> IsNameExists(string name);
        Task<Boolean> IsCodeExits(string name);
        Task<Department> DeleteDepartmentAsync(string code);
        Task<Department> UpdateDepartmentAsync(string code, DepertmentUpdateRequest aDepartment);
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;


        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Department> AddDepartmentAsync(DepartInsertRequest request)
        {
            Department department = new Department()
            {
                Code = request.Code,
                Name = request.Name
            };
            await _unitOfWork.DepartmentRepository.InsertAsync(department);

            if (await _unitOfWork.DbSaveChangeAsync())
            {
                return department;
            }
            throw new ExceptionManagementHelper("something went wrong");
        }

        public async Task<List<Department>> GetAllDepartmentAsync()
        {
            return  _unitOfWork.DepartmentRepository.QueryAll().Include(x=>x.Students).ToList();
        }

        public async Task<Department> FindADepartmentAsync(string code)
        {
            var department = await _unitOfWork.DepartmentRepository.GetSingleAsync(x=>x.Code== code);
            if (department == null)
            {
                throw new ExceptionManagementHelper("department not found");
            }

            return department;
        }

        public async Task<bool> IsNameExists(string name)
        {
            var department = await _unitOfWork.DepartmentRepository.GetSingleAsync(x => x.Name == name);
            if (department != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsCodeExits(string code)
        {
            var department = await _unitOfWork.DepartmentRepository.GetSingleAsync(x => x.Code == code);
            if (department != null)
            {
                return true;
            }

            return false;
        }

        public async Task<Department> DeleteDepartmentAsync(string code)
        {
            var department = await _unitOfWork.DepartmentRepository.GetSingleAsync(x => x.Code == code);

            if (department == null)
            {
                throw new ExceptionManagementHelper("department not found");
            }
            _unitOfWork.DepartmentRepository.DeleteAsync(department);
            
            if (await _unitOfWork.DbSaveChangeAsync())
            {
                return department;
            }
            throw new ExceptionManagementHelper("something went wrong");
        }

        public async Task<Department> UpdateDepartmentAsync(string code, DepertmentUpdateRequest aDepartment)
        {
            var department = await _unitOfWork.DepartmentRepository.GetSingleAsync(x => x.Code == code);

            if (department == null)
            {
                throw new ExceptionManagementHelper("department not found");
            }

            if (!string.IsNullOrWhiteSpace(aDepartment.Code))
            {
                var isCodeExistsAnotherDepartment = await _unitOfWork.DepartmentRepository.GetSingleAsync(x =>
                    x.Code == aDepartment.Code
                    && x.DepartmentId != department.DepartmentId);
                if (isCodeExistsAnotherDepartment == null)
                {
                    department.Code = aDepartment.Code;
                }
                else
                {
                    throw new ExceptionManagementHelper("code already exists different department");
                }
            }
            
            if (!string.IsNullOrWhiteSpace(aDepartment.Name))
            {
                var isCodeExistsAnotherDepartment = await _unitOfWork.DepartmentRepository.GetSingleAsync(x =>
                    x.Name == aDepartment.Name
                    && x.DepartmentId != department.DepartmentId);
                if (isCodeExistsAnotherDepartment == null)
                {
                    department.Name = aDepartment.Name;
                }
                else
                {
                    throw new ExceptionManagementHelper("name already exists different department");
                }
            }
            
            _unitOfWork.DepartmentRepository.Update(department);
            
            if (await _unitOfWork.DbSaveChangeAsync())
            {
                return department;
            }
            throw new ExceptionManagementHelper("something went wrong");
        }
    }
}