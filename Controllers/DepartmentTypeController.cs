using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;
using TaskApp.Core;
using TaskApp.Core.DTOs;
using TaskApp.Core.Helpers;
using TaskApp.Core.Models;

namespace TaskApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentTypeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentTypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
           _unitOfWork = unitOfWork;
           _mapper = mapper;
        }
        [HttpGet("GetDepartmentType")]
        public async Task<IActionResult> Get(int id)
        {
            var response = new BaseResponseWithData<DepartmentTypeDto>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var dep = await _unitOfWork.DepartmentTypes.GetAsync(x => x.Id == id);
                var department = _mapper.Map<DepartmentTypeDto>(dep);
                response.Data = department;
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.result = false;
                Error err = new Error();
                err.code = "E-1";
                err.Message = "Exception :" + ex.Message;
                response.errors.Add(err);
                return BadRequest(response);
            }
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = new BaseResponseWithDataList<DepartmentTypeDto>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var dep = (await _unitOfWork.DepartmentTypes.GetAllAsync());
                var department = _mapper.Map<IEnumerable<DepartmentTypeDto>>(dep);
                response.Data = department;
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.result = false;
                Error err = new Error();
                err.code = "E-1";
                err.Message = "Exception :" + ex.Message;
                response.errors.Add(err);
                return BadRequest(response);
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(DepartmentTypeDto dep)
        {
            var response = new BaseResponseWithData<DepartmentType>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var departmentType = _mapper.Map<DepartmentType>(dep);
                var depp = await _unitOfWork.DepartmentTypes.CreateAsync(departmentType);
                response.Data = depp;
                _unitOfWork.complete();
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.result = false;
                Error err = new Error();
                err.code = "E-1";
                err.Message = "Exception :" + ex.Message;
                response.errors.Add(err);
                return BadRequest(response);
            }
            
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(DepartmentTypeDto dep)
        {
            var response = new BaseResponseWithoutData();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var departmentType = _mapper.Map<DepartmentType>(dep);
                _unitOfWork.DepartmentTypes.DeleteAsync(departmentType);
                _unitOfWork.complete();
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.result = false;
                Error err = new Error();
                err.code = "E-1";
                err.Message = "Exception :" + ex.Message;
                response.errors.Add(err);
                return BadRequest(response);
            }
        }
        [HttpGet("Edit")]
        public async Task<IActionResult> Edit([FromQuery]DepartmentTypeDto dep)
        {
            var response = new BaseResponseWithData<DepartmentType>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var departmentType = _mapper.Map<DepartmentType>(dep);
                var depp = await _unitOfWork.DepartmentTypes.EditAsync(departmentType);
                response.Data = depp;
                _unitOfWork.complete();
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.result = false;
                Error err = new Error();
                err.code = "E-1";
                err.Message = "Exception :" + ex.Message;
                response.errors.Add(err);
                return BadRequest(response);
            }
        }
    }
}
