using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskApp.Core;
using TaskApp.Core.DTOs;
using TaskApp.Core.Helpers;
using TaskApp.Core.Models;

namespace TaskApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet("GetDepartment")]
        public async Task<IActionResult> Get(int id)
        {
            var response = new BaseResponseWithData<DepartmentDto>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var dep = await _unitOfWork.Departments.GetAsync(x => x.Id == id, "departmentType");
                var department = _mapper.Map<DepartmentDto>(dep);
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
            var response = new BaseResponseWithDataList<DepartmentDto>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var dep = await _unitOfWork.Departments.GetAllAsync("departmentType");
                var department = _mapper.Map<IEnumerable<DepartmentDto>>(dep);
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
        public async Task<IActionResult> Create(DepartmentDto dep)
        {
            var response = new BaseResponseWithData<Department>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var department = _mapper.Map<Department>(dep);
                var depp = await _unitOfWork.Departments.CreateAsync(department);
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
        public async Task<IActionResult> Delete(DepartmentDto dep)
        {
            var response = new BaseResponseWithoutData();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var department = _mapper.Map<Department>(dep);
                _unitOfWork.Departments.DeleteAsync(department);
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
        public async Task<IActionResult> Edit([FromQuery]DepartmentDto dep)
        {
            var response = new BaseResponseWithData<Department>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var department = _mapper.Map<Department>(dep);
                var department1 = await _unitOfWork.Departments.EditAsync(department);
                response.Data = department1;
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
