using Domain.Entities;
using Domain.Interfaces;
using FluentResults;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class TextFileNoteRepository : INoteRepository
    {
        private readonly IConfiguration _configuration;
        private readonly FileService _fileService;
        private readonly ILogger<TextFileNoteRepository> _logger;
        private readonly string _filePath;

        /// <inheritdoc />
        public TextFileNoteRepository(IConfiguration configuration, ILogger<TextFileNoteRepository> logger, FileService fileService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _filePath = _configuration["Settings:OutputFilePath"]
                       ?? throw new ArgumentNullException(nameof(_filePath), "Output file path cannot be null or empty.");

        }

        /// <inheritdoc />
        public async ValueTask<Result<INote>> CreateAsync(INote note)
        {
            var standardNote = new StandardNote(note.Id, note.Title, note.Text, DateTime.UtcNow, DateTime.UtcNow);
            string jsonContent = JsonSerializer.Serialize(standardNote);

            try
            {
                string safeTitle = string.Join("_", standardNote.Title.Split(Path.GetInvalidFileNameChars()));
                string filename = $"{standardNote.Id}-{safeTitle}-{standardNote.CreatedOn:yyyyMMddHHmmss}.txt";
                string fullPath = Path.Combine(_filePath, filename);

                await _fileService.WriteToFileAsync(fullPath, jsonContent);

                _logger.LogInformation($"File '{fullPath}' has been created and written successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while writing to the file.");
                return Result.Fail<INote>("Error writing to file.");
            }

            return Result.Ok<INote>(standardNote);
        }

        /// <inheritdoc />
        public async ValueTask<Result<int>> DeleteAsync(int id)
        {
            try
            {
                string[] files = _fileService.FindFiles(_filePath, id);

                if (files.Length == 0)
                {
                    _logger.LogWarning($"No file found with ID: {id}");
                    return Result.Fail<int>($"No file found with ID: {id}");
                }

                if (files.Length > 1)
                {
                    _logger.LogWarning($"Multiple files found with ID: {id}. Files: {string.Join(", ", files)}");
                    return Result.Fail<int>($"Multiple files found with ID: {id}. Delete operation aborted.");
                }

                await Task.Run(() => _fileService.DeleteFile(files[0]));
                _logger.LogInformation($"File '{files[0]}' deleted successfully.");

                return Result.Ok(id);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, $"Access denied when trying to delete file with ID: {id}");
                return Result.Fail<int>($"Access denied when trying to delete the file with ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while deleting file with ID: {id}");
                return Result.Fail<int>($"An error occurred while deleting the file with ID: {id}");
            }
        }

        /// <inheritdoc />
        public async ValueTask<Result<INote>> GetAsync(int id)
        {
            try
            {
                string[] files = _fileService.FindFiles(_filePath, id);

                if (files.Length == 0)
                {
                    _logger.LogWarning($"No file found with ID: {id}");
                    return Result.Fail<INote>($"No file found with ID: {id}");
                }

                if (files.Length > 1)
                {
                    _logger.LogWarning($"Multiple files found with ID: {id}. Files: {string.Join(", ", files)}");
                    return Result.Fail<INote>($"Multiple files found with ID: {id}. Retrieval operation aborted.");
                }

                string content = await _fileService.ReadFileAsync(files[0]);

                if (string.IsNullOrWhiteSpace(content))
                {
                    _logger.LogWarning($"The file at path {files[0]} is empty or contains only whitespace.");
                    return Result.Fail<INote>($"The file with ID {id} is empty.");
                }

                var standardNote = JsonSerializer.Deserialize<StandardNote>(content);

                if (standardNote == null)
                {
                    _logger.LogError($"Deserialization of the file at path {files[0]} failed.");
                    return Result.Fail<INote>($"Failed to parse the content of the file with ID: {id}");
                }

                return Result.Ok<INote>(standardNote);
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, $"An error occurred while accessing the file with ID: {id}");
                return Result.Fail<INote>($"An error occurred while accessing the file with ID: {id}");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, $"An error occurred while deserializing the content of the file with ID: {id}");
                return Result.Fail<INote>($"Failed to parse the content of the file with ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while retrieving the note with ID: {id}");
                return Result.Fail<INote>($"An unexpected error occurred while retrieving the note with ID: {id}");
            }
        }

        /// <inheritdoc />
        public async Task<Result<IReadOnlyCollection<INote>>> GetAsync(int pageNumber, int pageSize)
        {
            try
            {
                var paginatedFiles = _fileService.GetPaginatedFiles(_filePath, "*-*", pageNumber, pageSize);


                if (!paginatedFiles.Any())
                {
                    _logger.LogWarning($"No files found for page {pageNumber}.");
                    return Result.Fail<IReadOnlyCollection<INote>>($"No files found for page {pageNumber}.");
                }

                var notes = new List<INote>();
                foreach (var file in paginatedFiles)
                {
                    string content = await _fileService.ReadFileAsync(file);

                    if (string.IsNullOrWhiteSpace(content))
                    {
                        _logger.LogWarning($"File '{file}' is empty or contains only whitespace.");
                        continue;
                    }

                    var note = JsonSerializer.Deserialize<StandardNote>(content);
                    if (note == null)
                    {
                        _logger.LogError($"Failed to deserialize content from file '{file}'.");
                        continue;
                    }

                    notes.Add(note);
                }

                return Result.Ok<IReadOnlyCollection<INote>>(notes);
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "An error occurred while accessing files.");
                return Result.Fail<IReadOnlyCollection<INote>>("An error occurred while accessing files.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "An error occurred during deserialization.");
                return Result.Fail<IReadOnlyCollection<INote>>("Failed to parse file content.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return Result.Fail<IReadOnlyCollection<INote>>("An unexpected error occurred while retrieving notes.");
            }
        }

        /// <inheritdoc />
        public async ValueTask<Result<INote>> UpdateAsync(INote note)
        {
            try
            {
                string[] retrievedFiles = _fileService.FindFiles(_filePath, note.Id);
                if (retrievedFiles.Length == 0)
                {
                    _logger.LogWarning($"No file found with ID: {note.Id}");
                    return Result.Fail<INote>($"No file found with ID: {note.Id}");
                }

                if (retrievedFiles.Length > 1)
                {
                    _logger.LogWarning($"Multiple files found with ID: {note.Id}. Files: {string.Join(", ", retrievedFiles)}");
                    return Result.Fail<INote>($"Multiple files found with ID: {note.Id}. Update operation aborted.");
                }

                string content = await _fileService.ReadFileAsync(retrievedFiles[0]);
                if (string.IsNullOrWhiteSpace(content))
                {
                    _logger.LogWarning($"The file at path {retrievedFiles[0]} is empty or contains only whitespace.");
                    return Result.Fail<INote>($"The file with ID {note.Id} is empty.");
                }

                var currentNote = JsonSerializer.Deserialize<StandardNote>(content);
                if (currentNote == null)
                {
                    _logger.LogError($"Failed to deserialize content from file '{retrievedFiles[0]}'.");
                    return Result.Fail<INote>($"Failed to parse the content of the file with ID: {note.Id}");
                }

                var updatedNote = new StandardNote(currentNote.Id, note.Title, note.Text, currentNote.CreatedOn, DateTime.UtcNow);
                string updatedContent = JsonSerializer.Serialize(updatedNote);

                await _fileService.WriteToFileAsync(retrievedFiles[0], updatedContent);
                _logger.LogInformation($"File '{retrievedFiles[0]}' updated successfully.");

                return Result.Ok<INote>(updatedNote);
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, $"An error occurred while accessing the file with ID: {note.Id}");
                return Result.Fail<INote>($"An error occurred while accessing the file with ID: {note.Id}");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, $"An error occurred while deserializing or serializing the content of the file with ID: {note.Id}");
                return Result.Fail<INote>($"Failed to parse or update the content of the file with ID: {note.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while updating the note with ID: {note.Id}");
                return Result.Fail<INote>($"An unexpected error occurred while updating the note with ID: {note.Id}");
            }
        }
    }
}
