using System.DirectoryServices.AccountManagement;

namespace TiqUtils.Network.Windows
{
    public static class Ldap
    {
        public static string GetUserName(string userName, bool initalLetterName = true, string defaultName = "TestUser")
        {
            string result;
            try
            {
                using (var context = new PrincipalContext(ContextType.Domain))
                {
                    var principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName);
                    if (principal == null) return userName;

                    var firstName = initalLetterName ? principal.GivenName.Substring(0, 1) + "." : principal.GivenName;
                    var lastName = principal.Surname;
                    result = firstName + " " + lastName;
                }
            }
            catch
            {
                result = defaultName;
            }
            return result;
        }
    }
}
