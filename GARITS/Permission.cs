using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GARITS
{
    class Permission
    {
        private int roleID;
        private string permission;
        private bool access;
        private bool componentSet;
        private Object component;

        public Permission(int roleID, string permission, bool access)
        {
            this.roleID = roleID;
            this.permission = permission;
            this.access = access;
        }

        public string getPermission()
        {
            return permission;
        }

        public int getRoleID()
        {
            return roleID;
        }

        public bool getAccess()
        {
            return access;
        }

        public Object getComponent()
        {
            return component;
        }

        public void setComponent(Object obj)
        {
            if (!componentSet)
            {
                component = obj;
                componentSet = true;
            }
            else
                throw new Exception("Component added twice. Probably means someone used the same string twice.");
        }
    }
}
