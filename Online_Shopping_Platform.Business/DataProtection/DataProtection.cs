using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Shopping_Platform.Business.DataProtection
{
    public class DataProtection : IDataProtection
    {
        private readonly IDataProtector _protector;  // IDataProtector instance to handle encryption/decryption

        // Constructor that initializes the IDataProtector using the provider and a custom purpose string ("Security")
        public DataProtection(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("Security");
        }

        // Method to encrypt (protect) a string
        public string Protect(string text)
        {
            return _protector.Protect(text);  // Encrypt the input text
        }

        // Method to decrypt (unprotect) a string
        public string UnProtect(string protectedText)
        {
            return _protector.Unprotect(protectedText);  // Decrypt the input protected text
        }
    }

}
