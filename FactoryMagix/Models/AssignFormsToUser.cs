using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FactoryMagix.Models
{
    public class AssignFormsToUser
    {
        public int? SelectedRoleId { get; set; }
        public int? SelectedUserId { get; set; }

        public IEnumerable<MST_Role> DrRole { get; set; }
        public IEnumerable<User> DrUser { get; set; }
        public IEnumerable<MST_Module> DrModule { get; set; }
        public IEnumerable<REL_UserForm> DrRELUser { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
        public IEnumerable<string> SelectedUsers { get; set; }

    }


}