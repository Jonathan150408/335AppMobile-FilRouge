using FlashQuizz_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlashQuizz_v2.Services
{
    /// <summary>
    /// Une classe qui permet d'accèder aux données de la base de données (ici, des fichiers JSON)
    /// </summary>
    public class CardService
    {
        /// <summary>
        /// Le chemin de fichiers par lequel on peut retrouver le Card
        /// </summary>
        private readonly string _filePath;

        public CardService()
        {
            // Path to store the JSON file in app data
            _filePath = Path.Combine(
                FileSystem.AppDataDirectory,
                "Cards.json"
            );
        }
        /// <summary>
        /// L'équivalent d'un GET, ceci est l'opération READ et permet d'obtenir les données
        /// </summary>
        /// <returns>Les données du Card</returns>
        public async Task<List<Card>> LoadCardsAsync()
        {
            try
            {
                //si pas de fichier existant, on renvoie une liste vide
                if (!File.Exists(_filePath))
                {
                    return new List<Card>();
                }
                //get les données
                string json = await File.ReadAllTextAsync(_filePath);
                //convert de string en json
                List<Card>? Cards = JsonSerializer.Deserialize<List<Card>>(json);
                //return les données ou (si elles sont vides) une liste vide
                return Cards ?? new List<Card>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading: {ex.Message}");
                return new List<Card>();
            }
        }

        /// <summary>
        /// L'équivalent de PUT, ceci est l'opération UPDATE et permet de sauvegarder (en écransant) toutes les données d'un Card
        /// </summary>
        /// <param name="Cards"></param>
        /// <returns></returns>
        public async Task SaveCardsAsync(List<Card> Cards)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(Cards, options);
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving: {ex.Message}");
            }
        }
        /// <summary>
        /// Permet de recevoir le chemin actuel
        /// </summary>
        /// <returns>Une string contenant le chemin de fichiers</returns>
        public string GetFilePath()
        {
            return _filePath;
        }
    }
}
