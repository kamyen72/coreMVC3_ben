using CoreMVC3.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Reflection.Metadata;
using static CoreMVC2.Controllers.APIController;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using Microsoft.AspNetCore.Hosting;
using System.Web;
using CoreMVC3.Classes;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace CoreMVC3.Controllers
{
    [Route("/[controller]")]

    public class BetController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public BetController(IWebHostEnvironment env)
        {
            _env = env;
        }

        
    }
}
