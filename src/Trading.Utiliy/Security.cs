using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Trading.Utiliy
{
    public class Security
    {
        private readonly string _keyFilePath;
        private readonly object _lock = new object();

        public Security(string keyFilePath = "keys.dat")
        {
            _keyFilePath = keyFilePath;
        }

        /// <summary>
        /// 키-값 쌍을 저장합니다.
        /// </summary>
        public void SaveKeys(Dictionary<string, string> keys)
        {
            lock (_lock)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(keys);
                    var encryptedData = ProtectedData.Protect(Encoding.UTF8.GetBytes(json), null, DataProtectionScope.CurrentUser);
                    File.WriteAllBytes(_keyFilePath, encryptedData);
                }
                catch (Exception ex)
                {
                    // 예외 발생 시 로그 작성
                    Console.Error.WriteLine($"[SaveKeys] 키를 저장하는 동안 오류가 발생했습니다: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// 단일 키-값 쌍을 저장합니다.
        /// </summary>
        public void SaveKey(string key, string value)
        {
            lock (_lock)
            {
                var keys = LoadKeys();
                keys[key] = value;
                SaveKeys(keys);
            }
        }

        /// <summary>
        /// 저장된 모든 키-값 쌍을 로드합니다.
        /// </summary>
        public Dictionary<string, string> LoadKeys()
        {
            lock (_lock)
            {
                try
                {
                    if (File.Exists(_keyFilePath))
                    {
                        var encryptedData = File.ReadAllBytes(_keyFilePath);
                        var decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
                        var json = Encoding.UTF8.GetString(decryptedData);
                        var keys = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        return keys ?? new Dictionary<string, string>();
                    }
                    else
                    {
                        return new Dictionary<string, string>();
                    }
                }
                catch (Exception ex)
                {
                    // 예외 발생 시 로그 작성
                    Console.Error.WriteLine($"[LoadKeys] 키를 로드하는 동안 오류가 발생했습니다: {ex.Message}");
                    // 빈 딕셔너리를 반환하여 데이터 손실을 방지합니다.
                    return new Dictionary<string, string>();
                }
            }
        }

        /// <summary>
        /// 특정 키를 삭제합니다.
        /// </summary>
        public void DeleteKey(string key)
        {
            lock (_lock)
            {
                try
                {
                    var keys = LoadKeys();
                    if (keys.Remove(key))
                    {
                        SaveKeys(keys);
                    }
                    else
                    {
                        Console.Error.WriteLine($"[DeleteKey] 키 '{key}'를 찾을 수 없습니다.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"[DeleteKey] 키를 삭제하는 동안 오류가 발생했습니다: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// 모든 키를 삭제합니다.
        /// </summary>
        public void DeleteAllKeys()
        {
            lock (_lock)
            {
                try
                {
                    if (File.Exists(_keyFilePath))
                    {
                        File.Delete(_keyFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"[DeleteAllKeys] 키를 삭제하는 동안 오류가 발생했습니다: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
