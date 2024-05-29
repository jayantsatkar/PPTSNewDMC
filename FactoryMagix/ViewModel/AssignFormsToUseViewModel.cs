using FactoryMagix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryMagix.ViewModel
{
    public class AssignFormsToUseViewModel
    {
        public AssignFormsToUseViewModel()
        {
            RoleList = new List<string>();
            UserList = new List<string>();
            MoludeList = new List<MST_Module>();
            FormList = new List<REL_UserForm>();

        }
        public int RoleID { get; set; }
        public int UserID { get; set; }
        public IList<REL_UserForm> FormList { get; set; }
        public IList<MST_Module> MoludeList { get; set; }

        public IList<string> RoleList { get; set; }
        public IList<string> UserList { get; set; }
    }
}


//namespace FactoryMagix.ViewModel
//{
//    public class AssignFormsToUseViewModel
//    {
//        public AssignFormsToUseViewModel()
//        {
//            MoludeList = new List<string>();
//            FormList = new List<string>();
//            RoleList = new List<string>();
//            UserList = new List<string>();

//        }
//        public IList<string> RoleList { get; set; }
//        public IList<string> UserList { get; set; }
//        public IList<string> FormList { get; set; }
//        public IList<string> MoludeList { get; set; }
//    }
//}