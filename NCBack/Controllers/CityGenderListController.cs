using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NCBack.Data;

namespace NCBack.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CityGenderListController : Controller
{
    private readonly DataContext _context;

    public CityGenderListController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("getCityList")]
    public async Task<ActionResult> GetCityList()
    {
        var cityLists = _context.CityList.ToList().Distinct();
        return Ok(cityLists);
    }

    [HttpGet("getByIdCityList/{Id}")]
    public async Task<ActionResult> GetByIdCityList(int Id)
    {
        var city = _context.CityList.FirstOrDefault(c => c.Id == Id);
        return Ok(city);
    }

    [HttpGet("getGenderList")]
    public async Task<ActionResult> GetGenderList()
    {
        var genderLists = _context.GenderList.ToList().Distinct();
        return Ok(genderLists);
    }

    [HttpGet("getByIdGenderList/{Id}")]
    public async Task<ActionResult> GetByIdGenderList(int Id)
    {
        var gender = _context.GenderList.FirstOrDefault(g => g.Id == Id);
        return Ok(gender);
    }  
    
}