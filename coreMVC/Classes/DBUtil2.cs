using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using CoreMVC3.Classes;
using System.Data;
using System.Net;
using System.Xml.Linq;
using CoreMVC3.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Identity.Client;

namespace CoreMVC3.Classes
{
    public class DBUtil2
    {
        public List<MPlayerID> GetMPlayer(string dstart, string dend)
        {
            
            var localconnstr = db_master.connStr;

            string sql2 = "Select ID from MPlayer where UpdateDate between '" + dstart + "' and '" + dend + "' ";

            SqlConnection connection = new SqlConnection(localconnstr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            List <MPlayerID> transList = new List<MPlayerID>();

            int maxcount = myDataRows.Rows.Count;

            for (int x = 0; x < maxcount; x++)
            {
                DataRow thisrow = myDataRows.Rows[x];

                MPlayerID oneBet = new MPlayerID();
                oneBet.ID = int.Parse( thisrow["ID"].ToString() );
                //oneBet.BetTime = DateTime.Parse( thisrow["ShowResultDate"].ToString() );
                //oneBet.UpdateDate = DateTime.Parse(thisrow["UpdateDate"].ToString());
                //oneBet.Bet_3 = thisrow["SelectedNums"].ToString();
                //oneBet.IsWin = bool.Parse( thisrow["IsWin"].ToString() );
                //oneBet.TOver = decimal.Parse(thisrow["Price"].ToString())
                //oneBet.Capital = decimal.Parse(thisrow["DiscountPrice"].ToString());
                //oneBet.GameDealerMemberID = int.Parse(thisrow["GameDealerMemberID"].ToString());
                //oneBet.Username = thisrow["UserName"].ToString();

                transList.Add(oneBet);

                
            }
            return transList;

            //return "";

        }

        public BetTrans GetMPlayerInfo(string ID)
        {

            var localconnstr = db_master.connStr;

            string sql2 = "Select * from MPlayer where ID = " + ID + ";";

            SqlConnection connection = new SqlConnection(localconnstr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            List<BetTrans> transList = new List<BetTrans>();

            int maxcount = myDataRows.Rows.Count;
            BetTrans oneBet = new BetTrans();
            for (int x = 0; x < maxcount; x++)
            {
                DataRow thisrow = myDataRows.Rows[x];
                oneBet.ID = int.Parse(thisrow["ID"].ToString());
                oneBet.BetTime = DateTime.Parse(thisrow["CreateDate"].ToString());
                oneBet.UpdateDate = DateTime.Parse(thisrow["UpdateDate"].ToString());
                oneBet.Bet_3 = thisrow["SelectedNums"].ToString();
                oneBet.IsWin = bool.Parse(thisrow["IsWin"].ToString());
                oneBet.TOver = decimal.Parse(thisrow["Price"].ToString());
                oneBet.Capital = decimal.Parse(thisrow["DiscountPrice"].ToString());
                oneBet.GameDealerMemberID = int.Parse(thisrow["GameDealerMemberID"].ToString());
                oneBet.Username =  thisrow["UserName"].ToString() ;
                oneBet.Openning_Time = DateTime.Parse(thisrow["ShowResultDate"].ToString());
                oneBet.Bill_No_Ticket = thisrow["CurrentPeriod"].ToString();
            }
            return oneBet;

            //return "";

        }

        public decimal Get_MPlayer_TOver_by_Date_LotteryTypeId(string DateStart, string DateEnd, int LotteryTypeId)
        {
            var localconnstr = db_master.connStr;

            string sql = "";
            sql = sql + "select sum( cast(isnull(price, 0) as decimal) ) as TOver ";
            sql = sql + "from openrowset('SQLOLEDB', '192.82.60.148'; 'MasterUser'; '@master85092212', [MasterGHL].[dbo].[MPlayer]) a ";
            sql = sql + "inner join openrowset('SQLOLEDB', '192.82.60.148'; 'MasterUser'; '@master85092212', [MasterGHL].[dbo].[LotteryInfo]) b on a.LotteryInfoID = b.LotteryInfoID ";
            sql = sql + "where LotteryTypeID = @dbLotteryTypeID ";
            sql = sql + "and a.UpdateDate between '@dbDateStart' and '@dbDateEnd' ";

            string sql2 = sql.Replace("@dbLotteryTypeID", LotteryTypeId.ToString())
                             .Replace("@dbDateStart", DateStart)
                             .Replace("@dbDateEnd", DateEnd);

            SqlConnection connection = new SqlConnection(localconnstr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            int maxcount = myDataRows.Rows.Count;
            decimal returnval = 0;
            for (int x = 0; x < maxcount; x++)
            {
                DataRow dis = myDataRows.Rows[x];
                returnval = decimal.Parse( dis["TOver"].ToString() );
            }

            return returnval;
        }

        public decimal Get_MPlayer_Pending_by_Date_LotteryTypeId(string DateStart, string DateEnd, int LotteryTypeId)
        {
            var localconnstr = db_master.connStr;

            string sql = "";
            sql = sql + "select sum( ";
            sql = sql + "case ";
            sql = sql + "when iswin is null then cast(isnull(price, 0) as decimal) ";
            sql = sql + "else 0 ";
            sql = sql + "end ";
            sql = sql + ") as pending ";
            sql = sql + "from [MPlayer] a ";
            sql = sql + "inner join [LotteryInfo] b on a.LotteryInfoID = b.LotteryInfoID ";
            sql = sql + "where LotteryTypeID = @dbLotteryTypeID ";
            sql = sql + "and a.UpdateDate between '@dbDateStart' and '@dbDateEnd' ";

            string sql2 = sql.Replace("@dbLotteryTypeID", LotteryTypeId.ToString())
                             .Replace("@dbDateStart", DateStart)
                             .Replace("@dbDateEnd", DateEnd);

            SqlConnection connection = new SqlConnection(localconnstr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            int maxcount = myDataRows.Rows.Count;
            decimal returnval = 0;
            for (int x = 0; x < maxcount; x++)
            {
                DataRow dis = myDataRows.Rows[x];
                returnval = decimal.Parse(dis["pending"].ToString());
            }

            return returnval;
        }

        public decimal Get_MPlayer_AllLost_by_Date_LotteryTypeId(string DateStart, string DateEnd, int LotteryTypeId)
        {
            var localconnstr = db_master.connStr;

            string sql = "";
            sql = sql + "select sum( ";
            sql = sql + "case ";
            sql = sql + "when (iswin = 0) then cast(isnull(winMoney, 0) as decimal) - cast(isnull(DiscountPrice, 0) as decimal) ";
            sql = sql + "else 0 ";
            sql = sql + "end ";
            sql = sql + ") as AllLost ";
            sql = sql + "from [MPlayer] a ";
            sql = sql + "inner join [LotteryInfo] b on a.LotteryInfoID = b.LotteryInfoID ";
            sql = sql + "where LotteryTypeID = @dbLotteryTypeID ";
            sql = sql + "and a.UpdateDate between '@dbDateStart' and '@dbDateEnd' ";

            string sql2 = sql.Replace("@dbLotteryTypeID", LotteryTypeId.ToString())
                             .Replace("@dbDateStart", DateStart)
                             .Replace("@dbDateEnd", DateEnd);

            SqlConnection connection = new SqlConnection(localconnstr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            int maxcount = myDataRows.Rows.Count;
            decimal returnval = 0;
            for (int x = 0; x < maxcount; x++)
            {
                DataRow dis = myDataRows.Rows[x];
                returnval = decimal.Parse(dis["AllLost"].ToString());
            }

            return returnval;
        }

        public decimal Get_MPlayer_All4DWin_by_Date_LotteryTypeId(string DateStart, string DateEnd, int LotteryTypeId)
        {
            var localconnstr = db_master.connStr;

            string sql = "";
            sql = sql + "select sum( ";
            sql = sql + "case ";
            sql = sql + "when (iswin = 1) and (b.DrawTypeID between 142 and 152) then cast(isnull(winMoney, 0) as decimal) - cast(isnull(DiscountPrice, 0) as decimal) ";
            sql = sql + "else 0 ";
            sql = sql + "end ";
            sql = sql + ") as all4Dwin ";
            sql = sql + "from [MPlayer] a ";
            sql = sql + "inner join [LotteryInfo] b on a.LotteryInfoID = b.LotteryInfoID ";
            sql = sql + "where LotteryTypeID = @dbLotteryTypeID ";
            sql = sql + "and a.UpdateDate between '@dbDateStart' and '@dbDateEnd' ";

            string sql2 = sql.Replace("@dbLotteryTypeID", LotteryTypeId.ToString())
                             .Replace("@dbDateStart", DateStart)
                             .Replace("@dbDateEnd", DateEnd);

            SqlConnection connection = new SqlConnection(localconnstr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            int maxcount = myDataRows.Rows.Count;
            decimal returnval = 0;
            for (int x = 0; x < maxcount; x++)
            {
                DataRow dis = myDataRows.Rows[x];
                returnval = decimal.Parse(dis["all4Dwin"].ToString());
            }

            return returnval;
        }

        public MPlayerCalcFields Get_MPlayer_CalcFields(string DateStart, string DateEnd, int LotteryTypeId)
        {
            var localconnstr = db_master.connStr;

            string sql = "";
            sql = sql + "select ";
            sql = sql + "sum( cast(isnull(price, 0) as decimal) ) as TOver ";
            sql = sql + ", sum( ";
            sql = sql + "case ";
            sql = sql + "when iswin is null then cast(isnull(price, 0) as decimal) ";
            sql = sql + "else 0 ";
            sql = sql + "end ";
            sql = sql + ") as pending ";
            sql = sql + ", sum( ";
            sql = sql + "case ";
            sql = sql + "when (iswin = 0) then cast(isnull(winMoney, 0) as decimal) - cast(isnull(DiscountPrice, 0) as decimal) ";
            sql = sql + "else 0 ";
            sql = sql + "end ";
            sql = sql + ") as AllLost ";
            sql = sql + ", sum( ";
            sql = sql + "case ";
            sql = sql + "when (iswin = 1) and (b.DrawTypeID between 142 and 152) then cast(isnull(winMoney, 0) as decimal) - cast(isnull(DiscountPrice, 0) as decimal) ";
            sql = sql + "else 0 ";
            sql = sql + "end ";
            sql = sql + ") as all4Dwin ";

            sql = sql + ", sum( ";
            sql = sql + "case ";
            sql = sql + "when (iswin = 1) and (b.DrawTypeID NOT between 142 and 152) then cast(isnull(winMoney, 0) as decimal)";
            sql = sql + "else 0 ";
            sql = sql + "end ";
            sql = sql + ") as allnon4Dwin ";

            sql = sql + "from [MPlayer] a ";
            sql = sql + "inner join [LotteryInfo] b on a.LotteryInfoID = b.LotteryInfoID ";
            sql = sql + "where LotteryTypeID = @dbLotteryTypeID ";
            sql = sql + "and a.UpdateDate between '@dbDateStart' and '@dbDateEnd' ";

            string sql2 = sql.Replace("@dbLotteryTypeID", LotteryTypeId.ToString())
                             .Replace("@dbDateStart", DateStart)
                             .Replace("@dbDateEnd", DateEnd);

            SqlConnection connection = new SqlConnection(localconnstr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            int maxcount = myDataRows.Rows.Count;
            MPlayerCalcFields mpc = new MPlayerCalcFields();

            for (int x = 0; x < maxcount; x++)
            {
                DataRow dis = myDataRows.Rows[x];
                mpc.TOVer = decimal.Parse( dis["TOver"].ToString() );
                mpc.Pending = decimal.Parse(dis["pending"].ToString());
                mpc.AllLost = decimal.Parse(dis["AllLost"].ToString());
                mpc.All4dWin = decimal.Parse(dis["all4Dwin"].ToString());
                mpc.Allnon4dWin = decimal.Parse(dis["allnon4Dwin"].ToString());
                mpc.WL = mpc.AllLost + mpc.All4dWin + mpc.Allnon4dWin;
                decimal p90 = decimal.Parse( "0.9" );
                decimal p10 = decimal.Parse("0.1");
                decimal neg = decimal.Parse("-1");
                mpc.Agent_WL = decimal.Multiply( decimal.Multiply( mpc.WL , p90) , neg);
                mpc.Com_WL = decimal.Multiply(decimal.Multiply(mpc.WL, p10), neg);
            }

            return mpc;
        }

        public List<LotteryTypeSummary> Get_LotteryTypeSummary(string DateStart, string DateEnd)
        {
            var localconnstr = db_master.connStr;

            string sql = "";
            sql = sql + "select distinct ";
            sql = sql + "b.LotteryTypeID ";
            sql = sql + "from [MPlayer] a ";
            sql = sql + "left join [LotteryInfo] b on a.LotteryInfoId = b.LotteryInfoID ";
            sql = sql + "where a.UpdateDate between '@dbDateStart' and '@dbDateEnd' ";

            string sql2 = sql.Replace("@dbDateStart", DateStart)
                             .Replace("@dbDateEnd", DateEnd);

            SqlConnection connection = new SqlConnection(localconnstr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            List<LotteryTypeSummary> lotsum = new List<LotteryTypeSummary>();

            int maxcount = myDataRows.Rows.Count;
            decimal returnval = 0;
            for (int x = 0; x < maxcount; x++)
            {
                DataRow dis = myDataRows.Rows[x];
                
                LotteryTypeSummary sumone = new LotteryTypeSummary();
                sumone.LotteryTypeID = int.Parse( dis["LotteryTypeID"].ToString() );

                lotsum.Add(sumone);
            }

            return lotsum;
        }

        public string GetLotteryTypeName(string ID)
        {

            var localconnstr = db_master.connStr;

            string sql2 = "select distinct LotteryTypeName from LotteryType where LotteryTypeID = " + ID ;

            SqlConnection connection = new SqlConnection(localconnstr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            string TypeName = "";

            int maxcount = myDataRows.Rows.Count;
            for (int x = 0; x < maxcount; x++)
            {
                DataRow thisrow = myDataRows.Rows[x];
                TypeName = thisrow["LotteryTypeName"].ToString();
            }
            return TypeName;

            //return "";

        }
    }
}
