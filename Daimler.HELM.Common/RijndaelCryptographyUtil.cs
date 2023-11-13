using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Daimler.HELM.Common
{
    /// <summary>
    /// Add by Hanxt
    /// </summary>
    public class RijndaelKey
    {
        public string PassPhrase { get; set; }
        public string SaltValue { get; set; }
        public string HashAlgorithm { get; set; }
        public int PasswordIterations { get; set; }
        public string InitVector { get; set; }
        public int KeySize { get; set; }
    }

    public class RijndaelCryptographyUtil
    {
        #region Fields

		private string passPhrase = "Pts53+9e";
        private string saltValue = "s@1tVclue";
        private string hashAlgorithm = "SHA1";
		private int    passwordIterations = 2;
        private string initVector = "@3S643D4eDf6g79F";
        private int    keySize = 256;

        #endregion

        #region Constructor (s) / Destructor
        /// <summary>
        /// Creates a new instance of the
        /// <see cref="RijndaelCryptographyUtil"/> class.
        /// </summary>
        public RijndaelCryptographyUtil()
        {
			PassPhrase         = "Pts53+9e";        // can be any string
			SaltValue          = "s@1tVclue";        // can be any string
			HashAlgorithm      = "SHA1";             // can be "MD5"
			PasswordIterations = 2;                  // can be any number
			InitVector         = "@3S643D4eDf6g79F"; // must be 16 bytes
			KeySize            = 256;                // can be 192 or 128
        }
        public RijndaelCryptographyUtil(RijndaelKey key)
        {
            PassPhrase = key.PassPhrase != null ? key.PassPhrase : PassPhrase;        // can be any string
            SaltValue = key.SaltValue != null ? key.SaltValue : SaltValue;        // can be any string
            HashAlgorithm = key.HashAlgorithm != null ? key.HashAlgorithm : HashAlgorithm;             // can be "MD5"
            PasswordIterations = key.PasswordIterations!= 0 ? key.PasswordIterations : PasswordIterations;                  // can be any number
            InitVector = key.InitVector != null ? key.InitVector : InitVector; // must be 16 bytes
            KeySize = key.KeySize != 0 ? key.KeySize : KeySize;                // can be 192 or 128
        }
        #endregion

        #region Properties

		/// <summary>
		/// Pass Phrase
		/// </summary>
		public string PassPhrase
		{
			get { return passPhrase; }
			set { passPhrase = value; }
		}

		/// <summary>
		/// Salt Value
		/// </summary>
		public string SaltValue
		{
			get { return saltValue; }
			set { saltValue = value; }
		}

		/// <summary>
		/// Hash Algorithm
		/// </summary>
		public string HashAlgorithm
		{
			get { return hashAlgorithm; }
			set { hashAlgorithm = value; }
		}

		/// <summary>
		/// Password Iterations
		/// </summary>
		public int PasswordIterations
		{
			get { return passwordIterations; }
			set { passwordIterations = value; }
		}

		/// <summary>
		/// Init Vector
		/// </summary>
		public string InitVector
		{
			get { return initVector; }
			set { initVector = value; }
		}

		/// <summary>
		/// Key Size
		/// </summary>
		public int KeySize
		{
			get { return keySize; }
			set { keySize = value; }
		}

		#endregion

        #region Methods

		/// <summary>
		/// Encrypt
		/// </summary>
		/// <param name="clearText"></param>
		/// <returns></returns>
		public string Encrypt(string clearText)
		{
			// Convert strings into byte arrays.
			// Let us assume that strings only contain ASCII codes.
			// If strings include Unicode characters, use Unicode, UTF7, or UTF8 
			// encoding.
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] saltValueBytes  = Encoding.ASCII.GetBytes(saltValue);
        
			// Convert our plaintext into a byte array.
			// Let us assume that plaintext contains UTF8-encoded characters.
			byte[] plainTextBytes  = Encoding.UTF8.GetBytes(clearText);
        
			// First, we must create a password, from which the key will be derived.
			// This password will be generated from the specified passphrase and 
			// salt value. The password will be created using the specified hash 
			// algorithm. Password creation can be done in several iterations.
			PasswordDeriveBytes password = new PasswordDeriveBytes(
				passPhrase, 
				saltValueBytes, 
				hashAlgorithm, 
				passwordIterations);
        
			// Use the password to generate pseudo-random bytes for the encryption
			// key. Specify the size of the key in bytes (instead of bits).
			byte[] keyBytes = password.GetBytes(keySize / 8);
        
			// Create uninitialized Rijndael encryption object.
			RijndaelManaged symmetricKey = new RijndaelManaged();
        
			// It is reasonable to set encryption mode to Cipher Block Chaining
			// (CBC). Use default options for other symmetric key parameters.
			symmetricKey.Mode = CipherMode.CBC;        
        
			// Generate encryptor from the existing key bytes and initialization 
			// vector. Key size will be defined based on the number of the key 
			// bytes.
			ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
				keyBytes, 
				initVectorBytes);
        
			// Define memory stream which will be used to hold encrypted data.
			MemoryStream memoryStream = new MemoryStream();        
                
			// Define cryptographic stream (always use Write mode for encryption).
			CryptoStream cryptoStream = new CryptoStream(memoryStream, 
				encryptor, CryptoStreamMode.Write);

			// Start encrypting.
			cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                
			// Finish encrypting.
			cryptoStream.FlushFinalBlock();

			// Convert our encrypted data from a memory stream into a byte array.
			byte[] cipherTextBytes = memoryStream.ToArray();
                
			// Close both streams.
			memoryStream.Close();
			cryptoStream.Close();
        
			// Convert encrypted data into a base64-encoded string.
			string cipherText = Convert.ToBase64String(cipherTextBytes);
        
			// Return encrypted string.
			return cipherText;
		}

		/// <summary>
		/// Decrypt
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <returns></returns>
		public string Decrypt(string encryptedText)
		{
			// Convert strings defining encryption key characteristics into byte
			// arrays. Let us assume that strings only contain ASCII codes.
			// If strings include Unicode characters, use Unicode, UTF7, or UTF8
			// encoding.
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] saltValueBytes  = Encoding.ASCII.GetBytes(saltValue);
        
			// Convert our ciphertext into a byte array.
			byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
        
			// First, we must create a password, from which the key will be 
			// derived. This password will be generated from the specified 
			// passphrase and salt value. The password will be created using
			// the specified hash algorithm. Password creation can be done in
			// several iterations.
			PasswordDeriveBytes password = new PasswordDeriveBytes(
				passPhrase, 
				saltValueBytes, 
				hashAlgorithm, 
				passwordIterations);
        
			// Use the password to generate pseudo-random bytes for the encryption
			// key. Specify the size of the key in bytes (instead of bits).
			byte[] keyBytes = password.GetBytes(keySize / 8);
        
			// Create uninitialized Rijndael encryption object.
			RijndaelManaged    symmetricKey = new RijndaelManaged();
        
			// It is reasonable to set encryption mode to Cipher Block Chaining
			// (CBC). Use default options for other symmetric key parameters.
			symmetricKey.Mode = CipherMode.CBC;
        
			// Generate decryptor from the existing key bytes and initialization 
			// vector. Key size will be defined based on the number of the key 
			// bytes.
			ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
				keyBytes, 
				initVectorBytes);
        
			// Define memory stream which will be used to hold encrypted data.
			MemoryStream  memoryStream = new MemoryStream(cipherTextBytes);
                
			// Define cryptographic stream (always use Read mode for encryption).
			CryptoStream  cryptoStream = new CryptoStream(memoryStream, 
				decryptor,
				CryptoStreamMode.Read);

			// Since at this point we don't know what the size of decrypted data
			// will be, allocate the buffer long enough to hold ciphertext;
			// plaintext is never longer than ciphertext.
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];
        
			// Start decrypting.
			int decryptedByteCount = cryptoStream.Read(plainTextBytes, 
				0, 
				plainTextBytes.Length);
                
			// Close both streams.
			memoryStream.Close();
			cryptoStream.Close();
        
			// Convert decrypted data into a string. 
			// Let us assume that the original plaintext string was UTF8-encoded.
			string plainText = Encoding.UTF8.GetString(plainTextBytes, 
				0, 
				decryptedByteCount);
        
			// Return decrypted string.   
			return plainText;
		}

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="fileBuffer"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] fileBuffer)
        {
            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            byte[] plainTextBytes = fileBuffer;

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and 
            // salt value. The password will be created using the specified hash 
            // algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                passPhrase,
                saltValueBytes,
                hashAlgorithm,
                passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                keyBytes,
                initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                encryptor, CryptoStreamMode.Write);

            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Return encrypted byte[].
            return cipherTextBytes;
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="encryptBuffer"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] encryptBuffer)
        {
            // Convert strings defining encryption key characteristics into byte
            // arrays. Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our ciphertext into a byte array.
            byte[] cipherTextBytes = encryptBuffer;

            // First, we must create a password, from which the key will be 
            // derived. This password will be generated from the specified 
            // passphrase and salt value. The password will be created using
            // the specified hash algorithm. Password creation can be done in
            // several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                passPhrase,
                saltValueBytes,
                hashAlgorithm,
                passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                keyBytes,
                initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Define cryptographic stream (always use Read mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                decryptor,
                CryptoStreamMode.Read);

            // Since at this point we don't know what the size of decrypted data
            // will be, allocate the buffer long enough to hold ciphertext;
            // plaintext is never longer than ciphertext.
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Start decrypting.
            int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                0,
                plainTextBytes.Length);

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Return decrypted byte[].   
            return plainTextBytes;
        }

        #endregion

		
    }
}
