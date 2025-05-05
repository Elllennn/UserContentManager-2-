using UserContentManager.Contracts;

namespace UserContentManager.TestServices
{
    public class TestEditUser : ITestEditUser
    {
        public List<string> GetUsers()
        {
            var list = new List<string>
            {
                "am","Elen",""
            };
            return list;    
        }
    }
}
