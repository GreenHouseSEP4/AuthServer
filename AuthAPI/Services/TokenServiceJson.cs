using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MoneyTrackDatabaseAPI.Models;

namespace MoneyTrackDatabaseAPI.Services
{
    public class TokenServiceJson : ITokenService
        {
            private string TokensFile = "Tokens.json";
            private IList<string> AllTokens;

            public TokenServiceJson()
            {
                if (File.Exists(TokensFile))
                {
                    string TokensInJSON = File.ReadAllText(TokensFile);
                    AllTokens = JsonSerializer.Deserialize<IList<String>>(TokensInJSON);
                }
                else
                {
                    Seed();
                    Save();
                }
            }
            private void Seed()
            {
                IList<string> Tokens = new List<String>();
                AllTokens = Tokens.ToList();
            }

            private void Save()
            {
                string TokensInJson = JsonSerializer.Serialize(AllTokens);
                File.WriteAllText(TokensFile, TokensInJson);
            }

            public async Task AddToken(string token)
            {
                AllTokens.Add(token);
                Save();
            }
            


            public async Task Logout(string token)
            {
                var check = AllTokens.FirstOrDefault(s => s.Equals(token));
                if (check != null)
                {
                    AllTokens.Remove(check);
                    Save();
                }
                else
                {
                    throw new Exception("Token not found!");
                }
            }

            public async Task<bool> ContainsToken(string token)
            {
                var check = AllTokens.FirstOrDefault(s=>s.Equals(token));
                if(check!=null)
                {
                    return true;
                }
                return false;
            }
        }
}