using Domain.Entities;
using Domain.Interfaces;
using FluentResults;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class TextFileNoteRepository : INoteRepository
    {
        public async ValueTask<Result<INote>> CreateAsync(INote note)
        {
            string filePath = "C:\\Users\\jerzy\\OneDrive\\Documents\\output.txt"; 
            string content = $"Note ID: {note.Id}\nTitle: {note.Title}\nText: {note.Text}\nCreated On: {note.CreatedOn}\nModified On: {note.ModifiedOn}";

            try
            {
                // Write content to the file
                File.WriteAllText(filePath, content);
                Console.WriteLine($"File '{filePath}' has been created and written successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return Result.Fail<INote>("Error writing to file.");
            }

            // Return the result wrapped in a ValueTask
            return Result.Ok<INote>(new StandardNote(note.Id, note.Title, note.Text, note.CreatedOn, note.ModifiedOn));
        }

        public ValueTask<Result<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result<INote>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IReadOnlyCollection<INote>>> GetAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result<int>> GetCountAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result<INote>> UpdateAsync(INote note)
        {
            throw new NotImplementedException();
        }
    }
}
