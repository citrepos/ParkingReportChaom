using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ParkingManagementReport
{
    public class User
    {
        public string ID = "";
        public string Name = "";
        public string Username = ""; //Mac 2014/08/16
        public string Password = "";
        public string Address = "";
        public string Tel = "";
        public string WorkID = "";
        public int Level = 0;
        public bool LoginReady = false;
        public int intPrice = 0;
        public int intDiscount = 0;
        public int Grouprpt = 0; //Mac 2014/08/16
        //public bool booOffline = false;

        public int sumPrice = 0;
        public int sumDiscount = 0;       

    }
}
