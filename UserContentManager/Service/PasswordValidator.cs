namespace UserContentManager.Service
{
    public static class PasswordValidator
    {
        private static bool ContainsArmenianLetter(string password)
        {
            foreach (char c in password)
            {
                if ((c >= 'Ա' && c <= 'ֆ')) 
                    return true;
            }
            return false;
        }

   
        public static bool IsValidPassword(string password, string userName)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                return false;

            bool hasUppercase = false;
            bool hasLowercase = false;
            bool hasDigit = false;
            bool hasSpecialChar = false;
            bool hasArmenianLetter = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                    hasUppercase = true;

                if (char.IsLower(c))
                    hasLowercase = true;

                if (char.IsDigit(c))
                    hasDigit = true;

                if (!char.IsLetterOrDigit(c)) 
                    hasSpecialChar = true;

                if (ContainsArmenianLetter(password))
                    hasArmenianLetter = true;
            }
            if (password.IndexOf(userName, StringComparison.OrdinalIgnoreCase) >= 0)
                return false;

            return hasUppercase && hasLowercase && hasDigit && hasArmenianLetter && hasSpecialChar;
        }

    }
}
