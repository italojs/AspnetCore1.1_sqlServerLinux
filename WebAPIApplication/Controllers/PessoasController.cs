using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPIApplication
{
    [Route("api/Pessoas")]
    public class PessoasController : Controller
    {
        private readonly DataContext _dataContext;
        public PessoasController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetPessoas()
        {
            var pessoas = await _dataContext.Pessoas.ToListAsync();
            return Json(pessoas);
        }

        [HttpPost]
        public async Task<IActionResult> PostPessoas([FromBody]Pessoa model)
        {
            Dictionary<String,object> dct = new Dictionary<string,object>();
            try
            {
                await _dataContext.Pessoas.AddAsync(model);
                await _dataContext.SaveChangesAsync();

                dct.Add("Status",HttpStatusCode.OK);
                return Json(dct);
            }catch(Exception ex)
            {
                dct.Add("Status",HttpStatusCode.InternalServerError);
                dct.Add("Error",ex);
                return Json(dct);
            }

            
        }
    }
}