﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp.Model
{
    class DBA_Portfolio
    {

        public static void AddNewPortfolioToTable(Entities.Portfolio p)
        {

            string sql = "INSERT INTO Portfolio (Name, Email) VALUES (@Name,@Email)";
            SqlCommand cmd = new SqlCommand(sql, Globals.Db.conn);

            cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = p.Name;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = p.Email;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            

        }
        public static List<Entities.Portfolio> GetAll()
        {
            List<Entities.Portfolio> result = new List<Entities.Portfolio>();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Portfolio", Globals.Db.conn))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = (int)reader["portfolioId"];
                    string name = (string)reader["Name"];
                    string email = (string)reader["Email"];
                    decimal cash = (decimal)reader["Cash"];
                    decimal net = (decimal)reader["Net"];
                    decimal balance = (decimal)reader["Balance"];
                    Entities.Portfolio p = new Entities.Portfolio(id, name, email, cash, net, balance);
                    result.Add(p);
                }
            }
            return result;
        }

        public static void UpdateBalance(decimal d, Entities.Portfolio p)
        {
            string sql = "UPDATE Portfolio " +
                "SET Balance=@Balance " +
                "WHERE PortfolioId=@PortfolioId";

            SqlCommand cmd = new SqlCommand(sql, Globals.Db.conn);
            cmd.Parameters.Add("@Balance", SqlDbType.Money).Value = d;
            cmd.Parameters.Add("@PortfolioId", SqlDbType.Int).Value = p.PortfolioID;
            cmd.ExecuteNonQuery();
        }

        public static void UpdateNet(decimal net, Entities.Portfolio p)
        {
            string sql = "UPDATE Portfolio " +
                "SET Net=@Net " +
                "WHERE PortfolioId=@PortfolioId";

            SqlCommand cmd = new SqlCommand(sql, Globals.Db.conn);
            cmd.Parameters.Add("@Net", SqlDbType.Money).Value = net;
            cmd.Parameters.Add("@PortfolioId", SqlDbType.Int).Value = p.PortfolioID;
            cmd.ExecuteNonQuery();
        }


        public static Entities.Portfolio GetUpdatedPortfolio(Entities.Portfolio p)
        {
            Entities.Portfolio updatedPortfolio= new Entities.Portfolio (1,"temp","Temp",0,0,0);

            String sql = "SELECT * FROM Portfolio WHERE PortfolioId=@PortfolioId";

            SqlCommand cmdGetPortfolio = new SqlCommand(sql, Globals.Db.conn);
            cmdGetPortfolio.Parameters.Add("@PortfolioId", SqlDbType.Int).Value = p.PortfolioID;

            using (SqlDataReader reader = cmdGetPortfolio.ExecuteReader())

            {
                while (reader.Read())
                {
                    int id = (int)reader["portfolioId"];
                    string name = (string)reader["Name"];
                    string email = (string)reader["Email"];
                    decimal cash = (decimal)reader["Cash"];
                    decimal net = (decimal)reader["Net"];
                    decimal balance = (decimal)reader["Balance"];
                    updatedPortfolio = new Entities.Portfolio(id, name, email, cash, net, balance);
                }
            }
            return updatedPortfolio;
        }

        public static void deletePortfolioById(int PortfolioID)
        {
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Portfolio WHERE portfolioId = @portfolioId", Globals.Db.conn))
            {

                cmd.Parameters.AddWithValue("@portfolioId", PortfolioID);

                cmd.ExecuteNonQuery();
            }
        }
    }

    
}
