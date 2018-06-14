using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;


namespace PollingMVC_CS
{
    public class Poll
    {
     
            public int PLP_ID { get; set; }
            public string Assembler { get; set; }
            public string Assemble_Started { get; set; }
            public string Assemble_Ended { get; set; }
            public string Status { get; set; }
            public int Percent_Complete { get; set; }
            public string LastStepNote { get; set; }

        public enum StatusType
        {
            [Description("IN_PROGRESS")]
            IN_PROGRESS,
            [Description("COMPLETE")]
            COMPLETE
        }


        public Poll(string PLP_ID)
        {
            SqlConnection dbConn = new SqlConnection(ConnectFriendSQLClient());
            dbConn.Open();

            try
            {
                string tSQL = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/SQL/Poll_Get_Data.sql"));
                SqlDataAdapter tA = new SqlDataAdapter(tSQL, dbConn);
                tA.SelectCommand.Parameters.AddWithValue("@PLP_ID", PLP_ID);
                DataSet tSet = new DataSet();
                tA.Fill(tSet);
                if (tSet.Tables[0].Rows.Count > 0)
                {
                    DataRow tR = tSet.Tables[0].Rows[0];
                    PLP_ID =  tR["PLP_ID"].ToString();
                    Assembler = tR["Assembler"].ToString();
                    Assemble_Started = tR["Assemble_Started"].ToString();
                    if (tR["Assemble_Ended"] == System.DBNull.Value)
                        Assemble_Ended = tR["Assemble_Ended"].ToString();
                    else
                        Assemble_Ended = "";

                    Status = tR["Status"].ToString();
                    Percent_Complete = (int) tR["Percent_Complete"];
                    LastStepNote = tR["LastStepNote"].ToString();
                }
            }
            catch (Exception ex)
            {
            }

            finally
            {
                dbConn.Close();
            }
        }

        public static string KillAssembly(string PLP_ID)
        {
            SqlConnection dbConn = new SqlConnection(ConnectFriendSQLClient());
            dbConn.Open();



            string retval = "";
            try
            {
                System.Text.StringBuilder tb = new System.Text.StringBuilder();
                tb.AppendLine("		UPDATE PLP_Assemble SET  ");
                tb.AppendLine("								Assemble_Ended  = getDate(), ");
                tb.AppendLine("								Status=@Status, ");
                tb.AppendLine("								Percent_Complete=100 ");
                tb.AppendLine("	    WHERE PLP_ID=@PLP_ID; ");

                SqlCommand tC = new SqlCommand(tb.ToString(), dbConn);
                tC.Parameters.AddWithValue("@PLP_ID", PLP_ID);
                tC.Parameters.AddWithValue("@Status", StatusType.COMPLETE.ToString());
                tC.ExecuteNonQuery();
                retval = "Success";
            }
            catch (Exception ex)
            {
                retval = "ERROR";
            }
            finally
            {
                dbConn.Close();
            }

            return retval;
        }

        public static string UpdatePercentComplete(string PLP_ID, string Percent_Complete, string LastStepNote)
        {
            SqlConnection dbConn = new SqlConnection(ConnectFriendSQLClient());
            dbConn.Open();

            string retval = "";

            try
            {
                string tSQL = "UPDATE PLP_Assemble SET Percent_Complete=@Percent_Complete, LastStepNote=@LastStepNote WHERE PLP_ID=@PLP_ID;";
                SqlCommand tC = new SqlCommand(tSQL, dbConn);
                tC.Parameters.AddWithValue("@PLP_ID", PLP_ID);
                tC.Parameters.AddWithValue("@Percent_Complete", Percent_Complete);
                tC.Parameters.AddWithValue("@LastStepNote", LastStepNote);
                tC.ExecuteNonQuery();
                return "Success";
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbConn.Close();
            }

            return retval;
        }

        public static string InitAssemble(string PLP_ID, string UserID)
        {
            SqlConnection dbConn = new SqlConnection(ConnectFriendSQLClient());
            dbConn.Open();

            string retval = "";

            try
            {
                if (Poll.GetAssembleStatus(PLP_ID) != Poll.StatusType.IN_PROGRESS.ToString())
                {
                    string tSQL = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/SQL/Poll_Init.sql"));
                    SqlCommand tC = new SqlCommand(tSQL, dbConn);
                    tC.Parameters.AddWithValue("@PLP_ID", PLP_ID);
                    tC.Parameters.AddWithValue("@UserID", UserID);
                    tC.Parameters.AddWithValue("@Status", StatusType.IN_PROGRESS.ToString());
                    tC.ExecuteNonQuery();
                    retval = "SUCCESS";
                }
                else
                    retval = "PRICE LIST ALREADY BEING ASSEMBLED.";
            }

            catch (Exception ex)
            {
            }
            finally
            {
                dbConn.Close();
            }

            return retval;
        }

        public static string MarkAssemblyComplete(string PLP_ID)
        {
            SqlConnection dbConn = new SqlConnection(ConnectFriendSQLClient());
            dbConn.Open();

            string retval = "";

            try
            {
                string tSQL = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/SQL/Poll_AssemblyComplete.sql"));
                SqlCommand tC = new SqlCommand(tSQL, dbConn);
                tC.Parameters.AddWithValue("@PLP_ID", PLP_ID);
                tC.Parameters.AddWithValue("@Status", StatusType.COMPLETE.ToString());
                tC.ExecuteNonQuery();
                return "Success";
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbConn.Close();
            }

            return retval;
        }


        public static string GetAssembleStatus(string PLP_ID)
        {
            string retVal = "";
            SqlConnection dbConn = new SqlConnection(ConnectFriendSQLClient());
            dbConn.Open();

            try
            {
                System.Text.StringBuilder tb = new System.Text.StringBuilder();
                tb.AppendLine("select  ");
                tb.AppendLine("	isnull(status,'') as Status ");
                tb.AppendLine("FROM PLP_Assemble ");
                tb.AppendLine("WHERE PLP_ID=@PLP_ID ");
                SqlCommand tC = new SqlCommand(tb.ToString(), dbConn);
                tC.Parameters.AddWithValue("@PLP_ID", PLP_ID);
                SqlDataReader tR;
                tR = tC.ExecuteReader();
                if (tR.Read())
                    retVal = tR["Status"].ToString();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbConn.Close();
            }

            return retVal;
        }


        public static string ConnectFriendSQLClient()
        {
            string retVal = System.Configuration.ConfigurationManager.ConnectionStrings["PrimaryDB"].ConnectionString.ToString();

            return retVal;
        }





    }
}