﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Proje_Hastane
{
    internal class sqlbaglantisi
    {
        public SqlConnection baglanti()
        {
            SqlConnection baglan = new SqlConnection("Data Source=ONURPC\\MSSQLSERVER01;Initial Catalog=HastaneProje;Integrated Security=True;");
            baglan.Open();
            return baglan; 
        }

    }
}

