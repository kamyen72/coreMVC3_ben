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
using System.Text.Json;
using System.Text;

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

        [EnableCors("AllowAll")]
        [Route("TestBetTrans")]
        [HttpPost]
        public IActionResult TestBetTrans([FromBody] InputModel model)
        {
            var test = model.InputText;

            ReturnModel rm = new ReturnModel();
            rm.ReturnText = "ok";
            string rJason = JsonConvert.SerializeObject(rm);
            return Ok(rJason);
        }


        [EnableCors("AllowAll")]
        [Route("GetMPlayerRecs")]
        [HttpPost]
        public IActionResult GetMPlayerRecs([FromBody] twodates model)
        {
            twodates td = new twodates();
            td.dateStart = model.dateStart;
            td.dateEnd = model.dateEnd;

            List<MPlayerID> mybets = new List<MPlayerID>();

            DBUtil2 dbu = new DBUtil2();

            mybets = dbu.GetMPlayer(td.dateStart, td.dateEnd);
            var tt = mybets.Count;

            StringBuilder sj = new StringBuilder();

            sj.Append("[");
            int[] myIDs = new int[tt];

            for (int x = 0; x < tt; x++)
            {
                MPlayerID singleTran = mybets[x];
                string singleJ = System.Text.Json.JsonSerializer.Serialize(singleTran);
                if (x > 0)
                {
                    sj.Append(',');
                }
                sj.Append(singleJ);
                //sj.Append(',');
            }
            sj.Append(']');

            var test = sj;

            ReturnModel rm = new ReturnModel();
            rm.ReturnText = mybets.Count.ToString();
            //string rJason = JsonConvert.SerializeObject(mybets);
            string rJason = System.Text.Json.JsonSerializer.Serialize(mybets);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("GetMPlayerBetInfo")]
        [HttpPost]
        public IActionResult GetMPlayerBetInfo([FromBody] twodates mymodel)
        {
            var xid = mymodel.dateStart;

            BetTrans bt = new BetTrans();
            DBUtil2 dbu = new DBUtil2();
            bt = dbu.GetMPlayerInfo(xid);

            //var x = bt.Username;

            string rJason = JsonConvert.SerializeObject( bt );
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("GetLotteryTypeSummaryLevel")]
        [HttpPost]
        public IActionResult GetLotteryTypeSummaryLevel([FromBody] twodates mymodel)
        {
            var date1 = mymodel.dateStart;
            var date2 = mymodel.dateEnd;

            List<LotteryTypeSummary> finalsumm = null;

            DBUtil2 dbu = new DBUtil2();

            finalsumm = dbu.Get_LotteryTypeSummary(date1, date2);

            for (int i = 0; i < finalsumm.Count; i++)
            {
                var tt = finalsumm[i];
                int myid = tt.LotteryTypeID;
                tt.LotteryTypeName = dbu.GetLotteryTypeName(myid.ToString());
            }

            string rJason = JsonConvert.SerializeObject(finalsumm);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("GetCalculatedFields")]
        [HttpPost]
        public IActionResult GetCalculatedFields([FromBody] twodates mymodel)
        {
            var date1 = mymodel.dateStart;
            var date2 = mymodel.dateEnd;
            var LotteryTypeID = int.Parse( mymodel.LotteryTypeID.ToString() );

            DBUtil2 dbu = new DBUtil2();

            MPlayerCalcFields mpc = dbu.Get_MPlayer_CalcFields(date1, date2, LotteryTypeID);

            string rJason = JsonConvert.SerializeObject(mpc);
            return Ok(rJason);
        }
    }
}
