﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Session1Xamarin
{
    public class Class1
    {
        public class User_Type
        {


            public int userTypeId { get; set; }
            public string userTypeName { get; set; }



        }

        public class User {
            public string userId { get; set; }
            public string userName { get; set; }
            public string userPw { get; set; }
            public int userTypeIdFK { get; set; }
        }

        public class CustomizedView
        {
            public string ResourceName { get; set; }
            public string ResourceType { get; set; }
            public int NumberOfSkills { get; set; }
            public string AllocatedSkills { get; set; }
            public string AvailableQuantity { get; set; }
        }

        public class Resource_Type {
            public int resTypeId { get; set; }
            public string resTypeName { get; set; }
        }

        public class Skill {
            public int skillId { get; set; }
            public string skillName { get; set; }
        }




    }
}
