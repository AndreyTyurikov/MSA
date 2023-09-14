using System.Security.Cryptography;
using System.Text;

namespace UserMS.Services
{
    public class PasswordService : IPasswordService
    {
        public byte[] CreatePasswordHash(string openTextPassword)
        {
            SHA256 hasher = SHA256.Create();

            byte[] bytesOfPassword = Encoding.UTF8.GetBytes(openTextPassword);

            return hasher.ComputeHash(bytesOfPassword);
        }

        public bool ValidatePasswordAgainstHash(byte[] hashToCheck, string passwordToCheck)
        {
            byte[] hashOfPasswordToCheck = CreatePasswordHash(passwordToCheck);

            if (hashToCheck.Length != hashOfPasswordToCheck.Length) return false;

            for (int a = 0; a < hashToCheck.Length; a++)
            {
                //On the first mismatch -> exit
                if (hashToCheck[a] != hashOfPasswordToCheck[a]) return false;
            }

            return true;
        }
    }
}
