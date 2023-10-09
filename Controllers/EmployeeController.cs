using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskApp.Core;
using TaskApp.Core.DTOs;
using TaskApp.Core.Helpers;
using TaskApp.Core.Interfaces;
using TaskApp.Core.Models;

namespace TaskApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public EmployeeController(IUnitOfWork unitOfWork,IMapper mapper,IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpGet("GetEmployee")]
        public async Task<IActionResult> Get(int id)
        {
            var response = new BaseResponseWithData<EmployeeDto>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var emp = await _unitOfWork.Employees.GetAsync(x => x.Id == id, "department");
                var employee = _mapper.Map<EmployeeDto>(emp);
                response.Data = employee;
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
        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            var response = new BaseResponseWithData<EmployeeDto>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var emp = await _unitOfWork.Employees.GetByName(name);
                var employee = _mapper.Map<EmployeeDto>(emp);
                response.Data = employee;
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
        [HttpGet("GetTopSalary")]
        public async Task<IActionResult> GetTopSalary()
        {
            var response = new BaseResponseWithData<EmployeeDto>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var emp = await _unitOfWork.Employees.GetTopSalary();
                var employee = _mapper.Map<EmployeeDto>(emp.First());
                response.Data = employee;
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
            var response = new BaseResponseWithDataList<EmployeeDto>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var emp = await _unitOfWork.Employees.GetAllAsync("department");
                var employee = _mapper.Map<IEnumerable<EmployeeDto>>(emp);
                response.Data = employee;
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
        public async Task<IActionResult> Create(EmployeeDto emp)
        {
            var response = new BaseResponseWithData<Employee>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var employee = _mapper.Map<Employee>(emp);
                var empp = await _unitOfWork.Employees.CreateAsync(employee);
                response.Data = empp;
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
        public async Task<IActionResult> Delete(EmployeeDto emp)
        {
            var response = new BaseResponseWithoutData();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var employee = _mapper.Map<Employee>(emp);
                _unitOfWork.Employees.DeleteAsync(employee);
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
        public async Task<IActionResult> Edit([FromQuery]EmployeeDto emp)
        {
            var response = new BaseResponseWithData<Employee>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var employee = _mapper.Map<Employee>(emp);
                var empp = await _unitOfWork.Employees.EditAsync(employee);
                response.Data = empp;
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
        [HttpPost("Login")]
        public async Task<IActionResult> Login(EmployeeLoginDto emp)
        {
            var response = new BaseResponseWithData<AuthModel>();
            response.result = true;
            response.errors = new List<Error>();
            try
            {
                var auth = await _authService.Login(emp);
                response.Data = auth;
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
