using FlashQuizz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlashQuizz.Services
{
    /// <summary>
    /// Une classe qui permet d'accèder aux données de la base de données (ici, des fichiers JSON)
    /// </summary>
    public class DeckService
    {
        /// <summary>
        /// Le chemin de fichiers par lequel on peut retrouver le deck
        /// </summary>
        private readonly string _filePath;

        public DeckService()
        {
            // Path to store the JSON file in app data
            _filePath = Path.Combine(
                FileSystem.AppDataDirectory,
                "decks.json"
            );
        }
        /// <summary>
        /// L'équivalent d'un GET, ceci est l'opération READ et permet d'obtenir les données
        /// </summary>
        /// <returns>Les données du deck</returns>
        public async Task<List<Deck>> LoadDecksAsync()
        {
            try
            {
                //si pas de fichier existant, on renvoie une liste vide
                if (!File.Exists(_filePath))
                {
                    return new List<Deck>();
                }
                //get les données
                string json = await File.ReadAllTextAsync(_filePath);
                //convert de string en json
                List<Deck>? decks = JsonSerializer.Deserialize<List<Deck>>(json);
                //return les données ou (si elles sont vides) une liste vide
                return decks ?? new List<Deck>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading: {ex.Message}");
                return new List<Deck>();
            }
        }

        /// <summary>
        /// L'équivalent de PUT, ceci est l'opération UPDATE et permet de sauvegarder (en écransant) toutes les données d'un deck
        /// </summary>
        /// <param name="decks"></param>
        /// <returns></returns>
        public async Task SaveDecksAsync(List<Deck> decks)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(decks, options);
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
