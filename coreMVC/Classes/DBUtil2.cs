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

namespace CoreMVC3.Classes
{
    public class DBUtil2
    {
        public List<BetTrans> GetMPlayer(string dstart, string dend)
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

            List <BetTrans> transList = new List<BetTrans>();

            int maxcount = myDataRows.Rows.Count;

            for (int x = 0; x < maxcount; x++)
            {
                DataRow thisrow = myDataRows.Rows[x];

                BetTrans oneBet = new BetTrans();
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
                oneBet.Username = thisrow["UserName"].ToString();
                oneBet.Openning_Time = DateTime.Parse(thisrow["ShowResultDate"].ToString());
            }
            return oneBet;

            //return "";

        }
    }
}
